using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[ComImport]
	[Guid("0FC988D4-C935-4b97-A973-46282EA175C8")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface ICondition : IPersistStream
	{
		[PreserveSig]
		new void GetClassID(out Guid pClassID);

		[PreserveSig]
		new HResult IsDirty();

		[PreserveSig]
		new HResult Load([In][MarshalAs(UnmanagedType.Interface)] IStream stm);

		[PreserveSig]
		new HResult Save([In][MarshalAs(UnmanagedType.Interface)] IStream stm, bool fRemember);

		[PreserveSig]
		new HResult GetSizeMax(out ulong cbSize);

		[PreserveSig]
		HResult GetConditionType(out SearchConditionType pNodeType);

		[PreserveSig]
		HResult GetSubConditions([In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);

		[PreserveSig]
		HResult GetComparisonInfo([MarshalAs(UnmanagedType.LPWStr)] out string ppszPropertyName, out SearchConditionOperation pcop, [Out] PropVariant ppropvar);

		[PreserveSig]
		HResult GetValueType([MarshalAs(UnmanagedType.LPWStr)] out string ppszValueTypeName);

		[PreserveSig]
		HResult GetValueNormalization([MarshalAs(UnmanagedType.LPWStr)] out string ppszNormalization);

		[PreserveSig]
		HResult GetInputTerms(out IRichChunk ppPropertyTerm, out IRichChunk ppOperationTerm, out IRichChunk ppValueTerm);

		[PreserveSig]
		HResult Clone(out ICondition ppc);
	}
}
