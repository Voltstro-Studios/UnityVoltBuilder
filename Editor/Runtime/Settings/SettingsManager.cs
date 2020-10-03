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
				if (settings == null)
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
				if (Instance.ContainsKey<string>("BuildLocation")) return Instance.Get<string>("BuildLocation");

				Instance.Set("BuildLocation", "Builds/");
				Instance.Save();

				return Instance.Get<string>("BuildLocation");
			}
			set
			{
				Instance.Set("BuildLocation", value);
				Instance.Save();
			}
		}

		internal static string BuildFolderNameStyle
		{
			get
			{
				if (Instance.ContainsKey<string>("BuildFolderNameStyle")) return Instance.Get<string>("BuildFolderNameStyle");

				Instance.Set("BuildFolderNameStyle", "yy-MM-dd");
				Instance.Save();

				return Instance.Get<string>("BuildFolderNameStyle");
			}
			set
			{
				Instance.Set("BuildFolderNameStyle", value);
				Instance.Save();
			}
		}

		#endregion

		#region Build Player Settings

		internal static BuildTarget BuildTarget
		{
			get
			{
				if (Instance.ContainsKey<BuildTarget>("BuildTarget")) return Instance.Get<BuildTarget>("BuildTarget");

				Instance.Set("BuildTarget", BuildTarget.StandaloneWindows64);
				Instance.Save();

				return Instance.Get<BuildTarget>("BuildTarget");
			}
			set
			{
				Instance.Set("BuildTarget", value);
				Instance.Save();
			}
		}

		internal static bool ServerBuild
		{
			get
			{
				if (Instance.ContainsKey<bool>("ServerBuild")) return Instance.Get<bool>("ServerBuild");

				Instance.Set("ServerBuild", false);
				Instance.Save();

				return Instance.Get<bool>("ServerBuild");
			}
			set
			{
				Instance.Set("ServerBuild", value);
				Instance.Save();
			}
		}

		internal static bool CopyPdbFiles
		{
			get
			{
				if (Instance.ContainsKey<bool>("CopyPDBFiles")) return Instance.Get<bool>("CopyPDBFiles");

				Instance.Set("CopyPDBFiles", false);
				Instance.Save();

				return Instance.Get<bool>("CopyPDBFiles");
			}
			set
			{
				Instance.Set("CopyPDBFiles", value);
				Instance.Save();
			}
		}

		internal static bool DevelopmentBuild
		{
			get
			{
				if (Instance.ContainsKey<bool>("DevelopmentBuild")) return Instance.Get<bool>("DevelopmentBuild");

				Instance.Set("DevelopmentBuild", false);
				Instance.Save();

				return Instance.Get<bool>("DevelopmentBuild");
			}
			set
			{
				Instance.Set("DevelopmentBuild", value);
				Instance.Save();
			}
		}

		internal static bool AutoconnectProfiler
		{
			get
			{
				if (Instance.ContainsKey<bool>("AutoconnectProfiler")) return Instance.Get<bool>("AutoconnectProfiler");

				Instance.Set("AutoconnectProfiler", false);
				Instance.Save();

				return Instance.Get<bool>("AutoconnectProfiler");
			}
			set
			{
				Instance.Set("AutoconnectProfiler", value);
				Instance.Save();
			}
		}

		internal static bool DeepProfiling
		{
			get
			{
				if (Instance.ContainsKey<bool>("DeepProfiling")) return Instance.Get<bool>("DeepProfiling");

				Instance.Set("DeepProfiling", false);
				Instance.Save();

				return Instance.Get<bool>("DeepProfiling");
			}
			set
			{
				Instance.Set("DeepProfiling", value);
				Instance.Save();
			}
		}

		internal static bool ScriptDebugging
		{
			get
			{
				if (Instance.ContainsKey<bool>("ScriptDebugging")) return Instance.Get<bool>("ScriptDebugging");

				Instance.Set("ScriptDebugging", false);
				Instance.Save();

				return Instance.Get<bool>("ScriptDebugging");
			}
			set
			{
				Instance.Set("ScriptDebugging", value);
				Instance.Save();
			}
		}

		#endregion

		internal static List<string> BuildActions
		{
			get
			{
				if (!Instance.ContainsKey<List<string>>("BuildActions"))
				{
					Instance.Set("BuildActions", new List<string>());
					Instance.Save();
				}

				return Instance.Get<List<string>>("BuildActions");
			}
			set
			{
				Instance.Set("BuildActions", value);
				Instance.Save();
			}
		}

		#endregion
	}
}