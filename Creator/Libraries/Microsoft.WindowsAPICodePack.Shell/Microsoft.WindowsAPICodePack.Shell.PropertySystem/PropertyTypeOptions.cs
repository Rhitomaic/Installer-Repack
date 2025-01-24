using System;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	[Flags]
	public enum PropertyTypeOptions
	{
		None = 0,
		MultipleValues = 1,
		IsInnate = 2,
		IsGroup = 4,
		CanGroupBy = 8,
		CanStackBy = 0x10,
		IsTreeProperty = 0x20,
		IncludeInFullTextQuery = 0x40,
		IsViewable = 0x80,
		IsQueryable = 0x100,
		CanBePurged = 0x200,
		IsSystemProperty = int.MinValue,
		MaskAll = -2147483137
	}
}
