using System.IO.Compression;

namespace VoltBuilder
{
	public class DefaultZip : IZip
	{
		public void CompressDir(string directoryToCompress, string outPath)
		{
			ZipFile.CreateFromDirectory(directoryToCompress, outPath);
		}
	}
}