using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IPO.Net.utility {
    internal class TaskMultiResult<T> {
        protected List<Task<T>> requests = new();
        protected List<Task> continueActions = new();
        protected Action<T>? completeAction;

        public uint MaxMultiRequset { get; set; }

        public TaskMultiResult() {

        }

        public TaskMultiResult<T> SetCompleteAction(Action<T> action) {
            completeAction = action;
            return this;
        }

        public Task<T> AddRequest(Task<T> request) {
            requests.Add(request);
            if (completeAction != null) continueActions.Add(request.ContinueWith(t => completeAction.Invoke(t.Result)));
            return request;
        }

        public T[] Wait() {
            var reqs = requests.ToArray();
            var ls = new T[reqs.Length];
            Task.WaitAll(reqs);
            for (int i = 0; i < reqs.Length; i++)
                ls[i] = reqs[i].Result;
            return ls;
        }

        public T[] WaitAll() {
            var reqs = Wait();
            Task.WaitAll(continueActions.ToArray());
            return reqs;
        }

        public void Clear() {
            requests.Clear();
            continueActions.Clear();
        }
    }
}
