using System.Collections.Generic;
using UnityEditor;
using UnityVoltBuilder.Actions;

namespace UnityVoltBuilder.Build
{
    /// <summary>
    ///     Build options for <see cref="GameBuilder"/>
    /// </summary>
    public readonly struct GameBuildOptions
    {
        public readonly string BuildDir;

        public readonly BuildTarget BuildTarget;

        public readonly bool HeadlessBuild;

        public readonly bool DevBuild;

        public readonly bool AutoConnectProfiler;

        public readonly bool DeepProfiling;

        public readonly bool ScriptDebugging;

        public readonly bool CopyPdbFiles;

        public readonly bool ScriptsOnly;

        public readonly List<IBuildAction> BuildActions;

        public GameBuildOptions(string buildDir, BuildTarget buildTarget, List<IBuildAction> buildActions = null,
            bool headlessBuild = false, 
            bool devBuild = false, 
            bool autoConnectProfiler = false, 
            bool deepProfiling = false, 
            bool scriptDebugging = false, 
            bool copyPdbFiles = false, 
            bool scriptsOnly = false)
        {
            BuildDir = buildDir;
            BuildTarget = buildTarget;
            HeadlessBuild = headlessBuild;
            DevBuild = devBuild;
            AutoConnectProfiler = autoConnectProfiler;
            DeepProfiling = deepProfiling;
            ScriptDebugging = scriptDebugging;
            CopyPdbFiles = copyPdbFiles;
            ScriptsOnly = scriptsOnly;

            BuildActions = buildActions;
            if (buildActions == null)
                BuildActions = new List<IBuildAction>();
        }
    }
}