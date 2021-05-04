using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Media;
using Discord_Custom_Rich_Presence_Auto_Setter.Utils.Extensions;

namespace Discord_Custom_Rich_Presence_Auto_Setter.Utils
{
    public class ApplicationSettings : ApplicationSettings.IApplicationSettings
    {
		private readonly object _saveSettingsLock = new();
		private bool _saveSettingsAgain;
		private bool _saveSettingsRunning;
		private readonly InnerApplicationSettings _settings;
		private const string SettingsFileName = "settings";

		private object SaveSettingsLock {
			get { return _saveSettingsLock; }
		}
		private bool SaveSettingsAgain {
			get { return _saveSettingsAgain; }
			set { _saveSettingsAgain = value; }
		}
		private bool SaveSettingsRunning {
			get { return _saveSettingsRunning; }
			set { _saveSettingsRunning = value; }
		}

		private InnerApplicationSettings LoadSettings() {

			return  FileService.Instance.ReadJsonFromFile<InnerApplicationSettings>(SettingsFileName);
		}

		public ApplicationSettings()
        {




            _settings = LoadSettings();

            Settings.PropertyChanged += SettingChanged;
        }

        public interface IApplicationSettings
        {
			public long DefaultApplicationId { get; set; }
			public TimeSpan DiscordCommunicationGap { get; set; }
			public TimeSpan RequirementCheckSpan { get; set; }
        }

        private sealed class InnerApplicationSettings : IApplicationSettings, INotifyPropertyChanged
        {
            private long _defaultApplicationId;
			private TimeSpan _discordCommunicationGap;
			private TimeSpan _requirementCheckSpan;
			public long DefaultApplicationId {
				get { return _defaultApplicationId; }
				set { _defaultApplicationId = value; OnPropertyChanged(); }
			}
			public TimeSpan DiscordCommunicationGap {
				get { return _discordCommunicationGap; }
				set { _discordCommunicationGap = value; OnPropertyChanged(); }
			}
			public TimeSpan RequirementCheckSpan {
				get { return _requirementCheckSpan; }
				set { _requirementCheckSpan = value; OnPropertyChanged(); }
			}
			public event PropertyChangedEventHandler PropertyChanged;

			public InnerApplicationSettings() {
				DefaultApplicationId = 0;
				DiscordCommunicationGap = new TimeSpan(0, 0, 0,0, 500);

				RequirementCheckSpan = new TimeSpan(0, 0, 5);
			}
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

		private InnerApplicationSettings Settings {
			get { return _settings; }
		}
		public long DefaultApplicationId {
			get { return Settings.DefaultApplicationId; }
			set { Settings.DefaultApplicationId = value; }
		}
		public TimeSpan DiscordCommunicationGap {
			get => _settings.DiscordCommunicationGap;
			set => _settings.DiscordCommunicationGap = value;
		}
		public TimeSpan RequirementCheckSpan {
			get => _settings.RequirementCheckSpan;
			set => _settings.RequirementCheckSpan = value;
		}
	}
}
