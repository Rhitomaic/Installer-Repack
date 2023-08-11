using System;

namespace Microsoft.WindowsAPICodePack.Shell
{
	public class AeroGlassCompositionChangedEventArgs : EventArgs
	{
		public bool GlassAvailable { get; private set; }

		internal AeroGlassCompositionChangedEventArgs(bool avialbility)
		{
			GlassAvailable = avialbility;
		}
	}
}
