using System;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	internal static class PowerPersonalityGuids
	{
		internal static readonly Guid HighPerformance = new Guid(2355003354u, 59583, 19094, 154, 133, 166, 226, 58, 140, 99, 92);

		internal static readonly Guid PowerSaver = new Guid(2709787400u, 13633, 20395, 188, 129, 247, 21, 86, 242, 11, 74);

		internal static readonly Guid Automatic = new Guid(941310498u, 63124, 16880, 150, 133, byte.MaxValue, 91, 178, 96, 223, 46);

		internal static readonly Guid All = new Guid(1755441502, 5098, 16865, 128, 17, 12, 73, 108, 164, 144, 176);

		internal static PowerPersonality GuidToEnum(Guid guid)
		{
			if (guid == HighPerformance)
			{
				return PowerPersonality.HighPerformance;
			}
			if (guid == PowerSaver)
			{
				return PowerPersonality.PowerSaver;
			}
			if (guid == Automatic)
			{
				return PowerPersonality.Automatic;
			}
			return PowerPersonality.Unknown;
		}
	}
}
