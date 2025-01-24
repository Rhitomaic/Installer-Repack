using System;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("D6EBC66B-8921-4193-AFDD-A1789FB7FF57")]
	internal interface IQuerySolution : IConditionFactory
	{
		[PreserveSig]
		new HResult MakeNot([In] ICondition pcSub, [In] bool fSimplify, out ICondition ppcResult);

		[PreserveSig]
		new HResult MakeAndOr([In] SearchConditionType ct, [In] IEnumUnknown peuSubs, [In] bool fSimplify, out ICondition ppcResult);

		[PreserveSig]
		new HResult MakeLeaf([In][MarshalAs(UnmanagedType.LPWStr)] string pszPropertyName, [In] SearchConditionOperation cop, [In][MarshalAs(UnmanagedType.LPWStr)] string pszValueType, [In] PropVariant ppropvar, IRichChunk richChunk1, IRichChunk richChunk2, IRichChunk richChunk3, [In] bool fExpand, out ICondition ppcResult);

		[PreserveSig]
		new HResult Resolve();

		[PreserveSig]
		HResult GetQuery([MarshalAs(UnmanagedType.Interface)] out ICondition ppQueryNode, [MarshalAs(UnmanagedType.Interface)] out IEntity ppMainType);

		[PreserveSig]
		HResult GetErrors([In] ref Guid riid, out IntPtr ppParseErrors);

		[PreserveSig]
		HResult GetLexicalData([MarshalAs(UnmanagedType.LPWStr)] out string ppszInputString, out IntPtr ppTokens, out uint plcid, out IntPtr ppWordBreaker);
	}
}
