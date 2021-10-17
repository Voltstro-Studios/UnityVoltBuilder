using System.Diagnostics;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityVoltBuilder.Settings;

namespace UnityVoltBuilder.Actions
{
    public sealed class RunOnBuild : IBuildAction
    {
        public void OnGUI()
        {
            RunOnBuildSetting = EditorGUILayout.Toggle("Run on Build", RunOnBuildSetting);
            RunOnBuildArguments = EditorGUILayout.TextField("Arguments", RunOnBuildArguments);

            EditorGUILayout.Space();
        }

        public void OnBeforeBuild(string buildLocation, BuildTarget buildTarget, ref BuildOptions buildOptions)
        {
        }

        public void OnAfterBuild(string buildLocation, BuildReport report)
        {
            if (RunOnBuildSetting)
                Process.Start(buildLocation, RunOnBuildArguments);
        }
        
        private const string RunOnBuildSettingKey = "RunOnBuild";
        
        [PublicAPI]
        public static bool RunOnBuildSetting
        {
            get => SettingsManager.AddOrGetOption(RunOnBuildSettingKey, true);
            set => SettingsManager.SetOption(RunOnBuildSettingKey, value);
        }

        private const string RunOnBuildArgumentsKey = "RunOnBuildArguments";
        
        [PublicAPI]
        public static string RunOnBuildArguments
        {
            get => SettingsManager.AddOrGetOption(RunOnBuildArgumentsKey, string.Empty);
            set => SettingsManager.SetOption(RunOnBuildArgumentsKey, value);
        }
    }
}