using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Voltstro.UnityBuilder.Actions;
using Voltstro.UnityBuilder.GUI;
using Voltstro.UnityBuilder.Settings;

internal sealed class CopyFilesOnBuild : IBuildAction
{
	public void OnGUI()
	{
		if(GUILayout.Button("Open Files To Copy Window"))
			EditorWindow.GetWindow(typeof(CopyFilesWindow), true, "Files To Copy", true);
	}

	public void OnBeforeBuild(string buildLocation)
	{
		
	}

	public void OnAfterBuild(string buildLocation)
	{
		for (int i = 0; i < FilesToCopy.Count; i++)
		{
			if(!File.Exists(FilesToCopy[i])) continue;

			string fileDir = $"{buildLocation}/{CopyToWhere[i]}";
			if (!Directory.Exists(Path.GetDirectoryName(fileDir)))
				Directory.CreateDirectory(Path.GetDirectoryName(fileDir));

			File.Copy(FilesToCopy[i], fileDir, true);
		}
	}

	private static List<string> FilesToCopy
	{
		get
		{
			if (!SettingsManager.Instance.ContainsKey<List<string>>("FilesToCopy"))
			{
				SettingsManager.Instance.Set("FilesToCopy", new List<string>());
				SettingsManager.Instance.Save();
			}

			return SettingsManager.Instance.Get<List<string>>("FilesToCopy");
		}
		set
		{
			SettingsManager.Instance.Set("FilesToCopy", value);
			SettingsManager.Instance.Save();
		}
	}

	private static List<string> CopyToWhere
	{
		get
		{
			if (!SettingsManager.Instance.ContainsKey<List<string>>("CopyToWhere"))
			{
				SettingsManager.Instance.Set("CopyToWhere", new List<string>());
				SettingsManager.Instance.Save();
			}

			return SettingsManager.Instance.Get<List<string>>("CopyToWhere");
		}
		set
		{
			SettingsManager.Instance.Set("CopyToWhere", value);
			SettingsManager.Instance.Save();
		}
	}

	private class CopyFilesWindow : EditorWindow
	{
		private CopyFilesWindow()
		{
			minSize = new Vector2(404, 142);
		}

		private void OnGUI()
		{
			EditorGUILayout.LabelField("Copy Files On Build", GUIStyles.DropdownHeaderStyle);
			EditorGUILayout.Space();

			EditorGUILayout.BeginVertical(GUIStyles.DropdownContentStyle);
			EditorGUILayout.LabelField("These files will be copied to the build folder after a successful build");
			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.LabelField("File to copy");
			EditorGUILayout.LabelField("Where to (in build)");

			EditorGUILayout.EndHorizontal();

			for (int i = 0; i < FilesToCopy.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();

				FilesToCopy[i] =
					EditorGUILayout.TextField(FilesToCopy[i]);

				CopyToWhere[i] =
					EditorGUILayout.TextField(CopyToWhere[i]);

				if (GUILayout.Button("-"))
				{
					List<string> filesToCopy = FilesToCopy;
					List<string> copyToWhere = CopyToWhere;

					filesToCopy.Remove(FilesToCopy[i]);
					copyToWhere.Remove(CopyToWhere[i]);

					FilesToCopy = filesToCopy;
					CopyToWhere = copyToWhere;
				}

				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Add New"))
			{
				List<string> filesToCopy = FilesToCopy;
				List<string> copyToWhere = CopyToWhere;

				filesToCopy.Add("Assets/Example.txt");
				copyToWhere.Add("Example.txt");

				FilesToCopy = filesToCopy;
				CopyToWhere = copyToWhere;
			}

			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
		}
	}
}