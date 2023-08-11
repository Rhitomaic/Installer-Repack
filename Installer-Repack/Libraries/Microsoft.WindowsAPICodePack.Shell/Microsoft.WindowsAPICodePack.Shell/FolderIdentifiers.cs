using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.WindowsAPICodePack.Shell.Resources;

namespace Microsoft.WindowsAPICodePack.Shell
{
	internal static class FolderIdentifiers
	{
		private static Dictionary<Guid, string> folders;

		internal static Guid Computer;

		internal static Guid Conflict;

		internal static Guid ControlPanel;

		internal static Guid Desktop;

		internal static Guid Internet;

		internal static Guid Network;

		internal static Guid Printers;

		internal static Guid SyncManager;

		internal static Guid Connections;

		internal static Guid SyncSetup;

		internal static Guid SyncResults;

		internal static Guid RecycleBin;

		internal static Guid Fonts;

		internal static Guid Startup;

		internal static Guid Programs;

		internal static Guid StartMenu;

		internal static Guid Recent;

		internal static Guid SendTo;

		internal static Guid Documents;

		internal static Guid Favorites;

		internal static Guid NetHood;

		internal static Guid PrintHood;

		internal static Guid Templates;

		internal static Guid CommonStartup;

		internal static Guid CommonPrograms;

		internal static Guid CommonStartMenu;

		internal static Guid PublicDesktop;

		internal static Guid ProgramData;

		internal static Guid CommonTemplates;

		internal static Guid PublicDocuments;

		internal static Guid RoamingAppData;

		internal static Guid LocalAppData;

		internal static Guid LocalAppDataLow;

		internal static Guid InternetCache;

		internal static Guid Cookies;

		internal static Guid History;

		internal static Guid System;

		internal static Guid SystemX86;

		internal static Guid Windows;

		internal static Guid Profile;

		internal static Guid Pictures;

		internal static Guid ProgramFilesX86;

		internal static Guid ProgramFilesCommonX86;

		internal static Guid ProgramFilesX64;

		internal static Guid ProgramFilesCommonX64;

		internal static Guid ProgramFiles;

		internal static Guid ProgramFilesCommon;

		internal static Guid AdminTools;

		internal static Guid CommonAdminTools;

		internal static Guid Music;

		internal static Guid Videos;

		internal static Guid PublicPictures;

		internal static Guid PublicMusic;

		internal static Guid PublicVideos;

		internal static Guid ResourceDir;

		internal static Guid LocalizedResourcesDir;

		internal static Guid CommonOEMLinks;

		internal static Guid CDBurning;

		internal static Guid UserProfiles;

		internal static Guid Playlists;

		internal static Guid SamplePlaylists;

		internal static Guid SampleMusic;

		internal static Guid SamplePictures;

		internal static Guid SampleVideos;

		internal static Guid PhotoAlbums;

		internal static Guid Public;

		internal static Guid ChangeRemovePrograms;

		internal static Guid AppUpdates;

		internal static Guid AddNewPrograms;

		internal static Guid Downloads;

		internal static Guid PublicDownloads;

		internal static Guid SavedSearches;

		internal static Guid QuickLaunch;

		internal static Guid Contacts;

		internal static Guid SidebarParts;

		internal static Guid SidebarDefaultParts;

		internal static Guid TreeProperties;

		internal static Guid PublicGameTasks;

		internal static Guid GameTasks;

		internal static Guid SavedGames;

		internal static Guid Games;

		internal static Guid RecordedTV;

		internal static Guid SearchMapi;

		internal static Guid SearchCsc;

		internal static Guid Links;

		internal static Guid UsersFiles;

		internal static Guid SearchHome;

		internal static Guid OriginalImages;

		internal static Guid UserProgramFiles;

		internal static Guid UserProgramFilesCommon;

		internal static Guid Ringtones;

		internal static Guid PublicRingtones;

		internal static Guid UsersLibraries;

		internal static Guid DocumentsLibrary;

		internal static Guid MusicLibrary;

		internal static Guid PicturesLibrary;

		internal static Guid VideosLibrary;

		internal static Guid RecordedTVLibrary;

		internal static Guid OtherUsers;

		internal static Guid DeviceMetadataStore;

		internal static Guid Libraries;

		internal static Guid UserPinned;

		internal static Guid ImplicitAppShortcuts;

		static FolderIdentifiers()
		{
			Computer = new Guid(180388732u, 48120, 17706, 133, 13, 121, 208, 142, 102, 124, 167);
			Conflict = new Guid(1275001669, 13437, 16390, 165, 190, 172, 12, 176, 86, 113, 146);
			ControlPanel = new Guid(2192001771u, 44724, 18012, 160, 20, 208, 151, 238, 52, 109, 99);
			Desktop = new Guid(3032468538u, 56108, 16972, 176, 41, 127, 233, 154, 135, 198, 65);
			Internet = new Guid(1302296692, 19980, 18692, 150, 123, 64, 176, 210, 12, 62, 75);
			Network = new Guid(3523997380u, 23720, 18693, 174, 59, 191, 37, 30, 160, 155, 83);
			Printers = new Guid(1996246573u, 54957, 17689, 166, 99, 55, 189, 86, 6, 129, 133);
			SyncManager = new Guid(1130793976u, 49486, 18866, 151, 201, 116, 119, 132, 215, 132, 183);
			Connections = new Guid(1863113003, 11927, 17873, 136, byte.MaxValue, 176, 209, 134, 184, 222, 221);
			SyncSetup = new Guid(253837624u, 45523, 19088, 187, 169, 39, 203, 192, 197, 56, 154);
			SyncResults = new Guid(681220675u, 48708, 16471, 164, 27, 88, 122, 118, 215, 231, 249);
			RecycleBin = new Guid(3075686470u, 16075, 19480, 190, 78, 100, 205, 76, 183, 214, 172);
			Fonts = new Guid(4246899895u, 44561, 19171, 134, 76, 22, 243, 145, 10, 184, 254);
			Startup = new Guid(3111985339u, 62570, 19607, 186, 16, 94, 54, 8, 67, 8, 84);
			Programs = new Guid(2810142071u, 11819, 17603, 166, 162, 171, 166, 1, 5, 74, 81);
			StartMenu = new Guid(1650152387u, 43848, 20161, 186, 31, 161, 239, 65, 70, 252, 25);
			Recent = new Guid(2924527745u, 60370, 17290, 134, 85, 138, 9, 46, 52, 152, 122);
			SendTo = new Guid(2307064684u, 10176, 16459, 143, 8, 16, 45, 16, 220, 253, 116);
			Documents = new Guid(4258503376u, 9103, 18095, 173, 180, 108, 133, 72, 3, 105, 199);
			Favorites = new Guid(393738081, 26797, 19850, 135, 189, 48, 183, 89, 250, 51, 221);
			NetHood = new Guid(3316367187u, 57727, 16673, 137, 0, 134, 98, 111, 194, 201, 115);
			PrintHood = new Guid(2457124237u, 53201, 16835, 179, 94, 177, 63, 85, 167, 88, 244);
			Templates = new Guid(2788332520u, 26190, 18651, 160, 121, 223, 117, 158, 5, 9, 247);
			CommonStartup = new Guid(2191911477u, 55757, 18373, 150, 41, 225, 93, 47, 113, 78, 110);
			CommonPrograms = new Guid(20567118, 27390, 18930, 134, 144, 61, 175, 202, 230, byte.MaxValue, 184);
			CommonStartMenu = new Guid(2752599833u, 54830, 18717, 170, 124, 231, 75, 139, 227, 176, 103);
			PublicDesktop = new Guid(3299488781u, 61967, 18531, 175, 239, 248, 126, 242, 230, 186, 37);
			ProgramData = new Guid(1655397762u, 64961, 19907, 169, 221, 7, 13, 29, 73, 93, 151);
			CommonTemplates = new Guid(3108124647u, 22444, 17223, 145, 81, 176, 140, 108, 50, 209, 247);
			PublicDocuments = new Guid(3980928175u, 56548, 17832, 129, 226, 252, 121, 101, 8, 54, 52);
			RoamingAppData = new Guid(1052149211, 26105, 19702, 160, 58, 227, 239, 101, 114, 159, 61);
			LocalAppData = new Guid(4055050117u, 28602, 20431, 157, 85, 123, 142, 127, 21, 112, 145);
			LocalAppDataLow = new Guid(2770379172u, 6016, 20470, 189, 24, 22, 115, 67, 197, 175, 22);
			InternetCache = new Guid(891585000, 13246, 16977, 186, 133, 96, 7, 202, 237, 207, 157);
			Cookies = new Guid(722433629u, 49385, 16753, 144, 142, 8, 166, 17, 184, 79, 246);
			History = new Guid(3655109179u, 46980, 17198, 167, 129, 90, 17, 48, 167, 89, 99);
			System = new Guid(448876151, 743, 20061, 183, 68, 46, 177, 174, 81, 152, 183);
			SystemX86 = new Guid(3595710896u, 45809, 18519, 164, 206, 168, 231, 198, 234, 125, 39);
			Windows = new Guid(4086035460u, 7491, 17138, 147, 5, 103, 222, 11, 40, 252, 35);
			Profile = new Guid(1584170383, 3618, 18272, 154, 254, 234, 51, 23, 182, 113, 115);
			Pictures = new Guid(870482224, 19998, 18038, 131, 90, 152, 57, 92, 59, 195, 187);
			ProgramFilesX86 = new Guid(2086289647u, 41211, 19452, 135, 74, 192, 242, 224, 185, 250, 142);
			ProgramFilesCommonX86 = new Guid(3734457636u, 55750, 19774, 191, 145, 244, 69, 81, 32, 185, 23);
			ProgramFilesX64 = new Guid(1837142903, 27376, 17483, 137, 87, 163, 119, 63, 2, 32, 14);
			ProgramFilesCommonX64 = new Guid(1667618215, 3853, 17893, 135, 246, 13, 165, 107, 106, 79, 125);
			ProgramFiles = new Guid(2422105014u, 49599, 18766, 178, 156, 101, 183, 50, 211, 210, 26);
			ProgramFilesCommon = new Guid(4159827205u, 40813, 18338, 170, 174, 41, 211, 23, 198, 240, 102);
			AdminTools = new Guid(1917776240u, 42029, 20463, 159, 38, 182, 14, 132, 111, 186, 79);
			CommonAdminTools = new Guid(3493351037u, 47811, 18327, 143, 20, 203, 162, 41, 179, 146, 181);
			Music = new Guid(1272501617, 27929, 18643, 190, 151, 66, 34, 32, 8, 14, 67);
			Videos = new Guid(412654365u, 39349, 17755, 132, 28, 171, 124, 116, 228, 221, 252);
			PublicPictures = new Guid(3068918662u, 26887, 16700, 154, 247, 79, 194, 171, 240, 124, 197);
			PublicMusic = new Guid(840235701u, 38743, 17048, 187, 97, 146, 169, 222, 170, 68, byte.MaxValue);
			PublicVideos = new Guid(603985978, 24965, 18939, 162, 216, 74, 57, 42, 96, 43, 163);
			ResourceDir = new Guid(2328955953u, 10971, 17046, 168, 247, 228, 112, 18, 50, 201, 114);
			LocalizedResourcesDir = new Guid(704657246, 8780, 18910, 184, 209, 68, 13, 247, 239, 61, 220);
			CommonOEMLinks = new Guid(3250250448u, 4319, 17204, 190, 221, 122, 162, 11, 34, 122, 157);
			CDBurning = new Guid(2656217872u, 63501, 18911, 172, 184, 67, 48, 245, 104, 120, 85);
			UserProfiles = new Guid(123916914u, 50442, 19376, 163, 130, 105, 125, 205, 114, 155, 128);
			Playlists = new Guid(3734159815u, 33663, 20329, 163, 187, 134, 230, 49, 32, 74, 35);
			SamplePlaylists = new Guid(365586867, 12526, 18881, 172, 225, 107, 94, 195, 114, 175, 181);
			SampleMusic = new Guid(2991638120u, 62845, 20193, 166, 60, 41, 14, 231, 209, 170, 31);
			SamplePictures = new Guid(3297772864u, 9081, 19573, 132, 75, 100, 230, 250, 248, 113, 107);
			SampleVideos = new Guid(2241768852u, 11909, 18605, 167, 26, 9, 105, 203, 86, 166, 205);
			PhotoAlbums = new Guid(1775423376u, 64563, 20407, 154, 12, 235, 176, 240, 252, 180, 60);
			Public = new Guid(3755964066u, 51242, 19811, 144, 106, 86, 68, 172, 69, 115, 133);
			ChangeRemovePrograms = new Guid(3748816556u, 37492, 18535, 141, 85, 59, 214, 97, 222, 135, 45);
			AppUpdates = new Guid(2735066777u, 62759, 18731, 139, 26, 126, 118, 250, 152, 214, 228);
			AddNewPrograms = new Guid(3730954609u, 24252, 20226, 163, 169, 108, 130, 137, 94, 92, 4);
			Downloads = new Guid(927851152, 4671, 17765, 145, 100, 57, 196, 146, 94, 70, 123);
			PublicDownloads = new Guid(1029983387, 8120, 20272, 155, 69, 246, 112, 35, 95, 121, 192);
			SavedSearches = new Guid(2099067396u, 57019, 16661, 149, 207, 47, 41, 218, 41, 32, 218);
			QuickLaunch = new Guid(1386541089, 31605, 18601, 159, 107, 75, 135, 162, 16, 188, 143);
			Contacts = new Guid(1450723412u, 50891, 17963, 129, 105, 136, 227, 80, 172, 184, 130);
			SidebarParts = new Guid(2807903790u, 20732, 20407, 172, 44, 168, 190, 170, 49, 68, 147);
			SidebarDefaultParts = new Guid(2067361364u, 40645, 17152, 190, 10, 36, 130, 235, 174, 26, 38);
			TreeProperties = new Guid(1530349997u, 46239, 18881, 131, 235, 21, 55, 15, 189, 72, 130);
			PublicGameTasks = new Guid(3737068854u, 57768, 19545, 182, 162, 65, 69, 134, 71, 106, 234);
			GameTasks = new Guid(89108065, 19928, 18311, 128, 182, 9, 2, 32, 196, 183, 0);
			SavedGames = new Guid(1281110783u, 48029, 17328, 181, 180, 45, 114, 229, 78, 170, 164);
			Games = new Guid(3401919514u, 46397, 20188, 146, 215, 107, 46, 138, 193, 148, 52);
			RecordedTV = new Guid(3179667457u, 4398, 17182, 152, 59, 123, 21, 172, 9, byte.MaxValue, 241);
			SearchMapi = new Guid(2565606936u, 8344, 19780, 134, 68, 102, 151, 147, 21, 162, 129);
			SearchCsc = new Guid(3996312646u, 12746, 19130, 129, 79, 165, 235, 210, 253, 109, 94);
			Links = new Guid(3216627168u, 50857, 16460, 178, 178, 174, 109, 182, 175, 73, 104);
			UsersFiles = new Guid(4090367868u, 18689, 19148, 134, 72, 213, 212, 75, 4, 239, 143);
			SearchHome = new Guid(419641297u, 47306, 16673, 166, 57, 109, 71, 45, 22, 151, 42);
			OriginalImages = new Guid(741785770, 22546, 19335, 191, 208, 76, 208, 223, 177, 155, 57);
			UserProgramFiles = new Guid(1557638882, 8729, 19047, 184, 93, 108, 156, 225, 86, 96, 203);
			UserProgramFilesCommon = new Guid(3166515287u, 51804, 17954, 180, 45, 188, 86, 219, 10, 229, 22);
			Ringtones = new Guid(3362784331u, 62622, 16678, 169, 195, 181, 42, 31, 244, 17, 232);
			PublicRingtones = new Guid(3847596896u, 5435, 19735, 159, 4, 165, 254, 153, 252, 21, 236);
			UsersLibraries = new Guid(2734838877u, 57087, 17995, 171, 232, 97, 200, 100, 141, 147, 155);
			DocumentsLibrary = new Guid(2064494973u, 40146, 19091, 151, 51, 70, 204, 137, 2, 46, 124);
			MusicLibrary = new Guid(554871562u, 51306, 20478, 163, 104, 13, 233, 110, 71, 1, 46);
			PicturesLibrary = new Guid(2844831391u, 41019, 20096, 148, 188, 153, 18, 215, 80, 65, 4);
			VideosLibrary = new Guid(1226740271, 22083, 19188, 167, 235, 78, 122, 19, 141, 129, 116);
			RecordedTVLibrary = new Guid(443538338u, 62509, 17240, 167, 152, 183, 77, 116, 89, 38, 197);
			OtherUsers = new Guid(1381141099u, 47587, 19165, 182, 13, 88, 140, 45, 186, 132, 45);
			DeviceMetadataStore = new Guid(1558488553u, 58603, 18333, 184, 159, 19, 12, 2, 136, 97, 85);
			Libraries = new Guid(457090524u, 46471, 18310, 180, 239, 189, 29, 195, 50, 174, 174);
			UserPinned = new Guid(2654573995u, 8092, 20243, 184, 39, 72, 178, 75, 108, 113, 116);
			ImplicitAppShortcuts = new Guid(3165988207u, 31222, 19694, 183, 37, 220, 52, 228, 2, 253, 70);
			folders = new Dictionary<Guid, string>();
			Type typeFromHandle = typeof(FolderIdentifiers);
			FieldInfo[] fields = typeFromHandle.GetFields(BindingFlags.Static | BindingFlags.NonPublic);
			FieldInfo[] array = fields;
			foreach (FieldInfo fieldInfo in array)
			{
				if (fieldInfo.FieldType == typeof(Guid))
				{
					Guid key = (Guid)fieldInfo.GetValue(null);
					string name = fieldInfo.Name;
					folders.Add(key, name);
				}
			}
		}

		internal static string NameForGuid(Guid folderId)
		{
			if (!folders.TryGetValue(folderId, out var value))
			{
				throw new ArgumentException(LocalizedMessages.FolderIdsUnknownGuid, "folderId");
			}
			return value;
		}

		internal static SortedList<string, Guid> GetAllFolders()
		{
			ICollection<Guid> keys = folders.Keys;
			SortedList<string, Guid> sortedList = new SortedList<string, Guid>();
			foreach (Guid item in keys)
			{
				sortedList.Add(folders[item], item);
			}
			return sortedList;
		}
	}
}
