using System;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	internal static class PropertySystemNativeMethods
	{
		internal enum RelativeDescriptionType
		{
			General,
			Date,
			Size,
			Count,
			Revision,
			Length,
			Duration,
			Speed,
			Rate,
			Rating,
			Priority
		}

		[DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int PSGetNameFromPropertyKey(ref PropertyKey propkey, [MarshalAs(UnmanagedType.LPWStr)] out string ppszCanonicalName);

		[DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern HResult PSGetPropertyDescription(ref PropertyKey propkey, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IPropertyDescription ppv);

		[DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int PSGetPropertyKeyFromName([In][MarshalAs(UnmanagedType.LPWStr)] string pszCanonicalName, out PropertyKey propkey);

		[DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int PSGetPropertyDescriptionListFromString([In][MarshalAs(UnmanagedType.LPWStr)] string pszPropList, [In] ref Guid riid, out IPropertyDescriptionList ppv);
	}
}
