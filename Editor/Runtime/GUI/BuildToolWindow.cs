using UnityEditor;
using UnityEngine;
using UnityVoltBuilder.Actions;
using UnityVoltBuilder.Build;

namespace UnityVoltBuilder.GUI
{
    public sealed class BuildToolWindow : EditorWindow
    {
        public void OnGUI()
        {
            //Main title
            EditorGUILayout.Space(2f);
            EditorGUILayout.LabelField("Unity Volt Builder", GUIStyles.TitleStyle);
            EditorGUILayout.Space(15f);

            BuildSettings.DrawOptions();
            BuildActions.DrawOptions();
            GameBuilder.DrawOptions();
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