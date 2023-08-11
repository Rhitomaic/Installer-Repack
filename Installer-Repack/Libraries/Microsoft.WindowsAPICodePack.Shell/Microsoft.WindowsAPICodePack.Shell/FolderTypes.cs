using System;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.Shell.Resources;

namespace Microsoft.WindowsAPICodePack.Shell
{
	internal static class FolderTypes
	{
		internal static Guid NotSpecified;

		internal static Guid Invalid;

		internal static Guid Documents;

		internal static Guid Pictures;

		internal static Guid Music;

		internal static Guid MusicIcons;

		internal static Guid Games;

		internal static Guid ControlPanelCategory;

		internal static Guid ControlPanelClassic;

		internal static Guid Printers;

		internal static Guid RecycleBin;

		internal static Guid SoftwareExplorer;

		internal static Guid CompressedFolder;

		internal static Guid Contacts;

		internal static Guid Library;

		internal static Guid NetworkExplorer;

		internal static Guid UserFiles;

		internal static Guid GenericSearchResults;

		internal static Guid GenericLibrary;

		internal static Guid Videos;

		internal static Guid UsersLibraries;

		internal static Guid OtherUsers;

		internal static Guid Communications;

		internal static Guid RecordedTV;

		internal static Guid SavedGames;

		internal static Guid OpenSearch;

		internal static Guid SearchConnector;

		internal static Guid Searches;

		private static Dictionary<Guid, string> types;

		static FolderTypes()
		{
			NotSpecified = new Guid(1548691637u, 63593, 20100, 142, 96, 241, 29, 185, 124, 92, 199);
			Invalid = new Guid(1468037272u, 35919, 17506, 187, 99, 113, 4, 35, 128, 177, 9);
			Documents = new Guid(2101991206, 15393, 20229, 153, 170, 253, 194, 201, 71, 70, 86);
			Pictures = new Guid(3010006616u, 59745, 16955, 182, 135, 56, 110, 191, 216, 50, 57);
			Music = new Guid(2946237398u, 32185, 18965, 148, 100, 19, 191, 159, 182, 154, 42);
			MusicIcons = new Guid(192178171u, 33978, 19118, 160, 155, 21, 183, 16, 151, 175, 158);
			Games = new Guid(3062477008u, 30419, 19643, 135, 247, 88, 93, 14, 12, 224, 112);
			ControlPanelCategory = new Guid(3729720928u, 64016, 19343, 164, 148, 6, 139, 32, 178, 35, 7);
			ControlPanelClassic = new Guid(204969203u, 46405, 17322, 163, 41, 195, 116, 48, 197, 141, 42);
			Printers = new Guid(746307270u, 51268, 18954, 145, 250, 206, 246, 245, 156, 253, 161);
			RecycleBin = new Guid(3604602884u, 52615, 17451, 157, 87, 94, 10, 235, 79, 111, 114);
			SoftwareExplorer = new Guid(3597941019u, 21209, 19975, 131, 78, 103, 201, 134, 16, 243, 157);
			CompressedFolder = new Guid(2149662338u, 48381, 19535, 136, 23, 187, 39, 96, 18, 103, 169);
			Contacts = new Guid(3727388908u, 39927, 19091, 189, 61, 36, 63, 120, 129, 212, 146);
			Library = new Guid(1269693544u, 50348, 18198, 160, 160, 77, 93, 170, 107, 15, 62);
			NetworkExplorer = new Guid(634135595u, 39548, 20305, 128, 224, 122, 41, 40, 254, 190, 66);
			UserFiles = new Guid(3440363163u, 29154, 18149, 150, 144, 91, 205, 159, 87, 170, 179);
			GenericSearchResults = new Guid(2145262110u, 35633, 18853, 147, 184, 107, 225, 76, 250, 73, 67);
			GenericLibrary = new Guid(1598991258, 26675, 20321, 137, 157, 49, 207, 70, 151, 157, 73);
			Videos = new Guid(1604936711, 32375, 18492, 172, 147, 105, 29, 5, 133, 13, 232);
			UsersLibraries = new Guid(3302592265u, 24868, 20448, 153, 66, 130, 100, 22, 8, 45, 169);
			OtherUsers = new Guid(3006790912u, 40405, 17973, 166, 212, 218, 51, 253, 16, 43, 122);
			Communications = new Guid(2437373925u, 22635, 20154, 141, 117, 209, 116, 52, 184, 205, 246);
			RecordedTV = new Guid(1431806607, 23974, 20355, 136, 9, 194, 201, 138, 17, 166, 250);
			SavedGames = new Guid(3493212935u, 10443, 16646, 159, 35, 41, 86, 227, 229, 224, 231);
			OpenSearch = new Guid(2410649129u, 6528, 18175, 128, 35, 157, 206, 171, 156, 62, 227);
			SearchConnector = new Guid(2552702446u, 28487, 18334, 180, 71, 129, 43, 250, 125, 46, 143);
			Searches = new Guid(185311971, 16479, 16734, 166, 238, 202, 214, 37, 32, 120, 83);
			types = new Dictionary<Guid, string>();
			types.Add(NotSpecified, LocalizedMessages.FolderTypeNotSpecified);
			types.Add(Invalid, LocalizedMessages.FolderTypeInvalid);
			types.Add(Communications, LocalizedMessages.FolderTypeCommunications);
			types.Add(CompressedFolder, LocalizedMessages.FolderTypeCompressedFolder);
			types.Add(Contacts, LocalizedMessages.FolderTypeContacts);
			types.Add(ControlPanelCategory, LocalizedMessages.FolderTypeCategory);
			types.Add(ControlPanelClassic, LocalizedMessages.FolderTypeClassic);
			types.Add(Documents, LocalizedMessages.FolderTypeDocuments);
			types.Add(Games, LocalizedMessages.FolderTypeGames);
			types.Add(GenericSearchResults, LocalizedMessages.FolderTypeSearchResults);
			types.Add(GenericLibrary, LocalizedMessages.FolderTypeGenericLibrary);
			types.Add(Library, LocalizedMessages.FolderTypeLibrary);
			types.Add(Music, LocalizedMessages.FolderTypeMusic);
			types.Add(MusicIcons, LocalizedMessages.FolderTypeMusicIcons);
			types.Add(NetworkExplorer, LocalizedMessages.FolderTypeNetworkExplorer);
			types.Add(OtherUsers, LocalizedMessages.FolderTypeOtherUsers);
			types.Add(OpenSearch, LocalizedMessages.FolderTypeOpenSearch);
			types.Add(Pictures, LocalizedMessages.FolderTypePictures);
			types.Add(Printers, LocalizedMessages.FolderTypePrinters);
			types.Add(RecycleBin, LocalizedMessages.FolderTypeRecycleBin);
			types.Add(RecordedTV, LocalizedMessages.FolderTypeRecordedTV);
			types.Add(SoftwareExplorer, LocalizedMessages.FolderTypeSoftwareExplorer);
			types.Add(SavedGames, LocalizedMessages.FolderTypeSavedGames);
			types.Add(SearchConnector, LocalizedMessages.FolderTypeSearchConnector);
			types.Add(Searches, LocalizedMessages.FolderTypeSearches);
			types.Add(UsersLibraries, LocalizedMessages.FolderTypeUserLibraries);
			types.Add(UserFiles, LocalizedMessages.FolderTypeUserFiles);
			types.Add(Videos, LocalizedMessages.FolderTypeVideos);
		}

		internal static string GetFolderType(Guid typeId)
		{
			string value;
			return types.TryGetValue(typeId, out value) ? value : string.Empty;
		}
	}
}
