using Microsoft.WindowsAPICodePack.Controls.WindowsForms;

namespace Microsoft.WindowsAPICodePack.Controls
{
	public class ExplorerBrowserNavigationOptions
	{
		private ExplorerBrowser eb;

		public ExplorerBrowserNavigateOptions Flags
		{
			get
			{
				ExplorerBrowserOptions pdwFlag = (ExplorerBrowserOptions)0;
				if (eb.explorerBrowserControl != null)
				{
					eb.explorerBrowserControl.GetOptions(out pdwFlag);
					return (ExplorerBrowserNavigateOptions)pdwFlag;
				}
				return (ExplorerBrowserNavigateOptions)pdwFlag;
			}
			set
			{
				if (eb.explorerBrowserControl != null)
				{
					eb.explorerBrowserControl.SetOptions((ExplorerBrowserOptions)(value | (ExplorerBrowserNavigateOptions)2));
				}
			}
		}

		public bool NavigateOnce
		{
			get
			{
				return IsFlagSet(ExplorerBrowserNavigateOptions.NavigateOnce);
			}
			set
			{
				SetFlag(ExplorerBrowserNavigateOptions.NavigateOnce, value);
			}
		}

		public bool AlwaysNavigate
		{
			get
			{
				return IsFlagSet(ExplorerBrowserNavigateOptions.AlwaysNavigate);
			}
			set
			{
				SetFlag(ExplorerBrowserNavigateOptions.AlwaysNavigate, value);
			}
		}

		public ExplorerBrowserPaneVisibility PaneVisibility { get; private set; }

		internal ExplorerBrowserNavigationOptions(ExplorerBrowser eb)
		{
			this.eb = eb;
			PaneVisibility = new ExplorerBrowserPaneVisibility();
		}

		private bool IsFlagSet(ExplorerBrowserNavigateOptions flag)
		{
			return (Flags & flag) != 0;
		}

		private void SetFlag(ExplorerBrowserNavigateOptions flag, bool value)
		{
			if (value)
			{
				Flags |= flag;
			}
			else
			{
				Flags &= ~flag;
			}
		}
	}
}
