using System.Collections.Generic;
using UnityEditor;

namespace Voltstro.UnityBuilder.Settings
{
	internal static class SettingsManager
	{
		private const string PackageName = "dev.voltstro.voltunitybuilder";

		private static UnityEditor.SettingsManagement.Settings settings;

		/// <summary>
		/// The active <see cref="Settings"/> instance
		/// </summary>
		internal static UnityEditor.SettingsManagement.Settings Instance
		{
			get
			{
				if(settings == null)
					settings = new UnityEditor.SettingsManagement.Settings(PackageName);

				return settings;
			}
		}

		#region Properties

		#region Build Path Settings

		internal static string BuildLocation
		{
			get
			{
				if (settings.ContainsKey<string>("BuildLocation")) return settings.Get<string>("BuildLocation");

				settings.Set("BuildLocation", "Builds/");
				settings.Save();

				return settings.Get<string>("BuildLocation");
			}
			set
			{
				settings.Set("BuildLocation", value);
				settings.Save();
			}
		}

		internal static string BuildFolderNameStyle
		{
			get
			{
				if (settings.ContainsKey<string>("BuildFolderNameStyle")) return settings.Get<string>("BuildFolderNameStyle");

				settings.Set("BuildFolderNameStyle", "yy-MM-dd");
				settings.Save();

				return settings.Get<string>("BuildFolderNameStyle");
			}
			set
			{
				settings.Set("BuildFolderNameStyle", value);
				settings.Save();
			}
		}

		#endregion

		#region Build Player Settings

		internal static BuildTarget BuildTarget
		{
			get
			{
				if (settings.ContainsKey<BuildTarget>("BuildTarget")) return settings.Get<BuildTarget>("BuildTarget");

				settings.Set("BuildTarget", BuildTarget.StandaloneWindows64);
				settings.Save();

				return settings.Get<BuildTarget>("BuildTarget");
			}
			set
			{
				settings.Set("BuildTarget", value);
				settings.Save();
			}
		}

		internal static bool ServerBuild
		{
			get
			{
				if (settings.ContainsKey<bool>("ServerBuild")) return settings.Get<bool>("ServerBuild");

				settings.Set("ServerBuild", false);
				settings.Save();

				return settings.Get<bool>("ServerBuild");
			}
			set
			{
				settings.Set("ServerBuild", value);
				settings.Save();
			}
		}

		internal static bool CopyPdbFiles
		{
			get
			{
				if (settings.ContainsKey<bool>("CopyPDBFiles")) return settings.Get<bool>("CopyPDBFiles");

				settings.Set("CopyPDBFiles", false);
				settings.Save();

				return settings.Get<bool>("CopyPDBFiles");
			}
			set
			{
				settings.Set("CopyPDBFiles", value);
				settings.Save();
			}
		}

		internal static bool DevelopmentBuild
		{
			get
			{
				if (settings.ContainsKey<bool>("DevelopmentBuild")) return settings.Get<bool>("DevelopmentBuild");

				settings.Set("DevelopmentBuild", false);
				settings.Save();

				return settings.Get<bool>("DevelopmentBuild");
			}
			set
			{
				settings.Set("DevelopmentBuild", value);
				settings.Save();
			}
		}

		#endregion

		internal static List<string> BuildActions
		{
			get
			{
				if (settings.ContainsKey<List<string>>("BuildActions")) return settings.Get<List<string>>("BuildActions");

				settings.Set("BuildActions", new List<string>());
				settings.Save();

				return settings.Get<List<string>>("BuildActions");
			}
			set
			{
				settings.Set("BuildActions", value);
				settings.Save();
			}
		}

		internal static List<string> Scenes
		{
			get
			{
				if (settings.ContainsKey<List<string>>("Scenes")) return settings.Get<List<string>>("Scenes");

				settings.Set("Scenes", new List<string>());
				settings.Save();

				return settings.Get<List<string>>("Scenes");
			}
			set
			{
				settings.Set("Scenes", value);
				settings.Save();
			}
		}

		#endregion
	}
}