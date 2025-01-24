using System;

namespace Microsoft.WindowsAPICodePack.Controls
{
	[Flags]
	internal enum FolderOptions
	{
		AutoArrange = 1,
		AbbreviatedNames = 2,
		SnapToGrid = 4,
		OwnerData = 8,
		BestFitWindow = 0x10,
		Desktop = 0x20,
		SingleSelection = 0x40,
		NoSubfolders = 0x80,
		Transparent = 0x100,
		NoClientEdge = 0x200,
		NoScroll = 0x400,
		AlignLeft = 0x800,
		NoIcons = 0x1000,
		ShowSelectionAlways = 0x2000,
		NoVisible = 0x4000,
		SingleClickActivate = 0x8000,
		NoWebView = 0x10000,
		HideFilenames = 0x20000,
		CheckSelect = 0x40000,
		NoEnumRefresh = 0x80000,
		NoGrouping = 0x100000,
		FullRowSelect = 0x200000,
		NoFilters = 0x400000,
		NoColumnHeaders = 0x800000,
		NoHeaderInAllViews = 0x1000000,
		ExtendedTiles = 0x2000000,
		TriCheckSelect = 0x4000000,
		AutoCheckSelect = 0x8000000,
		NoBrowserViewState = 0x10000000,
		SubsetGroups = 0x20000000,
		UseSearchFolders = 0x40000000,
		AllowRightToLeftReading = int.MinValue
	}
}
