using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using VoltUnityBuilder.Actions;
using VoltUnityBuilder.GUI;
using VoltUnityBuilder.Settings;
using Debug = UnityEngine.Debug;

namespace VoltUnityBuilder.Build
{
	public static class GameBuilder
	{
		private const string CopyPdbFilesEditorString = "CopyPDBFiles";

		internal static void DrawOptions()
		{
			EditorGUILayout.BeginVertical(GUIStyles.DropdownContentStyle);

			GUILayout.Label("Build Commands", GUIStyles.DropdownHeaderStyle);

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Build Player"))
				BuildGameGUI($"{GetBuildDirectory()}{PlayerSettings.productName}-Quick/{PlayerSettings.productName}");
			if (GUILayout.Button("Scripts Only"))
				BuildGameGUI($"{GetBuildDirectory()}{PlayerSettings.productName}-Quick/{PlayerSettings.productName}", true);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("New Build"))
				BuildGameGUI(
					$"{GetBuildDirectory()}{PlayerSettings.productName}-{DateTime.Now.ToString(SettingsManager.BuildFolderNameStyle)}/{PlayerSettings.productName}");
			if (GUILayout.Button("Open Build Folder"))
			{
				if (!Directory.Exists(GetBuildDirectory()))
					Directory.CreateDirectory(GetBuildDirectory());

				Process.Start(GetBuildDirectory());
			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.EndVertical();
		}

		private static void BuildGameGUI(string buildDir, bool scriptsOnly = false)
		{
			BuildGame(buildDir, SettingsManager.BuildTarget, SettingsManager.ServerBuild, SettingsManager.DevelopmentBuild, 
				SettingsManager.AutoconnectProfiler, SettingsManager.DeepProfiling, SettingsManager.ScriptDebugging, 
				SettingsManager.CopyPdbFiles, scriptsOnly);
			GUIUtility.ExitGUI();
		}

		public static void BuildGame(string buildDir, BuildTarget buildTarget, bool headLessBuild, bool devBuild = false, bool autoConnectProfiler = false, 
			bool deepProfiling = false, bool scriptDebugging = false, bool copyPdbFiles = false, bool scriptsOnly = false)
		{
			Debug.Log($"Starting game build at {DateTime.Now:G}...");
			Stopwatch stopwatch = Stopwatch.StartNew();

			if (buildTarget == BuildTarget.StandaloneWindows || buildTarget == BuildTarget.StandaloneWindows64)
				buildDir += ".exe";

			Debug.Log($"Building to '{buildDir}'...");

			//Set target group
			#region Target Group

			BuildTargetGroup targetGroup;
			switch (buildTarget)
			{
				case BuildTarget.StandaloneLinux64:
				case BuildTarget.StandaloneWindows64:
				case BuildTarget.StandaloneOSX:
				case BuildTarget.StandaloneWindows:
					targetGroup = BuildTargetGroup.Standalone;
					break;
				case BuildTarget.iOS:
					targetGroup = BuildTargetGroup.iOS;
					break;
				case BuildTarget.Android:
					targetGroup = BuildTargetGroup.Android;
					break;
				case BuildTarget.WebGL:
					targetGroup = BuildTargetGroup.WebGL;
					break;
				case BuildTarget.WSAPlayer:
					targetGroup = BuildTargetGroup.WSA;
					break;
				case BuildTarget.PS4:
					targetGroup = BuildTargetGroup.PS4;
					break;
				case BuildTarget.XboxOne:
					targetGroup = BuildTargetGroup.XboxOne;
					break;
				case BuildTarget.tvOS:
					targetGroup = BuildTargetGroup.tvOS;
					break;
				case BuildTarget.Switch:
					targetGroup = BuildTargetGroup.Switch;
					break;
				case BuildTarget.Lumin:
					targetGroup = BuildTargetGroup.Lumin;
					break;
				case BuildTarget.Stadia:
					targetGroup = BuildTargetGroup.Stadia;
					break;
				case BuildTarget.CloudRendering:
					targetGroup = BuildTargetGroup.CloudRendering;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			#endregion

			//Setup build options
			BuildOptions options = BuildOptions.None;

			//Server/Headless mode
			if (headLessBuild)
				options |= BuildOptions.EnableHeadlessMode;

			//Copy PDB files
			string existingCopyPdbFilesOptions =
				EditorUserBuildSettings.GetPlatformSettings("Standalone", CopyPdbFilesEditorString);

			if (copyPdbFiles)
				EditorUserBuildSettings.SetPlatformSettings("Standalone", CopyPdbFilesEditorString,
					SettingsManager.CopyPdbFiles ? "true" : "false");

			//Dev build
			if (devBuild)
			{
				options |= BuildOptions.Development;

				if (autoConnectProfiler)
					options |= BuildOptions.ConnectWithProfiler;

				if (deepProfiling)
					options |= BuildOptions.EnableDeepProfilingSupport;

				if (scriptDebugging)
					options |= BuildOptions.AllowDebugging;
			}

			//Scripts only
			if (scriptsOnly)
				options |= BuildOptions.BuildScriptsOnly;

			//Run build action pre-build
			Debug.Log("Running build actions pre build...");
			try
			{
				BuildActions.RunPreActions(Path.GetDirectoryName(buildDir), buildTarget, ref options);
			}
			catch (Exception ex)
			{
				Debug.LogError($"An error occurred while running a build action's pre build! {ex}");
			}

			Debug.Log("Build actions pre build done!");

			Debug.Log("Building player...");

			//Build the player
			BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
			{
				locationPathName = buildDir,
				target = buildTarget,
				options = options,
				targetGroup = targetGroup,
				scenes = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray()
			});

			//Set CopyPDBFiles to it's original setting
			EditorUserBuildSettings.SetPlatformSettings("Standalone", CopyPdbFilesEditorString,
				existingCopyPdbFilesOptions);

			//If the build failed
			if (report.summary.result != BuildResult.Succeeded)
			{
				stopwatch.Stop();
				Debug.LogError($"Build failed for some reason! Completed in {stopwatch.ElapsedMilliseconds / 1000}s.");
				return;
			}

			//Run Build Action post build
			try
			{
				BuildActions.RunPostActions(Path.GetDirectoryName(buildDir), report);
			}
			catch (Exception ex)
			{
				Debug.LogError($"An error occurred while running a build action's post build! {ex}");
			}

			//End
			stopwatch.Stop();
			Debug.Log($"Build done in {stopwatch.ElapsedMilliseconds / 1000}s!");
		}

		public static string GetBuildDirectory()
		{
			return $"{Application.dataPath.Replace("Assets", "")}{SettingsManager.BuildLocation}";
		}
	}
}