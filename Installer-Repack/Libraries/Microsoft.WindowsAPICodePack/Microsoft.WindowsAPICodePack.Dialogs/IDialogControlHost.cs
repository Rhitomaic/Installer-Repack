namespace Microsoft.WindowsAPICodePack.Dialogs
{
	public interface IDialogControlHost
	{
		bool IsCollectionChangeAllowed();

		void ApplyCollectionChanged();

		bool IsControlPropertyChangeAllowed(string propertyName, DialogControl control);

		void ApplyControlPropertyChange(string propertyName, DialogControl control);
	}
}
