using System;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[ComImport]
	[Guid("2EBDEE67-3505-43f8-9946-EA44ABC8E5B0")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IQueryParser
	{
		[PreserveSig]
		HResult Parse([In][MarshalAs(UnmanagedType.LPWStr)] string pszInputString, [In] IEnumUnknown pCustomProperties, out IQuerySolution ppSolution);

		[PreserveSig]
		HResult SetOption([In] StructuredQuerySingleOption option, [In] PropVariant pOptionValue);

		[PreserveSig]
		HResult GetOption([In] StructuredQuerySingleOption option, [Out] PropVariant pOptionValue);

		[PreserveSig]
		HResult SetMultiOption([In] StructuredQueryMultipleOption option, [In][MarshalAs(UnmanagedType.LPWStr)] string pszOptionKey, [In] PropVariant pOptionValue);

		[PreserveSig]
		HResult GetSchemaProvider(out IntPtr ppSchemaProvider);

		[PreserveSig]
		HResult RestateToString([In] ICondition pCondition, [In] bool fUseEnglish, [MarshalAs(UnmanagedType.LPWStr)] out string ppszQueryString);

		[PreserveSig]
		HResult ParsePropertyValue([In][MarshalAs(UnmanagedType.LPWStr)] string pszPropertyName, [In][MarshalAs(UnmanagedType.LPWStr)] string pszInputString, out IQuerySolution ppSolution);

		[PreserveSig]
		HResult RestatePropertyValueToString([In] ICondition pCondition, [In] bool fUseEnglish, [MarshalAs(UnmanagedType.LPWStr)] out string ppszPropertyName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszQueryString);
	}
}
