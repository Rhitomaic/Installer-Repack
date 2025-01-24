using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Controls.WindowsPresentationFoundation
{
	public partial class CommandLink : UserControl, INotifyPropertyChanged, IComponentConnector
	{
		private string link;

		private string note;

		private ImageSource icon;

		public RoutedUICommand Command { get; set; }

		public string Link
		{
			get
			{
				return link;
			}
			set
			{
				link = value;
				if (this.PropertyChanged != null)
				{
					this.PropertyChanged(this, new PropertyChangedEventArgs("Link"));
				}
			}
		}

		public string Note
		{
			get
			{
				return note;
			}
			set
			{
				note = value;
				if (this.PropertyChanged != null)
				{
					this.PropertyChanged(this, new PropertyChangedEventArgs("Note"));
				}
			}
		}

		public ImageSource Icon
		{
			get
			{
				return icon;
			}
			set
			{
				icon = value;
				if (this.PropertyChanged != null)
				{
					this.PropertyChanged(this, new PropertyChangedEventArgs("Icon"));
				}
			}
		}

		public bool? IsCheck
		{
			get
			{
				return button.IsChecked;
			}
			set
			{
				button.IsChecked = value;
			}
		}

		public static bool IsPlatformSupported => CoreHelpers.RunningOnVista;

		public event RoutedEventHandler Click;

		public event PropertyChangedEventHandler PropertyChanged;

		public CommandLink()
		{
			CoreHelpers.ThrowIfNotVista();
			base.DataContext = this;
			InitializeComponent();
			button.Click += button_Click;
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			e.Source = this;
			if (this.Click != null)
			{
				this.Click(sender, e);
			}
		}
	}
}
