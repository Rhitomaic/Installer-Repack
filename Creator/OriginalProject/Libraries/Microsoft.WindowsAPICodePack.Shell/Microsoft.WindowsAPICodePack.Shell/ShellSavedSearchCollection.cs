using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	public class ShellSavedSearchCollection : ShellSearchCollection
	{
		internal ShellSavedSearchCollection(IShellItem2 shellItem)
			: base(shellItem)
		{
			CoreHelpers.ThrowIfNotVista();
		}
	}
}
