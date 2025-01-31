using System.Runtime.InteropServices;

namespace FSR3ModSetupUtilityEnhanced


{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetProcessDpiAwarenessContext(int dpiContext);

        const int DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 = -4;
        static void Main()
        {
            SetProcessDpiAwarenessContext(DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ApplicationConfiguration.Initialize();
            Application.Run(new mainForm());
        }
    }
}