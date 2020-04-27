using UnityEditor;
using UnityEditor.Build.Reporting;

public interface IGameBuild
{
	void DrawAssetBundleCommands(BuildTool buildTool);
	void BuildBundles(string buildPath, BuildAssetBundleOptions options, bool forced);

	void DrawBuildGameCommands(BuildTool buildTool);
	BuildReport BuildGame(string[] levels, string buildPath, string exeName, BuildTarget target, BuildOptions options);
}