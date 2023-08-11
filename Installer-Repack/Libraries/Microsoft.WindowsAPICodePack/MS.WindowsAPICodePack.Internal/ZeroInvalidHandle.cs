using System;
using System.Runtime.InteropServices;

namespace MS.WindowsAPICodePack.Internal
{
	public abstract class ZeroInvalidHandle : SafeHandle
	{
		public override bool IsInvalid => handle == IntPtr.Zero;

		protected ZeroInvalidHandle()
			: base(IntPtr.Zero, true)
		{
		}
	}
}
