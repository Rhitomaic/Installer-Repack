using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	public class GlassWindow : Window
	{
		private IntPtr windowHandle;

		public static bool AeroGlassCompositionEnabled
		{
			get
			{
				return DesktopWindowManagerNativeMethods.DwmIsCompositionEnabled();
			}
			set
			{
				DesktopWindowManagerNativeMethods.DwmEnableComposition(value ? CompositionEnable.Enable : CompositionEnable.Disable);
			}
		}

		public event EventHandler<AeroGlassCompositionChangedEventArgs> AeroGlassCompositionChanged;

		public void SetAeroGlassTransparency()
		{
			HwndSource.FromHwnd(windowHandle).CompositionTarget.BackgroundColor = Colors.Transparent;
			base.Background = Brushes.Transparent;
		}

		public void ExcludeElementFromAeroGlass(FrameworkElement element)
		{
			if (AeroGlassCompositionEnabled && element != null)
			{
				HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
				DesktopWindowManagerNativeMethods.GetWindowRect(hwndSource.Handle, out var rect);
				DesktopWindowManagerNativeMethods.GetClientRect(hwndSource.Handle, out var rect2);
				Size size = new Size((double)(rect.Right - rect.Left) - (double)(rect2.Right - rect2.Left), (double)(rect.Bottom - rect.Top) - (double)(rect2.Bottom - rect2.Top));
				GeneralTransform generalTransform = element.TransformToAncestor(this);
				Point point = generalTransform.Transform(new Point(0.0, 0.0));
				Point point2 = generalTransform.Transform(new Point(element.ActualWidth + size.Width, element.ActualHeight + size.Height));
				Margins m = default(Margins);
				m.LeftWidth = (int)point.X;
				m.RightWidth = (int)(base.ActualWidth - point2.X);
				m.TopHeight = (int)point.Y;
				m.BottomHeight = (int)(base.ActualHeight - point2.Y);
				DesktopWindowManagerNativeMethods.DwmExtendFrameIntoClientArea(windowHandle, ref m);
			}
		}

		public void ResetAeroGlass()
		{
			Margins m = new Margins(true);
			DesktopWindowManagerNativeMethods.DwmExtendFrameIntoClientArea(windowHandle, ref m);
		}

		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == 798 || msg == 799)
			{
				if (this.AeroGlassCompositionChanged != null)
				{
					this.AeroGlassCompositionChanged(this, new AeroGlassCompositionChangedEventArgs(AeroGlassCompositionEnabled));
				}
				handled = true;
			}
			return IntPtr.Zero;
		}

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);
			WindowInteropHelper windowInteropHelper = new WindowInteropHelper(this);
			windowHandle = windowInteropHelper.Handle;
			HwndSource hwndSource = HwndSource.FromHwnd(windowHandle);
			hwndSource.AddHook(WndProc);
			ResetAeroGlass();
		}
	}
}
