using UnityEditor;
using UnityEngine;

namespace UnityVoltBuilder.GUI
{
    internal class GUIStyles
    {
        private static GUIStyles instance;
        private readonly GUIStyle dropdownButtonStyle;
        private readonly GUIStyle dropdownContentStyle;
        private readonly GUIStyle dropdownHeaderStyle;

        private readonly GUIStyle titleStyle;

        internal GUIStyles()
        {
            //Title Style
            titleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 18
            };

            dropdownButtonStyle = new GUIStyle(UnityEngine.GUI.skin.button)
            {
                alignment = TextAnchor.MiddleLeft,
                fontStyle = FontStyle.Bold,
                margin = new RectOffset(5, 5, 0, 0)
            };

            dropdownContentStyle = new GUIStyle(UnityEngine.GUI.skin.textField)
            {
                padding = new RectOffset(5, 5, 5, 5),
                margin = new RectOffset(5, 5, 0, 0)
            };

            dropdownHeaderStyle = new GUIStyle(EditorStyles.helpBox)
            {
                fontStyle = FontStyle.Bold
            };
        }

        #region Internal Functions

        internal static void DrawDropdownButton(string text, ref bool dropdown)
        {
            if (GUILayout.Button(text, instance.dropdownButtonStyle)) dropdown = !dropdown;
        }

        #endregion

        #region Internal Getters

        /// <summary>
        ///     Active <see cref="GUIStyles" /> instance
        /// </summary>
        internal static GUIStyles Instance
        {
            get
            {
                if (instance == null)
                    instance = new GUIStyles();

                return instance;
            }
        }

        /// <summary>
        ///     Main title style
        /// </summary>
        internal static GUIStyle TitleStyle => Instance.titleStyle;

        internal static GUIStyle DropdownContentStyle => Instance.dropdownContentStyle;

        internal static GUIStyle DropdownHeaderStyle => Instance.dropdownHeaderStyle;

        #endregion
    }
}