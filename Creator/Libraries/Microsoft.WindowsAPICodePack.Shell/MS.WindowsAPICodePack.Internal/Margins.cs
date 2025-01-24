namespace MS.WindowsAPICodePack.Internal
{
	internal struct Margins
	{
		public int LeftWidth;

		public int RightWidth;

		public int TopHeight;

		public int BottomHeight;

		public Margins(bool fullWindow)
		{
			LeftWidth = (RightWidth = (TopHeight = (BottomHeight = (fullWindow ? (-1) : 0))));
		}
	}
}
