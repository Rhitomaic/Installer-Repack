using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using Microsoft.WindowsAPICodePack.Shell.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	public class ShellSearchFolder : ShellSearchCollection
	{
		private SearchCondition searchCondition;

		private string[] searchScopePaths;

		internal ISearchFolderItemFactory NativeSearchFolderItemFactory { get; set; }

		public SearchCondition SearchCondition
		{
			get
			{
				return searchCondition;
			}
			private set
			{
				searchCondition = value;
				NativeSearchFolderItemFactory.SetCondition(searchCondition.NativeSearchCondition);
			}
		}

		public IEnumerable<string> SearchScopePaths
		{
			get
			{
				try
				{
					string[] array = searchScopePaths;
					for (int i = 0; i < array.Length; i++)
					{
						yield return array[i];
					}
				}
				finally
				{
				}
			}
			private set
			{
				searchScopePaths = value.ToArray();
				List<IShellItem> list = new List<IShellItem>(searchScopePaths.Length);
				Guid riid = new Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE");
				Guid guid = new Guid("B63EA76D-1F85-456F-A19C-48159EFA858B");
				string[] array = searchScopePaths;
				foreach (string path in array)
				{
					IShellItem shellItem;
					int result = ShellNativeMethods.SHCreateItemFromParsingName(path, IntPtr.Zero, ref riid, out shellItem);
					if (CoreErrorHelper.Succeeded(result))
					{
						list.Add(shellItem);
					}
				}
				IShellItemArray scope = new ShellItemArray(list.ToArray());
				HResult hResult = NativeSearchFolderItemFactory.SetScope(scope);
				if (!CoreErrorHelper.Succeeded((int)hResult))
				{
					throw new ShellException((int)hResult);
				}
			}
		}

		internal override IShellItem NativeShellItem
		{
			get
			{
				Guid riid = new Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE");
				if (NativeSearchFolderItemFactory == null)
				{
					return null;
				}
				IShellItem ppv;
				int shellItem = NativeSearchFolderItemFactory.GetShellItem(ref riid, out ppv);
				if (!CoreErrorHelper.Succeeded(shellItem))
				{
					throw new ShellException(shellItem);
				}
				return ppv;
			}
		}

		public ShellSearchFolder(SearchCondition searchCondition, params ShellContainer[] searchScopePath)
		{
			CoreHelpers.ThrowIfNotVista();
			NativeSearchFolderItemFactory = (ISearchFolderItemFactory)new SearchFolderItemFactoryCoClass();
			SearchCondition = searchCondition;
			if (searchScopePath != null && searchScopePath.Length > 0 && searchScopePath[0] != null)
			{
				SearchScopePaths = searchScopePath.Select((ShellContainer cont) => cont.ParsingName);
			}
		}

		public ShellSearchFolder(SearchCondition searchCondition, params string[] searchScopePath)
		{
			CoreHelpers.ThrowIfNotVista();
			NativeSearchFolderItemFactory = (ISearchFolderItemFactory)new SearchFolderItemFactoryCoClass();
			if (searchScopePath != null && searchScopePath.Length > 0 && searchScopePath[0] != null)
			{
				SearchScopePaths = searchScopePath;
			}
			SearchCondition = searchCondition;
		}

		public void SetStacks(params string[] canonicalNames)
		{
			if (canonicalNames == null)
			{
				throw new ArgumentNullException("canonicalNames");
			}
			List<PropertyKey> list = new List<PropertyKey>();
			foreach (string pszCanonicalName in canonicalNames)
			{
				PropertyKey propkey;
				int num = PropertySystemNativeMethods.PSGetPropertyKeyFromName(pszCanonicalName, out propkey);
				if (!CoreErrorHelper.Succeeded(num))
				{
					throw new ArgumentException(LocalizedMessages.ShellInvalidCanonicalName, "canonicalNames", Marshal.GetExceptionForHR(num));
				}
				list.Add(propkey);
			}
			if (list.Count > 0)
			{
				SetStacks(list.ToArray());
			}
		}

		public void SetStacks(params PropertyKey[] propertyKeys)
		{
			if (propertyKeys != null && propertyKeys.Length > 0)
			{
				NativeSearchFolderItemFactory.SetStacks((uint)propertyKeys.Length, propertyKeys);
			}
		}

		public void SetDisplayName(string displayName)
		{
			HResult result = NativeSearchFolderItemFactory.SetDisplayName(displayName);
			if (!CoreErrorHelper.Succeeded(result))
			{
				throw new ShellException(result);
			}
		}

		public void SetIconSize(int value)
		{
			HResult result = NativeSearchFolderItemFactory.SetIconSize(value);
			if (!CoreErrorHelper.Succeeded(result))
			{
				throw new ShellException(result);
			}
		}

		public void SetFolderTypeID(Guid value)
		{
			HResult result = NativeSearchFolderItemFactory.SetFolderTypeID(value);
			if (!CoreErrorHelper.Succeeded(result))
			{
				throw new ShellException(result);
			}
		}

		public void SetFolderLogicalViewMode(FolderLogicalViewMode mode)
		{
			HResult result = NativeSearchFolderItemFactory.SetFolderLogicalViewMode(mode);
			if (!CoreErrorHelper.Succeeded(result))
			{
				throw new ShellException(result);
			}
		}

		public void SetVisibleColumns(PropertyKey[] value)
		{
			HResult hResult = NativeSearchFolderItemFactory.SetVisibleColumns((value != null) ? ((uint)value.Length) : 0u, value);
			if (!CoreErrorHelper.Succeeded(hResult))
			{
				throw new ShellException(LocalizedMessages.ShellSearchFolderUnableToSetVisibleColumns, Marshal.GetExceptionForHR((int)hResult));
			}
		}

		public void SortColumns(SortColumn[] value)
		{
			HResult hResult = NativeSearchFolderItemFactory.SetSortColumns((value != null) ? ((uint)value.Length) : 0u, value);
			if (!CoreErrorHelper.Succeeded(hResult))
			{
				throw new ShellException(LocalizedMessages.ShellSearchFolderUnableToSetSortColumns, Marshal.GetExceptionForHR((int)hResult));
			}
		}

		public void SetGroupColumn(PropertyKey propertyKey)
		{
			HResult result = NativeSearchFolderItemFactory.SetGroupColumn(ref propertyKey);
			if (!CoreErrorHelper.Succeeded(result))
			{
				throw new ShellException(result);
			}
		}
	}
}
