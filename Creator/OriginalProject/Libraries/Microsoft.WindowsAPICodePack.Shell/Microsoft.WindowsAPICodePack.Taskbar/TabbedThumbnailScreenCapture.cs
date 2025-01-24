using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.WindowsAPICodePack.Shell;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	public static class TabbedThumbnailScreenCapture
	{
		public static Bitmap GrabWindowBitmap(IntPtr windowHandle, System.Drawing.Size bitmapSize)
		{
			if (bitmapSize.Height <= 0 || bitmapSize.Width <= 0)
			{
				return null;
			}
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				intPtr = TabbedThumbnailNativeMethods.GetWindowDC(windowHandle);
				TabbedThumbnailNativeMethods.GetClientSize(windowHandle, out var size);
				if (size == System.Drawing.Size.Empty)
				{
					size = new System.Drawing.Size(200, 200);
				}
				System.Drawing.Size size2 = ((bitmapSize == System.Drawing.Size.Empty) ? size : bitmapSize);
				Bitmap bitmap = null;
				try
				{
					bitmap = new Bitmap(size2.Width, size2.Height);
					using (Graphics graphics = Graphics.FromImage(bitmap))
					{
						IntPtr hdc = graphics.GetHdc();
						uint operation = 13369376u;
						System.Drawing.Size nonClientArea = WindowUtilities.GetNonClientArea(windowHandle);
						bool flag = TabbedThumbnailNativeMethods.StretchBlt(hdc, 0, 0, bitmap.Width, bitmap.Height, intPtr, nonClientArea.Width, nonClientArea.Height, size.Width, size.Height, operation);
						graphics.ReleaseHdc(hdc);
						if (!flag)
						{
							return null;
						}
						return bitmap;
					}
				}
				catch
				{
					bitmap?.Dispose();
					throw;
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					TabbedThumbnailNativeMethods.ReleaseDC(windowHandle, intPtr);
				}
			}
		}

		public static Bitmap GrabWindowBitmap(UIElement element, int dpiX, int dpiY, int width, int height)
		{
			if (element is HwndHost hwndHost)
			{
				IntPtr handle = hwndHost.Handle;
				return GrabWindowBitmap(handle, new System.Drawing.Size(width, height));
			}
			Rect descendantBounds = VisualTreeHelper.GetDescendantBounds(element);
			if (descendantBounds.Height == 0.0 || descendantBounds.Width == 0.0)
			{
				return null;
			}
			RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)(descendantBounds.Width * (double)dpiX / 96.0), (int)(descendantBounds.Height * (double)dpiY / 96.0), dpiX, dpiY, PixelFormats.Default);
			DrawingVisual drawingVisual = new DrawingVisual();
			using (DrawingContext drawingContext = drawingVisual.RenderOpen())
			{
				VisualBrush brush = new VisualBrush(element);
				drawingContext.DrawRectangle(brush, null, new Rect(default(System.Windows.Point), descendantBounds.Size));
			}
			renderTargetBitmap.Render(drawingVisual);
			BitmapEncoder bitmapEncoder = new PngBitmapEncoder();
			bitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
			Bitmap bitmap;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				bitmapEncoder.Save(memoryStream);
				memoryStream.Position = 0L;
				bitmap = new Bitmap(memoryStream);
			}
			return (Bitmap)bitmap.GetThumbnailImage(width, height, null, IntPtr.Zero);
		}

		internal static Bitmap ResizeImageWithAspect(IntPtr originalHBitmap, int newWidth, int maxHeight, bool resizeIfWider)
		{
			Bitmap bitmap = Image.FromHbitmap(originalHBitmap);
			try
			{
				if (resizeIfWider && bitmap.Width <= newWidth)
				{
					newWidth = bitmap.Width;
				}
				int num = bitmap.Height * newWidth / bitmap.Width;
				if (num > maxHeight)
				{
					newWidth = bitmap.Width * maxHeight / bitmap.Height;
					num = maxHeight;
				}
				return (Bitmap)bitmap.GetThumbnailImage(newWidth, num, null, IntPtr.Zero);
			}
			finally
			{
				bitmap.Dispose();
				bitmap = null;
			}
		}
	}
}
