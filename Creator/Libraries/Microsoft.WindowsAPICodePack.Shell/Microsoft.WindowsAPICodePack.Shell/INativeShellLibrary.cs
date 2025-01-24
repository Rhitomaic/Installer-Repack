using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[ComImport]
	[CoClass(typeof(ShellLibraryCoClass))]
	[Guid("11A66EFA-382E-451A-9234-1E0E12EF3085")]
	internal interface INativeShellLibrary : IShellLibrary
	{
	}
}
