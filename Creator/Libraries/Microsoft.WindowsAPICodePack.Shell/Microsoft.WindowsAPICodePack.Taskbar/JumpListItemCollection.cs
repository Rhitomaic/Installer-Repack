using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	internal class JumpListItemCollection<T> : ICollection<T>, IEnumerable<T>, IEnumerable, INotifyCollectionChanged
	{
		private List<T> items = new List<T>();

		public bool IsReadOnly { get; set; }

		public int Count => items.Count;

		public event NotifyCollectionChangedEventHandler CollectionChanged = delegate
		{
		};

		public void Add(T item)
		{
			items.Add(item);
			this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
		}

		public bool Remove(T item)
		{
			bool flag = items.Remove(item);
			if (flag)
			{
				this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, 0));
			}
			return flag;
		}

		public void Clear()
		{
			items.Clear();
			this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		public bool Contains(T item)
		{
			return items.Contains(item);
		}

		public void CopyTo(T[] array, int index)
		{
			items.CopyTo(array, index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return items.GetEnumerator();
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return items.GetEnumerator();
		}
	}
}
