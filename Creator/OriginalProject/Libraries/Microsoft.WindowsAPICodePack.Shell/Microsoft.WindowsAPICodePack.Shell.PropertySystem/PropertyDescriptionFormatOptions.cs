using System;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	[Flags]
	public enum PropertyDescriptionFormatOptions
	{
		None = 0,
		PrefixName = 1,
		FileName = 2,
		AlwaysKB = 4,
		RightToLeft = 8,
		ShortTime = 0x10,
		LongTime = 0x20,
		HideTime = 0x40,
		ShortDate = 0x80,
		LongDate = 0x100,
		HideDate = 0x200,
		RelativeDate = 0x400,
		UseEditInvitation = 0x800,
		ReadOnly = 0x1000,
		NoAutoReadingOrder = 0x2000,
		SmartDateTime = 0x4000
	}
}
