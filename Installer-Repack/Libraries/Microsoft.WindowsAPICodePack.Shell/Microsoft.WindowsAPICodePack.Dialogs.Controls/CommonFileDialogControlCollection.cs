using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.WindowsAPICodePack.Shell.Resources;

namespace Microsoft.WindowsAPICodePack.Dialogs.Controls
{
	public sealed class CommonFileDialogControlCollection<T> : Collection<T> where T : DialogControl
	{
		private IDialogControlHost hostingDialog;

		public T this[string name]
		{
			get
			{
				if (string.IsNullOrEmpty(name))
				{
					throw new ArgumentException(LocalizedMessages.DialogControlCollectionEmptyName, "name");
				}
				foreach (T item in base.Items)
				{
					T current = item;
					if (current.Name == name)
					{
						return current;
					}
					if (!(current is CommonFileDialogGroupBox))
					{
						continue;
					}
                    CommonFileDialogGroupBox commonFileDialogGroupBox = current as CommonFileDialogGroupBox;

                    foreach (T item2 in commonFileDialogGroupBox.Items)
					{
						T result = item2;
						if (result.Name == name)
						{
							return result;
						}
					}
				}
				return null;
			}
		}

		internal CommonFileDialogControlCollection(IDialogControlHost host)
		{
			hostingDialog = host;
		}

		protected override void InsertItem(int index, T control)
		{
			if (base.Items.Contains(control))
			{
				throw new InvalidOperationException(LocalizedMessages.DialogControlCollectionMoreThanOneControl);
			}
			if (control.HostingDialog != null)
			{
				throw new InvalidOperationException(LocalizedMessages.DialogControlCollectionRemoveControlFirst);
			}
			if (!hostingDialog.IsCollectionChangeAllowed())
			{
				throw new InvalidOperationException(LocalizedMessages.DialogControlCollectionModifyingControls);
			}
			if (control is CommonFileDialogMenuItem)
			{
				throw new InvalidOperationException(LocalizedMessages.DialogControlCollectionMenuItemControlsCannotBeAdded);
			}
			control.HostingDialog = hostingDialog;
			base.InsertItem(index, control);
			hostingDialog.ApplyCollectionChanged();
		}

		protected override void RemoveItem(int index)
		{
			throw new NotSupportedException(LocalizedMessages.DialogControlCollectionCannotRemoveControls);
		}

		internal DialogControl GetControlbyId(int id)
		{
			return GetSubControlbyId(base.Items.Cast<DialogControl>(), id);
		}

		internal DialogControl GetSubControlbyId(IEnumerable<DialogControl> controlCollection, int id)
		{
			if (controlCollection == null)
			{
				return null;
			}
			foreach (DialogControl item in controlCollection)
			{
				if (item.Id == id)
				{
					return item;
				}
				if (item is CommonFileDialogGroupBox commonFileDialogGroupBox)
				{
					DialogControl subControlbyId = GetSubControlbyId(commonFileDialogGroupBox.Items, id);
					if (subControlbyId != null)
					{
						return subControlbyId;
					}
				}
			}
			return null;
		}
	}
}
