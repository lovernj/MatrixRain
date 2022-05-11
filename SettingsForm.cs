using Microsoft.Win32;

namespace MatrixRain
{
    public partial class SettingsForm : Form
    {
        private const int DefaultRunners = 15;
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
            //If CreateSubKey fails, it will return null.
            object? tmp = key.GetValue("ConcurrentRunners");
            if (tmp != null)
            {
                LinesInput.Value = (int)tmp;
            }
            else
            {
                key.SetValue("ConcurrentRunners", DefaultRunners, RegistryValueKind.DWord);
            }
        }
        /// Save settings into the registry
        /// </summary>
        private void SaveSettings()
        {
            // Create or get existing subkey
            RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\MatrixRainScreensaver");
            key.SetValue("ConcurrentRunners", LinesInput.Value, RegistryValueKind.DWord);
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