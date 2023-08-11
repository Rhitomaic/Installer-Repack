using System;

namespace Microsoft.WindowsAPICodePack.Shell.Interop
{
	public struct Message
	{
		private IntPtr windowHandle;

		private uint msg;

		private IntPtr wparam;

		private IntPtr lparam;

		private int time;

		private NativePoint point;

		public IntPtr WindowHandle => windowHandle;

		public uint Msg => msg;

		public IntPtr WParam => wparam;

		public IntPtr LParam => lparam;

		public int Time => time;

		public NativePoint Point => point;

		internal Message(IntPtr windowHandle, uint msg, IntPtr wparam, IntPtr lparam, int time, NativePoint point)
		{
			this = default(Message);
			this.windowHandle = windowHandle;
			this.msg = msg;
			this.wparam = wparam;
			this.lparam = lparam;
			this.time = time;
			this.point = point;
		}

		public static bool operator ==(Message first, Message second)
		{
			return first.WindowHandle == second.WindowHandle && first.Msg == second.Msg && first.WParam == second.WParam && first.LParam == second.LParam && first.Time == second.Time && first.Point == second.Point;
		}

		public static bool operator !=(Message first, Message second)
		{
			return !(first == second);
		}

		public override bool Equals(object obj)
		{
			return obj != null && obj is Message && this == (Message)obj;
		}

		public override int GetHashCode()
		{
			int hashCode = WindowHandle.GetHashCode();
			hashCode = hashCode * 31 + Msg.GetHashCode();
			hashCode = hashCode * 31 + WParam.GetHashCode();
			hashCode = hashCode * 31 + LParam.GetHashCode();
			hashCode = hashCode * 31 + Time.GetHashCode();
			return hashCode * 31 + Point.GetHashCode();
		}
	}
}
