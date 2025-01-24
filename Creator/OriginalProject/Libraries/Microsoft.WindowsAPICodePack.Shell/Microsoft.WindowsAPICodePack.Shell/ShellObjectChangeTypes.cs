using System;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[Flags]
	public enum ShellObjectChangeTypes
	{
		None = 0,
		ItemRename = 1,
		ItemCreate = 2,
		ItemDelete = 4,
		DirectoryCreate = 8,
		DirectoryDelete = 0x10,
		MediaInsert = 0x20,
		MediaRemove = 0x40,
		DriveRemove = 0x80,
		DriveAdd = 0x100,
		NetShare = 0x200,
		NetUnshare = 0x400,
		AttributesChange = 0x800,
		DirectoryContentsUpdate = 0x1000,
		Update = 0x2000,
		ServerDisconnect = 0x4000,
		SystemImageUpdate = 0x8000,
		DirectoryRename = 0x20000,
		FreeSpace = 0x40000,
		AssociationChange = 0x8000000,
		DiskEventsMask = 0x2381F,
		GlobalEventsMask = 0xC0581E0,
		AllEventsMask = int.MaxValue,
		FromInterrupt = int.MinValue
	}
}
