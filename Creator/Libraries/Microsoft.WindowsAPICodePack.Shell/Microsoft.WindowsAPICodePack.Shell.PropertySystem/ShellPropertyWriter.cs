using System;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Shell.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	public class ShellPropertyWriter : IDisposable
	{
		private ShellObject parentShellObject;

		internal IPropertyStore writablePropStore;

		protected ShellObject ParentShellObject
		{
			get
			{
				return parentShellObject;
			}
			private set
			{
				parentShellObject = value;
			}
		}

		internal ShellPropertyWriter(ShellObject parent)
		{
			ParentShellObject = parent;
			Guid riid = new Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99");
			try
			{
				int propertyStore = ParentShellObject.NativeShellItem2.GetPropertyStore(ShellNativeMethods.GetPropertyStoreOptions.ReadWrite, ref riid, out writablePropStore);
				if (!CoreErrorHelper.Succeeded(propertyStore))
				{
					throw new PropertySystemException(LocalizedMessages.ShellPropertyUnableToGetWritableProperty, Marshal.GetExceptionForHR(propertyStore));
				}
				if (ParentShellObject.NativePropertyStore == null)
				{
					ParentShellObject.NativePropertyStore = writablePropStore;
				}
			}
			catch (InvalidComObjectException innerException)
			{
				throw new PropertySystemException(LocalizedMessages.ShellPropertyUnableToGetWritableProperty, innerException);
			}
			catch (InvalidCastException)
			{
				throw new PropertySystemException(LocalizedMessages.ShellPropertyUnableToGetWritableProperty);
			}
		}

		public void WriteProperty(PropertyKey key, object value)
		{
			WriteProperty(key, value, true);
		}

		public void WriteProperty(PropertyKey key, object value, bool allowTruncatedValue)
		{
			if (writablePropStore == null)
			{
				throw new InvalidOperationException("Writeable store has been closed.");
			}
			using (PropVariant pv = PropVariant.FromObject(value))
			{
				HResult hResult = writablePropStore.SetValue(ref key, pv);
				if (!allowTruncatedValue && hResult == (HResult)262560)
				{
					Marshal.ReleaseComObject(writablePropStore);
					writablePropStore = null;
					throw new ArgumentOutOfRangeException("value", LocalizedMessages.ShellPropertyValueTruncated);
				}
				if (!CoreErrorHelper.Succeeded(hResult))
				{
					throw new PropertySystemException(LocalizedMessages.ShellPropertySetValue, Marshal.GetExceptionForHR((int)hResult));
				}
			}
		}

		public void WriteProperty(string canonicalName, object value)
		{
			WriteProperty(canonicalName, value, true);
		}

		public void WriteProperty(string canonicalName, object value, bool allowTruncatedValue)
		{
			PropertyKey propkey;
			int num = PropertySystemNativeMethods.PSGetPropertyKeyFromName(canonicalName, out propkey);
			if (!CoreErrorHelper.Succeeded(num))
			{
				throw new ArgumentException(LocalizedMessages.ShellInvalidCanonicalName, Marshal.GetExceptionForHR(num));
			}
			WriteProperty(propkey, value, allowTruncatedValue);
		}

		public void WriteProperty(IShellProperty shellProperty, object value)
		{
			WriteProperty(shellProperty, value, true);
		}

		public void WriteProperty(IShellProperty shellProperty, object value, bool allowTruncatedValue)
		{
			if (shellProperty == null)
			{
				throw new ArgumentNullException("shellProperty");
			}
			WriteProperty(shellProperty.PropertyKey, value, allowTruncatedValue);
		}

		public void WriteProperty<T>(ShellProperty<T> shellProperty, T value)
		{
			WriteProperty(shellProperty, value, true);
		}

		public void WriteProperty<T>(ShellProperty<T> shellProperty, T value, bool allowTruncatedValue)
		{
			if (shellProperty == null)
			{
				throw new ArgumentNullException("shellProperty");
			}
			WriteProperty(shellProperty.PropertyKey, value, allowTruncatedValue);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~ShellPropertyWriter()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			Close();
		}

		public void Close()
		{
			if (writablePropStore != null)
			{
				writablePropStore.Commit();
				Marshal.ReleaseComObject(writablePropStore);
				writablePropStore = null;
			}
			ParentShellObject.NativePropertyStore = null;
		}
	}
}
