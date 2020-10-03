public interface IBuildAction
{
	void OnGUI();

	void OnBeforeBuild(string buildLocation);

	void OnAfterBuild(string buildLocation);
}