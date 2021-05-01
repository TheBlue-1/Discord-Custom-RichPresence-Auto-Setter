using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Media;
using Discord_Custom_Rich_Presence_Auto_Setter.Utils.Extensions;

namespace Discord_Custom_Rich_Presence_Auto_Setter.Utils
{
    public class ApplicationSettings : ApplicationSettings.IApplicationSettings
    {
        private const string SettingsFileName = "settings";

        private object SaveSettingsLock { get; } = new();
        private bool SaveSettingsAgain { get; set; }
        private bool SaveSettingsRunning { get; set; }


        private Task<InnerApplicationSettings> LoadSettings() =>
             FileService.Instance.ReadJsonFromFile<InnerApplicationSettings>(SettingsFileName);

        public ApplicationSettings()
        {




            Settings = LoadSettings().WaitForResult();

            Settings.PropertyChanged += SettingChanged;
        }

        public interface IApplicationSettings
        {
            public long DefaultApplicationId { get; set; }
        }

        private sealed class InnerApplicationSettings : IApplicationSettings, INotifyPropertyChanged
        {
            private long _defaultApplicationId;
            public long DefaultApplicationId
            {
                get => _defaultApplicationId;
                set { _defaultApplicationId = value; OnPropertyChanged(); }
            }
            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private async void SettingChanged(object sender, PropertyChangedEventArgs e)
        {
            await SaveSettings();
        }

        private async Task SaveSettings()
        {
            lock (SaveSettingsLock)
            {
                if (SaveSettingsRunning)
                {
                    SaveSettingsAgain = true;
                    return;
                }
                SaveSettingsRunning = true;
            }
            while (true)
            {
                await FileService.Instance.SaveJsonToFile(SettingsFileName, Settings);
                lock (SaveSettingsLock)
                {
                    if (SaveSettingsAgain)
                    {
                        SaveSettingsAgain = false;
                        continue;
                    }
                    SaveSettingsRunning = false;
                    return;
                }
            }
        }
        private InnerApplicationSettings Settings { get; }
        public long DefaultApplicationId
        {
            get => Settings.DefaultApplicationId;
            set => Settings.DefaultApplicationId = value;
        }
    }
}
