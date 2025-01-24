using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	[ComImport]
	[Guid("6F79D558-3E96-4549-A1D1-7D75D2288814")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IPropertyDescription
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetPropertyKey(out PropertyKey pkey);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetCanonicalName([MarshalAs(UnmanagedType.LPWStr)] out string ppszName);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetPropertyType(out VarEnum pvartype);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetDisplayName(out IntPtr ppszName);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetEditInvitation(out IntPtr ppszInvite);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetTypeFlags([In] PropertyTypeOptions mask, out PropertyTypeOptions ppdtFlags);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetViewFlags(out PropertyViewOptions ppdvFlags);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetDefaultColumnWidth(out uint pcxChars);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetDisplayType(out PropertyDisplayType pdisplaytype);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetColumnState(out PropertyColumnStateOptions pcsFlags);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetGroupingRange(out PropertyGroupingRange pgr);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetRelativeDescriptionType(out PropertySystemNativeMethods.RelativeDescriptionType prdt);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetRelativeDescription([In] PropVariant propvar1, [In] PropVariant propvar2, [MarshalAs(UnmanagedType.LPWStr)] out string ppszDesc1, [MarshalAs(UnmanagedType.LPWStr)] out string ppszDesc2);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetSortDescription(out PropertySortDescription psd);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetSortDescriptionLabel([In] bool fDescending, out IntPtr ppszDescription);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetAggregationType(out PropertyAggregationType paggtype);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetConditionType(out PropertyConditionType pcontype, out PropertyConditionOperation popDefault);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetEnumTypeList([In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IPropertyEnumTypeList ppv);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void CoerceToCanonicalValue([In][Out] PropVariant propvar);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult FormatForDisplay([In] PropVariant propvar, [In] ref PropertyDescriptionFormatOptions pdfFlags, [MarshalAs(UnmanagedType.LPWStr)] out string ppszDisplay);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult IsValueCanonical([In] PropVariant propvar);
	}
}
