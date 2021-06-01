using System.Collections.Generic;
using UnityEditor;

namespace VoltUnityBuilder.Settings
{
	/// <summary>
	///		Manages settings for VoltUnityBuilder
	///		<para>
	///			Feel free to use this to store your settings for your custom build actions
	///		</para>
	/// </summary>
	public static class SettingsManager
	{
		private const string PackageName = "dev.voltstro.voltunitybuilder";

		private static UnityEditor.SettingsManagement.Settings settings;

		/// <summary>
		///		The active <see cref="Settings"/> instance
		/// </summary>
		public static UnityEditor.SettingsManagement.Settings Instance => settings ??= new UnityEditor.SettingsManagement.Settings(PackageName);

		///  <summary>
		/// 		Add or gets an option from <see cref="Instance"/>
		///  </summary>
		///  <typeparam name="T"></typeparam>
		///  <param name="key"></param>
		///  <param name="defaultValue"></param>
		///  <returns></returns>
		public static T AddOrGetOption<T>(string key, T defaultValue = default(T))
		{
			UnityEditor.SettingsManagement.Settings settingsInstance = Instance;
			if (!settingsInstance.ContainsKey<T>(key))
				SetOption<T>(key, defaultValue);

			return settingsInstance.Get<T>(key);
		}

		/// <summary>
		///		Sets an option from <see cref="Instance"/>
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <typeparam name="T"></typeparam>
		public static void SetOption<T>(string key, T value)
		{
			UnityEditor.SettingsManagement.Settings settingsInstance = Instance;
			settingsInstance.Set(key, value);
			settingsInstance.Save();
		}

		#region Properties

		#region Build Path Settings

		private const string BuildLocationKey = "BuildLocation";
		public static string BuildLocation
		{
			get => AddOrGetOption(BuildLocationKey, "Builds/");
			internal set => SetOption(BuildLocationKey, value);
		}

		private const string BuildFolderNameStyleKey = "BuildFolderNameStyle";
		public static string BuildFolderNameStyle
		{
			get => AddOrGetOption(BuildFolderNameStyleKey, "yy-MM-dd");
			internal set => SetOption(BuildFolderNameStyleKey, value);
		}

		#endregion

		#region Build Player Settings

		private const string BuildTargetKey = "BuildTarget";
		public static BuildTarget BuildTarget
		{
			get => AddOrGetOption(BuildTargetKey, EditorUserBuildSettings.activeBuildTarget);
			internal set => SetOption(BuildTargetKey, value);
		}

		private const string ServerBuildKey = "ServerBuild";
		public static bool ServerBuild
		{
			get => AddOrGetOption<bool>(ServerBuildKey);
			internal set => SetOption(ServerBuildKey, value);
		}

		private const string CopyPdbFilesKey = "CopyPDBFiles";
		public static bool CopyPdbFiles
		{
			get => AddOrGetOption<bool>(CopyPdbFilesKey);
			internal set => SetOption(CopyPdbFilesKey, value);
		}

		private const string DevelopmentBuildKey = "DevelopmentBuild";
		public static bool DevelopmentBuild
		{
			get => AddOrGetOption<bool>(DevelopmentBuildKey);
			internal set => SetOption(DevelopmentBuildKey, value);
		}

		private const string AutoconnectProfilerKey = "AutoconnectProfiler";
		public static bool AutoconnectProfiler
		{
			get => AddOrGetOption<bool>(AutoconnectProfilerKey);
			internal set => SetOption(AutoconnectProfilerKey, value);
		}

		private const string DeepProfilingKey = "DeepProfiling";
		public static bool DeepProfiling
		{
			get => AddOrGetOption<bool>(DeepProfilingKey);
			internal set => SetOption(DeepProfilingKey, value);
		}

		private const string ScriptDebuggingKey = "ScriptDebugging";
		public static bool ScriptDebugging
		{
			get => AddOrGetOption<bool>(ScriptDebuggingKey);
			internal set => SetOption(ScriptDebuggingKey, value);
		}

		#endregion

		private const string BuildActionsKey = "BuildActions";
		public static List<string> BuildActions
		{
			get => AddOrGetOption(BuildActionsKey, new List<string>());
			internal set => SetOption(BuildActionsKey, value);
		}

		#endregion
	}
}