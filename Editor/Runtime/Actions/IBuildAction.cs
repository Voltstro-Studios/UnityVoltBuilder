﻿using UnityEditor;
using UnityEditor.Build.Reporting;

namespace UnityVoltBuilder.Actions
{
    /// <summary>
    ///     Interface for a build action
    /// </summary>
    public interface IBuildAction
    {
        void OnGUI();

        void OnBeforeBuild(string buildLocation, BuildTarget buildTarget, ref BuildOptions buildOptions);

        void OnAfterBuild(string buildLocation, BuildReport report);
    }
}