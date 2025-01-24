using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	public class ShellPropertyEnumType
	{
		private string displayText;

		private PropEnumType? enumType;

		private object minValue;

		private object setValue;

		private object enumerationValue;

		private IPropertyEnumType NativePropertyEnumType { get; set; }

		public string DisplayText
		{
			get
			{
				if (displayText == null)
				{
					NativePropertyEnumType.GetDisplayText(out displayText);
				}
				return displayText;
			}
		}

		public PropEnumType EnumType
		{
			get
			{
				if (!enumType.HasValue)
				{
					NativePropertyEnumType.GetEnumType(out var penumtype);
					enumType = penumtype;
				}
				return enumType.Value;
			}
		}

		public object RangeMinValue
		{
			get
			{
				if (minValue == null)
				{
					using (PropVariant propVariant = new PropVariant())
					{
						NativePropertyEnumType.GetRangeMinValue(propVariant);
						minValue = propVariant.Value;
					}
				}
				return minValue;
			}
		}

		public object RangeSetValue
		{
			get
			{
				if (setValue == null)
				{
					using (PropVariant propVariant = new PropVariant())
					{
						NativePropertyEnumType.GetRangeSetValue(propVariant);
						setValue = propVariant.Value;
					}
				}
				return setValue;
			}
		}

		public object RangeValue
		{
			get
			{
				if (enumerationValue == null)
				{
					using (PropVariant propVariant = new PropVariant())
					{
						NativePropertyEnumType.GetValue(propVariant);
						enumerationValue = propVariant.Value;
					}
				}
				return enumerationValue;
			}
		}

		internal ShellPropertyEnumType(IPropertyEnumType nativePropertyEnumType)
		{
			NativePropertyEnumType = nativePropertyEnumType;
		}
	}
}
