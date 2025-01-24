using System;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	[Flags]
	public enum PropertyColumnStateOptions
	{
		None = 0,
		StringType = 1,
		IntegerType = 2,
		DateType = 3,
		TypeMask = 0xF,
		OnByDefault = 0x10,
		Slow = 0x20,
		Extended = 0x40,
		SecondaryUI = 0x80,
		Hidden = 0x100,
		PreferVariantCompare = 0x200,
		PreferFormatForDisplay = 0x400,
		NoSortByFolders = 0x800,
		ViewOnly = 0x10000,
		BatchRead = 0x20000,
		NoGroupBy = 0x40000,
		FixedWidth = 0x1000,
		NoDpiScale = 0x2000,
		FixedRatio = 0x4000,
		DisplayMask = 0xF000
	}
}
