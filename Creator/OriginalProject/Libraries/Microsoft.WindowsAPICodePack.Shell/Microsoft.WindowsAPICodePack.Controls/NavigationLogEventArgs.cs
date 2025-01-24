using System;

namespace Microsoft.WindowsAPICodePack.Controls
{
	public class NavigationLogEventArgs : EventArgs
	{
		public bool CanNavigateForwardChanged { get; set; }

		public bool CanNavigateBackwardChanged { get; set; }

		public bool LocationsChanged { get; set; }
	}
}
