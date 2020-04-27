using UnityEditor;

namespace VoltBuilder
{
	public class DefaultBuildConfig : IBuildConfig
	{
		public BuildTarget BuildTarget { get; set; }

		public bool DevBuild { get; set; }
		public bool CopyPDBFiles { get; set; }
		public bool ServerBuild { get; set; }
		public bool ZipFiles { get; set; }
	}
}