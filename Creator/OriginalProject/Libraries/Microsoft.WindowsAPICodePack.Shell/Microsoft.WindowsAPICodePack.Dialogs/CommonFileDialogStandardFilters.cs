using Microsoft.WindowsAPICodePack.Shell.Resources;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	public static class CommonFileDialogStandardFilters
	{
		private static CommonFileDialogFilter textFilesFilter;

		private static CommonFileDialogFilter pictureFilesFilter;

		private static CommonFileDialogFilter officeFilesFilter;

		public static CommonFileDialogFilter TextFiles
		{
			get
			{
				if (textFilesFilter == null)
				{
					textFilesFilter = new CommonFileDialogFilter(LocalizedMessages.CommonFiltersText, "*.txt");
				}
				return textFilesFilter;
			}
		}

		public static CommonFileDialogFilter PictureFiles
		{
			get
			{
				if (pictureFilesFilter == null)
				{
					pictureFilesFilter = new CommonFileDialogFilter(LocalizedMessages.CommonFiltersPicture, "*.bmp, *.jpg, *.jpeg, *.png, *.ico");
				}
				return pictureFilesFilter;
			}
		}

		public static CommonFileDialogFilter OfficeFiles
		{
			get
			{
				if (officeFilesFilter == null)
				{
					officeFilesFilter = new CommonFileDialogFilter(LocalizedMessages.CommonFiltersOffice, "*.doc, *.docx, *.xls, *.xlsx, *.ppt, *.pptx");
				}
				return officeFilesFilter;
			}
		}
	}
}
