#define DEBUG
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Shell.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	public class ShellProperty<T> : IShellProperty
	{
		private PropertyKey propertyKey;

		private string imageReferencePath = null;

		private int? imageReferenceIconIndex;

		private ShellPropertyDescription description = null;

		private ShellObject ParentShellObject { get; set; }

		private IPropertyStore NativePropertyStore { get; set; }

		public T Value
		{
			get
			{
				Debug.Assert(ValueType == ShellPropertyFactory.VarEnumToSystemType(Description.VarEnumType));
				using (PropVariant propVariant = new PropVariant())
				{
					if (ParentShellObject.NativePropertyStore != null)
					{
						ParentShellObject.NativePropertyStore.GetValue(ref propertyKey, propVariant);
					}
					else if (ParentShellObject != null)
					{
						ParentShellObject.NativeShellItem2.GetProperty(ref propertyKey, propVariant);
					}
					else if (NativePropertyStore != null)
					{
						NativePropertyStore.GetValue(ref propertyKey, propVariant);
					}
					return (propVariant.Value != null) ? ((T)propVariant.Value) : default(T);
				}
			}
			set
			{
				Debug.Assert(ValueType == ShellPropertyFactory.VarEnumToSystemType(Description.VarEnumType));
				if (typeof(T) != ValueType)
				{
					throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, LocalizedMessages.ShellPropertyWrongType, ValueType.Name));
				}
				if (value is Nullable)
				{
					Type typeFromHandle = typeof(T);
					PropertyInfo property = typeFromHandle.GetProperty("HasValue");
					if (property != null && !(bool)property.GetValue(value, null))
					{
						ClearValue();
						return;
					}
				}
				else if (value == null)
				{
					ClearValue();
					return;
				}
				if (ParentShellObject != null)
				{
					using (ShellPropertyWriter shellPropertyWriter = ParentShellObject.Properties.GetPropertyWriter())
					{
						shellPropertyWriter.WriteProperty(this, value, AllowSetTruncatedValue);
						return;
					}
				}
				if (NativePropertyStore != null)
				{
					throw new InvalidOperationException(LocalizedMessages.ShellPropertyCannotSetProperty);
				}
			}
		}

		public PropertyKey PropertyKey => propertyKey;

		public ShellPropertyDescription Description => description;

		public string CanonicalName => Description.CanonicalName;

		public object ValueAsObject
		{
			get
			{
				using (PropVariant propVariant = new PropVariant())
				{
					if (ParentShellObject != null)
					{
						IPropertyStore propertyStore = ShellPropertyCollection.CreateDefaultPropertyStore(ParentShellObject);
						propertyStore.GetValue(ref propertyKey, propVariant);
						Marshal.ReleaseComObject(propertyStore);
						propertyStore = null;
					}
					else if (NativePropertyStore != null)
					{
						NativePropertyStore.GetValue(ref propertyKey, propVariant);
					}
					return propVariant?.Value;
				}
			}
		}

		public Type ValueType
		{
			get
			{
				Debug.Assert(Description.ValueType == typeof(T));
				return Description.ValueType;
			}
		}

		public IconReference IconReference
		{
			get
			{
				if (!CoreHelpers.RunningOnWin7)
				{
					throw new PlatformNotSupportedException(LocalizedMessages.ShellPropertyWindows7);
				}
				GetImageReference();
				int resourceId = (imageReferenceIconIndex.HasValue ? imageReferenceIconIndex.Value : (-1));
				return new IconReference(imageReferencePath, resourceId);
			}
		}

		public bool AllowSetTruncatedValue { get; set; }

		private void GetImageReference()
		{
			IPropertyStore propertyStore = ShellPropertyCollection.CreateDefaultPropertyStore(ParentShellObject);
			using (PropVariant propVariant = new PropVariant())
			{
				propertyStore.GetValue(ref propertyKey, propVariant);
				Marshal.ReleaseComObject(propertyStore);
				propertyStore = null;
				((IPropertyDescription2)Description.NativePropertyDescription).GetImageReferenceForValue(propVariant, out var ppszImageRes);
				if (ppszImageRes != null)
				{
					int value = ShellNativeMethods.PathParseIconLocation(ref ppszImageRes);
					if (ppszImageRes != null)
					{
						imageReferencePath = ppszImageRes;
						imageReferenceIconIndex = value;
					}
				}
			}
		}

		private void StorePropVariantValue(PropVariant propVar)
		{
			Guid riid = new Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99");
			IPropertyStore ppv = null;
			try
			{
				int propertyStore = ParentShellObject.NativeShellItem2.GetPropertyStore(ShellNativeMethods.GetPropertyStoreOptions.ReadWrite, ref riid, out ppv);
				if (!CoreErrorHelper.Succeeded(propertyStore))
				{
					throw new PropertySystemException(LocalizedMessages.ShellPropertyUnableToGetWritableProperty, Marshal.GetExceptionForHR(propertyStore));
				}
				HResult hResult = ppv.SetValue(ref propertyKey, propVar);
				if (!AllowSetTruncatedValue && hResult == (HResult)262560)
				{
					throw new ArgumentOutOfRangeException("propVar", LocalizedMessages.ShellPropertyValueTruncated);
				}
				if (!CoreErrorHelper.Succeeded(hResult))
				{
					throw new PropertySystemException(LocalizedMessages.ShellPropertySetValue, Marshal.GetExceptionForHR((int)hResult));
				}
				ppv.Commit();
			}
			catch (InvalidComObjectException innerException)
			{
				throw new PropertySystemException(LocalizedMessages.ShellPropertyUnableToGetWritableProperty, innerException);
			}
			catch (InvalidCastException)
			{
				throw new PropertySystemException(LocalizedMessages.ShellPropertyUnableToGetWritableProperty);
			}
			finally
			{
				if (ppv != null)
				{
					Marshal.ReleaseComObject(ppv);
					ppv = null;
				}
			}
		}

		internal ShellProperty(PropertyKey propertyKey, ShellPropertyDescription description, ShellObject parent)
		{
			this.propertyKey = propertyKey;
			this.description = description;
			ParentShellObject = parent;
			AllowSetTruncatedValue = false;
		}

		internal ShellProperty(PropertyKey propertyKey, ShellPropertyDescription description, IPropertyStore propertyStore)
		{
			this.propertyKey = propertyKey;
			this.description = description;
			NativePropertyStore = propertyStore;
			AllowSetTruncatedValue = false;
		}

		public bool TryFormatForDisplay(PropertyDescriptionFormatOptions format, out string formattedString)
		{
			if (Description == null || Description.NativePropertyDescription == null)
			{
				formattedString = null;
				return false;
			}
			IPropertyStore propertyStore = ShellPropertyCollection.CreateDefaultPropertyStore(ParentShellObject);
			using (PropVariant propVariant = new PropVariant())
			{
				propertyStore.GetValue(ref propertyKey, propVariant);
				Marshal.ReleaseComObject(propertyStore);
				propertyStore = null;
				HResult result = Description.NativePropertyDescription.FormatForDisplay(propVariant, ref format, out formattedString);
				if (!CoreErrorHelper.Succeeded(result))
				{
					formattedString = null;
					return false;
				}
				return true;
			}
		}

		public string FormatForDisplay(PropertyDescriptionFormatOptions format)
		{
			if (Description == null || Description.NativePropertyDescription == null)
			{
				return null;
			}
			IPropertyStore propertyStore = ShellPropertyCollection.CreateDefaultPropertyStore(ParentShellObject);
			using (PropVariant propVariant = new PropVariant())
			{
				propertyStore.GetValue(ref propertyKey, propVariant);
				Marshal.ReleaseComObject(propertyStore);
				propertyStore = null;
				string ppszDisplay;
				HResult result = Description.NativePropertyDescription.FormatForDisplay(propVariant, ref format, out ppszDisplay);
				if (!CoreErrorHelper.Succeeded(result))
				{
					throw new ShellException(result);
				}
				return ppszDisplay;
			}
		}

		public void ClearValue()
		{
			using (PropVariant propVar = new PropVariant())
			{
				StorePropVariantValue(propVar);
			}
		}
	}
}
