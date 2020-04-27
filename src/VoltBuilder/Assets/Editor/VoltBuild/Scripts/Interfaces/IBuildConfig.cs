using UnityEditor;

public interface IBuildConfig
{
	BuildTarget BuildTarget { get; set; }
}