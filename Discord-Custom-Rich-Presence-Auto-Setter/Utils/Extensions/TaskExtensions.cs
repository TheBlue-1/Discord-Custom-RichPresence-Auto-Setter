#region
using System.Threading.Tasks;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Utils.Extensions {
	public static class TaskExtensions {
		public static T WaitForResult<T>(this Task<T> task) {
			task.Wait();
			return task.Result;
		}
	}
}
