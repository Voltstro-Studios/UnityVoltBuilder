using UnityEditor;
using UnityEngine;

namespace VoltBuilder
{
	public class BuildTool : EditorWindow
	{
		private ISceneSettings sceneSettings;
		private IBuildSettings buildSettings;
		private IGameBuild gameBuilder;

		[MenuItem("Tools/Volt Build/Build Tool")]
		public static void ShowWindow()
		{
			GetWindow(typeof(BuildTool), false, "Volt Build Tool");
		}

		private void OnEnable()
		{
			//Change these to use the classes you want
			sceneSettings = new DefaultSceneSettings();
			buildSettings = new DefaultBuildSettings();
			gameBuilder = new DefaultGameBuild();
		}

		private void OnGUI()
		{
			EditorGUILayout.LabelField("Volt Build Tool", EditorStyles.boldLabel);

			string inputName = EditorGUILayout.TextField("Project name: ", ConfigManager.Config.ProjectName);
			if (inputName != ConfigManager.Config.ProjectName)
			{
				ConfigManager.Config.ProjectName = inputName;
				ConfigManager.SaveConfig();
			}

			EditorGUILayout.Space();
			DrawSceneSettings();

			EditorGUILayout.Space();
			DrawBuildSettings();

			EditorGUILayout.Space();
			DrawBuildCommands();
		}

		#region Scene Setting Stuff

		private void DrawSceneSettings()
		{
			EditorGUILayout.LabelField("Scene Settings", EditorStyles.boldLabel);
			sceneSettings.DrawSceneSettings();
		}

		#endregion

		#region Build Setting Stuff

		private void DrawBuildSettings()
		{
			EditorGUILayout.LabelField("Build Settings", EditorStyles.boldLabel);
			EditorGUILayout.Space();

			buildSettings.DrawBuildSettings();
		}

		#endregion

		#region Build Player Stuff

		private void DrawBuildCommands()
		{
			EditorGUILayout.LabelField("Build Commands", EditorStyles.boldLabel);
			EditorGUILayout.Space();
			gameBuilder.DrawAssetBundleCommands();

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Build Game");
			gameBuilder.DrawBuildGameCommands();
		}

		/// <summary>
		/// Gets the path where builds go to
		/// </summary>
		/// <returns></returns>
		public static string GetBuildFolder()
		{
			return $"{Application.dataPath.Replace("Assets", "")}{ConfigManager.Config.BuildDir}";
		}

		#endregion
	}
}