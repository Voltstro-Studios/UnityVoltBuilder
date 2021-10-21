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
        private static readonly List<Action> WindowDraws = new List<Action>
        {
            BuildSettings.DrawOptions,
            BuildActions.DrawOptions,
#if ADDRESSABLES_SUPPORT
            AddressablesBuilder.DrawOptions,
#endif
            GameBuilder.DrawOptions
        };

        public void OnGUI()
        {
            //Main title
            EditorGUILayout.Space(2f);
            EditorGUILayout.LabelField("Unity Volt Builder", GUIStyles.TitleStyle);
            EditorGUILayout.Space(15f);

            foreach (Action windowDraw in WindowDraws)
            {
                try
                {
                    windowDraw.Invoke();
                    EditorGUILayout.Space(8f);
                }
                catch (ExitGUIException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error occured while drawing options! {ex}");
                }
            }
        }

        [PublicAPI]
        public static void AddOnDraw([NotNull] Action drawAction)
        {
            if (drawAction == null)
                throw new ArgumentNullException(nameof(drawAction), "Draw action cannot be null!");

            if (WindowDraws.Contains(drawAction))
            {
                Debug.LogError("Draw action has already been added!");
                return;
            }
            WindowDraws.Add(drawAction);
        }

        [MenuItem("Tools/Unity Volt Builder/Volt Builder")]
        public static void ShowWindow()
        {
            GetWindow(typeof(BuildToolWindow), false, "Volt Builder");
        }

        [MenuItem("Tools/Unity Volt Builder/Report an issue")]
        public static void OpenIssues()
        {
            Application.OpenURL("https://github.com/Voltstro-Studios/UnityVoltBuilder/issues");
        }
    }
}