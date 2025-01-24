#define DEBUG
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	internal class KnownFolderSettings
	{
		private FolderProperties knownFolderProperties;

		public string Path => knownFolderProperties.path;

		public FolderCategory Category => knownFolderProperties.category;

		public string CanonicalName => knownFolderProperties.canonicalName;

		public string Description => knownFolderProperties.description;

		public Guid ParentId => knownFolderProperties.parentId;

		public string RelativePath => knownFolderProperties.relativePath;

		public string Tooltip => knownFolderProperties.tooltip;

		public string TooltipResourceId => knownFolderProperties.tooltipResourceId;

		public string LocalizedName => knownFolderProperties.localizedName;

		public string LocalizedNameResourceId => knownFolderProperties.localizedNameResourceId;

		public string Security => knownFolderProperties.security;

		public FileAttributes FileAttributes => knownFolderProperties.fileAttributes;

		public DefinitionOptions DefinitionOptions => knownFolderProperties.definitionOptions;

		public Guid FolderTypeId => knownFolderProperties.folderTypeId;

		public string FolderType => knownFolderProperties.folderType;

		public Guid FolderId => knownFolderProperties.folderId;

		public bool PathExists => knownFolderProperties.pathExists;

		public RedirectionCapability Redirection => knownFolderProperties.redirection;

		internal KnownFolderSettings(IKnownFolderNative knownFolderNative)
		{
			GetFolderProperties(knownFolderNative);
		}

		private void GetFolderProperties(IKnownFolderNative knownFolderNative)
		{
			Debug.Assert(knownFolderNative != null);
			knownFolderNative.GetFolderDefinition(out var definition);
			try
			{
				knownFolderProperties.category = definition.category;
				knownFolderProperties.canonicalName = Marshal.PtrToStringUni(definition.name);
				knownFolderProperties.description = Marshal.PtrToStringUni(definition.description);
				knownFolderProperties.parentId = definition.parentId;
				knownFolderProperties.relativePath = Marshal.PtrToStringUni(definition.relativePath);
				knownFolderProperties.parsingName = Marshal.PtrToStringUni(definition.parsingName);
				knownFolderProperties.tooltipResourceId = Marshal.PtrToStringUni(definition.tooltip);
				knownFolderProperties.localizedNameResourceId = Marshal.PtrToStringUni(definition.localizedName);
				knownFolderProperties.iconResourceId = Marshal.PtrToStringUni(definition.icon);
				knownFolderProperties.security = Marshal.PtrToStringUni(definition.security);
				knownFolderProperties.fileAttributes = (FileAttributes)definition.attributes;
				knownFolderProperties.definitionOptions = definition.definitionOptions;
				knownFolderProperties.folderTypeId = definition.folderTypeId;
				knownFolderProperties.folderType = FolderTypes.GetFolderType(knownFolderProperties.folderTypeId);
				knownFolderProperties.path = GetPath(out var fileExists, knownFolderNative);
				knownFolderProperties.pathExists = fileExists;
				knownFolderProperties.redirection = knownFolderNative.GetRedirectionCapabilities();
				knownFolderProperties.tooltip = CoreHelpers.GetStringResource(knownFolderProperties.tooltipResourceId);
				knownFolderProperties.localizedName = CoreHelpers.GetStringResource(knownFolderProperties.localizedNameResourceId);
				knownFolderProperties.folderId = knownFolderNative.GetId();
			}
			finally
			{
				Marshal.FreeCoTaskMem(definition.name);
				Marshal.FreeCoTaskMem(definition.description);
				Marshal.FreeCoTaskMem(definition.relativePath);
				Marshal.FreeCoTaskMem(definition.parsingName);
				Marshal.FreeCoTaskMem(definition.tooltip);
				Marshal.FreeCoTaskMem(definition.localizedName);
				Marshal.FreeCoTaskMem(definition.icon);
				Marshal.FreeCoTaskMem(definition.security);
			}
		}

		private string GetPath(out bool fileExists, IKnownFolderNative knownFolderNative)
		{
			Debug.Assert(knownFolderNative != null);
			string result = string.Empty;
			fileExists = true;
			if (knownFolderProperties.category == FolderCategory.Virtual)
			{
				fileExists = false;
				return result;
			}
			try
			{
				result = knownFolderNative.GetPath(0);
			}
			catch (FileNotFoundException)
			{
				fileExists = false;
			}
			catch (DirectoryNotFoundException)
			{
				fileExists = false;
			}
			return result;
		}
	}
}
