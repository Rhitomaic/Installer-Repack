using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	public class JumpListCustomCategory
	{
		private string name;

		internal JumpListItemCollection<IJumpListItem> JumpListItems { get; private set; }

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				if (value != name)
				{
					name = value;
					this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
				}
			}
		}

		internal event NotifyCollectionChangedEventHandler CollectionChanged = delegate
		{
		};

		public void AddJumpListItems(params IJumpListItem[] items)
		{
			if (items != null)
			{
				foreach (IJumpListItem item in items)
				{
					JumpListItems.Add(item);
				}
			}
		}

		public JumpListCustomCategory(string categoryName)
		{
			Name = categoryName;
			JumpListItems = new JumpListItemCollection<IJumpListItem>();
			JumpListItems.CollectionChanged += OnJumpListCollectionChanged;
		}

		internal void OnJumpListCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			this.CollectionChanged(this, args);
		}

		internal void RemoveJumpListItem(string path)
		{
			List<IJumpListItem> list = new List<IJumpListItem>(JumpListItems.Where((IJumpListItem i) => string.Equals(path, i.Path, StringComparison.OrdinalIgnoreCase)));
			for (int j = 0; j < list.Count; j++)
			{
				JumpListItems.Remove(list[j]);
			}
		}
	}
}
