using Microsoft.WindowsAPICodePack.Shell;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	public abstract class JumpListTask
	{
		internal abstract IShellLinkW NativeShellLink { get; }
	}
}
