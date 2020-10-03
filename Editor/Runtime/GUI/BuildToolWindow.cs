using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Voltstro.UnityBuilder.Settings;

public class BuildToolWindow : EditorWindow
{
	[MenuItem("Tools/Volt Build/Build Tool")]
	public static void ShowWindow()
	{
		GetWindow(typeof(BuildToolWindow), false, "Volt Build Tool");
	}

	public void OnGUI()
	{
		//Main title
		EditorGUILayout.LabelField("Volt Unity Build", GUIStyles.TitleStyle);
		EditorGUILayout.Space();

		DrawSceneSettings();
		BuildSettings.DrawOptions();
		EditorGUILayout.Space();
		BuildActions.Instance.DrawOptions();
	}

	private void DrawSceneSettings()
	{
		if (!SettingsManager.Instance.ContainsKey<List<string>>("scenes"))
		{
			SettingsManager.Instance.Set("scenes", new List<string>());
			SettingsManager.Instance.Save();
		}

		//TODO: Draw a list of scenes
	}
}