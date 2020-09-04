using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace VoltBuilder
{
	public class CustomSceneSettings : ISceneSettings
	{
		private ReorderableList sceneList;

		public CustomSceneSettings()
		{
			sceneList = CreateScenesList();
		}

		private ReorderableList CreateScenesList()
		{
			ReorderableList list = new ReorderableList(ConfigManager.Config.Scenes, typeof(Scene), true, true, false, false)
			{
				drawHeaderCallback = rect =>
				{
					EditorGUI.LabelField(rect, "Scenes");
				},
				drawElementCallback = (rect, index, active, focused) =>
				{
					EditorGUI.LabelField(rect, (sceneList.list as List<Scene>)?[index].SceneName);
				},
				onReorderCallback = reorderableList =>
				{
					ConfigManager.Config.Scenes = (List<Scene>)reorderableList.list;
					ConfigManager.SaveConfig();
				}
			};

			sceneList = list;
			return list;
		}

		private Scene[] GetAllEnabledScenes()
		{
			List<Scene> scenes = new List<Scene>();
			foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
			{
				if (scene.enabled)
				{
					scenes.Add(new Scene
					{
						SceneName = scene.path.Substring(scene.path.LastIndexOf('/') + 1).Replace(".unity", ""),
						SceneLocation = scene.path
					});
				}
			}

			return scenes.ToArray();
		}

		private void ReloadScenes()
		{
			sceneList.list = GetAllEnabledScenes().ToList();
			ConfigManager.Config.Scenes = (List<Scene>)sceneList.list;
			ConfigManager.SaveConfig();
		}

		/// <inheritdoc/>
		public void DrawSceneSettings()
		{
			sceneList.DoLayoutList();

			GUILayout.BeginHorizontal();

			if (GUILayout.Button("Reload Scene List"))
				ReloadScenes();

			if (GUILayout.Button("Build Settings"))
				EditorWindow.GetWindow(System.Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"), true);

			GUILayout.EndHorizontal();
			GUILayout.Label("Hello!");
		}
	}
}