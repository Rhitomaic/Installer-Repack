namespace MS.WindowsAPICodePack.Internal
{
	public class SafeWindowHandle : ZeroInvalidHandle
	{
		protected override bool ReleaseHandle()
		{
			if (IsInvalid)
			{
				return true;
			}
			if (CoreNativeMethods.DestroyWindow(handle) != 0)
			{
				return true;
			}
			return false;
		}
	}
}
