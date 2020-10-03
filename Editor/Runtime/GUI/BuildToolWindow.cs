using UnityEditor;
using Voltstro.UnityBuilder.Actions;
using Voltstro.UnityBuilder.Build;

namespace Voltstro.UnityBuilder.GUI
{
	public class BuildToolWindow : EditorWindow
	{
		[MenuItem("Tools/Volt Build/Build Tool")]
		public static void ShowWindow()
		{
			GetWindow(typeof(BuildToolWindow), false, "Volt Build Tool");
		}

		public void OnGUI()
		{
			//Main title
			EditorGUILayout.LabelField("Volt Unity Build", GUIStyles.TitleStyle);
			EditorGUILayout.Space(15f);

			BuildSettings.DrawOptions();
			BuildActions.DrawOptions();
			GameBuilder.DrawOptions();
		}
	}
}