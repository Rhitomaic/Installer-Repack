using System;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[Flags]
	public enum DefinitionOptions
	{
		None = 0,
		LocalRedirectOnly = 2,
		Roamable = 4,
		Precreate = 8
	}
}
