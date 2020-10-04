using System.Diagnostics;
using UnityEditor;
using Voltstro.UnityBuilder.Settings;

namespace Voltstro.UnityBuilder.Actions
{
	internal sealed class RunOnBuild : IBuildAction
	{
		private const string SettingsRunOnBuild = "RunOnBuild";
		private const string SettingArguments = "RunOnBuildArguments";

		public void OnGUI()
		{
			RunOnBuildSetting = EditorGUILayout.Toggle("Run on Build", RunOnBuildSetting);
			RunOnBuildArguments = EditorGUILayout.TextField("Arguments", RunOnBuildArguments);

			EditorGUILayout.Space();
		}

		public void OnBeforeBuild(string buildLocation)
		{
		}

		public void OnAfterBuild(string buildLocation)
		{
			if (RunOnBuildSetting)
				Process.Start(buildLocation, RunOnBuildArguments);
		}

		private bool RunOnBuildSetting
		{
			get
			{
				if (!SettingsManager.Instance.ContainsKey<bool>(SettingsRunOnBuild))
				{
					SettingsManager.Instance.Set(SettingsRunOnBuild, true);
					SettingsManager.Instance.Save();
				}

				return SettingsManager.Instance.Get<bool>(SettingsRunOnBuild);
			}
			set
			{
				SettingsManager.Instance.Set(SettingsRunOnBuild, value);
				SettingsManager.Instance.Save();
			}
		}

		private string RunOnBuildArguments
		{
			get
			{
				if (!SettingsManager.Instance.ContainsKey<bool>(SettingArguments))
				{
					SettingsManager.Instance.Set(SettingArguments, "");
					SettingsManager.Instance.Save();
				}

				return SettingsManager.Instance.Get<string>(SettingArguments);
			}
			set
			{
				SettingsManager.Instance.Set(SettingArguments, value);
				SettingsManager.Instance.Save();
			}
		}
	}
}