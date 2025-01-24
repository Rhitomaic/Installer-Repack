using System;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	public abstract class TaskDialogButtonBase : TaskDialogControl
	{
		private string text;

		private bool enabled = true;

		private bool defaultControl;

		public string Text
		{
			get
			{
				return text;
			}
			set
			{
				CheckPropertyChangeAllowed("Text");
				text = value;
				ApplyPropertyChange("Text");
			}
		}

		public bool Enabled
		{
			get
			{
				return enabled;
			}
			set
			{
				CheckPropertyChangeAllowed("Enabled");
				enabled = value;
				ApplyPropertyChange("Enabled");
			}
		}

		public bool Default
		{
			get
			{
				return defaultControl;
			}
			set
			{
				CheckPropertyChangeAllowed("Default");
				defaultControl = value;
				ApplyPropertyChange("Default");
			}
		}

		public event EventHandler Click;

		protected TaskDialogButtonBase()
		{
		}

		protected TaskDialogButtonBase(string name, string text)
			: base(name)
		{
			this.text = text;
		}

		internal void RaiseClickEvent()
		{
			if (enabled && this.Click != null)
			{
				this.Click(this, EventArgs.Empty);
			}
		}

		public override string ToString()
		{
			return text ?? string.Empty;
		}
	}
}
