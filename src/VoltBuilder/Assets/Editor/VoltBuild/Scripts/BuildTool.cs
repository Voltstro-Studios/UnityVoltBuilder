using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class BuildTool : EditorWindow
{
	private ReorderableList scenesToBuildList;

	private string projectName;

	private ISceneSettings sceneSettings;
	private IBuildSettings buildSettings;
	private IGameBuild gameBuilder;

	[MenuItem("Tools/Volt Build/Build Tool")]
	public static void ShowWindow()
	{
		GetWindow(typeof(BuildTool));
	}

	private void OnGUI()
	{
		EditorGUILayout.LabelField("Volt Build Tool", EditorStyles.boldLabel);

		string inputName = EditorGUILayout.TextField("Project name: ", projectName);
		if (inputName != projectName)
		{
			projectName = inputName;
			SaveSettings();
		}

		EditorGUILayout.Space();
		DrawSceneSettings();

		EditorGUILayout.Space();
		DrawBuildSettings();

		EditorGUILayout.Space();
		DrawBuildCommands();
	}

	private void OnEnable()
	{
		sceneSettings = new DefaultSceneSettings();
		buildSettings = new DefaultBuildSettings();
		gameBuilder = new DefaultGameBuild();
		
		ReloadScenes();

		LoadSettings();
	}

	#region Config Mangement

	public void SaveSettings()
	{
		ConfigManager.Config.ProjectName = projectName;
		ConfigManager.Config.Scenes = (List<Scene>)scenesToBuildList.list;
		ConfigManager.SaveConfig();
	}

	public void LoadSettings()
	{
		projectName = ConfigManager.Config.ProjectName;
		scenesToBuildList.list = ConfigManager.Config.Scenes;
	}

	#endregion

	#region Scene Setting Stuff

	public void ReloadScenes()
	{
		scenesToBuildList = sceneSettings.CreateScenesList();

		scenesToBuildList.onReorderCallback += list => SaveSettings();
	}

	private void DrawSceneSettings()
	{
		EditorGUILayout.LabelField("Scene Settings", EditorStyles.boldLabel);
		scenesToBuildList.DoLayoutList();

		sceneSettings.DrawSceneSettings(this);
	}

	#endregion

	#region Build Setting Stuff

	private void DrawBuildSettings()
	{
		EditorGUILayout.LabelField("Build Settings", EditorStyles.boldLabel);
		EditorGUILayout.Space();

		buildSettings.DrawBuildSettings(this);
	}

	#endregion

	#region Build Player Stuff

	private void DrawBuildCommands()
	{
		EditorGUILayout.LabelField("Build Commands", EditorStyles.boldLabel);
		EditorGUILayout.Space();

		gameBuilder.DrawAssetBundleCommands(this);

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Build Game");

		gameBuilder.DrawBuildGameCommands(this);
	}

	public string GetBuildFolder()
	{
		return $"{Application.dataPath.Replace("Assets", "")}{ConfigManager.Config.BuildDir}";
	}

	#endregion
}