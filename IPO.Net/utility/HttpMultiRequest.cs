using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace IPO.Net.utility {
    internal class HttpMultiRequest : IDisposable {
        protected CancellationTokenSource _token = new();
        protected Queue<HttpRequestMessage> requestQuery = new();
        protected Queue<HttpResponseMessage> completeQuery = new();
        protected List<Task<HttpResponseMessage>> tasks = new();
        protected Action<HttpResponseMessage>? completeAction;
        protected Action<Exception>? faultedAction;

        public HttpClient Http { get; }

        public uint MaxMultiRequest { get; set; }

        public int QueryCount => requestQuery.Count;

        public int RequestCount => tasks.Count;

        public int CompleteCount => completeQuery.Count;

        public HttpMultiRequest(HttpClient http, uint maxMultiRequest = 6) {
            Http = http;
            MaxMultiRequest = maxMultiRequest;
            Loop(_token.Token).Start();
        }

        public HttpMultiRequest AddRequest(HttpRequestMessage request) {
            requestQuery.Enqueue(request);
            return this;
        }

        public HttpMultiRequest SetComplete(Action<HttpResponseMessage> action) {
            completeAction = action;
            return this;
        }

        public HttpMultiRequest SetFaulted(Action<Exception> action) {
            faultedAction = action;
            return this;
        }

        async Task Loop(CancellationToken token) {
            while (!token.IsCancellationRequested) {
                try {
                    if (tasks.Count < MaxMultiRequest && requestQuery.Count > 0)
                        tasks.Add(Http.SendAsync(requestQuery.Dequeue(), token));

                    var ix = Task.WaitAny(tasks.ToArray());
                    if (ix >= 0) {
                        var task = tasks[ix];
                        if (task == null || !task.IsCompleted) continue;

                        if (task.IsCompletedSuccessfully) {
                            var res = task.Result;
                            completeAction?.Invoke(res);
                            completeQuery.Enqueue(res);
                        } else faultedAction?.Invoke(task.Exception);

                        if (tasks.Count == MaxMultiRequest && requestQuery.Count > 0)
                            tasks[ix] = Http.SendAsync(requestQuery.Dequeue(), token);
                        else tasks.RemoveAt(ix);
                    }
                    else await Task.Delay(20, token);
                } catch (Exception ex) {
                    faultedAction?.Invoke(ex);
                    await Task.Delay(20, token);
                }
            }
        }

        public async Task WaitAll() {
            while (requestQuery.Count > 0 && tasks.Count > 0)
                await Task.Delay(20);
        }

        public HttpResponseMessage? Poll() =>
            completeQuery.TryDequeue(out var m) ? m : null;

        public void RequestClear() {
            requestQuery.Clear();
        }

        public void CancelAll() {
            _token.Cancel();
        }

        public void Dispose() {
            CancelAll();
        }
    }
}
