using System;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("A879E3C4-AF77-44fb-8F37-EBD1487CF920")]
	internal interface IQueryParserManager
	{
		[PreserveSig]
		HResult CreateLoadedParser([In][MarshalAs(UnmanagedType.LPWStr)] string pszCatalog, [In] ushort langidForKeywords, [In] ref Guid riid, out IQueryParser ppQueryParser);

		[PreserveSig]
		HResult InitializeOptions([In] bool fUnderstandNQS, [In] bool fAutoWildCard, [In] IQueryParser pQueryParser);

		[PreserveSig]
		HResult SetOption([In] QueryParserManagerOption option, [In] PropVariant pOptionValue);
	}
}
