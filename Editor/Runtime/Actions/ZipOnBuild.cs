#if ZIPPING_SUPPORT //We can only do this if the com.unity.sharp-zip-lib package is installed

using System.Diagnostics;
using System.IO;
using Unity.SharpZipLib.Utils;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityVoltBuilder.Settings;
using Debug = UnityEngine.Debug;

namespace UnityVoltBuilder.Actions
{
	public class ZipOnBuild : IBuildAction
	{
		public void OnGUI()
		{
			ZipBuild = EditorGUILayout.Toggle("Zip Build", ZipBuild);
		}

		public void OnBeforeBuild(string buildLocation, BuildTarget buildTarget, ref BuildOptions buildOptions)
		{
		}

		public void OnAfterBuild(string buildLocation, BuildReport report)
		{
			if (!ZipBuild) 
				return;
			
			Debug.Log("Compressing build...");
			
			EditorUtility.DisplayProgressBar("Compressing build...", "Compressing build...", 0.5f);
			Stopwatch stopwatch = Stopwatch.StartNew();
			
			string outPath = $"{buildLocation}/../{Path.GetFileName(buildLocation)}.zip";
			ZipUtility.CompressFolderToZip(outPath, null, buildLocation);

			stopwatch.Stop();
			EditorUtility.ClearProgressBar();
			
			Debug.Log($"Compressed build to {outPath}. Took {stopwatch.Elapsed.Seconds}s to compress.");
		}

		private const string ZipBuildKey = "ZipBuild";

		public static bool ZipBuild
		{
			get => SettingsManager.AddOrGetOption(ZipBuildKey, true);
			set => SettingsManager.SetOption(ZipBuildKey, value);
		}
	}
}

#endif