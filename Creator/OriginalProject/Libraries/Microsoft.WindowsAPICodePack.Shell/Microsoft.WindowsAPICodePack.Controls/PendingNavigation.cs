using Microsoft.WindowsAPICodePack.Shell;

namespace Microsoft.WindowsAPICodePack.Controls
{
	internal class PendingNavigation
	{
		internal ShellObject Location { get; set; }

		internal int Index { get; set; }

		internal PendingNavigation(ShellObject location, int index)
		{
			Location = location;
			Index = index;
		}
	}
}
