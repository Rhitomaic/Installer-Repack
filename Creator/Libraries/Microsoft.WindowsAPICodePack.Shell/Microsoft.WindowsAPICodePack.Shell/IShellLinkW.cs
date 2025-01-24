using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[ComImport]
	[Guid("000214F9-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IShellLinkW
	{
		void GetPath([Out][MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, IntPtr pfd, uint fFlags);

		void GetIDList(out IntPtr ppidl);

		void SetIDList(IntPtr pidl);

		void GetDescription([Out][MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxName);

		void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);

		void GetWorkingDirectory([Out][MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);

		void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);

		void GetArguments([Out][MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);

		void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);

		void GetHotKey(out short wHotKey);

		void SetHotKey(short wHotKey);

		void GetShowCmd(out uint iShowCmd);

		void SetShowCmd(uint iShowCmd);

		void GetIconLocation([MarshalAs(UnmanagedType.LPWStr)] out StringBuilder pszIconPath, int cchIconPath, out int iIcon);

		void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);

		void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, uint dwReserved);

		void Resolve(IntPtr hwnd, uint fFlags);

		void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
	}
}
