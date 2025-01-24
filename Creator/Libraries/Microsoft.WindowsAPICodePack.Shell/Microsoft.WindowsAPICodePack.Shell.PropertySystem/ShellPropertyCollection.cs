using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Shell.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	public class ShellPropertyCollection : ReadOnlyCollection<IShellProperty>, IDisposable
	{
		private ShellObject ParentShellObject { get; set; }

		private IPropertyStore NativePropertyStore { get; set; }

		public IShellProperty this[string canonicalName]
		{
			get
			{
				if (string.IsNullOrEmpty(canonicalName))
				{
					throw new ArgumentException(LocalizedMessages.PropertyCollectionNullCanonicalName, "canonicalName");
				}
				IShellProperty shellProperty = base.Items.FirstOrDefault((IShellProperty p) => p.CanonicalName == canonicalName);
				if (shellProperty == null)
				{
					throw new IndexOutOfRangeException(LocalizedMessages.PropertyCollectionCanonicalInvalidIndex);
				}
				return shellProperty;
			}
		}

		public IShellProperty this[PropertyKey key]
		{
			get
			{
				IShellProperty shellProperty = base.Items.FirstOrDefault((IShellProperty p) => p.PropertyKey == key);
				if (shellProperty != null)
				{
					return shellProperty;
				}
				throw new IndexOutOfRangeException(LocalizedMessages.PropertyCollectionInvalidIndex);
			}
		}

		internal ShellPropertyCollection(IPropertyStore nativePropertyStore)
			: base((IList<IShellProperty>)new List<IShellProperty>())
		{
			NativePropertyStore = nativePropertyStore;
			AddProperties(nativePropertyStore);
		}

		public ShellPropertyCollection(ShellObject parent)
			: base((IList<IShellProperty>)new List<IShellProperty>())
		{
			ParentShellObject = parent;
			IPropertyStore propertyStore = null;
			try
			{
				propertyStore = CreateDefaultPropertyStore(ParentShellObject);
				AddProperties(propertyStore);
			}
			catch
			{
				if (parent != null)
				{
					parent.Dispose();
				}
				throw;
			}
			finally
			{
				if (propertyStore != null)
				{
					Marshal.ReleaseComObject(propertyStore);
					propertyStore = null;
				}
			}
		}

		public ShellPropertyCollection(string path)
			: this(ShellObjectFactory.Create(path))
		{
		}

		private void AddProperties(IPropertyStore nativePropertyStore)
		{
			nativePropertyStore.GetCount(out var propertyCount);
			for (uint num = 0u; num < propertyCount; num++)
			{
				nativePropertyStore.GetAt(num, out var key);
				if (ParentShellObject != null)
				{
					base.Items.Add(ParentShellObject.Properties.CreateTypedProperty(key));
				}
				else
				{
					base.Items.Add(CreateTypedProperty(key, NativePropertyStore));
				}
			}
		}

		internal static IPropertyStore CreateDefaultPropertyStore(ShellObject shellObj)
		{
			IPropertyStore ppv = null;
			Guid riid = new Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99");
			int propertyStore = shellObj.NativeShellItem2.GetPropertyStore(ShellNativeMethods.GetPropertyStoreOptions.BestEffort, ref riid, out ppv);
			if (ppv == null || !CoreErrorHelper.Succeeded(propertyStore))
			{
				throw new ShellException(propertyStore);
			}
			return ppv;
		}

		public bool Contains(string canonicalName)
		{
			if (string.IsNullOrEmpty(canonicalName))
			{
				throw new ArgumentException(LocalizedMessages.PropertyCollectionNullCanonicalName, "canonicalName");
			}
			return base.Items.Any((IShellProperty p) => p.CanonicalName == canonicalName);
		}

		public bool Contains(PropertyKey key)
		{
			return base.Items.Any((IShellProperty p) => p.PropertyKey == key);
		}

		internal static IShellProperty CreateTypedProperty(PropertyKey propKey, IPropertyStore NativePropertyStore)
		{
			return ShellPropertyFactory.CreateShellProperty(propKey, NativePropertyStore);
		}

		protected virtual void Dispose(bool disposing)
		{
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

		~ShellPropertyCollection()
		{
			Dispose(false);
		}
	}
}
