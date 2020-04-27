using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace VoltBuilder
{
	public class DefaultGameBuild : IGameBuild
	{
		/// <inheritdoc/>
		public void DrawAssetBundleCommands(BuildTool buildTool)
		{
			EditorGUILayout.LabelField("Asset Bundles");
			GUILayout.BeginHorizontal();

			if (GUILayout.Button("Build Bundles"))
				BuildBundles($"{buildTool.GetBuildFolder()}/Bundles", BuildAssetBundleOptions.UncompressedAssetBundle, false);

			if (GUILayout.Button("Force Build Bundles"))
				BuildBundles($"{buildTool.GetBuildFolder()}/Bundles", BuildAssetBundleOptions.UncompressedAssetBundle, true);

			GUILayout.EndHorizontal();
		}

		/// <inheritdoc/>
		public void BuildBundles(string buildPath, BuildAssetBundleOptions options, bool forced)
		{
			if (!Directory.Exists(buildPath))
				Directory.CreateDirectory(buildPath);

			if (forced)
			{
				options |= BuildAssetBundleOptions.ForceRebuildAssetBundle;
			}

			BuildPipeline.BuildAssetBundles(buildPath, options, BuildTarget.StandaloneWindows64);
		}

		/// <inheritdoc/>
		public void DrawBuildGameCommands(BuildTool buildTool)
		{
			GUILayout.BeginHorizontal();

			if(GUILayout.Button("Build Game"))
				DoGameBuild($"{buildTool.GetBuildFolder()}", $"{ConfigManager.Config.ProjectName}-Quick/", $"{ConfigManager.Config.ProjectName}", false);
			if(GUILayout.Button("Scripts only"))
				DoGameBuild($"{buildTool.GetBuildFolder()}", $"{ConfigManager.Config.ProjectName}-Quick/", $"{ConfigManager.Config.ProjectName}", true);

			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			if(GUILayout.Button("Full New Build"))
				FullNewBuild(buildTool.GetBuildFolder(), ConfigManager.Config.ProjectName);

			if (GUILayout.Button("Open Build Folder"))
			{
				if (Directory.Exists(buildTool.GetBuildFolder()))
				{
					Process p = new Process
					{
						StartInfo = new ProcessStartInfo("explorer.exe", buildTool.GetBuildFolder().Replace("/", "\\"))
					};
					p.Start();
				}
				else
				{
					Debug.LogError($"Build folder doesn't exist yet!");
				}
			}

			GUILayout.EndHorizontal();
		}

		/// <inheritdoc/>
		public BuildReport BuildGame(string[] levels, string buildPath, string exeName, BuildTarget target, BuildOptions options)
		{
			if (!Directory.Exists(buildPath))
				Directory.CreateDirectory(buildPath);

			if (target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64)
				exeName += ".exe";

			return BuildPipeline.BuildPlayer(levels, buildPath + exeName, target, options);
		}

		#region Private Methods

		/// <summary>
		/// Does a game build
		/// </summary>
		/// <param name="buildFolder"></param>
		/// <param name="folderName"></param>
		/// <param name="projectName"></param>
		/// <param name="scriptsOnly"></param>
		private void DoGameBuild(string buildFolder, string folderName, string projectName, bool scriptsOnly)
		{
			Debug.Log($"Building game to `{buildFolder}{folderName}`...");

			List<string> levels = ConfigManager.Config.Scenes.Select(scene => scene.SceneLocation).ToList();

			bool isDevBuild = false;
			bool copyPdbFiles = false;
			bool serverBuild = false;
			bool zipFiles = false;

			//If our config manager is using the default build config, then setup these variables
			if (ConfigManager.GetBuildConfig(out DefaultBuildConfig config))
			{
				isDevBuild = config.DevBuild;
				copyPdbFiles = config.CopyPDBFiles;
				serverBuild = config.ServerBuild;
				zipFiles = config.ZipFiles;
			}

			BuildOptions buildOptions = BuildOptions.None;
			if (isDevBuild)
				buildOptions |= BuildOptions.Development;

			if (serverBuild)
				buildOptions |= BuildOptions.EnableHeadlessMode;

			if (scriptsOnly)
				buildOptions |= BuildOptions.BuildScriptsOnly;

			EditorUserBuildSettings.SetPlatformSettings("Standalone", "CopyPDBFiles", copyPdbFiles ? "true" : "false");

			BuildReport result = BuildGame(levels.ToArray(), buildFolder + folderName, projectName,
				BuildTarget.StandaloneWindows64, buildOptions);

			if (result.summary.result == BuildResult.Failed)
			{
				Debug.LogError("BUILD FAILED!");
				return;
			}

			Debug.Log("Build was a success!");

			//Zip files
			if (zipFiles)
			{
				Debug.Log("Zipping build...");
				IZip zip = new DefaultZip();
				zip.CompressDir(buildFolder + folderName, buildFolder + folderName.Replace("/", "") + ".zip");
				Debug.Log("Done!");
			}
		}

		/// <summary>
		/// Does a complete new game build, to a new folder
		/// </summary>
		/// <param name="buildFolder"></param>
		/// <param name="projectName"></param>
		private void FullNewBuild(string buildFolder, string projectName)
		{
			string buildFolderName = $"{projectName}-{DateTime.Now:yy-MM-dd}";
			int count = 0;
			bool folderExists = true;

			while (folderExists)
			{
				//0
				if (count == 0)
				{
					if (Directory.Exists($"{buildFolder}{buildFolderName}/"))
					{
						Debug.Log("Build for today already exists!");
						//The directory already exists, we have a build done already today
						count++;
					}
					else
					{
						//No build for today
						folderExists = false;
						continue;
					}
				}

				//It exists
				if (Directory.Exists($"{buildFolder}{buildFolderName}-{count}/"))
				{
					count++;
				}
				else
				{
					buildFolderName += $"-{count}";
					folderExists = false;
				}
			}

			Directory.CreateDirectory($"{buildFolder}{buildFolderName}/");

			DoGameBuild(buildFolder, $"{buildFolderName}/", projectName, false);
		}

		#endregion
	}
}