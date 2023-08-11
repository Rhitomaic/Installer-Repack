using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Shell;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	[ComImport]
	[CoClass(typeof(FileSaveDialogRCW))]
	[Guid("84BCCD23-5FDE-4CDB-AEA4-AF64B83D78AB")]
	internal interface NativeFileSaveDialog : IFileSaveDialog, IFileDialog, IModalWindow
	{
	}
}
