using System;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	internal class ShellItemArray : IShellItemArray
	{
		private List<IShellItem> shellItemsList = new List<IShellItem>();

		internal ShellItemArray(IShellItem[] shellItems)
		{
			shellItemsList.AddRange(shellItems);
		}

		public HResult BindToHandler(IntPtr pbc, ref Guid rbhid, ref Guid riid, out IntPtr ppvOut)
		{
			throw new NotSupportedException();
		}

		public HResult GetPropertyStore(int Flags, ref Guid riid, out IntPtr ppv)
		{
			throw new NotSupportedException();
		}

		public HResult GetPropertyDescriptionList(ref PropertyKey keyType, ref Guid riid, out IntPtr ppv)
		{
			throw new NotSupportedException();
		}

		public HResult GetAttributes(ShellNativeMethods.ShellItemAttributeOptions dwAttribFlags, ShellNativeMethods.ShellFileGetAttributesOptions sfgaoMask, out ShellNativeMethods.ShellFileGetAttributesOptions psfgaoAttribs)
		{
			throw new NotSupportedException();
		}

		public HResult GetCount(out uint pdwNumItems)
		{
			pdwNumItems = (uint)shellItemsList.Count;
			return HResult.Ok;
		}

		public HResult GetItemAt(uint dwIndex, out IShellItem ppsi)
		{
			if ((int)dwIndex < shellItemsList.Count)
			{
				ppsi = shellItemsList[(int)dwIndex];
				return HResult.Ok;
			}
			ppsi = null;
			return HResult.Fail;
		}

		public HResult EnumItems(out IntPtr ppenumShellItems)
		{
			throw new NotSupportedException();
		}
	}
}
