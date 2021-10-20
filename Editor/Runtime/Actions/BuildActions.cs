using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityVoltBuilder.GUI;
using UnityVoltBuilder.Settings;

namespace UnityVoltBuilder.Actions
{
    public class BuildActions
    {
        private static BuildActions instance;

        private static bool buildActions;
        private static int selectedIndex;
        private readonly Dictionary<string, IBuildAction> activeBuildActions;

        private readonly List<Type> availableActions;
        private readonly string[] dropdownOptions;

        private BuildActions()
        {
            activeBuildActions = new Dictionary<string, IBuildAction>();
            availableActions = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IBuildAction).IsAssignableFrom(p) && !p.IsInterface)
                .ToList();
            dropdownOptions = availableActions.Select(availableAction => availableAction.Name).ToArray();

            foreach (string buildAction in SettingsManager.BuildActions) AddBuildAction(buildAction);
        }

        /// <summary>
        ///     Active <see cref="BuildActions" /> instance
        /// </summary>
        private static BuildActions Instance => instance ??= new BuildActions();

        /// <summary>
        ///     Gets all active <see cref="IBuildAction"/>s
        /// </summary>
        /// <returns></returns>
        [PublicAPI]
        public static List<IBuildAction> GetBuildActions()
        {
            return Instance.activeBuildActions.Values.ToList();
        }

        internal static void DrawOptions()
        {
            GUIStyles.DrawDropdownButton("Build Actions", ref buildActions);
            if (buildActions)
            {
                EditorGUILayout.BeginVertical(GUIStyles.DropdownContentStyle);

                if (Instance.availableActions.Count == 0)
                {
                    EditorGUILayout.HelpBox("There are no build actions to add!", MessageType.Error, true);
                    return;
                }

                //Drop down of available build actions
                EditorGUILayout.BeginHorizontal();
                selectedIndex = EditorGUILayout.Popup(selectedIndex, Instance.dropdownOptions);

                //Add button
                if (GUILayout.Button("+")) Instance.AddBuildAction(Instance.dropdownOptions[selectedIndex]);

                EditorGUILayout.EndHorizontal();

                try
                {
                    //Do OnGUI for each option
                    foreach (KeyValuePair<string, IBuildAction> activeBuildAction in Instance.activeBuildActions)
                    {
                        EditorGUILayout.BeginVertical(GUIStyles.DropdownContentStyle);

                        if (activeBuildAction.Value == null)
                        {
                            EditorGUILayout.HelpBox($"The action {activeBuildAction.Key} no longer exists!",
                                MessageType.Error);
                            if (GUILayout.Button("Delete"))
                                Instance.DeleteBuildAction(activeBuildAction.Key);
                            continue;
                        }

                        //Draw the build action's OnGUI
                        EditorGUILayout.LabelField(activeBuildAction.Key, GUIStyles.DropdownHeaderStyle);
                        activeBuildAction.Value.OnGUI();

                        //Delete button
                        if (GUILayout.Button("Delete"))
                            Instance.DeleteBuildAction(activeBuildAction.Key);

                        EditorGUILayout.EndVertical();

                        if (!activeBuildAction.Equals(Instance.activeBuildActions.Last()))
                            EditorGUILayout.Space();
                    }
                }
                catch (InvalidOperationException)
                {
                }

                EditorGUILayout.EndVertical();
            }
        }

        private void AddBuildAction(string action)
        {
            if (activeBuildActions.ContainsKey(action))
                return;

            //First, get if that action still exists
            Type actionType = availableActions.FirstOrDefault(x => x.Name == action);
            if (actionType == null)
            {
                activeBuildActions.Add(action, null);
                return;
            }

            //Now to create it
            IBuildAction buildAction = (IBuildAction)Activator.CreateInstance(actionType);
            activeBuildActions.Add(action, buildAction);

            List<string> currentBuildActions = SettingsManager.BuildActions;
            if (currentBuildActions.Contains(action))
                return;

            currentBuildActions.Add(action);
            SettingsManager.BuildActions = currentBuildActions;
        }

        private void DeleteBuildAction(string action)
        {
            if (!activeBuildActions.ContainsKey(action)) return;

            activeBuildActions.Remove(action);

            List<string> currentBuildActions = SettingsManager.BuildActions;
            currentBuildActions.Remove(action);
            SettingsManager.BuildActions = currentBuildActions;
        }
    }
}