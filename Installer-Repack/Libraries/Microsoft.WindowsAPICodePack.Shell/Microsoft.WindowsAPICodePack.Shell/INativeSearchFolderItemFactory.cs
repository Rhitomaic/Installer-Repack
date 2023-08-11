using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[ComImport]
	[Guid("a0ffbc28-5482-4366-be27-3e81e78e06c2")]
	[CoClass(typeof(SearchFolderItemFactoryCoClass))]
	internal interface INativeSearchFolderItemFactory : ISearchFolderItemFactory
	{
	}
}
