using Microsoft.Win32;

namespace MatrixRain
{
    public partial class SettingsForm : Form
    {
        private const int DefaultRunners = 15;
        private const int DefaultMsPerTick = 20;
        public SettingsForm()
        {
            InitializeComponent();
            LoadSettings();
        }

        /// <summary>
        /// Load settings from the registry
        /// </summary>
        private void LoadSettings()
        {
            LinesInput.Value = DefaultRunners;
            RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\MatrixRainScreensaver");
            object? cncRunners = key.GetValue("ConcurrentRunners");
            object? mspertick = key.GetValue("MsPerTick");
            if (cncRunners != null)
                LinesInput.Value = (int)cncRunners;
            else
                key.SetValue("ConcurrentRunners", DefaultRunners, RegistryValueKind.DWord);

            if (mspertick != null)
                MsPerTickInput.Value = (int)mspertick;
            else
                key.SetValue("MsPerTick", DefaultMsPerTick, RegistryValueKind.DWord);

        }
        /// Save settings into the registry
        /// </summary>
        private void SaveSettings()
        {
            // Create or get existing subkey
            RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\MatrixRainScreensaver");
            key.SetValue("ConcurrentRunners", LinesInput.Value, RegistryValueKind.DWord);
            key.SetValue("MsPerTick",MsPerTickInput.Value, RegistryValueKind.DWord);
        }
        private void OkButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }
        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}