using System;
using System.Drawing;
using System.Windows.Forms;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	public class GlassForm : Form
	{
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
			BackColor = Color.Transparent;
		}

		public void ExcludeControlFromAeroGlass(Control control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (AeroGlassCompositionEnabled)
			{
				Rectangle rectangle = RectangleToScreen(base.ClientRectangle);
				Rectangle rectangle2 = control.RectangleToScreen(control.ClientRectangle);
				Margins m = default(Margins);
				m.LeftWidth = rectangle2.Left - rectangle.Left;
				m.RightWidth = rectangle.Right - rectangle2.Right;
				m.TopHeight = rectangle2.Top - rectangle.Top;
				m.BottomHeight = rectangle.Bottom - rectangle2.Bottom;
				DesktopWindowManagerNativeMethods.DwmExtendFrameIntoClientArea(base.Handle, ref m);
			}
		}

		public void ResetAeroGlass()
		{
			if (base.Handle != IntPtr.Zero)
			{
				Margins m = new Margins(true);
				DesktopWindowManagerNativeMethods.DwmExtendFrameIntoClientArea(base.Handle, ref m);
			}
		}

		protected override void WndProc(ref Message m)
		{
			if ((m.Msg == 798 || m.Msg == 799) && this.AeroGlassCompositionChanged != null)
			{
				this.AeroGlassCompositionChanged(this, new AeroGlassCompositionChangedEventArgs(AeroGlassCompositionEnabled));
			}
			base.WndProc(ref m);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			ResetAeroGlass();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (!base.DesignMode && AeroGlassCompositionEnabled)
			{
				e?.Graphics.FillRectangle(Brushes.Black, base.ClientRectangle);
			}
		}
	}
}
