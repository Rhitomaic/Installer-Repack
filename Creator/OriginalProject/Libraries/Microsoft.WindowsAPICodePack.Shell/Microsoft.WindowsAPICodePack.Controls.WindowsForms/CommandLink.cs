using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Controls.WindowsForms
{
	public class CommandLink : Button
	{
		private bool useElevationIcon;

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.Style = AddCommandLinkStyle(createParams.Style);
				return createParams;
			}
		}

		protected override Size DefaultSize => new Size(180, 60);

		[DefaultValue("(Note Text)")]
		[Category("Appearance")]
		[Description("Specifies the supporting note text.")]
		[Browsable(true)]
		public string NoteText
		{
			get
			{
				return GetNote(this);
			}
			set
			{
				SetNote(this, value);
			}
		}

		[Browsable(true)]
		[Category("Appearance")]
		[Description("Indicates whether the button should be decorated with the security shield icon (Windows Vista only).")]
		[DefaultValue(false)]
		public bool UseElevationIcon
		{
			get
			{
				return useElevationIcon;
			}
			set
			{
				useElevationIcon = value;
				SetShieldIcon(this, useElevationIcon);
			}
		}

		public static bool IsPlatformSupported => CoreHelpers.RunningOnVista;

		public CommandLink()
		{
			CoreHelpers.ThrowIfNotVista();
			base.FlatStyle = FlatStyle.System;
		}

		private static int AddCommandLinkStyle(int style)
		{
			if (CoreHelpers.RunningOnVista)
			{
				style |= 0xE;
			}
			return style;
		}

		private static string GetNote(Button Button)
		{
			IntPtr intPtr = CoreNativeMethods.SendMessage(Button.Handle, 5643u, IntPtr.Zero, IntPtr.Zero);
			int wparam = (int)intPtr + 1;
			StringBuilder stringBuilder = new StringBuilder(wparam);
			intPtr = CoreNativeMethods.SendMessage(Button.Handle, 5642u, ref wparam, stringBuilder);
			return stringBuilder.ToString();
		}

		private static void SetNote(Button button, string text)
		{
			CoreNativeMethods.SendMessage(button.Handle, 5641u, 0, text);
		}

		internal static void SetShieldIcon(Button Button, bool Show)
		{
			IntPtr lparam = new IntPtr(Show ? 1 : 0);
			CoreNativeMethods.SendMessage(Button.Handle, 5644u, IntPtr.Zero, lparam);
		}
	}
}
