using System;

namespace Microsoft.WindowsAPICodePack.Controls
{
	[Flags]
	public enum ExplorerBrowserContentSectionOptions
	{
		None = 0,
		AlignLeft = 0x800,
		AutoArrange = 1,
		CheckSelect = 0x8040000,
		ExtendedTiles = 0x2000000,
		FullRowSelect = 0x200000,
		HideFileNames = 0x20000,
		NoBrowserViewState = 0x10000000,
		NoColumnHeader = 0x800000,
		NoHeaderInAllViews = 0x1000000,
		NoIcons = 0x1000,
		NoSubfolders = 0x80,
		SingleClickActivate = 0x8000,
		SingleSelection = 0x40
	}
}
