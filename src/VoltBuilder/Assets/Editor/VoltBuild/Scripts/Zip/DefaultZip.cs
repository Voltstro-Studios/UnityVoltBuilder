using System.IO.Compression;

public class DefaultZip : IZip
{
	public void CompressDir(string directoryToCompress, string outPath)
	{
		ZipFile.CreateFromDirectory(directoryToCompress, outPath);
	}
}