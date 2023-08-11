using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	internal class JumpListCustomCategoryCollection : ICollection<JumpListCustomCategory>, IEnumerable<JumpListCustomCategory>, IEnumerable, INotifyCollectionChanged
	{
		private List<JumpListCustomCategory> categories = new List<JumpListCustomCategory>();

		public bool IsReadOnly { get; set; }

		public int Count => categories.Count;

		public event NotifyCollectionChangedEventHandler CollectionChanged = delegate
		{
		};

		public void Add(JumpListCustomCategory category)
		{
			if (category == null)
			{
				throw new ArgumentNullException("category");
			}
			categories.Add(category);
			this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, category));
			category.CollectionChanged += this.CollectionChanged;
			category.JumpListItems.CollectionChanged += this.CollectionChanged;
		}

		public bool Remove(JumpListCustomCategory category)
		{
			bool flag = categories.Remove(category);
			if (flag)
			{
				this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, 0));
			}
			return flag;
		}

		public void Clear()
		{
			categories.Clear();
			this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		public bool Contains(JumpListCustomCategory category)
		{
			return categories.Contains(category);
		}

		public void CopyTo(JumpListCustomCategory[] array, int index)
		{
			categories.CopyTo(array, index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return categories.GetEnumerator();
		}

		IEnumerator<JumpListCustomCategory> IEnumerable<JumpListCustomCategory>.GetEnumerator()
		{
			return categories.GetEnumerator();
		}
	}
}
