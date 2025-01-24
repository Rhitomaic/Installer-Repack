using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.WindowsAPICodePack.Shell
{
	internal class ChangeNotifyEventManager
	{
		private static readonly ShellObjectChangeTypes[] _changeOrder = new ShellObjectChangeTypes[22]
		{
			ShellObjectChangeTypes.ItemCreate,
			ShellObjectChangeTypes.ItemRename,
			ShellObjectChangeTypes.ItemDelete,
			ShellObjectChangeTypes.AttributesChange,
			ShellObjectChangeTypes.DirectoryCreate,
			ShellObjectChangeTypes.DirectoryDelete,
			ShellObjectChangeTypes.DirectoryContentsUpdate,
			ShellObjectChangeTypes.DirectoryRename,
			ShellObjectChangeTypes.Update,
			ShellObjectChangeTypes.MediaInsert,
			ShellObjectChangeTypes.MediaRemove,
			ShellObjectChangeTypes.DriveAdd,
			ShellObjectChangeTypes.DriveRemove,
			ShellObjectChangeTypes.NetShare,
			ShellObjectChangeTypes.NetUnshare,
			ShellObjectChangeTypes.ServerDisconnect,
			ShellObjectChangeTypes.SystemImageUpdate,
			ShellObjectChangeTypes.AssociationChange,
			ShellObjectChangeTypes.FreeSpace,
			ShellObjectChangeTypes.DiskEventsMask,
			ShellObjectChangeTypes.GlobalEventsMask,
			ShellObjectChangeTypes.AllEventsMask
		};

		private Dictionary<ShellObjectChangeTypes, Delegate> _events = new Dictionary<ShellObjectChangeTypes, Delegate>();

		public ShellObjectChangeTypes RegisteredTypes => _events.Keys.Aggregate(ShellObjectChangeTypes.None, (ShellObjectChangeTypes accumulator, ShellObjectChangeTypes changeType) => changeType | accumulator);

		public void Register(ShellObjectChangeTypes changeType, Delegate handler)
		{
			if (!_events.TryGetValue(changeType, out var value))
			{
				_events.Add(changeType, handler);
				return;
			}
			value = Delegate.Combine(value, handler);
			_events[changeType] = value;
		}

		public void Unregister(ShellObjectChangeTypes changeType, Delegate handler)
		{
			if (_events.TryGetValue(changeType, out var value))
			{
				value = Delegate.Remove(value, handler);
				if ((object)value == null)
				{
					_events.Remove(changeType);
				}
				else
				{
					_events[changeType] = value;
				}
			}
		}

		public void UnregisterAll()
		{
			_events.Clear();
		}

		public void Invoke(object sender, ShellObjectChangeTypes changeType, EventArgs args)
		{
			changeType &= ShellObjectChangeTypes.AllEventsMask;
			foreach (ShellObjectChangeTypes item in _changeOrder.Where((ShellObjectChangeTypes x) => (x & changeType) != 0))
			{
				if (_events.TryGetValue(item, out var value))
				{
					value.DynamicInvoke(sender, args);
				}
			}
		}
	}
}
