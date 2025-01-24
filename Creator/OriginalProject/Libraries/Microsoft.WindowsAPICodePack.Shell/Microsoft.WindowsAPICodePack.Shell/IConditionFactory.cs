using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[ComImport]
	[Guid("A5EFE073-B16F-474f-9F3E-9F8B497A3E08")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IConditionFactory
	{
		[PreserveSig]
		HResult MakeNot([In] ICondition pcSub, [In] bool fSimplify, out ICondition ppcResult);

		[PreserveSig]
		HResult MakeAndOr([In] SearchConditionType ct, [In] IEnumUnknown peuSubs, [In] bool fSimplify, out ICondition ppcResult);

		[PreserveSig]
		HResult MakeLeaf([In][MarshalAs(UnmanagedType.LPWStr)] string pszPropertyName, [In] SearchConditionOperation cop, [In][MarshalAs(UnmanagedType.LPWStr)] string pszValueType, [In] PropVariant ppropvar, IRichChunk richChunk1, IRichChunk richChunk2, IRichChunk richChunk3, [In] bool fExpand, out ICondition ppcResult);

		[PreserveSig]
		HResult Resolve();
	}
}
