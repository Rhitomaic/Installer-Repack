namespace MS.WindowsAPICodePack.Internal
{
	public class SafeIconHandle : ZeroInvalidHandle
	{
		protected override bool ReleaseHandle()
		{
			if (CoreNativeMethods.DestroyIcon(handle))
			{
				return true;
			}
			return false;
		}
	}
}
