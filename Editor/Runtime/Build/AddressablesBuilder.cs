#if ADDRESSABLES_SUPPORT

using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityVoltBuilder.GUI;
using Debug = UnityEngine.Debug;

namespace UnityVoltBuilder.Build
{
    public static class AddressablesBuilder
    {
        private static AddressableAssetSettings settings;
        
        static AddressablesBuilder()
        {
            settings = AddressableAssetSettingsDefaultObject.Settings;
        }
        
        public static void DrawOptions()
        {
            EditorGUILayout.BeginVertical(GUIStyles.DropdownContentStyle);

            GUILayout.Label("Addressable Commands", GUIStyles.DropdownHeaderStyle);
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Build Addressables"))
                BuildAddressables();
            
            if (GUILayout.Button("Clean Build"))
                CleanAddressablesBuild();
            
            EditorGUILayout.EndHorizontal();

            string[] names = new string[settings.DataBuilders.Count];
            for (int i = 0; i < settings.DataBuilders.Count; i++)
            {
                IDataBuilder m = settings.GetDataBuilder(i);
                if (m.CanBuildData<AddressablesPlayModeBuildResult>())
                    names[i] = m.Name;
            }
            
            settings.ActivePlayModeDataBuilderIndex = EditorGUILayout.Popup("Play Mode Script", settings.ActivePlayModeDataBuilderIndex, names);
            
            EditorGUILayout.EndVertical();
        }
        
        /// <summary>
        ///     Builds addressables
        /// </summary>
        [PublicAPI]
        public static void BuildAddressables()
        {
            Debug.Log("Building addressables...");
            
            Stopwatch stopwatch = Stopwatch.StartNew();
            AddressableAssetSettings.BuildPlayerContent();
            stopwatch.Stop();

            Debug.Log($"Addressable Build done in {stopwatch.ElapsedMilliseconds / 1000}s!");
        }

        /// <summary>
        ///     Cleans addressables build
        /// </summary>
        [PublicAPI]
        public static void CleanAddressablesBuild()
        {
            Debug.Log("Cleaning addressables build...");
            
            EditorUtility.DisplayProgressBar("Cleaning Addressables Build", "Cleaning addressables build...", 0.5f);
            Stopwatch stopwatch = Stopwatch.StartNew();
            AddressableAssetSettings.CleanPlayerContent();
            stopwatch.Stop();
            EditorUtility.ClearProgressBar();

            Debug.Log($"Build clean done in {stopwatch.ElapsedMilliseconds / 1000}s!");
        }
    }
}

#endif