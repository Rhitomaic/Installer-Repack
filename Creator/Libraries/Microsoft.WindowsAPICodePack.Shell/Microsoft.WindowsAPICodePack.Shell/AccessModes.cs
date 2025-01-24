using System;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[Flags]
	public enum AccessModes
	{
		Direct = 0,
		Transacted = 0x10000,
		Simple = 0x8000000,
		Read = 0,
		Write = 1,
		ReadWrite = 2,
		ShareDenyNone = 0x40,
		ShareDenyRead = 0x30,
		ShareDenyWrite = 0x20,
		ShareExclusive = 0x10,
		Priority = 0x40000,
		DeleteOnRelease = 0x4000000,
		NoScratch = 0x100000,
		Create = 0x1000,
		Convert = 0x20000,
		FailIfThere = 0,
		NoSnapshot = 0x200000,
		DirectSingleWriterMultipleReader = 0x400000
	}
}
