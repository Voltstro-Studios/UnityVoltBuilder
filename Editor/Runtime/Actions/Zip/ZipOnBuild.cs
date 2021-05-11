#if ZIP //We can only do this if the com.unity.sharp-zip-lib package is installed

using System.Diagnostics;
using System.IO;
using Unity.SharpZipLib.Utils;
using UnityEditor;
using UnityEditor.Build.Reporting;
using Voltstro.UnityBuilder.Settings;
using Debug = UnityEngine.Debug;

namespace Voltstro.UnityBuilder.Actions
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
			if (ZipBuild)
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				string outPath = $"{buildLocation}/../{Path.GetFileName(buildLocation)}.zip";
				Debug.Log("Compressing build...");
				ZipUtility.CompressFolderToZip(outPath, null, buildLocation);

				stopwatch.Stop();
				Debug.Log($"Compressed build to {outPath}. Took {stopwatch.Elapsed.Seconds}s to compress.");
			}
		}

		private static bool ZipBuild
		{
			get
			{
				if (!SettingsManager.Instance.ContainsKey<bool>("ZipBuild"))
				{
					SettingsManager.Instance.Set("ZipBuild", true);
					SettingsManager.Instance.Save();
				}

				return SettingsManager.Instance.Get<bool>("ZipBuild");
			}
			set
			{
				SettingsManager.Instance.Set("ZipBuild", value);
				SettingsManager.Instance.Save();
			}
		}
	}

}

#endif