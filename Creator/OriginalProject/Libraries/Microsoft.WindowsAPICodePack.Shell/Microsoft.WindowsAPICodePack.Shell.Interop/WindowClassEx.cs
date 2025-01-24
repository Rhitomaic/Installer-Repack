using System;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Shell.Interop
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct WindowClassEx
	{
		internal uint Size;

		internal uint Style;

		internal ShellObjectWatcherNativeMethods.WndProcDelegate WndProc;

		internal int ExtraClassBytes;

		internal int ExtraWindowBytes;

		internal IntPtr InstanceHandle;

		internal IntPtr IconHandle;

		internal IntPtr CursorHandle;

		internal IntPtr BackgroundBrushHandle;

		internal string MenuName;

		internal string ClassName;

		internal IntPtr SmallIconHandle;
	}
}
