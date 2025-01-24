using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Controls
{
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	internal class FolderSettings
	{
		public FolderViewMode ViewMode;

		public FolderOptions Options;
	}
}
