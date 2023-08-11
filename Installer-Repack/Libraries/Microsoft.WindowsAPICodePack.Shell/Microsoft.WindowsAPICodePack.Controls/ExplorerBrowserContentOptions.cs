using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Controls.WindowsForms;
using Microsoft.WindowsAPICodePack.Shell.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Controls
{
	public class ExplorerBrowserContentOptions
	{
		private ExplorerBrowser eb;

		internal FolderSettings folderSettings = new FolderSettings();

		public ExplorerBrowserViewMode ViewMode
		{
			get
			{
				return (ExplorerBrowserViewMode)folderSettings.ViewMode;
			}
			set
			{
				folderSettings.ViewMode = (FolderViewMode)value;
				if (eb.explorerBrowserControl != null)
				{
					eb.explorerBrowserControl.SetFolderSettings(folderSettings);
				}
			}
		}

		public ExplorerBrowserContentSectionOptions Flags
		{
			get
			{
				return (ExplorerBrowserContentSectionOptions)folderSettings.Options;
			}
			set
			{
				folderSettings.Options = (FolderOptions)(value | (ExplorerBrowserContentSectionOptions)1073741824 | (ExplorerBrowserContentSectionOptions)65536);
				if (eb.explorerBrowserControl != null)
				{
					eb.explorerBrowserControl.SetFolderSettings(folderSettings);
				}
			}
		}

		public bool AlignLeft
		{
			get
			{
				return IsFlagSet(ExplorerBrowserContentSectionOptions.AlignLeft);
			}
			set
			{
				SetFlag(ExplorerBrowserContentSectionOptions.AlignLeft, value);
			}
		}

		public bool AutoArrange
		{
			get
			{
				return IsFlagSet(ExplorerBrowserContentSectionOptions.AutoArrange);
			}
			set
			{
				SetFlag(ExplorerBrowserContentSectionOptions.AutoArrange, value);
			}
		}

		public bool CheckSelect
		{
			get
			{
				return IsFlagSet(ExplorerBrowserContentSectionOptions.CheckSelect);
			}
			set
			{
				SetFlag(ExplorerBrowserContentSectionOptions.CheckSelect, value);
			}
		}

		public bool ExtendedTiles
		{
			get
			{
				return IsFlagSet(ExplorerBrowserContentSectionOptions.ExtendedTiles);
			}
			set
			{
				SetFlag(ExplorerBrowserContentSectionOptions.ExtendedTiles, value);
			}
		}

		public bool FullRowSelect
		{
			get
			{
				return IsFlagSet(ExplorerBrowserContentSectionOptions.FullRowSelect);
			}
			set
			{
				SetFlag(ExplorerBrowserContentSectionOptions.FullRowSelect, value);
			}
		}

		public bool HideFileNames
		{
			get
			{
				return IsFlagSet(ExplorerBrowserContentSectionOptions.HideFileNames);
			}
			set
			{
				SetFlag(ExplorerBrowserContentSectionOptions.HideFileNames, value);
			}
		}

		public bool NoBrowserViewState
		{
			get
			{
				return IsFlagSet(ExplorerBrowserContentSectionOptions.NoBrowserViewState);
			}
			set
			{
				SetFlag(ExplorerBrowserContentSectionOptions.NoBrowserViewState, value);
			}
		}

		public bool NoColumnHeader
		{
			get
			{
				return IsFlagSet(ExplorerBrowserContentSectionOptions.NoColumnHeader);
			}
			set
			{
				SetFlag(ExplorerBrowserContentSectionOptions.NoColumnHeader, value);
			}
		}

		public bool NoHeaderInAllViews
		{
			get
			{
				return IsFlagSet(ExplorerBrowserContentSectionOptions.NoHeaderInAllViews);
			}
			set
			{
				SetFlag(ExplorerBrowserContentSectionOptions.NoHeaderInAllViews, value);
			}
		}

		public bool NoIcons
		{
			get
			{
				return IsFlagSet(ExplorerBrowserContentSectionOptions.NoIcons);
			}
			set
			{
				SetFlag(ExplorerBrowserContentSectionOptions.NoIcons, value);
			}
		}

		public bool NoSubfolders
		{
			get
			{
				return IsFlagSet(ExplorerBrowserContentSectionOptions.NoSubfolders);
			}
			set
			{
				SetFlag(ExplorerBrowserContentSectionOptions.NoSubfolders, value);
			}
		}

		public bool SingleClickActivate
		{
			get
			{
				return IsFlagSet(ExplorerBrowserContentSectionOptions.SingleClickActivate);
			}
			set
			{
				SetFlag(ExplorerBrowserContentSectionOptions.SingleClickActivate, value);
			}
		}

		public bool SingleSelection
		{
			get
			{
				return IsFlagSet(ExplorerBrowserContentSectionOptions.SingleSelection);
			}
			set
			{
				SetFlag(ExplorerBrowserContentSectionOptions.SingleSelection, value);
			}
		}

		public int ThumbnailSize
		{
			get
			{
				int piImageSize = 0;
				IFolderView2 folderView = eb.GetFolderView2();
				if (folderView != null)
				{
					try
					{
						int puViewMode = 0;
						HResult viewModeAndIconSize = folderView.GetViewModeAndIconSize(out puViewMode, out piImageSize);
						if (viewModeAndIconSize != 0)
						{
							throw new CommonControlException(LocalizedMessages.ExplorerBrowserIconSize, viewModeAndIconSize);
						}
					}
					finally
					{
						Marshal.ReleaseComObject(folderView);
						folderView = null;
					}
				}
				return piImageSize;
			}
			set
			{
				IFolderView2 folderView = eb.GetFolderView2();
				if (folderView == null)
				{
					return;
				}
				try
				{
					int puViewMode = 0;
					int piImageSize = 0;
					HResult viewModeAndIconSize = folderView.GetViewModeAndIconSize(out puViewMode, out piImageSize);
					if (viewModeAndIconSize != 0)
					{
						throw new CommonControlException(LocalizedMessages.ExplorerBrowserIconSize, viewModeAndIconSize);
					}
					viewModeAndIconSize = folderView.SetViewModeAndIconSize(puViewMode, value);
					if (viewModeAndIconSize != 0)
					{
						throw new CommonControlException(LocalizedMessages.ExplorerBrowserIconSize, viewModeAndIconSize);
					}
				}
				finally
				{
					Marshal.ReleaseComObject(folderView);
					folderView = null;
				}
			}
		}

		internal ExplorerBrowserContentOptions(ExplorerBrowser eb)
		{
			this.eb = eb;
		}

		private bool IsFlagSet(ExplorerBrowserContentSectionOptions flag)
		{
			return ((uint)folderSettings.Options & (uint)flag) != 0;
		}

		private void SetFlag(ExplorerBrowserContentSectionOptions flag, bool value)
		{
			if (value)
			{
				folderSettings.Options |= (FolderOptions)flag;
			}
			else
			{
				folderSettings.Options = (FolderOptions)((int)folderSettings.Options & (int)(~flag));
			}
			if (eb.explorerBrowserControl != null)
			{
				eb.explorerBrowserControl.SetFolderSettings(folderSettings);
			}
		}
	}
}
