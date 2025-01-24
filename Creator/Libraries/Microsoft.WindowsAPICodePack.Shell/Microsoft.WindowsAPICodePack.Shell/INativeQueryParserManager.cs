using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[ComImport]
	[CoClass(typeof(QueryParserManagerCoClass))]
	[Guid("A879E3C4-AF77-44fb-8F37-EBD1487CF920")]
	internal interface INativeQueryParserManager : IQueryParserManager
	{
	}
}
