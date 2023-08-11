using System;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.Controls.WindowsForms;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.Resources;

namespace Microsoft.WindowsAPICodePack.Controls
{
	public class ExplorerBrowserNavigationLog
	{
		private List<ShellObject> _locations = new List<ShellObject>();

		private ExplorerBrowser parent = null;

		private PendingNavigation pendingNavigation;

		private int currentLocationIndex = -1;

		public bool CanNavigateForward => CurrentLocationIndex < _locations.Count - 1;

		public bool CanNavigateBackward => CurrentLocationIndex > 0;

		public IEnumerable<ShellObject> Locations
		{
			get
			{
				foreach (ShellObject location in _locations)
				{
					yield return location;
				}
			}
		}

		public int CurrentLocationIndex => currentLocationIndex;

		public ShellObject CurrentLocation
		{
			get
			{
				if (currentLocationIndex < 0)
				{
					return null;
				}
				return _locations[currentLocationIndex];
			}
		}

		public event EventHandler<NavigationLogEventArgs> NavigationLogChanged;

		public void ClearLog()
		{
			if (_locations.Count != 0)
			{
				bool canNavigateBackward = CanNavigateBackward;
				bool canNavigateForward = CanNavigateForward;
				_locations.Clear();
				currentLocationIndex = -1;
				NavigationLogEventArgs navigationLogEventArgs = new NavigationLogEventArgs();
				navigationLogEventArgs.LocationsChanged = true;
				navigationLogEventArgs.CanNavigateBackwardChanged = canNavigateBackward != CanNavigateBackward;
				navigationLogEventArgs.CanNavigateForwardChanged = canNavigateForward != CanNavigateForward;
				if (this.NavigationLogChanged != null)
				{
					this.NavigationLogChanged(this, navigationLogEventArgs);
				}
			}
		}

		internal ExplorerBrowserNavigationLog(ExplorerBrowser parent)
		{
			if (parent == null)
			{
				throw new ArgumentException(LocalizedMessages.NavigationLogNullParent, "parent");
			}
			this.parent = parent;
			this.parent.NavigationComplete += OnNavigationComplete;
			this.parent.NavigationFailed += OnNavigationFailed;
		}

		private void OnNavigationFailed(object sender, NavigationFailedEventArgs args)
		{
			pendingNavigation = null;
		}

		private void OnNavigationComplete(object sender, NavigationCompleteEventArgs args)
		{
			NavigationLogEventArgs navigationLogEventArgs = new NavigationLogEventArgs();
			bool canNavigateBackward = CanNavigateBackward;
			bool canNavigateForward = CanNavigateForward;
			if (pendingNavigation != null)
			{
				int piOrder = 0;
				pendingNavigation.Location.NativeShellItem.Compare(args.NewLocation.NativeShellItem, SICHINTF.SICHINT_ALLFIELDS, out piOrder);
				if (piOrder != 0)
				{
					if (currentLocationIndex < _locations.Count - 1)
					{
						_locations.RemoveRange(currentLocationIndex + 1, _locations.Count - (currentLocationIndex + 1));
					}
					_locations.Add(args.NewLocation);
					currentLocationIndex = _locations.Count - 1;
					navigationLogEventArgs.LocationsChanged = true;
				}
				else
				{
					currentLocationIndex = pendingNavigation.Index;
					navigationLogEventArgs.LocationsChanged = false;
				}
				pendingNavigation = null;
			}
			else
			{
				if (currentLocationIndex < _locations.Count - 1)
				{
					_locations.RemoveRange(currentLocationIndex + 1, _locations.Count - (currentLocationIndex + 1));
				}
				_locations.Add(args.NewLocation);
				currentLocationIndex = _locations.Count - 1;
				navigationLogEventArgs.LocationsChanged = true;
			}
			navigationLogEventArgs.CanNavigateBackwardChanged = canNavigateBackward != CanNavigateBackward;
			navigationLogEventArgs.CanNavigateForwardChanged = canNavigateForward != CanNavigateForward;
			if (this.NavigationLogChanged != null)
			{
				this.NavigationLogChanged(this, navigationLogEventArgs);
			}
		}

		internal bool NavigateLog(NavigationLogDirection direction)
		{
			int num = 0;
			if (direction == NavigationLogDirection.Backward && CanNavigateBackward)
			{
				num = currentLocationIndex - 1;
			}
			else
			{
				if (direction != 0 || !CanNavigateForward)
				{
					return false;
				}
				num = currentLocationIndex + 1;
			}
			ShellObject shellObject = _locations[num];
			pendingNavigation = new PendingNavigation(shellObject, num);
			parent.Navigate(shellObject);
			return true;
		}

		internal bool NavigateLog(int index)
		{
			if (index >= _locations.Count || index < 0)
			{
				return false;
			}
			if (index == currentLocationIndex)
			{
				return false;
			}
			ShellObject shellObject = _locations[index];
			pendingNavigation = new PendingNavigation(shellObject, index);
			parent.Navigate(shellObject);
			return true;
		}
	}
}
