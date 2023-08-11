using System;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[Flags]
	internal enum WindowStyles
	{
		Border = 0x800000,
		Caption = 0xC00000,
		Child = 0x40000000,
		ChildWindow = 0x40000000,
		ClipChildren = 0x2000000,
		ClipSiblings = 0x4000000,
		Disabled = 0x8000000,
		DialogFrame = 0x40000,
		Group = 0x20000,
		HorizontalScroll = 0x100000,
		Iconic = 0x20000000,
		Maximize = 0x1000000,
		MaximizeBox = 0x10000,
		Minimize = 0x20000000,
		MinimizeBox = 0x20000,
		Overlapped = 0,
		Popup = int.MinValue,
		SizeBox = 0x40000,
		SystemMenu = 0x80000,
		Tabstop = 0x10000,
		ThickFrame = 0x40000,
		Tiled = 0,
		Visible = 0x10000000,
		VerticalScroll = 0x200000,
		TiledWindowMask = 0xCF0000,
		PopupWindowMask = -2138570752,
		OverlappedWindowMask = 0xCF0000
	}
}
