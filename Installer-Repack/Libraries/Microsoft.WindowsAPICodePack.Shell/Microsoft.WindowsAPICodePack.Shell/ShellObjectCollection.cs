using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.WindowsAPICodePack.Shell.Resources;

namespace Microsoft.WindowsAPICodePack.Shell
{
	public class ShellObjectCollection : IDisposable, IList<ShellObject>, ICollection<ShellObject>, IEnumerable<ShellObject>, IEnumerable
	{
		private List<ShellObject> content = new List<ShellObject>();

		private bool readOnly;

		private bool isDisposed;

		public int Count => content.Count;

		public ShellObject this[int index]
		{
			get
			{
				return content[index];
			}
			set
			{
				if (readOnly)
				{
					throw new InvalidOperationException(LocalizedMessages.ShellObjectCollectionInsertReadOnly);
				}
				content[index] = value;
			}
		}

		int ICollection<ShellObject>.Count => content.Count;

		public bool IsReadOnly => readOnly;

		internal ShellObjectCollection(IShellItemArray iArray, bool readOnly)
		{
			this.readOnly = readOnly;
			if (iArray == null)
			{
				return;
			}
			try
			{
				uint pdwNumItems = 0u;
				iArray.GetCount(out pdwNumItems);
				content.Capacity = (int)pdwNumItems;
				for (uint num = 0u; num < pdwNumItems; num++)
				{
					IShellItem ppsi = null;
					iArray.GetItemAt(num, out ppsi);
					content.Add(ShellObjectFactory.Create(ppsi));
				}
			}
			finally
			{
				Marshal.ReleaseComObject(iArray);
			}
		}

		public static ShellObjectCollection FromDataObject(IDataObject dataObject)
		{
			Guid riid = new Guid("B63EA76D-1F85-456F-A19C-48159EFA858B");
			ShellNativeMethods.SHCreateShellItemArrayFromDataObject(dataObject, ref riid, out var iShellItemArray);
			return new ShellObjectCollection(iShellItemArray, true);
		}

		public ShellObjectCollection()
		{
		}

		~ShellObjectCollection()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (isDisposed)
			{
				return;
			}
			if (disposing)
			{
				foreach (ShellObject item in content)
				{
					item.Dispose();
				}
				content.Clear();
			}
			isDisposed = true;
		}

		public IEnumerator GetEnumerator()
		{
			foreach (ShellObject item in content)
			{
				yield return item;
			}
		}

		public MemoryStream BuildShellIDList()
		{
			if (content.Count == 0)
			{
				throw new InvalidOperationException(LocalizedMessages.ShellObjectCollectionEmptyCollection);
			}
			MemoryStream memoryStream = new MemoryStream();
			try
			{
				BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
				uint num = (uint)(content.Count + 1);
				IntPtr[] array = new IntPtr[num];
				for (int i = 0; i < num; i++)
				{
					if (i == 0)
					{
						ref IntPtr reference = ref array[i];
						reference = ((ShellObject)KnownFolders.Desktop).PIDL;
					}
					else
					{
						ref IntPtr reference2 = ref array[i];
						reference2 = content[i - 1].PIDL;
					}
				}
				uint[] array2 = new uint[num + 1];
				for (int i = 0; i < num; i++)
				{
					if (i == 0)
					{
						array2[0] = (uint)(4 * (array2.Length + 1));
					}
					else
					{
						array2[i] = array2[i - 1] + ShellNativeMethods.ILGetSize(array[i - 1]);
					}
				}
				binaryWriter.Write(content.Count);
				uint[] array3 = array2;
				foreach (uint value in array3)
				{
					binaryWriter.Write(value);
				}
				IntPtr[] array4 = array;
				foreach (IntPtr intPtr in array4)
				{
					byte[] array5 = new byte[ShellNativeMethods.ILGetSize(intPtr)];
					Marshal.Copy(intPtr, array5, 0, array5.Length);
					binaryWriter.Write(array5, 0, array5.Length);
				}
			}
			catch
			{
				memoryStream.Dispose();
				throw;
			}
			return memoryStream;
		}

		public int IndexOf(ShellObject item)
		{
			return content.IndexOf(item);
		}

		public void Insert(int index, ShellObject item)
		{
			if (readOnly)
			{
				throw new InvalidOperationException(LocalizedMessages.ShellObjectCollectionInsertReadOnly);
			}
			content.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			if (readOnly)
			{
				throw new InvalidOperationException(LocalizedMessages.ShellObjectCollectionRemoveReadOnly);
			}
			content.RemoveAt(index);
		}

		public void Add(ShellObject item)
		{
			if (readOnly)
			{
				throw new InvalidOperationException(LocalizedMessages.ShellObjectCollectionInsertReadOnly);
			}
			content.Add(item);
		}

		public void Clear()
		{
			if (readOnly)
			{
				throw new InvalidOperationException(LocalizedMessages.ShellObjectCollectionRemoveReadOnly);
			}
			content.Clear();
		}

		public bool Contains(ShellObject item)
		{
			return content.Contains(item);
		}

		public void CopyTo(ShellObject[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Length < arrayIndex + content.Count)
			{
				throw new ArgumentException(LocalizedMessages.ShellObjectCollectionArrayTooSmall, "array");
			}
			for (int i = 0; i < content.Count; i++)
			{
				array[i + arrayIndex] = content[i];
			}
		}

		public bool Remove(ShellObject item)
		{
			if (readOnly)
			{
				throw new InvalidOperationException(LocalizedMessages.ShellObjectCollectionRemoveReadOnly);
			}
			return content.Remove(item);
		}

		IEnumerator<ShellObject> IEnumerable<ShellObject>.GetEnumerator()
		{
			foreach (ShellObject item in content)
			{
				yield return item;
			}
		}
	}
}
