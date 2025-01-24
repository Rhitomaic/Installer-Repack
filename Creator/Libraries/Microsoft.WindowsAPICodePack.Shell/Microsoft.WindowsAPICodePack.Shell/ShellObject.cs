using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using Microsoft.WindowsAPICodePack.Shell.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	public abstract class ShellObject : IDisposable, IEquatable<ShellObject>
	{
		internal IShellItem2 nativeShellItem;

		private string _internalParsingName;

		private string _internalName;

		private IntPtr _internalPIDL = IntPtr.Zero;

		private ShellProperties properties;

		private ShellThumbnail thumbnail;

		private ShellObject parentShellObject;

		private static MD5CryptoServiceProvider hashProvider = new MD5CryptoServiceProvider();

		private int? hashValue;

		public static bool IsPlatformSupported => CoreHelpers.RunningOnVista;

		internal virtual IShellItem2 NativeShellItem2
		{
			get
			{
				if (nativeShellItem == null && ParsingName != null)
				{
					Guid riid = new Guid("7E9FB0D3-919F-4307-AB2E-9B1860310C93");
					int num = ShellNativeMethods.SHCreateItemFromParsingName(ParsingName, IntPtr.Zero, ref riid, out nativeShellItem);
					if (nativeShellItem == null || !CoreErrorHelper.Succeeded(num))
					{
						throw new ShellException(LocalizedMessages.ShellObjectCreationFailed, Marshal.GetExceptionForHR(num));
					}
				}
				return nativeShellItem;
			}
		}

		internal virtual IShellItem NativeShellItem => NativeShellItem2;

		internal IPropertyStore NativePropertyStore { get; set; }

		public ShellProperties Properties
		{
			get
			{
				if (properties == null)
				{
					properties = new ShellProperties(this);
				}
				return properties;
			}
		}

		public virtual string ParsingName
		{
			get
			{
				if (_internalParsingName == null && nativeShellItem != null)
				{
					_internalParsingName = ShellHelper.GetParsingName(nativeShellItem);
				}
				return _internalParsingName ?? string.Empty;
			}
			protected set
			{
				_internalParsingName = value;
			}
		}

		public virtual string Name
		{
			get
			{
				if (_internalName == null && NativeShellItem != null)
				{
					IntPtr ppszName = IntPtr.Zero;
					if (NativeShellItem.GetDisplayName(ShellNativeMethods.ShellItemDesignNameOptions.Normal, out ppszName) == HResult.Ok && ppszName != IntPtr.Zero)
					{
						_internalName = Marshal.PtrToStringAuto(ppszName);
						Marshal.FreeCoTaskMem(ppszName);
					}
				}
				return _internalName;
			}
			protected set
			{
				_internalName = value;
			}
		}

		internal virtual IntPtr PIDL
		{
			get
			{
				if (_internalPIDL == IntPtr.Zero && NativeShellItem != null)
				{
					_internalPIDL = ShellHelper.PidlFromShellItem(NativeShellItem);
				}
				return _internalPIDL;
			}
			set
			{
				_internalPIDL = value;
			}
		}

		public bool IsLink
		{
			get
			{
				try
				{
					NativeShellItem.GetAttributes(ShellNativeMethods.ShellFileGetAttributesOptions.Link, out var psfgaoAttribs);
					return (psfgaoAttribs & ShellNativeMethods.ShellFileGetAttributesOptions.Link) != 0;
				}
				catch (FileNotFoundException)
				{
					return false;
				}
				catch (NullReferenceException)
				{
					return false;
				}
			}
		}

		public bool IsFileSystemObject
		{
			get
			{
				try
				{
					NativeShellItem.GetAttributes(ShellNativeMethods.ShellFileGetAttributesOptions.FileSystem, out var psfgaoAttribs);
					return (psfgaoAttribs & ShellNativeMethods.ShellFileGetAttributesOptions.FileSystem) != 0;
				}
				catch (FileNotFoundException)
				{
					return false;
				}
				catch (NullReferenceException)
				{
					return false;
				}
			}
		}

		public ShellThumbnail Thumbnail
		{
			get
			{
				if (thumbnail == null)
				{
					thumbnail = new ShellThumbnail(this);
				}
				return thumbnail;
			}
		}

		public ShellObject Parent
		{
			get
			{
				if (parentShellObject == null && NativeShellItem2 != null)
				{
					IShellItem ppsi;
					HResult parent = NativeShellItem2.GetParent(out ppsi);
					if (parent != 0 || ppsi == null)
					{
						if (parent == HResult.NoObject)
						{
							return null;
						}
						throw new ShellException(parent);
					}
					parentShellObject = ShellObjectFactory.Create(ppsi);
				}
				return parentShellObject;
			}
		}

		public static ShellObject FromParsingName(string parsingName)
		{
			return ShellObjectFactory.Create(parsingName);
		}

		internal ShellObject()
		{
		}

		internal ShellObject(IShellItem2 shellItem)
		{
			nativeShellItem = shellItem;
		}

		public void Update(IBindCtx bindContext)
		{
			HResult result = HResult.Ok;
			if (NativeShellItem2 != null)
			{
				result = NativeShellItem2.Update(bindContext);
			}
			if (CoreErrorHelper.Failed(result))
			{
				throw new ShellException(result);
			}
		}

		public override string ToString()
		{
			return Name;
		}

		public virtual string GetDisplayName(DisplayNameType displayNameType)
		{
			string ppszName = null;
			HResult hResult = HResult.Ok;
			if (NativeShellItem2 != null)
			{
				hResult = NativeShellItem2.GetDisplayName((ShellNativeMethods.ShellItemDesignNameOptions)displayNameType, out ppszName);
			}
			if (hResult != 0)
			{
				throw new ShellException(LocalizedMessages.ShellObjectCannotGetDisplayName, hResult);
			}
			return ppszName;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_internalName = null;
				_internalParsingName = null;
				properties = null;
				thumbnail = null;
				parentShellObject = null;
			}
			if (properties != null)
			{
				properties.Dispose();
			}
			if (_internalPIDL != IntPtr.Zero)
			{
				ShellNativeMethods.ILFree(_internalPIDL);
				_internalPIDL = IntPtr.Zero;
			}
			if (nativeShellItem != null)
			{
				Marshal.ReleaseComObject(nativeShellItem);
				nativeShellItem = null;
			}
			if (NativePropertyStore != null)
			{
				Marshal.ReleaseComObject(NativePropertyStore);
				NativePropertyStore = null;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~ShellObject()
		{
			Dispose(false);
		}

		public override int GetHashCode()
		{
			if (!hashValue.HasValue)
			{
				uint num = ShellNativeMethods.ILGetSize(PIDL);
				if (num != 0)
				{
					byte[] array = new byte[num];
					Marshal.Copy(PIDL, array, 0, (int)num);
					byte[] value = hashProvider.ComputeHash(array);
					hashValue = BitConverter.ToInt32(value, 0);
				}
				else
				{
					hashValue = 0;
				}
			}
			return hashValue.Value;
		}

		public bool Equals(ShellObject other)
		{
			bool result = false;
			if (other != null)
			{
				IShellItem shellItem = NativeShellItem;
				IShellItem shellItem2 = other.NativeShellItem;
				if (shellItem != null && shellItem2 != null)
				{
					int piOrder = 0;
					result = shellItem.Compare(shellItem2, SICHINTF.SICHINT_ALLFIELDS, out piOrder) == HResult.Ok && piOrder == 0;
				}
			}
			return result;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as ShellObject);
		}

		public static bool operator ==(ShellObject leftShellObject, ShellObject rightShellObject)
		{
			return leftShellObject?.Equals(rightShellObject) ?? ((object)rightShellObject == null);
		}

		public static bool operator !=(ShellObject leftShellObject, ShellObject rightShellObject)
		{
			return !(leftShellObject == rightShellObject);
		}
	}
}
