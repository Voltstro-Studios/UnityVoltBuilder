using UnityEditor;

namespace VoltBuilder
{
	public interface IBuildConfig
	{
		BuildTarget BuildTarget { get; set; }
	}
}