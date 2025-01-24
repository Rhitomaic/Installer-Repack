using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Controls.WindowsForms
{
	public sealed class ExplorerBrowser : UserControl, IServiceProvider, IExplorerPaneVisibility, IExplorerBrowserEvents, ICommDlgBrowser3, IMessageFilter
	{
		private IShellItemArray shellItemsArray;

		private ShellObjectCollection itemsCollection;

		private IShellItemArray selectedShellItemsArray;

		private ShellObjectCollection selectedItemsCollection;

		internal ExplorerBrowserClass explorerBrowserControl;

		internal uint eventsCookie;

		private string propertyBagName = typeof(ExplorerBrowser).FullName;

		private ShellObject antecreationNavigationTarget;

		private ExplorerBrowserViewEvents viewEvents;

		public ExplorerBrowserNavigationOptions NavigationOptions { get; private set; }

		public ExplorerBrowserContentOptions ContentOptions { get; private set; }

		public ShellObjectCollection Items
		{
			get
			{
				if (shellItemsArray != null)
				{
					Marshal.ReleaseComObject(shellItemsArray);
				}
				if (itemsCollection != null)
				{
					itemsCollection.Dispose();
					itemsCollection = null;
				}
				shellItemsArray = GetItemsArray();
				itemsCollection = new ShellObjectCollection(shellItemsArray, true);
				return itemsCollection;
			}
		}

		public ShellObjectCollection SelectedItems
		{
			get
			{
				if (selectedShellItemsArray != null)
				{
					Marshal.ReleaseComObject(selectedShellItemsArray);
				}
				if (selectedItemsCollection != null)
				{
					selectedItemsCollection.Dispose();
					selectedItemsCollection = null;
				}
				selectedShellItemsArray = GetSelectedItemsArray();
				selectedItemsCollection = new ShellObjectCollection(selectedShellItemsArray, true);
				return selectedItemsCollection;
			}
		}

		public ExplorerBrowserNavigationLog NavigationLog { get; private set; }

		public string PropertyBagName
		{
			get
			{
				return propertyBagName;
			}
			set
			{
				propertyBagName = value;
				if (explorerBrowserControl != null)
				{
					explorerBrowserControl.SetPropertyBag(propertyBagName);
				}
			}
		}

		public event EventHandler SelectionChanged;

		public event EventHandler ItemsChanged;

		public event EventHandler<NavigationPendingEventArgs> NavigationPending;

		public event EventHandler<NavigationCompleteEventArgs> NavigationComplete;

		public event EventHandler<NavigationFailedEventArgs> NavigationFailed;

		public event EventHandler ViewEnumerationComplete;

		public event EventHandler ViewSelectedItemChanged;

		public void Navigate(ShellObject shellObject)
		{
			if (shellObject == null)
			{
				throw new ArgumentNullException("shellObject");
			}
			if (explorerBrowserControl == null)
			{
				antecreationNavigationTarget = shellObject;
				return;
			}
			HResult hResult = explorerBrowserControl.BrowseToObject(shellObject.NativeShellItem, 0u);
			switch (hResult)
			{
			case HResult.ResourceInUse:
			case HResult.Canceled:
				if (this.NavigationFailed != null)
				{
					NavigationFailedEventArgs navigationFailedEventArgs = new NavigationFailedEventArgs();
					navigationFailedEventArgs.FailedLocation = shellObject;
					this.NavigationFailed(this, navigationFailedEventArgs);
					return;
				}
				break;
			case HResult.Ok:
				return;
			}
			throw new CommonControlException(LocalizedMessages.ExplorerBrowserBrowseToObjectFailed, hResult);
		}

		public bool NavigateLogLocation(NavigationLogDirection direction)
		{
			return NavigationLog.NavigateLog(direction);
		}

		public bool NavigateLogLocation(int navigationLogIndex)
		{
			return NavigationLog.NavigateLog(navigationLogIndex);
		}

		public ExplorerBrowser()
		{
			NavigationOptions = new ExplorerBrowserNavigationOptions(this);
			ContentOptions = new ExplorerBrowserContentOptions(this);
			NavigationLog = new ExplorerBrowserNavigationLog(this);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (base.DesignMode && e != null)
			{
				using (LinearGradientBrush brush = new LinearGradientBrush(base.ClientRectangle, Color.Aqua, Color.CadetBlue, LinearGradientMode.ForwardDiagonal))
				{
					e.Graphics.FillRectangle(brush, base.ClientRectangle);
				}
				using (Font font = new Font("Garamond", 30f))
				{
					using (StringFormat stringFormat = new StringFormat())
					{
						stringFormat.Alignment = StringAlignment.Center;
						stringFormat.LineAlignment = StringAlignment.Center;
						e.Graphics.DrawString("ExplorerBrowserControl", font, Brushes.White, base.ClientRectangle, stringFormat);
					}
				}
			}
			base.OnPaint(e);
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			if (!base.DesignMode)
			{
				explorerBrowserControl = new ExplorerBrowserClass();
				ExplorerBrowserNativeMethods.IUnknown_SetSite(explorerBrowserControl, this);
				explorerBrowserControl.Advise(Marshal.GetComInterfaceForObject(this, typeof(IExplorerBrowserEvents)), out eventsCookie);
				viewEvents = new ExplorerBrowserViewEvents(this);
				NativeRect prc = default(NativeRect);
				prc.Top = base.ClientRectangle.Top;
				prc.Left = base.ClientRectangle.Left;
				prc.Right = base.ClientRectangle.Right;
				prc.Bottom = base.ClientRectangle.Bottom;
				explorerBrowserControl.Initialize(base.Handle, ref prc, null);
				explorerBrowserControl.SetOptions(ExplorerBrowserOptions.ShowFrames);
				explorerBrowserControl.SetPropertyBag(propertyBagName);
				if (antecreationNavigationTarget != null)
				{
					BeginInvoke((MethodInvoker)delegate
					{
						Navigate(antecreationNavigationTarget);
						antecreationNavigationTarget = null;
					});
				}
			}
			Application.AddMessageFilter(this);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			if (explorerBrowserControl != null)
			{
				NativeRect rcBrowser = default(NativeRect);
				rcBrowser.Top = base.ClientRectangle.Top;
				rcBrowser.Left = base.ClientRectangle.Left;
				rcBrowser.Right = base.ClientRectangle.Right;
				rcBrowser.Bottom = base.ClientRectangle.Bottom;
				IntPtr phdwp = IntPtr.Zero;
				explorerBrowserControl.SetRect(ref phdwp, rcBrowser);
			}
			base.OnSizeChanged(e);
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			if (explorerBrowserControl != null)
			{
				viewEvents.DisconnectFromView();
				explorerBrowserControl.Unadvise(eventsCookie);
				ExplorerBrowserNativeMethods.IUnknown_SetSite(explorerBrowserControl, null);
				explorerBrowserControl.Destroy();
				Marshal.ReleaseComObject(explorerBrowserControl);
				explorerBrowserControl = null;
			}
			base.OnHandleDestroyed(e);
		}

		HResult IServiceProvider.QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject)
		{
			if (guidService.CompareTo(new Guid("e07010ec-bc17-44c0-97b0-46c7c95b9edc")) == 0)
			{
				ppvObject = Marshal.GetComInterfaceForObject(this, typeof(IExplorerPaneVisibility));
				return HResult.Ok;
			}
			if (guidService.CompareTo(new Guid("000214F1-0000-0000-C000-000000000046")) == 0)
			{
				if (riid.CompareTo(new Guid("000214F1-0000-0000-C000-000000000046")) == 0)
				{
					ppvObject = Marshal.GetComInterfaceForObject(this, typeof(ICommDlgBrowser3));
					return HResult.Ok;
				}
				if (riid.CompareTo(new Guid("c8ad25a1-3294-41ee-8165-71174bd01c57")) == 0)
				{
					ppvObject = Marshal.GetComInterfaceForObject(this, typeof(ICommDlgBrowser3));
					return HResult.Ok;
				}
				ppvObject = IntPtr.Zero;
				return HResult.NoInterface;
			}
			IntPtr zero = IntPtr.Zero;
			ppvObject = zero;
			return HResult.NoInterface;
		}

		HResult IExplorerPaneVisibility.GetPaneState(ref Guid explorerPane, out ExplorerPaneState peps)
		{
			switch (explorerPane.ToString())
			{
			case "b4e9db8b-34ba-4c39-b5cc-16a1bd2c411c":
				peps = VisibilityToPaneState(NavigationOptions.PaneVisibility.AdvancedQuery);
				break;
			case "d9745868-ca5f-4a76-91cd-f5a129fbb076":
				peps = VisibilityToPaneState(NavigationOptions.PaneVisibility.Commands);
				break;
			case "72e81700-e3ec-4660-bf24-3c3b7b648806":
				peps = VisibilityToPaneState(NavigationOptions.PaneVisibility.CommandsOrganize);
				break;
			case "21f7c32d-eeaa-439b-bb51-37b96fd6a943":
				peps = VisibilityToPaneState(NavigationOptions.PaneVisibility.CommandsView);
				break;
			case "43abf98b-89b8-472d-b9ce-e69b8229f019":
				peps = VisibilityToPaneState(NavigationOptions.PaneVisibility.Details);
				break;
			case "cb316b22-25f7-42b8-8a09-540d23a43c2f":
				peps = VisibilityToPaneState(NavigationOptions.PaneVisibility.Navigation);
				break;
			case "893c63d1-45c8-4d17-be19-223be71be365":
				peps = VisibilityToPaneState(NavigationOptions.PaneVisibility.Preview);
				break;
			case "65bcde4f-4f07-4f27-83a7-1afca4df7ddd":
				peps = VisibilityToPaneState(NavigationOptions.PaneVisibility.Query);
				break;
			default:
				peps = VisibilityToPaneState(PaneVisibilityState.Show);
				break;
			}
			return HResult.Ok;
		}

		private static ExplorerPaneState VisibilityToPaneState(PaneVisibilityState visibility)
		{
			switch (visibility)
			{
			case PaneVisibilityState.DoNotCare:
				return ExplorerPaneState.DoNotCare;
			case PaneVisibilityState.Hide:
				return (ExplorerPaneState)131074;
			case PaneVisibilityState.Show:
				return (ExplorerPaneState)131073;
			default:
				throw new ArgumentException("unexpected PaneVisibilityState");
			}
		}

		HResult IExplorerBrowserEvents.OnNavigationPending(IntPtr pidlFolder)
		{
			bool flag = false;
			if (this.NavigationPending != null)
			{
				NavigationPendingEventArgs navigationPendingEventArgs = new NavigationPendingEventArgs();
				navigationPendingEventArgs.PendingLocation = ShellObjectFactory.Create(pidlFolder);
				if (navigationPendingEventArgs.PendingLocation != null)
				{
					Delegate[] invocationList = this.NavigationPending.GetInvocationList();
					foreach (Delegate @delegate in invocationList)
					{
						@delegate.DynamicInvoke(this, navigationPendingEventArgs);
						if (navigationPendingEventArgs.Cancel)
						{
							flag = true;
						}
					}
				}
			}
			return flag ? HResult.Canceled : HResult.Ok;
		}

		HResult IExplorerBrowserEvents.OnViewCreated(object psv)
		{
			viewEvents.ConnectToView((IShellView)psv);
			return HResult.Ok;
		}

		HResult IExplorerBrowserEvents.OnNavigationComplete(IntPtr pidlFolder)
		{
			ContentOptions.folderSettings.ViewMode = GetCurrentViewMode();
			if (this.NavigationComplete != null)
			{
				NavigationCompleteEventArgs navigationCompleteEventArgs = new NavigationCompleteEventArgs();
				navigationCompleteEventArgs.NewLocation = ShellObjectFactory.Create(pidlFolder);
				this.NavigationComplete(this, navigationCompleteEventArgs);
			}
			return HResult.Ok;
		}

		HResult IExplorerBrowserEvents.OnNavigationFailed(IntPtr pidlFolder)
		{
			if (this.NavigationFailed != null)
			{
				NavigationFailedEventArgs navigationFailedEventArgs = new NavigationFailedEventArgs();
				navigationFailedEventArgs.FailedLocation = ShellObjectFactory.Create(pidlFolder);
				this.NavigationFailed(this, navigationFailedEventArgs);
			}
			return HResult.Ok;
		}

		HResult ICommDlgBrowser3.OnDefaultCommand(IntPtr ppshv)
		{
			return HResult.False;
		}

		HResult ICommDlgBrowser3.OnStateChange(IntPtr ppshv, CommDlgBrowserStateChange uChange)
		{
			if (uChange == CommDlgBrowserStateChange.SelectionChange)
			{
				FireSelectionChanged();
			}
			return HResult.Ok;
		}

		HResult ICommDlgBrowser3.IncludeObject(IntPtr ppshv, IntPtr pidl)
		{
			FireContentChanged();
			return HResult.Ok;
		}

		HResult ICommDlgBrowser3.GetDefaultMenuText(IShellView shellView, IntPtr text, int cchMax)
		{
			return HResult.False;
		}

		HResult ICommDlgBrowser3.GetViewFlags(out uint pdwFlags)
		{
			pdwFlags = 1u;
			return HResult.Ok;
		}

		HResult ICommDlgBrowser3.Notify(IntPtr pshv, CommDlgBrowserNotifyType notifyType)
		{
			return HResult.Ok;
		}

		HResult ICommDlgBrowser3.GetCurrentFilter(StringBuilder pszFileSpec, int cchFileSpec)
		{
			return HResult.Ok;
		}

		HResult ICommDlgBrowser3.OnColumnClicked(IShellView ppshv, int iColumn)
		{
			return HResult.Ok;
		}

		HResult ICommDlgBrowser3.OnPreViewCreated(IShellView ppshv)
		{
			return HResult.Ok;
		}

		bool IMessageFilter.PreFilterMessage(ref Message m)
		{
			HResult hResult = HResult.False;
			if (explorerBrowserControl != null)
			{
				hResult = ((IInputObject)explorerBrowserControl).TranslateAcceleratorIO(ref m);
			}
			return hResult == HResult.Ok;
		}

		internal FolderViewMode GetCurrentViewMode()
		{
			IFolderView2 folderView = GetFolderView2();
			uint pViewMode = 0u;
			if (folderView != null)
			{
				try
				{
					HResult currentViewMode = folderView.GetCurrentViewMode(out pViewMode);
					if (currentViewMode != 0)
					{
						throw new ShellException(currentViewMode);
					}
				}
				finally
				{
					Marshal.ReleaseComObject(folderView);
					folderView = null;
				}
			}
			return (FolderViewMode)pViewMode;
		}

		internal IFolderView2 GetFolderView2()
		{
			Guid riid = new Guid("1af3a467-214f-4298-908e-06b03e0b39f9");
			IntPtr ppv = IntPtr.Zero;
			if (explorerBrowserControl != null)
			{
				HResult currentView = explorerBrowserControl.GetCurrentView(ref riid, out ppv);
				switch (currentView)
				{
				case HResult.NoInterface:
				case HResult.Fail:
					return null;
				default:
					throw new CommonControlException(LocalizedMessages.ExplorerBrowserFailedToGetView, currentView);
				case HResult.Ok:
					return (IFolderView2)Marshal.GetObjectForIUnknown(ppv);
				}
			}
			return null;
		}

		internal IShellItemArray GetSelectedItemsArray()
		{
			IShellItemArray result = null;
			IFolderView2 folderView = GetFolderView2();
			if (folderView != null)
			{
				try
				{
					Guid riid = new Guid("B63EA76D-1F85-456F-A19C-48159EFA858B");
					object ppv = null;
					HResult hResult = folderView.Items(1u, ref riid, out ppv);
					result = ppv as IShellItemArray;
					if (hResult != 0 && hResult != HResult.ElementNotFound && hResult != HResult.Fail)
					{
						throw new CommonControlException(LocalizedMessages.ExplorerBrowserUnexpectedError, hResult);
					}
				}
				finally
				{
					Marshal.ReleaseComObject(folderView);
					folderView = null;
				}
			}
			return result;
		}

		internal int GetItemsCount()
		{
			int pcItems = 0;
			IFolderView2 folderView = GetFolderView2();
			if (folderView != null)
			{
				try
				{
					HResult hResult = folderView.ItemCount(2u, out pcItems);
					if (hResult != 0 && hResult != HResult.ElementNotFound && hResult != HResult.Fail)
					{
						throw new CommonControlException(LocalizedMessages.ExplorerBrowserItemCount, hResult);
					}
				}
				finally
				{
					Marshal.ReleaseComObject(folderView);
					folderView = null;
				}
			}
			return pcItems;
		}

		internal int GetSelectedItemsCount()
		{
			int pcItems = 0;
			IFolderView2 folderView = GetFolderView2();
			if (folderView != null)
			{
				try
				{
					HResult hResult = folderView.ItemCount(1u, out pcItems);
					if (hResult != 0 && hResult != HResult.ElementNotFound && hResult != HResult.Fail)
					{
						throw new CommonControlException(LocalizedMessages.ExplorerBrowserSelectedItemCount, hResult);
					}
				}
				finally
				{
					Marshal.ReleaseComObject(folderView);
					folderView = null;
				}
			}
			return pcItems;
		}

		internal IShellItemArray GetItemsArray()
		{
			IShellItemArray result = null;
			IFolderView2 folderView = GetFolderView2();
			if (folderView != null)
			{
				try
				{
					Guid riid = new Guid("B63EA76D-1F85-456F-A19C-48159EFA858B");
					object ppv = null;
					HResult hResult = folderView.Items(2u, ref riid, out ppv);
					if (hResult != 0 && hResult != HResult.Fail && hResult != HResult.ElementNotFound && hResult != HResult.InvalidArguments)
					{
						throw new CommonControlException(LocalizedMessages.ExplorerBrowserViewItems, hResult);
					}
					result = ppv as IShellItemArray;
				}
				finally
				{
					Marshal.ReleaseComObject(folderView);
					folderView = null;
				}
			}
			return result;
		}

		internal void FireSelectionChanged()
		{
			if (this.SelectionChanged != null)
			{
				this.SelectionChanged(this, EventArgs.Empty);
			}
		}

		internal void FireContentChanged()
		{
			if (this.ItemsChanged != null)
			{
				this.ItemsChanged(this, EventArgs.Empty);
			}
		}

		internal void FireContentEnumerationComplete()
		{
			if (this.ViewEnumerationComplete != null)
			{
				this.ViewEnumerationComplete(this, EventArgs.Empty);
			}
		}

		internal void FireSelectedItemChanged()
		{
			if (this.ViewSelectedItemChanged != null)
			{
				this.ViewSelectedItemChanged(this, EventArgs.Empty);
			}
		}
	}
}
