using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	public sealed class ShellSearchConnector : ShellSearchCollection
	{
		public new static bool IsPlatformSupported => CoreHelpers.RunningOnWin7;

		internal ShellSearchConnector()
		{
			CoreHelpers.ThrowIfNotWin7();
		}

		internal ShellSearchConnector(IShellItem2 shellItem)
			: this()
		{
			nativeShellItem = shellItem;
		}
	}
}
