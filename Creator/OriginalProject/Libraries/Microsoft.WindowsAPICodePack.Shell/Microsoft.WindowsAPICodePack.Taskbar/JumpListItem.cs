using Microsoft.WindowsAPICodePack.Shell;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	public class JumpListItem : ShellFile, IJumpListItem
	{
		public new string Path
		{
			get
			{
				return base.Path;
			}
			set
			{
				base.ParsingName = value;
			}
		}

		public JumpListItem(string path)
			: base(path)
		{
		}
	}
}
