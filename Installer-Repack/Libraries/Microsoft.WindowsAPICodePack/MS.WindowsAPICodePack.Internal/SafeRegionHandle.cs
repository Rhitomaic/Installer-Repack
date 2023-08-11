namespace MS.WindowsAPICodePack.Internal
{
	public class SafeRegionHandle : ZeroInvalidHandle
	{
		protected override bool ReleaseHandle()
		{
			if (CoreNativeMethods.DeleteObject(handle))
			{
				return true;
			}
			return false;
		}
	}
}
