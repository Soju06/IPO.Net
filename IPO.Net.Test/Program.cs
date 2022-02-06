using System.Text;

namespace IPO.Net.Test {
    internal static class Program {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        async static Task Main() {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            ApplicationConfiguration.Initialize();
            Application.Run(new Main());
        }
    }
}