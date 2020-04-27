using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace VoltBuilder
{
	public class DefaultSceneSettings : ISceneSettings
	{
		/// <inheritdoc/>
		public Scene[] GetAllScenes()
		{
			List<Scene> scenes = new List<Scene>();
			foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
			{
				if (scene.enabled)
				{
					scenes.Add(new Scene
					{
						SceneName = scene.path.Substring(scene.path.LastIndexOf('/') + 1),
						SceneLocation = scene.path
					});
				}
			}

			return scenes.ToArray();
		}

		/// <inheritdoc/>
		public ReorderableList CreateScenesList()
		{
			ReorderableList list = new ReorderableList(GetAllScenes().ToList(), typeof(string), true, true, false, false)
			{
				drawHeaderCallback = rect =>
				{
					EditorGUI.LabelField(rect, "Scenes");
				},
				drawElementCallback = (rect, index, active, focused) =>
				{
					EditorGUI.LabelField(rect, ConfigManager.Config.Scenes[index].SceneName);
				}
			};

			return list;
		}

		/// <inheritdoc/>
		public void DrawSceneSettings(BuildTool buildTool)
		{
			GUILayout.BeginHorizontal();

			if (GUILayout.Button("Reload Scene List"))
			{
				buildTool.ReloadScenes();
				buildTool.SaveSettings();
			}

			if (GUILayout.Button("Build Settings"))
				EditorWindow.GetWindow(System.Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"), true);

			GUILayout.EndHorizontal();
		}
	}
}