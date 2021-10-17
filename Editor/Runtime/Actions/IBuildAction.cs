using UnityEditor;
using UnityEditor.Build.Reporting;

namespace UnityVoltBuilder.Actions
{
    public interface IBuildAction
    {
        void OnGUI();

        void OnBeforeBuild(string buildLocation, BuildTarget buildTarget, ref BuildOptions buildOptions);

        void OnAfterBuild(string buildLocation, BuildReport report);
    }
}