using System;
using System.Security;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[SuppressUnmanagedCodeSecurity]
	internal static class KnownFoldersSafeNativeMethods
	{
		internal struct NativeFolderDefinition
		{
			internal FolderCategory category;

			internal IntPtr name;

			internal IntPtr description;

			internal Guid parentId;

			internal IntPtr relativePath;

			internal IntPtr parsingName;

			internal IntPtr tooltip;

			internal IntPtr localizedName;

			internal IntPtr icon;

			internal IntPtr security;

			internal uint attributes;

			internal DefinitionOptions definitionOptions;

			internal Guid folderTypeId;
		}
	}
}
