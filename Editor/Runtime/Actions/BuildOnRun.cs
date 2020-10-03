using UnityEditor;
using Voltstro.UnityBuilder.Settings;

public class BuildOnRun : IBuildAction
{
	private const string SettingsKey = "RunOnBuild";

	public void OnGUI()
	{
		if (!SettingsManager.Instance.ContainsKey<bool>(SettingsKey))
		{
			SettingsManager.Instance.Set(SettingsKey, true);
			SettingsManager.Instance.Save();
		}

		bool buildOnRun = EditorGUILayout.Toggle("Build On Run", SettingsManager.Instance.Get<bool>(SettingsKey));
		if (buildOnRun != SettingsManager.Instance.Get<bool>(SettingsKey))
		{
			SettingsManager.Instance.Set(SettingsKey, buildOnRun);
			SettingsManager.Instance.Save();
		}
	}

	public void OnBeforeBuild(string buildLocation)
	{
	}

	public void OnAfterBuild(string buildLocation)
	{
		
	}
}