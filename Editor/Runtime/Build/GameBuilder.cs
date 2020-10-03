using System;
using System.Diagnostics;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using Voltstro.UnityBuilder.Settings;
using Debug = UnityEngine.Debug;

public static class GameBuilder
{
	internal static void DrawOptions()
	{
		if (GUILayout.Button("Build Player"))
		{
			BuildGame($"{GetBuildDirectory()}{PlayerSettings.productName}-Quick/{PlayerSettings.productName}");
		}
	}

	public static void BuildGame(string buildDir)
	{
		Debug.Log($"Starting game build at {DateTime.Now:G}...");
		Stopwatch stopwatch = Stopwatch.StartNew();

		//Get our settings that we need
		BuildTarget buildTarget = SettingsManager.BuildTarget;

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
		if (SettingsManager.ServerBuild)
			options |= BuildOptions.EnableHeadlessMode;

		//Dev build
		if (SettingsManager.DevelopmentBuild)
			options |= BuildOptions.Development;

		//Copy PDB files
		if(SettingsManager.CopyPdbFiles)
			EditorUserBuildSettings.SetPlatformSettings("Standalone", "CopyPDBFiles", SettingsManager.CopyPdbFiles ? "true" : "false");

		//Run build action pre-build
		Debug.Log("Running build actions pre build...");
		try
		{
			BuildActions.RunPreActions(buildDir);
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
			targetGroup = targetGroup
		});

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
			BuildActions.RunPostActions(buildDir);
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