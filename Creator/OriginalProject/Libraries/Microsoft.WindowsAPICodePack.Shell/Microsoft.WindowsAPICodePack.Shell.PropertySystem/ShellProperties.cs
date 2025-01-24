using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.WindowsAPICodePack.Shell.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	public class ShellProperties : IDisposable
	{
		public class PropertySystem : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			private PropertySystemAppUserModel internalPropertySystemAppUserModel;

			private PropertySystemAudio internalPropertySystemAudio;

			private PropertySystemCalendar internalPropertySystemCalendar;

			private PropertySystemCommunication internalPropertySystemCommunication;

			private PropertySystemComputer internalPropertySystemComputer;

			private PropertySystemContact internalPropertySystemContact;

			private PropertySystemDevice internalPropertySystemDevice;

			private PropertySystemDeviceInterface internalPropertySystemDeviceInterface;

			private PropertySystemDevices internalPropertySystemDevices;

			private PropertySystemDocument internalPropertySystemDocument;

			private PropertySystemDRM internalPropertySystemDRM;

			private PropertySystemGPS internalPropertySystemGPS;

			private PropertySystemIdentity internalPropertySystemIdentity;

			private PropertySystemIdentityProvider internalPropertySystemIdentityProvider;

			private PropertySystemImage internalPropertySystemImage;

			private PropertySystemJournal internalPropertySystemJournal;

			private PropertySystemLayoutPattern internalPropertySystemLayoutPattern;

			private PropertySystemLink internalPropertySystemLink;

			private PropertySystemMedia internalPropertySystemMedia;

			private PropertySystemMessage internalPropertySystemMessage;

			private PropertySystemMusic internalPropertySystemMusic;

			private PropertySystemNote internalPropertySystemNote;

			private PropertySystemPhoto internalPropertySystemPhoto;

			private PropertySystemPropGroup internalPropertySystemPropGroup;

			private PropertySystemPropList internalPropertySystemPropList;

			private PropertySystemRecordedTV internalPropertySystemRecordedTV;

			private PropertySystemSearch internalPropertySystemSearch;

			private PropertySystemShell internalPropertySystemShell;

			private PropertySystemSoftware internalPropertySystemSoftware;

			private PropertySystemSync internalPropertySystemSync;

			private PropertySystemTask internalPropertySystemTask;

			private PropertySystemVideo internalPropertySystemVideo;

			private PropertySystemVolume internalPropertySystemVolume;

			public ShellProperty<int?> AcquisitionID
			{
				get
				{
					PropertyKey acquisitionID = SystemProperties.System.AcquisitionID;
					if (!hashtable.ContainsKey(acquisitionID))
					{
						hashtable.Add(acquisitionID, shellObjectParent.Properties.CreateTypedProperty<int?>(acquisitionID));
					}
					return hashtable[acquisitionID] as ShellProperty<int?>;
				}
			}

			public ShellProperty<string> ApplicationName
			{
				get
				{
					PropertyKey applicationName = SystemProperties.System.ApplicationName;
					if (!hashtable.ContainsKey(applicationName))
					{
						hashtable.Add(applicationName, shellObjectParent.Properties.CreateTypedProperty<string>(applicationName));
					}
					return hashtable[applicationName] as ShellProperty<string>;
				}
			}

			public ShellProperty<string[]> Author
			{
				get
				{
					PropertyKey author = SystemProperties.System.Author;
					if (!hashtable.ContainsKey(author))
					{
						hashtable.Add(author, shellObjectParent.Properties.CreateTypedProperty<string[]>(author));
					}
					return hashtable[author] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<ulong?> Capacity
			{
				get
				{
					PropertyKey capacity = SystemProperties.System.Capacity;
					if (!hashtable.ContainsKey(capacity))
					{
						hashtable.Add(capacity, shellObjectParent.Properties.CreateTypedProperty<ulong?>(capacity));
					}
					return hashtable[capacity] as ShellProperty<ulong?>;
				}
			}

			public ShellProperty<string[]> Category
			{
				get
				{
					PropertyKey category = SystemProperties.System.Category;
					if (!hashtable.ContainsKey(category))
					{
						hashtable.Add(category, shellObjectParent.Properties.CreateTypedProperty<string[]>(category));
					}
					return hashtable[category] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string> Comment
			{
				get
				{
					PropertyKey comment = SystemProperties.System.Comment;
					if (!hashtable.ContainsKey(comment))
					{
						hashtable.Add(comment, shellObjectParent.Properties.CreateTypedProperty<string>(comment));
					}
					return hashtable[comment] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> Company
			{
				get
				{
					PropertyKey company = SystemProperties.System.Company;
					if (!hashtable.ContainsKey(company))
					{
						hashtable.Add(company, shellObjectParent.Properties.CreateTypedProperty<string>(company));
					}
					return hashtable[company] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ComputerName
			{
				get
				{
					PropertyKey computerName = SystemProperties.System.ComputerName;
					if (!hashtable.ContainsKey(computerName))
					{
						hashtable.Add(computerName, shellObjectParent.Properties.CreateTypedProperty<string>(computerName));
					}
					return hashtable[computerName] as ShellProperty<string>;
				}
			}

			public ShellProperty<IntPtr[]> ContainedItems
			{
				get
				{
					PropertyKey containedItems = SystemProperties.System.ContainedItems;
					if (!hashtable.ContainsKey(containedItems))
					{
						hashtable.Add(containedItems, shellObjectParent.Properties.CreateTypedProperty<IntPtr[]>(containedItems));
					}
					return hashtable[containedItems] as ShellProperty<IntPtr[]>;
				}
			}

			public ShellProperty<string> ContentStatus
			{
				get
				{
					PropertyKey contentStatus = SystemProperties.System.ContentStatus;
					if (!hashtable.ContainsKey(contentStatus))
					{
						hashtable.Add(contentStatus, shellObjectParent.Properties.CreateTypedProperty<string>(contentStatus));
					}
					return hashtable[contentStatus] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ContentType
			{
				get
				{
					PropertyKey contentType = SystemProperties.System.ContentType;
					if (!hashtable.ContainsKey(contentType))
					{
						hashtable.Add(contentType, shellObjectParent.Properties.CreateTypedProperty<string>(contentType));
					}
					return hashtable[contentType] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> Copyright
			{
				get
				{
					PropertyKey copyright = SystemProperties.System.Copyright;
					if (!hashtable.ContainsKey(copyright))
					{
						hashtable.Add(copyright, shellObjectParent.Properties.CreateTypedProperty<string>(copyright));
					}
					return hashtable[copyright] as ShellProperty<string>;
				}
			}

			public ShellProperty<DateTime?> DateAccessed
			{
				get
				{
					PropertyKey dateAccessed = SystemProperties.System.DateAccessed;
					if (!hashtable.ContainsKey(dateAccessed))
					{
						hashtable.Add(dateAccessed, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(dateAccessed));
					}
					return hashtable[dateAccessed] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<DateTime?> DateAcquired
			{
				get
				{
					PropertyKey dateAcquired = SystemProperties.System.DateAcquired;
					if (!hashtable.ContainsKey(dateAcquired))
					{
						hashtable.Add(dateAcquired, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(dateAcquired));
					}
					return hashtable[dateAcquired] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<DateTime?> DateArchived
			{
				get
				{
					PropertyKey dateArchived = SystemProperties.System.DateArchived;
					if (!hashtable.ContainsKey(dateArchived))
					{
						hashtable.Add(dateArchived, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(dateArchived));
					}
					return hashtable[dateArchived] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<DateTime?> DateCompleted
			{
				get
				{
					PropertyKey dateCompleted = SystemProperties.System.DateCompleted;
					if (!hashtable.ContainsKey(dateCompleted))
					{
						hashtable.Add(dateCompleted, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(dateCompleted));
					}
					return hashtable[dateCompleted] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<DateTime?> DateCreated
			{
				get
				{
					PropertyKey dateCreated = SystemProperties.System.DateCreated;
					if (!hashtable.ContainsKey(dateCreated))
					{
						hashtable.Add(dateCreated, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(dateCreated));
					}
					return hashtable[dateCreated] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<DateTime?> DateImported
			{
				get
				{
					PropertyKey dateImported = SystemProperties.System.DateImported;
					if (!hashtable.ContainsKey(dateImported))
					{
						hashtable.Add(dateImported, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(dateImported));
					}
					return hashtable[dateImported] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<DateTime?> DateModified
			{
				get
				{
					PropertyKey dateModified = SystemProperties.System.DateModified;
					if (!hashtable.ContainsKey(dateModified))
					{
						hashtable.Add(dateModified, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(dateModified));
					}
					return hashtable[dateModified] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<byte[]> DescriptionID
			{
				get
				{
					PropertyKey descriptionID = SystemProperties.System.DescriptionID;
					if (!hashtable.ContainsKey(descriptionID))
					{
						hashtable.Add(descriptionID, shellObjectParent.Properties.CreateTypedProperty<byte[]>(descriptionID));
					}
					return hashtable[descriptionID] as ShellProperty<byte[]>;
				}
			}

			public ShellProperty<DateTime?> DueDate
			{
				get
				{
					PropertyKey dueDate = SystemProperties.System.DueDate;
					if (!hashtable.ContainsKey(dueDate))
					{
						hashtable.Add(dueDate, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(dueDate));
					}
					return hashtable[dueDate] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<DateTime?> EndDate
			{
				get
				{
					PropertyKey endDate = SystemProperties.System.EndDate;
					if (!hashtable.ContainsKey(endDate))
					{
						hashtable.Add(endDate, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(endDate));
					}
					return hashtable[endDate] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<ulong?> FileAllocationSize
			{
				get
				{
					PropertyKey fileAllocationSize = SystemProperties.System.FileAllocationSize;
					if (!hashtable.ContainsKey(fileAllocationSize))
					{
						hashtable.Add(fileAllocationSize, shellObjectParent.Properties.CreateTypedProperty<ulong?>(fileAllocationSize));
					}
					return hashtable[fileAllocationSize] as ShellProperty<ulong?>;
				}
			}

			public ShellProperty<uint?> FileAttributes
			{
				get
				{
					PropertyKey fileAttributes = SystemProperties.System.FileAttributes;
					if (!hashtable.ContainsKey(fileAttributes))
					{
						hashtable.Add(fileAttributes, shellObjectParent.Properties.CreateTypedProperty<uint?>(fileAttributes));
					}
					return hashtable[fileAttributes] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<ulong?> FileCount
			{
				get
				{
					PropertyKey fileCount = SystemProperties.System.FileCount;
					if (!hashtable.ContainsKey(fileCount))
					{
						hashtable.Add(fileCount, shellObjectParent.Properties.CreateTypedProperty<ulong?>(fileCount));
					}
					return hashtable[fileCount] as ShellProperty<ulong?>;
				}
			}

			public ShellProperty<string> FileDescription
			{
				get
				{
					PropertyKey fileDescription = SystemProperties.System.FileDescription;
					if (!hashtable.ContainsKey(fileDescription))
					{
						hashtable.Add(fileDescription, shellObjectParent.Properties.CreateTypedProperty<string>(fileDescription));
					}
					return hashtable[fileDescription] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> FileExtension
			{
				get
				{
					PropertyKey fileExtension = SystemProperties.System.FileExtension;
					if (!hashtable.ContainsKey(fileExtension))
					{
						hashtable.Add(fileExtension, shellObjectParent.Properties.CreateTypedProperty<string>(fileExtension));
					}
					return hashtable[fileExtension] as ShellProperty<string>;
				}
			}

			public ShellProperty<ulong?> FileFRN
			{
				get
				{
					PropertyKey fileFRN = SystemProperties.System.FileFRN;
					if (!hashtable.ContainsKey(fileFRN))
					{
						hashtable.Add(fileFRN, shellObjectParent.Properties.CreateTypedProperty<ulong?>(fileFRN));
					}
					return hashtable[fileFRN] as ShellProperty<ulong?>;
				}
			}

			public ShellProperty<string> FileName
			{
				get
				{
					PropertyKey fileName = SystemProperties.System.FileName;
					if (!hashtable.ContainsKey(fileName))
					{
						hashtable.Add(fileName, shellObjectParent.Properties.CreateTypedProperty<string>(fileName));
					}
					return hashtable[fileName] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> FileOwner
			{
				get
				{
					PropertyKey fileOwner = SystemProperties.System.FileOwner;
					if (!hashtable.ContainsKey(fileOwner))
					{
						hashtable.Add(fileOwner, shellObjectParent.Properties.CreateTypedProperty<string>(fileOwner));
					}
					return hashtable[fileOwner] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> FileVersion
			{
				get
				{
					PropertyKey fileVersion = SystemProperties.System.FileVersion;
					if (!hashtable.ContainsKey(fileVersion))
					{
						hashtable.Add(fileVersion, shellObjectParent.Properties.CreateTypedProperty<string>(fileVersion));
					}
					return hashtable[fileVersion] as ShellProperty<string>;
				}
			}

			public ShellProperty<byte[]> FindData
			{
				get
				{
					PropertyKey findData = SystemProperties.System.FindData;
					if (!hashtable.ContainsKey(findData))
					{
						hashtable.Add(findData, shellObjectParent.Properties.CreateTypedProperty<byte[]>(findData));
					}
					return hashtable[findData] as ShellProperty<byte[]>;
				}
			}

			public ShellProperty<ushort?> FlagColor
			{
				get
				{
					PropertyKey flagColor = SystemProperties.System.FlagColor;
					if (!hashtable.ContainsKey(flagColor))
					{
						hashtable.Add(flagColor, shellObjectParent.Properties.CreateTypedProperty<ushort?>(flagColor));
					}
					return hashtable[flagColor] as ShellProperty<ushort?>;
				}
			}

			public ShellProperty<string> FlagColorText
			{
				get
				{
					PropertyKey flagColorText = SystemProperties.System.FlagColorText;
					if (!hashtable.ContainsKey(flagColorText))
					{
						hashtable.Add(flagColorText, shellObjectParent.Properties.CreateTypedProperty<string>(flagColorText));
					}
					return hashtable[flagColorText] as ShellProperty<string>;
				}
			}

			public ShellProperty<int?> FlagStatus
			{
				get
				{
					PropertyKey flagStatus = SystemProperties.System.FlagStatus;
					if (!hashtable.ContainsKey(flagStatus))
					{
						hashtable.Add(flagStatus, shellObjectParent.Properties.CreateTypedProperty<int?>(flagStatus));
					}
					return hashtable[flagStatus] as ShellProperty<int?>;
				}
			}

			public ShellProperty<string> FlagStatusText
			{
				get
				{
					PropertyKey flagStatusText = SystemProperties.System.FlagStatusText;
					if (!hashtable.ContainsKey(flagStatusText))
					{
						hashtable.Add(flagStatusText, shellObjectParent.Properties.CreateTypedProperty<string>(flagStatusText));
					}
					return hashtable[flagStatusText] as ShellProperty<string>;
				}
			}

			public ShellProperty<ulong?> FreeSpace
			{
				get
				{
					PropertyKey freeSpace = SystemProperties.System.FreeSpace;
					if (!hashtable.ContainsKey(freeSpace))
					{
						hashtable.Add(freeSpace, shellObjectParent.Properties.CreateTypedProperty<ulong?>(freeSpace));
					}
					return hashtable[freeSpace] as ShellProperty<ulong?>;
				}
			}

			public ShellProperty<string> FullText
			{
				get
				{
					PropertyKey fullText = SystemProperties.System.FullText;
					if (!hashtable.ContainsKey(fullText))
					{
						hashtable.Add(fullText, shellObjectParent.Properties.CreateTypedProperty<string>(fullText));
					}
					return hashtable[fullText] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> IdentityProperty
			{
				get
				{
					PropertyKey identityProperty = SystemProperties.System.IdentityProperty;
					if (!hashtable.ContainsKey(identityProperty))
					{
						hashtable.Add(identityProperty, shellObjectParent.Properties.CreateTypedProperty<string>(identityProperty));
					}
					return hashtable[identityProperty] as ShellProperty<string>;
				}
			}

			public ShellProperty<string[]> ImageParsingName
			{
				get
				{
					PropertyKey imageParsingName = SystemProperties.System.ImageParsingName;
					if (!hashtable.ContainsKey(imageParsingName))
					{
						hashtable.Add(imageParsingName, shellObjectParent.Properties.CreateTypedProperty<string[]>(imageParsingName));
					}
					return hashtable[imageParsingName] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<int?> Importance
			{
				get
				{
					PropertyKey importance = SystemProperties.System.Importance;
					if (!hashtable.ContainsKey(importance))
					{
						hashtable.Add(importance, shellObjectParent.Properties.CreateTypedProperty<int?>(importance));
					}
					return hashtable[importance] as ShellProperty<int?>;
				}
			}

			public ShellProperty<string> ImportanceText
			{
				get
				{
					PropertyKey importanceText = SystemProperties.System.ImportanceText;
					if (!hashtable.ContainsKey(importanceText))
					{
						hashtable.Add(importanceText, shellObjectParent.Properties.CreateTypedProperty<string>(importanceText));
					}
					return hashtable[importanceText] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> InfoTipText
			{
				get
				{
					PropertyKey infoTipText = SystemProperties.System.InfoTipText;
					if (!hashtable.ContainsKey(infoTipText))
					{
						hashtable.Add(infoTipText, shellObjectParent.Properties.CreateTypedProperty<string>(infoTipText));
					}
					return hashtable[infoTipText] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> InternalName
			{
				get
				{
					PropertyKey internalName = SystemProperties.System.InternalName;
					if (!hashtable.ContainsKey(internalName))
					{
						hashtable.Add(internalName, shellObjectParent.Properties.CreateTypedProperty<string>(internalName));
					}
					return hashtable[internalName] as ShellProperty<string>;
				}
			}

			public ShellProperty<bool?> IsAttachment
			{
				get
				{
					PropertyKey isAttachment = SystemProperties.System.IsAttachment;
					if (!hashtable.ContainsKey(isAttachment))
					{
						hashtable.Add(isAttachment, shellObjectParent.Properties.CreateTypedProperty<bool?>(isAttachment));
					}
					return hashtable[isAttachment] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> IsDefaultNonOwnerSaveLocation
			{
				get
				{
					PropertyKey isDefaultNonOwnerSaveLocation = SystemProperties.System.IsDefaultNonOwnerSaveLocation;
					if (!hashtable.ContainsKey(isDefaultNonOwnerSaveLocation))
					{
						hashtable.Add(isDefaultNonOwnerSaveLocation, shellObjectParent.Properties.CreateTypedProperty<bool?>(isDefaultNonOwnerSaveLocation));
					}
					return hashtable[isDefaultNonOwnerSaveLocation] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> IsDefaultSaveLocation
			{
				get
				{
					PropertyKey isDefaultSaveLocation = SystemProperties.System.IsDefaultSaveLocation;
					if (!hashtable.ContainsKey(isDefaultSaveLocation))
					{
						hashtable.Add(isDefaultSaveLocation, shellObjectParent.Properties.CreateTypedProperty<bool?>(isDefaultSaveLocation));
					}
					return hashtable[isDefaultSaveLocation] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> IsDeleted
			{
				get
				{
					PropertyKey isDeleted = SystemProperties.System.IsDeleted;
					if (!hashtable.ContainsKey(isDeleted))
					{
						hashtable.Add(isDeleted, shellObjectParent.Properties.CreateTypedProperty<bool?>(isDeleted));
					}
					return hashtable[isDeleted] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> IsEncrypted
			{
				get
				{
					PropertyKey isEncrypted = SystemProperties.System.IsEncrypted;
					if (!hashtable.ContainsKey(isEncrypted))
					{
						hashtable.Add(isEncrypted, shellObjectParent.Properties.CreateTypedProperty<bool?>(isEncrypted));
					}
					return hashtable[isEncrypted] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> IsFlagged
			{
				get
				{
					PropertyKey isFlagged = SystemProperties.System.IsFlagged;
					if (!hashtable.ContainsKey(isFlagged))
					{
						hashtable.Add(isFlagged, shellObjectParent.Properties.CreateTypedProperty<bool?>(isFlagged));
					}
					return hashtable[isFlagged] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> IsFlaggedComplete
			{
				get
				{
					PropertyKey isFlaggedComplete = SystemProperties.System.IsFlaggedComplete;
					if (!hashtable.ContainsKey(isFlaggedComplete))
					{
						hashtable.Add(isFlaggedComplete, shellObjectParent.Properties.CreateTypedProperty<bool?>(isFlaggedComplete));
					}
					return hashtable[isFlaggedComplete] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> IsIncomplete
			{
				get
				{
					PropertyKey isIncomplete = SystemProperties.System.IsIncomplete;
					if (!hashtable.ContainsKey(isIncomplete))
					{
						hashtable.Add(isIncomplete, shellObjectParent.Properties.CreateTypedProperty<bool?>(isIncomplete));
					}
					return hashtable[isIncomplete] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> IsLocationSupported
			{
				get
				{
					PropertyKey isLocationSupported = SystemProperties.System.IsLocationSupported;
					if (!hashtable.ContainsKey(isLocationSupported))
					{
						hashtable.Add(isLocationSupported, shellObjectParent.Properties.CreateTypedProperty<bool?>(isLocationSupported));
					}
					return hashtable[isLocationSupported] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> IsPinnedToNamespaceTree
			{
				get
				{
					PropertyKey isPinnedToNamespaceTree = SystemProperties.System.IsPinnedToNamespaceTree;
					if (!hashtable.ContainsKey(isPinnedToNamespaceTree))
					{
						hashtable.Add(isPinnedToNamespaceTree, shellObjectParent.Properties.CreateTypedProperty<bool?>(isPinnedToNamespaceTree));
					}
					return hashtable[isPinnedToNamespaceTree] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> IsRead
			{
				get
				{
					PropertyKey isRead = SystemProperties.System.IsRead;
					if (!hashtable.ContainsKey(isRead))
					{
						hashtable.Add(isRead, shellObjectParent.Properties.CreateTypedProperty<bool?>(isRead));
					}
					return hashtable[isRead] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> IsSearchOnlyItem
			{
				get
				{
					PropertyKey isSearchOnlyItem = SystemProperties.System.IsSearchOnlyItem;
					if (!hashtable.ContainsKey(isSearchOnlyItem))
					{
						hashtable.Add(isSearchOnlyItem, shellObjectParent.Properties.CreateTypedProperty<bool?>(isSearchOnlyItem));
					}
					return hashtable[isSearchOnlyItem] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> IsSendToTarget
			{
				get
				{
					PropertyKey isSendToTarget = SystemProperties.System.IsSendToTarget;
					if (!hashtable.ContainsKey(isSendToTarget))
					{
						hashtable.Add(isSendToTarget, shellObjectParent.Properties.CreateTypedProperty<bool?>(isSendToTarget));
					}
					return hashtable[isSendToTarget] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> IsShared
			{
				get
				{
					PropertyKey isShared = SystemProperties.System.IsShared;
					if (!hashtable.ContainsKey(isShared))
					{
						hashtable.Add(isShared, shellObjectParent.Properties.CreateTypedProperty<bool?>(isShared));
					}
					return hashtable[isShared] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<string[]> ItemAuthors
			{
				get
				{
					PropertyKey itemAuthors = SystemProperties.System.ItemAuthors;
					if (!hashtable.ContainsKey(itemAuthors))
					{
						hashtable.Add(itemAuthors, shellObjectParent.Properties.CreateTypedProperty<string[]>(itemAuthors));
					}
					return hashtable[itemAuthors] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string> ItemClassType
			{
				get
				{
					PropertyKey itemClassType = SystemProperties.System.ItemClassType;
					if (!hashtable.ContainsKey(itemClassType))
					{
						hashtable.Add(itemClassType, shellObjectParent.Properties.CreateTypedProperty<string>(itemClassType));
					}
					return hashtable[itemClassType] as ShellProperty<string>;
				}
			}

			public ShellProperty<DateTime?> ItemDate
			{
				get
				{
					PropertyKey itemDate = SystemProperties.System.ItemDate;
					if (!hashtable.ContainsKey(itemDate))
					{
						hashtable.Add(itemDate, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(itemDate));
					}
					return hashtable[itemDate] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<string> ItemFolderNameDisplay
			{
				get
				{
					PropertyKey itemFolderNameDisplay = SystemProperties.System.ItemFolderNameDisplay;
					if (!hashtable.ContainsKey(itemFolderNameDisplay))
					{
						hashtable.Add(itemFolderNameDisplay, shellObjectParent.Properties.CreateTypedProperty<string>(itemFolderNameDisplay));
					}
					return hashtable[itemFolderNameDisplay] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ItemFolderPathDisplay
			{
				get
				{
					PropertyKey itemFolderPathDisplay = SystemProperties.System.ItemFolderPathDisplay;
					if (!hashtable.ContainsKey(itemFolderPathDisplay))
					{
						hashtable.Add(itemFolderPathDisplay, shellObjectParent.Properties.CreateTypedProperty<string>(itemFolderPathDisplay));
					}
					return hashtable[itemFolderPathDisplay] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ItemFolderPathDisplayNarrow
			{
				get
				{
					PropertyKey itemFolderPathDisplayNarrow = SystemProperties.System.ItemFolderPathDisplayNarrow;
					if (!hashtable.ContainsKey(itemFolderPathDisplayNarrow))
					{
						hashtable.Add(itemFolderPathDisplayNarrow, shellObjectParent.Properties.CreateTypedProperty<string>(itemFolderPathDisplayNarrow));
					}
					return hashtable[itemFolderPathDisplayNarrow] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ItemName
			{
				get
				{
					PropertyKey itemName = SystemProperties.System.ItemName;
					if (!hashtable.ContainsKey(itemName))
					{
						hashtable.Add(itemName, shellObjectParent.Properties.CreateTypedProperty<string>(itemName));
					}
					return hashtable[itemName] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ItemNameDisplay
			{
				get
				{
					PropertyKey itemNameDisplay = SystemProperties.System.ItemNameDisplay;
					if (!hashtable.ContainsKey(itemNameDisplay))
					{
						hashtable.Add(itemNameDisplay, shellObjectParent.Properties.CreateTypedProperty<string>(itemNameDisplay));
					}
					return hashtable[itemNameDisplay] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ItemNamePrefix
			{
				get
				{
					PropertyKey itemNamePrefix = SystemProperties.System.ItemNamePrefix;
					if (!hashtable.ContainsKey(itemNamePrefix))
					{
						hashtable.Add(itemNamePrefix, shellObjectParent.Properties.CreateTypedProperty<string>(itemNamePrefix));
					}
					return hashtable[itemNamePrefix] as ShellProperty<string>;
				}
			}

			public ShellProperty<string[]> ItemParticipants
			{
				get
				{
					PropertyKey itemParticipants = SystemProperties.System.ItemParticipants;
					if (!hashtable.ContainsKey(itemParticipants))
					{
						hashtable.Add(itemParticipants, shellObjectParent.Properties.CreateTypedProperty<string[]>(itemParticipants));
					}
					return hashtable[itemParticipants] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string> ItemPathDisplay
			{
				get
				{
					PropertyKey itemPathDisplay = SystemProperties.System.ItemPathDisplay;
					if (!hashtable.ContainsKey(itemPathDisplay))
					{
						hashtable.Add(itemPathDisplay, shellObjectParent.Properties.CreateTypedProperty<string>(itemPathDisplay));
					}
					return hashtable[itemPathDisplay] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ItemPathDisplayNarrow
			{
				get
				{
					PropertyKey itemPathDisplayNarrow = SystemProperties.System.ItemPathDisplayNarrow;
					if (!hashtable.ContainsKey(itemPathDisplayNarrow))
					{
						hashtable.Add(itemPathDisplayNarrow, shellObjectParent.Properties.CreateTypedProperty<string>(itemPathDisplayNarrow));
					}
					return hashtable[itemPathDisplayNarrow] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ItemType
			{
				get
				{
					PropertyKey itemType = SystemProperties.System.ItemType;
					if (!hashtable.ContainsKey(itemType))
					{
						hashtable.Add(itemType, shellObjectParent.Properties.CreateTypedProperty<string>(itemType));
					}
					return hashtable[itemType] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ItemTypeText
			{
				get
				{
					PropertyKey itemTypeText = SystemProperties.System.ItemTypeText;
					if (!hashtable.ContainsKey(itemTypeText))
					{
						hashtable.Add(itemTypeText, shellObjectParent.Properties.CreateTypedProperty<string>(itemTypeText));
					}
					return hashtable[itemTypeText] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ItemUrl
			{
				get
				{
					PropertyKey itemUrl = SystemProperties.System.ItemUrl;
					if (!hashtable.ContainsKey(itemUrl))
					{
						hashtable.Add(itemUrl, shellObjectParent.Properties.CreateTypedProperty<string>(itemUrl));
					}
					return hashtable[itemUrl] as ShellProperty<string>;
				}
			}

			public ShellProperty<string[]> Keywords
			{
				get
				{
					PropertyKey keywords = SystemProperties.System.Keywords;
					if (!hashtable.ContainsKey(keywords))
					{
						hashtable.Add(keywords, shellObjectParent.Properties.CreateTypedProperty<string[]>(keywords));
					}
					return hashtable[keywords] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string[]> Kind
			{
				get
				{
					PropertyKey kind = SystemProperties.System.Kind;
					if (!hashtable.ContainsKey(kind))
					{
						hashtable.Add(kind, shellObjectParent.Properties.CreateTypedProperty<string[]>(kind));
					}
					return hashtable[kind] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string> KindText
			{
				get
				{
					PropertyKey kindText = SystemProperties.System.KindText;
					if (!hashtable.ContainsKey(kindText))
					{
						hashtable.Add(kindText, shellObjectParent.Properties.CreateTypedProperty<string>(kindText));
					}
					return hashtable[kindText] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> Language
			{
				get
				{
					PropertyKey language = SystemProperties.System.Language;
					if (!hashtable.ContainsKey(language))
					{
						hashtable.Add(language, shellObjectParent.Properties.CreateTypedProperty<string>(language));
					}
					return hashtable[language] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> MileageInformation
			{
				get
				{
					PropertyKey mileageInformation = SystemProperties.System.MileageInformation;
					if (!hashtable.ContainsKey(mileageInformation))
					{
						hashtable.Add(mileageInformation, shellObjectParent.Properties.CreateTypedProperty<string>(mileageInformation));
					}
					return hashtable[mileageInformation] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> MIMEType
			{
				get
				{
					PropertyKey mIMEType = SystemProperties.System.MIMEType;
					if (!hashtable.ContainsKey(mIMEType))
					{
						hashtable.Add(mIMEType, shellObjectParent.Properties.CreateTypedProperty<string>(mIMEType));
					}
					return hashtable[mIMEType] as ShellProperty<string>;
				}
			}

			public ShellProperty<IntPtr?> NamespaceClsid
			{
				get
				{
					PropertyKey namespaceClsid = SystemProperties.System.NamespaceClsid;
					if (!hashtable.ContainsKey(namespaceClsid))
					{
						hashtable.Add(namespaceClsid, shellObjectParent.Properties.CreateTypedProperty<IntPtr?>(namespaceClsid));
					}
					return hashtable[namespaceClsid] as ShellProperty<IntPtr?>;
				}
			}

			public ShellProperty<object> Null
			{
				get
				{
					PropertyKey @null = SystemProperties.System.Null;
					if (!hashtable.ContainsKey(@null))
					{
						hashtable.Add(@null, shellObjectParent.Properties.CreateTypedProperty<object>(@null));
					}
					return hashtable[@null] as ShellProperty<object>;
				}
			}

			public ShellProperty<uint?> OfflineAvailability
			{
				get
				{
					PropertyKey offlineAvailability = SystemProperties.System.OfflineAvailability;
					if (!hashtable.ContainsKey(offlineAvailability))
					{
						hashtable.Add(offlineAvailability, shellObjectParent.Properties.CreateTypedProperty<uint?>(offlineAvailability));
					}
					return hashtable[offlineAvailability] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> OfflineStatus
			{
				get
				{
					PropertyKey offlineStatus = SystemProperties.System.OfflineStatus;
					if (!hashtable.ContainsKey(offlineStatus))
					{
						hashtable.Add(offlineStatus, shellObjectParent.Properties.CreateTypedProperty<uint?>(offlineStatus));
					}
					return hashtable[offlineStatus] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string> OriginalFileName
			{
				get
				{
					PropertyKey originalFileName = SystemProperties.System.OriginalFileName;
					if (!hashtable.ContainsKey(originalFileName))
					{
						hashtable.Add(originalFileName, shellObjectParent.Properties.CreateTypedProperty<string>(originalFileName));
					}
					return hashtable[originalFileName] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> OwnerSid
			{
				get
				{
					PropertyKey ownerSid = SystemProperties.System.OwnerSid;
					if (!hashtable.ContainsKey(ownerSid))
					{
						hashtable.Add(ownerSid, shellObjectParent.Properties.CreateTypedProperty<string>(ownerSid));
					}
					return hashtable[ownerSid] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ParentalRating
			{
				get
				{
					PropertyKey parentalRating = SystemProperties.System.ParentalRating;
					if (!hashtable.ContainsKey(parentalRating))
					{
						hashtable.Add(parentalRating, shellObjectParent.Properties.CreateTypedProperty<string>(parentalRating));
					}
					return hashtable[parentalRating] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ParentalRatingReason
			{
				get
				{
					PropertyKey parentalRatingReason = SystemProperties.System.ParentalRatingReason;
					if (!hashtable.ContainsKey(parentalRatingReason))
					{
						hashtable.Add(parentalRatingReason, shellObjectParent.Properties.CreateTypedProperty<string>(parentalRatingReason));
					}
					return hashtable[parentalRatingReason] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ParentalRatingsOrganization
			{
				get
				{
					PropertyKey parentalRatingsOrganization = SystemProperties.System.ParentalRatingsOrganization;
					if (!hashtable.ContainsKey(parentalRatingsOrganization))
					{
						hashtable.Add(parentalRatingsOrganization, shellObjectParent.Properties.CreateTypedProperty<string>(parentalRatingsOrganization));
					}
					return hashtable[parentalRatingsOrganization] as ShellProperty<string>;
				}
			}

			public ShellProperty<object> ParsingBindContext
			{
				get
				{
					PropertyKey parsingBindContext = SystemProperties.System.ParsingBindContext;
					if (!hashtable.ContainsKey(parsingBindContext))
					{
						hashtable.Add(parsingBindContext, shellObjectParent.Properties.CreateTypedProperty<object>(parsingBindContext));
					}
					return hashtable[parsingBindContext] as ShellProperty<object>;
				}
			}

			public ShellProperty<string> ParsingName
			{
				get
				{
					PropertyKey parsingName = SystemProperties.System.ParsingName;
					if (!hashtable.ContainsKey(parsingName))
					{
						hashtable.Add(parsingName, shellObjectParent.Properties.CreateTypedProperty<string>(parsingName));
					}
					return hashtable[parsingName] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ParsingPath
			{
				get
				{
					PropertyKey parsingPath = SystemProperties.System.ParsingPath;
					if (!hashtable.ContainsKey(parsingPath))
					{
						hashtable.Add(parsingPath, shellObjectParent.Properties.CreateTypedProperty<string>(parsingPath));
					}
					return hashtable[parsingPath] as ShellProperty<string>;
				}
			}

			public ShellProperty<int?> PerceivedType
			{
				get
				{
					PropertyKey perceivedType = SystemProperties.System.PerceivedType;
					if (!hashtable.ContainsKey(perceivedType))
					{
						hashtable.Add(perceivedType, shellObjectParent.Properties.CreateTypedProperty<int?>(perceivedType));
					}
					return hashtable[perceivedType] as ShellProperty<int?>;
				}
			}

			public ShellProperty<uint?> PercentFull
			{
				get
				{
					PropertyKey percentFull = SystemProperties.System.PercentFull;
					if (!hashtable.ContainsKey(percentFull))
					{
						hashtable.Add(percentFull, shellObjectParent.Properties.CreateTypedProperty<uint?>(percentFull));
					}
					return hashtable[percentFull] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<ushort?> Priority
			{
				get
				{
					PropertyKey priority = SystemProperties.System.Priority;
					if (!hashtable.ContainsKey(priority))
					{
						hashtable.Add(priority, shellObjectParent.Properties.CreateTypedProperty<ushort?>(priority));
					}
					return hashtable[priority] as ShellProperty<ushort?>;
				}
			}

			public ShellProperty<string> PriorityText
			{
				get
				{
					PropertyKey priorityText = SystemProperties.System.PriorityText;
					if (!hashtable.ContainsKey(priorityText))
					{
						hashtable.Add(priorityText, shellObjectParent.Properties.CreateTypedProperty<string>(priorityText));
					}
					return hashtable[priorityText] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> Project
			{
				get
				{
					PropertyKey project = SystemProperties.System.Project;
					if (!hashtable.ContainsKey(project))
					{
						hashtable.Add(project, shellObjectParent.Properties.CreateTypedProperty<string>(project));
					}
					return hashtable[project] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ProviderItemID
			{
				get
				{
					PropertyKey providerItemID = SystemProperties.System.ProviderItemID;
					if (!hashtable.ContainsKey(providerItemID))
					{
						hashtable.Add(providerItemID, shellObjectParent.Properties.CreateTypedProperty<string>(providerItemID));
					}
					return hashtable[providerItemID] as ShellProperty<string>;
				}
			}

			public ShellProperty<uint?> Rating
			{
				get
				{
					PropertyKey rating = SystemProperties.System.Rating;
					if (!hashtable.ContainsKey(rating))
					{
						hashtable.Add(rating, shellObjectParent.Properties.CreateTypedProperty<uint?>(rating));
					}
					return hashtable[rating] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string> RatingText
			{
				get
				{
					PropertyKey ratingText = SystemProperties.System.RatingText;
					if (!hashtable.ContainsKey(ratingText))
					{
						hashtable.Add(ratingText, shellObjectParent.Properties.CreateTypedProperty<string>(ratingText));
					}
					return hashtable[ratingText] as ShellProperty<string>;
				}
			}

			public ShellProperty<ushort?> Sensitivity
			{
				get
				{
					PropertyKey sensitivity = SystemProperties.System.Sensitivity;
					if (!hashtable.ContainsKey(sensitivity))
					{
						hashtable.Add(sensitivity, shellObjectParent.Properties.CreateTypedProperty<ushort?>(sensitivity));
					}
					return hashtable[sensitivity] as ShellProperty<ushort?>;
				}
			}

			public ShellProperty<string> SensitivityText
			{
				get
				{
					PropertyKey sensitivityText = SystemProperties.System.SensitivityText;
					if (!hashtable.ContainsKey(sensitivityText))
					{
						hashtable.Add(sensitivityText, shellObjectParent.Properties.CreateTypedProperty<string>(sensitivityText));
					}
					return hashtable[sensitivityText] as ShellProperty<string>;
				}
			}

			public ShellProperty<uint?> SFGAOFlags
			{
				get
				{
					PropertyKey sFGAOFlags = SystemProperties.System.SFGAOFlags;
					if (!hashtable.ContainsKey(sFGAOFlags))
					{
						hashtable.Add(sFGAOFlags, shellObjectParent.Properties.CreateTypedProperty<uint?>(sFGAOFlags));
					}
					return hashtable[sFGAOFlags] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string[]> SharedWith
			{
				get
				{
					PropertyKey sharedWith = SystemProperties.System.SharedWith;
					if (!hashtable.ContainsKey(sharedWith))
					{
						hashtable.Add(sharedWith, shellObjectParent.Properties.CreateTypedProperty<string[]>(sharedWith));
					}
					return hashtable[sharedWith] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<uint?> ShareUserRating
			{
				get
				{
					PropertyKey shareUserRating = SystemProperties.System.ShareUserRating;
					if (!hashtable.ContainsKey(shareUserRating))
					{
						hashtable.Add(shareUserRating, shellObjectParent.Properties.CreateTypedProperty<uint?>(shareUserRating));
					}
					return hashtable[shareUserRating] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> SharingStatus
			{
				get
				{
					PropertyKey sharingStatus = SystemProperties.System.SharingStatus;
					if (!hashtable.ContainsKey(sharingStatus))
					{
						hashtable.Add(sharingStatus, shellObjectParent.Properties.CreateTypedProperty<uint?>(sharingStatus));
					}
					return hashtable[sharingStatus] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> SimpleRating
			{
				get
				{
					PropertyKey simpleRating = SystemProperties.System.SimpleRating;
					if (!hashtable.ContainsKey(simpleRating))
					{
						hashtable.Add(simpleRating, shellObjectParent.Properties.CreateTypedProperty<uint?>(simpleRating));
					}
					return hashtable[simpleRating] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<ulong?> Size
			{
				get
				{
					PropertyKey size = SystemProperties.System.Size;
					if (!hashtable.ContainsKey(size))
					{
						hashtable.Add(size, shellObjectParent.Properties.CreateTypedProperty<ulong?>(size));
					}
					return hashtable[size] as ShellProperty<ulong?>;
				}
			}

			public ShellProperty<string> SoftwareUsed
			{
				get
				{
					PropertyKey softwareUsed = SystemProperties.System.SoftwareUsed;
					if (!hashtable.ContainsKey(softwareUsed))
					{
						hashtable.Add(softwareUsed, shellObjectParent.Properties.CreateTypedProperty<string>(softwareUsed));
					}
					return hashtable[softwareUsed] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> SourceItem
			{
				get
				{
					PropertyKey sourceItem = SystemProperties.System.SourceItem;
					if (!hashtable.ContainsKey(sourceItem))
					{
						hashtable.Add(sourceItem, shellObjectParent.Properties.CreateTypedProperty<string>(sourceItem));
					}
					return hashtable[sourceItem] as ShellProperty<string>;
				}
			}

			public ShellProperty<DateTime?> StartDate
			{
				get
				{
					PropertyKey startDate = SystemProperties.System.StartDate;
					if (!hashtable.ContainsKey(startDate))
					{
						hashtable.Add(startDate, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(startDate));
					}
					return hashtable[startDate] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<string> Status
			{
				get
				{
					PropertyKey status = SystemProperties.System.Status;
					if (!hashtable.ContainsKey(status))
					{
						hashtable.Add(status, shellObjectParent.Properties.CreateTypedProperty<string>(status));
					}
					return hashtable[status] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> Subject
			{
				get
				{
					PropertyKey subject = SystemProperties.System.Subject;
					if (!hashtable.ContainsKey(subject))
					{
						hashtable.Add(subject, shellObjectParent.Properties.CreateTypedProperty<string>(subject));
					}
					return hashtable[subject] as ShellProperty<string>;
				}
			}

			public ShellProperty<IntPtr?> Thumbnail
			{
				get
				{
					PropertyKey thumbnail = SystemProperties.System.Thumbnail;
					if (!hashtable.ContainsKey(thumbnail))
					{
						hashtable.Add(thumbnail, shellObjectParent.Properties.CreateTypedProperty<IntPtr?>(thumbnail));
					}
					return hashtable[thumbnail] as ShellProperty<IntPtr?>;
				}
			}

			public ShellProperty<ulong?> ThumbnailCacheId
			{
				get
				{
					PropertyKey thumbnailCacheId = SystemProperties.System.ThumbnailCacheId;
					if (!hashtable.ContainsKey(thumbnailCacheId))
					{
						hashtable.Add(thumbnailCacheId, shellObjectParent.Properties.CreateTypedProperty<ulong?>(thumbnailCacheId));
					}
					return hashtable[thumbnailCacheId] as ShellProperty<ulong?>;
				}
			}

			public ShellProperty<IStream> ThumbnailStream
			{
				get
				{
					PropertyKey thumbnailStream = SystemProperties.System.ThumbnailStream;
					if (!hashtable.ContainsKey(thumbnailStream))
					{
						hashtable.Add(thumbnailStream, shellObjectParent.Properties.CreateTypedProperty<IStream>(thumbnailStream));
					}
					return hashtable[thumbnailStream] as ShellProperty<IStream>;
				}
			}

			public ShellProperty<string> Title
			{
				get
				{
					PropertyKey title = SystemProperties.System.Title;
					if (!hashtable.ContainsKey(title))
					{
						hashtable.Add(title, shellObjectParent.Properties.CreateTypedProperty<string>(title));
					}
					return hashtable[title] as ShellProperty<string>;
				}
			}

			public ShellProperty<ulong?> TotalFileSize
			{
				get
				{
					PropertyKey totalFileSize = SystemProperties.System.TotalFileSize;
					if (!hashtable.ContainsKey(totalFileSize))
					{
						hashtable.Add(totalFileSize, shellObjectParent.Properties.CreateTypedProperty<ulong?>(totalFileSize));
					}
					return hashtable[totalFileSize] as ShellProperty<ulong?>;
				}
			}

			public ShellProperty<string> Trademarks
			{
				get
				{
					PropertyKey trademarks = SystemProperties.System.Trademarks;
					if (!hashtable.ContainsKey(trademarks))
					{
						hashtable.Add(trademarks, shellObjectParent.Properties.CreateTypedProperty<string>(trademarks));
					}
					return hashtable[trademarks] as ShellProperty<string>;
				}
			}

			public PropertySystemAppUserModel AppUserModel
			{
				get
				{
					if (internalPropertySystemAppUserModel == null)
					{
						internalPropertySystemAppUserModel = new PropertySystemAppUserModel(shellObjectParent);
					}
					return internalPropertySystemAppUserModel;
				}
			}

			public PropertySystemAudio Audio
			{
				get
				{
					if (internalPropertySystemAudio == null)
					{
						internalPropertySystemAudio = new PropertySystemAudio(shellObjectParent);
					}
					return internalPropertySystemAudio;
				}
			}

			public PropertySystemCalendar Calendar
			{
				get
				{
					if (internalPropertySystemCalendar == null)
					{
						internalPropertySystemCalendar = new PropertySystemCalendar(shellObjectParent);
					}
					return internalPropertySystemCalendar;
				}
			}

			public PropertySystemCommunication Communication
			{
				get
				{
					if (internalPropertySystemCommunication == null)
					{
						internalPropertySystemCommunication = new PropertySystemCommunication(shellObjectParent);
					}
					return internalPropertySystemCommunication;
				}
			}

			public PropertySystemComputer Computer
			{
				get
				{
					if (internalPropertySystemComputer == null)
					{
						internalPropertySystemComputer = new PropertySystemComputer(shellObjectParent);
					}
					return internalPropertySystemComputer;
				}
			}

			public PropertySystemContact Contact
			{
				get
				{
					if (internalPropertySystemContact == null)
					{
						internalPropertySystemContact = new PropertySystemContact(shellObjectParent);
					}
					return internalPropertySystemContact;
				}
			}

			public PropertySystemDevice Device
			{
				get
				{
					if (internalPropertySystemDevice == null)
					{
						internalPropertySystemDevice = new PropertySystemDevice(shellObjectParent);
					}
					return internalPropertySystemDevice;
				}
			}

			public PropertySystemDeviceInterface DeviceInterface
			{
				get
				{
					if (internalPropertySystemDeviceInterface == null)
					{
						internalPropertySystemDeviceInterface = new PropertySystemDeviceInterface(shellObjectParent);
					}
					return internalPropertySystemDeviceInterface;
				}
			}

			public PropertySystemDevices Devices
			{
				get
				{
					if (internalPropertySystemDevices == null)
					{
						internalPropertySystemDevices = new PropertySystemDevices(shellObjectParent);
					}
					return internalPropertySystemDevices;
				}
			}

			public PropertySystemDocument Document
			{
				get
				{
					if (internalPropertySystemDocument == null)
					{
						internalPropertySystemDocument = new PropertySystemDocument(shellObjectParent);
					}
					return internalPropertySystemDocument;
				}
			}

			public PropertySystemDRM DRM
			{
				get
				{
					if (internalPropertySystemDRM == null)
					{
						internalPropertySystemDRM = new PropertySystemDRM(shellObjectParent);
					}
					return internalPropertySystemDRM;
				}
			}

			public PropertySystemGPS GPS
			{
				get
				{
					if (internalPropertySystemGPS == null)
					{
						internalPropertySystemGPS = new PropertySystemGPS(shellObjectParent);
					}
					return internalPropertySystemGPS;
				}
			}

			public PropertySystemIdentity Identity
			{
				get
				{
					if (internalPropertySystemIdentity == null)
					{
						internalPropertySystemIdentity = new PropertySystemIdentity(shellObjectParent);
					}
					return internalPropertySystemIdentity;
				}
			}

			public PropertySystemIdentityProvider IdentityProvider
			{
				get
				{
					if (internalPropertySystemIdentityProvider == null)
					{
						internalPropertySystemIdentityProvider = new PropertySystemIdentityProvider(shellObjectParent);
					}
					return internalPropertySystemIdentityProvider;
				}
			}

			public PropertySystemImage Image
			{
				get
				{
					if (internalPropertySystemImage == null)
					{
						internalPropertySystemImage = new PropertySystemImage(shellObjectParent);
					}
					return internalPropertySystemImage;
				}
			}

			public PropertySystemJournal Journal
			{
				get
				{
					if (internalPropertySystemJournal == null)
					{
						internalPropertySystemJournal = new PropertySystemJournal(shellObjectParent);
					}
					return internalPropertySystemJournal;
				}
			}

			public PropertySystemLayoutPattern LayoutPattern
			{
				get
				{
					if (internalPropertySystemLayoutPattern == null)
					{
						internalPropertySystemLayoutPattern = new PropertySystemLayoutPattern(shellObjectParent);
					}
					return internalPropertySystemLayoutPattern;
				}
			}

			public PropertySystemLink Link
			{
				get
				{
					if (internalPropertySystemLink == null)
					{
						internalPropertySystemLink = new PropertySystemLink(shellObjectParent);
					}
					return internalPropertySystemLink;
				}
			}

			public PropertySystemMedia Media
			{
				get
				{
					if (internalPropertySystemMedia == null)
					{
						internalPropertySystemMedia = new PropertySystemMedia(shellObjectParent);
					}
					return internalPropertySystemMedia;
				}
			}

			public PropertySystemMessage Message
			{
				get
				{
					if (internalPropertySystemMessage == null)
					{
						internalPropertySystemMessage = new PropertySystemMessage(shellObjectParent);
					}
					return internalPropertySystemMessage;
				}
			}

			public PropertySystemMusic Music
			{
				get
				{
					if (internalPropertySystemMusic == null)
					{
						internalPropertySystemMusic = new PropertySystemMusic(shellObjectParent);
					}
					return internalPropertySystemMusic;
				}
			}

			public PropertySystemNote Note
			{
				get
				{
					if (internalPropertySystemNote == null)
					{
						internalPropertySystemNote = new PropertySystemNote(shellObjectParent);
					}
					return internalPropertySystemNote;
				}
			}

			public PropertySystemPhoto Photo
			{
				get
				{
					if (internalPropertySystemPhoto == null)
					{
						internalPropertySystemPhoto = new PropertySystemPhoto(shellObjectParent);
					}
					return internalPropertySystemPhoto;
				}
			}

			public PropertySystemPropGroup PropGroup
			{
				get
				{
					if (internalPropertySystemPropGroup == null)
					{
						internalPropertySystemPropGroup = new PropertySystemPropGroup(shellObjectParent);
					}
					return internalPropertySystemPropGroup;
				}
			}

			public PropertySystemPropList PropList
			{
				get
				{
					if (internalPropertySystemPropList == null)
					{
						internalPropertySystemPropList = new PropertySystemPropList(shellObjectParent);
					}
					return internalPropertySystemPropList;
				}
			}

			public PropertySystemRecordedTV RecordedTV
			{
				get
				{
					if (internalPropertySystemRecordedTV == null)
					{
						internalPropertySystemRecordedTV = new PropertySystemRecordedTV(shellObjectParent);
					}
					return internalPropertySystemRecordedTV;
				}
			}

			public PropertySystemSearch Search
			{
				get
				{
					if (internalPropertySystemSearch == null)
					{
						internalPropertySystemSearch = new PropertySystemSearch(shellObjectParent);
					}
					return internalPropertySystemSearch;
				}
			}

			public PropertySystemShell Shell
			{
				get
				{
					if (internalPropertySystemShell == null)
					{
						internalPropertySystemShell = new PropertySystemShell(shellObjectParent);
					}
					return internalPropertySystemShell;
				}
			}

			public PropertySystemSoftware Software
			{
				get
				{
					if (internalPropertySystemSoftware == null)
					{
						internalPropertySystemSoftware = new PropertySystemSoftware(shellObjectParent);
					}
					return internalPropertySystemSoftware;
				}
			}

			public PropertySystemSync Sync
			{
				get
				{
					if (internalPropertySystemSync == null)
					{
						internalPropertySystemSync = new PropertySystemSync(shellObjectParent);
					}
					return internalPropertySystemSync;
				}
			}

			public PropertySystemTask Task
			{
				get
				{
					if (internalPropertySystemTask == null)
					{
						internalPropertySystemTask = new PropertySystemTask(shellObjectParent);
					}
					return internalPropertySystemTask;
				}
			}

			public PropertySystemVideo Video
			{
				get
				{
					if (internalPropertySystemVideo == null)
					{
						internalPropertySystemVideo = new PropertySystemVideo(shellObjectParent);
					}
					return internalPropertySystemVideo;
				}
			}

			public PropertySystemVolume Volume
			{
				get
				{
					if (internalPropertySystemVolume == null)
					{
						internalPropertySystemVolume = new PropertySystemVolume(shellObjectParent);
					}
					return internalPropertySystemVolume;
				}
			}

			internal PropertySystem(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemAppUserModel : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<bool?> ExcludeFromShowInNewInstall
			{
				get
				{
					PropertyKey excludeFromShowInNewInstall = SystemProperties.System.AppUserModel.ExcludeFromShowInNewInstall;
					if (!hashtable.ContainsKey(excludeFromShowInNewInstall))
					{
						hashtable.Add(excludeFromShowInNewInstall, shellObjectParent.Properties.CreateTypedProperty<bool?>(excludeFromShowInNewInstall));
					}
					return hashtable[excludeFromShowInNewInstall] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<string> ID
			{
				get
				{
					PropertyKey iD = SystemProperties.System.AppUserModel.ID;
					if (!hashtable.ContainsKey(iD))
					{
						hashtable.Add(iD, shellObjectParent.Properties.CreateTypedProperty<string>(iD));
					}
					return hashtable[iD] as ShellProperty<string>;
				}
			}

			public ShellProperty<bool?> IsDestinationListSeparator
			{
				get
				{
					PropertyKey isDestinationListSeparator = SystemProperties.System.AppUserModel.IsDestinationListSeparator;
					if (!hashtable.ContainsKey(isDestinationListSeparator))
					{
						hashtable.Add(isDestinationListSeparator, shellObjectParent.Properties.CreateTypedProperty<bool?>(isDestinationListSeparator));
					}
					return hashtable[isDestinationListSeparator] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> PreventPinning
			{
				get
				{
					PropertyKey preventPinning = SystemProperties.System.AppUserModel.PreventPinning;
					if (!hashtable.ContainsKey(preventPinning))
					{
						hashtable.Add(preventPinning, shellObjectParent.Properties.CreateTypedProperty<bool?>(preventPinning));
					}
					return hashtable[preventPinning] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<string> RelaunchCommand
			{
				get
				{
					PropertyKey relaunchCommand = SystemProperties.System.AppUserModel.RelaunchCommand;
					if (!hashtable.ContainsKey(relaunchCommand))
					{
						hashtable.Add(relaunchCommand, shellObjectParent.Properties.CreateTypedProperty<string>(relaunchCommand));
					}
					return hashtable[relaunchCommand] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> RelaunchDisplayNameResource
			{
				get
				{
					PropertyKey relaunchDisplayNameResource = SystemProperties.System.AppUserModel.RelaunchDisplayNameResource;
					if (!hashtable.ContainsKey(relaunchDisplayNameResource))
					{
						hashtable.Add(relaunchDisplayNameResource, shellObjectParent.Properties.CreateTypedProperty<string>(relaunchDisplayNameResource));
					}
					return hashtable[relaunchDisplayNameResource] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> RelaunchIconResource
			{
				get
				{
					PropertyKey relaunchIconResource = SystemProperties.System.AppUserModel.RelaunchIconResource;
					if (!hashtable.ContainsKey(relaunchIconResource))
					{
						hashtable.Add(relaunchIconResource, shellObjectParent.Properties.CreateTypedProperty<string>(relaunchIconResource));
					}
					return hashtable[relaunchIconResource] as ShellProperty<string>;
				}
			}

			internal PropertySystemAppUserModel(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemAudio : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<uint?> ChannelCount
			{
				get
				{
					PropertyKey channelCount = SystemProperties.System.Audio.ChannelCount;
					if (!hashtable.ContainsKey(channelCount))
					{
						hashtable.Add(channelCount, shellObjectParent.Properties.CreateTypedProperty<uint?>(channelCount));
					}
					return hashtable[channelCount] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string> Compression
			{
				get
				{
					PropertyKey compression = SystemProperties.System.Audio.Compression;
					if (!hashtable.ContainsKey(compression))
					{
						hashtable.Add(compression, shellObjectParent.Properties.CreateTypedProperty<string>(compression));
					}
					return hashtable[compression] as ShellProperty<string>;
				}
			}

			public ShellProperty<uint?> EncodingBitrate
			{
				get
				{
					PropertyKey encodingBitrate = SystemProperties.System.Audio.EncodingBitrate;
					if (!hashtable.ContainsKey(encodingBitrate))
					{
						hashtable.Add(encodingBitrate, shellObjectParent.Properties.CreateTypedProperty<uint?>(encodingBitrate));
					}
					return hashtable[encodingBitrate] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string> Format
			{
				get
				{
					PropertyKey format = SystemProperties.System.Audio.Format;
					if (!hashtable.ContainsKey(format))
					{
						hashtable.Add(format, shellObjectParent.Properties.CreateTypedProperty<string>(format));
					}
					return hashtable[format] as ShellProperty<string>;
				}
			}

			public ShellProperty<bool?> IsVariableBitrate
			{
				get
				{
					PropertyKey isVariableBitrate = SystemProperties.System.Audio.IsVariableBitrate;
					if (!hashtable.ContainsKey(isVariableBitrate))
					{
						hashtable.Add(isVariableBitrate, shellObjectParent.Properties.CreateTypedProperty<bool?>(isVariableBitrate));
					}
					return hashtable[isVariableBitrate] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<uint?> PeakValue
			{
				get
				{
					PropertyKey peakValue = SystemProperties.System.Audio.PeakValue;
					if (!hashtable.ContainsKey(peakValue))
					{
						hashtable.Add(peakValue, shellObjectParent.Properties.CreateTypedProperty<uint?>(peakValue));
					}
					return hashtable[peakValue] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> SampleRate
			{
				get
				{
					PropertyKey sampleRate = SystemProperties.System.Audio.SampleRate;
					if (!hashtable.ContainsKey(sampleRate))
					{
						hashtable.Add(sampleRate, shellObjectParent.Properties.CreateTypedProperty<uint?>(sampleRate));
					}
					return hashtable[sampleRate] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> SampleSize
			{
				get
				{
					PropertyKey sampleSize = SystemProperties.System.Audio.SampleSize;
					if (!hashtable.ContainsKey(sampleSize))
					{
						hashtable.Add(sampleSize, shellObjectParent.Properties.CreateTypedProperty<uint?>(sampleSize));
					}
					return hashtable[sampleSize] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string> StreamName
			{
				get
				{
					PropertyKey streamName = SystemProperties.System.Audio.StreamName;
					if (!hashtable.ContainsKey(streamName))
					{
						hashtable.Add(streamName, shellObjectParent.Properties.CreateTypedProperty<string>(streamName));
					}
					return hashtable[streamName] as ShellProperty<string>;
				}
			}

			public ShellProperty<ushort?> StreamNumber
			{
				get
				{
					PropertyKey streamNumber = SystemProperties.System.Audio.StreamNumber;
					if (!hashtable.ContainsKey(streamNumber))
					{
						hashtable.Add(streamNumber, shellObjectParent.Properties.CreateTypedProperty<ushort?>(streamNumber));
					}
					return hashtable[streamNumber] as ShellProperty<ushort?>;
				}
			}

			internal PropertySystemAudio(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemCalendar : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<string> Duration
			{
				get
				{
					PropertyKey duration = SystemProperties.System.Calendar.Duration;
					if (!hashtable.ContainsKey(duration))
					{
						hashtable.Add(duration, shellObjectParent.Properties.CreateTypedProperty<string>(duration));
					}
					return hashtable[duration] as ShellProperty<string>;
				}
			}

			public ShellProperty<bool?> IsOnline
			{
				get
				{
					PropertyKey isOnline = SystemProperties.System.Calendar.IsOnline;
					if (!hashtable.ContainsKey(isOnline))
					{
						hashtable.Add(isOnline, shellObjectParent.Properties.CreateTypedProperty<bool?>(isOnline));
					}
					return hashtable[isOnline] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> IsRecurring
			{
				get
				{
					PropertyKey isRecurring = SystemProperties.System.Calendar.IsRecurring;
					if (!hashtable.ContainsKey(isRecurring))
					{
						hashtable.Add(isRecurring, shellObjectParent.Properties.CreateTypedProperty<bool?>(isRecurring));
					}
					return hashtable[isRecurring] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<string> Location
			{
				get
				{
					PropertyKey location = SystemProperties.System.Calendar.Location;
					if (!hashtable.ContainsKey(location))
					{
						hashtable.Add(location, shellObjectParent.Properties.CreateTypedProperty<string>(location));
					}
					return hashtable[location] as ShellProperty<string>;
				}
			}

			public ShellProperty<string[]> OptionalAttendeeAddresses
			{
				get
				{
					PropertyKey optionalAttendeeAddresses = SystemProperties.System.Calendar.OptionalAttendeeAddresses;
					if (!hashtable.ContainsKey(optionalAttendeeAddresses))
					{
						hashtable.Add(optionalAttendeeAddresses, shellObjectParent.Properties.CreateTypedProperty<string[]>(optionalAttendeeAddresses));
					}
					return hashtable[optionalAttendeeAddresses] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string[]> OptionalAttendeeNames
			{
				get
				{
					PropertyKey optionalAttendeeNames = SystemProperties.System.Calendar.OptionalAttendeeNames;
					if (!hashtable.ContainsKey(optionalAttendeeNames))
					{
						hashtable.Add(optionalAttendeeNames, shellObjectParent.Properties.CreateTypedProperty<string[]>(optionalAttendeeNames));
					}
					return hashtable[optionalAttendeeNames] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string> OrganizerAddress
			{
				get
				{
					PropertyKey organizerAddress = SystemProperties.System.Calendar.OrganizerAddress;
					if (!hashtable.ContainsKey(organizerAddress))
					{
						hashtable.Add(organizerAddress, shellObjectParent.Properties.CreateTypedProperty<string>(organizerAddress));
					}
					return hashtable[organizerAddress] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> OrganizerName
			{
				get
				{
					PropertyKey organizerName = SystemProperties.System.Calendar.OrganizerName;
					if (!hashtable.ContainsKey(organizerName))
					{
						hashtable.Add(organizerName, shellObjectParent.Properties.CreateTypedProperty<string>(organizerName));
					}
					return hashtable[organizerName] as ShellProperty<string>;
				}
			}

			public ShellProperty<DateTime?> ReminderTime
			{
				get
				{
					PropertyKey reminderTime = SystemProperties.System.Calendar.ReminderTime;
					if (!hashtable.ContainsKey(reminderTime))
					{
						hashtable.Add(reminderTime, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(reminderTime));
					}
					return hashtable[reminderTime] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<string[]> RequiredAttendeeAddresses
			{
				get
				{
					PropertyKey requiredAttendeeAddresses = SystemProperties.System.Calendar.RequiredAttendeeAddresses;
					if (!hashtable.ContainsKey(requiredAttendeeAddresses))
					{
						hashtable.Add(requiredAttendeeAddresses, shellObjectParent.Properties.CreateTypedProperty<string[]>(requiredAttendeeAddresses));
					}
					return hashtable[requiredAttendeeAddresses] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string[]> RequiredAttendeeNames
			{
				get
				{
					PropertyKey requiredAttendeeNames = SystemProperties.System.Calendar.RequiredAttendeeNames;
					if (!hashtable.ContainsKey(requiredAttendeeNames))
					{
						hashtable.Add(requiredAttendeeNames, shellObjectParent.Properties.CreateTypedProperty<string[]>(requiredAttendeeNames));
					}
					return hashtable[requiredAttendeeNames] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string[]> Resources
			{
				get
				{
					PropertyKey resources = SystemProperties.System.Calendar.Resources;
					if (!hashtable.ContainsKey(resources))
					{
						hashtable.Add(resources, shellObjectParent.Properties.CreateTypedProperty<string[]>(resources));
					}
					return hashtable[resources] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<ushort?> ResponseStatus
			{
				get
				{
					PropertyKey responseStatus = SystemProperties.System.Calendar.ResponseStatus;
					if (!hashtable.ContainsKey(responseStatus))
					{
						hashtable.Add(responseStatus, shellObjectParent.Properties.CreateTypedProperty<ushort?>(responseStatus));
					}
					return hashtable[responseStatus] as ShellProperty<ushort?>;
				}
			}

			public ShellProperty<ushort?> ShowTimeAs
			{
				get
				{
					PropertyKey showTimeAs = SystemProperties.System.Calendar.ShowTimeAs;
					if (!hashtable.ContainsKey(showTimeAs))
					{
						hashtable.Add(showTimeAs, shellObjectParent.Properties.CreateTypedProperty<ushort?>(showTimeAs));
					}
					return hashtable[showTimeAs] as ShellProperty<ushort?>;
				}
			}

			public ShellProperty<string> ShowTimeAsText
			{
				get
				{
					PropertyKey showTimeAsText = SystemProperties.System.Calendar.ShowTimeAsText;
					if (!hashtable.ContainsKey(showTimeAsText))
					{
						hashtable.Add(showTimeAsText, shellObjectParent.Properties.CreateTypedProperty<string>(showTimeAsText));
					}
					return hashtable[showTimeAsText] as ShellProperty<string>;
				}
			}

			internal PropertySystemCalendar(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemCommunication : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<string> AccountName
			{
				get
				{
					PropertyKey accountName = SystemProperties.System.Communication.AccountName;
					if (!hashtable.ContainsKey(accountName))
					{
						hashtable.Add(accountName, shellObjectParent.Properties.CreateTypedProperty<string>(accountName));
					}
					return hashtable[accountName] as ShellProperty<string>;
				}
			}

			public ShellProperty<DateTime?> DateItemExpires
			{
				get
				{
					PropertyKey dateItemExpires = SystemProperties.System.Communication.DateItemExpires;
					if (!hashtable.ContainsKey(dateItemExpires))
					{
						hashtable.Add(dateItemExpires, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(dateItemExpires));
					}
					return hashtable[dateItemExpires] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<int?> FollowUpIconIndex
			{
				get
				{
					PropertyKey followUpIconIndex = SystemProperties.System.Communication.FollowUpIconIndex;
					if (!hashtable.ContainsKey(followUpIconIndex))
					{
						hashtable.Add(followUpIconIndex, shellObjectParent.Properties.CreateTypedProperty<int?>(followUpIconIndex));
					}
					return hashtable[followUpIconIndex] as ShellProperty<int?>;
				}
			}

			public ShellProperty<bool?> HeaderItem
			{
				get
				{
					PropertyKey headerItem = SystemProperties.System.Communication.HeaderItem;
					if (!hashtable.ContainsKey(headerItem))
					{
						hashtable.Add(headerItem, shellObjectParent.Properties.CreateTypedProperty<bool?>(headerItem));
					}
					return hashtable[headerItem] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<string> PolicyTag
			{
				get
				{
					PropertyKey policyTag = SystemProperties.System.Communication.PolicyTag;
					if (!hashtable.ContainsKey(policyTag))
					{
						hashtable.Add(policyTag, shellObjectParent.Properties.CreateTypedProperty<string>(policyTag));
					}
					return hashtable[policyTag] as ShellProperty<string>;
				}
			}

			public ShellProperty<int?> SecurityFlags
			{
				get
				{
					PropertyKey securityFlags = SystemProperties.System.Communication.SecurityFlags;
					if (!hashtable.ContainsKey(securityFlags))
					{
						hashtable.Add(securityFlags, shellObjectParent.Properties.CreateTypedProperty<int?>(securityFlags));
					}
					return hashtable[securityFlags] as ShellProperty<int?>;
				}
			}

			public ShellProperty<string> Suffix
			{
				get
				{
					PropertyKey suffix = SystemProperties.System.Communication.Suffix;
					if (!hashtable.ContainsKey(suffix))
					{
						hashtable.Add(suffix, shellObjectParent.Properties.CreateTypedProperty<string>(suffix));
					}
					return hashtable[suffix] as ShellProperty<string>;
				}
			}

			public ShellProperty<ushort?> TaskStatus
			{
				get
				{
					PropertyKey taskStatus = SystemProperties.System.Communication.TaskStatus;
					if (!hashtable.ContainsKey(taskStatus))
					{
						hashtable.Add(taskStatus, shellObjectParent.Properties.CreateTypedProperty<ushort?>(taskStatus));
					}
					return hashtable[taskStatus] as ShellProperty<ushort?>;
				}
			}

			public ShellProperty<string> TaskStatusText
			{
				get
				{
					PropertyKey taskStatusText = SystemProperties.System.Communication.TaskStatusText;
					if (!hashtable.ContainsKey(taskStatusText))
					{
						hashtable.Add(taskStatusText, shellObjectParent.Properties.CreateTypedProperty<string>(taskStatusText));
					}
					return hashtable[taskStatusText] as ShellProperty<string>;
				}
			}

			internal PropertySystemCommunication(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemComputer : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<ulong[]> DecoratedFreeSpace
			{
				get
				{
					PropertyKey decoratedFreeSpace = SystemProperties.System.Computer.DecoratedFreeSpace;
					if (!hashtable.ContainsKey(decoratedFreeSpace))
					{
						hashtable.Add(decoratedFreeSpace, shellObjectParent.Properties.CreateTypedProperty<ulong[]>(decoratedFreeSpace));
					}
					return hashtable[decoratedFreeSpace] as ShellProperty<ulong[]>;
				}
			}

			internal PropertySystemComputer(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemContact : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			private PropertyContactJA internalPropertyContactJA;

			public ShellProperty<DateTime?> Anniversary
			{
				get
				{
					PropertyKey anniversary = SystemProperties.System.Contact.Anniversary;
					if (!hashtable.ContainsKey(anniversary))
					{
						hashtable.Add(anniversary, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(anniversary));
					}
					return hashtable[anniversary] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<string> AssistantName
			{
				get
				{
					PropertyKey assistantName = SystemProperties.System.Contact.AssistantName;
					if (!hashtable.ContainsKey(assistantName))
					{
						hashtable.Add(assistantName, shellObjectParent.Properties.CreateTypedProperty<string>(assistantName));
					}
					return hashtable[assistantName] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> AssistantTelephone
			{
				get
				{
					PropertyKey assistantTelephone = SystemProperties.System.Contact.AssistantTelephone;
					if (!hashtable.ContainsKey(assistantTelephone))
					{
						hashtable.Add(assistantTelephone, shellObjectParent.Properties.CreateTypedProperty<string>(assistantTelephone));
					}
					return hashtable[assistantTelephone] as ShellProperty<string>;
				}
			}

			public ShellProperty<DateTime?> Birthday
			{
				get
				{
					PropertyKey birthday = SystemProperties.System.Contact.Birthday;
					if (!hashtable.ContainsKey(birthday))
					{
						hashtable.Add(birthday, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(birthday));
					}
					return hashtable[birthday] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<string> BusinessAddress
			{
				get
				{
					PropertyKey businessAddress = SystemProperties.System.Contact.BusinessAddress;
					if (!hashtable.ContainsKey(businessAddress))
					{
						hashtable.Add(businessAddress, shellObjectParent.Properties.CreateTypedProperty<string>(businessAddress));
					}
					return hashtable[businessAddress] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> BusinessAddressCity
			{
				get
				{
					PropertyKey businessAddressCity = SystemProperties.System.Contact.BusinessAddressCity;
					if (!hashtable.ContainsKey(businessAddressCity))
					{
						hashtable.Add(businessAddressCity, shellObjectParent.Properties.CreateTypedProperty<string>(businessAddressCity));
					}
					return hashtable[businessAddressCity] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> BusinessAddressCountry
			{
				get
				{
					PropertyKey businessAddressCountry = SystemProperties.System.Contact.BusinessAddressCountry;
					if (!hashtable.ContainsKey(businessAddressCountry))
					{
						hashtable.Add(businessAddressCountry, shellObjectParent.Properties.CreateTypedProperty<string>(businessAddressCountry));
					}
					return hashtable[businessAddressCountry] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> BusinessAddressPostalCode
			{
				get
				{
					PropertyKey businessAddressPostalCode = SystemProperties.System.Contact.BusinessAddressPostalCode;
					if (!hashtable.ContainsKey(businessAddressPostalCode))
					{
						hashtable.Add(businessAddressPostalCode, shellObjectParent.Properties.CreateTypedProperty<string>(businessAddressPostalCode));
					}
					return hashtable[businessAddressPostalCode] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> BusinessAddressPostOfficeBox
			{
				get
				{
					PropertyKey businessAddressPostOfficeBox = SystemProperties.System.Contact.BusinessAddressPostOfficeBox;
					if (!hashtable.ContainsKey(businessAddressPostOfficeBox))
					{
						hashtable.Add(businessAddressPostOfficeBox, shellObjectParent.Properties.CreateTypedProperty<string>(businessAddressPostOfficeBox));
					}
					return hashtable[businessAddressPostOfficeBox] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> BusinessAddressState
			{
				get
				{
					PropertyKey businessAddressState = SystemProperties.System.Contact.BusinessAddressState;
					if (!hashtable.ContainsKey(businessAddressState))
					{
						hashtable.Add(businessAddressState, shellObjectParent.Properties.CreateTypedProperty<string>(businessAddressState));
					}
					return hashtable[businessAddressState] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> BusinessAddressStreet
			{
				get
				{
					PropertyKey businessAddressStreet = SystemProperties.System.Contact.BusinessAddressStreet;
					if (!hashtable.ContainsKey(businessAddressStreet))
					{
						hashtable.Add(businessAddressStreet, shellObjectParent.Properties.CreateTypedProperty<string>(businessAddressStreet));
					}
					return hashtable[businessAddressStreet] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> BusinessFaxNumber
			{
				get
				{
					PropertyKey businessFaxNumber = SystemProperties.System.Contact.BusinessFaxNumber;
					if (!hashtable.ContainsKey(businessFaxNumber))
					{
						hashtable.Add(businessFaxNumber, shellObjectParent.Properties.CreateTypedProperty<string>(businessFaxNumber));
					}
					return hashtable[businessFaxNumber] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> BusinessHomepage
			{
				get
				{
					PropertyKey businessHomepage = SystemProperties.System.Contact.BusinessHomepage;
					if (!hashtable.ContainsKey(businessHomepage))
					{
						hashtable.Add(businessHomepage, shellObjectParent.Properties.CreateTypedProperty<string>(businessHomepage));
					}
					return hashtable[businessHomepage] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> BusinessTelephone
			{
				get
				{
					PropertyKey businessTelephone = SystemProperties.System.Contact.BusinessTelephone;
					if (!hashtable.ContainsKey(businessTelephone))
					{
						hashtable.Add(businessTelephone, shellObjectParent.Properties.CreateTypedProperty<string>(businessTelephone));
					}
					return hashtable[businessTelephone] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> CallbackTelephone
			{
				get
				{
					PropertyKey callbackTelephone = SystemProperties.System.Contact.CallbackTelephone;
					if (!hashtable.ContainsKey(callbackTelephone))
					{
						hashtable.Add(callbackTelephone, shellObjectParent.Properties.CreateTypedProperty<string>(callbackTelephone));
					}
					return hashtable[callbackTelephone] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> CarTelephone
			{
				get
				{
					PropertyKey carTelephone = SystemProperties.System.Contact.CarTelephone;
					if (!hashtable.ContainsKey(carTelephone))
					{
						hashtable.Add(carTelephone, shellObjectParent.Properties.CreateTypedProperty<string>(carTelephone));
					}
					return hashtable[carTelephone] as ShellProperty<string>;
				}
			}

			public ShellProperty<string[]> Children
			{
				get
				{
					PropertyKey children = SystemProperties.System.Contact.Children;
					if (!hashtable.ContainsKey(children))
					{
						hashtable.Add(children, shellObjectParent.Properties.CreateTypedProperty<string[]>(children));
					}
					return hashtable[children] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string> CompanyMainTelephone
			{
				get
				{
					PropertyKey companyMainTelephone = SystemProperties.System.Contact.CompanyMainTelephone;
					if (!hashtable.ContainsKey(companyMainTelephone))
					{
						hashtable.Add(companyMainTelephone, shellObjectParent.Properties.CreateTypedProperty<string>(companyMainTelephone));
					}
					return hashtable[companyMainTelephone] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> Department
			{
				get
				{
					PropertyKey department = SystemProperties.System.Contact.Department;
					if (!hashtable.ContainsKey(department))
					{
						hashtable.Add(department, shellObjectParent.Properties.CreateTypedProperty<string>(department));
					}
					return hashtable[department] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> EmailAddress
			{
				get
				{
					PropertyKey emailAddress = SystemProperties.System.Contact.EmailAddress;
					if (!hashtable.ContainsKey(emailAddress))
					{
						hashtable.Add(emailAddress, shellObjectParent.Properties.CreateTypedProperty<string>(emailAddress));
					}
					return hashtable[emailAddress] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> EmailAddress2
			{
				get
				{
					PropertyKey emailAddress = SystemProperties.System.Contact.EmailAddress2;
					if (!hashtable.ContainsKey(emailAddress))
					{
						hashtable.Add(emailAddress, shellObjectParent.Properties.CreateTypedProperty<string>(emailAddress));
					}
					return hashtable[emailAddress] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> EmailAddress3
			{
				get
				{
					PropertyKey emailAddress = SystemProperties.System.Contact.EmailAddress3;
					if (!hashtable.ContainsKey(emailAddress))
					{
						hashtable.Add(emailAddress, shellObjectParent.Properties.CreateTypedProperty<string>(emailAddress));
					}
					return hashtable[emailAddress] as ShellProperty<string>;
				}
			}

			public ShellProperty<string[]> EmailAddresses
			{
				get
				{
					PropertyKey emailAddresses = SystemProperties.System.Contact.EmailAddresses;
					if (!hashtable.ContainsKey(emailAddresses))
					{
						hashtable.Add(emailAddresses, shellObjectParent.Properties.CreateTypedProperty<string[]>(emailAddresses));
					}
					return hashtable[emailAddresses] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string> EmailName
			{
				get
				{
					PropertyKey emailName = SystemProperties.System.Contact.EmailName;
					if (!hashtable.ContainsKey(emailName))
					{
						hashtable.Add(emailName, shellObjectParent.Properties.CreateTypedProperty<string>(emailName));
					}
					return hashtable[emailName] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> FileAsName
			{
				get
				{
					PropertyKey fileAsName = SystemProperties.System.Contact.FileAsName;
					if (!hashtable.ContainsKey(fileAsName))
					{
						hashtable.Add(fileAsName, shellObjectParent.Properties.CreateTypedProperty<string>(fileAsName));
					}
					return hashtable[fileAsName] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> FirstName
			{
				get
				{
					PropertyKey firstName = SystemProperties.System.Contact.FirstName;
					if (!hashtable.ContainsKey(firstName))
					{
						hashtable.Add(firstName, shellObjectParent.Properties.CreateTypedProperty<string>(firstName));
					}
					return hashtable[firstName] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> FullName
			{
				get
				{
					PropertyKey fullName = SystemProperties.System.Contact.FullName;
					if (!hashtable.ContainsKey(fullName))
					{
						hashtable.Add(fullName, shellObjectParent.Properties.CreateTypedProperty<string>(fullName));
					}
					return hashtable[fullName] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> Gender
			{
				get
				{
					PropertyKey gender = SystemProperties.System.Contact.Gender;
					if (!hashtable.ContainsKey(gender))
					{
						hashtable.Add(gender, shellObjectParent.Properties.CreateTypedProperty<string>(gender));
					}
					return hashtable[gender] as ShellProperty<string>;
				}
			}

			public ShellProperty<ushort?> GenderValue
			{
				get
				{
					PropertyKey genderValue = SystemProperties.System.Contact.GenderValue;
					if (!hashtable.ContainsKey(genderValue))
					{
						hashtable.Add(genderValue, shellObjectParent.Properties.CreateTypedProperty<ushort?>(genderValue));
					}
					return hashtable[genderValue] as ShellProperty<ushort?>;
				}
			}

			public ShellProperty<string[]> Hobbies
			{
				get
				{
					PropertyKey hobbies = SystemProperties.System.Contact.Hobbies;
					if (!hashtable.ContainsKey(hobbies))
					{
						hashtable.Add(hobbies, shellObjectParent.Properties.CreateTypedProperty<string[]>(hobbies));
					}
					return hashtable[hobbies] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string> HomeAddress
			{
				get
				{
					PropertyKey homeAddress = SystemProperties.System.Contact.HomeAddress;
					if (!hashtable.ContainsKey(homeAddress))
					{
						hashtable.Add(homeAddress, shellObjectParent.Properties.CreateTypedProperty<string>(homeAddress));
					}
					return hashtable[homeAddress] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> HomeAddressCity
			{
				get
				{
					PropertyKey homeAddressCity = SystemProperties.System.Contact.HomeAddressCity;
					if (!hashtable.ContainsKey(homeAddressCity))
					{
						hashtable.Add(homeAddressCity, shellObjectParent.Properties.CreateTypedProperty<string>(homeAddressCity));
					}
					return hashtable[homeAddressCity] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> HomeAddressCountry
			{
				get
				{
					PropertyKey homeAddressCountry = SystemProperties.System.Contact.HomeAddressCountry;
					if (!hashtable.ContainsKey(homeAddressCountry))
					{
						hashtable.Add(homeAddressCountry, shellObjectParent.Properties.CreateTypedProperty<string>(homeAddressCountry));
					}
					return hashtable[homeAddressCountry] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> HomeAddressPostalCode
			{
				get
				{
					PropertyKey homeAddressPostalCode = SystemProperties.System.Contact.HomeAddressPostalCode;
					if (!hashtable.ContainsKey(homeAddressPostalCode))
					{
						hashtable.Add(homeAddressPostalCode, shellObjectParent.Properties.CreateTypedProperty<string>(homeAddressPostalCode));
					}
					return hashtable[homeAddressPostalCode] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> HomeAddressPostOfficeBox
			{
				get
				{
					PropertyKey homeAddressPostOfficeBox = SystemProperties.System.Contact.HomeAddressPostOfficeBox;
					if (!hashtable.ContainsKey(homeAddressPostOfficeBox))
					{
						hashtable.Add(homeAddressPostOfficeBox, shellObjectParent.Properties.CreateTypedProperty<string>(homeAddressPostOfficeBox));
					}
					return hashtable[homeAddressPostOfficeBox] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> HomeAddressState
			{
				get
				{
					PropertyKey homeAddressState = SystemProperties.System.Contact.HomeAddressState;
					if (!hashtable.ContainsKey(homeAddressState))
					{
						hashtable.Add(homeAddressState, shellObjectParent.Properties.CreateTypedProperty<string>(homeAddressState));
					}
					return hashtable[homeAddressState] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> HomeAddressStreet
			{
				get
				{
					PropertyKey homeAddressStreet = SystemProperties.System.Contact.HomeAddressStreet;
					if (!hashtable.ContainsKey(homeAddressStreet))
					{
						hashtable.Add(homeAddressStreet, shellObjectParent.Properties.CreateTypedProperty<string>(homeAddressStreet));
					}
					return hashtable[homeAddressStreet] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> HomeFaxNumber
			{
				get
				{
					PropertyKey homeFaxNumber = SystemProperties.System.Contact.HomeFaxNumber;
					if (!hashtable.ContainsKey(homeFaxNumber))
					{
						hashtable.Add(homeFaxNumber, shellObjectParent.Properties.CreateTypedProperty<string>(homeFaxNumber));
					}
					return hashtable[homeFaxNumber] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> HomeTelephone
			{
				get
				{
					PropertyKey homeTelephone = SystemProperties.System.Contact.HomeTelephone;
					if (!hashtable.ContainsKey(homeTelephone))
					{
						hashtable.Add(homeTelephone, shellObjectParent.Properties.CreateTypedProperty<string>(homeTelephone));
					}
					return hashtable[homeTelephone] as ShellProperty<string>;
				}
			}

			public ShellProperty<string[]> IMAddress
			{
				get
				{
					PropertyKey iMAddress = SystemProperties.System.Contact.IMAddress;
					if (!hashtable.ContainsKey(iMAddress))
					{
						hashtable.Add(iMAddress, shellObjectParent.Properties.CreateTypedProperty<string[]>(iMAddress));
					}
					return hashtable[iMAddress] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string> Initials
			{
				get
				{
					PropertyKey initials = SystemProperties.System.Contact.Initials;
					if (!hashtable.ContainsKey(initials))
					{
						hashtable.Add(initials, shellObjectParent.Properties.CreateTypedProperty<string>(initials));
					}
					return hashtable[initials] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> JobTitle
			{
				get
				{
					PropertyKey jobTitle = SystemProperties.System.Contact.JobTitle;
					if (!hashtable.ContainsKey(jobTitle))
					{
						hashtable.Add(jobTitle, shellObjectParent.Properties.CreateTypedProperty<string>(jobTitle));
					}
					return hashtable[jobTitle] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> Label
			{
				get
				{
					PropertyKey label = SystemProperties.System.Contact.Label;
					if (!hashtable.ContainsKey(label))
					{
						hashtable.Add(label, shellObjectParent.Properties.CreateTypedProperty<string>(label));
					}
					return hashtable[label] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> LastName
			{
				get
				{
					PropertyKey lastName = SystemProperties.System.Contact.LastName;
					if (!hashtable.ContainsKey(lastName))
					{
						hashtable.Add(lastName, shellObjectParent.Properties.CreateTypedProperty<string>(lastName));
					}
					return hashtable[lastName] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> MailingAddress
			{
				get
				{
					PropertyKey mailingAddress = SystemProperties.System.Contact.MailingAddress;
					if (!hashtable.ContainsKey(mailingAddress))
					{
						hashtable.Add(mailingAddress, shellObjectParent.Properties.CreateTypedProperty<string>(mailingAddress));
					}
					return hashtable[mailingAddress] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> MiddleName
			{
				get
				{
					PropertyKey middleName = SystemProperties.System.Contact.MiddleName;
					if (!hashtable.ContainsKey(middleName))
					{
						hashtable.Add(middleName, shellObjectParent.Properties.CreateTypedProperty<string>(middleName));
					}
					return hashtable[middleName] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> MobileTelephone
			{
				get
				{
					PropertyKey mobileTelephone = SystemProperties.System.Contact.MobileTelephone;
					if (!hashtable.ContainsKey(mobileTelephone))
					{
						hashtable.Add(mobileTelephone, shellObjectParent.Properties.CreateTypedProperty<string>(mobileTelephone));
					}
					return hashtable[mobileTelephone] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> Nickname
			{
				get
				{
					PropertyKey nickname = SystemProperties.System.Contact.Nickname;
					if (!hashtable.ContainsKey(nickname))
					{
						hashtable.Add(nickname, shellObjectParent.Properties.CreateTypedProperty<string>(nickname));
					}
					return hashtable[nickname] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> OfficeLocation
			{
				get
				{
					PropertyKey officeLocation = SystemProperties.System.Contact.OfficeLocation;
					if (!hashtable.ContainsKey(officeLocation))
					{
						hashtable.Add(officeLocation, shellObjectParent.Properties.CreateTypedProperty<string>(officeLocation));
					}
					return hashtable[officeLocation] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> OtherAddress
			{
				get
				{
					PropertyKey otherAddress = SystemProperties.System.Contact.OtherAddress;
					if (!hashtable.ContainsKey(otherAddress))
					{
						hashtable.Add(otherAddress, shellObjectParent.Properties.CreateTypedProperty<string>(otherAddress));
					}
					return hashtable[otherAddress] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> OtherAddressCity
			{
				get
				{
					PropertyKey otherAddressCity = SystemProperties.System.Contact.OtherAddressCity;
					if (!hashtable.ContainsKey(otherAddressCity))
					{
						hashtable.Add(otherAddressCity, shellObjectParent.Properties.CreateTypedProperty<string>(otherAddressCity));
					}
					return hashtable[otherAddressCity] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> OtherAddressCountry
			{
				get
				{
					PropertyKey otherAddressCountry = SystemProperties.System.Contact.OtherAddressCountry;
					if (!hashtable.ContainsKey(otherAddressCountry))
					{
						hashtable.Add(otherAddressCountry, shellObjectParent.Properties.CreateTypedProperty<string>(otherAddressCountry));
					}
					return hashtable[otherAddressCountry] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> OtherAddressPostalCode
			{
				get
				{
					PropertyKey otherAddressPostalCode = SystemProperties.System.Contact.OtherAddressPostalCode;
					if (!hashtable.ContainsKey(otherAddressPostalCode))
					{
						hashtable.Add(otherAddressPostalCode, shellObjectParent.Properties.CreateTypedProperty<string>(otherAddressPostalCode));
					}
					return hashtable[otherAddressPostalCode] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> OtherAddressPostOfficeBox
			{
				get
				{
					PropertyKey otherAddressPostOfficeBox = SystemProperties.System.Contact.OtherAddressPostOfficeBox;
					if (!hashtable.ContainsKey(otherAddressPostOfficeBox))
					{
						hashtable.Add(otherAddressPostOfficeBox, shellObjectParent.Properties.CreateTypedProperty<string>(otherAddressPostOfficeBox));
					}
					return hashtable[otherAddressPostOfficeBox] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> OtherAddressState
			{
				get
				{
					PropertyKey otherAddressState = SystemProperties.System.Contact.OtherAddressState;
					if (!hashtable.ContainsKey(otherAddressState))
					{
						hashtable.Add(otherAddressState, shellObjectParent.Properties.CreateTypedProperty<string>(otherAddressState));
					}
					return hashtable[otherAddressState] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> OtherAddressStreet
			{
				get
				{
					PropertyKey otherAddressStreet = SystemProperties.System.Contact.OtherAddressStreet;
					if (!hashtable.ContainsKey(otherAddressStreet))
					{
						hashtable.Add(otherAddressStreet, shellObjectParent.Properties.CreateTypedProperty<string>(otherAddressStreet));
					}
					return hashtable[otherAddressStreet] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> PagerTelephone
			{
				get
				{
					PropertyKey pagerTelephone = SystemProperties.System.Contact.PagerTelephone;
					if (!hashtable.ContainsKey(pagerTelephone))
					{
						hashtable.Add(pagerTelephone, shellObjectParent.Properties.CreateTypedProperty<string>(pagerTelephone));
					}
					return hashtable[pagerTelephone] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> PersonalTitle
			{
				get
				{
					PropertyKey personalTitle = SystemProperties.System.Contact.PersonalTitle;
					if (!hashtable.ContainsKey(personalTitle))
					{
						hashtable.Add(personalTitle, shellObjectParent.Properties.CreateTypedProperty<string>(personalTitle));
					}
					return hashtable[personalTitle] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> PrimaryAddressCity
			{
				get
				{
					PropertyKey primaryAddressCity = SystemProperties.System.Contact.PrimaryAddressCity;
					if (!hashtable.ContainsKey(primaryAddressCity))
					{
						hashtable.Add(primaryAddressCity, shellObjectParent.Properties.CreateTypedProperty<string>(primaryAddressCity));
					}
					return hashtable[primaryAddressCity] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> PrimaryAddressCountry
			{
				get
				{
					PropertyKey primaryAddressCountry = SystemProperties.System.Contact.PrimaryAddressCountry;
					if (!hashtable.ContainsKey(primaryAddressCountry))
					{
						hashtable.Add(primaryAddressCountry, shellObjectParent.Properties.CreateTypedProperty<string>(primaryAddressCountry));
					}
					return hashtable[primaryAddressCountry] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> PrimaryAddressPostalCode
			{
				get
				{
					PropertyKey primaryAddressPostalCode = SystemProperties.System.Contact.PrimaryAddressPostalCode;
					if (!hashtable.ContainsKey(primaryAddressPostalCode))
					{
						hashtable.Add(primaryAddressPostalCode, shellObjectParent.Properties.CreateTypedProperty<string>(primaryAddressPostalCode));
					}
					return hashtable[primaryAddressPostalCode] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> PrimaryAddressPostOfficeBox
			{
				get
				{
					PropertyKey primaryAddressPostOfficeBox = SystemProperties.System.Contact.PrimaryAddressPostOfficeBox;
					if (!hashtable.ContainsKey(primaryAddressPostOfficeBox))
					{
						hashtable.Add(primaryAddressPostOfficeBox, shellObjectParent.Properties.CreateTypedProperty<string>(primaryAddressPostOfficeBox));
					}
					return hashtable[primaryAddressPostOfficeBox] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> PrimaryAddressState
			{
				get
				{
					PropertyKey primaryAddressState = SystemProperties.System.Contact.PrimaryAddressState;
					if (!hashtable.ContainsKey(primaryAddressState))
					{
						hashtable.Add(primaryAddressState, shellObjectParent.Properties.CreateTypedProperty<string>(primaryAddressState));
					}
					return hashtable[primaryAddressState] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> PrimaryAddressStreet
			{
				get
				{
					PropertyKey primaryAddressStreet = SystemProperties.System.Contact.PrimaryAddressStreet;
					if (!hashtable.ContainsKey(primaryAddressStreet))
					{
						hashtable.Add(primaryAddressStreet, shellObjectParent.Properties.CreateTypedProperty<string>(primaryAddressStreet));
					}
					return hashtable[primaryAddressStreet] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> PrimaryEmailAddress
			{
				get
				{
					PropertyKey primaryEmailAddress = SystemProperties.System.Contact.PrimaryEmailAddress;
					if (!hashtable.ContainsKey(primaryEmailAddress))
					{
						hashtable.Add(primaryEmailAddress, shellObjectParent.Properties.CreateTypedProperty<string>(primaryEmailAddress));
					}
					return hashtable[primaryEmailAddress] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> PrimaryTelephone
			{
				get
				{
					PropertyKey primaryTelephone = SystemProperties.System.Contact.PrimaryTelephone;
					if (!hashtable.ContainsKey(primaryTelephone))
					{
						hashtable.Add(primaryTelephone, shellObjectParent.Properties.CreateTypedProperty<string>(primaryTelephone));
					}
					return hashtable[primaryTelephone] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> Profession
			{
				get
				{
					PropertyKey profession = SystemProperties.System.Contact.Profession;
					if (!hashtable.ContainsKey(profession))
					{
						hashtable.Add(profession, shellObjectParent.Properties.CreateTypedProperty<string>(profession));
					}
					return hashtable[profession] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> SpouseName
			{
				get
				{
					PropertyKey spouseName = SystemProperties.System.Contact.SpouseName;
					if (!hashtable.ContainsKey(spouseName))
					{
						hashtable.Add(spouseName, shellObjectParent.Properties.CreateTypedProperty<string>(spouseName));
					}
					return hashtable[spouseName] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> Suffix
			{
				get
				{
					PropertyKey suffix = SystemProperties.System.Contact.Suffix;
					if (!hashtable.ContainsKey(suffix))
					{
						hashtable.Add(suffix, shellObjectParent.Properties.CreateTypedProperty<string>(suffix));
					}
					return hashtable[suffix] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> TelexNumber
			{
				get
				{
					PropertyKey telexNumber = SystemProperties.System.Contact.TelexNumber;
					if (!hashtable.ContainsKey(telexNumber))
					{
						hashtable.Add(telexNumber, shellObjectParent.Properties.CreateTypedProperty<string>(telexNumber));
					}
					return hashtable[telexNumber] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> TTYTDDTelephone
			{
				get
				{
					PropertyKey tTYTDDTelephone = SystemProperties.System.Contact.TTYTDDTelephone;
					if (!hashtable.ContainsKey(tTYTDDTelephone))
					{
						hashtable.Add(tTYTDDTelephone, shellObjectParent.Properties.CreateTypedProperty<string>(tTYTDDTelephone));
					}
					return hashtable[tTYTDDTelephone] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> Webpage
			{
				get
				{
					PropertyKey webpage = SystemProperties.System.Contact.Webpage;
					if (!hashtable.ContainsKey(webpage))
					{
						hashtable.Add(webpage, shellObjectParent.Properties.CreateTypedProperty<string>(webpage));
					}
					return hashtable[webpage] as ShellProperty<string>;
				}
			}

			public PropertyContactJA JA
			{
				get
				{
					if (internalPropertyContactJA == null)
					{
						internalPropertyContactJA = new PropertyContactJA(shellObjectParent);
					}
					return internalPropertyContactJA;
				}
			}

			internal PropertySystemContact(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertyContactJA : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<string> CompanyNamePhonetic
			{
				get
				{
					PropertyKey companyNamePhonetic = SystemProperties.System.Contact.JA.CompanyNamePhonetic;
					if (!hashtable.ContainsKey(companyNamePhonetic))
					{
						hashtable.Add(companyNamePhonetic, shellObjectParent.Properties.CreateTypedProperty<string>(companyNamePhonetic));
					}
					return hashtable[companyNamePhonetic] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> FirstNamePhonetic
			{
				get
				{
					PropertyKey firstNamePhonetic = SystemProperties.System.Contact.JA.FirstNamePhonetic;
					if (!hashtable.ContainsKey(firstNamePhonetic))
					{
						hashtable.Add(firstNamePhonetic, shellObjectParent.Properties.CreateTypedProperty<string>(firstNamePhonetic));
					}
					return hashtable[firstNamePhonetic] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> LastNamePhonetic
			{
				get
				{
					PropertyKey lastNamePhonetic = SystemProperties.System.Contact.JA.LastNamePhonetic;
					if (!hashtable.ContainsKey(lastNamePhonetic))
					{
						hashtable.Add(lastNamePhonetic, shellObjectParent.Properties.CreateTypedProperty<string>(lastNamePhonetic));
					}
					return hashtable[lastNamePhonetic] as ShellProperty<string>;
				}
			}

			internal PropertyContactJA(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemDevice : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<string> PrinterUrl
			{
				get
				{
					PropertyKey printerUrl = SystemProperties.System.Device.PrinterUrl;
					if (!hashtable.ContainsKey(printerUrl))
					{
						hashtable.Add(printerUrl, shellObjectParent.Properties.CreateTypedProperty<string>(printerUrl));
					}
					return hashtable[printerUrl] as ShellProperty<string>;
				}
			}

			internal PropertySystemDevice(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemDeviceInterface : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<string> PrinterDriverDirectory
			{
				get
				{
					PropertyKey printerDriverDirectory = SystemProperties.System.DeviceInterface.PrinterDriverDirectory;
					if (!hashtable.ContainsKey(printerDriverDirectory))
					{
						hashtable.Add(printerDriverDirectory, shellObjectParent.Properties.CreateTypedProperty<string>(printerDriverDirectory));
					}
					return hashtable[printerDriverDirectory] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> PrinterDriverName
			{
				get
				{
					PropertyKey printerDriverName = SystemProperties.System.DeviceInterface.PrinterDriverName;
					if (!hashtable.ContainsKey(printerDriverName))
					{
						hashtable.Add(printerDriverName, shellObjectParent.Properties.CreateTypedProperty<string>(printerDriverName));
					}
					return hashtable[printerDriverName] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> PrinterName
			{
				get
				{
					PropertyKey printerName = SystemProperties.System.DeviceInterface.PrinterName;
					if (!hashtable.ContainsKey(printerName))
					{
						hashtable.Add(printerName, shellObjectParent.Properties.CreateTypedProperty<string>(printerName));
					}
					return hashtable[printerName] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> PrinterPortName
			{
				get
				{
					PropertyKey printerPortName = SystemProperties.System.DeviceInterface.PrinterPortName;
					if (!hashtable.ContainsKey(printerPortName))
					{
						hashtable.Add(printerPortName, shellObjectParent.Properties.CreateTypedProperty<string>(printerPortName));
					}
					return hashtable[printerPortName] as ShellProperty<string>;
				}
			}

			internal PropertySystemDeviceInterface(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemDevices : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			private PropertyDevicesNotifications internalPropertyDevicesNotifications;

			public ShellProperty<byte?> BatteryLife
			{
				get
				{
					PropertyKey batteryLife = SystemProperties.System.Devices.BatteryLife;
					if (!hashtable.ContainsKey(batteryLife))
					{
						hashtable.Add(batteryLife, shellObjectParent.Properties.CreateTypedProperty<byte?>(batteryLife));
					}
					return hashtable[batteryLife] as ShellProperty<byte?>;
				}
			}

			public ShellProperty<byte?> BatteryPlusCharging
			{
				get
				{
					PropertyKey batteryPlusCharging = SystemProperties.System.Devices.BatteryPlusCharging;
					if (!hashtable.ContainsKey(batteryPlusCharging))
					{
						hashtable.Add(batteryPlusCharging, shellObjectParent.Properties.CreateTypedProperty<byte?>(batteryPlusCharging));
					}
					return hashtable[batteryPlusCharging] as ShellProperty<byte?>;
				}
			}

			public ShellProperty<string> BatteryPlusChargingText
			{
				get
				{
					PropertyKey batteryPlusChargingText = SystemProperties.System.Devices.BatteryPlusChargingText;
					if (!hashtable.ContainsKey(batteryPlusChargingText))
					{
						hashtable.Add(batteryPlusChargingText, shellObjectParent.Properties.CreateTypedProperty<string>(batteryPlusChargingText));
					}
					return hashtable[batteryPlusChargingText] as ShellProperty<string>;
				}
			}

			public ShellProperty<string[]> Category
			{
				get
				{
					PropertyKey category = SystemProperties.System.Devices.Category;
					if (!hashtable.ContainsKey(category))
					{
						hashtable.Add(category, shellObjectParent.Properties.CreateTypedProperty<string[]>(category));
					}
					return hashtable[category] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string[]> CategoryGroup
			{
				get
				{
					PropertyKey categoryGroup = SystemProperties.System.Devices.CategoryGroup;
					if (!hashtable.ContainsKey(categoryGroup))
					{
						hashtable.Add(categoryGroup, shellObjectParent.Properties.CreateTypedProperty<string[]>(categoryGroup));
					}
					return hashtable[categoryGroup] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string[]> CategoryPlural
			{
				get
				{
					PropertyKey categoryPlural = SystemProperties.System.Devices.CategoryPlural;
					if (!hashtable.ContainsKey(categoryPlural))
					{
						hashtable.Add(categoryPlural, shellObjectParent.Properties.CreateTypedProperty<string[]>(categoryPlural));
					}
					return hashtable[categoryPlural] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<byte?> ChargingState
			{
				get
				{
					PropertyKey chargingState = SystemProperties.System.Devices.ChargingState;
					if (!hashtable.ContainsKey(chargingState))
					{
						hashtable.Add(chargingState, shellObjectParent.Properties.CreateTypedProperty<byte?>(chargingState));
					}
					return hashtable[chargingState] as ShellProperty<byte?>;
				}
			}

			public ShellProperty<bool?> Connected
			{
				get
				{
					PropertyKey connected = SystemProperties.System.Devices.Connected;
					if (!hashtable.ContainsKey(connected))
					{
						hashtable.Add(connected, shellObjectParent.Properties.CreateTypedProperty<bool?>(connected));
					}
					return hashtable[connected] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<IntPtr?> ContainerId
			{
				get
				{
					PropertyKey containerId = SystemProperties.System.Devices.ContainerId;
					if (!hashtable.ContainsKey(containerId))
					{
						hashtable.Add(containerId, shellObjectParent.Properties.CreateTypedProperty<IntPtr?>(containerId));
					}
					return hashtable[containerId] as ShellProperty<IntPtr?>;
				}
			}

			public ShellProperty<string> DefaultTooltip
			{
				get
				{
					PropertyKey defaultTooltip = SystemProperties.System.Devices.DefaultTooltip;
					if (!hashtable.ContainsKey(defaultTooltip))
					{
						hashtable.Add(defaultTooltip, shellObjectParent.Properties.CreateTypedProperty<string>(defaultTooltip));
					}
					return hashtable[defaultTooltip] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> DeviceDescription1
			{
				get
				{
					PropertyKey deviceDescription = SystemProperties.System.Devices.DeviceDescription1;
					if (!hashtable.ContainsKey(deviceDescription))
					{
						hashtable.Add(deviceDescription, shellObjectParent.Properties.CreateTypedProperty<string>(deviceDescription));
					}
					return hashtable[deviceDescription] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> DeviceDescription2
			{
				get
				{
					PropertyKey deviceDescription = SystemProperties.System.Devices.DeviceDescription2;
					if (!hashtable.ContainsKey(deviceDescription))
					{
						hashtable.Add(deviceDescription, shellObjectParent.Properties.CreateTypedProperty<string>(deviceDescription));
					}
					return hashtable[deviceDescription] as ShellProperty<string>;
				}
			}

			public ShellProperty<string[]> DiscoveryMethod
			{
				get
				{
					PropertyKey discoveryMethod = SystemProperties.System.Devices.DiscoveryMethod;
					if (!hashtable.ContainsKey(discoveryMethod))
					{
						hashtable.Add(discoveryMethod, shellObjectParent.Properties.CreateTypedProperty<string[]>(discoveryMethod));
					}
					return hashtable[discoveryMethod] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string> FriendlyName
			{
				get
				{
					PropertyKey friendlyName = SystemProperties.System.Devices.FriendlyName;
					if (!hashtable.ContainsKey(friendlyName))
					{
						hashtable.Add(friendlyName, shellObjectParent.Properties.CreateTypedProperty<string>(friendlyName));
					}
					return hashtable[friendlyName] as ShellProperty<string>;
				}
			}

			public ShellProperty<string[]> FunctionPaths
			{
				get
				{
					PropertyKey functionPaths = SystemProperties.System.Devices.FunctionPaths;
					if (!hashtable.ContainsKey(functionPaths))
					{
						hashtable.Add(functionPaths, shellObjectParent.Properties.CreateTypedProperty<string[]>(functionPaths));
					}
					return hashtable[functionPaths] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string[]> InterfacePaths
			{
				get
				{
					PropertyKey interfacePaths = SystemProperties.System.Devices.InterfacePaths;
					if (!hashtable.ContainsKey(interfacePaths))
					{
						hashtable.Add(interfacePaths, shellObjectParent.Properties.CreateTypedProperty<string[]>(interfacePaths));
					}
					return hashtable[interfacePaths] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<bool?> IsDefault
			{
				get
				{
					PropertyKey isDefault = SystemProperties.System.Devices.IsDefault;
					if (!hashtable.ContainsKey(isDefault))
					{
						hashtable.Add(isDefault, shellObjectParent.Properties.CreateTypedProperty<bool?>(isDefault));
					}
					return hashtable[isDefault] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> IsNetworkConnected
			{
				get
				{
					PropertyKey isNetworkConnected = SystemProperties.System.Devices.IsNetworkConnected;
					if (!hashtable.ContainsKey(isNetworkConnected))
					{
						hashtable.Add(isNetworkConnected, shellObjectParent.Properties.CreateTypedProperty<bool?>(isNetworkConnected));
					}
					return hashtable[isNetworkConnected] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> IsShared
			{
				get
				{
					PropertyKey isShared = SystemProperties.System.Devices.IsShared;
					if (!hashtable.ContainsKey(isShared))
					{
						hashtable.Add(isShared, shellObjectParent.Properties.CreateTypedProperty<bool?>(isShared));
					}
					return hashtable[isShared] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> IsSoftwareInstalling
			{
				get
				{
					PropertyKey isSoftwareInstalling = SystemProperties.System.Devices.IsSoftwareInstalling;
					if (!hashtable.ContainsKey(isSoftwareInstalling))
					{
						hashtable.Add(isSoftwareInstalling, shellObjectParent.Properties.CreateTypedProperty<bool?>(isSoftwareInstalling));
					}
					return hashtable[isSoftwareInstalling] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> LaunchDeviceStageFromExplorer
			{
				get
				{
					PropertyKey launchDeviceStageFromExplorer = SystemProperties.System.Devices.LaunchDeviceStageFromExplorer;
					if (!hashtable.ContainsKey(launchDeviceStageFromExplorer))
					{
						hashtable.Add(launchDeviceStageFromExplorer, shellObjectParent.Properties.CreateTypedProperty<bool?>(launchDeviceStageFromExplorer));
					}
					return hashtable[launchDeviceStageFromExplorer] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> LocalMachine
			{
				get
				{
					PropertyKey localMachine = SystemProperties.System.Devices.LocalMachine;
					if (!hashtable.ContainsKey(localMachine))
					{
						hashtable.Add(localMachine, shellObjectParent.Properties.CreateTypedProperty<bool?>(localMachine));
					}
					return hashtable[localMachine] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<string> Manufacturer
			{
				get
				{
					PropertyKey manufacturer = SystemProperties.System.Devices.Manufacturer;
					if (!hashtable.ContainsKey(manufacturer))
					{
						hashtable.Add(manufacturer, shellObjectParent.Properties.CreateTypedProperty<string>(manufacturer));
					}
					return hashtable[manufacturer] as ShellProperty<string>;
				}
			}

			public ShellProperty<byte?> MissedCalls
			{
				get
				{
					PropertyKey missedCalls = SystemProperties.System.Devices.MissedCalls;
					if (!hashtable.ContainsKey(missedCalls))
					{
						hashtable.Add(missedCalls, shellObjectParent.Properties.CreateTypedProperty<byte?>(missedCalls));
					}
					return hashtable[missedCalls] as ShellProperty<byte?>;
				}
			}

			public ShellProperty<string> ModelName
			{
				get
				{
					PropertyKey modelName = SystemProperties.System.Devices.ModelName;
					if (!hashtable.ContainsKey(modelName))
					{
						hashtable.Add(modelName, shellObjectParent.Properties.CreateTypedProperty<string>(modelName));
					}
					return hashtable[modelName] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ModelNumber
			{
				get
				{
					PropertyKey modelNumber = SystemProperties.System.Devices.ModelNumber;
					if (!hashtable.ContainsKey(modelNumber))
					{
						hashtable.Add(modelNumber, shellObjectParent.Properties.CreateTypedProperty<string>(modelNumber));
					}
					return hashtable[modelNumber] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> NetworkedTooltip
			{
				get
				{
					PropertyKey networkedTooltip = SystemProperties.System.Devices.NetworkedTooltip;
					if (!hashtable.ContainsKey(networkedTooltip))
					{
						hashtable.Add(networkedTooltip, shellObjectParent.Properties.CreateTypedProperty<string>(networkedTooltip));
					}
					return hashtable[networkedTooltip] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> NetworkName
			{
				get
				{
					PropertyKey networkName = SystemProperties.System.Devices.NetworkName;
					if (!hashtable.ContainsKey(networkName))
					{
						hashtable.Add(networkName, shellObjectParent.Properties.CreateTypedProperty<string>(networkName));
					}
					return hashtable[networkName] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> NetworkType
			{
				get
				{
					PropertyKey networkType = SystemProperties.System.Devices.NetworkType;
					if (!hashtable.ContainsKey(networkType))
					{
						hashtable.Add(networkType, shellObjectParent.Properties.CreateTypedProperty<string>(networkType));
					}
					return hashtable[networkType] as ShellProperty<string>;
				}
			}

			public ShellProperty<ushort?> NewPictures
			{
				get
				{
					PropertyKey newPictures = SystemProperties.System.Devices.NewPictures;
					if (!hashtable.ContainsKey(newPictures))
					{
						hashtable.Add(newPictures, shellObjectParent.Properties.CreateTypedProperty<ushort?>(newPictures));
					}
					return hashtable[newPictures] as ShellProperty<ushort?>;
				}
			}

			public ShellProperty<string> Notification
			{
				get
				{
					PropertyKey notification = SystemProperties.System.Devices.Notification;
					if (!hashtable.ContainsKey(notification))
					{
						hashtable.Add(notification, shellObjectParent.Properties.CreateTypedProperty<string>(notification));
					}
					return hashtable[notification] as ShellProperty<string>;
				}
			}

			public ShellProperty<IntPtr?> NotificationStore
			{
				get
				{
					PropertyKey notificationStore = SystemProperties.System.Devices.NotificationStore;
					if (!hashtable.ContainsKey(notificationStore))
					{
						hashtable.Add(notificationStore, shellObjectParent.Properties.CreateTypedProperty<IntPtr?>(notificationStore));
					}
					return hashtable[notificationStore] as ShellProperty<IntPtr?>;
				}
			}

			public ShellProperty<bool?> NotWorkingProperly
			{
				get
				{
					PropertyKey notWorkingProperly = SystemProperties.System.Devices.NotWorkingProperly;
					if (!hashtable.ContainsKey(notWorkingProperly))
					{
						hashtable.Add(notWorkingProperly, shellObjectParent.Properties.CreateTypedProperty<bool?>(notWorkingProperly));
					}
					return hashtable[notWorkingProperly] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> Paired
			{
				get
				{
					PropertyKey paired = SystemProperties.System.Devices.Paired;
					if (!hashtable.ContainsKey(paired))
					{
						hashtable.Add(paired, shellObjectParent.Properties.CreateTypedProperty<bool?>(paired));
					}
					return hashtable[paired] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<string> PrimaryCategory
			{
				get
				{
					PropertyKey primaryCategory = SystemProperties.System.Devices.PrimaryCategory;
					if (!hashtable.ContainsKey(primaryCategory))
					{
						hashtable.Add(primaryCategory, shellObjectParent.Properties.CreateTypedProperty<string>(primaryCategory));
					}
					return hashtable[primaryCategory] as ShellProperty<string>;
				}
			}

			public ShellProperty<byte?> Roaming
			{
				get
				{
					PropertyKey roaming = SystemProperties.System.Devices.Roaming;
					if (!hashtable.ContainsKey(roaming))
					{
						hashtable.Add(roaming, shellObjectParent.Properties.CreateTypedProperty<byte?>(roaming));
					}
					return hashtable[roaming] as ShellProperty<byte?>;
				}
			}

			public ShellProperty<bool?> SafeRemovalRequired
			{
				get
				{
					PropertyKey safeRemovalRequired = SystemProperties.System.Devices.SafeRemovalRequired;
					if (!hashtable.ContainsKey(safeRemovalRequired))
					{
						hashtable.Add(safeRemovalRequired, shellObjectParent.Properties.CreateTypedProperty<bool?>(safeRemovalRequired));
					}
					return hashtable[safeRemovalRequired] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<string> SharedTooltip
			{
				get
				{
					PropertyKey sharedTooltip = SystemProperties.System.Devices.SharedTooltip;
					if (!hashtable.ContainsKey(sharedTooltip))
					{
						hashtable.Add(sharedTooltip, shellObjectParent.Properties.CreateTypedProperty<string>(sharedTooltip));
					}
					return hashtable[sharedTooltip] as ShellProperty<string>;
				}
			}

			public ShellProperty<byte?> SignalStrength
			{
				get
				{
					PropertyKey signalStrength = SystemProperties.System.Devices.SignalStrength;
					if (!hashtable.ContainsKey(signalStrength))
					{
						hashtable.Add(signalStrength, shellObjectParent.Properties.CreateTypedProperty<byte?>(signalStrength));
					}
					return hashtable[signalStrength] as ShellProperty<byte?>;
				}
			}

			public ShellProperty<string> Status1
			{
				get
				{
					PropertyKey status = SystemProperties.System.Devices.Status1;
					if (!hashtable.ContainsKey(status))
					{
						hashtable.Add(status, shellObjectParent.Properties.CreateTypedProperty<string>(status));
					}
					return hashtable[status] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> Status2
			{
				get
				{
					PropertyKey status = SystemProperties.System.Devices.Status2;
					if (!hashtable.ContainsKey(status))
					{
						hashtable.Add(status, shellObjectParent.Properties.CreateTypedProperty<string>(status));
					}
					return hashtable[status] as ShellProperty<string>;
				}
			}

			public ShellProperty<ulong?> StorageCapacity
			{
				get
				{
					PropertyKey storageCapacity = SystemProperties.System.Devices.StorageCapacity;
					if (!hashtable.ContainsKey(storageCapacity))
					{
						hashtable.Add(storageCapacity, shellObjectParent.Properties.CreateTypedProperty<ulong?>(storageCapacity));
					}
					return hashtable[storageCapacity] as ShellProperty<ulong?>;
				}
			}

			public ShellProperty<ulong?> StorageFreeSpace
			{
				get
				{
					PropertyKey storageFreeSpace = SystemProperties.System.Devices.StorageFreeSpace;
					if (!hashtable.ContainsKey(storageFreeSpace))
					{
						hashtable.Add(storageFreeSpace, shellObjectParent.Properties.CreateTypedProperty<ulong?>(storageFreeSpace));
					}
					return hashtable[storageFreeSpace] as ShellProperty<ulong?>;
				}
			}

			public ShellProperty<uint?> StorageFreeSpacePercent
			{
				get
				{
					PropertyKey storageFreeSpacePercent = SystemProperties.System.Devices.StorageFreeSpacePercent;
					if (!hashtable.ContainsKey(storageFreeSpacePercent))
					{
						hashtable.Add(storageFreeSpacePercent, shellObjectParent.Properties.CreateTypedProperty<uint?>(storageFreeSpacePercent));
					}
					return hashtable[storageFreeSpacePercent] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<byte?> TextMessages
			{
				get
				{
					PropertyKey textMessages = SystemProperties.System.Devices.TextMessages;
					if (!hashtable.ContainsKey(textMessages))
					{
						hashtable.Add(textMessages, shellObjectParent.Properties.CreateTypedProperty<byte?>(textMessages));
					}
					return hashtable[textMessages] as ShellProperty<byte?>;
				}
			}

			public ShellProperty<byte?> Voicemail
			{
				get
				{
					PropertyKey voicemail = SystemProperties.System.Devices.Voicemail;
					if (!hashtable.ContainsKey(voicemail))
					{
						hashtable.Add(voicemail, shellObjectParent.Properties.CreateTypedProperty<byte?>(voicemail));
					}
					return hashtable[voicemail] as ShellProperty<byte?>;
				}
			}

			public PropertyDevicesNotifications Notifications
			{
				get
				{
					if (internalPropertyDevicesNotifications == null)
					{
						internalPropertyDevicesNotifications = new PropertyDevicesNotifications(shellObjectParent);
					}
					return internalPropertyDevicesNotifications;
				}
			}

			internal PropertySystemDevices(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertyDevicesNotifications : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<byte?> LowBattery
			{
				get
				{
					PropertyKey lowBattery = SystemProperties.System.Devices.Notifications.LowBattery;
					if (!hashtable.ContainsKey(lowBattery))
					{
						hashtable.Add(lowBattery, shellObjectParent.Properties.CreateTypedProperty<byte?>(lowBattery));
					}
					return hashtable[lowBattery] as ShellProperty<byte?>;
				}
			}

			public ShellProperty<byte?> MissedCall
			{
				get
				{
					PropertyKey missedCall = SystemProperties.System.Devices.Notifications.MissedCall;
					if (!hashtable.ContainsKey(missedCall))
					{
						hashtable.Add(missedCall, shellObjectParent.Properties.CreateTypedProperty<byte?>(missedCall));
					}
					return hashtable[missedCall] as ShellProperty<byte?>;
				}
			}

			public ShellProperty<byte?> NewMessage
			{
				get
				{
					PropertyKey newMessage = SystemProperties.System.Devices.Notifications.NewMessage;
					if (!hashtable.ContainsKey(newMessage))
					{
						hashtable.Add(newMessage, shellObjectParent.Properties.CreateTypedProperty<byte?>(newMessage));
					}
					return hashtable[newMessage] as ShellProperty<byte?>;
				}
			}

			public ShellProperty<byte?> NewVoicemail
			{
				get
				{
					PropertyKey newVoicemail = SystemProperties.System.Devices.Notifications.NewVoicemail;
					if (!hashtable.ContainsKey(newVoicemail))
					{
						hashtable.Add(newVoicemail, shellObjectParent.Properties.CreateTypedProperty<byte?>(newVoicemail));
					}
					return hashtable[newVoicemail] as ShellProperty<byte?>;
				}
			}

			public ShellProperty<ulong?> StorageFull
			{
				get
				{
					PropertyKey storageFull = SystemProperties.System.Devices.Notifications.StorageFull;
					if (!hashtable.ContainsKey(storageFull))
					{
						hashtable.Add(storageFull, shellObjectParent.Properties.CreateTypedProperty<ulong?>(storageFull));
					}
					return hashtable[storageFull] as ShellProperty<ulong?>;
				}
			}

			public ShellProperty<ulong?> StorageFullLinkText
			{
				get
				{
					PropertyKey storageFullLinkText = SystemProperties.System.Devices.Notifications.StorageFullLinkText;
					if (!hashtable.ContainsKey(storageFullLinkText))
					{
						hashtable.Add(storageFullLinkText, shellObjectParent.Properties.CreateTypedProperty<ulong?>(storageFullLinkText));
					}
					return hashtable[storageFullLinkText] as ShellProperty<ulong?>;
				}
			}

			internal PropertyDevicesNotifications(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemDocument : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<int?> ByteCount
			{
				get
				{
					PropertyKey byteCount = SystemProperties.System.Document.ByteCount;
					if (!hashtable.ContainsKey(byteCount))
					{
						hashtable.Add(byteCount, shellObjectParent.Properties.CreateTypedProperty<int?>(byteCount));
					}
					return hashtable[byteCount] as ShellProperty<int?>;
				}
			}

			public ShellProperty<int?> CharacterCount
			{
				get
				{
					PropertyKey characterCount = SystemProperties.System.Document.CharacterCount;
					if (!hashtable.ContainsKey(characterCount))
					{
						hashtable.Add(characterCount, shellObjectParent.Properties.CreateTypedProperty<int?>(characterCount));
					}
					return hashtable[characterCount] as ShellProperty<int?>;
				}
			}

			public ShellProperty<string> ClientID
			{
				get
				{
					PropertyKey clientID = SystemProperties.System.Document.ClientID;
					if (!hashtable.ContainsKey(clientID))
					{
						hashtable.Add(clientID, shellObjectParent.Properties.CreateTypedProperty<string>(clientID));
					}
					return hashtable[clientID] as ShellProperty<string>;
				}
			}

			public ShellProperty<string[]> Contributor
			{
				get
				{
					PropertyKey contributor = SystemProperties.System.Document.Contributor;
					if (!hashtable.ContainsKey(contributor))
					{
						hashtable.Add(contributor, shellObjectParent.Properties.CreateTypedProperty<string[]>(contributor));
					}
					return hashtable[contributor] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<DateTime?> DateCreated
			{
				get
				{
					PropertyKey dateCreated = SystemProperties.System.Document.DateCreated;
					if (!hashtable.ContainsKey(dateCreated))
					{
						hashtable.Add(dateCreated, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(dateCreated));
					}
					return hashtable[dateCreated] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<DateTime?> DatePrinted
			{
				get
				{
					PropertyKey datePrinted = SystemProperties.System.Document.DatePrinted;
					if (!hashtable.ContainsKey(datePrinted))
					{
						hashtable.Add(datePrinted, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(datePrinted));
					}
					return hashtable[datePrinted] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<DateTime?> DateSaved
			{
				get
				{
					PropertyKey dateSaved = SystemProperties.System.Document.DateSaved;
					if (!hashtable.ContainsKey(dateSaved))
					{
						hashtable.Add(dateSaved, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(dateSaved));
					}
					return hashtable[dateSaved] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<string> Division
			{
				get
				{
					PropertyKey division = SystemProperties.System.Document.Division;
					if (!hashtable.ContainsKey(division))
					{
						hashtable.Add(division, shellObjectParent.Properties.CreateTypedProperty<string>(division));
					}
					return hashtable[division] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> DocumentID
			{
				get
				{
					PropertyKey documentID = SystemProperties.System.Document.DocumentID;
					if (!hashtable.ContainsKey(documentID))
					{
						hashtable.Add(documentID, shellObjectParent.Properties.CreateTypedProperty<string>(documentID));
					}
					return hashtable[documentID] as ShellProperty<string>;
				}
			}

			public ShellProperty<int?> HiddenSlideCount
			{
				get
				{
					PropertyKey hiddenSlideCount = SystemProperties.System.Document.HiddenSlideCount;
					if (!hashtable.ContainsKey(hiddenSlideCount))
					{
						hashtable.Add(hiddenSlideCount, shellObjectParent.Properties.CreateTypedProperty<int?>(hiddenSlideCount));
					}
					return hashtable[hiddenSlideCount] as ShellProperty<int?>;
				}
			}

			public ShellProperty<string> LastAuthor
			{
				get
				{
					PropertyKey lastAuthor = SystemProperties.System.Document.LastAuthor;
					if (!hashtable.ContainsKey(lastAuthor))
					{
						hashtable.Add(lastAuthor, shellObjectParent.Properties.CreateTypedProperty<string>(lastAuthor));
					}
					return hashtable[lastAuthor] as ShellProperty<string>;
				}
			}

			public ShellProperty<int?> LineCount
			{
				get
				{
					PropertyKey lineCount = SystemProperties.System.Document.LineCount;
					if (!hashtable.ContainsKey(lineCount))
					{
						hashtable.Add(lineCount, shellObjectParent.Properties.CreateTypedProperty<int?>(lineCount));
					}
					return hashtable[lineCount] as ShellProperty<int?>;
				}
			}

			public ShellProperty<string> Manager
			{
				get
				{
					PropertyKey manager = SystemProperties.System.Document.Manager;
					if (!hashtable.ContainsKey(manager))
					{
						hashtable.Add(manager, shellObjectParent.Properties.CreateTypedProperty<string>(manager));
					}
					return hashtable[manager] as ShellProperty<string>;
				}
			}

			public ShellProperty<int?> MultimediaClipCount
			{
				get
				{
					PropertyKey multimediaClipCount = SystemProperties.System.Document.MultimediaClipCount;
					if (!hashtable.ContainsKey(multimediaClipCount))
					{
						hashtable.Add(multimediaClipCount, shellObjectParent.Properties.CreateTypedProperty<int?>(multimediaClipCount));
					}
					return hashtable[multimediaClipCount] as ShellProperty<int?>;
				}
			}

			public ShellProperty<int?> NoteCount
			{
				get
				{
					PropertyKey noteCount = SystemProperties.System.Document.NoteCount;
					if (!hashtable.ContainsKey(noteCount))
					{
						hashtable.Add(noteCount, shellObjectParent.Properties.CreateTypedProperty<int?>(noteCount));
					}
					return hashtable[noteCount] as ShellProperty<int?>;
				}
			}

			public ShellProperty<int?> PageCount
			{
				get
				{
					PropertyKey pageCount = SystemProperties.System.Document.PageCount;
					if (!hashtable.ContainsKey(pageCount))
					{
						hashtable.Add(pageCount, shellObjectParent.Properties.CreateTypedProperty<int?>(pageCount));
					}
					return hashtable[pageCount] as ShellProperty<int?>;
				}
			}

			public ShellProperty<int?> ParagraphCount
			{
				get
				{
					PropertyKey paragraphCount = SystemProperties.System.Document.ParagraphCount;
					if (!hashtable.ContainsKey(paragraphCount))
					{
						hashtable.Add(paragraphCount, shellObjectParent.Properties.CreateTypedProperty<int?>(paragraphCount));
					}
					return hashtable[paragraphCount] as ShellProperty<int?>;
				}
			}

			public ShellProperty<string> PresentationFormat
			{
				get
				{
					PropertyKey presentationFormat = SystemProperties.System.Document.PresentationFormat;
					if (!hashtable.ContainsKey(presentationFormat))
					{
						hashtable.Add(presentationFormat, shellObjectParent.Properties.CreateTypedProperty<string>(presentationFormat));
					}
					return hashtable[presentationFormat] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> RevisionNumber
			{
				get
				{
					PropertyKey revisionNumber = SystemProperties.System.Document.RevisionNumber;
					if (!hashtable.ContainsKey(revisionNumber))
					{
						hashtable.Add(revisionNumber, shellObjectParent.Properties.CreateTypedProperty<string>(revisionNumber));
					}
					return hashtable[revisionNumber] as ShellProperty<string>;
				}
			}

			public ShellProperty<int?> Security
			{
				get
				{
					PropertyKey security = SystemProperties.System.Document.Security;
					if (!hashtable.ContainsKey(security))
					{
						hashtable.Add(security, shellObjectParent.Properties.CreateTypedProperty<int?>(security));
					}
					return hashtable[security] as ShellProperty<int?>;
				}
			}

			public ShellProperty<int?> SlideCount
			{
				get
				{
					PropertyKey slideCount = SystemProperties.System.Document.SlideCount;
					if (!hashtable.ContainsKey(slideCount))
					{
						hashtable.Add(slideCount, shellObjectParent.Properties.CreateTypedProperty<int?>(slideCount));
					}
					return hashtable[slideCount] as ShellProperty<int?>;
				}
			}

			public ShellProperty<string> Template
			{
				get
				{
					PropertyKey template = SystemProperties.System.Document.Template;
					if (!hashtable.ContainsKey(template))
					{
						hashtable.Add(template, shellObjectParent.Properties.CreateTypedProperty<string>(template));
					}
					return hashtable[template] as ShellProperty<string>;
				}
			}

			public ShellProperty<ulong?> TotalEditingTime
			{
				get
				{
					PropertyKey totalEditingTime = SystemProperties.System.Document.TotalEditingTime;
					if (!hashtable.ContainsKey(totalEditingTime))
					{
						hashtable.Add(totalEditingTime, shellObjectParent.Properties.CreateTypedProperty<ulong?>(totalEditingTime));
					}
					return hashtable[totalEditingTime] as ShellProperty<ulong?>;
				}
			}

			public ShellProperty<string> Version
			{
				get
				{
					PropertyKey version = SystemProperties.System.Document.Version;
					if (!hashtable.ContainsKey(version))
					{
						hashtable.Add(version, shellObjectParent.Properties.CreateTypedProperty<string>(version));
					}
					return hashtable[version] as ShellProperty<string>;
				}
			}

			public ShellProperty<int?> WordCount
			{
				get
				{
					PropertyKey wordCount = SystemProperties.System.Document.WordCount;
					if (!hashtable.ContainsKey(wordCount))
					{
						hashtable.Add(wordCount, shellObjectParent.Properties.CreateTypedProperty<int?>(wordCount));
					}
					return hashtable[wordCount] as ShellProperty<int?>;
				}
			}

			internal PropertySystemDocument(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemDRM : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<DateTime?> DatePlayExpires
			{
				get
				{
					PropertyKey datePlayExpires = SystemProperties.System.DRM.DatePlayExpires;
					if (!hashtable.ContainsKey(datePlayExpires))
					{
						hashtable.Add(datePlayExpires, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(datePlayExpires));
					}
					return hashtable[datePlayExpires] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<DateTime?> DatePlayStarts
			{
				get
				{
					PropertyKey datePlayStarts = SystemProperties.System.DRM.DatePlayStarts;
					if (!hashtable.ContainsKey(datePlayStarts))
					{
						hashtable.Add(datePlayStarts, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(datePlayStarts));
					}
					return hashtable[datePlayStarts] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<string> Description
			{
				get
				{
					PropertyKey description = SystemProperties.System.DRM.Description;
					if (!hashtable.ContainsKey(description))
					{
						hashtable.Add(description, shellObjectParent.Properties.CreateTypedProperty<string>(description));
					}
					return hashtable[description] as ShellProperty<string>;
				}
			}

			public ShellProperty<bool?> IsProtected
			{
				get
				{
					PropertyKey isProtected = SystemProperties.System.DRM.IsProtected;
					if (!hashtable.ContainsKey(isProtected))
					{
						hashtable.Add(isProtected, shellObjectParent.Properties.CreateTypedProperty<bool?>(isProtected));
					}
					return hashtable[isProtected] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<uint?> PlayCount
			{
				get
				{
					PropertyKey playCount = SystemProperties.System.DRM.PlayCount;
					if (!hashtable.ContainsKey(playCount))
					{
						hashtable.Add(playCount, shellObjectParent.Properties.CreateTypedProperty<uint?>(playCount));
					}
					return hashtable[playCount] as ShellProperty<uint?>;
				}
			}

			internal PropertySystemDRM(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemGPS : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<double?> Altitude
			{
				get
				{
					PropertyKey altitude = SystemProperties.System.GPS.Altitude;
					if (!hashtable.ContainsKey(altitude))
					{
						hashtable.Add(altitude, shellObjectParent.Properties.CreateTypedProperty<double?>(altitude));
					}
					return hashtable[altitude] as ShellProperty<double?>;
				}
			}

			public ShellProperty<uint?> AltitudeDenominator
			{
				get
				{
					PropertyKey altitudeDenominator = SystemProperties.System.GPS.AltitudeDenominator;
					if (!hashtable.ContainsKey(altitudeDenominator))
					{
						hashtable.Add(altitudeDenominator, shellObjectParent.Properties.CreateTypedProperty<uint?>(altitudeDenominator));
					}
					return hashtable[altitudeDenominator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> AltitudeNumerator
			{
				get
				{
					PropertyKey altitudeNumerator = SystemProperties.System.GPS.AltitudeNumerator;
					if (!hashtable.ContainsKey(altitudeNumerator))
					{
						hashtable.Add(altitudeNumerator, shellObjectParent.Properties.CreateTypedProperty<uint?>(altitudeNumerator));
					}
					return hashtable[altitudeNumerator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<byte?> AltitudeRef
			{
				get
				{
					PropertyKey altitudeRef = SystemProperties.System.GPS.AltitudeRef;
					if (!hashtable.ContainsKey(altitudeRef))
					{
						hashtable.Add(altitudeRef, shellObjectParent.Properties.CreateTypedProperty<byte?>(altitudeRef));
					}
					return hashtable[altitudeRef] as ShellProperty<byte?>;
				}
			}

			public ShellProperty<string> AreaInformation
			{
				get
				{
					PropertyKey areaInformation = SystemProperties.System.GPS.AreaInformation;
					if (!hashtable.ContainsKey(areaInformation))
					{
						hashtable.Add(areaInformation, shellObjectParent.Properties.CreateTypedProperty<string>(areaInformation));
					}
					return hashtable[areaInformation] as ShellProperty<string>;
				}
			}

			public ShellProperty<DateTime?> Date
			{
				get
				{
					PropertyKey date = SystemProperties.System.GPS.Date;
					if (!hashtable.ContainsKey(date))
					{
						hashtable.Add(date, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(date));
					}
					return hashtable[date] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<double?> DestinationBearing
			{
				get
				{
					PropertyKey destinationBearing = SystemProperties.System.GPS.DestinationBearing;
					if (!hashtable.ContainsKey(destinationBearing))
					{
						hashtable.Add(destinationBearing, shellObjectParent.Properties.CreateTypedProperty<double?>(destinationBearing));
					}
					return hashtable[destinationBearing] as ShellProperty<double?>;
				}
			}

			public ShellProperty<uint?> DestinationBearingDenominator
			{
				get
				{
					PropertyKey destinationBearingDenominator = SystemProperties.System.GPS.DestinationBearingDenominator;
					if (!hashtable.ContainsKey(destinationBearingDenominator))
					{
						hashtable.Add(destinationBearingDenominator, shellObjectParent.Properties.CreateTypedProperty<uint?>(destinationBearingDenominator));
					}
					return hashtable[destinationBearingDenominator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> DestinationBearingNumerator
			{
				get
				{
					PropertyKey destinationBearingNumerator = SystemProperties.System.GPS.DestinationBearingNumerator;
					if (!hashtable.ContainsKey(destinationBearingNumerator))
					{
						hashtable.Add(destinationBearingNumerator, shellObjectParent.Properties.CreateTypedProperty<uint?>(destinationBearingNumerator));
					}
					return hashtable[destinationBearingNumerator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string> DestinationBearingRef
			{
				get
				{
					PropertyKey destinationBearingRef = SystemProperties.System.GPS.DestinationBearingRef;
					if (!hashtable.ContainsKey(destinationBearingRef))
					{
						hashtable.Add(destinationBearingRef, shellObjectParent.Properties.CreateTypedProperty<string>(destinationBearingRef));
					}
					return hashtable[destinationBearingRef] as ShellProperty<string>;
				}
			}

			public ShellProperty<double?> DestinationDistance
			{
				get
				{
					PropertyKey destinationDistance = SystemProperties.System.GPS.DestinationDistance;
					if (!hashtable.ContainsKey(destinationDistance))
					{
						hashtable.Add(destinationDistance, shellObjectParent.Properties.CreateTypedProperty<double?>(destinationDistance));
					}
					return hashtable[destinationDistance] as ShellProperty<double?>;
				}
			}

			public ShellProperty<uint?> DestinationDistanceDenominator
			{
				get
				{
					PropertyKey destinationDistanceDenominator = SystemProperties.System.GPS.DestinationDistanceDenominator;
					if (!hashtable.ContainsKey(destinationDistanceDenominator))
					{
						hashtable.Add(destinationDistanceDenominator, shellObjectParent.Properties.CreateTypedProperty<uint?>(destinationDistanceDenominator));
					}
					return hashtable[destinationDistanceDenominator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> DestinationDistanceNumerator
			{
				get
				{
					PropertyKey destinationDistanceNumerator = SystemProperties.System.GPS.DestinationDistanceNumerator;
					if (!hashtable.ContainsKey(destinationDistanceNumerator))
					{
						hashtable.Add(destinationDistanceNumerator, shellObjectParent.Properties.CreateTypedProperty<uint?>(destinationDistanceNumerator));
					}
					return hashtable[destinationDistanceNumerator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string> DestinationDistanceRef
			{
				get
				{
					PropertyKey destinationDistanceRef = SystemProperties.System.GPS.DestinationDistanceRef;
					if (!hashtable.ContainsKey(destinationDistanceRef))
					{
						hashtable.Add(destinationDistanceRef, shellObjectParent.Properties.CreateTypedProperty<string>(destinationDistanceRef));
					}
					return hashtable[destinationDistanceRef] as ShellProperty<string>;
				}
			}

			public ShellProperty<double[]> DestinationLatitude
			{
				get
				{
					PropertyKey destinationLatitude = SystemProperties.System.GPS.DestinationLatitude;
					if (!hashtable.ContainsKey(destinationLatitude))
					{
						hashtable.Add(destinationLatitude, shellObjectParent.Properties.CreateTypedProperty<double[]>(destinationLatitude));
					}
					return hashtable[destinationLatitude] as ShellProperty<double[]>;
				}
			}

			public ShellProperty<uint[]> DestinationLatitudeDenominator
			{
				get
				{
					PropertyKey destinationLatitudeDenominator = SystemProperties.System.GPS.DestinationLatitudeDenominator;
					if (!hashtable.ContainsKey(destinationLatitudeDenominator))
					{
						hashtable.Add(destinationLatitudeDenominator, shellObjectParent.Properties.CreateTypedProperty<uint[]>(destinationLatitudeDenominator));
					}
					return hashtable[destinationLatitudeDenominator] as ShellProperty<uint[]>;
				}
			}

			public ShellProperty<uint[]> DestinationLatitudeNumerator
			{
				get
				{
					PropertyKey destinationLatitudeNumerator = SystemProperties.System.GPS.DestinationLatitudeNumerator;
					if (!hashtable.ContainsKey(destinationLatitudeNumerator))
					{
						hashtable.Add(destinationLatitudeNumerator, shellObjectParent.Properties.CreateTypedProperty<uint[]>(destinationLatitudeNumerator));
					}
					return hashtable[destinationLatitudeNumerator] as ShellProperty<uint[]>;
				}
			}

			public ShellProperty<string> DestinationLatitudeRef
			{
				get
				{
					PropertyKey destinationLatitudeRef = SystemProperties.System.GPS.DestinationLatitudeRef;
					if (!hashtable.ContainsKey(destinationLatitudeRef))
					{
						hashtable.Add(destinationLatitudeRef, shellObjectParent.Properties.CreateTypedProperty<string>(destinationLatitudeRef));
					}
					return hashtable[destinationLatitudeRef] as ShellProperty<string>;
				}
			}

			public ShellProperty<double[]> DestinationLongitude
			{
				get
				{
					PropertyKey destinationLongitude = SystemProperties.System.GPS.DestinationLongitude;
					if (!hashtable.ContainsKey(destinationLongitude))
					{
						hashtable.Add(destinationLongitude, shellObjectParent.Properties.CreateTypedProperty<double[]>(destinationLongitude));
					}
					return hashtable[destinationLongitude] as ShellProperty<double[]>;
				}
			}

			public ShellProperty<uint[]> DestinationLongitudeDenominator
			{
				get
				{
					PropertyKey destinationLongitudeDenominator = SystemProperties.System.GPS.DestinationLongitudeDenominator;
					if (!hashtable.ContainsKey(destinationLongitudeDenominator))
					{
						hashtable.Add(destinationLongitudeDenominator, shellObjectParent.Properties.CreateTypedProperty<uint[]>(destinationLongitudeDenominator));
					}
					return hashtable[destinationLongitudeDenominator] as ShellProperty<uint[]>;
				}
			}

			public ShellProperty<uint[]> DestinationLongitudeNumerator
			{
				get
				{
					PropertyKey destinationLongitudeNumerator = SystemProperties.System.GPS.DestinationLongitudeNumerator;
					if (!hashtable.ContainsKey(destinationLongitudeNumerator))
					{
						hashtable.Add(destinationLongitudeNumerator, shellObjectParent.Properties.CreateTypedProperty<uint[]>(destinationLongitudeNumerator));
					}
					return hashtable[destinationLongitudeNumerator] as ShellProperty<uint[]>;
				}
			}

			public ShellProperty<string> DestinationLongitudeRef
			{
				get
				{
					PropertyKey destinationLongitudeRef = SystemProperties.System.GPS.DestinationLongitudeRef;
					if (!hashtable.ContainsKey(destinationLongitudeRef))
					{
						hashtable.Add(destinationLongitudeRef, shellObjectParent.Properties.CreateTypedProperty<string>(destinationLongitudeRef));
					}
					return hashtable[destinationLongitudeRef] as ShellProperty<string>;
				}
			}

			public ShellProperty<ushort?> Differential
			{
				get
				{
					PropertyKey differential = SystemProperties.System.GPS.Differential;
					if (!hashtable.ContainsKey(differential))
					{
						hashtable.Add(differential, shellObjectParent.Properties.CreateTypedProperty<ushort?>(differential));
					}
					return hashtable[differential] as ShellProperty<ushort?>;
				}
			}

			public ShellProperty<double?> DOP
			{
				get
				{
					PropertyKey dOP = SystemProperties.System.GPS.DOP;
					if (!hashtable.ContainsKey(dOP))
					{
						hashtable.Add(dOP, shellObjectParent.Properties.CreateTypedProperty<double?>(dOP));
					}
					return hashtable[dOP] as ShellProperty<double?>;
				}
			}

			public ShellProperty<uint?> DOPDenominator
			{
				get
				{
					PropertyKey dOPDenominator = SystemProperties.System.GPS.DOPDenominator;
					if (!hashtable.ContainsKey(dOPDenominator))
					{
						hashtable.Add(dOPDenominator, shellObjectParent.Properties.CreateTypedProperty<uint?>(dOPDenominator));
					}
					return hashtable[dOPDenominator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> DOPNumerator
			{
				get
				{
					PropertyKey dOPNumerator = SystemProperties.System.GPS.DOPNumerator;
					if (!hashtable.ContainsKey(dOPNumerator))
					{
						hashtable.Add(dOPNumerator, shellObjectParent.Properties.CreateTypedProperty<uint?>(dOPNumerator));
					}
					return hashtable[dOPNumerator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<double?> ImageDirection
			{
				get
				{
					PropertyKey imageDirection = SystemProperties.System.GPS.ImageDirection;
					if (!hashtable.ContainsKey(imageDirection))
					{
						hashtable.Add(imageDirection, shellObjectParent.Properties.CreateTypedProperty<double?>(imageDirection));
					}
					return hashtable[imageDirection] as ShellProperty<double?>;
				}
			}

			public ShellProperty<uint?> ImageDirectionDenominator
			{
				get
				{
					PropertyKey imageDirectionDenominator = SystemProperties.System.GPS.ImageDirectionDenominator;
					if (!hashtable.ContainsKey(imageDirectionDenominator))
					{
						hashtable.Add(imageDirectionDenominator, shellObjectParent.Properties.CreateTypedProperty<uint?>(imageDirectionDenominator));
					}
					return hashtable[imageDirectionDenominator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> ImageDirectionNumerator
			{
				get
				{
					PropertyKey imageDirectionNumerator = SystemProperties.System.GPS.ImageDirectionNumerator;
					if (!hashtable.ContainsKey(imageDirectionNumerator))
					{
						hashtable.Add(imageDirectionNumerator, shellObjectParent.Properties.CreateTypedProperty<uint?>(imageDirectionNumerator));
					}
					return hashtable[imageDirectionNumerator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string> ImageDirectionRef
			{
				get
				{
					PropertyKey imageDirectionRef = SystemProperties.System.GPS.ImageDirectionRef;
					if (!hashtable.ContainsKey(imageDirectionRef))
					{
						hashtable.Add(imageDirectionRef, shellObjectParent.Properties.CreateTypedProperty<string>(imageDirectionRef));
					}
					return hashtable[imageDirectionRef] as ShellProperty<string>;
				}
			}

			public ShellProperty<double[]> Latitude
			{
				get
				{
					PropertyKey latitude = SystemProperties.System.GPS.Latitude;
					if (!hashtable.ContainsKey(latitude))
					{
						hashtable.Add(latitude, shellObjectParent.Properties.CreateTypedProperty<double[]>(latitude));
					}
					return hashtable[latitude] as ShellProperty<double[]>;
				}
			}

			public ShellProperty<uint[]> LatitudeDenominator
			{
				get
				{
					PropertyKey latitudeDenominator = SystemProperties.System.GPS.LatitudeDenominator;
					if (!hashtable.ContainsKey(latitudeDenominator))
					{
						hashtable.Add(latitudeDenominator, shellObjectParent.Properties.CreateTypedProperty<uint[]>(latitudeDenominator));
					}
					return hashtable[latitudeDenominator] as ShellProperty<uint[]>;
				}
			}

			public ShellProperty<uint[]> LatitudeNumerator
			{
				get
				{
					PropertyKey latitudeNumerator = SystemProperties.System.GPS.LatitudeNumerator;
					if (!hashtable.ContainsKey(latitudeNumerator))
					{
						hashtable.Add(latitudeNumerator, shellObjectParent.Properties.CreateTypedProperty<uint[]>(latitudeNumerator));
					}
					return hashtable[latitudeNumerator] as ShellProperty<uint[]>;
				}
			}

			public ShellProperty<string> LatitudeRef
			{
				get
				{
					PropertyKey latitudeRef = SystemProperties.System.GPS.LatitudeRef;
					if (!hashtable.ContainsKey(latitudeRef))
					{
						hashtable.Add(latitudeRef, shellObjectParent.Properties.CreateTypedProperty<string>(latitudeRef));
					}
					return hashtable[latitudeRef] as ShellProperty<string>;
				}
			}

			public ShellProperty<double[]> Longitude
			{
				get
				{
					PropertyKey longitude = SystemProperties.System.GPS.Longitude;
					if (!hashtable.ContainsKey(longitude))
					{
						hashtable.Add(longitude, shellObjectParent.Properties.CreateTypedProperty<double[]>(longitude));
					}
					return hashtable[longitude] as ShellProperty<double[]>;
				}
			}

			public ShellProperty<uint[]> LongitudeDenominator
			{
				get
				{
					PropertyKey longitudeDenominator = SystemProperties.System.GPS.LongitudeDenominator;
					if (!hashtable.ContainsKey(longitudeDenominator))
					{
						hashtable.Add(longitudeDenominator, shellObjectParent.Properties.CreateTypedProperty<uint[]>(longitudeDenominator));
					}
					return hashtable[longitudeDenominator] as ShellProperty<uint[]>;
				}
			}

			public ShellProperty<uint[]> LongitudeNumerator
			{
				get
				{
					PropertyKey longitudeNumerator = SystemProperties.System.GPS.LongitudeNumerator;
					if (!hashtable.ContainsKey(longitudeNumerator))
					{
						hashtable.Add(longitudeNumerator, shellObjectParent.Properties.CreateTypedProperty<uint[]>(longitudeNumerator));
					}
					return hashtable[longitudeNumerator] as ShellProperty<uint[]>;
				}
			}

			public ShellProperty<string> LongitudeRef
			{
				get
				{
					PropertyKey longitudeRef = SystemProperties.System.GPS.LongitudeRef;
					if (!hashtable.ContainsKey(longitudeRef))
					{
						hashtable.Add(longitudeRef, shellObjectParent.Properties.CreateTypedProperty<string>(longitudeRef));
					}
					return hashtable[longitudeRef] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> MapDatum
			{
				get
				{
					PropertyKey mapDatum = SystemProperties.System.GPS.MapDatum;
					if (!hashtable.ContainsKey(mapDatum))
					{
						hashtable.Add(mapDatum, shellObjectParent.Properties.CreateTypedProperty<string>(mapDatum));
					}
					return hashtable[mapDatum] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> MeasureMode
			{
				get
				{
					PropertyKey measureMode = SystemProperties.System.GPS.MeasureMode;
					if (!hashtable.ContainsKey(measureMode))
					{
						hashtable.Add(measureMode, shellObjectParent.Properties.CreateTypedProperty<string>(measureMode));
					}
					return hashtable[measureMode] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ProcessingMethod
			{
				get
				{
					PropertyKey processingMethod = SystemProperties.System.GPS.ProcessingMethod;
					if (!hashtable.ContainsKey(processingMethod))
					{
						hashtable.Add(processingMethod, shellObjectParent.Properties.CreateTypedProperty<string>(processingMethod));
					}
					return hashtable[processingMethod] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> Satellites
			{
				get
				{
					PropertyKey satellites = SystemProperties.System.GPS.Satellites;
					if (!hashtable.ContainsKey(satellites))
					{
						hashtable.Add(satellites, shellObjectParent.Properties.CreateTypedProperty<string>(satellites));
					}
					return hashtable[satellites] as ShellProperty<string>;
				}
			}

			public ShellProperty<double?> Speed
			{
				get
				{
					PropertyKey speed = SystemProperties.System.GPS.Speed;
					if (!hashtable.ContainsKey(speed))
					{
						hashtable.Add(speed, shellObjectParent.Properties.CreateTypedProperty<double?>(speed));
					}
					return hashtable[speed] as ShellProperty<double?>;
				}
			}

			public ShellProperty<uint?> SpeedDenominator
			{
				get
				{
					PropertyKey speedDenominator = SystemProperties.System.GPS.SpeedDenominator;
					if (!hashtable.ContainsKey(speedDenominator))
					{
						hashtable.Add(speedDenominator, shellObjectParent.Properties.CreateTypedProperty<uint?>(speedDenominator));
					}
					return hashtable[speedDenominator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> SpeedNumerator
			{
				get
				{
					PropertyKey speedNumerator = SystemProperties.System.GPS.SpeedNumerator;
					if (!hashtable.ContainsKey(speedNumerator))
					{
						hashtable.Add(speedNumerator, shellObjectParent.Properties.CreateTypedProperty<uint?>(speedNumerator));
					}
					return hashtable[speedNumerator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string> SpeedRef
			{
				get
				{
					PropertyKey speedRef = SystemProperties.System.GPS.SpeedRef;
					if (!hashtable.ContainsKey(speedRef))
					{
						hashtable.Add(speedRef, shellObjectParent.Properties.CreateTypedProperty<string>(speedRef));
					}
					return hashtable[speedRef] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> Status
			{
				get
				{
					PropertyKey status = SystemProperties.System.GPS.Status;
					if (!hashtable.ContainsKey(status))
					{
						hashtable.Add(status, shellObjectParent.Properties.CreateTypedProperty<string>(status));
					}
					return hashtable[status] as ShellProperty<string>;
				}
			}

			public ShellProperty<double?> Track
			{
				get
				{
					PropertyKey track = SystemProperties.System.GPS.Track;
					if (!hashtable.ContainsKey(track))
					{
						hashtable.Add(track, shellObjectParent.Properties.CreateTypedProperty<double?>(track));
					}
					return hashtable[track] as ShellProperty<double?>;
				}
			}

			public ShellProperty<uint?> TrackDenominator
			{
				get
				{
					PropertyKey trackDenominator = SystemProperties.System.GPS.TrackDenominator;
					if (!hashtable.ContainsKey(trackDenominator))
					{
						hashtable.Add(trackDenominator, shellObjectParent.Properties.CreateTypedProperty<uint?>(trackDenominator));
					}
					return hashtable[trackDenominator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> TrackNumerator
			{
				get
				{
					PropertyKey trackNumerator = SystemProperties.System.GPS.TrackNumerator;
					if (!hashtable.ContainsKey(trackNumerator))
					{
						hashtable.Add(trackNumerator, shellObjectParent.Properties.CreateTypedProperty<uint?>(trackNumerator));
					}
					return hashtable[trackNumerator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string> TrackRef
			{
				get
				{
					PropertyKey trackRef = SystemProperties.System.GPS.TrackRef;
					if (!hashtable.ContainsKey(trackRef))
					{
						hashtable.Add(trackRef, shellObjectParent.Properties.CreateTypedProperty<string>(trackRef));
					}
					return hashtable[trackRef] as ShellProperty<string>;
				}
			}

			public ShellProperty<byte[]> VersionID
			{
				get
				{
					PropertyKey versionID = SystemProperties.System.GPS.VersionID;
					if (!hashtable.ContainsKey(versionID))
					{
						hashtable.Add(versionID, shellObjectParent.Properties.CreateTypedProperty<byte[]>(versionID));
					}
					return hashtable[versionID] as ShellProperty<byte[]>;
				}
			}

			internal PropertySystemGPS(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemIdentity : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<byte[]> Blob
			{
				get
				{
					PropertyKey blob = SystemProperties.System.Identity.Blob;
					if (!hashtable.ContainsKey(blob))
					{
						hashtable.Add(blob, shellObjectParent.Properties.CreateTypedProperty<byte[]>(blob));
					}
					return hashtable[blob] as ShellProperty<byte[]>;
				}
			}

			public ShellProperty<string> DisplayName
			{
				get
				{
					PropertyKey displayName = SystemProperties.System.Identity.DisplayName;
					if (!hashtable.ContainsKey(displayName))
					{
						hashtable.Add(displayName, shellObjectParent.Properties.CreateTypedProperty<string>(displayName));
					}
					return hashtable[displayName] as ShellProperty<string>;
				}
			}

			public ShellProperty<bool?> IsMeIdentity
			{
				get
				{
					PropertyKey isMeIdentity = SystemProperties.System.Identity.IsMeIdentity;
					if (!hashtable.ContainsKey(isMeIdentity))
					{
						hashtable.Add(isMeIdentity, shellObjectParent.Properties.CreateTypedProperty<bool?>(isMeIdentity));
					}
					return hashtable[isMeIdentity] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<string> PrimaryEmailAddress
			{
				get
				{
					PropertyKey primaryEmailAddress = SystemProperties.System.Identity.PrimaryEmailAddress;
					if (!hashtable.ContainsKey(primaryEmailAddress))
					{
						hashtable.Add(primaryEmailAddress, shellObjectParent.Properties.CreateTypedProperty<string>(primaryEmailAddress));
					}
					return hashtable[primaryEmailAddress] as ShellProperty<string>;
				}
			}

			public ShellProperty<IntPtr?> ProviderID
			{
				get
				{
					PropertyKey providerID = SystemProperties.System.Identity.ProviderID;
					if (!hashtable.ContainsKey(providerID))
					{
						hashtable.Add(providerID, shellObjectParent.Properties.CreateTypedProperty<IntPtr?>(providerID));
					}
					return hashtable[providerID] as ShellProperty<IntPtr?>;
				}
			}

			public ShellProperty<string> UniqueID
			{
				get
				{
					PropertyKey uniqueID = SystemProperties.System.Identity.UniqueID;
					if (!hashtable.ContainsKey(uniqueID))
					{
						hashtable.Add(uniqueID, shellObjectParent.Properties.CreateTypedProperty<string>(uniqueID));
					}
					return hashtable[uniqueID] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> UserName
			{
				get
				{
					PropertyKey userName = SystemProperties.System.Identity.UserName;
					if (!hashtable.ContainsKey(userName))
					{
						hashtable.Add(userName, shellObjectParent.Properties.CreateTypedProperty<string>(userName));
					}
					return hashtable[userName] as ShellProperty<string>;
				}
			}

			internal PropertySystemIdentity(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemIdentityProvider : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<string> Name
			{
				get
				{
					PropertyKey name = SystemProperties.System.IdentityProvider.Name;
					if (!hashtable.ContainsKey(name))
					{
						hashtable.Add(name, shellObjectParent.Properties.CreateTypedProperty<string>(name));
					}
					return hashtable[name] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> Picture
			{
				get
				{
					PropertyKey picture = SystemProperties.System.IdentityProvider.Picture;
					if (!hashtable.ContainsKey(picture))
					{
						hashtable.Add(picture, shellObjectParent.Properties.CreateTypedProperty<string>(picture));
					}
					return hashtable[picture] as ShellProperty<string>;
				}
			}

			internal PropertySystemIdentityProvider(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemImage : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<uint?> BitDepth
			{
				get
				{
					PropertyKey bitDepth = SystemProperties.System.Image.BitDepth;
					if (!hashtable.ContainsKey(bitDepth))
					{
						hashtable.Add(bitDepth, shellObjectParent.Properties.CreateTypedProperty<uint?>(bitDepth));
					}
					return hashtable[bitDepth] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<ushort?> ColorSpace
			{
				get
				{
					PropertyKey colorSpace = SystemProperties.System.Image.ColorSpace;
					if (!hashtable.ContainsKey(colorSpace))
					{
						hashtable.Add(colorSpace, shellObjectParent.Properties.CreateTypedProperty<ushort?>(colorSpace));
					}
					return hashtable[colorSpace] as ShellProperty<ushort?>;
				}
			}

			public ShellProperty<double?> CompressedBitsPerPixel
			{
				get
				{
					PropertyKey compressedBitsPerPixel = SystemProperties.System.Image.CompressedBitsPerPixel;
					if (!hashtable.ContainsKey(compressedBitsPerPixel))
					{
						hashtable.Add(compressedBitsPerPixel, shellObjectParent.Properties.CreateTypedProperty<double?>(compressedBitsPerPixel));
					}
					return hashtable[compressedBitsPerPixel] as ShellProperty<double?>;
				}
			}

			public ShellProperty<uint?> CompressedBitsPerPixelDenominator
			{
				get
				{
					PropertyKey compressedBitsPerPixelDenominator = SystemProperties.System.Image.CompressedBitsPerPixelDenominator;
					if (!hashtable.ContainsKey(compressedBitsPerPixelDenominator))
					{
						hashtable.Add(compressedBitsPerPixelDenominator, shellObjectParent.Properties.CreateTypedProperty<uint?>(compressedBitsPerPixelDenominator));
					}
					return hashtable[compressedBitsPerPixelDenominator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> CompressedBitsPerPixelNumerator
			{
				get
				{
					PropertyKey compressedBitsPerPixelNumerator = SystemProperties.System.Image.CompressedBitsPerPixelNumerator;
					if (!hashtable.ContainsKey(compressedBitsPerPixelNumerator))
					{
						hashtable.Add(compressedBitsPerPixelNumerator, shellObjectParent.Properties.CreateTypedProperty<uint?>(compressedBitsPerPixelNumerator));
					}
					return hashtable[compressedBitsPerPixelNumerator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<ushort?> Compression
			{
				get
				{
					PropertyKey compression = SystemProperties.System.Image.Compression;
					if (!hashtable.ContainsKey(compression))
					{
						hashtable.Add(compression, shellObjectParent.Properties.CreateTypedProperty<ushort?>(compression));
					}
					return hashtable[compression] as ShellProperty<ushort?>;
				}
			}

			public ShellProperty<string> CompressionText
			{
				get
				{
					PropertyKey compressionText = SystemProperties.System.Image.CompressionText;
					if (!hashtable.ContainsKey(compressionText))
					{
						hashtable.Add(compressionText, shellObjectParent.Properties.CreateTypedProperty<string>(compressionText));
					}
					return hashtable[compressionText] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> Dimensions
			{
				get
				{
					PropertyKey dimensions = SystemProperties.System.Image.Dimensions;
					if (!hashtable.ContainsKey(dimensions))
					{
						hashtable.Add(dimensions, shellObjectParent.Properties.CreateTypedProperty<string>(dimensions));
					}
					return hashtable[dimensions] as ShellProperty<string>;
				}
			}

			public ShellProperty<double?> HorizontalResolution
			{
				get
				{
					PropertyKey horizontalResolution = SystemProperties.System.Image.HorizontalResolution;
					if (!hashtable.ContainsKey(horizontalResolution))
					{
						hashtable.Add(horizontalResolution, shellObjectParent.Properties.CreateTypedProperty<double?>(horizontalResolution));
					}
					return hashtable[horizontalResolution] as ShellProperty<double?>;
				}
			}

			public ShellProperty<uint?> HorizontalSize
			{
				get
				{
					PropertyKey horizontalSize = SystemProperties.System.Image.HorizontalSize;
					if (!hashtable.ContainsKey(horizontalSize))
					{
						hashtable.Add(horizontalSize, shellObjectParent.Properties.CreateTypedProperty<uint?>(horizontalSize));
					}
					return hashtable[horizontalSize] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string> ImageID
			{
				get
				{
					PropertyKey imageID = SystemProperties.System.Image.ImageID;
					if (!hashtable.ContainsKey(imageID))
					{
						hashtable.Add(imageID, shellObjectParent.Properties.CreateTypedProperty<string>(imageID));
					}
					return hashtable[imageID] as ShellProperty<string>;
				}
			}

			public ShellProperty<short?> ResolutionUnit
			{
				get
				{
					PropertyKey resolutionUnit = SystemProperties.System.Image.ResolutionUnit;
					if (!hashtable.ContainsKey(resolutionUnit))
					{
						hashtable.Add(resolutionUnit, shellObjectParent.Properties.CreateTypedProperty<short?>(resolutionUnit));
					}
					return hashtable[resolutionUnit] as ShellProperty<short?>;
				}
			}

			public ShellProperty<double?> VerticalResolution
			{
				get
				{
					PropertyKey verticalResolution = SystemProperties.System.Image.VerticalResolution;
					if (!hashtable.ContainsKey(verticalResolution))
					{
						hashtable.Add(verticalResolution, shellObjectParent.Properties.CreateTypedProperty<double?>(verticalResolution));
					}
					return hashtable[verticalResolution] as ShellProperty<double?>;
				}
			}

			public ShellProperty<uint?> VerticalSize
			{
				get
				{
					PropertyKey verticalSize = SystemProperties.System.Image.VerticalSize;
					if (!hashtable.ContainsKey(verticalSize))
					{
						hashtable.Add(verticalSize, shellObjectParent.Properties.CreateTypedProperty<uint?>(verticalSize));
					}
					return hashtable[verticalSize] as ShellProperty<uint?>;
				}
			}

			internal PropertySystemImage(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemJournal : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<string[]> Contacts
			{
				get
				{
					PropertyKey contacts = SystemProperties.System.Journal.Contacts;
					if (!hashtable.ContainsKey(contacts))
					{
						hashtable.Add(contacts, shellObjectParent.Properties.CreateTypedProperty<string[]>(contacts));
					}
					return hashtable[contacts] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string> EntryType
			{
				get
				{
					PropertyKey entryType = SystemProperties.System.Journal.EntryType;
					if (!hashtable.ContainsKey(entryType))
					{
						hashtable.Add(entryType, shellObjectParent.Properties.CreateTypedProperty<string>(entryType));
					}
					return hashtable[entryType] as ShellProperty<string>;
				}
			}

			internal PropertySystemJournal(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemLayoutPattern : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<string> ContentViewModeForBrowse
			{
				get
				{
					PropertyKey contentViewModeForBrowse = SystemProperties.System.LayoutPattern.ContentViewModeForBrowse;
					if (!hashtable.ContainsKey(contentViewModeForBrowse))
					{
						hashtable.Add(contentViewModeForBrowse, shellObjectParent.Properties.CreateTypedProperty<string>(contentViewModeForBrowse));
					}
					return hashtable[contentViewModeForBrowse] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ContentViewModeForSearch
			{
				get
				{
					PropertyKey contentViewModeForSearch = SystemProperties.System.LayoutPattern.ContentViewModeForSearch;
					if (!hashtable.ContainsKey(contentViewModeForSearch))
					{
						hashtable.Add(contentViewModeForSearch, shellObjectParent.Properties.CreateTypedProperty<string>(contentViewModeForSearch));
					}
					return hashtable[contentViewModeForSearch] as ShellProperty<string>;
				}
			}

			internal PropertySystemLayoutPattern(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemLink : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<string> Arguments
			{
				get
				{
					PropertyKey arguments = SystemProperties.System.Link.Arguments;
					if (!hashtable.ContainsKey(arguments))
					{
						hashtable.Add(arguments, shellObjectParent.Properties.CreateTypedProperty<string>(arguments));
					}
					return hashtable[arguments] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> Comment
			{
				get
				{
					PropertyKey comment = SystemProperties.System.Link.Comment;
					if (!hashtable.ContainsKey(comment))
					{
						hashtable.Add(comment, shellObjectParent.Properties.CreateTypedProperty<string>(comment));
					}
					return hashtable[comment] as ShellProperty<string>;
				}
			}

			public ShellProperty<DateTime?> DateVisited
			{
				get
				{
					PropertyKey dateVisited = SystemProperties.System.Link.DateVisited;
					if (!hashtable.ContainsKey(dateVisited))
					{
						hashtable.Add(dateVisited, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(dateVisited));
					}
					return hashtable[dateVisited] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<string> Description
			{
				get
				{
					PropertyKey description = SystemProperties.System.Link.Description;
					if (!hashtable.ContainsKey(description))
					{
						hashtable.Add(description, shellObjectParent.Properties.CreateTypedProperty<string>(description));
					}
					return hashtable[description] as ShellProperty<string>;
				}
			}

			public ShellProperty<int?> Status
			{
				get
				{
					PropertyKey status = SystemProperties.System.Link.Status;
					if (!hashtable.ContainsKey(status))
					{
						hashtable.Add(status, shellObjectParent.Properties.CreateTypedProperty<int?>(status));
					}
					return hashtable[status] as ShellProperty<int?>;
				}
			}

			public ShellProperty<string[]> TargetExtension
			{
				get
				{
					PropertyKey targetExtension = SystemProperties.System.Link.TargetExtension;
					if (!hashtable.ContainsKey(targetExtension))
					{
						hashtable.Add(targetExtension, shellObjectParent.Properties.CreateTypedProperty<string[]>(targetExtension));
					}
					return hashtable[targetExtension] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string> TargetParsingPath
			{
				get
				{
					PropertyKey targetParsingPath = SystemProperties.System.Link.TargetParsingPath;
					if (!hashtable.ContainsKey(targetParsingPath))
					{
						hashtable.Add(targetParsingPath, shellObjectParent.Properties.CreateTypedProperty<string>(targetParsingPath));
					}
					return hashtable[targetParsingPath] as ShellProperty<string>;
				}
			}

			public ShellProperty<uint?> TargetSFGAOFlags
			{
				get
				{
					PropertyKey targetSFGAOFlags = SystemProperties.System.Link.TargetSFGAOFlags;
					if (!hashtable.ContainsKey(targetSFGAOFlags))
					{
						hashtable.Add(targetSFGAOFlags, shellObjectParent.Properties.CreateTypedProperty<uint?>(targetSFGAOFlags));
					}
					return hashtable[targetSFGAOFlags] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string[]> TargetSFGAOFlagsStrings
			{
				get
				{
					PropertyKey targetSFGAOFlagsStrings = SystemProperties.System.Link.TargetSFGAOFlagsStrings;
					if (!hashtable.ContainsKey(targetSFGAOFlagsStrings))
					{
						hashtable.Add(targetSFGAOFlagsStrings, shellObjectParent.Properties.CreateTypedProperty<string[]>(targetSFGAOFlagsStrings));
					}
					return hashtable[targetSFGAOFlagsStrings] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string> TargetUrl
			{
				get
				{
					PropertyKey targetUrl = SystemProperties.System.Link.TargetUrl;
					if (!hashtable.ContainsKey(targetUrl))
					{
						hashtable.Add(targetUrl, shellObjectParent.Properties.CreateTypedProperty<string>(targetUrl));
					}
					return hashtable[targetUrl] as ShellProperty<string>;
				}
			}

			internal PropertySystemLink(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemMedia : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<string> AuthorUrl
			{
				get
				{
					PropertyKey authorUrl = SystemProperties.System.Media.AuthorUrl;
					if (!hashtable.ContainsKey(authorUrl))
					{
						hashtable.Add(authorUrl, shellObjectParent.Properties.CreateTypedProperty<string>(authorUrl));
					}
					return hashtable[authorUrl] as ShellProperty<string>;
				}
			}

			public ShellProperty<uint?> AverageLevel
			{
				get
				{
					PropertyKey averageLevel = SystemProperties.System.Media.AverageLevel;
					if (!hashtable.ContainsKey(averageLevel))
					{
						hashtable.Add(averageLevel, shellObjectParent.Properties.CreateTypedProperty<uint?>(averageLevel));
					}
					return hashtable[averageLevel] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string> ClassPrimaryID
			{
				get
				{
					PropertyKey classPrimaryID = SystemProperties.System.Media.ClassPrimaryID;
					if (!hashtable.ContainsKey(classPrimaryID))
					{
						hashtable.Add(classPrimaryID, shellObjectParent.Properties.CreateTypedProperty<string>(classPrimaryID));
					}
					return hashtable[classPrimaryID] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ClassSecondaryID
			{
				get
				{
					PropertyKey classSecondaryID = SystemProperties.System.Media.ClassSecondaryID;
					if (!hashtable.ContainsKey(classSecondaryID))
					{
						hashtable.Add(classSecondaryID, shellObjectParent.Properties.CreateTypedProperty<string>(classSecondaryID));
					}
					return hashtable[classSecondaryID] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> CollectionGroupID
			{
				get
				{
					PropertyKey collectionGroupID = SystemProperties.System.Media.CollectionGroupID;
					if (!hashtable.ContainsKey(collectionGroupID))
					{
						hashtable.Add(collectionGroupID, shellObjectParent.Properties.CreateTypedProperty<string>(collectionGroupID));
					}
					return hashtable[collectionGroupID] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> CollectionID
			{
				get
				{
					PropertyKey collectionID = SystemProperties.System.Media.CollectionID;
					if (!hashtable.ContainsKey(collectionID))
					{
						hashtable.Add(collectionID, shellObjectParent.Properties.CreateTypedProperty<string>(collectionID));
					}
					return hashtable[collectionID] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ContentDistributor
			{
				get
				{
					PropertyKey contentDistributor = SystemProperties.System.Media.ContentDistributor;
					if (!hashtable.ContainsKey(contentDistributor))
					{
						hashtable.Add(contentDistributor, shellObjectParent.Properties.CreateTypedProperty<string>(contentDistributor));
					}
					return hashtable[contentDistributor] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ContentID
			{
				get
				{
					PropertyKey contentID = SystemProperties.System.Media.ContentID;
					if (!hashtable.ContainsKey(contentID))
					{
						hashtable.Add(contentID, shellObjectParent.Properties.CreateTypedProperty<string>(contentID));
					}
					return hashtable[contentID] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> CreatorApplication
			{
				get
				{
					PropertyKey creatorApplication = SystemProperties.System.Media.CreatorApplication;
					if (!hashtable.ContainsKey(creatorApplication))
					{
						hashtable.Add(creatorApplication, shellObjectParent.Properties.CreateTypedProperty<string>(creatorApplication));
					}
					return hashtable[creatorApplication] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> CreatorApplicationVersion
			{
				get
				{
					PropertyKey creatorApplicationVersion = SystemProperties.System.Media.CreatorApplicationVersion;
					if (!hashtable.ContainsKey(creatorApplicationVersion))
					{
						hashtable.Add(creatorApplicationVersion, shellObjectParent.Properties.CreateTypedProperty<string>(creatorApplicationVersion));
					}
					return hashtable[creatorApplicationVersion] as ShellProperty<string>;
				}
			}

			public ShellProperty<DateTime?> DateEncoded
			{
				get
				{
					PropertyKey dateEncoded = SystemProperties.System.Media.DateEncoded;
					if (!hashtable.ContainsKey(dateEncoded))
					{
						hashtable.Add(dateEncoded, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(dateEncoded));
					}
					return hashtable[dateEncoded] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<string> DateReleased
			{
				get
				{
					PropertyKey dateReleased = SystemProperties.System.Media.DateReleased;
					if (!hashtable.ContainsKey(dateReleased))
					{
						hashtable.Add(dateReleased, shellObjectParent.Properties.CreateTypedProperty<string>(dateReleased));
					}
					return hashtable[dateReleased] as ShellProperty<string>;
				}
			}

			public ShellProperty<ulong?> Duration
			{
				get
				{
					PropertyKey duration = SystemProperties.System.Media.Duration;
					if (!hashtable.ContainsKey(duration))
					{
						hashtable.Add(duration, shellObjectParent.Properties.CreateTypedProperty<ulong?>(duration));
					}
					return hashtable[duration] as ShellProperty<ulong?>;
				}
			}

			public ShellProperty<string> DVDID
			{
				get
				{
					PropertyKey dVDID = SystemProperties.System.Media.DVDID;
					if (!hashtable.ContainsKey(dVDID))
					{
						hashtable.Add(dVDID, shellObjectParent.Properties.CreateTypedProperty<string>(dVDID));
					}
					return hashtable[dVDID] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> EncodedBy
			{
				get
				{
					PropertyKey encodedBy = SystemProperties.System.Media.EncodedBy;
					if (!hashtable.ContainsKey(encodedBy))
					{
						hashtable.Add(encodedBy, shellObjectParent.Properties.CreateTypedProperty<string>(encodedBy));
					}
					return hashtable[encodedBy] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> EncodingSettings
			{
				get
				{
					PropertyKey encodingSettings = SystemProperties.System.Media.EncodingSettings;
					if (!hashtable.ContainsKey(encodingSettings))
					{
						hashtable.Add(encodingSettings, shellObjectParent.Properties.CreateTypedProperty<string>(encodingSettings));
					}
					return hashtable[encodingSettings] as ShellProperty<string>;
				}
			}

			public ShellProperty<uint?> FrameCount
			{
				get
				{
					PropertyKey frameCount = SystemProperties.System.Media.FrameCount;
					if (!hashtable.ContainsKey(frameCount))
					{
						hashtable.Add(frameCount, shellObjectParent.Properties.CreateTypedProperty<uint?>(frameCount));
					}
					return hashtable[frameCount] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string> MCDI
			{
				get
				{
					PropertyKey mCDI = SystemProperties.System.Media.MCDI;
					if (!hashtable.ContainsKey(mCDI))
					{
						hashtable.Add(mCDI, shellObjectParent.Properties.CreateTypedProperty<string>(mCDI));
					}
					return hashtable[mCDI] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> MetadataContentProvider
			{
				get
				{
					PropertyKey metadataContentProvider = SystemProperties.System.Media.MetadataContentProvider;
					if (!hashtable.ContainsKey(metadataContentProvider))
					{
						hashtable.Add(metadataContentProvider, shellObjectParent.Properties.CreateTypedProperty<string>(metadataContentProvider));
					}
					return hashtable[metadataContentProvider] as ShellProperty<string>;
				}
			}

			public ShellProperty<string[]> Producer
			{
				get
				{
					PropertyKey producer = SystemProperties.System.Media.Producer;
					if (!hashtable.ContainsKey(producer))
					{
						hashtable.Add(producer, shellObjectParent.Properties.CreateTypedProperty<string[]>(producer));
					}
					return hashtable[producer] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string> PromotionUrl
			{
				get
				{
					PropertyKey promotionUrl = SystemProperties.System.Media.PromotionUrl;
					if (!hashtable.ContainsKey(promotionUrl))
					{
						hashtable.Add(promotionUrl, shellObjectParent.Properties.CreateTypedProperty<string>(promotionUrl));
					}
					return hashtable[promotionUrl] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ProtectionType
			{
				get
				{
					PropertyKey protectionType = SystemProperties.System.Media.ProtectionType;
					if (!hashtable.ContainsKey(protectionType))
					{
						hashtable.Add(protectionType, shellObjectParent.Properties.CreateTypedProperty<string>(protectionType));
					}
					return hashtable[protectionType] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ProviderRating
			{
				get
				{
					PropertyKey providerRating = SystemProperties.System.Media.ProviderRating;
					if (!hashtable.ContainsKey(providerRating))
					{
						hashtable.Add(providerRating, shellObjectParent.Properties.CreateTypedProperty<string>(providerRating));
					}
					return hashtable[providerRating] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ProviderStyle
			{
				get
				{
					PropertyKey providerStyle = SystemProperties.System.Media.ProviderStyle;
					if (!hashtable.ContainsKey(providerStyle))
					{
						hashtable.Add(providerStyle, shellObjectParent.Properties.CreateTypedProperty<string>(providerStyle));
					}
					return hashtable[providerStyle] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> Publisher
			{
				get
				{
					PropertyKey publisher = SystemProperties.System.Media.Publisher;
					if (!hashtable.ContainsKey(publisher))
					{
						hashtable.Add(publisher, shellObjectParent.Properties.CreateTypedProperty<string>(publisher));
					}
					return hashtable[publisher] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> SubscriptionContentId
			{
				get
				{
					PropertyKey subscriptionContentId = SystemProperties.System.Media.SubscriptionContentId;
					if (!hashtable.ContainsKey(subscriptionContentId))
					{
						hashtable.Add(subscriptionContentId, shellObjectParent.Properties.CreateTypedProperty<string>(subscriptionContentId));
					}
					return hashtable[subscriptionContentId] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> Subtitle
			{
				get
				{
					PropertyKey subtitle = SystemProperties.System.Media.Subtitle;
					if (!hashtable.ContainsKey(subtitle))
					{
						hashtable.Add(subtitle, shellObjectParent.Properties.CreateTypedProperty<string>(subtitle));
					}
					return hashtable[subtitle] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> UniqueFileIdentifier
			{
				get
				{
					PropertyKey uniqueFileIdentifier = SystemProperties.System.Media.UniqueFileIdentifier;
					if (!hashtable.ContainsKey(uniqueFileIdentifier))
					{
						hashtable.Add(uniqueFileIdentifier, shellObjectParent.Properties.CreateTypedProperty<string>(uniqueFileIdentifier));
					}
					return hashtable[uniqueFileIdentifier] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> UserNoAutoInfo
			{
				get
				{
					PropertyKey userNoAutoInfo = SystemProperties.System.Media.UserNoAutoInfo;
					if (!hashtable.ContainsKey(userNoAutoInfo))
					{
						hashtable.Add(userNoAutoInfo, shellObjectParent.Properties.CreateTypedProperty<string>(userNoAutoInfo));
					}
					return hashtable[userNoAutoInfo] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> UserWebUrl
			{
				get
				{
					PropertyKey userWebUrl = SystemProperties.System.Media.UserWebUrl;
					if (!hashtable.ContainsKey(userWebUrl))
					{
						hashtable.Add(userWebUrl, shellObjectParent.Properties.CreateTypedProperty<string>(userWebUrl));
					}
					return hashtable[userWebUrl] as ShellProperty<string>;
				}
			}

			public ShellProperty<string[]> Writer
			{
				get
				{
					PropertyKey writer = SystemProperties.System.Media.Writer;
					if (!hashtable.ContainsKey(writer))
					{
						hashtable.Add(writer, shellObjectParent.Properties.CreateTypedProperty<string[]>(writer));
					}
					return hashtable[writer] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<uint?> Year
			{
				get
				{
					PropertyKey year = SystemProperties.System.Media.Year;
					if (!hashtable.ContainsKey(year))
					{
						hashtable.Add(year, shellObjectParent.Properties.CreateTypedProperty<uint?>(year));
					}
					return hashtable[year] as ShellProperty<uint?>;
				}
			}

			internal PropertySystemMedia(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemMessage : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<string> AttachmentContents
			{
				get
				{
					PropertyKey attachmentContents = SystemProperties.System.Message.AttachmentContents;
					if (!hashtable.ContainsKey(attachmentContents))
					{
						hashtable.Add(attachmentContents, shellObjectParent.Properties.CreateTypedProperty<string>(attachmentContents));
					}
					return hashtable[attachmentContents] as ShellProperty<string>;
				}
			}

			public ShellProperty<string[]> AttachmentNames
			{
				get
				{
					PropertyKey attachmentNames = SystemProperties.System.Message.AttachmentNames;
					if (!hashtable.ContainsKey(attachmentNames))
					{
						hashtable.Add(attachmentNames, shellObjectParent.Properties.CreateTypedProperty<string[]>(attachmentNames));
					}
					return hashtable[attachmentNames] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string[]> BccAddress
			{
				get
				{
					PropertyKey bccAddress = SystemProperties.System.Message.BccAddress;
					if (!hashtable.ContainsKey(bccAddress))
					{
						hashtable.Add(bccAddress, shellObjectParent.Properties.CreateTypedProperty<string[]>(bccAddress));
					}
					return hashtable[bccAddress] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string[]> BccName
			{
				get
				{
					PropertyKey bccName = SystemProperties.System.Message.BccName;
					if (!hashtable.ContainsKey(bccName))
					{
						hashtable.Add(bccName, shellObjectParent.Properties.CreateTypedProperty<string[]>(bccName));
					}
					return hashtable[bccName] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string[]> CcAddress
			{
				get
				{
					PropertyKey ccAddress = SystemProperties.System.Message.CcAddress;
					if (!hashtable.ContainsKey(ccAddress))
					{
						hashtable.Add(ccAddress, shellObjectParent.Properties.CreateTypedProperty<string[]>(ccAddress));
					}
					return hashtable[ccAddress] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string[]> CcName
			{
				get
				{
					PropertyKey ccName = SystemProperties.System.Message.CcName;
					if (!hashtable.ContainsKey(ccName))
					{
						hashtable.Add(ccName, shellObjectParent.Properties.CreateTypedProperty<string[]>(ccName));
					}
					return hashtable[ccName] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string> ConversationID
			{
				get
				{
					PropertyKey conversationID = SystemProperties.System.Message.ConversationID;
					if (!hashtable.ContainsKey(conversationID))
					{
						hashtable.Add(conversationID, shellObjectParent.Properties.CreateTypedProperty<string>(conversationID));
					}
					return hashtable[conversationID] as ShellProperty<string>;
				}
			}

			public ShellProperty<byte[]> ConversationIndex
			{
				get
				{
					PropertyKey conversationIndex = SystemProperties.System.Message.ConversationIndex;
					if (!hashtable.ContainsKey(conversationIndex))
					{
						hashtable.Add(conversationIndex, shellObjectParent.Properties.CreateTypedProperty<byte[]>(conversationIndex));
					}
					return hashtable[conversationIndex] as ShellProperty<byte[]>;
				}
			}

			public ShellProperty<DateTime?> DateReceived
			{
				get
				{
					PropertyKey dateReceived = SystemProperties.System.Message.DateReceived;
					if (!hashtable.ContainsKey(dateReceived))
					{
						hashtable.Add(dateReceived, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(dateReceived));
					}
					return hashtable[dateReceived] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<DateTime?> DateSent
			{
				get
				{
					PropertyKey dateSent = SystemProperties.System.Message.DateSent;
					if (!hashtable.ContainsKey(dateSent))
					{
						hashtable.Add(dateSent, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(dateSent));
					}
					return hashtable[dateSent] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<int?> Flags
			{
				get
				{
					PropertyKey flags = SystemProperties.System.Message.Flags;
					if (!hashtable.ContainsKey(flags))
					{
						hashtable.Add(flags, shellObjectParent.Properties.CreateTypedProperty<int?>(flags));
					}
					return hashtable[flags] as ShellProperty<int?>;
				}
			}

			public ShellProperty<string[]> FromAddress
			{
				get
				{
					PropertyKey fromAddress = SystemProperties.System.Message.FromAddress;
					if (!hashtable.ContainsKey(fromAddress))
					{
						hashtable.Add(fromAddress, shellObjectParent.Properties.CreateTypedProperty<string[]>(fromAddress));
					}
					return hashtable[fromAddress] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string[]> FromName
			{
				get
				{
					PropertyKey fromName = SystemProperties.System.Message.FromName;
					if (!hashtable.ContainsKey(fromName))
					{
						hashtable.Add(fromName, shellObjectParent.Properties.CreateTypedProperty<string[]>(fromName));
					}
					return hashtable[fromName] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<bool?> HasAttachments
			{
				get
				{
					PropertyKey hasAttachments = SystemProperties.System.Message.HasAttachments;
					if (!hashtable.ContainsKey(hasAttachments))
					{
						hashtable.Add(hasAttachments, shellObjectParent.Properties.CreateTypedProperty<bool?>(hasAttachments));
					}
					return hashtable[hasAttachments] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<int?> IsFwdOrReply
			{
				get
				{
					PropertyKey isFwdOrReply = SystemProperties.System.Message.IsFwdOrReply;
					if (!hashtable.ContainsKey(isFwdOrReply))
					{
						hashtable.Add(isFwdOrReply, shellObjectParent.Properties.CreateTypedProperty<int?>(isFwdOrReply));
					}
					return hashtable[isFwdOrReply] as ShellProperty<int?>;
				}
			}

			public ShellProperty<string> MessageClass
			{
				get
				{
					PropertyKey messageClass = SystemProperties.System.Message.MessageClass;
					if (!hashtable.ContainsKey(messageClass))
					{
						hashtable.Add(messageClass, shellObjectParent.Properties.CreateTypedProperty<string>(messageClass));
					}
					return hashtable[messageClass] as ShellProperty<string>;
				}
			}

			public ShellProperty<bool?> ProofInProgress
			{
				get
				{
					PropertyKey proofInProgress = SystemProperties.System.Message.ProofInProgress;
					if (!hashtable.ContainsKey(proofInProgress))
					{
						hashtable.Add(proofInProgress, shellObjectParent.Properties.CreateTypedProperty<bool?>(proofInProgress));
					}
					return hashtable[proofInProgress] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<string> SenderAddress
			{
				get
				{
					PropertyKey senderAddress = SystemProperties.System.Message.SenderAddress;
					if (!hashtable.ContainsKey(senderAddress))
					{
						hashtable.Add(senderAddress, shellObjectParent.Properties.CreateTypedProperty<string>(senderAddress));
					}
					return hashtable[senderAddress] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> SenderName
			{
				get
				{
					PropertyKey senderName = SystemProperties.System.Message.SenderName;
					if (!hashtable.ContainsKey(senderName))
					{
						hashtable.Add(senderName, shellObjectParent.Properties.CreateTypedProperty<string>(senderName));
					}
					return hashtable[senderName] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> Store
			{
				get
				{
					PropertyKey store = SystemProperties.System.Message.Store;
					if (!hashtable.ContainsKey(store))
					{
						hashtable.Add(store, shellObjectParent.Properties.CreateTypedProperty<string>(store));
					}
					return hashtable[store] as ShellProperty<string>;
				}
			}

			public ShellProperty<string[]> ToAddress
			{
				get
				{
					PropertyKey toAddress = SystemProperties.System.Message.ToAddress;
					if (!hashtable.ContainsKey(toAddress))
					{
						hashtable.Add(toAddress, shellObjectParent.Properties.CreateTypedProperty<string[]>(toAddress));
					}
					return hashtable[toAddress] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<int?> ToDoFlags
			{
				get
				{
					PropertyKey toDoFlags = SystemProperties.System.Message.ToDoFlags;
					if (!hashtable.ContainsKey(toDoFlags))
					{
						hashtable.Add(toDoFlags, shellObjectParent.Properties.CreateTypedProperty<int?>(toDoFlags));
					}
					return hashtable[toDoFlags] as ShellProperty<int?>;
				}
			}

			public ShellProperty<string> ToDoTitle
			{
				get
				{
					PropertyKey toDoTitle = SystemProperties.System.Message.ToDoTitle;
					if (!hashtable.ContainsKey(toDoTitle))
					{
						hashtable.Add(toDoTitle, shellObjectParent.Properties.CreateTypedProperty<string>(toDoTitle));
					}
					return hashtable[toDoTitle] as ShellProperty<string>;
				}
			}

			public ShellProperty<string[]> ToName
			{
				get
				{
					PropertyKey toName = SystemProperties.System.Message.ToName;
					if (!hashtable.ContainsKey(toName))
					{
						hashtable.Add(toName, shellObjectParent.Properties.CreateTypedProperty<string[]>(toName));
					}
					return hashtable[toName] as ShellProperty<string[]>;
				}
			}

			internal PropertySystemMessage(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemMusic : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<string> AlbumArtist
			{
				get
				{
					PropertyKey albumArtist = SystemProperties.System.Music.AlbumArtist;
					if (!hashtable.ContainsKey(albumArtist))
					{
						hashtable.Add(albumArtist, shellObjectParent.Properties.CreateTypedProperty<string>(albumArtist));
					}
					return hashtable[albumArtist] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> AlbumID
			{
				get
				{
					PropertyKey albumID = SystemProperties.System.Music.AlbumID;
					if (!hashtable.ContainsKey(albumID))
					{
						hashtable.Add(albumID, shellObjectParent.Properties.CreateTypedProperty<string>(albumID));
					}
					return hashtable[albumID] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> AlbumTitle
			{
				get
				{
					PropertyKey albumTitle = SystemProperties.System.Music.AlbumTitle;
					if (!hashtable.ContainsKey(albumTitle))
					{
						hashtable.Add(albumTitle, shellObjectParent.Properties.CreateTypedProperty<string>(albumTitle));
					}
					return hashtable[albumTitle] as ShellProperty<string>;
				}
			}

			public ShellProperty<string[]> Artist
			{
				get
				{
					PropertyKey artist = SystemProperties.System.Music.Artist;
					if (!hashtable.ContainsKey(artist))
					{
						hashtable.Add(artist, shellObjectParent.Properties.CreateTypedProperty<string[]>(artist));
					}
					return hashtable[artist] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string> BeatsPerMinute
			{
				get
				{
					PropertyKey beatsPerMinute = SystemProperties.System.Music.BeatsPerMinute;
					if (!hashtable.ContainsKey(beatsPerMinute))
					{
						hashtable.Add(beatsPerMinute, shellObjectParent.Properties.CreateTypedProperty<string>(beatsPerMinute));
					}
					return hashtable[beatsPerMinute] as ShellProperty<string>;
				}
			}

			public ShellProperty<string[]> Composer
			{
				get
				{
					PropertyKey composer = SystemProperties.System.Music.Composer;
					if (!hashtable.ContainsKey(composer))
					{
						hashtable.Add(composer, shellObjectParent.Properties.CreateTypedProperty<string[]>(composer));
					}
					return hashtable[composer] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string[]> Conductor
			{
				get
				{
					PropertyKey conductor = SystemProperties.System.Music.Conductor;
					if (!hashtable.ContainsKey(conductor))
					{
						hashtable.Add(conductor, shellObjectParent.Properties.CreateTypedProperty<string[]>(conductor));
					}
					return hashtable[conductor] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string> ContentGroupDescription
			{
				get
				{
					PropertyKey contentGroupDescription = SystemProperties.System.Music.ContentGroupDescription;
					if (!hashtable.ContainsKey(contentGroupDescription))
					{
						hashtable.Add(contentGroupDescription, shellObjectParent.Properties.CreateTypedProperty<string>(contentGroupDescription));
					}
					return hashtable[contentGroupDescription] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> DisplayArtist
			{
				get
				{
					PropertyKey displayArtist = SystemProperties.System.Music.DisplayArtist;
					if (!hashtable.ContainsKey(displayArtist))
					{
						hashtable.Add(displayArtist, shellObjectParent.Properties.CreateTypedProperty<string>(displayArtist));
					}
					return hashtable[displayArtist] as ShellProperty<string>;
				}
			}

			public ShellProperty<string[]> Genre
			{
				get
				{
					PropertyKey genre = SystemProperties.System.Music.Genre;
					if (!hashtable.ContainsKey(genre))
					{
						hashtable.Add(genre, shellObjectParent.Properties.CreateTypedProperty<string[]>(genre));
					}
					return hashtable[genre] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string> InitialKey
			{
				get
				{
					PropertyKey initialKey = SystemProperties.System.Music.InitialKey;
					if (!hashtable.ContainsKey(initialKey))
					{
						hashtable.Add(initialKey, shellObjectParent.Properties.CreateTypedProperty<string>(initialKey));
					}
					return hashtable[initialKey] as ShellProperty<string>;
				}
			}

			public ShellProperty<bool?> IsCompilation
			{
				get
				{
					PropertyKey isCompilation = SystemProperties.System.Music.IsCompilation;
					if (!hashtable.ContainsKey(isCompilation))
					{
						hashtable.Add(isCompilation, shellObjectParent.Properties.CreateTypedProperty<bool?>(isCompilation));
					}
					return hashtable[isCompilation] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<string> Lyrics
			{
				get
				{
					PropertyKey lyrics = SystemProperties.System.Music.Lyrics;
					if (!hashtable.ContainsKey(lyrics))
					{
						hashtable.Add(lyrics, shellObjectParent.Properties.CreateTypedProperty<string>(lyrics));
					}
					return hashtable[lyrics] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> Mood
			{
				get
				{
					PropertyKey mood = SystemProperties.System.Music.Mood;
					if (!hashtable.ContainsKey(mood))
					{
						hashtable.Add(mood, shellObjectParent.Properties.CreateTypedProperty<string>(mood));
					}
					return hashtable[mood] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> PartOfSet
			{
				get
				{
					PropertyKey partOfSet = SystemProperties.System.Music.PartOfSet;
					if (!hashtable.ContainsKey(partOfSet))
					{
						hashtable.Add(partOfSet, shellObjectParent.Properties.CreateTypedProperty<string>(partOfSet));
					}
					return hashtable[partOfSet] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> Period
			{
				get
				{
					PropertyKey period = SystemProperties.System.Music.Period;
					if (!hashtable.ContainsKey(period))
					{
						hashtable.Add(period, shellObjectParent.Properties.CreateTypedProperty<string>(period));
					}
					return hashtable[period] as ShellProperty<string>;
				}
			}

			public ShellProperty<byte[]> SynchronizedLyrics
			{
				get
				{
					PropertyKey synchronizedLyrics = SystemProperties.System.Music.SynchronizedLyrics;
					if (!hashtable.ContainsKey(synchronizedLyrics))
					{
						hashtable.Add(synchronizedLyrics, shellObjectParent.Properties.CreateTypedProperty<byte[]>(synchronizedLyrics));
					}
					return hashtable[synchronizedLyrics] as ShellProperty<byte[]>;
				}
			}

			public ShellProperty<uint?> TrackNumber
			{
				get
				{
					PropertyKey trackNumber = SystemProperties.System.Music.TrackNumber;
					if (!hashtable.ContainsKey(trackNumber))
					{
						hashtable.Add(trackNumber, shellObjectParent.Properties.CreateTypedProperty<uint?>(trackNumber));
					}
					return hashtable[trackNumber] as ShellProperty<uint?>;
				}
			}

			internal PropertySystemMusic(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemNote : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<ushort?> Color
			{
				get
				{
					PropertyKey color = SystemProperties.System.Note.Color;
					if (!hashtable.ContainsKey(color))
					{
						hashtable.Add(color, shellObjectParent.Properties.CreateTypedProperty<ushort?>(color));
					}
					return hashtable[color] as ShellProperty<ushort?>;
				}
			}

			public ShellProperty<string> ColorText
			{
				get
				{
					PropertyKey colorText = SystemProperties.System.Note.ColorText;
					if (!hashtable.ContainsKey(colorText))
					{
						hashtable.Add(colorText, shellObjectParent.Properties.CreateTypedProperty<string>(colorText));
					}
					return hashtable[colorText] as ShellProperty<string>;
				}
			}

			internal PropertySystemNote(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemPhoto : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<double?> Aperture
			{
				get
				{
					PropertyKey aperture = SystemProperties.System.Photo.Aperture;
					if (!hashtable.ContainsKey(aperture))
					{
						hashtable.Add(aperture, shellObjectParent.Properties.CreateTypedProperty<double?>(aperture));
					}
					return hashtable[aperture] as ShellProperty<double?>;
				}
			}

			public ShellProperty<uint?> ApertureDenominator
			{
				get
				{
					PropertyKey apertureDenominator = SystemProperties.System.Photo.ApertureDenominator;
					if (!hashtable.ContainsKey(apertureDenominator))
					{
						hashtable.Add(apertureDenominator, shellObjectParent.Properties.CreateTypedProperty<uint?>(apertureDenominator));
					}
					return hashtable[apertureDenominator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> ApertureNumerator
			{
				get
				{
					PropertyKey apertureNumerator = SystemProperties.System.Photo.ApertureNumerator;
					if (!hashtable.ContainsKey(apertureNumerator))
					{
						hashtable.Add(apertureNumerator, shellObjectParent.Properties.CreateTypedProperty<uint?>(apertureNumerator));
					}
					return hashtable[apertureNumerator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<double?> Brightness
			{
				get
				{
					PropertyKey brightness = SystemProperties.System.Photo.Brightness;
					if (!hashtable.ContainsKey(brightness))
					{
						hashtable.Add(brightness, shellObjectParent.Properties.CreateTypedProperty<double?>(brightness));
					}
					return hashtable[brightness] as ShellProperty<double?>;
				}
			}

			public ShellProperty<uint?> BrightnessDenominator
			{
				get
				{
					PropertyKey brightnessDenominator = SystemProperties.System.Photo.BrightnessDenominator;
					if (!hashtable.ContainsKey(brightnessDenominator))
					{
						hashtable.Add(brightnessDenominator, shellObjectParent.Properties.CreateTypedProperty<uint?>(brightnessDenominator));
					}
					return hashtable[brightnessDenominator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> BrightnessNumerator
			{
				get
				{
					PropertyKey brightnessNumerator = SystemProperties.System.Photo.BrightnessNumerator;
					if (!hashtable.ContainsKey(brightnessNumerator))
					{
						hashtable.Add(brightnessNumerator, shellObjectParent.Properties.CreateTypedProperty<uint?>(brightnessNumerator));
					}
					return hashtable[brightnessNumerator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string> CameraManufacturer
			{
				get
				{
					PropertyKey cameraManufacturer = SystemProperties.System.Photo.CameraManufacturer;
					if (!hashtable.ContainsKey(cameraManufacturer))
					{
						hashtable.Add(cameraManufacturer, shellObjectParent.Properties.CreateTypedProperty<string>(cameraManufacturer));
					}
					return hashtable[cameraManufacturer] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> CameraModel
			{
				get
				{
					PropertyKey cameraModel = SystemProperties.System.Photo.CameraModel;
					if (!hashtable.ContainsKey(cameraModel))
					{
						hashtable.Add(cameraModel, shellObjectParent.Properties.CreateTypedProperty<string>(cameraModel));
					}
					return hashtable[cameraModel] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> CameraSerialNumber
			{
				get
				{
					PropertyKey cameraSerialNumber = SystemProperties.System.Photo.CameraSerialNumber;
					if (!hashtable.ContainsKey(cameraSerialNumber))
					{
						hashtable.Add(cameraSerialNumber, shellObjectParent.Properties.CreateTypedProperty<string>(cameraSerialNumber));
					}
					return hashtable[cameraSerialNumber] as ShellProperty<string>;
				}
			}

			public ShellProperty<uint?> Contrast
			{
				get
				{
					PropertyKey contrast = SystemProperties.System.Photo.Contrast;
					if (!hashtable.ContainsKey(contrast))
					{
						hashtable.Add(contrast, shellObjectParent.Properties.CreateTypedProperty<uint?>(contrast));
					}
					return hashtable[contrast] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string> ContrastText
			{
				get
				{
					PropertyKey contrastText = SystemProperties.System.Photo.ContrastText;
					if (!hashtable.ContainsKey(contrastText))
					{
						hashtable.Add(contrastText, shellObjectParent.Properties.CreateTypedProperty<string>(contrastText));
					}
					return hashtable[contrastText] as ShellProperty<string>;
				}
			}

			public ShellProperty<DateTime?> DateTaken
			{
				get
				{
					PropertyKey dateTaken = SystemProperties.System.Photo.DateTaken;
					if (!hashtable.ContainsKey(dateTaken))
					{
						hashtable.Add(dateTaken, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(dateTaken));
					}
					return hashtable[dateTaken] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<double?> DigitalZoom
			{
				get
				{
					PropertyKey digitalZoom = SystemProperties.System.Photo.DigitalZoom;
					if (!hashtable.ContainsKey(digitalZoom))
					{
						hashtable.Add(digitalZoom, shellObjectParent.Properties.CreateTypedProperty<double?>(digitalZoom));
					}
					return hashtable[digitalZoom] as ShellProperty<double?>;
				}
			}

			public ShellProperty<uint?> DigitalZoomDenominator
			{
				get
				{
					PropertyKey digitalZoomDenominator = SystemProperties.System.Photo.DigitalZoomDenominator;
					if (!hashtable.ContainsKey(digitalZoomDenominator))
					{
						hashtable.Add(digitalZoomDenominator, shellObjectParent.Properties.CreateTypedProperty<uint?>(digitalZoomDenominator));
					}
					return hashtable[digitalZoomDenominator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> DigitalZoomNumerator
			{
				get
				{
					PropertyKey digitalZoomNumerator = SystemProperties.System.Photo.DigitalZoomNumerator;
					if (!hashtable.ContainsKey(digitalZoomNumerator))
					{
						hashtable.Add(digitalZoomNumerator, shellObjectParent.Properties.CreateTypedProperty<uint?>(digitalZoomNumerator));
					}
					return hashtable[digitalZoomNumerator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string[]> Event
			{
				get
				{
					PropertyKey @event = SystemProperties.System.Photo.Event;
					if (!hashtable.ContainsKey(@event))
					{
						hashtable.Add(@event, shellObjectParent.Properties.CreateTypedProperty<string[]>(@event));
					}
					return hashtable[@event] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<string> EXIFVersion
			{
				get
				{
					PropertyKey eXIFVersion = SystemProperties.System.Photo.EXIFVersion;
					if (!hashtable.ContainsKey(eXIFVersion))
					{
						hashtable.Add(eXIFVersion, shellObjectParent.Properties.CreateTypedProperty<string>(eXIFVersion));
					}
					return hashtable[eXIFVersion] as ShellProperty<string>;
				}
			}

			public ShellProperty<double?> ExposureBias
			{
				get
				{
					PropertyKey exposureBias = SystemProperties.System.Photo.ExposureBias;
					if (!hashtable.ContainsKey(exposureBias))
					{
						hashtable.Add(exposureBias, shellObjectParent.Properties.CreateTypedProperty<double?>(exposureBias));
					}
					return hashtable[exposureBias] as ShellProperty<double?>;
				}
			}

			public ShellProperty<int?> ExposureBiasDenominator
			{
				get
				{
					PropertyKey exposureBiasDenominator = SystemProperties.System.Photo.ExposureBiasDenominator;
					if (!hashtable.ContainsKey(exposureBiasDenominator))
					{
						hashtable.Add(exposureBiasDenominator, shellObjectParent.Properties.CreateTypedProperty<int?>(exposureBiasDenominator));
					}
					return hashtable[exposureBiasDenominator] as ShellProperty<int?>;
				}
			}

			public ShellProperty<int?> ExposureBiasNumerator
			{
				get
				{
					PropertyKey exposureBiasNumerator = SystemProperties.System.Photo.ExposureBiasNumerator;
					if (!hashtable.ContainsKey(exposureBiasNumerator))
					{
						hashtable.Add(exposureBiasNumerator, shellObjectParent.Properties.CreateTypedProperty<int?>(exposureBiasNumerator));
					}
					return hashtable[exposureBiasNumerator] as ShellProperty<int?>;
				}
			}

			public ShellProperty<double?> ExposureIndex
			{
				get
				{
					PropertyKey exposureIndex = SystemProperties.System.Photo.ExposureIndex;
					if (!hashtable.ContainsKey(exposureIndex))
					{
						hashtable.Add(exposureIndex, shellObjectParent.Properties.CreateTypedProperty<double?>(exposureIndex));
					}
					return hashtable[exposureIndex] as ShellProperty<double?>;
				}
			}

			public ShellProperty<uint?> ExposureIndexDenominator
			{
				get
				{
					PropertyKey exposureIndexDenominator = SystemProperties.System.Photo.ExposureIndexDenominator;
					if (!hashtable.ContainsKey(exposureIndexDenominator))
					{
						hashtable.Add(exposureIndexDenominator, shellObjectParent.Properties.CreateTypedProperty<uint?>(exposureIndexDenominator));
					}
					return hashtable[exposureIndexDenominator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> ExposureIndexNumerator
			{
				get
				{
					PropertyKey exposureIndexNumerator = SystemProperties.System.Photo.ExposureIndexNumerator;
					if (!hashtable.ContainsKey(exposureIndexNumerator))
					{
						hashtable.Add(exposureIndexNumerator, shellObjectParent.Properties.CreateTypedProperty<uint?>(exposureIndexNumerator));
					}
					return hashtable[exposureIndexNumerator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> ExposureProgram
			{
				get
				{
					PropertyKey exposureProgram = SystemProperties.System.Photo.ExposureProgram;
					if (!hashtable.ContainsKey(exposureProgram))
					{
						hashtable.Add(exposureProgram, shellObjectParent.Properties.CreateTypedProperty<uint?>(exposureProgram));
					}
					return hashtable[exposureProgram] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string> ExposureProgramText
			{
				get
				{
					PropertyKey exposureProgramText = SystemProperties.System.Photo.ExposureProgramText;
					if (!hashtable.ContainsKey(exposureProgramText))
					{
						hashtable.Add(exposureProgramText, shellObjectParent.Properties.CreateTypedProperty<string>(exposureProgramText));
					}
					return hashtable[exposureProgramText] as ShellProperty<string>;
				}
			}

			public ShellProperty<double?> ExposureTime
			{
				get
				{
					PropertyKey exposureTime = SystemProperties.System.Photo.ExposureTime;
					if (!hashtable.ContainsKey(exposureTime))
					{
						hashtable.Add(exposureTime, shellObjectParent.Properties.CreateTypedProperty<double?>(exposureTime));
					}
					return hashtable[exposureTime] as ShellProperty<double?>;
				}
			}

			public ShellProperty<uint?> ExposureTimeDenominator
			{
				get
				{
					PropertyKey exposureTimeDenominator = SystemProperties.System.Photo.ExposureTimeDenominator;
					if (!hashtable.ContainsKey(exposureTimeDenominator))
					{
						hashtable.Add(exposureTimeDenominator, shellObjectParent.Properties.CreateTypedProperty<uint?>(exposureTimeDenominator));
					}
					return hashtable[exposureTimeDenominator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> ExposureTimeNumerator
			{
				get
				{
					PropertyKey exposureTimeNumerator = SystemProperties.System.Photo.ExposureTimeNumerator;
					if (!hashtable.ContainsKey(exposureTimeNumerator))
					{
						hashtable.Add(exposureTimeNumerator, shellObjectParent.Properties.CreateTypedProperty<uint?>(exposureTimeNumerator));
					}
					return hashtable[exposureTimeNumerator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<byte?> Flash
			{
				get
				{
					PropertyKey flash = SystemProperties.System.Photo.Flash;
					if (!hashtable.ContainsKey(flash))
					{
						hashtable.Add(flash, shellObjectParent.Properties.CreateTypedProperty<byte?>(flash));
					}
					return hashtable[flash] as ShellProperty<byte?>;
				}
			}

			public ShellProperty<double?> FlashEnergy
			{
				get
				{
					PropertyKey flashEnergy = SystemProperties.System.Photo.FlashEnergy;
					if (!hashtable.ContainsKey(flashEnergy))
					{
						hashtable.Add(flashEnergy, shellObjectParent.Properties.CreateTypedProperty<double?>(flashEnergy));
					}
					return hashtable[flashEnergy] as ShellProperty<double?>;
				}
			}

			public ShellProperty<uint?> FlashEnergyDenominator
			{
				get
				{
					PropertyKey flashEnergyDenominator = SystemProperties.System.Photo.FlashEnergyDenominator;
					if (!hashtable.ContainsKey(flashEnergyDenominator))
					{
						hashtable.Add(flashEnergyDenominator, shellObjectParent.Properties.CreateTypedProperty<uint?>(flashEnergyDenominator));
					}
					return hashtable[flashEnergyDenominator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> FlashEnergyNumerator
			{
				get
				{
					PropertyKey flashEnergyNumerator = SystemProperties.System.Photo.FlashEnergyNumerator;
					if (!hashtable.ContainsKey(flashEnergyNumerator))
					{
						hashtable.Add(flashEnergyNumerator, shellObjectParent.Properties.CreateTypedProperty<uint?>(flashEnergyNumerator));
					}
					return hashtable[flashEnergyNumerator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string> FlashManufacturer
			{
				get
				{
					PropertyKey flashManufacturer = SystemProperties.System.Photo.FlashManufacturer;
					if (!hashtable.ContainsKey(flashManufacturer))
					{
						hashtable.Add(flashManufacturer, shellObjectParent.Properties.CreateTypedProperty<string>(flashManufacturer));
					}
					return hashtable[flashManufacturer] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> FlashModel
			{
				get
				{
					PropertyKey flashModel = SystemProperties.System.Photo.FlashModel;
					if (!hashtable.ContainsKey(flashModel))
					{
						hashtable.Add(flashModel, shellObjectParent.Properties.CreateTypedProperty<string>(flashModel));
					}
					return hashtable[flashModel] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> FlashText
			{
				get
				{
					PropertyKey flashText = SystemProperties.System.Photo.FlashText;
					if (!hashtable.ContainsKey(flashText))
					{
						hashtable.Add(flashText, shellObjectParent.Properties.CreateTypedProperty<string>(flashText));
					}
					return hashtable[flashText] as ShellProperty<string>;
				}
			}

			public ShellProperty<double?> FNumber
			{
				get
				{
					PropertyKey fNumber = SystemProperties.System.Photo.FNumber;
					if (!hashtable.ContainsKey(fNumber))
					{
						hashtable.Add(fNumber, shellObjectParent.Properties.CreateTypedProperty<double?>(fNumber));
					}
					return hashtable[fNumber] as ShellProperty<double?>;
				}
			}

			public ShellProperty<uint?> FNumberDenominator
			{
				get
				{
					PropertyKey fNumberDenominator = SystemProperties.System.Photo.FNumberDenominator;
					if (!hashtable.ContainsKey(fNumberDenominator))
					{
						hashtable.Add(fNumberDenominator, shellObjectParent.Properties.CreateTypedProperty<uint?>(fNumberDenominator));
					}
					return hashtable[fNumberDenominator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> FNumberNumerator
			{
				get
				{
					PropertyKey fNumberNumerator = SystemProperties.System.Photo.FNumberNumerator;
					if (!hashtable.ContainsKey(fNumberNumerator))
					{
						hashtable.Add(fNumberNumerator, shellObjectParent.Properties.CreateTypedProperty<uint?>(fNumberNumerator));
					}
					return hashtable[fNumberNumerator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<double?> FocalLength
			{
				get
				{
					PropertyKey focalLength = SystemProperties.System.Photo.FocalLength;
					if (!hashtable.ContainsKey(focalLength))
					{
						hashtable.Add(focalLength, shellObjectParent.Properties.CreateTypedProperty<double?>(focalLength));
					}
					return hashtable[focalLength] as ShellProperty<double?>;
				}
			}

			public ShellProperty<uint?> FocalLengthDenominator
			{
				get
				{
					PropertyKey focalLengthDenominator = SystemProperties.System.Photo.FocalLengthDenominator;
					if (!hashtable.ContainsKey(focalLengthDenominator))
					{
						hashtable.Add(focalLengthDenominator, shellObjectParent.Properties.CreateTypedProperty<uint?>(focalLengthDenominator));
					}
					return hashtable[focalLengthDenominator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<ushort?> FocalLengthInFilm
			{
				get
				{
					PropertyKey focalLengthInFilm = SystemProperties.System.Photo.FocalLengthInFilm;
					if (!hashtable.ContainsKey(focalLengthInFilm))
					{
						hashtable.Add(focalLengthInFilm, shellObjectParent.Properties.CreateTypedProperty<ushort?>(focalLengthInFilm));
					}
					return hashtable[focalLengthInFilm] as ShellProperty<ushort?>;
				}
			}

			public ShellProperty<uint?> FocalLengthNumerator
			{
				get
				{
					PropertyKey focalLengthNumerator = SystemProperties.System.Photo.FocalLengthNumerator;
					if (!hashtable.ContainsKey(focalLengthNumerator))
					{
						hashtable.Add(focalLengthNumerator, shellObjectParent.Properties.CreateTypedProperty<uint?>(focalLengthNumerator));
					}
					return hashtable[focalLengthNumerator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<double?> FocalPlaneXResolution
			{
				get
				{
					PropertyKey focalPlaneXResolution = SystemProperties.System.Photo.FocalPlaneXResolution;
					if (!hashtable.ContainsKey(focalPlaneXResolution))
					{
						hashtable.Add(focalPlaneXResolution, shellObjectParent.Properties.CreateTypedProperty<double?>(focalPlaneXResolution));
					}
					return hashtable[focalPlaneXResolution] as ShellProperty<double?>;
				}
			}

			public ShellProperty<uint?> FocalPlaneXResolutionDenominator
			{
				get
				{
					PropertyKey focalPlaneXResolutionDenominator = SystemProperties.System.Photo.FocalPlaneXResolutionDenominator;
					if (!hashtable.ContainsKey(focalPlaneXResolutionDenominator))
					{
						hashtable.Add(focalPlaneXResolutionDenominator, shellObjectParent.Properties.CreateTypedProperty<uint?>(focalPlaneXResolutionDenominator));
					}
					return hashtable[focalPlaneXResolutionDenominator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> FocalPlaneXResolutionNumerator
			{
				get
				{
					PropertyKey focalPlaneXResolutionNumerator = SystemProperties.System.Photo.FocalPlaneXResolutionNumerator;
					if (!hashtable.ContainsKey(focalPlaneXResolutionNumerator))
					{
						hashtable.Add(focalPlaneXResolutionNumerator, shellObjectParent.Properties.CreateTypedProperty<uint?>(focalPlaneXResolutionNumerator));
					}
					return hashtable[focalPlaneXResolutionNumerator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<double?> FocalPlaneYResolution
			{
				get
				{
					PropertyKey focalPlaneYResolution = SystemProperties.System.Photo.FocalPlaneYResolution;
					if (!hashtable.ContainsKey(focalPlaneYResolution))
					{
						hashtable.Add(focalPlaneYResolution, shellObjectParent.Properties.CreateTypedProperty<double?>(focalPlaneYResolution));
					}
					return hashtable[focalPlaneYResolution] as ShellProperty<double?>;
				}
			}

			public ShellProperty<uint?> FocalPlaneYResolutionDenominator
			{
				get
				{
					PropertyKey focalPlaneYResolutionDenominator = SystemProperties.System.Photo.FocalPlaneYResolutionDenominator;
					if (!hashtable.ContainsKey(focalPlaneYResolutionDenominator))
					{
						hashtable.Add(focalPlaneYResolutionDenominator, shellObjectParent.Properties.CreateTypedProperty<uint?>(focalPlaneYResolutionDenominator));
					}
					return hashtable[focalPlaneYResolutionDenominator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> FocalPlaneYResolutionNumerator
			{
				get
				{
					PropertyKey focalPlaneYResolutionNumerator = SystemProperties.System.Photo.FocalPlaneYResolutionNumerator;
					if (!hashtable.ContainsKey(focalPlaneYResolutionNumerator))
					{
						hashtable.Add(focalPlaneYResolutionNumerator, shellObjectParent.Properties.CreateTypedProperty<uint?>(focalPlaneYResolutionNumerator));
					}
					return hashtable[focalPlaneYResolutionNumerator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<double?> GainControl
			{
				get
				{
					PropertyKey gainControl = SystemProperties.System.Photo.GainControl;
					if (!hashtable.ContainsKey(gainControl))
					{
						hashtable.Add(gainControl, shellObjectParent.Properties.CreateTypedProperty<double?>(gainControl));
					}
					return hashtable[gainControl] as ShellProperty<double?>;
				}
			}

			public ShellProperty<uint?> GainControlDenominator
			{
				get
				{
					PropertyKey gainControlDenominator = SystemProperties.System.Photo.GainControlDenominator;
					if (!hashtable.ContainsKey(gainControlDenominator))
					{
						hashtable.Add(gainControlDenominator, shellObjectParent.Properties.CreateTypedProperty<uint?>(gainControlDenominator));
					}
					return hashtable[gainControlDenominator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> GainControlNumerator
			{
				get
				{
					PropertyKey gainControlNumerator = SystemProperties.System.Photo.GainControlNumerator;
					if (!hashtable.ContainsKey(gainControlNumerator))
					{
						hashtable.Add(gainControlNumerator, shellObjectParent.Properties.CreateTypedProperty<uint?>(gainControlNumerator));
					}
					return hashtable[gainControlNumerator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string> GainControlText
			{
				get
				{
					PropertyKey gainControlText = SystemProperties.System.Photo.GainControlText;
					if (!hashtable.ContainsKey(gainControlText))
					{
						hashtable.Add(gainControlText, shellObjectParent.Properties.CreateTypedProperty<string>(gainControlText));
					}
					return hashtable[gainControlText] as ShellProperty<string>;
				}
			}

			public ShellProperty<ushort?> ISOSpeed
			{
				get
				{
					PropertyKey iSOSpeed = SystemProperties.System.Photo.ISOSpeed;
					if (!hashtable.ContainsKey(iSOSpeed))
					{
						hashtable.Add(iSOSpeed, shellObjectParent.Properties.CreateTypedProperty<ushort?>(iSOSpeed));
					}
					return hashtable[iSOSpeed] as ShellProperty<ushort?>;
				}
			}

			public ShellProperty<string> LensManufacturer
			{
				get
				{
					PropertyKey lensManufacturer = SystemProperties.System.Photo.LensManufacturer;
					if (!hashtable.ContainsKey(lensManufacturer))
					{
						hashtable.Add(lensManufacturer, shellObjectParent.Properties.CreateTypedProperty<string>(lensManufacturer));
					}
					return hashtable[lensManufacturer] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> LensModel
			{
				get
				{
					PropertyKey lensModel = SystemProperties.System.Photo.LensModel;
					if (!hashtable.ContainsKey(lensModel))
					{
						hashtable.Add(lensModel, shellObjectParent.Properties.CreateTypedProperty<string>(lensModel));
					}
					return hashtable[lensModel] as ShellProperty<string>;
				}
			}

			public ShellProperty<uint?> LightSource
			{
				get
				{
					PropertyKey lightSource = SystemProperties.System.Photo.LightSource;
					if (!hashtable.ContainsKey(lightSource))
					{
						hashtable.Add(lightSource, shellObjectParent.Properties.CreateTypedProperty<uint?>(lightSource));
					}
					return hashtable[lightSource] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<byte[]> MakerNote
			{
				get
				{
					PropertyKey makerNote = SystemProperties.System.Photo.MakerNote;
					if (!hashtable.ContainsKey(makerNote))
					{
						hashtable.Add(makerNote, shellObjectParent.Properties.CreateTypedProperty<byte[]>(makerNote));
					}
					return hashtable[makerNote] as ShellProperty<byte[]>;
				}
			}

			public ShellProperty<ulong?> MakerNoteOffset
			{
				get
				{
					PropertyKey makerNoteOffset = SystemProperties.System.Photo.MakerNoteOffset;
					if (!hashtable.ContainsKey(makerNoteOffset))
					{
						hashtable.Add(makerNoteOffset, shellObjectParent.Properties.CreateTypedProperty<ulong?>(makerNoteOffset));
					}
					return hashtable[makerNoteOffset] as ShellProperty<ulong?>;
				}
			}

			public ShellProperty<double?> MaxAperture
			{
				get
				{
					PropertyKey maxAperture = SystemProperties.System.Photo.MaxAperture;
					if (!hashtable.ContainsKey(maxAperture))
					{
						hashtable.Add(maxAperture, shellObjectParent.Properties.CreateTypedProperty<double?>(maxAperture));
					}
					return hashtable[maxAperture] as ShellProperty<double?>;
				}
			}

			public ShellProperty<uint?> MaxApertureDenominator
			{
				get
				{
					PropertyKey maxApertureDenominator = SystemProperties.System.Photo.MaxApertureDenominator;
					if (!hashtable.ContainsKey(maxApertureDenominator))
					{
						hashtable.Add(maxApertureDenominator, shellObjectParent.Properties.CreateTypedProperty<uint?>(maxApertureDenominator));
					}
					return hashtable[maxApertureDenominator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> MaxApertureNumerator
			{
				get
				{
					PropertyKey maxApertureNumerator = SystemProperties.System.Photo.MaxApertureNumerator;
					if (!hashtable.ContainsKey(maxApertureNumerator))
					{
						hashtable.Add(maxApertureNumerator, shellObjectParent.Properties.CreateTypedProperty<uint?>(maxApertureNumerator));
					}
					return hashtable[maxApertureNumerator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<ushort?> MeteringMode
			{
				get
				{
					PropertyKey meteringMode = SystemProperties.System.Photo.MeteringMode;
					if (!hashtable.ContainsKey(meteringMode))
					{
						hashtable.Add(meteringMode, shellObjectParent.Properties.CreateTypedProperty<ushort?>(meteringMode));
					}
					return hashtable[meteringMode] as ShellProperty<ushort?>;
				}
			}

			public ShellProperty<string> MeteringModeText
			{
				get
				{
					PropertyKey meteringModeText = SystemProperties.System.Photo.MeteringModeText;
					if (!hashtable.ContainsKey(meteringModeText))
					{
						hashtable.Add(meteringModeText, shellObjectParent.Properties.CreateTypedProperty<string>(meteringModeText));
					}
					return hashtable[meteringModeText] as ShellProperty<string>;
				}
			}

			public ShellProperty<ushort?> Orientation
			{
				get
				{
					PropertyKey orientation = SystemProperties.System.Photo.Orientation;
					if (!hashtable.ContainsKey(orientation))
					{
						hashtable.Add(orientation, shellObjectParent.Properties.CreateTypedProperty<ushort?>(orientation));
					}
					return hashtable[orientation] as ShellProperty<ushort?>;
				}
			}

			public ShellProperty<string> OrientationText
			{
				get
				{
					PropertyKey orientationText = SystemProperties.System.Photo.OrientationText;
					if (!hashtable.ContainsKey(orientationText))
					{
						hashtable.Add(orientationText, shellObjectParent.Properties.CreateTypedProperty<string>(orientationText));
					}
					return hashtable[orientationText] as ShellProperty<string>;
				}
			}

			public ShellProperty<string[]> PeopleNames
			{
				get
				{
					PropertyKey peopleNames = SystemProperties.System.Photo.PeopleNames;
					if (!hashtable.ContainsKey(peopleNames))
					{
						hashtable.Add(peopleNames, shellObjectParent.Properties.CreateTypedProperty<string[]>(peopleNames));
					}
					return hashtable[peopleNames] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<ushort?> PhotometricInterpretation
			{
				get
				{
					PropertyKey photometricInterpretation = SystemProperties.System.Photo.PhotometricInterpretation;
					if (!hashtable.ContainsKey(photometricInterpretation))
					{
						hashtable.Add(photometricInterpretation, shellObjectParent.Properties.CreateTypedProperty<ushort?>(photometricInterpretation));
					}
					return hashtable[photometricInterpretation] as ShellProperty<ushort?>;
				}
			}

			public ShellProperty<string> PhotometricInterpretationText
			{
				get
				{
					PropertyKey photometricInterpretationText = SystemProperties.System.Photo.PhotometricInterpretationText;
					if (!hashtable.ContainsKey(photometricInterpretationText))
					{
						hashtable.Add(photometricInterpretationText, shellObjectParent.Properties.CreateTypedProperty<string>(photometricInterpretationText));
					}
					return hashtable[photometricInterpretationText] as ShellProperty<string>;
				}
			}

			public ShellProperty<uint?> ProgramMode
			{
				get
				{
					PropertyKey programMode = SystemProperties.System.Photo.ProgramMode;
					if (!hashtable.ContainsKey(programMode))
					{
						hashtable.Add(programMode, shellObjectParent.Properties.CreateTypedProperty<uint?>(programMode));
					}
					return hashtable[programMode] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string> ProgramModeText
			{
				get
				{
					PropertyKey programModeText = SystemProperties.System.Photo.ProgramModeText;
					if (!hashtable.ContainsKey(programModeText))
					{
						hashtable.Add(programModeText, shellObjectParent.Properties.CreateTypedProperty<string>(programModeText));
					}
					return hashtable[programModeText] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> RelatedSoundFile
			{
				get
				{
					PropertyKey relatedSoundFile = SystemProperties.System.Photo.RelatedSoundFile;
					if (!hashtable.ContainsKey(relatedSoundFile))
					{
						hashtable.Add(relatedSoundFile, shellObjectParent.Properties.CreateTypedProperty<string>(relatedSoundFile));
					}
					return hashtable[relatedSoundFile] as ShellProperty<string>;
				}
			}

			public ShellProperty<uint?> Saturation
			{
				get
				{
					PropertyKey saturation = SystemProperties.System.Photo.Saturation;
					if (!hashtable.ContainsKey(saturation))
					{
						hashtable.Add(saturation, shellObjectParent.Properties.CreateTypedProperty<uint?>(saturation));
					}
					return hashtable[saturation] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string> SaturationText
			{
				get
				{
					PropertyKey saturationText = SystemProperties.System.Photo.SaturationText;
					if (!hashtable.ContainsKey(saturationText))
					{
						hashtable.Add(saturationText, shellObjectParent.Properties.CreateTypedProperty<string>(saturationText));
					}
					return hashtable[saturationText] as ShellProperty<string>;
				}
			}

			public ShellProperty<uint?> Sharpness
			{
				get
				{
					PropertyKey sharpness = SystemProperties.System.Photo.Sharpness;
					if (!hashtable.ContainsKey(sharpness))
					{
						hashtable.Add(sharpness, shellObjectParent.Properties.CreateTypedProperty<uint?>(sharpness));
					}
					return hashtable[sharpness] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string> SharpnessText
			{
				get
				{
					PropertyKey sharpnessText = SystemProperties.System.Photo.SharpnessText;
					if (!hashtable.ContainsKey(sharpnessText))
					{
						hashtable.Add(sharpnessText, shellObjectParent.Properties.CreateTypedProperty<string>(sharpnessText));
					}
					return hashtable[sharpnessText] as ShellProperty<string>;
				}
			}

			public ShellProperty<double?> ShutterSpeed
			{
				get
				{
					PropertyKey shutterSpeed = SystemProperties.System.Photo.ShutterSpeed;
					if (!hashtable.ContainsKey(shutterSpeed))
					{
						hashtable.Add(shutterSpeed, shellObjectParent.Properties.CreateTypedProperty<double?>(shutterSpeed));
					}
					return hashtable[shutterSpeed] as ShellProperty<double?>;
				}
			}

			public ShellProperty<int?> ShutterSpeedDenominator
			{
				get
				{
					PropertyKey shutterSpeedDenominator = SystemProperties.System.Photo.ShutterSpeedDenominator;
					if (!hashtable.ContainsKey(shutterSpeedDenominator))
					{
						hashtable.Add(shutterSpeedDenominator, shellObjectParent.Properties.CreateTypedProperty<int?>(shutterSpeedDenominator));
					}
					return hashtable[shutterSpeedDenominator] as ShellProperty<int?>;
				}
			}

			public ShellProperty<int?> ShutterSpeedNumerator
			{
				get
				{
					PropertyKey shutterSpeedNumerator = SystemProperties.System.Photo.ShutterSpeedNumerator;
					if (!hashtable.ContainsKey(shutterSpeedNumerator))
					{
						hashtable.Add(shutterSpeedNumerator, shellObjectParent.Properties.CreateTypedProperty<int?>(shutterSpeedNumerator));
					}
					return hashtable[shutterSpeedNumerator] as ShellProperty<int?>;
				}
			}

			public ShellProperty<double?> SubjectDistance
			{
				get
				{
					PropertyKey subjectDistance = SystemProperties.System.Photo.SubjectDistance;
					if (!hashtable.ContainsKey(subjectDistance))
					{
						hashtable.Add(subjectDistance, shellObjectParent.Properties.CreateTypedProperty<double?>(subjectDistance));
					}
					return hashtable[subjectDistance] as ShellProperty<double?>;
				}
			}

			public ShellProperty<uint?> SubjectDistanceDenominator
			{
				get
				{
					PropertyKey subjectDistanceDenominator = SystemProperties.System.Photo.SubjectDistanceDenominator;
					if (!hashtable.ContainsKey(subjectDistanceDenominator))
					{
						hashtable.Add(subjectDistanceDenominator, shellObjectParent.Properties.CreateTypedProperty<uint?>(subjectDistanceDenominator));
					}
					return hashtable[subjectDistanceDenominator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> SubjectDistanceNumerator
			{
				get
				{
					PropertyKey subjectDistanceNumerator = SystemProperties.System.Photo.SubjectDistanceNumerator;
					if (!hashtable.ContainsKey(subjectDistanceNumerator))
					{
						hashtable.Add(subjectDistanceNumerator, shellObjectParent.Properties.CreateTypedProperty<uint?>(subjectDistanceNumerator));
					}
					return hashtable[subjectDistanceNumerator] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string[]> TagViewAggregate
			{
				get
				{
					PropertyKey tagViewAggregate = SystemProperties.System.Photo.TagViewAggregate;
					if (!hashtable.ContainsKey(tagViewAggregate))
					{
						hashtable.Add(tagViewAggregate, shellObjectParent.Properties.CreateTypedProperty<string[]>(tagViewAggregate));
					}
					return hashtable[tagViewAggregate] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<bool?> TranscodedForSync
			{
				get
				{
					PropertyKey transcodedForSync = SystemProperties.System.Photo.TranscodedForSync;
					if (!hashtable.ContainsKey(transcodedForSync))
					{
						hashtable.Add(transcodedForSync, shellObjectParent.Properties.CreateTypedProperty<bool?>(transcodedForSync));
					}
					return hashtable[transcodedForSync] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<uint?> WhiteBalance
			{
				get
				{
					PropertyKey whiteBalance = SystemProperties.System.Photo.WhiteBalance;
					if (!hashtable.ContainsKey(whiteBalance))
					{
						hashtable.Add(whiteBalance, shellObjectParent.Properties.CreateTypedProperty<uint?>(whiteBalance));
					}
					return hashtable[whiteBalance] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string> WhiteBalanceText
			{
				get
				{
					PropertyKey whiteBalanceText = SystemProperties.System.Photo.WhiteBalanceText;
					if (!hashtable.ContainsKey(whiteBalanceText))
					{
						hashtable.Add(whiteBalanceText, shellObjectParent.Properties.CreateTypedProperty<string>(whiteBalanceText));
					}
					return hashtable[whiteBalanceText] as ShellProperty<string>;
				}
			}

			internal PropertySystemPhoto(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemPropGroup : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<object> Advanced
			{
				get
				{
					PropertyKey advanced = SystemProperties.System.PropGroup.Advanced;
					if (!hashtable.ContainsKey(advanced))
					{
						hashtable.Add(advanced, shellObjectParent.Properties.CreateTypedProperty<object>(advanced));
					}
					return hashtable[advanced] as ShellProperty<object>;
				}
			}

			public ShellProperty<object> Audio
			{
				get
				{
					PropertyKey audio = SystemProperties.System.PropGroup.Audio;
					if (!hashtable.ContainsKey(audio))
					{
						hashtable.Add(audio, shellObjectParent.Properties.CreateTypedProperty<object>(audio));
					}
					return hashtable[audio] as ShellProperty<object>;
				}
			}

			public ShellProperty<object> Calendar
			{
				get
				{
					PropertyKey calendar = SystemProperties.System.PropGroup.Calendar;
					if (!hashtable.ContainsKey(calendar))
					{
						hashtable.Add(calendar, shellObjectParent.Properties.CreateTypedProperty<object>(calendar));
					}
					return hashtable[calendar] as ShellProperty<object>;
				}
			}

			public ShellProperty<object> Camera
			{
				get
				{
					PropertyKey camera = SystemProperties.System.PropGroup.Camera;
					if (!hashtable.ContainsKey(camera))
					{
						hashtable.Add(camera, shellObjectParent.Properties.CreateTypedProperty<object>(camera));
					}
					return hashtable[camera] as ShellProperty<object>;
				}
			}

			public ShellProperty<object> Contact
			{
				get
				{
					PropertyKey contact = SystemProperties.System.PropGroup.Contact;
					if (!hashtable.ContainsKey(contact))
					{
						hashtable.Add(contact, shellObjectParent.Properties.CreateTypedProperty<object>(contact));
					}
					return hashtable[contact] as ShellProperty<object>;
				}
			}

			public ShellProperty<object> Content
			{
				get
				{
					PropertyKey content = SystemProperties.System.PropGroup.Content;
					if (!hashtable.ContainsKey(content))
					{
						hashtable.Add(content, shellObjectParent.Properties.CreateTypedProperty<object>(content));
					}
					return hashtable[content] as ShellProperty<object>;
				}
			}

			public ShellProperty<object> Description
			{
				get
				{
					PropertyKey description = SystemProperties.System.PropGroup.Description;
					if (!hashtable.ContainsKey(description))
					{
						hashtable.Add(description, shellObjectParent.Properties.CreateTypedProperty<object>(description));
					}
					return hashtable[description] as ShellProperty<object>;
				}
			}

			public ShellProperty<object> FileSystem
			{
				get
				{
					PropertyKey fileSystem = SystemProperties.System.PropGroup.FileSystem;
					if (!hashtable.ContainsKey(fileSystem))
					{
						hashtable.Add(fileSystem, shellObjectParent.Properties.CreateTypedProperty<object>(fileSystem));
					}
					return hashtable[fileSystem] as ShellProperty<object>;
				}
			}

			public ShellProperty<object> General
			{
				get
				{
					PropertyKey general = SystemProperties.System.PropGroup.General;
					if (!hashtable.ContainsKey(general))
					{
						hashtable.Add(general, shellObjectParent.Properties.CreateTypedProperty<object>(general));
					}
					return hashtable[general] as ShellProperty<object>;
				}
			}

			public ShellProperty<object> GPS
			{
				get
				{
					PropertyKey gPS = SystemProperties.System.PropGroup.GPS;
					if (!hashtable.ContainsKey(gPS))
					{
						hashtable.Add(gPS, shellObjectParent.Properties.CreateTypedProperty<object>(gPS));
					}
					return hashtable[gPS] as ShellProperty<object>;
				}
			}

			public ShellProperty<object> Image
			{
				get
				{
					PropertyKey image = SystemProperties.System.PropGroup.Image;
					if (!hashtable.ContainsKey(image))
					{
						hashtable.Add(image, shellObjectParent.Properties.CreateTypedProperty<object>(image));
					}
					return hashtable[image] as ShellProperty<object>;
				}
			}

			public ShellProperty<object> Media
			{
				get
				{
					PropertyKey media = SystemProperties.System.PropGroup.Media;
					if (!hashtable.ContainsKey(media))
					{
						hashtable.Add(media, shellObjectParent.Properties.CreateTypedProperty<object>(media));
					}
					return hashtable[media] as ShellProperty<object>;
				}
			}

			public ShellProperty<object> MediaAdvanced
			{
				get
				{
					PropertyKey mediaAdvanced = SystemProperties.System.PropGroup.MediaAdvanced;
					if (!hashtable.ContainsKey(mediaAdvanced))
					{
						hashtable.Add(mediaAdvanced, shellObjectParent.Properties.CreateTypedProperty<object>(mediaAdvanced));
					}
					return hashtable[mediaAdvanced] as ShellProperty<object>;
				}
			}

			public ShellProperty<object> Message
			{
				get
				{
					PropertyKey message = SystemProperties.System.PropGroup.Message;
					if (!hashtable.ContainsKey(message))
					{
						hashtable.Add(message, shellObjectParent.Properties.CreateTypedProperty<object>(message));
					}
					return hashtable[message] as ShellProperty<object>;
				}
			}

			public ShellProperty<object> Music
			{
				get
				{
					PropertyKey music = SystemProperties.System.PropGroup.Music;
					if (!hashtable.ContainsKey(music))
					{
						hashtable.Add(music, shellObjectParent.Properties.CreateTypedProperty<object>(music));
					}
					return hashtable[music] as ShellProperty<object>;
				}
			}

			public ShellProperty<object> Origin
			{
				get
				{
					PropertyKey origin = SystemProperties.System.PropGroup.Origin;
					if (!hashtable.ContainsKey(origin))
					{
						hashtable.Add(origin, shellObjectParent.Properties.CreateTypedProperty<object>(origin));
					}
					return hashtable[origin] as ShellProperty<object>;
				}
			}

			public ShellProperty<object> PhotoAdvanced
			{
				get
				{
					PropertyKey photoAdvanced = SystemProperties.System.PropGroup.PhotoAdvanced;
					if (!hashtable.ContainsKey(photoAdvanced))
					{
						hashtable.Add(photoAdvanced, shellObjectParent.Properties.CreateTypedProperty<object>(photoAdvanced));
					}
					return hashtable[photoAdvanced] as ShellProperty<object>;
				}
			}

			public ShellProperty<object> RecordedTV
			{
				get
				{
					PropertyKey recordedTV = SystemProperties.System.PropGroup.RecordedTV;
					if (!hashtable.ContainsKey(recordedTV))
					{
						hashtable.Add(recordedTV, shellObjectParent.Properties.CreateTypedProperty<object>(recordedTV));
					}
					return hashtable[recordedTV] as ShellProperty<object>;
				}
			}

			public ShellProperty<object> Video
			{
				get
				{
					PropertyKey video = SystemProperties.System.PropGroup.Video;
					if (!hashtable.ContainsKey(video))
					{
						hashtable.Add(video, shellObjectParent.Properties.CreateTypedProperty<object>(video));
					}
					return hashtable[video] as ShellProperty<object>;
				}
			}

			internal PropertySystemPropGroup(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemPropList : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<string> ConflictPrompt
			{
				get
				{
					PropertyKey conflictPrompt = SystemProperties.System.PropList.ConflictPrompt;
					if (!hashtable.ContainsKey(conflictPrompt))
					{
						hashtable.Add(conflictPrompt, shellObjectParent.Properties.CreateTypedProperty<string>(conflictPrompt));
					}
					return hashtable[conflictPrompt] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ContentViewModeForBrowse
			{
				get
				{
					PropertyKey contentViewModeForBrowse = SystemProperties.System.PropList.ContentViewModeForBrowse;
					if (!hashtable.ContainsKey(contentViewModeForBrowse))
					{
						hashtable.Add(contentViewModeForBrowse, shellObjectParent.Properties.CreateTypedProperty<string>(contentViewModeForBrowse));
					}
					return hashtable[contentViewModeForBrowse] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ContentViewModeForSearch
			{
				get
				{
					PropertyKey contentViewModeForSearch = SystemProperties.System.PropList.ContentViewModeForSearch;
					if (!hashtable.ContainsKey(contentViewModeForSearch))
					{
						hashtable.Add(contentViewModeForSearch, shellObjectParent.Properties.CreateTypedProperty<string>(contentViewModeForSearch));
					}
					return hashtable[contentViewModeForSearch] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ExtendedTileInfo
			{
				get
				{
					PropertyKey extendedTileInfo = SystemProperties.System.PropList.ExtendedTileInfo;
					if (!hashtable.ContainsKey(extendedTileInfo))
					{
						hashtable.Add(extendedTileInfo, shellObjectParent.Properties.CreateTypedProperty<string>(extendedTileInfo));
					}
					return hashtable[extendedTileInfo] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> FileOperationPrompt
			{
				get
				{
					PropertyKey fileOperationPrompt = SystemProperties.System.PropList.FileOperationPrompt;
					if (!hashtable.ContainsKey(fileOperationPrompt))
					{
						hashtable.Add(fileOperationPrompt, shellObjectParent.Properties.CreateTypedProperty<string>(fileOperationPrompt));
					}
					return hashtable[fileOperationPrompt] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> FullDetails
			{
				get
				{
					PropertyKey fullDetails = SystemProperties.System.PropList.FullDetails;
					if (!hashtable.ContainsKey(fullDetails))
					{
						hashtable.Add(fullDetails, shellObjectParent.Properties.CreateTypedProperty<string>(fullDetails));
					}
					return hashtable[fullDetails] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> InfoTip
			{
				get
				{
					PropertyKey infoTip = SystemProperties.System.PropList.InfoTip;
					if (!hashtable.ContainsKey(infoTip))
					{
						hashtable.Add(infoTip, shellObjectParent.Properties.CreateTypedProperty<string>(infoTip));
					}
					return hashtable[infoTip] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> NonPersonal
			{
				get
				{
					PropertyKey nonPersonal = SystemProperties.System.PropList.NonPersonal;
					if (!hashtable.ContainsKey(nonPersonal))
					{
						hashtable.Add(nonPersonal, shellObjectParent.Properties.CreateTypedProperty<string>(nonPersonal));
					}
					return hashtable[nonPersonal] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> PreviewDetails
			{
				get
				{
					PropertyKey previewDetails = SystemProperties.System.PropList.PreviewDetails;
					if (!hashtable.ContainsKey(previewDetails))
					{
						hashtable.Add(previewDetails, shellObjectParent.Properties.CreateTypedProperty<string>(previewDetails));
					}
					return hashtable[previewDetails] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> PreviewTitle
			{
				get
				{
					PropertyKey previewTitle = SystemProperties.System.PropList.PreviewTitle;
					if (!hashtable.ContainsKey(previewTitle))
					{
						hashtable.Add(previewTitle, shellObjectParent.Properties.CreateTypedProperty<string>(previewTitle));
					}
					return hashtable[previewTitle] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> QuickTip
			{
				get
				{
					PropertyKey quickTip = SystemProperties.System.PropList.QuickTip;
					if (!hashtable.ContainsKey(quickTip))
					{
						hashtable.Add(quickTip, shellObjectParent.Properties.CreateTypedProperty<string>(quickTip));
					}
					return hashtable[quickTip] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> TileInfo
			{
				get
				{
					PropertyKey tileInfo = SystemProperties.System.PropList.TileInfo;
					if (!hashtable.ContainsKey(tileInfo))
					{
						hashtable.Add(tileInfo, shellObjectParent.Properties.CreateTypedProperty<string>(tileInfo));
					}
					return hashtable[tileInfo] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> XPDetailsPanel
			{
				get
				{
					PropertyKey xPDetailsPanel = SystemProperties.System.PropList.XPDetailsPanel;
					if (!hashtable.ContainsKey(xPDetailsPanel))
					{
						hashtable.Add(xPDetailsPanel, shellObjectParent.Properties.CreateTypedProperty<string>(xPDetailsPanel));
					}
					return hashtable[xPDetailsPanel] as ShellProperty<string>;
				}
			}

			internal PropertySystemPropList(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemRecordedTV : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<uint?> ChannelNumber
			{
				get
				{
					PropertyKey channelNumber = SystemProperties.System.RecordedTV.ChannelNumber;
					if (!hashtable.ContainsKey(channelNumber))
					{
						hashtable.Add(channelNumber, shellObjectParent.Properties.CreateTypedProperty<uint?>(channelNumber));
					}
					return hashtable[channelNumber] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string> Credits
			{
				get
				{
					PropertyKey credits = SystemProperties.System.RecordedTV.Credits;
					if (!hashtable.ContainsKey(credits))
					{
						hashtable.Add(credits, shellObjectParent.Properties.CreateTypedProperty<string>(credits));
					}
					return hashtable[credits] as ShellProperty<string>;
				}
			}

			public ShellProperty<DateTime?> DateContentExpires
			{
				get
				{
					PropertyKey dateContentExpires = SystemProperties.System.RecordedTV.DateContentExpires;
					if (!hashtable.ContainsKey(dateContentExpires))
					{
						hashtable.Add(dateContentExpires, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(dateContentExpires));
					}
					return hashtable[dateContentExpires] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<string> EpisodeName
			{
				get
				{
					PropertyKey episodeName = SystemProperties.System.RecordedTV.EpisodeName;
					if (!hashtable.ContainsKey(episodeName))
					{
						hashtable.Add(episodeName, shellObjectParent.Properties.CreateTypedProperty<string>(episodeName));
					}
					return hashtable[episodeName] as ShellProperty<string>;
				}
			}

			public ShellProperty<bool?> IsATSCContent
			{
				get
				{
					PropertyKey isATSCContent = SystemProperties.System.RecordedTV.IsATSCContent;
					if (!hashtable.ContainsKey(isATSCContent))
					{
						hashtable.Add(isATSCContent, shellObjectParent.Properties.CreateTypedProperty<bool?>(isATSCContent));
					}
					return hashtable[isATSCContent] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> IsClosedCaptioningAvailable
			{
				get
				{
					PropertyKey isClosedCaptioningAvailable = SystemProperties.System.RecordedTV.IsClosedCaptioningAvailable;
					if (!hashtable.ContainsKey(isClosedCaptioningAvailable))
					{
						hashtable.Add(isClosedCaptioningAvailable, shellObjectParent.Properties.CreateTypedProperty<bool?>(isClosedCaptioningAvailable));
					}
					return hashtable[isClosedCaptioningAvailable] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> IsDTVContent
			{
				get
				{
					PropertyKey isDTVContent = SystemProperties.System.RecordedTV.IsDTVContent;
					if (!hashtable.ContainsKey(isDTVContent))
					{
						hashtable.Add(isDTVContent, shellObjectParent.Properties.CreateTypedProperty<bool?>(isDTVContent));
					}
					return hashtable[isDTVContent] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> IsHDContent
			{
				get
				{
					PropertyKey isHDContent = SystemProperties.System.RecordedTV.IsHDContent;
					if (!hashtable.ContainsKey(isHDContent))
					{
						hashtable.Add(isHDContent, shellObjectParent.Properties.CreateTypedProperty<bool?>(isHDContent));
					}
					return hashtable[isHDContent] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> IsRepeatBroadcast
			{
				get
				{
					PropertyKey isRepeatBroadcast = SystemProperties.System.RecordedTV.IsRepeatBroadcast;
					if (!hashtable.ContainsKey(isRepeatBroadcast))
					{
						hashtable.Add(isRepeatBroadcast, shellObjectParent.Properties.CreateTypedProperty<bool?>(isRepeatBroadcast));
					}
					return hashtable[isRepeatBroadcast] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> IsSAP
			{
				get
				{
					PropertyKey isSAP = SystemProperties.System.RecordedTV.IsSAP;
					if (!hashtable.ContainsKey(isSAP))
					{
						hashtable.Add(isSAP, shellObjectParent.Properties.CreateTypedProperty<bool?>(isSAP));
					}
					return hashtable[isSAP] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<string> NetworkAffiliation
			{
				get
				{
					PropertyKey networkAffiliation = SystemProperties.System.RecordedTV.NetworkAffiliation;
					if (!hashtable.ContainsKey(networkAffiliation))
					{
						hashtable.Add(networkAffiliation, shellObjectParent.Properties.CreateTypedProperty<string>(networkAffiliation));
					}
					return hashtable[networkAffiliation] as ShellProperty<string>;
				}
			}

			public ShellProperty<DateTime?> OriginalBroadcastDate
			{
				get
				{
					PropertyKey originalBroadcastDate = SystemProperties.System.RecordedTV.OriginalBroadcastDate;
					if (!hashtable.ContainsKey(originalBroadcastDate))
					{
						hashtable.Add(originalBroadcastDate, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(originalBroadcastDate));
					}
					return hashtable[originalBroadcastDate] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<string> ProgramDescription
			{
				get
				{
					PropertyKey programDescription = SystemProperties.System.RecordedTV.ProgramDescription;
					if (!hashtable.ContainsKey(programDescription))
					{
						hashtable.Add(programDescription, shellObjectParent.Properties.CreateTypedProperty<string>(programDescription));
					}
					return hashtable[programDescription] as ShellProperty<string>;
				}
			}

			public ShellProperty<DateTime?> RecordingTime
			{
				get
				{
					PropertyKey recordingTime = SystemProperties.System.RecordedTV.RecordingTime;
					if (!hashtable.ContainsKey(recordingTime))
					{
						hashtable.Add(recordingTime, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(recordingTime));
					}
					return hashtable[recordingTime] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<string> StationCallSign
			{
				get
				{
					PropertyKey stationCallSign = SystemProperties.System.RecordedTV.StationCallSign;
					if (!hashtable.ContainsKey(stationCallSign))
					{
						hashtable.Add(stationCallSign, shellObjectParent.Properties.CreateTypedProperty<string>(stationCallSign));
					}
					return hashtable[stationCallSign] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> StationName
			{
				get
				{
					PropertyKey stationName = SystemProperties.System.RecordedTV.StationName;
					if (!hashtable.ContainsKey(stationName))
					{
						hashtable.Add(stationName, shellObjectParent.Properties.CreateTypedProperty<string>(stationName));
					}
					return hashtable[stationName] as ShellProperty<string>;
				}
			}

			internal PropertySystemRecordedTV(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemSearch : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<string> AutoSummary
			{
				get
				{
					PropertyKey autoSummary = SystemProperties.System.Search.AutoSummary;
					if (!hashtable.ContainsKey(autoSummary))
					{
						hashtable.Add(autoSummary, shellObjectParent.Properties.CreateTypedProperty<string>(autoSummary));
					}
					return hashtable[autoSummary] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ContainerHash
			{
				get
				{
					PropertyKey containerHash = SystemProperties.System.Search.ContainerHash;
					if (!hashtable.ContainsKey(containerHash))
					{
						hashtable.Add(containerHash, shellObjectParent.Properties.CreateTypedProperty<string>(containerHash));
					}
					return hashtable[containerHash] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> Contents
			{
				get
				{
					PropertyKey contents = SystemProperties.System.Search.Contents;
					if (!hashtable.ContainsKey(contents))
					{
						hashtable.Add(contents, shellObjectParent.Properties.CreateTypedProperty<string>(contents));
					}
					return hashtable[contents] as ShellProperty<string>;
				}
			}

			public ShellProperty<int?> EntryID
			{
				get
				{
					PropertyKey entryID = SystemProperties.System.Search.EntryID;
					if (!hashtable.ContainsKey(entryID))
					{
						hashtable.Add(entryID, shellObjectParent.Properties.CreateTypedProperty<int?>(entryID));
					}
					return hashtable[entryID] as ShellProperty<int?>;
				}
			}

			public ShellProperty<byte[]> ExtendedProperties
			{
				get
				{
					PropertyKey extendedProperties = SystemProperties.System.Search.ExtendedProperties;
					if (!hashtable.ContainsKey(extendedProperties))
					{
						hashtable.Add(extendedProperties, shellObjectParent.Properties.CreateTypedProperty<byte[]>(extendedProperties));
					}
					return hashtable[extendedProperties] as ShellProperty<byte[]>;
				}
			}

			public ShellProperty<DateTime?> GatherTime
			{
				get
				{
					PropertyKey gatherTime = SystemProperties.System.Search.GatherTime;
					if (!hashtable.ContainsKey(gatherTime))
					{
						hashtable.Add(gatherTime, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(gatherTime));
					}
					return hashtable[gatherTime] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<int?> HitCount
			{
				get
				{
					PropertyKey hitCount = SystemProperties.System.Search.HitCount;
					if (!hashtable.ContainsKey(hitCount))
					{
						hashtable.Add(hitCount, shellObjectParent.Properties.CreateTypedProperty<int?>(hitCount));
					}
					return hashtable[hitCount] as ShellProperty<int?>;
				}
			}

			public ShellProperty<bool?> IsClosedDirectory
			{
				get
				{
					PropertyKey isClosedDirectory = SystemProperties.System.Search.IsClosedDirectory;
					if (!hashtable.ContainsKey(isClosedDirectory))
					{
						hashtable.Add(isClosedDirectory, shellObjectParent.Properties.CreateTypedProperty<bool?>(isClosedDirectory));
					}
					return hashtable[isClosedDirectory] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> IsFullyContained
			{
				get
				{
					PropertyKey isFullyContained = SystemProperties.System.Search.IsFullyContained;
					if (!hashtable.ContainsKey(isFullyContained))
					{
						hashtable.Add(isFullyContained, shellObjectParent.Properties.CreateTypedProperty<bool?>(isFullyContained));
					}
					return hashtable[isFullyContained] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<string> QueryFocusedSummary
			{
				get
				{
					PropertyKey queryFocusedSummary = SystemProperties.System.Search.QueryFocusedSummary;
					if (!hashtable.ContainsKey(queryFocusedSummary))
					{
						hashtable.Add(queryFocusedSummary, shellObjectParent.Properties.CreateTypedProperty<string>(queryFocusedSummary));
					}
					return hashtable[queryFocusedSummary] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> QueryFocusedSummaryWithFallback
			{
				get
				{
					PropertyKey queryFocusedSummaryWithFallback = SystemProperties.System.Search.QueryFocusedSummaryWithFallback;
					if (!hashtable.ContainsKey(queryFocusedSummaryWithFallback))
					{
						hashtable.Add(queryFocusedSummaryWithFallback, shellObjectParent.Properties.CreateTypedProperty<string>(queryFocusedSummaryWithFallback));
					}
					return hashtable[queryFocusedSummaryWithFallback] as ShellProperty<string>;
				}
			}

			public ShellProperty<int?> Rank
			{
				get
				{
					PropertyKey rank = SystemProperties.System.Search.Rank;
					if (!hashtable.ContainsKey(rank))
					{
						hashtable.Add(rank, shellObjectParent.Properties.CreateTypedProperty<int?>(rank));
					}
					return hashtable[rank] as ShellProperty<int?>;
				}
			}

			public ShellProperty<string> Store
			{
				get
				{
					PropertyKey store = SystemProperties.System.Search.Store;
					if (!hashtable.ContainsKey(store))
					{
						hashtable.Add(store, shellObjectParent.Properties.CreateTypedProperty<string>(store));
					}
					return hashtable[store] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> UrlToIndex
			{
				get
				{
					PropertyKey urlToIndex = SystemProperties.System.Search.UrlToIndex;
					if (!hashtable.ContainsKey(urlToIndex))
					{
						hashtable.Add(urlToIndex, shellObjectParent.Properties.CreateTypedProperty<string>(urlToIndex));
					}
					return hashtable[urlToIndex] as ShellProperty<string>;
				}
			}

			public ShellProperty<object> UrlToIndexWithModificationTime
			{
				get
				{
					PropertyKey urlToIndexWithModificationTime = SystemProperties.System.Search.UrlToIndexWithModificationTime;
					if (!hashtable.ContainsKey(urlToIndexWithModificationTime))
					{
						hashtable.Add(urlToIndexWithModificationTime, shellObjectParent.Properties.CreateTypedProperty<object>(urlToIndexWithModificationTime));
					}
					return hashtable[urlToIndexWithModificationTime] as ShellProperty<object>;
				}
			}

			internal PropertySystemSearch(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemShell : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<string> OmitFromView
			{
				get
				{
					PropertyKey omitFromView = SystemProperties.System.Shell.OmitFromView;
					if (!hashtable.ContainsKey(omitFromView))
					{
						hashtable.Add(omitFromView, shellObjectParent.Properties.CreateTypedProperty<string>(omitFromView));
					}
					return hashtable[omitFromView] as ShellProperty<string>;
				}
			}

			public ShellProperty<string[]> SFGAOFlagsStrings
			{
				get
				{
					PropertyKey sFGAOFlagsStrings = SystemProperties.System.Shell.SFGAOFlagsStrings;
					if (!hashtable.ContainsKey(sFGAOFlagsStrings))
					{
						hashtable.Add(sFGAOFlagsStrings, shellObjectParent.Properties.CreateTypedProperty<string[]>(sFGAOFlagsStrings));
					}
					return hashtable[sFGAOFlagsStrings] as ShellProperty<string[]>;
				}
			}

			internal PropertySystemShell(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemSoftware : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<DateTime?> DateLastUsed
			{
				get
				{
					PropertyKey dateLastUsed = SystemProperties.System.Software.DateLastUsed;
					if (!hashtable.ContainsKey(dateLastUsed))
					{
						hashtable.Add(dateLastUsed, shellObjectParent.Properties.CreateTypedProperty<DateTime?>(dateLastUsed));
					}
					return hashtable[dateLastUsed] as ShellProperty<DateTime?>;
				}
			}

			public ShellProperty<string> ProductName
			{
				get
				{
					PropertyKey productName = SystemProperties.System.Software.ProductName;
					if (!hashtable.ContainsKey(productName))
					{
						hashtable.Add(productName, shellObjectParent.Properties.CreateTypedProperty<string>(productName));
					}
					return hashtable[productName] as ShellProperty<string>;
				}
			}

			internal PropertySystemSoftware(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemSync : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<string> Comments
			{
				get
				{
					PropertyKey comments = SystemProperties.System.Sync.Comments;
					if (!hashtable.ContainsKey(comments))
					{
						hashtable.Add(comments, shellObjectParent.Properties.CreateTypedProperty<string>(comments));
					}
					return hashtable[comments] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ConflictDescription
			{
				get
				{
					PropertyKey conflictDescription = SystemProperties.System.Sync.ConflictDescription;
					if (!hashtable.ContainsKey(conflictDescription))
					{
						hashtable.Add(conflictDescription, shellObjectParent.Properties.CreateTypedProperty<string>(conflictDescription));
					}
					return hashtable[conflictDescription] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ConflictFirstLocation
			{
				get
				{
					PropertyKey conflictFirstLocation = SystemProperties.System.Sync.ConflictFirstLocation;
					if (!hashtable.ContainsKey(conflictFirstLocation))
					{
						hashtable.Add(conflictFirstLocation, shellObjectParent.Properties.CreateTypedProperty<string>(conflictFirstLocation));
					}
					return hashtable[conflictFirstLocation] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ConflictSecondLocation
			{
				get
				{
					PropertyKey conflictSecondLocation = SystemProperties.System.Sync.ConflictSecondLocation;
					if (!hashtable.ContainsKey(conflictSecondLocation))
					{
						hashtable.Add(conflictSecondLocation, shellObjectParent.Properties.CreateTypedProperty<string>(conflictSecondLocation));
					}
					return hashtable[conflictSecondLocation] as ShellProperty<string>;
				}
			}

			public ShellProperty<IntPtr?> HandlerCollectionID
			{
				get
				{
					PropertyKey handlerCollectionID = SystemProperties.System.Sync.HandlerCollectionID;
					if (!hashtable.ContainsKey(handlerCollectionID))
					{
						hashtable.Add(handlerCollectionID, shellObjectParent.Properties.CreateTypedProperty<IntPtr?>(handlerCollectionID));
					}
					return hashtable[handlerCollectionID] as ShellProperty<IntPtr?>;
				}
			}

			public ShellProperty<string> HandlerID
			{
				get
				{
					PropertyKey handlerID = SystemProperties.System.Sync.HandlerID;
					if (!hashtable.ContainsKey(handlerID))
					{
						hashtable.Add(handlerID, shellObjectParent.Properties.CreateTypedProperty<string>(handlerID));
					}
					return hashtable[handlerID] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> HandlerName
			{
				get
				{
					PropertyKey handlerName = SystemProperties.System.Sync.HandlerName;
					if (!hashtable.ContainsKey(handlerName))
					{
						hashtable.Add(handlerName, shellObjectParent.Properties.CreateTypedProperty<string>(handlerName));
					}
					return hashtable[handlerName] as ShellProperty<string>;
				}
			}

			public ShellProperty<uint?> HandlerType
			{
				get
				{
					PropertyKey handlerType = SystemProperties.System.Sync.HandlerType;
					if (!hashtable.ContainsKey(handlerType))
					{
						hashtable.Add(handlerType, shellObjectParent.Properties.CreateTypedProperty<uint?>(handlerType));
					}
					return hashtable[handlerType] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string> HandlerTypeLabel
			{
				get
				{
					PropertyKey handlerTypeLabel = SystemProperties.System.Sync.HandlerTypeLabel;
					if (!hashtable.ContainsKey(handlerTypeLabel))
					{
						hashtable.Add(handlerTypeLabel, shellObjectParent.Properties.CreateTypedProperty<string>(handlerTypeLabel));
					}
					return hashtable[handlerTypeLabel] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ItemID
			{
				get
				{
					PropertyKey itemID = SystemProperties.System.Sync.ItemID;
					if (!hashtable.ContainsKey(itemID))
					{
						hashtable.Add(itemID, shellObjectParent.Properties.CreateTypedProperty<string>(itemID));
					}
					return hashtable[itemID] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> ItemName
			{
				get
				{
					PropertyKey itemName = SystemProperties.System.Sync.ItemName;
					if (!hashtable.ContainsKey(itemName))
					{
						hashtable.Add(itemName, shellObjectParent.Properties.CreateTypedProperty<string>(itemName));
					}
					return hashtable[itemName] as ShellProperty<string>;
				}
			}

			public ShellProperty<uint?> ProgressPercentage
			{
				get
				{
					PropertyKey progressPercentage = SystemProperties.System.Sync.ProgressPercentage;
					if (!hashtable.ContainsKey(progressPercentage))
					{
						hashtable.Add(progressPercentage, shellObjectParent.Properties.CreateTypedProperty<uint?>(progressPercentage));
					}
					return hashtable[progressPercentage] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> State
			{
				get
				{
					PropertyKey state = SystemProperties.System.Sync.State;
					if (!hashtable.ContainsKey(state))
					{
						hashtable.Add(state, shellObjectParent.Properties.CreateTypedProperty<uint?>(state));
					}
					return hashtable[state] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string> Status
			{
				get
				{
					PropertyKey status = SystemProperties.System.Sync.Status;
					if (!hashtable.ContainsKey(status))
					{
						hashtable.Add(status, shellObjectParent.Properties.CreateTypedProperty<string>(status));
					}
					return hashtable[status] as ShellProperty<string>;
				}
			}

			internal PropertySystemSync(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemTask : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<string> BillingInformation
			{
				get
				{
					PropertyKey billingInformation = SystemProperties.System.Task.BillingInformation;
					if (!hashtable.ContainsKey(billingInformation))
					{
						hashtable.Add(billingInformation, shellObjectParent.Properties.CreateTypedProperty<string>(billingInformation));
					}
					return hashtable[billingInformation] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> CompletionStatus
			{
				get
				{
					PropertyKey completionStatus = SystemProperties.System.Task.CompletionStatus;
					if (!hashtable.ContainsKey(completionStatus))
					{
						hashtable.Add(completionStatus, shellObjectParent.Properties.CreateTypedProperty<string>(completionStatus));
					}
					return hashtable[completionStatus] as ShellProperty<string>;
				}
			}

			public ShellProperty<string> Owner
			{
				get
				{
					PropertyKey owner = SystemProperties.System.Task.Owner;
					if (!hashtable.ContainsKey(owner))
					{
						hashtable.Add(owner, shellObjectParent.Properties.CreateTypedProperty<string>(owner));
					}
					return hashtable[owner] as ShellProperty<string>;
				}
			}

			internal PropertySystemTask(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemVideo : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<string> Compression
			{
				get
				{
					PropertyKey compression = SystemProperties.System.Video.Compression;
					if (!hashtable.ContainsKey(compression))
					{
						hashtable.Add(compression, shellObjectParent.Properties.CreateTypedProperty<string>(compression));
					}
					return hashtable[compression] as ShellProperty<string>;
				}
			}

			public ShellProperty<string[]> Director
			{
				get
				{
					PropertyKey director = SystemProperties.System.Video.Director;
					if (!hashtable.ContainsKey(director))
					{
						hashtable.Add(director, shellObjectParent.Properties.CreateTypedProperty<string[]>(director));
					}
					return hashtable[director] as ShellProperty<string[]>;
				}
			}

			public ShellProperty<uint?> EncodingBitrate
			{
				get
				{
					PropertyKey encodingBitrate = SystemProperties.System.Video.EncodingBitrate;
					if (!hashtable.ContainsKey(encodingBitrate))
					{
						hashtable.Add(encodingBitrate, shellObjectParent.Properties.CreateTypedProperty<uint?>(encodingBitrate));
					}
					return hashtable[encodingBitrate] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> FourCC
			{
				get
				{
					PropertyKey fourCC = SystemProperties.System.Video.FourCC;
					if (!hashtable.ContainsKey(fourCC))
					{
						hashtable.Add(fourCC, shellObjectParent.Properties.CreateTypedProperty<uint?>(fourCC));
					}
					return hashtable[fourCC] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> FrameHeight
			{
				get
				{
					PropertyKey frameHeight = SystemProperties.System.Video.FrameHeight;
					if (!hashtable.ContainsKey(frameHeight))
					{
						hashtable.Add(frameHeight, shellObjectParent.Properties.CreateTypedProperty<uint?>(frameHeight));
					}
					return hashtable[frameHeight] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> FrameRate
			{
				get
				{
					PropertyKey frameRate = SystemProperties.System.Video.FrameRate;
					if (!hashtable.ContainsKey(frameRate))
					{
						hashtable.Add(frameRate, shellObjectParent.Properties.CreateTypedProperty<uint?>(frameRate));
					}
					return hashtable[frameRate] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> FrameWidth
			{
				get
				{
					PropertyKey frameWidth = SystemProperties.System.Video.FrameWidth;
					if (!hashtable.ContainsKey(frameWidth))
					{
						hashtable.Add(frameWidth, shellObjectParent.Properties.CreateTypedProperty<uint?>(frameWidth));
					}
					return hashtable[frameWidth] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> HorizontalAspectRatio
			{
				get
				{
					PropertyKey horizontalAspectRatio = SystemProperties.System.Video.HorizontalAspectRatio;
					if (!hashtable.ContainsKey(horizontalAspectRatio))
					{
						hashtable.Add(horizontalAspectRatio, shellObjectParent.Properties.CreateTypedProperty<uint?>(horizontalAspectRatio));
					}
					return hashtable[horizontalAspectRatio] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<uint?> SampleSize
			{
				get
				{
					PropertyKey sampleSize = SystemProperties.System.Video.SampleSize;
					if (!hashtable.ContainsKey(sampleSize))
					{
						hashtable.Add(sampleSize, shellObjectParent.Properties.CreateTypedProperty<uint?>(sampleSize));
					}
					return hashtable[sampleSize] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<string> StreamName
			{
				get
				{
					PropertyKey streamName = SystemProperties.System.Video.StreamName;
					if (!hashtable.ContainsKey(streamName))
					{
						hashtable.Add(streamName, shellObjectParent.Properties.CreateTypedProperty<string>(streamName));
					}
					return hashtable[streamName] as ShellProperty<string>;
				}
			}

			public ShellProperty<ushort?> StreamNumber
			{
				get
				{
					PropertyKey streamNumber = SystemProperties.System.Video.StreamNumber;
					if (!hashtable.ContainsKey(streamNumber))
					{
						hashtable.Add(streamNumber, shellObjectParent.Properties.CreateTypedProperty<ushort?>(streamNumber));
					}
					return hashtable[streamNumber] as ShellProperty<ushort?>;
				}
			}

			public ShellProperty<uint?> TotalBitrate
			{
				get
				{
					PropertyKey totalBitrate = SystemProperties.System.Video.TotalBitrate;
					if (!hashtable.ContainsKey(totalBitrate))
					{
						hashtable.Add(totalBitrate, shellObjectParent.Properties.CreateTypedProperty<uint?>(totalBitrate));
					}
					return hashtable[totalBitrate] as ShellProperty<uint?>;
				}
			}

			public ShellProperty<bool?> TranscodedForSync
			{
				get
				{
					PropertyKey transcodedForSync = SystemProperties.System.Video.TranscodedForSync;
					if (!hashtable.ContainsKey(transcodedForSync))
					{
						hashtable.Add(transcodedForSync, shellObjectParent.Properties.CreateTypedProperty<bool?>(transcodedForSync));
					}
					return hashtable[transcodedForSync] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<uint?> VerticalAspectRatio
			{
				get
				{
					PropertyKey verticalAspectRatio = SystemProperties.System.Video.VerticalAspectRatio;
					if (!hashtable.ContainsKey(verticalAspectRatio))
					{
						hashtable.Add(verticalAspectRatio, shellObjectParent.Properties.CreateTypedProperty<uint?>(verticalAspectRatio));
					}
					return hashtable[verticalAspectRatio] as ShellProperty<uint?>;
				}
			}

			internal PropertySystemVideo(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		public class PropertySystemVolume : PropertyStoreItems
		{
			private ShellObject shellObjectParent;

			private Hashtable hashtable = new Hashtable();

			public ShellProperty<string> FileSystem
			{
				get
				{
					PropertyKey fileSystem = SystemProperties.System.Volume.FileSystem;
					if (!hashtable.ContainsKey(fileSystem))
					{
						hashtable.Add(fileSystem, shellObjectParent.Properties.CreateTypedProperty<string>(fileSystem));
					}
					return hashtable[fileSystem] as ShellProperty<string>;
				}
			}

			public ShellProperty<bool?> IsMappedDrive
			{
				get
				{
					PropertyKey isMappedDrive = SystemProperties.System.Volume.IsMappedDrive;
					if (!hashtable.ContainsKey(isMappedDrive))
					{
						hashtable.Add(isMappedDrive, shellObjectParent.Properties.CreateTypedProperty<bool?>(isMappedDrive));
					}
					return hashtable[isMappedDrive] as ShellProperty<bool?>;
				}
			}

			public ShellProperty<bool?> IsRoot
			{
				get
				{
					PropertyKey isRoot = SystemProperties.System.Volume.IsRoot;
					if (!hashtable.ContainsKey(isRoot))
					{
						hashtable.Add(isRoot, shellObjectParent.Properties.CreateTypedProperty<bool?>(isRoot));
					}
					return hashtable[isRoot] as ShellProperty<bool?>;
				}
			}

			internal PropertySystemVolume(ShellObject parent)
			{
				shellObjectParent = parent;
			}
		}

		private ShellPropertyCollection defaultPropertyCollection;

		private PropertySystem propertySystem;

		private ShellObject ParentShellObject { get; set; }

		public PropertySystem System
		{
			get
			{
				if (propertySystem == null)
				{
					propertySystem = new PropertySystem(ParentShellObject);
				}
				return propertySystem;
			}
		}

		public ShellPropertyCollection DefaultPropertyCollection
		{
			get
			{
				if (defaultPropertyCollection == null)
				{
					defaultPropertyCollection = new ShellPropertyCollection(ParentShellObject);
				}
				return defaultPropertyCollection;
			}
		}

		internal ShellProperties(ShellObject parent)
		{
			ParentShellObject = parent;
		}

		public IShellProperty GetProperty(PropertyKey key)
		{
			return CreateTypedProperty(key);
		}

		public IShellProperty GetProperty(string canonicalName)
		{
			return CreateTypedProperty(canonicalName);
		}

		public ShellProperty<T> GetProperty<T>(PropertyKey key)
		{
			return CreateTypedProperty(key) as ShellProperty<T>;
		}

		public ShellProperty<T> GetProperty<T>(string canonicalName)
		{
			return CreateTypedProperty(canonicalName) as ShellProperty<T>;
		}

		public ShellPropertyWriter GetPropertyWriter()
		{
			return new ShellPropertyWriter(ParentShellObject);
		}

		internal IShellProperty CreateTypedProperty<T>(PropertyKey propKey)
		{
			ShellPropertyDescription propertyDescription = ShellPropertyDescriptionsCache.Cache.GetPropertyDescription(propKey);
			return new ShellProperty<T>(propKey, propertyDescription, ParentShellObject);
		}

		internal IShellProperty CreateTypedProperty(PropertyKey propKey)
		{
			return ShellPropertyFactory.CreateShellProperty(propKey, ParentShellObject);
		}

		internal IShellProperty CreateTypedProperty(string canonicalName)
		{
			PropertyKey propkey;
			int num = PropertySystemNativeMethods.PSGetPropertyKeyFromName(canonicalName, out propkey);
			if (!CoreErrorHelper.Succeeded(num))
			{
				throw new ArgumentException(LocalizedMessages.ShellInvalidCanonicalName, Marshal.GetExceptionForHR(num));
			}
			return CreateTypedProperty(propkey);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposed)
		{
			if (disposed && defaultPropertyCollection != null)
			{
				defaultPropertyCollection.Dispose();
			}
		}
	}
}
