namespace Microsoft.WindowsAPICodePack.Shell
{
	public struct NativePoint
	{
		public int X { get; set; }

		public int Y { get; set; }

		public NativePoint(int x, int y)
		{
			this = default(NativePoint);
			X = x;
			Y = y;
		}

		public static bool operator ==(NativePoint first, NativePoint second)
		{
			return first.X == second.X && first.Y == second.Y;
		}

		public static bool operator !=(NativePoint first, NativePoint second)
		{
			return !(first == second);
		}

		public override bool Equals(object obj)
		{
			return obj != null && obj is NativePoint && this == (NativePoint)obj;
		}

		public override int GetHashCode()
		{
			int hashCode = X.GetHashCode();
			return hashCode * 31 + Y.GetHashCode();
		}
	}
}
