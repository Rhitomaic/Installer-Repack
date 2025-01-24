#define DEBUG
using System;
using System.Diagnostics;
using Microsoft.WindowsAPICodePack.Resources;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	public abstract class DialogControl
	{
		private static int nextId = 9;

		private string name;

		public IDialogControlHost HostingDialog { get; set; }

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentException(LocalizedMessages.DialogControlNameCannotBeEmpty);
				}
				if (!string.IsNullOrEmpty(name))
				{
					throw new InvalidOperationException(LocalizedMessages.DialogControlsCannotBeRenamed);
				}
				name = value;
			}
		}

		public int Id { get; private set; }

		protected DialogControl()
		{
			Id = nextId;
			if (nextId == int.MaxValue)
			{
				nextId = 9;
			}
			else
			{
				nextId++;
			}
		}

		protected DialogControl(string name)
			: this()
		{
			Name = name;
		}

		protected void CheckPropertyChangeAllowed(string propName)
		{
			Debug.Assert(!string.IsNullOrEmpty(propName), "Property to change was not specified");
			if (HostingDialog != null)
			{
				HostingDialog.IsControlPropertyChangeAllowed(propName, this);
			}
		}

		protected void ApplyPropertyChange(string propName)
		{
			Debug.Assert(!string.IsNullOrEmpty(propName), "Property changed was not specified");
			if (HostingDialog != null)
			{
				HostingDialog.ApplyControlPropertyChange(propName, this);
			}
		}

		public override bool Equals(object obj)
		{
			if (obj is DialogControl dialogControl)
			{
				return Id == dialogControl.Id;
			}
			return false;
		}

		public override int GetHashCode()
		{
			if (Name == null)
			{
				return ToString().GetHashCode();
			}
			return Name.GetHashCode();
		}
	}
}
