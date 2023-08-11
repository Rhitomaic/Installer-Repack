#define TRACE
using System;
using System.Diagnostics;
using Microsoft.WindowsAPICodePack.Shell.Interop;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	internal class ChangeNotifyLock
	{
		private uint _event = 0u;

		public bool FromSystemInterrupt => (_event & 0x80000000u) != 0;

		public int ImageIndex { get; private set; }

		public string ItemName { get; private set; }

		public string ItemName2 { get; private set; }

		public ShellObjectChangeTypes ChangeType => (ShellObjectChangeTypes)_event;

		internal ChangeNotifyLock(Message message)
		{
			IntPtr pidl;
			IntPtr intPtr = ShellNativeMethods.SHChangeNotification_Lock(message.WParam, (int)message.LParam, out pidl, out _event);
			try
			{
				Trace.TraceInformation("Message: {0}", (ShellObjectChangeTypes)_event);
				ShellNativeMethods.ShellNotifyStruct shellNotifyStruct = pidl.MarshalAs<ShellNativeMethods.ShellNotifyStruct>();
				Guid riid = new Guid("7E9FB0D3-919F-4307-AB2E-9B1860310C93");
				IShellItem2 ppv;
				string ppszName;
				if (shellNotifyStruct.item1 != IntPtr.Zero && (_event & 0x8000) == 0)
				{
					if (CoreErrorHelper.Succeeded(ShellNativeMethods.SHCreateItemFromIDList(shellNotifyStruct.item1, ref riid, out ppv)))
					{
						ppv.GetDisplayName(ShellNativeMethods.ShellItemDesignNameOptions.FileSystemPath, out ppszName);
						ItemName = ppszName;
						Trace.TraceInformation("Item1: {0}", ItemName);
					}
				}
				else
				{
					ImageIndex = shellNotifyStruct.item1.ToInt32();
				}
				if (shellNotifyStruct.item2 != IntPtr.Zero && CoreErrorHelper.Succeeded(ShellNativeMethods.SHCreateItemFromIDList(shellNotifyStruct.item2, ref riid, out ppv)))
				{
					ppv.GetDisplayName(ShellNativeMethods.ShellItemDesignNameOptions.FileSystemPath, out ppszName);
					ItemName2 = ppszName;
					Trace.TraceInformation("Item2: {0}", ItemName2);
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					ShellNativeMethods.SHChangeNotification_Unlock(intPtr);
				}
			}
		}
	}
}
