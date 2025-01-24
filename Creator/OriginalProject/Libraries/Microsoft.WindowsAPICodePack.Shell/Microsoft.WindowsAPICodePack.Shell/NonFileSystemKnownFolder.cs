#define DEBUG
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Shell
{
	public class NonFileSystemKnownFolder : ShellNonFileSystemFolder, IKnownFolder, IEnumerable<ShellObject>, IEnumerable, IDisposable
	{
		private IKnownFolderNative knownFolderNative;

		private KnownFolderSettings knownFolderSettings;

		private KnownFolderSettings KnownFolderSettings
		{
			get
			{
				if (knownFolderNative == null)
				{
					if (nativeShellItem != null && base.PIDL == IntPtr.Zero)
					{
						base.PIDL = ShellHelper.PidlFromShellItem(nativeShellItem);
					}
					if (base.PIDL != IntPtr.Zero)
					{
						knownFolderNative = KnownFolderHelper.FromPIDL(base.PIDL);
					}
					Debug.Assert(knownFolderNative != null);
				}
				if (knownFolderSettings == null)
				{
					knownFolderSettings = new KnownFolderSettings(knownFolderNative);
				}
				return knownFolderSettings;
			}
		}

		public string Path => KnownFolderSettings.Path;

		public FolderCategory Category => KnownFolderSettings.Category;

		public string CanonicalName => KnownFolderSettings.CanonicalName;

		public string Description => KnownFolderSettings.Description;

		public Guid ParentId => KnownFolderSettings.ParentId;

		public string RelativePath => KnownFolderSettings.RelativePath;

		public override string ParsingName => base.ParsingName;

		public string Tooltip => KnownFolderSettings.Tooltip;

		public string TooltipResourceId => KnownFolderSettings.TooltipResourceId;

		public string LocalizedName => KnownFolderSettings.LocalizedName;

		public string LocalizedNameResourceId => KnownFolderSettings.LocalizedNameResourceId;

		public string Security => KnownFolderSettings.Security;

		public FileAttributes FileAttributes => KnownFolderSettings.FileAttributes;

		public DefinitionOptions DefinitionOptions => KnownFolderSettings.DefinitionOptions;

		public Guid FolderTypeId => KnownFolderSettings.FolderTypeId;

		public string FolderType => KnownFolderSettings.FolderType;

		public Guid FolderId => KnownFolderSettings.FolderId;

		public bool PathExists => KnownFolderSettings.PathExists;

		public RedirectionCapability Redirection => KnownFolderSettings.Redirection;

		internal NonFileSystemKnownFolder(IShellItem2 shellItem)
			: base(shellItem)
		{
		}

		internal NonFileSystemKnownFolder(IKnownFolderNative kf)
		{
			Debug.Assert(kf != null);
			knownFolderNative = kf;
			Guid interfaceGuid = new Guid("7E9FB0D3-919F-4307-AB2E-9B1860310C93");
			knownFolderNative.GetShellItem(0, ref interfaceGuid, out nativeShellItem);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				knownFolderSettings = null;
			}
			if (knownFolderNative != null)
			{
				Marshal.ReleaseComObject(knownFolderNative);
				knownFolderNative = null;
			}
			base.Dispose(disposing);
		}
	}
}
