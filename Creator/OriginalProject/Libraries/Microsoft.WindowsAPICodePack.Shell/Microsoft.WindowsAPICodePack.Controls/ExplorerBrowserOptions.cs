using System;

namespace Microsoft.WindowsAPICodePack.Controls
{
	[Flags]
	internal enum ExplorerBrowserOptions
	{
		NavigateOnce = 1,
		ShowFrames = 2,
		AlwaysNavigate = 4,
		NoTravelLog = 8,
		NoWrapperWindow = 0x10,
		HtmlSharepointView = 0x20
	}
}
