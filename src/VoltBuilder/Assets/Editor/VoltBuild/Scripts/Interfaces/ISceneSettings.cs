using UnityEditorInternal;

namespace VoltBuilder
{
	public interface ISceneSettings
	{
		/// <summary>
		/// Gets all active scenes
		/// </summary>
		/// <returns></returns>
		Scene[] GetAllScenes();

		/// <summary>
		/// Creates a nice reorder-able list of the scenes
		/// </summary>
		/// <returns></returns>
		ReorderableList CreateScenesList();

		/// <summary>
		/// Draws other scene settings that you might want
		/// </summary>
		void DrawSceneSettings(BuildTool buildTool);
	}
}