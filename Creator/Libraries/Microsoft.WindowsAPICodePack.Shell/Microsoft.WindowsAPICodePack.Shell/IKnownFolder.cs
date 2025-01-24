using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.WindowsAPICodePack.Shell
{
	public interface IKnownFolder : IDisposable, IEnumerable<ShellObject>, IEnumerable
	{
		string Path { get; }

		FolderCategory Category { get; }

		string CanonicalName { get; }

		string Description { get; }

		Guid ParentId { get; }

		string RelativePath { get; }

		string ParsingName { get; }

		string Tooltip { get; }

		string TooltipResourceId { get; }

		string LocalizedName { get; }

		string LocalizedNameResourceId { get; }

		string Security { get; }

		FileAttributes FileAttributes { get; }

		DefinitionOptions DefinitionOptions { get; }

		Guid FolderTypeId { get; }

		string FolderType { get; }

		Guid FolderId { get; }

		bool PathExists { get; }

		RedirectionCapability Redirection { get; }
	}
}
