using UnityEditor;
using UnityEngine;
using VoltBuilder;

[InitializeOnLoad]
public class Startup
{
	static Startup()
	{
		BuildTool.SceneSettings = new CustomSceneSettings();
		BuildTool.BuildSettings = new CustomBuildSettings();
		BuildTool.GameBuilder = new CustomGameBuilder();
		Debug.Log("Overridden BuildTool's interfaces.");
	}
}