namespace Microsoft.WindowsAPICodePack.Dialogs.Controls
{
	public abstract class CommonFileDialogControl : DialogControl
	{
		private string textValue;

		private bool enabled = true;

		private bool visible = true;

		private bool isAdded;

		public virtual string Text
		{
			get
			{
				return textValue;
			}
			set
			{
				if (value != textValue)
				{
					textValue = value;
					ApplyPropertyChange("Text");
				}
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
				if (value != enabled)
				{
					enabled = value;
					ApplyPropertyChange("Enabled");
				}
			}
		}

		public bool Visible
		{
			get
			{
				return visible;
			}
			set
			{
				if (value != visible)
				{
					visible = value;
					ApplyPropertyChange("Visible");
				}
			}
		}

		internal bool IsAdded
		{
			get
			{
				return isAdded;
			}
			set
			{
				isAdded = value;
			}
		}

		protected CommonFileDialogControl()
		{
		}

		protected CommonFileDialogControl(string text)
		{
			textValue = text;
		}

		protected CommonFileDialogControl(string name, string text)
			: base(name)
		{
			textValue = text;
		}

		internal abstract void Attach(IFileDialogCustomize dialog);

		internal virtual void SyncUnmanagedProperties()
		{
			ApplyPropertyChange("Enabled");
			ApplyPropertyChange("Visible");
		}
	}
}
