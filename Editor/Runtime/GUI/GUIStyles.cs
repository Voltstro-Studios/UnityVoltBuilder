using UnityEditor;
using UnityEngine;

namespace UnityVoltBuilder.GUI
{
    /// <summary>
    ///     Has GUI styles that is used by UnityVoltBuilder
    /// </summary>
    public class GUIStyles
    {
        private static GUIStyles instance;
        
        private readonly GUIStyle dropdownButtonStyle;
        private readonly GUIStyle dropdownContentStyle;
        private readonly GUIStyle dropdownHeaderStyle;

        private readonly GUIStyle titleStyle;

        private GUIStyles()
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

        /// <summary>
        ///     Draw a dropdown button
        /// </summary>
        /// <param name="text"></param>
        /// <param name="dropdown"></param>
        public static void DrawDropdownButton(string text, ref bool dropdown)
        {
            if (GUILayout.Button(text, instance.dropdownButtonStyle)) dropdown = !dropdown;
        }

        #endregion

        #region Internal Getters

        /// <summary>
        ///     Active <see cref="GUIStyles" /> instance
        /// </summary>
        private static GUIStyles Instance
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
        public static GUIStyle TitleStyle => Instance.titleStyle;

        /// <summary>
        ///     Style for dropdown content
        /// </summary>
        public static GUIStyle DropdownContentStyle => Instance.dropdownContentStyle;

        /// <summary>
        ///     Style for dropdown header
        /// </summary>
        public static GUIStyle DropdownHeaderStyle => Instance.dropdownHeaderStyle;

        #endregion
    }
}