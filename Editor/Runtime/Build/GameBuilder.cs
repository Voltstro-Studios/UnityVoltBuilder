﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityVoltBuilder.Actions;
using UnityVoltBuilder.GUI;
using UnityVoltBuilder.Settings;
using Debug = UnityEngine.Debug;

namespace UnityVoltBuilder.Build
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
                BuildGameGUI($"{GetBuildDirectory()}{PlayerSettings.productName}-Quick/{PlayerSettings.productName}",
                    true);
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
            BuildGame(ToGameBuildOptions(buildDir, scriptsOnly));
            GUIUtility.ExitGUI();
        }
        
        /// <summary>
        ///     Takes all the options in <see cref="SettingsManager"/> and converts them to a <see cref="GameBuildOptions"/>
        /// </summary>
        /// <returns></returns>
        [PublicAPI]
        public static GameBuildOptions ToGameBuildOptions(string buildLocation, bool scriptsOnly, 
            BuildTarget? buildTarget = null, 
            [CanBeNull] List<IBuildAction> buildActions = null) => 
            new GameBuildOptions(
            buildLocation,
            buildTarget ?? SettingsManager.BuildTarget,
            buildActions ?? BuildActions.GetBuildActions(),
            SettingsManager.ServerBuild, 
            SettingsManager.DevelopmentBuild, 
            SettingsManager.AutoconnectProfiler, 
            SettingsManager.DeepProfiling, 
            SettingsManager.ScriptDebugging, 
            SettingsManager.CopyPdbFiles,
            scriptsOnly);

        /// <summary>
        ///     Builds the game
        /// </summary>
        /// <param name="gameBuildOptions"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [PublicAPI]
        public static void BuildGame(GameBuildOptions gameBuildOptions)
        {
            Debug.Log($"Starting game build at {DateTime.Now:G}...");
            Stopwatch stopwatch = Stopwatch.StartNew();
            
            string fullBuildDir = gameBuildOptions.BuildDir;
            if (gameBuildOptions.BuildTarget == BuildTarget.StandaloneWindows || gameBuildOptions.BuildTarget == BuildTarget.StandaloneWindows64)
                fullBuildDir += ".exe";

            string buildDir = Path.GetDirectoryName(fullBuildDir);
            
            Debug.Log($"Building to '{fullBuildDir}'...");

            //Set target group
            #region Target Group

            BuildTargetGroup targetGroup;
            switch (gameBuildOptions.BuildTarget)
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
            if (gameBuildOptions.HeadlessBuild)
                options |= BuildOptions.EnableHeadlessMode;

            //Copy PDB files
            string existingCopyPdbFilesOptions =
                EditorUserBuildSettings.GetPlatformSettings("Standalone", CopyPdbFilesEditorString);

            if (gameBuildOptions.CopyPdbFiles)
                EditorUserBuildSettings.SetPlatformSettings("Standalone", CopyPdbFilesEditorString,
                    SettingsManager.CopyPdbFiles ? "true" : "false");

            //Dev build
            if (gameBuildOptions.DevBuild)
            {
                options |= BuildOptions.Development;

                if (gameBuildOptions.AutoConnectProfiler)
                    options |= BuildOptions.ConnectWithProfiler;

                if (gameBuildOptions.DeepProfiling)
                    options |= BuildOptions.EnableDeepProfilingSupport;

                if (gameBuildOptions.ScriptDebugging)
                    options |= BuildOptions.AllowDebugging;
            }

            //Scripts only
            if (gameBuildOptions.ScriptsOnly)
                options |= BuildOptions.BuildScriptsOnly;

            //Run build action pre-build
            Debug.Log("Running build actions pre build...");
            try
            {
                foreach (IBuildAction buildAction in gameBuildOptions.BuildActions)
                    buildAction?.OnBeforeBuild(buildDir, gameBuildOptions.BuildTarget, ref options);
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
                locationPathName = fullBuildDir,
                target = gameBuildOptions.BuildTarget,
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
                foreach (IBuildAction buildAction in gameBuildOptions.BuildActions)
                    buildAction?.OnAfterBuild(buildDir, report);
            }
            catch (Exception ex)
            {
                Debug.LogError($"An error occurred while running a build action's post build! {ex}");
            }

            //End
            stopwatch.Stop();
            Debug.Log($"Build done in {stopwatch.ElapsedMilliseconds / 1000}s!");
        }

        /// <summary>
        ///     Gets the set build directory
        /// </summary>
        /// <returns></returns>
        [PublicAPI]
        public static string GetBuildDirectory()
        {
            return $"{Application.dataPath.Replace("Assets", "")}{SettingsManager.BuildLocation}";
        }
    }
}