using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Markup;
using System.Windows.Threading;
using Microsoft.WindowsAPICodePack.Controls.WindowsForms;
using Microsoft.WindowsAPICodePack.Shell;

namespace Microsoft.WindowsAPICodePack.Controls.WindowsPresentationFoundation
{
	public partial class ExplorerBrowser : UserControl, IDisposable, IComponentConnector
	{
		private ObservableCollection<ShellObject> selectedItems;

		private ObservableCollection<ShellObject> items;

		private ObservableCollection<ShellObject> navigationLog;

		private DispatcherTimer dtCLRUpdater = new DispatcherTimer();

		private ShellObject initialNavigationTarget;

		private ExplorerBrowserViewMode? initialViewMode;

		private AutoResetEvent itemsChanged = new AutoResetEvent(false);

		private AutoResetEvent selectionChanged = new AutoResetEvent(false);

		private int selectionChangeWaitCount;

		private static readonly DependencyPropertyKey ItemsPropertyKey = DependencyProperty.RegisterReadOnly("Items", typeof(ObservableCollection<ShellObject>), typeof(ExplorerBrowser), new PropertyMetadata(null));

		public static readonly DependencyProperty ItemsProperty = ItemsPropertyKey.DependencyProperty;

		private static readonly DependencyPropertyKey SelectedItemsPropertyKey = DependencyProperty.RegisterReadOnly("SelectedItems", typeof(ObservableCollection<ShellObject>), typeof(ExplorerBrowser), new PropertyMetadata(null));

		private static readonly DependencyPropertyKey NavigationLogPropertyKey = DependencyProperty.RegisterReadOnly("NavigationLog", typeof(ObservableCollection<ShellObject>), typeof(ExplorerBrowser), new PropertyMetadata(null));

		public static readonly DependencyProperty NavigationLogProperty = NavigationLogPropertyKey.DependencyProperty;

		public static readonly DependencyProperty SelectedItemsProperty = SelectedItemsPropertyKey.DependencyProperty;

		public static readonly DependencyProperty NavigationTargetProperty = DependencyProperty.Register("NavigationTarget", typeof(ShellObject), typeof(ExplorerBrowser), new PropertyMetadata(null, navigationTargetChanged));

		internal static DependencyProperty AlignLeftProperty = DependencyProperty.Register("AlignLeft", typeof(bool), typeof(ExplorerBrowser), new PropertyMetadata(false, OnAlignLeftChanged));

		internal static DependencyProperty AutoArrangeProperty = DependencyProperty.Register("AutoArrange", typeof(bool), typeof(ExplorerBrowser), new PropertyMetadata(false, OnAutoArrangeChanged));

		internal static DependencyProperty CheckSelectProperty = DependencyProperty.Register("CheckSelect", typeof(bool), typeof(ExplorerBrowser), new PropertyMetadata(false, OnCheckSelectChanged));

		internal static DependencyProperty ExtendedTilesProperty = DependencyProperty.Register("ExtendedTiles", typeof(bool), typeof(ExplorerBrowser), new PropertyMetadata(false, OnExtendedTilesChanged));

		internal static DependencyProperty FullRowSelectProperty = DependencyProperty.Register("FullRowSelect", typeof(bool), typeof(ExplorerBrowser), new PropertyMetadata(false, OnFullRowSelectChanged));

		internal static DependencyProperty HideFileNamesProperty = DependencyProperty.Register("HideFileNames", typeof(bool), typeof(ExplorerBrowser), new PropertyMetadata(false, OnHideFileNamesChanged));

		internal static DependencyProperty NoBrowserViewStateProperty = DependencyProperty.Register("NoBrowserViewState", typeof(bool), typeof(ExplorerBrowser), new PropertyMetadata(false, OnNoBrowserViewStateChanged));

		internal static DependencyProperty NoColumnHeaderProperty = DependencyProperty.Register("NoColumnHeader", typeof(bool), typeof(ExplorerBrowser), new PropertyMetadata(false, OnNoColumnHeaderChanged));

		internal static DependencyProperty NoHeaderInAllViewsProperty = DependencyProperty.Register("NoHeaderInAllViews", typeof(bool), typeof(ExplorerBrowser), new PropertyMetadata(false, OnNoHeaderInAllViewsChanged));

		internal static DependencyProperty NoIconsProperty = DependencyProperty.Register("NoIcons", typeof(bool), typeof(ExplorerBrowser), new PropertyMetadata(false, OnNoIconsChanged));

		internal static DependencyProperty NoSubfoldersProperty = DependencyProperty.Register("NoSubfolders", typeof(bool), typeof(ExplorerBrowser), new PropertyMetadata(false, OnNoSubfoldersChanged));

		internal static DependencyProperty SingleClickActivateProperty = DependencyProperty.Register("SingleClickActivate", typeof(bool), typeof(ExplorerBrowser), new PropertyMetadata(false, OnSingleClickActivateChanged));

		internal static DependencyProperty SingleSelectionProperty = DependencyProperty.Register("SingleSelection", typeof(bool), typeof(ExplorerBrowser), new PropertyMetadata(false, OnSingleSelectionChanged));

		internal static DependencyProperty ThumbnailSizeProperty = DependencyProperty.Register("ThumbnailSize", typeof(int), typeof(ExplorerBrowser), new PropertyMetadata(32, OnThumbnailSizeChanged));

		internal static DependencyProperty ViewModeProperty = DependencyProperty.Register("ViewMode", typeof(ExplorerBrowserViewMode), typeof(ExplorerBrowser), new PropertyMetadata(ExplorerBrowserViewMode.Auto, OnViewModeChanged));

		internal static DependencyProperty AlwaysNavigateProperty = DependencyProperty.Register("AlwaysNavigate", typeof(bool), typeof(ExplorerBrowser), new PropertyMetadata(false, OnAlwaysNavigateChanged));

		internal static DependencyProperty NavigateOnceProperty = DependencyProperty.Register("NavigateOnce", typeof(bool), typeof(ExplorerBrowser), new PropertyMetadata(false, OnNavigateOnceChanged));

		internal static DependencyProperty AdvancedQueryPaneProperty = DependencyProperty.Register("AdvancedQueryPane", typeof(PaneVisibilityState), typeof(ExplorerBrowser), new PropertyMetadata(PaneVisibilityState.DoNotCare, OnAdvancedQueryPaneChanged));

		internal static DependencyProperty CommandsPaneProperty = DependencyProperty.Register("CommandsPane", typeof(PaneVisibilityState), typeof(ExplorerBrowser), new PropertyMetadata(PaneVisibilityState.DoNotCare, OnCommandsPaneChanged));

		internal static DependencyProperty CommandsOrganizePaneProperty = DependencyProperty.Register("CommandsOrganizePane", typeof(PaneVisibilityState), typeof(ExplorerBrowser), new PropertyMetadata(PaneVisibilityState.DoNotCare, OnCommandsOrganizePaneChanged));

		internal static DependencyProperty CommandsViewPaneProperty = DependencyProperty.Register("CommandsViewPane", typeof(PaneVisibilityState), typeof(ExplorerBrowser), new PropertyMetadata(PaneVisibilityState.DoNotCare, OnCommandsViewPaneChanged));

		internal static DependencyProperty DetailsPaneProperty = DependencyProperty.Register("DetailsPane", typeof(PaneVisibilityState), typeof(ExplorerBrowser), new PropertyMetadata(PaneVisibilityState.DoNotCare, OnDetailsPaneChanged));

		internal static DependencyProperty NavigationPaneProperty = DependencyProperty.Register("NavigationPane", typeof(PaneVisibilityState), typeof(ExplorerBrowser), new PropertyMetadata(PaneVisibilityState.DoNotCare, OnNavigationPaneChanged));

		internal static DependencyProperty PreviewPaneProperty = DependencyProperty.Register("PreviewPane", typeof(PaneVisibilityState), typeof(ExplorerBrowser), new PropertyMetadata(PaneVisibilityState.DoNotCare, OnPreviewPaneChanged));

		internal static DependencyProperty QueryPaneProperty = DependencyProperty.Register("QueryPane", typeof(PaneVisibilityState), typeof(ExplorerBrowser), new PropertyMetadata(PaneVisibilityState.DoNotCare, OnQueryPaneChanged));

		internal static DependencyProperty NavigationLogIndexProperty = DependencyProperty.Register("NavigationLogIndex", typeof(int), typeof(ExplorerBrowser), new PropertyMetadata(0, OnNavigationLogIndexChanged));

		public Microsoft.WindowsAPICodePack.Controls.WindowsForms.ExplorerBrowser ExplorerBrowserControl { get; set; }

		public ObservableCollection<ShellObject> Items
		{
			get
			{
				return (ObservableCollection<ShellObject>)GetValue(ItemsProperty);
			}
			set
			{
				SetValue(ItemsPropertyKey, value);
			}
		}

		public ObservableCollection<ShellObject> SelectedItems
		{
			get
			{
				return (ObservableCollection<ShellObject>)GetValue(SelectedItemsProperty);
			}
			internal set
			{
				SetValue(SelectedItemsPropertyKey, value);
			}
		}

		public ObservableCollection<ShellObject> NavigationLog
		{
			get
			{
				return (ObservableCollection<ShellObject>)GetValue(NavigationLogProperty);
			}
			internal set
			{
				SetValue(NavigationLogPropertyKey, value);
			}
		}

		public ShellObject NavigationTarget
		{
			get
			{
				return (ShellObject)GetValue(NavigationTargetProperty);
			}
			set
			{
				SetValue(NavigationTargetProperty, value);
			}
		}

		public bool AlignLeft
		{
			get
			{
				return (bool)GetValue(AlignLeftProperty);
			}
			set
			{
				SetValue(AlignLeftProperty, value);
			}
		}

		public bool AutoArrange
		{
			get
			{
				return (bool)GetValue(AutoArrangeProperty);
			}
			set
			{
				SetValue(AutoArrangeProperty, value);
			}
		}

		public bool CheckSelect
		{
			get
			{
				return (bool)GetValue(CheckSelectProperty);
			}
			set
			{
				SetValue(CheckSelectProperty, value);
			}
		}

		public bool ExtendedTiles
		{
			get
			{
				return (bool)GetValue(ExtendedTilesProperty);
			}
			set
			{
				SetValue(ExtendedTilesProperty, value);
			}
		}

		public bool FullRowSelect
		{
			get
			{
				return (bool)GetValue(FullRowSelectProperty);
			}
			set
			{
				SetValue(FullRowSelectProperty, value);
			}
		}

		public bool HideFileNames
		{
			get
			{
				return (bool)GetValue(HideFileNamesProperty);
			}
			set
			{
				SetValue(HideFileNamesProperty, value);
			}
		}

		public bool NoBrowserViewState
		{
			get
			{
				return (bool)GetValue(NoBrowserViewStateProperty);
			}
			set
			{
				SetValue(NoBrowserViewStateProperty, value);
			}
		}

		public bool NoColumnHeader
		{
			get
			{
				return (bool)GetValue(NoColumnHeaderProperty);
			}
			set
			{
				SetValue(NoColumnHeaderProperty, value);
			}
		}

		public bool NoHeaderInAllViews
		{
			get
			{
				return (bool)GetValue(NoHeaderInAllViewsProperty);
			}
			set
			{
				SetValue(NoHeaderInAllViewsProperty, value);
			}
		}

		public bool NoIcons
		{
			get
			{
				return (bool)GetValue(NoIconsProperty);
			}
			set
			{
				SetValue(NoIconsProperty, value);
			}
		}

		public bool NoSubfolders
		{
			get
			{
				return (bool)GetValue(NoSubfoldersProperty);
			}
			set
			{
				SetValue(NoSubfoldersProperty, value);
			}
		}

		public bool SingleClickActivate
		{
			get
			{
				return (bool)GetValue(SingleClickActivateProperty);
			}
			set
			{
				SetValue(SingleClickActivateProperty, value);
			}
		}

		public bool SingleSelection
		{
			get
			{
				return (bool)GetValue(SingleSelectionProperty);
			}
			set
			{
				SetValue(SingleSelectionProperty, value);
			}
		}

		public int ThumbnailSize
		{
			get
			{
				return (int)GetValue(ThumbnailSizeProperty);
			}
			set
			{
				SetValue(ThumbnailSizeProperty, value);
			}
		}

		public ExplorerBrowserViewMode ViewMode
		{
			get
			{
				return (ExplorerBrowserViewMode)GetValue(ViewModeProperty);
			}
			set
			{
				SetValue(ViewModeProperty, value);
			}
		}

		public bool AlwaysNavigate
		{
			get
			{
				return (bool)GetValue(AlwaysNavigateProperty);
			}
			set
			{
				SetValue(AlwaysNavigateProperty, value);
			}
		}

		public bool NavigateOnce
		{
			get
			{
				return (bool)GetValue(NavigateOnceProperty);
			}
			set
			{
				SetValue(NavigateOnceProperty, value);
			}
		}

		public PaneVisibilityState AdvancedQueryPane
		{
			get
			{
				return (PaneVisibilityState)GetValue(AdvancedQueryPaneProperty);
			}
			set
			{
				SetValue(AdvancedQueryPaneProperty, value);
			}
		}

		public PaneVisibilityState CommandsPane
		{
			get
			{
				return (PaneVisibilityState)GetValue(CommandsPaneProperty);
			}
			set
			{
				SetValue(CommandsPaneProperty, value);
			}
		}

		public PaneVisibilityState CommandsOrganizePane
		{
			get
			{
				return (PaneVisibilityState)GetValue(CommandsOrganizePaneProperty);
			}
			set
			{
				SetValue(CommandsOrganizePaneProperty, value);
			}
		}

		public PaneVisibilityState CommandsViewPane
		{
			get
			{
				return (PaneVisibilityState)GetValue(CommandsViewPaneProperty);
			}
			set
			{
				SetValue(CommandsViewPaneProperty, value);
			}
		}

		public PaneVisibilityState DetailsPane
		{
			get
			{
				return (PaneVisibilityState)GetValue(DetailsPaneProperty);
			}
			set
			{
				SetValue(DetailsPaneProperty, value);
			}
		}

		public PaneVisibilityState NavigationPane
		{
			get
			{
				return (PaneVisibilityState)GetValue(NavigationPaneProperty);
			}
			set
			{
				SetValue(NavigationPaneProperty, value);
			}
		}

		public PaneVisibilityState PreviewPane
		{
			get
			{
				return (PaneVisibilityState)GetValue(PreviewPaneProperty);
			}
			set
			{
				SetValue(PreviewPaneProperty, value);
			}
		}

		public PaneVisibilityState QueryPane
		{
			get
			{
				return (PaneVisibilityState)GetValue(QueryPaneProperty);
			}
			set
			{
				SetValue(QueryPaneProperty, value);
			}
		}

		public int NavigationLogIndex
		{
			get
			{
				return (int)GetValue(NavigationLogIndexProperty);
			}
			set
			{
				SetValue(NavigationLogIndexProperty, value);
			}
		}

		public ExplorerBrowser()
		{
			InitializeComponent();
			ExplorerBrowserControl = new Microsoft.WindowsAPICodePack.Controls.WindowsForms.ExplorerBrowser();
			SelectedItems = (selectedItems = new ObservableCollection<ShellObject>());
			Items = (items = new ObservableCollection<ShellObject>());
			NavigationLog = (navigationLog = new ObservableCollection<ShellObject>());
			ExplorerBrowserControl.ItemsChanged += ItemsChanged;
			ExplorerBrowserControl.SelectionChanged += SelectionChanged;
			ExplorerBrowserControl.ViewEnumerationComplete += ExplorerBrowserControl_ViewEnumerationComplete;
			ExplorerBrowserControl.ViewSelectedItemChanged += ExplorerBrowserControl_ViewSelectedItemChanged;
			ExplorerBrowserControl.NavigationLog.NavigationLogChanged += NavigationLogChanged;
			WindowsFormsHost windowsFormsHost = new WindowsFormsHost();
			try
			{
				windowsFormsHost.Child = ExplorerBrowserControl;
				root.Children.Clear();
				root.Children.Add(windowsFormsHost);
			}
			catch
			{
				windowsFormsHost.Dispose();
				throw;
			}
			base.Loaded += ExplorerBrowser_Loaded;
		}

		private void ExplorerBrowserControl_ViewSelectedItemChanged(object sender, EventArgs e)
		{
		}

		private void ExplorerBrowserControl_ViewEnumerationComplete(object sender, EventArgs e)
		{
			itemsChanged.Set();
			selectionChanged.Set();
		}

		private void ExplorerBrowser_Loaded(object sender, RoutedEventArgs e)
		{
			dtCLRUpdater.Tick += UpdateDependencyPropertiesFromCLRPRoperties;
			dtCLRUpdater.Interval = new TimeSpan(1000000L);
			dtCLRUpdater.Start();
			if (initialNavigationTarget != null)
			{
				ExplorerBrowserControl.Navigate(initialNavigationTarget);
				initialNavigationTarget = null;
			}
			if (initialViewMode.HasValue)
			{
				ExplorerBrowserControl.ContentOptions.ViewMode = initialViewMode.Value;
				initialViewMode = null;
			}
		}

		private void UpdateDependencyPropertiesFromCLRPRoperties(object sender, EventArgs e)
		{
			AlignLeft = ExplorerBrowserControl.ContentOptions.AlignLeft;
			AutoArrange = ExplorerBrowserControl.ContentOptions.AutoArrange;
			CheckSelect = ExplorerBrowserControl.ContentOptions.CheckSelect;
			ExtendedTiles = ExplorerBrowserControl.ContentOptions.ExtendedTiles;
			FullRowSelect = ExplorerBrowserControl.ContentOptions.FullRowSelect;
			HideFileNames = ExplorerBrowserControl.ContentOptions.HideFileNames;
			NoBrowserViewState = ExplorerBrowserControl.ContentOptions.NoBrowserViewState;
			NoColumnHeader = ExplorerBrowserControl.ContentOptions.NoColumnHeader;
			NoHeaderInAllViews = ExplorerBrowserControl.ContentOptions.NoHeaderInAllViews;
			NoIcons = ExplorerBrowserControl.ContentOptions.NoIcons;
			NoSubfolders = ExplorerBrowserControl.ContentOptions.NoSubfolders;
			SingleClickActivate = ExplorerBrowserControl.ContentOptions.SingleClickActivate;
			SingleSelection = ExplorerBrowserControl.ContentOptions.SingleSelection;
			ThumbnailSize = ExplorerBrowserControl.ContentOptions.ThumbnailSize;
			ViewMode = ExplorerBrowserControl.ContentOptions.ViewMode;
			AlwaysNavigate = ExplorerBrowserControl.NavigationOptions.AlwaysNavigate;
			NavigateOnce = ExplorerBrowserControl.NavigationOptions.NavigateOnce;
			AdvancedQueryPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.AdvancedQuery;
			CommandsPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.Commands;
			CommandsOrganizePane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.CommandsOrganize;
			CommandsViewPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.CommandsView;
			DetailsPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.Details;
			NavigationPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.Navigation;
			PreviewPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.Preview;
			QueryPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.Query;
			NavigationLogIndex = ExplorerBrowserControl.NavigationLog.CurrentLocationIndex;
			if (itemsChanged.WaitOne(1, false))
			{
				items.Clear();
				foreach (ShellObject item2 in ExplorerBrowserControl.Items)
				{
					items.Add(item2);
				}
			}
			if (selectionChanged.WaitOne(1, false))
			{
				selectionChangeWaitCount = 4;
			}
			else
			{
				if (selectionChangeWaitCount <= 0)
				{
					return;
				}
				selectionChangeWaitCount--;
				if (selectionChangeWaitCount != 0)
				{
					return;
				}
				selectedItems.Clear();
				foreach (ShellObject selectedItem in ExplorerBrowserControl.SelectedItems)
				{
					selectedItems.Add(selectedItem);
				}
			}
		}

		private void NavigationLogChanged(object sender, NavigationLogEventArgs args)
		{
			navigationLog.Clear();
			foreach (ShellObject location in ExplorerBrowserControl.NavigationLog.Locations)
			{
				navigationLog.Add(location);
			}
		}

		private void SelectionChanged(object sender, EventArgs e)
		{
			selectionChanged.Set();
		}

		private void ItemsChanged(object sender, EventArgs e)
		{
			itemsChanged.Set();
		}

		private static void navigationTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl.explorerBrowserControl != null)
			{
				explorerBrowser.ExplorerBrowserControl.Navigate((ShellObject)e.NewValue);
			}
			else
			{
				explorerBrowser.initialNavigationTarget = (ShellObject)e.NewValue;
			}
		}

		private static void OnAlignLeftChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl != null)
			{
				explorerBrowser.ExplorerBrowserControl.ContentOptions.AlignLeft = (bool)e.NewValue;
			}
		}

		private static void OnAutoArrangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl != null)
			{
				explorerBrowser.ExplorerBrowserControl.ContentOptions.AutoArrange = (bool)e.NewValue;
			}
		}

		private static void OnCheckSelectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl != null)
			{
				explorerBrowser.ExplorerBrowserControl.ContentOptions.CheckSelect = (bool)e.NewValue;
			}
		}

		private static void OnExtendedTilesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl != null)
			{
				explorerBrowser.ExplorerBrowserControl.ContentOptions.ExtendedTiles = (bool)e.NewValue;
			}
		}

		private static void OnFullRowSelectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl != null)
			{
				explorerBrowser.ExplorerBrowserControl.ContentOptions.FullRowSelect = (bool)e.NewValue;
			}
		}

		private static void OnHideFileNamesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl != null)
			{
				explorerBrowser.ExplorerBrowserControl.ContentOptions.HideFileNames = (bool)e.NewValue;
			}
		}

		private static void OnNoBrowserViewStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl != null)
			{
				explorerBrowser.ExplorerBrowserControl.ContentOptions.NoBrowserViewState = (bool)e.NewValue;
			}
		}

		private static void OnNoColumnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl != null)
			{
				explorerBrowser.ExplorerBrowserControl.ContentOptions.NoColumnHeader = (bool)e.NewValue;
			}
		}

		private static void OnNoHeaderInAllViewsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl != null)
			{
				explorerBrowser.ExplorerBrowserControl.ContentOptions.NoHeaderInAllViews = (bool)e.NewValue;
			}
		}

		private static void OnNoIconsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl != null)
			{
				explorerBrowser.ExplorerBrowserControl.ContentOptions.NoIcons = (bool)e.NewValue;
			}
		}

		private static void OnNoSubfoldersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl != null)
			{
				explorerBrowser.ExplorerBrowserControl.ContentOptions.NoSubfolders = (bool)e.NewValue;
			}
		}

		private static void OnSingleClickActivateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl != null)
			{
				explorerBrowser.ExplorerBrowserControl.ContentOptions.SingleClickActivate = (bool)e.NewValue;
			}
		}

		private static void OnSingleSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl != null)
			{
				explorerBrowser.ExplorerBrowserControl.ContentOptions.SingleSelection = (bool)e.NewValue;
			}
		}

		private static void OnThumbnailSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl != null)
			{
				explorerBrowser.ExplorerBrowserControl.ContentOptions.ThumbnailSize = (int)e.NewValue;
			}
		}

		private static void OnViewModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl != null)
			{
				if (explorerBrowser.ExplorerBrowserControl.explorerBrowserControl == null)
				{
					explorerBrowser.initialViewMode = (ExplorerBrowserViewMode)e.NewValue;
				}
				else
				{
					explorerBrowser.ExplorerBrowserControl.ContentOptions.ViewMode = (ExplorerBrowserViewMode)e.NewValue;
				}
			}
		}

		private static void OnAlwaysNavigateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl != null)
			{
				explorerBrowser.ExplorerBrowserControl.NavigationOptions.AlwaysNavigate = (bool)e.NewValue;
			}
		}

		private static void OnNavigateOnceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl != null)
			{
				explorerBrowser.ExplorerBrowserControl.NavigationOptions.NavigateOnce = (bool)e.NewValue;
			}
		}

		private static void OnAdvancedQueryPaneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl != null)
			{
				explorerBrowser.ExplorerBrowserControl.NavigationOptions.PaneVisibility.AdvancedQuery = (PaneVisibilityState)e.NewValue;
			}
		}

		private static void OnCommandsPaneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl != null)
			{
				explorerBrowser.ExplorerBrowserControl.NavigationOptions.PaneVisibility.Commands = (PaneVisibilityState)e.NewValue;
			}
		}

		private static void OnCommandsOrganizePaneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl != null)
			{
				explorerBrowser.ExplorerBrowserControl.NavigationOptions.PaneVisibility.CommandsOrganize = (PaneVisibilityState)e.NewValue;
			}
		}

		private static void OnCommandsViewPaneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl != null)
			{
				explorerBrowser.ExplorerBrowserControl.NavigationOptions.PaneVisibility.CommandsView = (PaneVisibilityState)e.NewValue;
			}
		}

		private static void OnDetailsPaneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl != null)
			{
				explorerBrowser.ExplorerBrowserControl.NavigationOptions.PaneVisibility.Details = (PaneVisibilityState)e.NewValue;
			}
		}

		private static void OnNavigationPaneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl != null)
			{
				explorerBrowser.ExplorerBrowserControl.NavigationOptions.PaneVisibility.Navigation = (PaneVisibilityState)e.NewValue;
			}
		}

		private static void OnPreviewPaneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl != null)
			{
				explorerBrowser.ExplorerBrowserControl.NavigationOptions.PaneVisibility.Preview = (PaneVisibilityState)e.NewValue;
			}
		}

		private static void OnQueryPaneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl != null)
			{
				explorerBrowser.ExplorerBrowserControl.NavigationOptions.PaneVisibility.Query = (PaneVisibilityState)e.NewValue;
			}
		}

		private static void OnNavigationLogIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ExplorerBrowser explorerBrowser = d as ExplorerBrowser;
			if (explorerBrowser.ExplorerBrowserControl != null)
			{
				explorerBrowser.ExplorerBrowserControl.NavigationLog.NavigateLog((int)e.NewValue);
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposed)
		{
			if (disposed)
			{
				if (itemsChanged != null)
				{
					itemsChanged.Close();
				}
				if (selectionChanged != null)
				{
					selectionChanged.Close();
				}
			}
		}
	}
}
