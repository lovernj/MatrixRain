
#define ONEMONITOR
#if ONEMONITOR
    #undef ONEMONITOR
#endif
namespace MatrixRain
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0)
            {
                string firstArgument = args[0].ToLower().Trim();
                string secondArgument = "";

                // Handle cases where arguments are separated by colon. 
                // Examples: /c:1234567 or /P:1234567
                if (firstArgument.Length > 2)
                {
                    secondArgument = firstArgument[3..].Trim();
                    firstArgument = firstArgument[..2];
                }
                else if (args.Length > 1)
                {
                    secondArgument = args[1];
                }

                if (firstArgument == "/c")           // Configuration mode
                {
                    Application.Run(new SettingsForm());
                }
                else if (firstArgument == "/p")      // Preview mode
                {
                    if (secondArgument.Length > 0)
                    {
                        MessageBox.Show("Sorry, but the expected window handle was not provided.",
                            "ScreenSaver", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    IntPtr previewWndHandle = new IntPtr(long.Parse(secondArgument));
                    Application.Run(new Screensaver(previewWndHandle));
                }
                else if (firstArgument == "/s")      // Full-screen mode
                {
                    ShowScreenSaver();
                    Application.Run();
                }
                else    // Undefined argument
                {
                    MessageBox.Show("Sorry, but the command line argument \"" + firstArgument +
                        "\" is not valid.", "ScreenSaver",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else    // No arguments - treat like /c
            {
                Application.Run(new SettingsForm());
            }
        }

        /// <summary>
        /// Display the form on each of the computer's monitors.
        /// </summary>
        private static void ShowScreenSaver()
        {
            //Code for showing on one monitor
#if ONEMONITOR
            Screensaver screensaver = new(Screen.AllScreens[2].Bounds);
            screensaver.Show();
#else

            //Code for showing on all monitors
            foreach (Screen screen in Screen.AllScreens)
            {
                Screensaver screensaver = new(screen.Bounds);
                screensaver.Show();
            }
#endif
        }

    }
}