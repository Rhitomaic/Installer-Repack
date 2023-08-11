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
	public class StockIcon : IDisposable
	{
		private StockIconIdentifier identifier = StockIconIdentifier.Application;

		private StockIconSize currentSize = StockIconSize.Large;

		private bool linkOverlay;

		private bool selected;

		private bool invalidateIcon = true;

		private IntPtr hIcon = IntPtr.Zero;

		public bool Selected
		{
			get
			{
				return selected;
			}
			set
			{
				selected = value;
				invalidateIcon = true;
			}
		}

		public bool LinkOverlay
		{
			get
			{
				return linkOverlay;
			}
			set
			{
				linkOverlay = value;
				invalidateIcon = true;
			}
		}

		public StockIconSize CurrentSize
		{
			get
			{
				return currentSize;
			}
			set
			{
				currentSize = value;
				invalidateIcon = true;
			}
		}

		public StockIconIdentifier Identifier
		{
			get
			{
				return identifier;
			}
			set
			{
				identifier = value;
				invalidateIcon = true;
			}
		}

		public Bitmap Bitmap
		{
			get
			{
				UpdateHIcon();
				return (hIcon != IntPtr.Zero) ? Bitmap.FromHicon(hIcon) : null;
			}
		}

		public BitmapSource BitmapSource
		{
			get
			{
				UpdateHIcon();
				return (hIcon != IntPtr.Zero) ? Imaging.CreateBitmapSourceFromHIcon(hIcon, Int32Rect.Empty, null) : null;
			}
		}

		public Icon Icon
		{
			get
			{
				UpdateHIcon();
				return (hIcon != IntPtr.Zero) ? Icon.FromHandle(hIcon) : null;
			}
		}

		public StockIcon(StockIconIdentifier id)
		{
			identifier = id;
			invalidateIcon = true;
		}

		public StockIcon(StockIconIdentifier id, StockIconSize size, bool isLinkOverlay, bool isSelected)
		{
			identifier = id;
			linkOverlay = isLinkOverlay;
			selected = isSelected;
			currentSize = size;
			invalidateIcon = true;
		}

		private void UpdateHIcon()
		{
			if (invalidateIcon)
			{
				if (hIcon != IntPtr.Zero)
				{
					CoreNativeMethods.DestroyIcon(hIcon);
				}
				hIcon = GetHIcon();
				invalidateIcon = false;
			}
		}

		private IntPtr GetHIcon()
		{
			StockIconsNativeMethods.StockIconOptions stockIconOptions = StockIconsNativeMethods.StockIconOptions.Handle;
			stockIconOptions = ((CurrentSize == StockIconSize.Small) ? (stockIconOptions | StockIconsNativeMethods.StockIconOptions.Small) : ((CurrentSize != StockIconSize.ShellSize) ? stockIconOptions : (stockIconOptions | StockIconsNativeMethods.StockIconOptions.ShellSize)));
			if (Selected)
			{
				stockIconOptions |= StockIconsNativeMethods.StockIconOptions.Selected;
			}
			if (LinkOverlay)
			{
				stockIconOptions |= StockIconsNativeMethods.StockIconOptions.LinkOverlay;
			}
			StockIconsNativeMethods.StockIconInfo info = default(StockIconsNativeMethods.StockIconInfo);
			info.StuctureSize = (uint)Marshal.SizeOf(typeof(StockIconsNativeMethods.StockIconInfo));
			switch (StockIconsNativeMethods.SHGetStockIconInfo(identifier, stockIconOptions, ref info))
			{
			case HResult.InvalidArguments:
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, LocalizedMessages.StockIconInvalidGuid, identifier));
			default:
				return IntPtr.Zero;
			case HResult.Ok:
				return info.Handle;
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
			}
			if (hIcon != IntPtr.Zero)
			{
				CoreNativeMethods.DestroyIcon(hIcon);
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~StockIcon()
		{
			Dispose(false);
		}
	}
}
