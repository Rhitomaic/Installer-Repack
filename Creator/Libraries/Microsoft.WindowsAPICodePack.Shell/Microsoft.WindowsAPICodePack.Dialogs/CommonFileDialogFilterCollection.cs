using System.Collections.ObjectModel;
using Microsoft.WindowsAPICodePack.Shell;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	public class CommonFileDialogFilterCollection : Collection<CommonFileDialogFilter>
	{
		internal CommonFileDialogFilterCollection()
		{
		}

		internal ShellNativeMethods.FilterSpec[] GetAllFilterSpecs()
		{
			ShellNativeMethods.FilterSpec[] array = new ShellNativeMethods.FilterSpec[base.Count];
			for (int i = 0; i < base.Count; i++)
			{
				ref ShellNativeMethods.FilterSpec reference = ref array[i];
				reference = base[i].GetFilterSpec();
			}
			return array;
		}
	}
}
