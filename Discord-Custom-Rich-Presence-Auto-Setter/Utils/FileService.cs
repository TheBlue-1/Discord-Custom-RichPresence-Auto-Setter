#region
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Utils {
	public class FileService : INotifyPropertyChanged {
		private const string ApplicationFolderName = "Discord_Custom_Rich_Presence_Auto_Setter";
		private const string JsonFileEnding = ".json";
		private const string SaveFileVersion = "v1";

		private static readonly JsonSerializerSettings JsonSettings = new() { TypeNameHandling = TypeNameHandling.Auto };

		private readonly object _logFile = new();

		public static string ApplicationFolderPath =>
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationFolderName, SaveFileVersion);
		public static FileService Instance { get; } = new();

		public long TotalFileBytes {
			get {
				DirectoryInfo directoryInfo = new(ApplicationFolderPath);
				return directoryInfo.GetFiles(string.Empty, SearchOption.AllDirectories).Sum(fileInfo => fileInfo.Length);
			}
		}

		static FileService() { }

		private FileService() {
			if (!Directory.Exists(ApplicationFolderPath)) {
				Directory.CreateDirectory(ApplicationFolderPath);
			}
		}

		public bool JsonFileExists(string filename) => File.Exists(Path.Combine(ApplicationFolderPath, filename + JsonFileEnding));

		public void Log(string message, string filePrefix = "") {
			string fileName = Path.Combine(ApplicationFolderPath, filePrefix + "log.txt");
			message = $"{DateTime.UtcNow}: {message}\n";
			lock (_logFile) {
				File.AppendAllText(fileName, message);
			}
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public T ReadJsonFromFile<T>(string fileName)
			where T : new() {
			fileName = Path.Combine(ApplicationFolderPath, fileName + JsonFileEnding);
			if (!File.Exists(fileName)) {
				return new T();
			}
			string json = File.ReadAllText(fileName);
			T val = JsonConvert.DeserializeObject<T>(json, JsonSettings);
			return val ?? new T();
		}

		public async Task SaveJsonToFile<T>(string fileName, T content) {
			await Task.Run(() => {
				fileName = Path.Combine(ApplicationFolderPath, fileName + JsonFileEnding);
				string json = JsonConvert.SerializeObject(content, JsonSettings);
				File.WriteAllText(Path.Combine(ApplicationFolderPath, fileName), json);
			});
			OnPropertyChanged(nameof (TotalFileBytes));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
