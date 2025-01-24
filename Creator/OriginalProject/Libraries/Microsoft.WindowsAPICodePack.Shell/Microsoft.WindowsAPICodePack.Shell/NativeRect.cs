namespace Microsoft.WindowsAPICodePack.Shell
{
	public struct NativeRect
	{
		public int Left { get; set; }

		public int Top { get; set; }

		public int Right { get; set; }

		public int Bottom { get; set; }

		public NativeRect(int left, int top, int right, int bottom)
		{
			this = default(NativeRect);
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		public static bool operator ==(NativeRect first, NativeRect second)
		{
			return first.Left == second.Left && first.Top == second.Top && first.Right == second.Right && first.Bottom == second.Bottom;
		}

		public static bool operator !=(NativeRect first, NativeRect second)
		{
			return !(first == second);
		}

		public override bool Equals(object obj)
		{
			return obj != null && obj is NativeRect && this == (NativeRect)obj;
		}

		public override int GetHashCode()
		{
			int hashCode = Left.GetHashCode();
			hashCode = hashCode * 31 + Top.GetHashCode();
			hashCode = hashCode * 31 + Right.GetHashCode();
			return hashCode * 31 + Bottom.GetHashCode();
		}
	}
}
