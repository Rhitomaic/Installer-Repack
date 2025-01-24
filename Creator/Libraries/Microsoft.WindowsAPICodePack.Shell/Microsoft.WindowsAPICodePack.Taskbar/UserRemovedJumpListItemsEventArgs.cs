using System;
using System.Collections;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	public class UserRemovedJumpListItemsEventArgs : EventArgs
	{
		private readonly IEnumerable _removedItems;

		public IEnumerable RemovedItems => _removedItems;

		internal UserRemovedJumpListItemsEventArgs(IEnumerable RemovedItems)
		{
			_removedItems = RemovedItems;
		}
	}
}
