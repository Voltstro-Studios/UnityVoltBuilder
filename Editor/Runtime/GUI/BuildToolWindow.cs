using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityVoltBuilder.Actions;
using UnityVoltBuilder.Build;

namespace UnityVoltBuilder.GUI
{
    public sealed class BuildToolWindow : EditorWindow
    {
        private readonly List<Action> windowDraws = new List<Action>
        {
            BuildSettings.DrawOptions,
            BuildActions.DrawOptions,
            GameBuilder.DrawOptions
        };

        public void OnGUI()
        {
            //Main title
            EditorGUILayout.Space(2f);
            EditorGUILayout.LabelField("Unity Volt Builder", GUIStyles.TitleStyle);
            EditorGUILayout.Space(15f);

            foreach (Action windowDraw in windowDraws)
            {
                try
                {
                    windowDraw.Invoke();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error occured while drawing options! {ex}");
                }
            }
        }

        [PublicAPI]
        public void AddOnDraw([NotNull] Action drawAction)
        {
            if (drawAction == null)
                throw new ArgumentNullException(nameof(drawAction), "Draw action cannot be null!");

            if (windowDraws.Contains(drawAction))
            {
                Debug.LogError("Draw action has already been added!");
                return;
            }
            windowDraws.Add(drawAction);
        }

        [MenuItem("Tools/Volt Unity Builder/Volt Builder")]
        public static void ShowWindow()
        {
            GetWindow(typeof(BuildToolWindow), false, "Volt Builder");
        }

        [MenuItem("Tools/Volt Unity Builder/Report an issue")]
        public static void OpenIssues()
        {
            Application.OpenURL("https://github.com/Voltstro-Studios/UnityVoltBuilder/issues");
        }
    }
}