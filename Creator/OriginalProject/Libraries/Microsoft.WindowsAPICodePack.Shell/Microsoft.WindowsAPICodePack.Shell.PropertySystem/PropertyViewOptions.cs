using System;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	[Flags]
	public enum PropertyViewOptions
	{
		None = 0,
		CenterAlign = 1,
		RightAlign = 2,
		BeginNewGroup = 4,
		FillArea = 8,
		SortDescending = 0x10,
		ShowOnlyIfPresent = 0x20,
		ShowByDefault = 0x40,
		ShowInPrimaryList = 0x80,
		ShowInSecondaryList = 0x100,
		HideLabel = 0x200,
		Hidden = 0x800,
		CanWrap = 0x1000,
		MaskAll = 0x3FF
	}
}
