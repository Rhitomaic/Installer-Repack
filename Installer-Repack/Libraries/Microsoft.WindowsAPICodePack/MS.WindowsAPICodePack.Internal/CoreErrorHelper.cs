namespace MS.WindowsAPICodePack.Internal
{
	internal static class CoreErrorHelper
	{
		private const int FacilityWin32 = 7;

		public const int Ignored = 0;

		public static int HResultFromWin32(int win32ErrorCode)
		{
			if (win32ErrorCode > 0)
			{
				win32ErrorCode = (win32ErrorCode & 0xFFFF) | 0x70000 | int.MinValue;
			}
			return win32ErrorCode;
		}

		public static bool Succeeded(int result)
		{
			return result >= 0;
		}

		public static bool Succeeded(HResult result)
		{
			return Succeeded((int)result);
		}

		public static bool Failed(HResult result)
		{
			return !Succeeded(result);
		}

		public static bool Failed(int result)
		{
			return !Succeeded(result);
		}

		public static bool Matches(int result, int win32ErrorCode)
		{
			return result == HResultFromWin32(win32ErrorCode);
		}
	}
}
