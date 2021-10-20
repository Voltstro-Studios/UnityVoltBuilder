using UnityEditor;
using UnityEngine;
using UnityVoltBuilder.Settings;

namespace UnityVoltBuilder.GUI
{
    internal static class BuildSettings
    {
        private static bool buildSetting;

        internal static void DrawOptions()
        {
            GUIStyles.DrawDropdownButton("Build Settings", ref buildSetting);
            if (buildSetting)
            {
                EditorGUILayout.BeginVertical(GUIStyles.DropdownContentStyle);
                DrawHeader("Build Path Settings");

                SettingsManager.BuildLocation =
                    EditorGUILayout.TextField("Build Location", SettingsManager.BuildLocation);
                SettingsManager.BuildFolderNameStyle =
                    EditorGUILayout.TextField("Build Folder Name", SettingsManager.BuildFolderNameStyle);

                EditorGUILayout.Space(10);
                DrawHeader("Build Player Settings");

                SettingsManager.BuildTarget =
                    (BuildTarget)EditorGUILayout.EnumPopup("Build Target", SettingsManager.BuildTarget);
                SettingsManager.ServerBuild = EditorGUILayout.Toggle("Server Build", SettingsManager.ServerBuild);
                SettingsManager.CopyPdbFiles = EditorGUILayout.Toggle("Copy PDB Files", SettingsManager.CopyPdbFiles);

                EditorGUILayout.Space(10);
                DrawHeader("Build Development Settings");
                SettingsManager.DevelopmentBuild =
                    EditorGUILayout.Toggle("Development Build", SettingsManager.DevelopmentBuild);

                if (SettingsManager.DevelopmentBuild)
                {
                    SettingsManager.AutoconnectProfiler =
                        EditorGUILayout.Toggle("Autoconnect Profiler", SettingsManager.AutoconnectProfiler);
                    SettingsManager.DeepProfiling =
                        EditorGUILayout.Toggle("Deep Profiling", SettingsManager.DeepProfiling);
                    SettingsManager.ScriptDebugging =
                        EditorGUILayout.Toggle("Script Debugging", SettingsManager.ScriptDebugging);
                }

                EditorGUILayout.EndVertical();
            }
        }

        private static void DrawHeader(string header)
        {
            GUILayout.Label(header, GUIStyles.DropdownHeaderStyle);
        }
    }
}