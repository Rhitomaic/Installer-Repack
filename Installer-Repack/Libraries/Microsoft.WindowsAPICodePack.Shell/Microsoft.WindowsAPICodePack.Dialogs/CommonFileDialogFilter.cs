using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using Microsoft.WindowsAPICodePack.Shell;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	public class CommonFileDialogFilter
	{
		private Collection<string> extensions;

		private string rawDisplayName;

		private bool showExtensions = true;

		public string DisplayName
		{
			get
			{
				if (showExtensions)
				{
					return string.Format(CultureInfo.InvariantCulture, "{0} ({1})", rawDisplayName, GetDisplayExtensionList(extensions));
				}
				return rawDisplayName;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException("value");
				}
				rawDisplayName = value;
			}
		}

		public Collection<string> Extensions => extensions;

		public bool ShowExtensions
		{
			get
			{
				return showExtensions;
			}
			set
			{
				showExtensions = value;
			}
		}

		public CommonFileDialogFilter()
		{
			extensions = new Collection<string>();
		}

		public CommonFileDialogFilter(string rawDisplayName, string extensionList)
			: this()
		{
			if (string.IsNullOrEmpty(extensionList))
			{
				throw new ArgumentNullException("extensionList");
			}
			this.rawDisplayName = rawDisplayName;
			string[] array = extensionList.Split(',', ';');
			string[] array2 = array;
			foreach (string rawExtension in array2)
			{
				extensions.Add(NormalizeExtension(rawExtension));
			}
		}

		private static string NormalizeExtension(string rawExtension)
		{
			rawExtension = rawExtension.Trim();
			rawExtension = rawExtension.Replace("*.", null);
			rawExtension = rawExtension.Replace(".", null);
			return rawExtension;
		}

		private static string GetDisplayExtensionList(Collection<string> extensions)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string extension in extensions)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("*.");
				stringBuilder.Append(extension);
			}
			return stringBuilder.ToString();
		}

		internal ShellNativeMethods.FilterSpec GetFilterSpec()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string extension in extensions)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(";");
				}
				stringBuilder.Append("*.");
				stringBuilder.Append(extension);
			}
			return new ShellNativeMethods.FilterSpec(DisplayName, stringBuilder.ToString());
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} ({1})", rawDisplayName, GetDisplayExtensionList(extensions));
		}
	}
}
