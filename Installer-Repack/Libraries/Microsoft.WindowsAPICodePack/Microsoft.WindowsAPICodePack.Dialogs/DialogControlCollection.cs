using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.WindowsAPICodePack.Resources;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	public sealed class DialogControlCollection<T> : Collection<T> where T : DialogControl
	{
		private IDialogControlHost hostingDialog;

		public T this[string name]
		{
			get
			{
				if (string.IsNullOrEmpty(name))
				{
					throw new ArgumentException(LocalizedMessages.DialogCollectionControlNameNull, "name");
				}
				return base.Items.FirstOrDefault((T x) => x.Name == name);
			}
		}

		internal DialogControlCollection(IDialogControlHost host)
		{
			hostingDialog = host;
		}

		protected override void InsertItem(int index, T control)
		{
			if (base.Items.Contains(control))
			{
				throw new InvalidOperationException(LocalizedMessages.DialogCollectionCannotHaveDuplicateNames);
			}
			if (control.HostingDialog != null)
			{
				throw new InvalidOperationException(LocalizedMessages.DialogCollectionControlAlreadyHosted);
			}
			if (!hostingDialog.IsCollectionChangeAllowed())
			{
				throw new InvalidOperationException(LocalizedMessages.DialogCollectionModifyShowingDialog);
			}
			control.HostingDialog = hostingDialog;
			base.InsertItem(index, control);
			hostingDialog.ApplyCollectionChanged();
		}

		protected override void RemoveItem(int index)
		{
			if (!hostingDialog.IsCollectionChangeAllowed())
			{
				throw new InvalidOperationException(LocalizedMessages.DialogCollectionModifyShowingDialog);
			}
			DialogControl dialogControl = base.Items[index];
			dialogControl.HostingDialog = null;
			base.RemoveItem(index);
			hostingDialog.ApplyCollectionChanged();
		}

		internal DialogControl GetControlbyId(int id)
		{
			return base.Items.FirstOrDefault((T x) => x.Id == id);
		}
	}
}
