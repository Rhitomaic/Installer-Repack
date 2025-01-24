using System;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	[Flags]
	public enum ExecutionStates
	{
		None = 0,
		SystemRequired = 1,
		DisplayRequired = 2,
		AwayModeRequired = 0x40,
		Continuous = int.MinValue
	}
}
