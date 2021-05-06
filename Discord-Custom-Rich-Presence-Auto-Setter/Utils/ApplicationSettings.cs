#region
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Utils {
	
	public class ApplicationSettings : ApplicationSettings.IApplicationSettings {
		private const string SettingsFileName = "settings";

		private object SaveSettingsLock { get; } = new();

		private InnerApplicationSettings Settings { get; }
		public long DefaultApplicationId {
			get => Settings.DefaultApplicationId;
			set => Settings.DefaultApplicationId = value;
		}
		public TimeSpan DiscordCommunicationGap {
			get => Settings.DiscordCommunicationGap;
			set => Settings.DiscordCommunicationGap = value;
		}
		public TimeSpan RequirementCheckSpan {
			get => Settings.RequirementCheckSpan;
			set => Settings.RequirementCheckSpan = value;
		}
		private bool SaveSettingsAgain { get; set; }
		private bool SaveSettingsRunning { get; set; }

		public ApplicationSettings() {
			Settings = LoadSettings();

			Settings.PropertyChanged += SettingChanged;
		}

		private static InnerApplicationSettings LoadSettings() => FileService.Instance.ReadJsonFromFile<InnerApplicationSettings>(SettingsFileName);

		private async Task SaveSettings() {
			lock (SaveSettingsLock) {
				if (SaveSettingsRunning) {
					SaveSettingsAgain = true;
					return;
				}
				SaveSettingsRunning = true;
			}
			while (true) {
				await FileService.Instance.SaveJsonToFile(SettingsFileName, Settings);
				lock (SaveSettingsLock) {
					if (SaveSettingsAgain) {
						SaveSettingsAgain = false;
						continue;
					}
					SaveSettingsRunning = false;
					return;
				}
			}
		}

		private async void SettingChanged(object sender, PropertyChangedEventArgs e) {
			await SaveSettings();
		}
		[SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
		public interface IApplicationSettings {
			public long DefaultApplicationId { get; set; }
			public TimeSpan DiscordCommunicationGap { get; set; }
			public TimeSpan RequirementCheckSpan { get; set; }
		}

		private sealed class InnerApplicationSettings : IApplicationSettings, INotifyPropertyChanged {
			private long _defaultApplicationId;
			private TimeSpan _discordCommunicationGap;
			private TimeSpan _requirementCheckSpan;
			public long DefaultApplicationId {
				get => _defaultApplicationId;
				set {
					_defaultApplicationId = value;
					OnPropertyChanged();
				}
			}
			public TimeSpan DiscordCommunicationGap {
				get => _discordCommunicationGap;
				set {
					_discordCommunicationGap = value;
					OnPropertyChanged();
				}
			}
			public TimeSpan RequirementCheckSpan {
				get => _requirementCheckSpan;
				set {
					_requirementCheckSpan = value;
					OnPropertyChanged();
				}
			}

			public InnerApplicationSettings() {
				DefaultApplicationId = 0;
				DiscordCommunicationGap = new TimeSpan(0, 0, 0, 0, 500);

				RequirementCheckSpan = new TimeSpan(0, 0, 5);
			}

			private void OnPropertyChanged([CallerMemberName] string propertyName = null) {
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}

			public event PropertyChangedEventHandler PropertyChanged;
		}
	}
}
