using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Microsoft.WindowsAPICodePack.Shell
{
	internal struct FolderProperties
	{
		internal string name;

		internal FolderCategory category;

		internal string canonicalName;

		internal string description;

		internal Guid parentId;

		internal string parent;

		internal string relativePath;

		internal string parsingName;

		internal string tooltipResourceId;

		internal string tooltip;

		internal string localizedName;

		internal string localizedNameResourceId;

		internal string iconResourceId;

		internal BitmapSource icon;

		internal DefinitionOptions definitionOptions;

		internal FileAttributes fileAttributes;

		internal Guid folderTypeId;

		internal string folderType;

		internal Guid folderId;

		internal string path;

		internal bool pathExists;

		internal RedirectionCapability redirection;

		internal string security;
	}
}
