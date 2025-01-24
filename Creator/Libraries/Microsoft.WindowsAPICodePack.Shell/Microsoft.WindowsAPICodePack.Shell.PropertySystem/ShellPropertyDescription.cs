using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	public class ShellPropertyDescription : IDisposable
	{
		private IPropertyDescription nativePropertyDescription;

		private string canonicalName;

		private PropertyKey propertyKey;

		private string displayName;

		private string editInvitation;

		private VarEnum? varEnumType = null;

		private PropertyDisplayType? displayType;

		private PropertyAggregationType? aggregationTypes;

		private uint? defaultColumWidth;

		private PropertyTypeOptions? propertyTypeFlags;

		private PropertyViewOptions? propertyViewFlags;

		private Type valueType;

		private ReadOnlyCollection<ShellPropertyEnumType> propertyEnumTypes;

		private PropertyColumnStateOptions? columnState;

		private PropertyConditionType? conditionType;

		private PropertyConditionOperation? conditionOperation;

		private PropertyGroupingRange? groupingRange;

		private PropertySortDescription? sortDescription;

		public string CanonicalName
		{
			get
			{
				if (canonicalName == null)
				{
					PropertySystemNativeMethods.PSGetNameFromPropertyKey(ref propertyKey, out canonicalName);
				}
				return canonicalName;
			}
		}

		public PropertyKey PropertyKey => propertyKey;

		public string DisplayName
		{
			get
			{
				if (NativePropertyDescription != null && displayName == null)
				{
					IntPtr ppszName = IntPtr.Zero;
					HResult result = NativePropertyDescription.GetDisplayName(out ppszName);
					if (CoreErrorHelper.Succeeded(result) && ppszName != IntPtr.Zero)
					{
						displayName = Marshal.PtrToStringUni(ppszName);
						Marshal.FreeCoTaskMem(ppszName);
					}
				}
				return displayName;
			}
		}

		public string EditInvitation
		{
			get
			{
				if (NativePropertyDescription != null && editInvitation == null)
				{
					IntPtr ppszInvite = IntPtr.Zero;
					HResult result = NativePropertyDescription.GetEditInvitation(out ppszInvite);
					if (CoreErrorHelper.Succeeded(result) && ppszInvite != IntPtr.Zero)
					{
						editInvitation = Marshal.PtrToStringUni(ppszInvite);
						Marshal.FreeCoTaskMem(ppszInvite);
					}
				}
				return editInvitation;
			}
		}

		public VarEnum VarEnumType
		{
			get
			{
				if (NativePropertyDescription != null && !varEnumType.HasValue)
				{
					VarEnum pvartype;
					HResult propertyType = NativePropertyDescription.GetPropertyType(out pvartype);
					if (CoreErrorHelper.Succeeded(propertyType))
					{
						varEnumType = pvartype;
					}
				}
				return varEnumType.HasValue ? varEnumType.Value : VarEnum.VT_EMPTY;
			}
		}

		public Type ValueType
		{
			get
			{
				if (valueType == null)
				{
					valueType = ShellPropertyFactory.VarEnumToSystemType(VarEnumType);
				}
				return valueType;
			}
		}

		public PropertyDisplayType DisplayType
		{
			get
			{
				if (NativePropertyDescription != null && !displayType.HasValue)
				{
					PropertyDisplayType pdisplaytype;
					HResult result = NativePropertyDescription.GetDisplayType(out pdisplaytype);
					if (CoreErrorHelper.Succeeded(result))
					{
						displayType = pdisplaytype;
					}
				}
				return displayType.HasValue ? displayType.Value : PropertyDisplayType.String;
			}
		}

		public uint DefaultColumWidth
		{
			get
			{
				if (NativePropertyDescription != null && !defaultColumWidth.HasValue)
				{
					uint pcxChars;
					HResult defaultColumnWidth = NativePropertyDescription.GetDefaultColumnWidth(out pcxChars);
					if (CoreErrorHelper.Succeeded(defaultColumnWidth))
					{
						defaultColumWidth = pcxChars;
					}
				}
				return defaultColumWidth.HasValue ? defaultColumWidth.Value : 0u;
			}
		}

		public PropertyAggregationType AggregationTypes
		{
			get
			{
				if (NativePropertyDescription != null && !aggregationTypes.HasValue)
				{
					PropertyAggregationType paggtype;
					HResult aggregationType = NativePropertyDescription.GetAggregationType(out paggtype);
					if (CoreErrorHelper.Succeeded(aggregationType))
					{
						aggregationTypes = paggtype;
					}
				}
				return aggregationTypes.HasValue ? aggregationTypes.Value : PropertyAggregationType.Default;
			}
		}

		public ReadOnlyCollection<ShellPropertyEnumType> PropertyEnumTypes
		{
			get
			{
				if (NativePropertyDescription != null && propertyEnumTypes == null)
				{
					List<ShellPropertyEnumType> list = new List<ShellPropertyEnumType>();
					Guid riid = new Guid("A99400F4-3D84-4557-94BA-1242FB2CC9A6");
					IPropertyEnumTypeList ppv;
					HResult enumTypeList = NativePropertyDescription.GetEnumTypeList(ref riid, out ppv);
					if (ppv != null && CoreErrorHelper.Succeeded(enumTypeList))
					{
						ppv.GetCount(out var pctypes);
						riid = new Guid("11E1FBF9-2D56-4A6B-8DB3-7CD193A471F2");
						for (uint num = 0u; num < pctypes; num++)
						{
							ppv.GetAt(num, ref riid, out var ppv2);
							list.Add(new ShellPropertyEnumType(ppv2));
						}
					}
					propertyEnumTypes = new ReadOnlyCollection<ShellPropertyEnumType>(list);
				}
				return propertyEnumTypes;
			}
		}

		public PropertyColumnStateOptions ColumnState
		{
			get
			{
				if (NativePropertyDescription != null && !columnState.HasValue)
				{
					PropertyColumnStateOptions pcsFlags;
					HResult result = NativePropertyDescription.GetColumnState(out pcsFlags);
					if (CoreErrorHelper.Succeeded(result))
					{
						columnState = pcsFlags;
					}
				}
				return columnState.HasValue ? columnState.Value : PropertyColumnStateOptions.None;
			}
		}

		public PropertyConditionType ConditionType
		{
			get
			{
				if (NativePropertyDescription != null && !conditionType.HasValue)
				{
					PropertyConditionType pcontype;
					PropertyConditionOperation popDefault;
					HResult result = NativePropertyDescription.GetConditionType(out pcontype, out popDefault);
					if (CoreErrorHelper.Succeeded(result))
					{
						conditionOperation = popDefault;
						conditionType = pcontype;
					}
				}
				return conditionType.HasValue ? conditionType.Value : PropertyConditionType.None;
			}
		}

		public PropertyConditionOperation ConditionOperation
		{
			get
			{
				if (NativePropertyDescription != null && !conditionOperation.HasValue)
				{
					PropertyConditionType pcontype;
					PropertyConditionOperation popDefault;
					HResult result = NativePropertyDescription.GetConditionType(out pcontype, out popDefault);
					if (CoreErrorHelper.Succeeded(result))
					{
						conditionOperation = popDefault;
						conditionType = pcontype;
					}
				}
				return conditionOperation.HasValue ? conditionOperation.Value : PropertyConditionOperation.Implicit;
			}
		}

		public PropertyGroupingRange GroupingRange
		{
			get
			{
				if (NativePropertyDescription != null && !groupingRange.HasValue)
				{
					PropertyGroupingRange pgr;
					HResult result = NativePropertyDescription.GetGroupingRange(out pgr);
					if (CoreErrorHelper.Succeeded(result))
					{
						groupingRange = pgr;
					}
				}
				return groupingRange.HasValue ? groupingRange.Value : PropertyGroupingRange.Discrete;
			}
		}

		public PropertySortDescription SortDescription
		{
			get
			{
				if (NativePropertyDescription != null && !sortDescription.HasValue)
				{
					PropertySortDescription psd;
					HResult result = NativePropertyDescription.GetSortDescription(out psd);
					if (CoreErrorHelper.Succeeded(result))
					{
						sortDescription = psd;
					}
				}
				return sortDescription.HasValue ? sortDescription.Value : PropertySortDescription.General;
			}
		}

		public PropertyTypeOptions TypeFlags
		{
			get
			{
				if (NativePropertyDescription != null && !propertyTypeFlags.HasValue)
				{
					PropertyTypeOptions ppdtFlags;
					HResult typeFlags = NativePropertyDescription.GetTypeFlags(PropertyTypeOptions.MaskAll, out ppdtFlags);
					propertyTypeFlags = (CoreErrorHelper.Succeeded(typeFlags) ? ppdtFlags : PropertyTypeOptions.None);
				}
				return propertyTypeFlags.HasValue ? propertyTypeFlags.Value : PropertyTypeOptions.None;
			}
		}

		public PropertyViewOptions ViewFlags
		{
			get
			{
				if (NativePropertyDescription != null && !propertyViewFlags.HasValue)
				{
					PropertyViewOptions ppdvFlags;
					HResult viewFlags = NativePropertyDescription.GetViewFlags(out ppdvFlags);
					propertyViewFlags = (CoreErrorHelper.Succeeded(viewFlags) ? ppdvFlags : PropertyViewOptions.None);
				}
				return propertyViewFlags.HasValue ? propertyViewFlags.Value : PropertyViewOptions.None;
			}
		}

		public bool HasSystemDescription => NativePropertyDescription != null;

		internal IPropertyDescription NativePropertyDescription
		{
			get
			{
				if (nativePropertyDescription == null)
				{
					Guid riid = new Guid("6F79D558-3E96-4549-A1D1-7D75D2288814");
					PropertySystemNativeMethods.PSGetPropertyDescription(ref propertyKey, ref riid, out nativePropertyDescription);
				}
				return nativePropertyDescription;
			}
		}

		public string GetSortDescriptionLabel(bool descending)
		{
			IntPtr ppszDescription = IntPtr.Zero;
			string result = string.Empty;
			if (NativePropertyDescription != null)
			{
				HResult sortDescriptionLabel = NativePropertyDescription.GetSortDescriptionLabel(descending, out ppszDescription);
				if (CoreErrorHelper.Succeeded(sortDescriptionLabel) && ppszDescription != IntPtr.Zero)
				{
					result = Marshal.PtrToStringUni(ppszDescription);
					Marshal.FreeCoTaskMem(ppszDescription);
				}
			}
			return result;
		}

		internal ShellPropertyDescription(PropertyKey key)
		{
			propertyKey = key;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (nativePropertyDescription != null)
			{
				Marshal.ReleaseComObject(nativePropertyDescription);
				nativePropertyDescription = null;
			}
			if (disposing)
			{
				canonicalName = null;
				displayName = null;
				editInvitation = null;
				defaultColumWidth = null;
				valueType = null;
				propertyEnumTypes = null;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~ShellPropertyDescription()
		{
			Dispose(false);
		}
	}
}
