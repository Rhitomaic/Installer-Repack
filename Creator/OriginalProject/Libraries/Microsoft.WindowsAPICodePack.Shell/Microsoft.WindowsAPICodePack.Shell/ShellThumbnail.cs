using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Microsoft.WindowsAPICodePack.Shell.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	public class ShellThumbnail
	{
		private IShellItem shellItemNative;

		private System.Windows.Size currentSize = new System.Windows.Size(256.0, 256.0);

		private ShellThumbnailFormatOption formatOption = ShellThumbnailFormatOption.Default;

		public System.Windows.Size CurrentSize
		{
			get
			{
				return currentSize;
			}
			set
			{
				if (value.Height == 0.0 || value.Width == 0.0)
				{
					throw new ArgumentOutOfRangeException("value", LocalizedMessages.ShellThumbnailSizeCannotBe0);
				}
				System.Windows.Size size = ((FormatOption == ShellThumbnailFormatOption.IconOnly) ? DefaultIconSize.Maximum : DefaultThumbnailSize.Maximum);
				if (value.Height > size.Height || value.Width > size.Width)
				{
					throw new ArgumentOutOfRangeException("value", string.Format(CultureInfo.InvariantCulture, LocalizedMessages.ShellThumbnailCurrentSizeRange, size.ToString()));
				}
				currentSize = value;
			}
		}

		public Bitmap Bitmap => GetBitmap(CurrentSize);

		public BitmapSource BitmapSource => GetBitmapSource(CurrentSize);

		public Icon Icon => Icon.FromHandle(Bitmap.GetHicon());

		public Bitmap SmallBitmap => GetBitmap(DefaultIconSize.Small, DefaultThumbnailSize.Small);

		public BitmapSource SmallBitmapSource => GetBitmapSource(DefaultIconSize.Small, DefaultThumbnailSize.Small);

		public Icon SmallIcon => Icon.FromHandle(SmallBitmap.GetHicon());

		public Bitmap MediumBitmap => GetBitmap(DefaultIconSize.Medium, DefaultThumbnailSize.Medium);

		public BitmapSource MediumBitmapSource => GetBitmapSource(DefaultIconSize.Medium, DefaultThumbnailSize.Medium);

		public Icon MediumIcon => Icon.FromHandle(MediumBitmap.GetHicon());

		public Bitmap LargeBitmap => GetBitmap(DefaultIconSize.Large, DefaultThumbnailSize.Large);

		public BitmapSource LargeBitmapSource => GetBitmapSource(DefaultIconSize.Large, DefaultThumbnailSize.Large);

		public Icon LargeIcon => Icon.FromHandle(LargeBitmap.GetHicon());

		public Bitmap ExtraLargeBitmap => GetBitmap(DefaultIconSize.ExtraLarge, DefaultThumbnailSize.ExtraLarge);

		public BitmapSource ExtraLargeBitmapSource => GetBitmapSource(DefaultIconSize.ExtraLarge, DefaultThumbnailSize.ExtraLarge);

		public Icon ExtraLargeIcon => Icon.FromHandle(ExtraLargeBitmap.GetHicon());

		public ShellThumbnailRetrievalOption RetrievalOption { get; set; }

		public ShellThumbnailFormatOption FormatOption
		{
			get
			{
				return formatOption;
			}
			set
			{
				formatOption = value;
				if (FormatOption == ShellThumbnailFormatOption.IconOnly && (CurrentSize.Height > DefaultIconSize.Maximum.Height || CurrentSize.Width > DefaultIconSize.Maximum.Width))
				{
					CurrentSize = DefaultIconSize.Maximum;
				}
			}
		}

		public bool AllowBiggerSize { get; set; }

		internal ShellThumbnail(ShellObject shellObject)
		{
			if (shellObject == null || shellObject.NativeShellItem == null)
			{
				throw new ArgumentNullException("shellObject");
			}
			shellItemNative = shellObject.NativeShellItem;
		}

		private ShellNativeMethods.SIIGBF CalculateFlags()
		{
			ShellNativeMethods.SIIGBF sIIGBF = ShellNativeMethods.SIIGBF.ResizeToFit;
			if (AllowBiggerSize)
			{
				sIIGBF |= ShellNativeMethods.SIIGBF.BiggerSizeOk;
			}
			if (RetrievalOption == ShellThumbnailRetrievalOption.CacheOnly)
			{
				sIIGBF |= ShellNativeMethods.SIIGBF.InCacheOnly;
			}
			else if (RetrievalOption == ShellThumbnailRetrievalOption.MemoryOnly)
			{
				sIIGBF |= ShellNativeMethods.SIIGBF.MemoryOnly;
			}
			if (FormatOption == ShellThumbnailFormatOption.IconOnly)
			{
				sIIGBF |= ShellNativeMethods.SIIGBF.IconOnly;
			}
			else if (FormatOption == ShellThumbnailFormatOption.ThumbnailOnly)
			{
				sIIGBF |= ShellNativeMethods.SIIGBF.ThumbnailOnly;
			}
			return sIIGBF;
		}

		private IntPtr GetHBitmap(System.Windows.Size size)
		{
			IntPtr phbm = IntPtr.Zero;
			CoreNativeMethods.Size size2 = default(CoreNativeMethods.Size);
			size2.Width = Convert.ToInt32(size.Width);
			size2.Height = Convert.ToInt32(size.Height);
			HResult image = ((IShellItemImageFactory)shellItemNative).GetImage(size2, CalculateFlags(), out phbm);
			switch (image)
			{
			case HResult.Ok:
				return phbm;
			case (HResult)(-2147175936):
				if (FormatOption == ShellThumbnailFormatOption.ThumbnailOnly)
				{
					throw new InvalidOperationException(LocalizedMessages.ShellThumbnailDoesNotHaveThumbnail, Marshal.GetExceptionForHR((int)image));
				}
				break;
			}
			if (image == (HResult)(-2147221164))
			{
				throw new NotSupportedException(LocalizedMessages.ShellThumbnailNoHandler, Marshal.GetExceptionForHR((int)image));
			}
			throw new ShellException(image);
		}

		private Bitmap GetBitmap(System.Windows.Size iconOnlySize, System.Windows.Size thumbnailSize)
		{
			return GetBitmap((FormatOption == ShellThumbnailFormatOption.IconOnly) ? iconOnlySize : thumbnailSize);
		}

		private Bitmap GetBitmap(System.Windows.Size size)
		{
			IntPtr hBitmap = GetHBitmap(size);
			Bitmap result = Image.FromHbitmap(hBitmap);
			ShellNativeMethods.DeleteObject(hBitmap);
			return result;
		}

		private BitmapSource GetBitmapSource(System.Windows.Size iconOnlySize, System.Windows.Size thumbnailSize)
		{
			return GetBitmapSource((FormatOption == ShellThumbnailFormatOption.IconOnly) ? iconOnlySize : thumbnailSize);
		}

		private BitmapSource GetBitmapSource(System.Windows.Size size)
		{
			IntPtr hBitmap = GetHBitmap(size);
			BitmapSource result = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
			ShellNativeMethods.DeleteObject(hBitmap);
			return result;
		}
	}
}
