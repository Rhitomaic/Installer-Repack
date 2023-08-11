using System;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Shell.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	public static class SystemProperties
	{
		public static class System
		{
			public static class AppUserModel
			{
				public static PropertyKey ExcludeFromShowInNewInstall => new PropertyKey(new Guid("{9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3}"), 8);

				public static PropertyKey ID => new PropertyKey(new Guid("{9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3}"), 5);

				public static PropertyKey IsDestinationListSeparator => new PropertyKey(new Guid("{9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3}"), 6);

				public static PropertyKey PreventPinning => new PropertyKey(new Guid("{9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3}"), 9);

				public static PropertyKey RelaunchCommand => new PropertyKey(new Guid("{9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3}"), 2);

				public static PropertyKey RelaunchDisplayNameResource => new PropertyKey(new Guid("{9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3}"), 4);

				public static PropertyKey RelaunchIconResource => new PropertyKey(new Guid("{9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3}"), 3);
			}

			public static class Audio
			{
				public static PropertyKey ChannelCount => new PropertyKey(new Guid("{64440490-4C8B-11D1-8B70-080036B11A03}"), 7);

				public static PropertyKey Compression => new PropertyKey(new Guid("{64440490-4C8B-11D1-8B70-080036B11A03}"), 10);

				public static PropertyKey EncodingBitrate => new PropertyKey(new Guid("{64440490-4C8B-11D1-8B70-080036B11A03}"), 4);

				public static PropertyKey Format => new PropertyKey(new Guid("{64440490-4C8B-11D1-8B70-080036B11A03}"), 2);

				public static PropertyKey IsVariableBitrate => new PropertyKey(new Guid("{E6822FEE-8C17-4D62-823C-8E9CFCBD1D5C}"), 100);

				public static PropertyKey PeakValue => new PropertyKey(new Guid("{2579E5D0-1116-4084-BD9A-9B4F7CB4DF5E}"), 100);

				public static PropertyKey SampleRate => new PropertyKey(new Guid("{64440490-4C8B-11D1-8B70-080036B11A03}"), 5);

				public static PropertyKey SampleSize => new PropertyKey(new Guid("{64440490-4C8B-11D1-8B70-080036B11A03}"), 6);

				public static PropertyKey StreamName => new PropertyKey(new Guid("{64440490-4C8B-11D1-8B70-080036B11A03}"), 9);

				public static PropertyKey StreamNumber => new PropertyKey(new Guid("{64440490-4C8B-11D1-8B70-080036B11A03}"), 8);
			}

			public static class Calendar
			{
				public static PropertyKey Duration => new PropertyKey(new Guid("{293CA35A-09AA-4DD2-B180-1FE245728A52}"), 100);

				public static PropertyKey IsOnline => new PropertyKey(new Guid("{BFEE9149-E3E2-49A7-A862-C05988145CEC}"), 100);

				public static PropertyKey IsRecurring => new PropertyKey(new Guid("{315B9C8D-80A9-4EF9-AE16-8E746DA51D70}"), 100);

				public static PropertyKey Location => new PropertyKey(new Guid("{F6272D18-CECC-40B1-B26A-3911717AA7BD}"), 100);

				public static PropertyKey OptionalAttendeeAddresses => new PropertyKey(new Guid("{D55BAE5A-3892-417A-A649-C6AC5AAAEAB3}"), 100);

				public static PropertyKey OptionalAttendeeNames => new PropertyKey(new Guid("{09429607-582D-437F-84C3-DE93A2B24C3C}"), 100);

				public static PropertyKey OrganizerAddress => new PropertyKey(new Guid("{744C8242-4DF5-456C-AB9E-014EFB9021E3}"), 100);

				public static PropertyKey OrganizerName => new PropertyKey(new Guid("{AAA660F9-9865-458E-B484-01BC7FE3973E}"), 100);

				public static PropertyKey ReminderTime => new PropertyKey(new Guid("{72FC5BA4-24F9-4011-9F3F-ADD27AFAD818}"), 100);

				public static PropertyKey RequiredAttendeeAddresses => new PropertyKey(new Guid("{0BA7D6C3-568D-4159-AB91-781A91FB71E5}"), 100);

				public static PropertyKey RequiredAttendeeNames => new PropertyKey(new Guid("{B33AF30B-F552-4584-936C-CB93E5CDA29F}"), 100);

				public static PropertyKey Resources => new PropertyKey(new Guid("{00F58A38-C54B-4C40-8696-97235980EAE1}"), 100);

				public static PropertyKey ResponseStatus => new PropertyKey(new Guid("{188C1F91-3C40-4132-9EC5-D8B03B72A8A2}"), 100);

				public static PropertyKey ShowTimeAs => new PropertyKey(new Guid("{5BF396D4-5EB2-466F-BDE9-2FB3F2361D6E}"), 100);

				public static PropertyKey ShowTimeAsText => new PropertyKey(new Guid("{53DA57CF-62C0-45C4-81DE-7610BCEFD7F5}"), 100);
			}

			public static class Communication
			{
				public static PropertyKey AccountName => new PropertyKey(new Guid("{E3E0584C-B788-4A5A-BB20-7F5A44C9ACDD}"), 9);

				public static PropertyKey DateItemExpires => new PropertyKey(new Guid("{428040AC-A177-4C8A-9760-F6F761227F9A}"), 100);

				public static PropertyKey FollowUpIconIndex => new PropertyKey(new Guid("{83A6347E-6FE4-4F40-BA9C-C4865240D1F4}"), 100);

				public static PropertyKey HeaderItem => new PropertyKey(new Guid("{C9C34F84-2241-4401-B607-BD20ED75AE7F}"), 100);

				public static PropertyKey PolicyTag => new PropertyKey(new Guid("{EC0B4191-AB0B-4C66-90B6-C6637CDEBBAB}"), 100);

				public static PropertyKey SecurityFlags => new PropertyKey(new Guid("{8619A4B6-9F4D-4429-8C0F-B996CA59E335}"), 100);

				public static PropertyKey Suffix => new PropertyKey(new Guid("{807B653A-9E91-43EF-8F97-11CE04EE20C5}"), 100);

				public static PropertyKey TaskStatus => new PropertyKey(new Guid("{BE1A72C6-9A1D-46B7-AFE7-AFAF8CEF4999}"), 100);

				public static PropertyKey TaskStatusText => new PropertyKey(new Guid("{A6744477-C237-475B-A075-54F34498292A}"), 100);
			}

			public static class Computer
			{
				public static PropertyKey DecoratedFreeSpace => new PropertyKey(new Guid("{9B174B35-40FF-11D2-A27E-00C04FC30871}"), 7);
			}

			public static class Contact
			{
				public static class JA
				{
					public static PropertyKey CompanyNamePhonetic => new PropertyKey(new Guid("{897B3694-FE9E-43E6-8066-260F590C0100}"), 2);

					public static PropertyKey FirstNamePhonetic => new PropertyKey(new Guid("{897B3694-FE9E-43E6-8066-260F590C0100}"), 3);

					public static PropertyKey LastNamePhonetic => new PropertyKey(new Guid("{897B3694-FE9E-43E6-8066-260F590C0100}"), 4);
				}

				public static PropertyKey Anniversary => new PropertyKey(new Guid("{9AD5BADB-CEA7-4470-A03D-B84E51B9949E}"), 100);

				public static PropertyKey AssistantName => new PropertyKey(new Guid("{CD102C9C-5540-4A88-A6F6-64E4981C8CD1}"), 100);

				public static PropertyKey AssistantTelephone => new PropertyKey(new Guid("{9A93244D-A7AD-4FF8-9B99-45EE4CC09AF6}"), 100);

				public static PropertyKey Birthday => new PropertyKey(new Guid("{176DC63C-2688-4E89-8143-A347800F25E9}"), 47);

				public static PropertyKey BusinessAddress => new PropertyKey(new Guid("{730FB6DD-CF7C-426B-A03F-BD166CC9EE24}"), 100);

				public static PropertyKey BusinessAddressCity => new PropertyKey(new Guid("{402B5934-EC5A-48C3-93E6-85E86A2D934E}"), 100);

				public static PropertyKey BusinessAddressCountry => new PropertyKey(new Guid("{B0B87314-FCF6-4FEB-8DFF-A50DA6AF561C}"), 100);

				public static PropertyKey BusinessAddressPostalCode => new PropertyKey(new Guid("{E1D4A09E-D758-4CD1-B6EC-34A8B5A73F80}"), 100);

				public static PropertyKey BusinessAddressPostOfficeBox => new PropertyKey(new Guid("{BC4E71CE-17F9-48D5-BEE9-021DF0EA5409}"), 100);

				public static PropertyKey BusinessAddressState => new PropertyKey(new Guid("{446F787F-10C4-41CB-A6C4-4D0343551597}"), 100);

				public static PropertyKey BusinessAddressStreet => new PropertyKey(new Guid("{DDD1460F-C0BF-4553-8CE4-10433C908FB0}"), 100);

				public static PropertyKey BusinessFaxNumber => new PropertyKey(new Guid("{91EFF6F3-2E27-42CA-933E-7C999FBE310B}"), 100);

				public static PropertyKey BusinessHomepage => new PropertyKey(new Guid("{56310920-2491-4919-99CE-EADB06FAFDB2}"), 100);

				public static PropertyKey BusinessTelephone => new PropertyKey(new Guid("{6A15E5A0-0A1E-4CD7-BB8C-D2F1B0C929BC}"), 100);

				public static PropertyKey CallbackTelephone => new PropertyKey(new Guid("{BF53D1C3-49E0-4F7F-8567-5A821D8AC542}"), 100);

				public static PropertyKey CarTelephone => new PropertyKey(new Guid("{8FDC6DEA-B929-412B-BA90-397A257465FE}"), 100);

				public static PropertyKey Children => new PropertyKey(new Guid("{D4729704-8EF1-43EF-9024-2BD381187FD5}"), 100);

				public static PropertyKey CompanyMainTelephone => new PropertyKey(new Guid("{8589E481-6040-473D-B171-7FA89C2708ED}"), 100);

				public static PropertyKey Department => new PropertyKey(new Guid("{FC9F7306-FF8F-4D49-9FB6-3FFE5C0951EC}"), 100);

				public static PropertyKey EmailAddress => new PropertyKey(new Guid("{F8FA7FA3-D12B-4785-8A4E-691A94F7A3E7}"), 100);

				public static PropertyKey EmailAddress2 => new PropertyKey(new Guid("{38965063-EDC8-4268-8491-B7723172CF29}"), 100);

				public static PropertyKey EmailAddress3 => new PropertyKey(new Guid("{644D37B4-E1B3-4BAD-B099-7E7C04966ACA}"), 100);

				public static PropertyKey EmailAddresses => new PropertyKey(new Guid("{84D8F337-981D-44B3-9615-C7596DBA17E3}"), 100);

				public static PropertyKey EmailName => new PropertyKey(new Guid("{CC6F4F24-6083-4BD4-8754-674D0DE87AB8}"), 100);

				public static PropertyKey FileAsName => new PropertyKey(new Guid("{F1A24AA7-9CA7-40F6-89EC-97DEF9FFE8DB}"), 100);

				public static PropertyKey FirstName => new PropertyKey(new Guid("{14977844-6B49-4AAD-A714-A4513BF60460}"), 100);

				public static PropertyKey FullName => new PropertyKey(new Guid("{635E9051-50A5-4BA2-B9DB-4ED056C77296}"), 100);

				public static PropertyKey Gender => new PropertyKey(new Guid("{3C8CEE58-D4F0-4CF9-B756-4E5D24447BCD}"), 100);

				public static PropertyKey GenderValue => new PropertyKey(new Guid("{3C8CEE58-D4F0-4CF9-B756-4E5D24447BCD}"), 101);

				public static PropertyKey Hobbies => new PropertyKey(new Guid("{5DC2253F-5E11-4ADF-9CFE-910DD01E3E70}"), 100);

				public static PropertyKey HomeAddress => new PropertyKey(new Guid("{98F98354-617A-46B8-8560-5B1B64BF1F89}"), 100);

				public static PropertyKey HomeAddressCity => new PropertyKey(new Guid("{176DC63C-2688-4E89-8143-A347800F25E9}"), 65);

				public static PropertyKey HomeAddressCountry => new PropertyKey(new Guid("{08A65AA1-F4C9-43DD-9DDF-A33D8E7EAD85}"), 100);

				public static PropertyKey HomeAddressPostalCode => new PropertyKey(new Guid("{8AFCC170-8A46-4B53-9EEE-90BAE7151E62}"), 100);

				public static PropertyKey HomeAddressPostOfficeBox => new PropertyKey(new Guid("{7B9F6399-0A3F-4B12-89BD-4ADC51C918AF}"), 100);

				public static PropertyKey HomeAddressState => new PropertyKey(new Guid("{C89A23D0-7D6D-4EB8-87D4-776A82D493E5}"), 100);

				public static PropertyKey HomeAddressStreet => new PropertyKey(new Guid("{0ADEF160-DB3F-4308-9A21-06237B16FA2A}"), 100);

				public static PropertyKey HomeFaxNumber => new PropertyKey(new Guid("{660E04D6-81AB-4977-A09F-82313113AB26}"), 100);

				public static PropertyKey HomeTelephone => new PropertyKey(new Guid("{176DC63C-2688-4E89-8143-A347800F25E9}"), 20);

				public static PropertyKey IMAddress => new PropertyKey(new Guid("{D68DBD8A-3374-4B81-9972-3EC30682DB3D}"), 100);

				public static PropertyKey Initials => new PropertyKey(new Guid("{F3D8F40D-50CB-44A2-9718-40CB9119495D}"), 100);

				public static PropertyKey JobTitle => new PropertyKey(new Guid("{176DC63C-2688-4E89-8143-A347800F25E9}"), 6);

				public static PropertyKey Label => new PropertyKey(new Guid("{97B0AD89-DF49-49CC-834E-660974FD755B}"), 100);

				public static PropertyKey LastName => new PropertyKey(new Guid("{8F367200-C270-457C-B1D4-E07C5BCD90C7}"), 100);

				public static PropertyKey MailingAddress => new PropertyKey(new Guid("{C0AC206A-827E-4650-95AE-77E2BB74FCC9}"), 100);

				public static PropertyKey MiddleName => new PropertyKey(new Guid("{176DC63C-2688-4E89-8143-A347800F25E9}"), 71);

				public static PropertyKey MobileTelephone => new PropertyKey(new Guid("{176DC63C-2688-4E89-8143-A347800F25E9}"), 35);

				public static PropertyKey Nickname => new PropertyKey(new Guid("{176DC63C-2688-4E89-8143-A347800F25E9}"), 74);

				public static PropertyKey OfficeLocation => new PropertyKey(new Guid("{176DC63C-2688-4E89-8143-A347800F25E9}"), 7);

				public static PropertyKey OtherAddress => new PropertyKey(new Guid("{508161FA-313B-43D5-83A1-C1ACCF68622C}"), 100);

				public static PropertyKey OtherAddressCity => new PropertyKey(new Guid("{6E682923-7F7B-4F0C-A337-CFCA296687BF}"), 100);

				public static PropertyKey OtherAddressCountry => new PropertyKey(new Guid("{8F167568-0AAE-4322-8ED9-6055B7B0E398}"), 100);

				public static PropertyKey OtherAddressPostalCode => new PropertyKey(new Guid("{95C656C1-2ABF-4148-9ED3-9EC602E3B7CD}"), 100);

				public static PropertyKey OtherAddressPostOfficeBox => new PropertyKey(new Guid("{8B26EA41-058F-43F6-AECC-4035681CE977}"), 100);

				public static PropertyKey OtherAddressState => new PropertyKey(new Guid("{71B377D6-E570-425F-A170-809FAE73E54E}"), 100);

				public static PropertyKey OtherAddressStreet => new PropertyKey(new Guid("{FF962609-B7D6-4999-862D-95180D529AEA}"), 100);

				public static PropertyKey PagerTelephone => new PropertyKey(new Guid("{D6304E01-F8F5-4F45-8B15-D024A6296789}"), 100);

				public static PropertyKey PersonalTitle => new PropertyKey(new Guid("{176DC63C-2688-4E89-8143-A347800F25E9}"), 69);

				public static PropertyKey PrimaryAddressCity => new PropertyKey(new Guid("{C8EA94F0-A9E3-4969-A94B-9C62A95324E0}"), 100);

				public static PropertyKey PrimaryAddressCountry => new PropertyKey(new Guid("{E53D799D-0F3F-466E-B2FF-74634A3CB7A4}"), 100);

				public static PropertyKey PrimaryAddressPostalCode => new PropertyKey(new Guid("{18BBD425-ECFD-46EF-B612-7B4A6034EDA0}"), 100);

				public static PropertyKey PrimaryAddressPostOfficeBox => new PropertyKey(new Guid("{DE5EF3C7-46E1-484E-9999-62C5308394C1}"), 100);

				public static PropertyKey PrimaryAddressState => new PropertyKey(new Guid("{F1176DFE-7138-4640-8B4C-AE375DC70A6D}"), 100);

				public static PropertyKey PrimaryAddressStreet => new PropertyKey(new Guid("{63C25B20-96BE-488F-8788-C09C407AD812}"), 100);

				public static PropertyKey PrimaryEmailAddress => new PropertyKey(new Guid("{176DC63C-2688-4E89-8143-A347800F25E9}"), 48);

				public static PropertyKey PrimaryTelephone => new PropertyKey(new Guid("{176DC63C-2688-4E89-8143-A347800F25E9}"), 25);

				public static PropertyKey Profession => new PropertyKey(new Guid("{7268AF55-1CE4-4F6E-A41F-B6E4EF10E4A9}"), 100);

				public static PropertyKey SpouseName => new PropertyKey(new Guid("{9D2408B6-3167-422B-82B0-F583B7A7CFE3}"), 100);

				public static PropertyKey Suffix => new PropertyKey(new Guid("{176DC63C-2688-4E89-8143-A347800F25E9}"), 73);

				public static PropertyKey TelexNumber => new PropertyKey(new Guid("{C554493C-C1F7-40C1-A76C-EF8C0614003E}"), 100);

				public static PropertyKey TTYTDDTelephone => new PropertyKey(new Guid("{AAF16BAC-2B55-45E6-9F6D-415EB94910DF}"), 100);

				public static PropertyKey Webpage => new PropertyKey(new Guid("{E3E0584C-B788-4A5A-BB20-7F5A44C9ACDD}"), 18);
			}

			public static class JA
			{
				public static PropertyKey CompanyNamePhonetic => new PropertyKey(new Guid("{897B3694-FE9E-43E6-8066-260F590C0100}"), 2);

				public static PropertyKey FirstNamePhonetic => new PropertyKey(new Guid("{897B3694-FE9E-43E6-8066-260F590C0100}"), 3);

				public static PropertyKey LastNamePhonetic => new PropertyKey(new Guid("{897B3694-FE9E-43E6-8066-260F590C0100}"), 4);
			}

			public static class Device
			{
				public static PropertyKey PrinterUrl => new PropertyKey(new Guid("{0B48F35A-BE6E-4F17-B108-3C4073D1669A}"), 15);
			}

			public static class DeviceInterface
			{
				public static PropertyKey PrinterDriverDirectory => new PropertyKey(new Guid("{847C66DE-B8D6-4AF9-ABC3-6F4F926BC039}"), 14);

				public static PropertyKey PrinterDriverName => new PropertyKey(new Guid("{AFC47170-14F5-498C-8F30-B0D19BE449C6}"), 11);

				public static PropertyKey PrinterName => new PropertyKey(new Guid("{0A7B84EF-0C27-463F-84EF-06C5070001BE}"), 10);

				public static PropertyKey PrinterPortName => new PropertyKey(new Guid("{EEC7B761-6F94-41B1-949F-C729720DD13C}"), 12);
			}

			public static class Devices
			{
				public static class Notifications
				{
					public static PropertyKey LowBattery => new PropertyKey(new Guid("{C4C07F2B-8524-4E66-AE3A-A6235F103BEB}"), 2);

					public static PropertyKey MissedCall => new PropertyKey(new Guid("{6614EF48-4EFE-4424-9EDA-C79F404EDF3E}"), 2);

					public static PropertyKey NewMessage => new PropertyKey(new Guid("{2BE9260A-2012-4742-A555-F41B638B7DCB}"), 2);

					public static PropertyKey NewVoicemail => new PropertyKey(new Guid("{59569556-0A08-4212-95B9-FAE2AD6413DB}"), 2);

					public static PropertyKey StorageFull => new PropertyKey(new Guid("{A0E00EE1-F0C7-4D41-B8E7-26A7BD8D38B0}"), 2);

					public static PropertyKey StorageFullLinkText => new PropertyKey(new Guid("{A0E00EE1-F0C7-4D41-B8E7-26A7BD8D38B0}"), 3);
				}

				public static PropertyKey BatteryLife => new PropertyKey(new Guid("{49CD1F76-5626-4B17-A4E8-18B4AA1A2213}"), 10);

				public static PropertyKey BatteryPlusCharging => new PropertyKey(new Guid("{49CD1F76-5626-4B17-A4E8-18B4AA1A2213}"), 22);

				public static PropertyKey BatteryPlusChargingText => new PropertyKey(new Guid("{49CD1F76-5626-4B17-A4E8-18B4AA1A2213}"), 23);

				public static PropertyKey Category => new PropertyKey(new Guid("{78C34FC8-104A-4ACA-9EA4-524D52996E57}"), 91);

				public static PropertyKey CategoryGroup => new PropertyKey(new Guid("{78C34FC8-104A-4ACA-9EA4-524D52996E57}"), 94);

				public static PropertyKey CategoryPlural => new PropertyKey(new Guid("{78C34FC8-104A-4ACA-9EA4-524D52996E57}"), 92);

				public static PropertyKey ChargingState => new PropertyKey(new Guid("{49CD1F76-5626-4B17-A4E8-18B4AA1A2213}"), 11);

				public static PropertyKey Connected => new PropertyKey(new Guid("{78C34FC8-104A-4ACA-9EA4-524D52996E57}"), 55);

				public static PropertyKey ContainerId => new PropertyKey(new Guid("{8C7ED206-3F8A-4827-B3AB-AE9E1FAEFC6C}"), 2);

				public static PropertyKey DefaultTooltip => new PropertyKey(new Guid("{880F70A2-6082-47AC-8AAB-A739D1A300C3}"), 153);

				public static PropertyKey DeviceDescription1 => new PropertyKey(new Guid("{78C34FC8-104A-4ACA-9EA4-524D52996E57}"), 81);

				public static PropertyKey DeviceDescription2 => new PropertyKey(new Guid("{78C34FC8-104A-4ACA-9EA4-524D52996E57}"), 82);

				public static PropertyKey DiscoveryMethod => new PropertyKey(new Guid("{78C34FC8-104A-4ACA-9EA4-524D52996E57}"), 52);

				public static PropertyKey FriendlyName => new PropertyKey(new Guid("{656A3BB3-ECC0-43FD-8477-4AE0404A96CD}"), 12288);

				public static PropertyKey FunctionPaths => new PropertyKey(new Guid("{D08DD4C0-3A9E-462E-8290-7B636B2576B9}"), 3);

				public static PropertyKey InterfacePaths => new PropertyKey(new Guid("{D08DD4C0-3A9E-462E-8290-7B636B2576B9}"), 2);

				public static PropertyKey IsDefault => new PropertyKey(new Guid("{78C34FC8-104A-4ACA-9EA4-524D52996E57}"), 86);

				public static PropertyKey IsNetworkConnected => new PropertyKey(new Guid("{78C34FC8-104A-4ACA-9EA4-524D52996E57}"), 85);

				public static PropertyKey IsShared => new PropertyKey(new Guid("{78C34FC8-104A-4ACA-9EA4-524D52996E57}"), 84);

				public static PropertyKey IsSoftwareInstalling => new PropertyKey(new Guid("{83DA6326-97A6-4088-9453-A1923F573B29}"), 9);

				public static PropertyKey LaunchDeviceStageFromExplorer => new PropertyKey(new Guid("{78C34FC8-104A-4ACA-9EA4-524D52996E57}"), 77);

				public static PropertyKey LocalMachine => new PropertyKey(new Guid("{78C34FC8-104A-4ACA-9EA4-524D52996E57}"), 70);

				public static PropertyKey Manufacturer => new PropertyKey(new Guid("{656A3BB3-ECC0-43FD-8477-4AE0404A96CD}"), 8192);

				public static PropertyKey MissedCalls => new PropertyKey(new Guid("{49CD1F76-5626-4B17-A4E8-18B4AA1A2213}"), 5);

				public static PropertyKey ModelName => new PropertyKey(new Guid("{656A3BB3-ECC0-43FD-8477-4AE0404A96CD}"), 8194);

				public static PropertyKey ModelNumber => new PropertyKey(new Guid("{656A3BB3-ECC0-43FD-8477-4AE0404A96CD}"), 8195);

				public static PropertyKey NetworkedTooltip => new PropertyKey(new Guid("{880F70A2-6082-47AC-8AAB-A739D1A300C3}"), 152);

				public static PropertyKey NetworkName => new PropertyKey(new Guid("{49CD1F76-5626-4B17-A4E8-18B4AA1A2213}"), 7);

				public static PropertyKey NetworkType => new PropertyKey(new Guid("{49CD1F76-5626-4B17-A4E8-18B4AA1A2213}"), 8);

				public static PropertyKey NewPictures => new PropertyKey(new Guid("{49CD1F76-5626-4B17-A4E8-18B4AA1A2213}"), 4);

				public static PropertyKey Notification => new PropertyKey(new Guid("{06704B0C-E830-4C81-9178-91E4E95A80A0}"), 3);

				public static PropertyKey NotificationStore => new PropertyKey(new Guid("{06704B0C-E830-4C81-9178-91E4E95A80A0}"), 2);

				public static PropertyKey NotWorkingProperly => new PropertyKey(new Guid("{78C34FC8-104A-4ACA-9EA4-524D52996E57}"), 83);

				public static PropertyKey Paired => new PropertyKey(new Guid("{78C34FC8-104A-4ACA-9EA4-524D52996E57}"), 56);

				public static PropertyKey PrimaryCategory => new PropertyKey(new Guid("{D08DD4C0-3A9E-462E-8290-7B636B2576B9}"), 10);

				public static PropertyKey Roaming => new PropertyKey(new Guid("{49CD1F76-5626-4B17-A4E8-18B4AA1A2213}"), 9);

				public static PropertyKey SafeRemovalRequired => new PropertyKey(new Guid("{AFD97640-86A3-4210-B67C-289C41AABE55}"), 2);

				public static PropertyKey SharedTooltip => new PropertyKey(new Guid("{880F70A2-6082-47AC-8AAB-A739D1A300C3}"), 151);

				public static PropertyKey SignalStrength => new PropertyKey(new Guid("{49CD1F76-5626-4B17-A4E8-18B4AA1A2213}"), 2);

				public static PropertyKey Status1 => new PropertyKey(new Guid("{D08DD4C0-3A9E-462E-8290-7B636B2576B9}"), 257);

				public static PropertyKey Status2 => new PropertyKey(new Guid("{D08DD4C0-3A9E-462E-8290-7B636B2576B9}"), 258);

				public static PropertyKey StorageCapacity => new PropertyKey(new Guid("{49CD1F76-5626-4B17-A4E8-18B4AA1A2213}"), 12);

				public static PropertyKey StorageFreeSpace => new PropertyKey(new Guid("{49CD1F76-5626-4B17-A4E8-18B4AA1A2213}"), 13);

				public static PropertyKey StorageFreeSpacePercent => new PropertyKey(new Guid("{49CD1F76-5626-4B17-A4E8-18B4AA1A2213}"), 14);

				public static PropertyKey TextMessages => new PropertyKey(new Guid("{49CD1F76-5626-4B17-A4E8-18B4AA1A2213}"), 3);

				public static PropertyKey Voicemail => new PropertyKey(new Guid("{49CD1F76-5626-4B17-A4E8-18B4AA1A2213}"), 6);
			}

			public static class Notifications
			{
				public static PropertyKey LowBattery => new PropertyKey(new Guid("{C4C07F2B-8524-4E66-AE3A-A6235F103BEB}"), 2);

				public static PropertyKey MissedCall => new PropertyKey(new Guid("{6614EF48-4EFE-4424-9EDA-C79F404EDF3E}"), 2);

				public static PropertyKey NewMessage => new PropertyKey(new Guid("{2BE9260A-2012-4742-A555-F41B638B7DCB}"), 2);

				public static PropertyKey NewVoicemail => new PropertyKey(new Guid("{59569556-0A08-4212-95B9-FAE2AD6413DB}"), 2);

				public static PropertyKey StorageFull => new PropertyKey(new Guid("{A0E00EE1-F0C7-4D41-B8E7-26A7BD8D38B0}"), 2);

				public static PropertyKey StorageFullLinkText => new PropertyKey(new Guid("{A0E00EE1-F0C7-4D41-B8E7-26A7BD8D38B0}"), 3);
			}

			public static class Document
			{
				public static PropertyKey ByteCount => new PropertyKey(new Guid("{D5CDD502-2E9C-101B-9397-08002B2CF9AE}"), 4);

				public static PropertyKey CharacterCount => new PropertyKey(new Guid("{F29F85E0-4FF9-1068-AB91-08002B27B3D9}"), 16);

				public static PropertyKey ClientID => new PropertyKey(new Guid("{276D7BB0-5B34-4FB0-AA4B-158ED12A1809}"), 100);

				public static PropertyKey Contributor => new PropertyKey(new Guid("{F334115E-DA1B-4509-9B3D-119504DC7ABB}"), 100);

				public static PropertyKey DateCreated => new PropertyKey(new Guid("{F29F85E0-4FF9-1068-AB91-08002B27B3D9}"), 12);

				public static PropertyKey DatePrinted => new PropertyKey(new Guid("{F29F85E0-4FF9-1068-AB91-08002B27B3D9}"), 11);

				public static PropertyKey DateSaved => new PropertyKey(new Guid("{F29F85E0-4FF9-1068-AB91-08002B27B3D9}"), 13);

				public static PropertyKey Division => new PropertyKey(new Guid("{1E005EE6-BF27-428B-B01C-79676ACD2870}"), 100);

				public static PropertyKey DocumentID => new PropertyKey(new Guid("{E08805C8-E395-40DF-80D2-54F0D6C43154}"), 100);

				public static PropertyKey HiddenSlideCount => new PropertyKey(new Guid("{D5CDD502-2E9C-101B-9397-08002B2CF9AE}"), 9);

				public static PropertyKey LastAuthor => new PropertyKey(new Guid("{F29F85E0-4FF9-1068-AB91-08002B27B3D9}"), 8);

				public static PropertyKey LineCount => new PropertyKey(new Guid("{D5CDD502-2E9C-101B-9397-08002B2CF9AE}"), 5);

				public static PropertyKey Manager => new PropertyKey(new Guid("{D5CDD502-2E9C-101B-9397-08002B2CF9AE}"), 14);

				public static PropertyKey MultimediaClipCount => new PropertyKey(new Guid("{D5CDD502-2E9C-101B-9397-08002B2CF9AE}"), 10);

				public static PropertyKey NoteCount => new PropertyKey(new Guid("{D5CDD502-2E9C-101B-9397-08002B2CF9AE}"), 8);

				public static PropertyKey PageCount => new PropertyKey(new Guid("{F29F85E0-4FF9-1068-AB91-08002B27B3D9}"), 14);

				public static PropertyKey ParagraphCount => new PropertyKey(new Guid("{D5CDD502-2E9C-101B-9397-08002B2CF9AE}"), 6);

				public static PropertyKey PresentationFormat => new PropertyKey(new Guid("{D5CDD502-2E9C-101B-9397-08002B2CF9AE}"), 3);

				public static PropertyKey RevisionNumber => new PropertyKey(new Guid("{F29F85E0-4FF9-1068-AB91-08002B27B3D9}"), 9);

				public static PropertyKey Security => new PropertyKey(new Guid("{F29F85E0-4FF9-1068-AB91-08002B27B3D9}"), 19);

				public static PropertyKey SlideCount => new PropertyKey(new Guid("{D5CDD502-2E9C-101B-9397-08002B2CF9AE}"), 7);

				public static PropertyKey Template => new PropertyKey(new Guid("{F29F85E0-4FF9-1068-AB91-08002B27B3D9}"), 7);

				public static PropertyKey TotalEditingTime => new PropertyKey(new Guid("{F29F85E0-4FF9-1068-AB91-08002B27B3D9}"), 10);

				public static PropertyKey Version => new PropertyKey(new Guid("{D5CDD502-2E9C-101B-9397-08002B2CF9AE}"), 29);

				public static PropertyKey WordCount => new PropertyKey(new Guid("{F29F85E0-4FF9-1068-AB91-08002B27B3D9}"), 15);
			}

			public static class DRM
			{
				public static PropertyKey DatePlayExpires => new PropertyKey(new Guid("{AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED}"), 6);

				public static PropertyKey DatePlayStarts => new PropertyKey(new Guid("{AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED}"), 5);

				public static PropertyKey Description => new PropertyKey(new Guid("{AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED}"), 3);

				public static PropertyKey IsProtected => new PropertyKey(new Guid("{AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED}"), 2);

				public static PropertyKey PlayCount => new PropertyKey(new Guid("{AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED}"), 4);
			}

			public static class GPS
			{
				public static PropertyKey Altitude => new PropertyKey(new Guid("{827EDB4F-5B73-44A7-891D-FDFFABEA35CA}"), 100);

				public static PropertyKey AltitudeDenominator => new PropertyKey(new Guid("{78342DCB-E358-4145-AE9A-6BFE4E0F9F51}"), 100);

				public static PropertyKey AltitudeNumerator => new PropertyKey(new Guid("{2DAD1EB7-816D-40D3-9EC3-C9773BE2AADE}"), 100);

				public static PropertyKey AltitudeRef => new PropertyKey(new Guid("{46AC629D-75EA-4515-867F-6DC4321C5844}"), 100);

				public static PropertyKey AreaInformation => new PropertyKey(new Guid("{972E333E-AC7E-49F1-8ADF-A70D07A9BCAB}"), 100);

				public static PropertyKey Date => new PropertyKey(new Guid("{3602C812-0F3B-45F0-85AD-603468D69423}"), 100);

				public static PropertyKey DestinationBearing => new PropertyKey(new Guid("{C66D4B3C-E888-47CC-B99F-9DCA3EE34DEA}"), 100);

				public static PropertyKey DestinationBearingDenominator => new PropertyKey(new Guid("{7ABCF4F8-7C3F-4988-AC91-8D2C2E97ECA5}"), 100);

				public static PropertyKey DestinationBearingNumerator => new PropertyKey(new Guid("{BA3B1DA9-86EE-4B5D-A2A4-A271A429F0CF}"), 100);

				public static PropertyKey DestinationBearingRef => new PropertyKey(new Guid("{9AB84393-2A0F-4B75-BB22-7279786977CB}"), 100);

				public static PropertyKey DestinationDistance => new PropertyKey(new Guid("{A93EAE04-6804-4F24-AC81-09B266452118}"), 100);

				public static PropertyKey DestinationDistanceDenominator => new PropertyKey(new Guid("{9BC2C99B-AC71-4127-9D1C-2596D0D7DCB7}"), 100);

				public static PropertyKey DestinationDistanceNumerator => new PropertyKey(new Guid("{2BDA47DA-08C6-4FE1-80BC-A72FC517C5D0}"), 100);

				public static PropertyKey DestinationDistanceRef => new PropertyKey(new Guid("{ED4DF2D3-8695-450B-856F-F5C1C53ACB66}"), 100);

				public static PropertyKey DestinationLatitude => new PropertyKey(new Guid("{9D1D7CC5-5C39-451C-86B3-928E2D18CC47}"), 100);

				public static PropertyKey DestinationLatitudeDenominator => new PropertyKey(new Guid("{3A372292-7FCA-49A7-99D5-E47BB2D4E7AB}"), 100);

				public static PropertyKey DestinationLatitudeNumerator => new PropertyKey(new Guid("{ECF4B6F6-D5A6-433C-BB92-4076650FC890}"), 100);

				public static PropertyKey DestinationLatitudeRef => new PropertyKey(new Guid("{CEA820B9-CE61-4885-A128-005D9087C192}"), 100);

				public static PropertyKey DestinationLongitude => new PropertyKey(new Guid("{47A96261-CB4C-4807-8AD3-40B9D9DBC6BC}"), 100);

				public static PropertyKey DestinationLongitudeDenominator => new PropertyKey(new Guid("{425D69E5-48AD-4900-8D80-6EB6B8D0AC86}"), 100);

				public static PropertyKey DestinationLongitudeNumerator => new PropertyKey(new Guid("{A3250282-FB6D-48D5-9A89-DBCACE75CCCF}"), 100);

				public static PropertyKey DestinationLongitudeRef => new PropertyKey(new Guid("{182C1EA6-7C1C-4083-AB4B-AC6C9F4ED128}"), 100);

				public static PropertyKey Differential => new PropertyKey(new Guid("{AAF4EE25-BD3B-4DD7-BFC4-47F77BB00F6D}"), 100);

				public static PropertyKey DOP => new PropertyKey(new Guid("{0CF8FB02-1837-42F1-A697-A7017AA289B9}"), 100);

				public static PropertyKey DOPDenominator => new PropertyKey(new Guid("{A0BE94C5-50BA-487B-BD35-0654BE8881ED}"), 100);

				public static PropertyKey DOPNumerator => new PropertyKey(new Guid("{47166B16-364F-4AA0-9F31-E2AB3DF449C3}"), 100);

				public static PropertyKey ImageDirection => new PropertyKey(new Guid("{16473C91-D017-4ED9-BA4D-B6BAA55DBCF8}"), 100);

				public static PropertyKey ImageDirectionDenominator => new PropertyKey(new Guid("{10B24595-41A2-4E20-93C2-5761C1395F32}"), 100);

				public static PropertyKey ImageDirectionNumerator => new PropertyKey(new Guid("{DC5877C7-225F-45F7-BAC7-E81334B6130A}"), 100);

				public static PropertyKey ImageDirectionRef => new PropertyKey(new Guid("{A4AAA5B7-1AD0-445F-811A-0F8F6E67F6B5}"), 100);

				public static PropertyKey Latitude => new PropertyKey(new Guid("{8727CFFF-4868-4EC6-AD5B-81B98521D1AB}"), 100);

				public static PropertyKey LatitudeDenominator => new PropertyKey(new Guid("{16E634EE-2BFF-497B-BD8A-4341AD39EEB9}"), 100);

				public static PropertyKey LatitudeNumerator => new PropertyKey(new Guid("{7DDAAAD1-CCC8-41AE-B750-B2CB8031AEA2}"), 100);

				public static PropertyKey LatitudeRef => new PropertyKey(new Guid("{029C0252-5B86-46C7-ACA0-2769FFC8E3D4}"), 100);

				public static PropertyKey Longitude => new PropertyKey(new Guid("{C4C4DBB2-B593-466B-BBDA-D03D27D5E43A}"), 100);

				public static PropertyKey LongitudeDenominator => new PropertyKey(new Guid("{BE6E176C-4534-4D2C-ACE5-31DEDAC1606B}"), 100);

				public static PropertyKey LongitudeNumerator => new PropertyKey(new Guid("{02B0F689-A914-4E45-821D-1DDA452ED2C4}"), 100);

				public static PropertyKey LongitudeRef => new PropertyKey(new Guid("{33DCF22B-28D5-464C-8035-1EE9EFD25278}"), 100);

				public static PropertyKey MapDatum => new PropertyKey(new Guid("{2CA2DAE6-EDDC-407D-BEF1-773942ABFA95}"), 100);

				public static PropertyKey MeasureMode => new PropertyKey(new Guid("{A015ED5D-AAEA-4D58-8A86-3C586920EA0B}"), 100);

				public static PropertyKey ProcessingMethod => new PropertyKey(new Guid("{59D49E61-840F-4AA9-A939-E2099B7F6399}"), 100);

				public static PropertyKey Satellites => new PropertyKey(new Guid("{467EE575-1F25-4557-AD4E-B8B58B0D9C15}"), 100);

				public static PropertyKey Speed => new PropertyKey(new Guid("{DA5D0862-6E76-4E1B-BABD-70021BD25494}"), 100);

				public static PropertyKey SpeedDenominator => new PropertyKey(new Guid("{7D122D5A-AE5E-4335-8841-D71E7CE72F53}"), 100);

				public static PropertyKey SpeedNumerator => new PropertyKey(new Guid("{ACC9CE3D-C213-4942-8B48-6D0820F21C6D}"), 100);

				public static PropertyKey SpeedRef => new PropertyKey(new Guid("{ECF7F4C9-544F-4D6D-9D98-8AD79ADAF453}"), 100);

				public static PropertyKey Status => new PropertyKey(new Guid("{125491F4-818F-46B2-91B5-D537753617B2}"), 100);

				public static PropertyKey Track => new PropertyKey(new Guid("{76C09943-7C33-49E3-9E7E-CDBA872CFADA}"), 100);

				public static PropertyKey TrackDenominator => new PropertyKey(new Guid("{C8D1920C-01F6-40C0-AC86-2F3A4AD00770}"), 100);

				public static PropertyKey TrackNumerator => new PropertyKey(new Guid("{702926F4-44A6-43E1-AE71-45627116893B}"), 100);

				public static PropertyKey TrackRef => new PropertyKey(new Guid("{35DBE6FE-44C3-4400-AAAE-D2C799C407E8}"), 100);

				public static PropertyKey VersionID => new PropertyKey(new Guid("{22704DA4-C6B2-4A99-8E56-F16DF8C92599}"), 100);
			}

			public static class Identity
			{
				public static PropertyKey Blob => new PropertyKey(new Guid("{8C3B93A4-BAED-1A83-9A32-102EE313F6EB}"), 100);

				public static PropertyKey DisplayName => new PropertyKey(new Guid("{7D683FC9-D155-45A8-BB1F-89D19BCB792F}"), 100);

				public static PropertyKey IsMeIdentity => new PropertyKey(new Guid("{A4108708-09DF-4377-9DFC-6D99986D5A67}"), 100);

				public static PropertyKey PrimaryEmailAddress => new PropertyKey(new Guid("{FCC16823-BAED-4F24-9B32-A0982117F7FA}"), 100);

				public static PropertyKey ProviderID => new PropertyKey(new Guid("{74A7DE49-FA11-4D3D-A006-DB7E08675916}"), 100);

				public static PropertyKey UniqueID => new PropertyKey(new Guid("{E55FC3B0-2B60-4220-918E-B21E8BF16016}"), 100);

				public static PropertyKey UserName => new PropertyKey(new Guid("{C4322503-78CA-49C6-9ACC-A68E2AFD7B6B}"), 100);
			}

			public static class IdentityProvider
			{
				public static PropertyKey Name => new PropertyKey(new Guid("{B96EFF7B-35CA-4A35-8607-29E3A54C46EA}"), 100);

				public static PropertyKey Picture => new PropertyKey(new Guid("{2425166F-5642-4864-992F-98FD98F294C3}"), 100);
			}

			public static class Image
			{
				public static PropertyKey BitDepth => new PropertyKey(new Guid("{6444048F-4C8B-11D1-8B70-080036B11A03}"), 7);

				public static PropertyKey ColorSpace => new PropertyKey(new Guid("{14B81DA1-0135-4D31-96D9-6CBFC9671A99}"), 40961);

				public static PropertyKey CompressedBitsPerPixel => new PropertyKey(new Guid("{364B6FA9-37AB-482A-BE2B-AE02F60D4318}"), 100);

				public static PropertyKey CompressedBitsPerPixelDenominator => new PropertyKey(new Guid("{1F8844E1-24AD-4508-9DFD-5326A415CE02}"), 100);

				public static PropertyKey CompressedBitsPerPixelNumerator => new PropertyKey(new Guid("{D21A7148-D32C-4624-8900-277210F79C0F}"), 100);

				public static PropertyKey Compression => new PropertyKey(new Guid("{14B81DA1-0135-4D31-96D9-6CBFC9671A99}"), 259);

				public static PropertyKey CompressionText => new PropertyKey(new Guid("{3F08E66F-2F44-4BB9-A682-AC35D2562322}"), 100);

				public static PropertyKey Dimensions => new PropertyKey(new Guid("{6444048F-4C8B-11D1-8B70-080036B11A03}"), 13);

				public static PropertyKey HorizontalResolution => new PropertyKey(new Guid("{6444048F-4C8B-11D1-8B70-080036B11A03}"), 5);

				public static PropertyKey HorizontalSize => new PropertyKey(new Guid("{6444048F-4C8B-11D1-8B70-080036B11A03}"), 3);

				public static PropertyKey ImageID => new PropertyKey(new Guid("{10DABE05-32AA-4C29-BF1A-63E2D220587F}"), 100);

				public static PropertyKey ResolutionUnit => new PropertyKey(new Guid("{19B51FA6-1F92-4A5C-AB48-7DF0ABD67444}"), 100);

				public static PropertyKey VerticalResolution => new PropertyKey(new Guid("{6444048F-4C8B-11D1-8B70-080036B11A03}"), 6);

				public static PropertyKey VerticalSize => new PropertyKey(new Guid("{6444048F-4C8B-11D1-8B70-080036B11A03}"), 4);
			}

			public static class Journal
			{
				public static PropertyKey Contacts => new PropertyKey(new Guid("{DEA7C82C-1D89-4A66-9427-A4E3DEBABCB1}"), 100);

				public static PropertyKey EntryType => new PropertyKey(new Guid("{95BEB1FC-326D-4644-B396-CD3ED90E6DDF}"), 100);
			}

			public static class LayoutPattern
			{
				public static PropertyKey ContentViewModeForBrowse => new PropertyKey(new Guid("{C9944A21-A406-48FE-8225-AEC7E24C211B}"), 500);

				public static PropertyKey ContentViewModeForSearch => new PropertyKey(new Guid("{C9944A21-A406-48FE-8225-AEC7E24C211B}"), 501);
			}

			public static class Link
			{
				public static PropertyKey Arguments => new PropertyKey(new Guid("{436F2667-14E2-4FEB-B30A-146C53B5B674}"), 100);

				public static PropertyKey Comment => new PropertyKey(new Guid("{B9B4B3FC-2B51-4A42-B5D8-324146AFCF25}"), 5);

				public static PropertyKey DateVisited => new PropertyKey(new Guid("{5CBF2787-48CF-4208-B90E-EE5E5D420294}"), 23);

				public static PropertyKey Description => new PropertyKey(new Guid("{5CBF2787-48CF-4208-B90E-EE5E5D420294}"), 21);

				public static PropertyKey Status => new PropertyKey(new Guid("{B9B4B3FC-2B51-4A42-B5D8-324146AFCF25}"), 3);

				public static PropertyKey TargetExtension => new PropertyKey(new Guid("{7A7D76F4-B630-4BD7-95FF-37CC51A975C9}"), 2);

				public static PropertyKey TargetParsingPath => new PropertyKey(new Guid("{B9B4B3FC-2B51-4A42-B5D8-324146AFCF25}"), 2);

				public static PropertyKey TargetSFGAOFlags => new PropertyKey(new Guid("{B9B4B3FC-2B51-4A42-B5D8-324146AFCF25}"), 8);

				public static PropertyKey TargetSFGAOFlagsStrings => new PropertyKey(new Guid("{D6942081-D53B-443D-AD47-5E059D9CD27A}"), 3);

				public static PropertyKey TargetUrl => new PropertyKey(new Guid("{5CBF2787-48CF-4208-B90E-EE5E5D420294}"), 2);
			}

			public static class Media
			{
				public static PropertyKey AuthorUrl => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 32);

				public static PropertyKey AverageLevel => new PropertyKey(new Guid("{09EDD5B6-B301-43C5-9990-D00302EFFD46}"), 100);

				public static PropertyKey ClassPrimaryID => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 13);

				public static PropertyKey ClassSecondaryID => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 14);

				public static PropertyKey CollectionGroupID => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 24);

				public static PropertyKey CollectionID => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 25);

				public static PropertyKey ContentDistributor => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 18);

				public static PropertyKey ContentID => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 26);

				public static PropertyKey CreatorApplication => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 27);

				public static PropertyKey CreatorApplicationVersion => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 28);

				public static PropertyKey DateEncoded => new PropertyKey(new Guid("{2E4B640D-5019-46D8-8881-55414CC5CAA0}"), 100);

				public static PropertyKey DateReleased => new PropertyKey(new Guid("{DE41CC29-6971-4290-B472-F59F2E2F31E2}"), 100);

				public static PropertyKey Duration => new PropertyKey(new Guid("{64440490-4C8B-11D1-8B70-080036B11A03}"), 3);

				public static PropertyKey DVDID => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 15);

				public static PropertyKey EncodedBy => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 36);

				public static PropertyKey EncodingSettings => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 37);

				public static PropertyKey FrameCount => new PropertyKey(new Guid("{6444048F-4C8B-11D1-8B70-080036B11A03}"), 12);

				public static PropertyKey MCDI => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 16);

				public static PropertyKey MetadataContentProvider => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 17);

				public static PropertyKey Producer => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 22);

				public static PropertyKey PromotionUrl => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 33);

				public static PropertyKey ProtectionType => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 38);

				public static PropertyKey ProviderRating => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 39);

				public static PropertyKey ProviderStyle => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 40);

				public static PropertyKey Publisher => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 30);

				public static PropertyKey SubscriptionContentId => new PropertyKey(new Guid("{9AEBAE7A-9644-487D-A92C-657585ED751A}"), 100);

				public static PropertyKey Subtitle => new PropertyKey(new Guid("{56A3372E-CE9C-11D2-9F0E-006097C686F6}"), 38);

				public static PropertyKey UniqueFileIdentifier => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 35);

				public static PropertyKey UserNoAutoInfo => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 41);

				public static PropertyKey UserWebUrl => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 34);

				public static PropertyKey Writer => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 23);

				public static PropertyKey Year => new PropertyKey(new Guid("{56A3372E-CE9C-11D2-9F0E-006097C686F6}"), 5);
			}

			public static class Message
			{
				public static PropertyKey AttachmentContents => new PropertyKey(new Guid("{3143BF7C-80A8-4854-8880-E2E40189BDD0}"), 100);

				public static PropertyKey AttachmentNames => new PropertyKey(new Guid("{E3E0584C-B788-4A5A-BB20-7F5A44C9ACDD}"), 21);

				public static PropertyKey BccAddress => new PropertyKey(new Guid("{E3E0584C-B788-4A5A-BB20-7F5A44C9ACDD}"), 2);

				public static PropertyKey BccName => new PropertyKey(new Guid("{E3E0584C-B788-4A5A-BB20-7F5A44C9ACDD}"), 3);

				public static PropertyKey CcAddress => new PropertyKey(new Guid("{E3E0584C-B788-4A5A-BB20-7F5A44C9ACDD}"), 4);

				public static PropertyKey CcName => new PropertyKey(new Guid("{E3E0584C-B788-4A5A-BB20-7F5A44C9ACDD}"), 5);

				public static PropertyKey ConversationID => new PropertyKey(new Guid("{DC8F80BD-AF1E-4289-85B6-3DFC1B493992}"), 100);

				public static PropertyKey ConversationIndex => new PropertyKey(new Guid("{DC8F80BD-AF1E-4289-85B6-3DFC1B493992}"), 101);

				public static PropertyKey DateReceived => new PropertyKey(new Guid("{E3E0584C-B788-4A5A-BB20-7F5A44C9ACDD}"), 20);

				public static PropertyKey DateSent => new PropertyKey(new Guid("{E3E0584C-B788-4A5A-BB20-7F5A44C9ACDD}"), 19);

				public static PropertyKey Flags => new PropertyKey(new Guid("{A82D9EE7-CA67-4312-965E-226BCEA85023}"), 100);

				public static PropertyKey FromAddress => new PropertyKey(new Guid("{E3E0584C-B788-4A5A-BB20-7F5A44C9ACDD}"), 13);

				public static PropertyKey FromName => new PropertyKey(new Guid("{E3E0584C-B788-4A5A-BB20-7F5A44C9ACDD}"), 14);

				public static PropertyKey HasAttachments => new PropertyKey(new Guid("{9C1FCF74-2D97-41BA-B4AE-CB2E3661A6E4}"), 8);

				public static PropertyKey IsFwdOrReply => new PropertyKey(new Guid("{9A9BC088-4F6D-469E-9919-E705412040F9}"), 100);

				public static PropertyKey MessageClass => new PropertyKey(new Guid("{CD9ED458-08CE-418F-A70E-F912C7BB9C5C}"), 103);

				public static PropertyKey ProofInProgress => new PropertyKey(new Guid("{9098F33C-9A7D-48A8-8DE5-2E1227A64E91}"), 100);

				public static PropertyKey SenderAddress => new PropertyKey(new Guid("{0BE1C8E7-1981-4676-AE14-FDD78F05A6E7}"), 100);

				public static PropertyKey SenderName => new PropertyKey(new Guid("{0DA41CFA-D224-4A18-AE2F-596158DB4B3A}"), 100);

				public static PropertyKey Store => new PropertyKey(new Guid("{E3E0584C-B788-4A5A-BB20-7F5A44C9ACDD}"), 15);

				public static PropertyKey ToAddress => new PropertyKey(new Guid("{E3E0584C-B788-4A5A-BB20-7F5A44C9ACDD}"), 16);

				public static PropertyKey ToDoFlags => new PropertyKey(new Guid("{1F856A9F-6900-4ABA-9505-2D5F1B4D66CB}"), 100);

				public static PropertyKey ToDoTitle => new PropertyKey(new Guid("{BCCC8A3C-8CEF-42E5-9B1C-C69079398BC7}"), 100);

				public static PropertyKey ToName => new PropertyKey(new Guid("{E3E0584C-B788-4A5A-BB20-7F5A44C9ACDD}"), 17);
			}

			public static class Music
			{
				public static PropertyKey AlbumArtist => new PropertyKey(new Guid("{56A3372E-CE9C-11D2-9F0E-006097C686F6}"), 13);

				public static PropertyKey AlbumID => new PropertyKey(new Guid("{56A3372E-CE9C-11D2-9F0E-006097C686F6}"), 100);

				public static PropertyKey AlbumTitle => new PropertyKey(new Guid("{56A3372E-CE9C-11D2-9F0E-006097C686F6}"), 4);

				public static PropertyKey Artist => new PropertyKey(new Guid("{56A3372E-CE9C-11D2-9F0E-006097C686F6}"), 2);

				public static PropertyKey BeatsPerMinute => new PropertyKey(new Guid("{56A3372E-CE9C-11D2-9F0E-006097C686F6}"), 35);

				public static PropertyKey Composer => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 19);

				public static PropertyKey Conductor => new PropertyKey(new Guid("{56A3372E-CE9C-11D2-9F0E-006097C686F6}"), 36);

				public static PropertyKey ContentGroupDescription => new PropertyKey(new Guid("{56A3372E-CE9C-11D2-9F0E-006097C686F6}"), 33);

				public static PropertyKey DisplayArtist => new PropertyKey(new Guid("{FD122953-FA93-4EF7-92C3-04C946B2F7C8}"), 100);

				public static PropertyKey Genre => new PropertyKey(new Guid("{56A3372E-CE9C-11D2-9F0E-006097C686F6}"), 11);

				public static PropertyKey InitialKey => new PropertyKey(new Guid("{56A3372E-CE9C-11D2-9F0E-006097C686F6}"), 34);

				public static PropertyKey IsCompilation => new PropertyKey(new Guid("{C449D5CB-9EA4-4809-82E8-AF9D59DED6D1}"), 100);

				public static PropertyKey Lyrics => new PropertyKey(new Guid("{56A3372E-CE9C-11D2-9F0E-006097C686F6}"), 12);

				public static PropertyKey Mood => new PropertyKey(new Guid("{56A3372E-CE9C-11D2-9F0E-006097C686F6}"), 39);

				public static PropertyKey PartOfSet => new PropertyKey(new Guid("{56A3372E-CE9C-11D2-9F0E-006097C686F6}"), 37);

				public static PropertyKey Period => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 31);

				public static PropertyKey SynchronizedLyrics => new PropertyKey(new Guid("{6B223B6A-162E-4AA9-B39F-05D678FC6D77}"), 100);

				public static PropertyKey TrackNumber => new PropertyKey(new Guid("{56A3372E-CE9C-11D2-9F0E-006097C686F6}"), 7);
			}

			public static class Note
			{
				public static PropertyKey Color => new PropertyKey(new Guid("{4776CAFA-BCE4-4CB1-A23E-265E76D8EB11}"), 100);

				public static PropertyKey ColorText => new PropertyKey(new Guid("{46B4E8DE-CDB2-440D-885C-1658EB65B914}"), 100);
			}

			public static class Photo
			{
				public static PropertyKey Aperture => new PropertyKey(new Guid("{14B81DA1-0135-4D31-96D9-6CBFC9671A99}"), 37378);

				public static PropertyKey ApertureDenominator => new PropertyKey(new Guid("{E1A9A38B-6685-46BD-875E-570DC7AD7320}"), 100);

				public static PropertyKey ApertureNumerator => new PropertyKey(new Guid("{0337ECEC-39FB-4581-A0BD-4C4CC51E9914}"), 100);

				public static PropertyKey Brightness => new PropertyKey(new Guid("{1A701BF6-478C-4361-83AB-3701BB053C58}"), 100);

				public static PropertyKey BrightnessDenominator => new PropertyKey(new Guid("{6EBE6946-2321-440A-90F0-C043EFD32476}"), 100);

				public static PropertyKey BrightnessNumerator => new PropertyKey(new Guid("{9E7D118F-B314-45A0-8CFB-D654B917C9E9}"), 100);

				public static PropertyKey CameraManufacturer => new PropertyKey(new Guid("{14B81DA1-0135-4D31-96D9-6CBFC9671A99}"), 271);

				public static PropertyKey CameraModel => new PropertyKey(new Guid("{14B81DA1-0135-4D31-96D9-6CBFC9671A99}"), 272);

				public static PropertyKey CameraSerialNumber => new PropertyKey(new Guid("{14B81DA1-0135-4D31-96D9-6CBFC9671A99}"), 273);

				public static PropertyKey Contrast => new PropertyKey(new Guid("{2A785BA9-8D23-4DED-82E6-60A350C86A10}"), 100);

				public static PropertyKey ContrastText => new PropertyKey(new Guid("{59DDE9F2-5253-40EA-9A8B-479E96C6249A}"), 100);

				public static PropertyKey DateTaken => new PropertyKey(new Guid("{14B81DA1-0135-4D31-96D9-6CBFC9671A99}"), 36867);

				public static PropertyKey DigitalZoom => new PropertyKey(new Guid("{F85BF840-A925-4BC2-B0C4-8E36B598679E}"), 100);

				public static PropertyKey DigitalZoomDenominator => new PropertyKey(new Guid("{745BAF0E-E5C1-4CFB-8A1B-D031A0A52393}"), 100);

				public static PropertyKey DigitalZoomNumerator => new PropertyKey(new Guid("{16CBB924-6500-473B-A5BE-F1599BCBE413}"), 100);

				public static PropertyKey Event => new PropertyKey(new Guid("{14B81DA1-0135-4D31-96D9-6CBFC9671A99}"), 18248);

				public static PropertyKey EXIFVersion => new PropertyKey(new Guid("{D35F743A-EB2E-47F2-A286-844132CB1427}"), 100);

				public static PropertyKey ExposureBias => new PropertyKey(new Guid("{14B81DA1-0135-4D31-96D9-6CBFC9671A99}"), 37380);

				public static PropertyKey ExposureBiasDenominator => new PropertyKey(new Guid("{AB205E50-04B7-461C-A18C-2F233836E627}"), 100);

				public static PropertyKey ExposureBiasNumerator => new PropertyKey(new Guid("{738BF284-1D87-420B-92CF-5834BF6EF9ED}"), 100);

				public static PropertyKey ExposureIndex => new PropertyKey(new Guid("{967B5AF8-995A-46ED-9E11-35B3C5B9782D}"), 100);

				public static PropertyKey ExposureIndexDenominator => new PropertyKey(new Guid("{93112F89-C28B-492F-8A9D-4BE2062CEE8A}"), 100);

				public static PropertyKey ExposureIndexNumerator => new PropertyKey(new Guid("{CDEDCF30-8919-44DF-8F4C-4EB2FFDB8D89}"), 100);

				public static PropertyKey ExposureProgram => new PropertyKey(new Guid("{14B81DA1-0135-4D31-96D9-6CBFC9671A99}"), 34850);

				public static PropertyKey ExposureProgramText => new PropertyKey(new Guid("{FEC690B7-5F30-4646-AE47-4CAAFBA884A3}"), 100);

				public static PropertyKey ExposureTime => new PropertyKey(new Guid("{14B81DA1-0135-4D31-96D9-6CBFC9671A99}"), 33434);

				public static PropertyKey ExposureTimeDenominator => new PropertyKey(new Guid("{55E98597-AD16-42E0-B624-21599A199838}"), 100);

				public static PropertyKey ExposureTimeNumerator => new PropertyKey(new Guid("{257E44E2-9031-4323-AC38-85C552871B2E}"), 100);

				public static PropertyKey Flash => new PropertyKey(new Guid("{14B81DA1-0135-4D31-96D9-6CBFC9671A99}"), 37385);

				public static PropertyKey FlashEnergy => new PropertyKey(new Guid("{14B81DA1-0135-4D31-96D9-6CBFC9671A99}"), 41483);

				public static PropertyKey FlashEnergyDenominator => new PropertyKey(new Guid("{D7B61C70-6323-49CD-A5FC-C84277162C97}"), 100);

				public static PropertyKey FlashEnergyNumerator => new PropertyKey(new Guid("{FCAD3D3D-0858-400F-AAA3-2F66CCE2A6BC}"), 100);

				public static PropertyKey FlashManufacturer => new PropertyKey(new Guid("{AABAF6C9-E0C5-4719-8585-57B103E584FE}"), 100);

				public static PropertyKey FlashModel => new PropertyKey(new Guid("{FE83BB35-4D1A-42E2-916B-06F3E1AF719E}"), 100);

				public static PropertyKey FlashText => new PropertyKey(new Guid("{6B8B68F6-200B-47EA-8D25-D8050F57339F}"), 100);

				public static PropertyKey FNumber => new PropertyKey(new Guid("{14B81DA1-0135-4D31-96D9-6CBFC9671A99}"), 33437);

				public static PropertyKey FNumberDenominator => new PropertyKey(new Guid("{E92A2496-223B-4463-A4E3-30EABBA79D80}"), 100);

				public static PropertyKey FNumberNumerator => new PropertyKey(new Guid("{1B97738A-FDFC-462F-9D93-1957E08BE90C}"), 100);

				public static PropertyKey FocalLength => new PropertyKey(new Guid("{14B81DA1-0135-4D31-96D9-6CBFC9671A99}"), 37386);

				public static PropertyKey FocalLengthDenominator => new PropertyKey(new Guid("{305BC615-DCA1-44A5-9FD4-10C0BA79412E}"), 100);

				public static PropertyKey FocalLengthInFilm => new PropertyKey(new Guid("{A0E74609-B84D-4F49-B860-462BD9971F98}"), 100);

				public static PropertyKey FocalLengthNumerator => new PropertyKey(new Guid("{776B6B3B-1E3D-4B0C-9A0E-8FBAF2A8492A}"), 100);

				public static PropertyKey FocalPlaneXResolution => new PropertyKey(new Guid("{CFC08D97-C6F7-4484-89DD-EBEF4356FE76}"), 100);

				public static PropertyKey FocalPlaneXResolutionDenominator => new PropertyKey(new Guid("{0933F3F5-4786-4F46-A8E8-D64DD37FA521}"), 100);

				public static PropertyKey FocalPlaneXResolutionNumerator => new PropertyKey(new Guid("{DCCB10AF-B4E2-4B88-95F9-031B4D5AB490}"), 100);

				public static PropertyKey FocalPlaneYResolution => new PropertyKey(new Guid("{4FFFE4D0-914F-4AC4-8D6F-C9C61DE169B1}"), 100);

				public static PropertyKey FocalPlaneYResolutionDenominator => new PropertyKey(new Guid("{1D6179A6-A876-4031-B013-3347B2B64DC8}"), 100);

				public static PropertyKey FocalPlaneYResolutionNumerator => new PropertyKey(new Guid("{A2E541C5-4440-4BA8-867E-75CFC06828CD}"), 100);

				public static PropertyKey GainControl => new PropertyKey(new Guid("{FA304789-00C7-4D80-904A-1E4DCC7265AA}"), 100);

				public static PropertyKey GainControlDenominator => new PropertyKey(new Guid("{42864DFD-9DA4-4F77-BDED-4AAD7B256735}"), 100);

				public static PropertyKey GainControlNumerator => new PropertyKey(new Guid("{8E8ECF7C-B7B8-4EB8-A63F-0EE715C96F9E}"), 100);

				public static PropertyKey GainControlText => new PropertyKey(new Guid("{C06238B2-0BF9-4279-A723-25856715CB9D}"), 100);

				public static PropertyKey ISOSpeed => new PropertyKey(new Guid("{14B81DA1-0135-4D31-96D9-6CBFC9671A99}"), 34855);

				public static PropertyKey LensManufacturer => new PropertyKey(new Guid("{E6DDCAF7-29C5-4F0A-9A68-D19412EC7090}"), 100);

				public static PropertyKey LensModel => new PropertyKey(new Guid("{E1277516-2B5F-4869-89B1-2E585BD38B7A}"), 100);

				public static PropertyKey LightSource => new PropertyKey(new Guid("{14B81DA1-0135-4D31-96D9-6CBFC9671A99}"), 37384);

				public static PropertyKey MakerNote => new PropertyKey(new Guid("{FA303353-B659-4052-85E9-BCAC79549B84}"), 100);

				public static PropertyKey MakerNoteOffset => new PropertyKey(new Guid("{813F4124-34E6-4D17-AB3E-6B1F3C2247A1}"), 100);

				public static PropertyKey MaxAperture => new PropertyKey(new Guid("{08F6D7C2-E3F2-44FC-AF1E-5AA5C81A2D3E}"), 100);

				public static PropertyKey MaxApertureDenominator => new PropertyKey(new Guid("{C77724D4-601F-46C5-9B89-C53F93BCEB77}"), 100);

				public static PropertyKey MaxApertureNumerator => new PropertyKey(new Guid("{C107E191-A459-44C5-9AE6-B952AD4B906D}"), 100);

				public static PropertyKey MeteringMode => new PropertyKey(new Guid("{14B81DA1-0135-4D31-96D9-6CBFC9671A99}"), 37383);

				public static PropertyKey MeteringModeText => new PropertyKey(new Guid("{F628FD8C-7BA8-465A-A65B-C5AA79263A9E}"), 100);

				public static PropertyKey Orientation => new PropertyKey(new Guid("{14B81DA1-0135-4D31-96D9-6CBFC9671A99}"), 274);

				public static PropertyKey OrientationText => new PropertyKey(new Guid("{A9EA193C-C511-498A-A06B-58E2776DCC28}"), 100);

				public static PropertyKey PeopleNames => new PropertyKey(new Guid("{E8309B6E-084C-49B4-B1FC-90A80331B638}"), 100);

				public static PropertyKey PhotometricInterpretation => new PropertyKey(new Guid("{341796F1-1DF9-4B1C-A564-91BDEFA43877}"), 100);

				public static PropertyKey PhotometricInterpretationText => new PropertyKey(new Guid("{821437D6-9EAB-4765-A589-3B1CBBD22A61}"), 100);

				public static PropertyKey ProgramMode => new PropertyKey(new Guid("{6D217F6D-3F6A-4825-B470-5F03CA2FBE9B}"), 100);

				public static PropertyKey ProgramModeText => new PropertyKey(new Guid("{7FE3AA27-2648-42F3-89B0-454E5CB150C3}"), 100);

				public static PropertyKey RelatedSoundFile => new PropertyKey(new Guid("{318A6B45-087F-4DC2-B8CC-05359551FC9E}"), 100);

				public static PropertyKey Saturation => new PropertyKey(new Guid("{49237325-A95A-4F67-B211-816B2D45D2E0}"), 100);

				public static PropertyKey SaturationText => new PropertyKey(new Guid("{61478C08-B600-4A84-BBE4-E99C45F0A072}"), 100);

				public static PropertyKey Sharpness => new PropertyKey(new Guid("{FC6976DB-8349-4970-AE97-B3C5316A08F0}"), 100);

				public static PropertyKey SharpnessText => new PropertyKey(new Guid("{51EC3F47-DD50-421D-8769-334F50424B1E}"), 100);

				public static PropertyKey ShutterSpeed => new PropertyKey(new Guid("{14B81DA1-0135-4D31-96D9-6CBFC9671A99}"), 37377);

				public static PropertyKey ShutterSpeedDenominator => new PropertyKey(new Guid("{E13D8975-81C7-4948-AE3F-37CAE11E8FF7}"), 100);

				public static PropertyKey ShutterSpeedNumerator => new PropertyKey(new Guid("{16EA4042-D6F4-4BCA-8349-7C78D30FB333}"), 100);

				public static PropertyKey SubjectDistance => new PropertyKey(new Guid("{14B81DA1-0135-4D31-96D9-6CBFC9671A99}"), 37382);

				public static PropertyKey SubjectDistanceDenominator => new PropertyKey(new Guid("{0C840A88-B043-466D-9766-D4B26DA3FA77}"), 100);

				public static PropertyKey SubjectDistanceNumerator => new PropertyKey(new Guid("{8AF4961C-F526-43E5-AA81-DB768219178D}"), 100);

				public static PropertyKey TagViewAggregate => new PropertyKey(new Guid("{B812F15D-C2D8-4BBF-BACD-79744346113F}"), 100);

				public static PropertyKey TranscodedForSync => new PropertyKey(new Guid("{9A8EBB75-6458-4E82-BACB-35C0095B03BB}"), 100);

				public static PropertyKey WhiteBalance => new PropertyKey(new Guid("{EE3D3D8A-5381-4CFA-B13B-AAF66B5F4EC9}"), 100);

				public static PropertyKey WhiteBalanceText => new PropertyKey(new Guid("{6336B95E-C7A7-426D-86FD-7AE3D39C84B4}"), 100);
			}

			public static class PropGroup
			{
				public static PropertyKey Advanced => new PropertyKey(new Guid("{900A403B-097B-4B95-8AE2-071FDAEEB118}"), 100);

				public static PropertyKey Audio => new PropertyKey(new Guid("{2804D469-788F-48AA-8570-71B9C187E138}"), 100);

				public static PropertyKey Calendar => new PropertyKey(new Guid("{9973D2B5-BFD8-438A-BA94-5349B293181A}"), 100);

				public static PropertyKey Camera => new PropertyKey(new Guid("{DE00DE32-547E-4981-AD4B-542F2E9007D8}"), 100);

				public static PropertyKey Contact => new PropertyKey(new Guid("{DF975FD3-250A-4004-858F-34E29A3E37AA}"), 100);

				public static PropertyKey Content => new PropertyKey(new Guid("{D0DAB0BA-368A-4050-A882-6C010FD19A4F}"), 100);

				public static PropertyKey Description => new PropertyKey(new Guid("{8969B275-9475-4E00-A887-FF93B8B41E44}"), 100);

				public static PropertyKey FileSystem => new PropertyKey(new Guid("{E3A7D2C1-80FC-4B40-8F34-30EA111BDC2E}"), 100);

				public static PropertyKey General => new PropertyKey(new Guid("{CC301630-B192-4C22-B372-9F4C6D338E07}"), 100);

				public static PropertyKey GPS => new PropertyKey(new Guid("{F3713ADA-90E3-4E11-AAE5-FDC17685B9BE}"), 100);

				public static PropertyKey Image => new PropertyKey(new Guid("{E3690A87-0FA8-4A2A-9A9F-FCE8827055AC}"), 100);

				public static PropertyKey Media => new PropertyKey(new Guid("{61872CF7-6B5E-4B4B-AC2D-59DA84459248}"), 100);

				public static PropertyKey MediaAdvanced => new PropertyKey(new Guid("{8859A284-DE7E-4642-99BA-D431D044B1EC}"), 100);

				public static PropertyKey Message => new PropertyKey(new Guid("{7FD7259D-16B4-4135-9F97-7C96ECD2FA9E}"), 100);

				public static PropertyKey Music => new PropertyKey(new Guid("{68DD6094-7216-40F1-A029-43FE7127043F}"), 100);

				public static PropertyKey Origin => new PropertyKey(new Guid("{2598D2FB-5569-4367-95DF-5CD3A177E1A5}"), 100);

				public static PropertyKey PhotoAdvanced => new PropertyKey(new Guid("{0CB2BF5A-9EE7-4A86-8222-F01E07FDADAF}"), 100);

				public static PropertyKey RecordedTV => new PropertyKey(new Guid("{E7B33238-6584-4170-A5C0-AC25EFD9DA56}"), 100);

				public static PropertyKey Video => new PropertyKey(new Guid("{BEBE0920-7671-4C54-A3EB-49FDDFC191EE}"), 100);
			}

			public static class PropList
			{
				public static PropertyKey ConflictPrompt => new PropertyKey(new Guid("{C9944A21-A406-48FE-8225-AEC7E24C211B}"), 11);

				public static PropertyKey ContentViewModeForBrowse => new PropertyKey(new Guid("{C9944A21-A406-48FE-8225-AEC7E24C211B}"), 13);

				public static PropertyKey ContentViewModeForSearch => new PropertyKey(new Guid("{C9944A21-A406-48FE-8225-AEC7E24C211B}"), 14);

				public static PropertyKey ExtendedTileInfo => new PropertyKey(new Guid("{C9944A21-A406-48FE-8225-AEC7E24C211B}"), 9);

				public static PropertyKey FileOperationPrompt => new PropertyKey(new Guid("{C9944A21-A406-48FE-8225-AEC7E24C211B}"), 10);

				public static PropertyKey FullDetails => new PropertyKey(new Guid("{C9944A21-A406-48FE-8225-AEC7E24C211B}"), 2);

				public static PropertyKey InfoTip => new PropertyKey(new Guid("{C9944A21-A406-48FE-8225-AEC7E24C211B}"), 4);

				public static PropertyKey NonPersonal => new PropertyKey(new Guid("{49D1091F-082E-493F-B23F-D2308AA9668C}"), 100);

				public static PropertyKey PreviewDetails => new PropertyKey(new Guid("{C9944A21-A406-48FE-8225-AEC7E24C211B}"), 8);

				public static PropertyKey PreviewTitle => new PropertyKey(new Guid("{C9944A21-A406-48FE-8225-AEC7E24C211B}"), 6);

				public static PropertyKey QuickTip => new PropertyKey(new Guid("{C9944A21-A406-48FE-8225-AEC7E24C211B}"), 5);

				public static PropertyKey TileInfo => new PropertyKey(new Guid("{C9944A21-A406-48FE-8225-AEC7E24C211B}"), 3);

				public static PropertyKey XPDetailsPanel => new PropertyKey(new Guid("{F2275480-F782-4291-BD94-F13693513AEC}"), 0);
			}

			public static class RecordedTV
			{
				public static PropertyKey ChannelNumber => new PropertyKey(new Guid("{6D748DE2-8D38-4CC3-AC60-F009B057C557}"), 7);

				public static PropertyKey Credits => new PropertyKey(new Guid("{6D748DE2-8D38-4CC3-AC60-F009B057C557}"), 4);

				public static PropertyKey DateContentExpires => new PropertyKey(new Guid("{6D748DE2-8D38-4CC3-AC60-F009B057C557}"), 15);

				public static PropertyKey EpisodeName => new PropertyKey(new Guid("{6D748DE2-8D38-4CC3-AC60-F009B057C557}"), 2);

				public static PropertyKey IsATSCContent => new PropertyKey(new Guid("{6D748DE2-8D38-4CC3-AC60-F009B057C557}"), 16);

				public static PropertyKey IsClosedCaptioningAvailable => new PropertyKey(new Guid("{6D748DE2-8D38-4CC3-AC60-F009B057C557}"), 12);

				public static PropertyKey IsDTVContent => new PropertyKey(new Guid("{6D748DE2-8D38-4CC3-AC60-F009B057C557}"), 17);

				public static PropertyKey IsHDContent => new PropertyKey(new Guid("{6D748DE2-8D38-4CC3-AC60-F009B057C557}"), 18);

				public static PropertyKey IsRepeatBroadcast => new PropertyKey(new Guid("{6D748DE2-8D38-4CC3-AC60-F009B057C557}"), 13);

				public static PropertyKey IsSAP => new PropertyKey(new Guid("{6D748DE2-8D38-4CC3-AC60-F009B057C557}"), 14);

				public static PropertyKey NetworkAffiliation => new PropertyKey(new Guid("{2C53C813-FB63-4E22-A1AB-0B331CA1E273}"), 100);

				public static PropertyKey OriginalBroadcastDate => new PropertyKey(new Guid("{4684FE97-8765-4842-9C13-F006447B178C}"), 100);

				public static PropertyKey ProgramDescription => new PropertyKey(new Guid("{6D748DE2-8D38-4CC3-AC60-F009B057C557}"), 3);

				public static PropertyKey RecordingTime => new PropertyKey(new Guid("{A5477F61-7A82-4ECA-9DDE-98B69B2479B3}"), 100);

				public static PropertyKey StationCallSign => new PropertyKey(new Guid("{6D748DE2-8D38-4CC3-AC60-F009B057C557}"), 5);

				public static PropertyKey StationName => new PropertyKey(new Guid("{1B5439E7-EBA1-4AF8-BDD7-7AF1D4549493}"), 100);
			}

			public static class Search
			{
				public static PropertyKey AutoSummary => new PropertyKey(new Guid("{560C36C0-503A-11CF-BAA1-00004C752A9A}"), 2);

				public static PropertyKey ContainerHash => new PropertyKey(new Guid("{BCEEE283-35DF-4D53-826A-F36A3EEFC6BE}"), 100);

				public static PropertyKey Contents => new PropertyKey(new Guid("{B725F130-47EF-101A-A5F1-02608C9EEBAC}"), 19);

				public static PropertyKey EntryID => new PropertyKey(new Guid("{49691C90-7E17-101A-A91C-08002B2ECDA9}"), 5);

				public static PropertyKey ExtendedProperties => new PropertyKey(new Guid("{7B03B546-FA4F-4A52-A2FE-03D5311E5865}"), 100);

				public static PropertyKey GatherTime => new PropertyKey(new Guid("{0B63E350-9CCC-11D0-BCDB-00805FCCCE04}"), 8);

				public static PropertyKey HitCount => new PropertyKey(new Guid("{49691C90-7E17-101A-A91C-08002B2ECDA9}"), 4);

				public static PropertyKey IsClosedDirectory => new PropertyKey(new Guid("{0B63E343-9CCC-11D0-BCDB-00805FCCCE04}"), 23);

				public static PropertyKey IsFullyContained => new PropertyKey(new Guid("{0B63E343-9CCC-11D0-BCDB-00805FCCCE04}"), 24);

				public static PropertyKey QueryFocusedSummary => new PropertyKey(new Guid("{560C36C0-503A-11CF-BAA1-00004C752A9A}"), 3);

				public static PropertyKey QueryFocusedSummaryWithFallback => new PropertyKey(new Guid("{560C36C0-503A-11CF-BAA1-00004C752A9A}"), 4);

				public static PropertyKey Rank => new PropertyKey(new Guid("{49691C90-7E17-101A-A91C-08002B2ECDA9}"), 3);

				public static PropertyKey Store => new PropertyKey(new Guid("{A06992B3-8CAF-4ED7-A547-B259E32AC9FC}"), 100);

				public static PropertyKey UrlToIndex => new PropertyKey(new Guid("{0B63E343-9CCC-11D0-BCDB-00805FCCCE04}"), 2);

				public static PropertyKey UrlToIndexWithModificationTime => new PropertyKey(new Guid("{0B63E343-9CCC-11D0-BCDB-00805FCCCE04}"), 12);
			}

			public static class Shell
			{
				public static PropertyKey OmitFromView => new PropertyKey(new Guid("{DE35258C-C695-4CBC-B982-38B0AD24CED0}"), 2);

				public static PropertyKey SFGAOFlagsStrings => new PropertyKey(new Guid("{D6942081-D53B-443D-AD47-5E059D9CD27A}"), 2);
			}

			public static class Software
			{
				public static PropertyKey DateLastUsed => new PropertyKey(new Guid("{841E4F90-FF59-4D16-8947-E81BBFFAB36D}"), 16);

				public static PropertyKey ProductName => new PropertyKey(new Guid("{0CEF7D53-FA64-11D1-A203-0000F81FEDEE}"), 7);
			}

			public static class Sync
			{
				public static PropertyKey Comments => new PropertyKey(new Guid("{7BD5533E-AF15-44DB-B8C8-BD6624E1D032}"), 13);

				public static PropertyKey ConflictDescription => new PropertyKey(new Guid("{CE50C159-2FB8-41FD-BE68-D3E042E274BC}"), 4);

				public static PropertyKey ConflictFirstLocation => new PropertyKey(new Guid("{CE50C159-2FB8-41FD-BE68-D3E042E274BC}"), 6);

				public static PropertyKey ConflictSecondLocation => new PropertyKey(new Guid("{CE50C159-2FB8-41FD-BE68-D3E042E274BC}"), 7);

				public static PropertyKey HandlerCollectionID => new PropertyKey(new Guid("{7BD5533E-AF15-44DB-B8C8-BD6624E1D032}"), 2);

				public static PropertyKey HandlerID => new PropertyKey(new Guid("{7BD5533E-AF15-44DB-B8C8-BD6624E1D032}"), 3);

				public static PropertyKey HandlerName => new PropertyKey(new Guid("{CE50C159-2FB8-41FD-BE68-D3E042E274BC}"), 2);

				public static PropertyKey HandlerType => new PropertyKey(new Guid("{7BD5533E-AF15-44DB-B8C8-BD6624E1D032}"), 8);

				public static PropertyKey HandlerTypeLabel => new PropertyKey(new Guid("{7BD5533E-AF15-44DB-B8C8-BD6624E1D032}"), 9);

				public static PropertyKey ItemID => new PropertyKey(new Guid("{7BD5533E-AF15-44DB-B8C8-BD6624E1D032}"), 6);

				public static PropertyKey ItemName => new PropertyKey(new Guid("{CE50C159-2FB8-41FD-BE68-D3E042E274BC}"), 3);

				public static PropertyKey ProgressPercentage => new PropertyKey(new Guid("{7BD5533E-AF15-44DB-B8C8-BD6624E1D032}"), 23);

				public static PropertyKey State => new PropertyKey(new Guid("{7BD5533E-AF15-44DB-B8C8-BD6624E1D032}"), 24);

				public static PropertyKey Status => new PropertyKey(new Guid("{7BD5533E-AF15-44DB-B8C8-BD6624E1D032}"), 10);
			}

			public static class Task
			{
				public static PropertyKey BillingInformation => new PropertyKey(new Guid("{D37D52C6-261C-4303-82B3-08B926AC6F12}"), 100);

				public static PropertyKey CompletionStatus => new PropertyKey(new Guid("{084D8A0A-E6D5-40DE-BF1F-C8820E7C877C}"), 100);

				public static PropertyKey Owner => new PropertyKey(new Guid("{08C7CC5F-60F2-4494-AD75-55E3E0B5ADD0}"), 100);
			}

			public static class Video
			{
				public static PropertyKey Compression => new PropertyKey(new Guid("{64440491-4C8B-11D1-8B70-080036B11A03}"), 10);

				public static PropertyKey Director => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 20);

				public static PropertyKey EncodingBitrate => new PropertyKey(new Guid("{64440491-4C8B-11D1-8B70-080036B11A03}"), 8);

				public static PropertyKey FourCC => new PropertyKey(new Guid("{64440491-4C8B-11D1-8B70-080036B11A03}"), 44);

				public static PropertyKey FrameHeight => new PropertyKey(new Guid("{64440491-4C8B-11D1-8B70-080036B11A03}"), 4);

				public static PropertyKey FrameRate => new PropertyKey(new Guid("{64440491-4C8B-11D1-8B70-080036B11A03}"), 6);

				public static PropertyKey FrameWidth => new PropertyKey(new Guid("{64440491-4C8B-11D1-8B70-080036B11A03}"), 3);

				public static PropertyKey HorizontalAspectRatio => new PropertyKey(new Guid("{64440491-4C8B-11D1-8B70-080036B11A03}"), 42);

				public static PropertyKey SampleSize => new PropertyKey(new Guid("{64440491-4C8B-11D1-8B70-080036B11A03}"), 9);

				public static PropertyKey StreamName => new PropertyKey(new Guid("{64440491-4C8B-11D1-8B70-080036B11A03}"), 2);

				public static PropertyKey StreamNumber => new PropertyKey(new Guid("{64440491-4C8B-11D1-8B70-080036B11A03}"), 11);

				public static PropertyKey TotalBitrate => new PropertyKey(new Guid("{64440491-4C8B-11D1-8B70-080036B11A03}"), 43);

				public static PropertyKey TranscodedForSync => new PropertyKey(new Guid("{64440491-4C8B-11D1-8B70-080036B11A03}"), 46);

				public static PropertyKey VerticalAspectRatio => new PropertyKey(new Guid("{64440491-4C8B-11D1-8B70-080036B11A03}"), 45);
			}

			public static class Volume
			{
				public static PropertyKey FileSystem => new PropertyKey(new Guid("{9B174B35-40FF-11D2-A27E-00C04FC30871}"), 4);

				public static PropertyKey IsMappedDrive => new PropertyKey(new Guid("{149C0B69-2C2D-48FC-808F-D318D78C4636}"), 2);

				public static PropertyKey IsRoot => new PropertyKey(new Guid("{9B174B35-40FF-11D2-A27E-00C04FC30871}"), 10);
			}

			public static PropertyKey AcquisitionID => new PropertyKey(new Guid("{65A98875-3C80-40AB-ABBC-EFDAF77DBEE2}"), 100);

			public static PropertyKey ApplicationName => new PropertyKey(new Guid("{F29F85E0-4FF9-1068-AB91-08002B27B3D9}"), 18);

			public static PropertyKey Author => new PropertyKey(new Guid("{F29F85E0-4FF9-1068-AB91-08002B27B3D9}"), 4);

			public static PropertyKey Capacity => new PropertyKey(new Guid("{9B174B35-40FF-11D2-A27E-00C04FC30871}"), 3);

			public static PropertyKey Category => new PropertyKey(new Guid("{D5CDD502-2E9C-101B-9397-08002B2CF9AE}"), 2);

			public static PropertyKey Comment => new PropertyKey(new Guid("{F29F85E0-4FF9-1068-AB91-08002B27B3D9}"), 6);

			public static PropertyKey Company => new PropertyKey(new Guid("{D5CDD502-2E9C-101B-9397-08002B2CF9AE}"), 15);

			public static PropertyKey ComputerName => new PropertyKey(new Guid("{28636AA6-953D-11D2-B5D6-00C04FD918D0}"), 5);

			public static PropertyKey ContainedItems => new PropertyKey(new Guid("{28636AA6-953D-11D2-B5D6-00C04FD918D0}"), 29);

			public static PropertyKey ContentStatus => new PropertyKey(new Guid("{D5CDD502-2E9C-101B-9397-08002B2CF9AE}"), 27);

			public static PropertyKey ContentType => new PropertyKey(new Guid("{D5CDD502-2E9C-101B-9397-08002B2CF9AE}"), 26);

			public static PropertyKey Copyright => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 11);

			public static PropertyKey DateAccessed => new PropertyKey(new Guid("{B725F130-47EF-101A-A5F1-02608C9EEBAC}"), 16);

			public static PropertyKey DateAcquired => new PropertyKey(new Guid("{2CBAA8F5-D81F-47CA-B17A-F8D822300131}"), 100);

			public static PropertyKey DateArchived => new PropertyKey(new Guid("{43F8D7B7-A444-4F87-9383-52271C9B915C}"), 100);

			public static PropertyKey DateCompleted => new PropertyKey(new Guid("{72FAB781-ACDA-43E5-B155-B2434F85E678}"), 100);

			public static PropertyKey DateCreated => new PropertyKey(new Guid("{B725F130-47EF-101A-A5F1-02608C9EEBAC}"), 15);

			public static PropertyKey DateImported => new PropertyKey(new Guid("{14B81DA1-0135-4D31-96D9-6CBFC9671A99}"), 18258);

			public static PropertyKey DateModified => new PropertyKey(new Guid("{B725F130-47EF-101A-A5F1-02608C9EEBAC}"), 14);

			public static PropertyKey DescriptionID => new PropertyKey(new Guid("{28636AA6-953D-11D2-B5D6-00C04FD918D0}"), 2);

			public static PropertyKey DueDate => new PropertyKey(new Guid("{3F8472B5-E0AF-4DB2-8071-C53FE76AE7CE}"), 100);

			public static PropertyKey EndDate => new PropertyKey(new Guid("{C75FAA05-96FD-49E7-9CB4-9F601082D553}"), 100);

			public static PropertyKey FileAllocationSize => new PropertyKey(new Guid("{B725F130-47EF-101A-A5F1-02608C9EEBAC}"), 18);

			public static PropertyKey FileAttributes => new PropertyKey(new Guid("{B725F130-47EF-101A-A5F1-02608C9EEBAC}"), 13);

			public static PropertyKey FileCount => new PropertyKey(new Guid("{28636AA6-953D-11D2-B5D6-00C04FD918D0}"), 12);

			public static PropertyKey FileDescription => new PropertyKey(new Guid("{0CEF7D53-FA64-11D1-A203-0000F81FEDEE}"), 3);

			public static PropertyKey FileExtension => new PropertyKey(new Guid("{E4F10A3C-49E6-405D-8288-A23BD4EEAA6C}"), 100);

			public static PropertyKey FileFRN => new PropertyKey(new Guid("{B725F130-47EF-101A-A5F1-02608C9EEBAC}"), 21);

			public static PropertyKey FileName => new PropertyKey(new Guid("{41CF5AE0-F75A-4806-BD87-59C7D9248EB9}"), 100);

			public static PropertyKey FileOwner => new PropertyKey(new Guid("{9B174B34-40FF-11D2-A27E-00C04FC30871}"), 4);

			public static PropertyKey FileVersion => new PropertyKey(new Guid("{0CEF7D53-FA64-11D1-A203-0000F81FEDEE}"), 4);

			public static PropertyKey FindData => new PropertyKey(new Guid("{28636AA6-953D-11D2-B5D6-00C04FD918D0}"), 0);

			public static PropertyKey FlagColor => new PropertyKey(new Guid("{67DF94DE-0CA7-4D6F-B792-053A3E4F03CF}"), 100);

			public static PropertyKey FlagColorText => new PropertyKey(new Guid("{45EAE747-8E2A-40AE-8CBF-CA52ABA6152A}"), 100);

			public static PropertyKey FlagStatus => new PropertyKey(new Guid("{E3E0584C-B788-4A5A-BB20-7F5A44C9ACDD}"), 12);

			public static PropertyKey FlagStatusText => new PropertyKey(new Guid("{DC54FD2E-189D-4871-AA01-08C2F57A4ABC}"), 100);

			public static PropertyKey FreeSpace => new PropertyKey(new Guid("{9B174B35-40FF-11D2-A27E-00C04FC30871}"), 2);

			public static PropertyKey FullText => new PropertyKey(new Guid("{1E3EE840-BC2B-476C-8237-2ACD1A839B22}"), 6);

			public static PropertyKey IdentityProperty => new PropertyKey(new Guid("{A26F4AFC-7346-4299-BE47-EB1AE613139F}"), 100);

			public static PropertyKey ImageParsingName => new PropertyKey(new Guid("{D7750EE0-C6A4-48EC-B53E-B87B52E6D073}"), 100);

			public static PropertyKey Importance => new PropertyKey(new Guid("{E3E0584C-B788-4A5A-BB20-7F5A44C9ACDD}"), 11);

			public static PropertyKey ImportanceText => new PropertyKey(new Guid("{A3B29791-7713-4E1D-BB40-17DB85F01831}"), 100);

			public static PropertyKey InfoTipText => new PropertyKey(new Guid("{C9944A21-A406-48FE-8225-AEC7E24C211B}"), 17);

			public static PropertyKey InternalName => new PropertyKey(new Guid("{0CEF7D53-FA64-11D1-A203-0000F81FEDEE}"), 5);

			public static PropertyKey IsAttachment => new PropertyKey(new Guid("{F23F425C-71A1-4FA8-922F-678EA4A60408}"), 100);

			public static PropertyKey IsDefaultNonOwnerSaveLocation => new PropertyKey(new Guid("{5D76B67F-9B3D-44BB-B6AE-25DA4F638A67}"), 5);

			public static PropertyKey IsDefaultSaveLocation => new PropertyKey(new Guid("{5D76B67F-9B3D-44BB-B6AE-25DA4F638A67}"), 3);

			public static PropertyKey IsDeleted => new PropertyKey(new Guid("{5CDA5FC8-33EE-4FF3-9094-AE7BD8868C4D}"), 100);

			public static PropertyKey IsEncrypted => new PropertyKey(new Guid("{90E5E14E-648B-4826-B2AA-ACAF790E3513}"), 10);

			public static PropertyKey IsFlagged => new PropertyKey(new Guid("{5DA84765-E3FF-4278-86B0-A27967FBDD03}"), 100);

			public static PropertyKey IsFlaggedComplete => new PropertyKey(new Guid("{A6F360D2-55F9-48DE-B909-620E090A647C}"), 100);

			public static PropertyKey IsIncomplete => new PropertyKey(new Guid("{346C8BD1-2E6A-4C45-89A4-61B78E8E700F}"), 100);

			public static PropertyKey IsLocationSupported => new PropertyKey(new Guid("{5D76B67F-9B3D-44BB-B6AE-25DA4F638A67}"), 8);

			public static PropertyKey IsPinnedToNamespaceTree => new PropertyKey(new Guid("{5D76B67F-9B3D-44BB-B6AE-25DA4F638A67}"), 2);

			public static PropertyKey IsRead => new PropertyKey(new Guid("{E3E0584C-B788-4A5A-BB20-7F5A44C9ACDD}"), 10);

			public static PropertyKey IsSearchOnlyItem => new PropertyKey(new Guid("{5D76B67F-9B3D-44BB-B6AE-25DA4F638A67}"), 4);

			public static PropertyKey IsSendToTarget => new PropertyKey(new Guid("{28636AA6-953D-11D2-B5D6-00C04FD918D0}"), 33);

			public static PropertyKey IsShared => new PropertyKey(new Guid("{EF884C5B-2BFE-41BB-AAE5-76EEDF4F9902}"), 100);

			public static PropertyKey ItemAuthors => new PropertyKey(new Guid("{D0A04F0A-462A-48A4-BB2F-3706E88DBD7D}"), 100);

			public static PropertyKey ItemClassType => new PropertyKey(new Guid("{048658AD-2DB8-41A4-BBB6-AC1EF1207EB1}"), 100);

			public static PropertyKey ItemDate => new PropertyKey(new Guid("{F7DB74B4-4287-4103-AFBA-F1B13DCD75CF}"), 100);

			public static PropertyKey ItemFolderNameDisplay => new PropertyKey(new Guid("{B725F130-47EF-101A-A5F1-02608C9EEBAC}"), 2);

			public static PropertyKey ItemFolderPathDisplay => new PropertyKey(new Guid("{E3E0584C-B788-4A5A-BB20-7F5A44C9ACDD}"), 6);

			public static PropertyKey ItemFolderPathDisplayNarrow => new PropertyKey(new Guid("{DABD30ED-0043-4789-A7F8-D013A4736622}"), 100);

			public static PropertyKey ItemName => new PropertyKey(new Guid("{6B8DA074-3B5C-43BC-886F-0A2CDCE00B6F}"), 100);

			public static PropertyKey ItemNameDisplay => new PropertyKey(new Guid("{B725F130-47EF-101A-A5F1-02608C9EEBAC}"), 10);

			public static PropertyKey ItemNamePrefix => new PropertyKey(new Guid("{D7313FF1-A77A-401C-8C99-3DBDD68ADD36}"), 100);

			public static PropertyKey ItemParticipants => new PropertyKey(new Guid("{D4D0AA16-9948-41A4-AA85-D97FF9646993}"), 100);

			public static PropertyKey ItemPathDisplay => new PropertyKey(new Guid("{E3E0584C-B788-4A5A-BB20-7F5A44C9ACDD}"), 7);

			public static PropertyKey ItemPathDisplayNarrow => new PropertyKey(new Guid("{28636AA6-953D-11D2-B5D6-00C04FD918D0}"), 8);

			public static PropertyKey ItemType => new PropertyKey(new Guid("{28636AA6-953D-11D2-B5D6-00C04FD918D0}"), 11);

			public static PropertyKey ItemTypeText => new PropertyKey(new Guid("{B725F130-47EF-101A-A5F1-02608C9EEBAC}"), 4);

			public static PropertyKey ItemUrl => new PropertyKey(new Guid("{49691C90-7E17-101A-A91C-08002B2ECDA9}"), 9);

			public static PropertyKey Keywords => new PropertyKey(new Guid("{F29F85E0-4FF9-1068-AB91-08002B27B3D9}"), 5);

			public static PropertyKey Kind => new PropertyKey(new Guid("{1E3EE840-BC2B-476C-8237-2ACD1A839B22}"), 3);

			public static PropertyKey KindText => new PropertyKey(new Guid("{F04BEF95-C585-4197-A2B7-DF46FDC9EE6D}"), 100);

			public static PropertyKey Language => new PropertyKey(new Guid("{D5CDD502-2E9C-101B-9397-08002B2CF9AE}"), 28);

			public static PropertyKey MileageInformation => new PropertyKey(new Guid("{FDF84370-031A-4ADD-9E91-0D775F1C6605}"), 100);

			public static PropertyKey MIMEType => new PropertyKey(new Guid("{0B63E350-9CCC-11D0-BCDB-00805FCCCE04}"), 5);

			public static PropertyKey NamespaceClsid => new PropertyKey(new Guid("{28636AA6-953D-11D2-B5D6-00C04FD918D0}"), 6);

			public static PropertyKey Null => new PropertyKey(new Guid("{00000000-0000-0000-0000-000000000000}"), 0);

			public static PropertyKey OfflineAvailability => new PropertyKey(new Guid("{A94688B6-7D9F-4570-A648-E3DFC0AB2B3F}"), 100);

			public static PropertyKey OfflineStatus => new PropertyKey(new Guid("{6D24888F-4718-4BDA-AFED-EA0FB4386CD8}"), 100);

			public static PropertyKey OriginalFileName => new PropertyKey(new Guid("{0CEF7D53-FA64-11D1-A203-0000F81FEDEE}"), 6);

			public static PropertyKey OwnerSid => new PropertyKey(new Guid("{5D76B67F-9B3D-44BB-B6AE-25DA4F638A67}"), 6);

			public static PropertyKey ParentalRating => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 21);

			public static PropertyKey ParentalRatingReason => new PropertyKey(new Guid("{10984E0A-F9F2-4321-B7EF-BAF195AF4319}"), 100);

			public static PropertyKey ParentalRatingsOrganization => new PropertyKey(new Guid("{A7FE0840-1344-46F0-8D37-52ED712A4BF9}"), 100);

			public static PropertyKey ParsingBindContext => new PropertyKey(new Guid("{DFB9A04D-362F-4CA3-B30B-0254B17B5B84}"), 100);

			public static PropertyKey ParsingName => new PropertyKey(new Guid("{28636AA6-953D-11D2-B5D6-00C04FD918D0}"), 24);

			public static PropertyKey ParsingPath => new PropertyKey(new Guid("{28636AA6-953D-11D2-B5D6-00C04FD918D0}"), 30);

			public static PropertyKey PerceivedType => new PropertyKey(new Guid("{28636AA6-953D-11D2-B5D6-00C04FD918D0}"), 9);

			public static PropertyKey PercentFull => new PropertyKey(new Guid("{9B174B35-40FF-11D2-A27E-00C04FC30871}"), 5);

			public static PropertyKey Priority => new PropertyKey(new Guid("{9C1FCF74-2D97-41BA-B4AE-CB2E3661A6E4}"), 5);

			public static PropertyKey PriorityText => new PropertyKey(new Guid("{D98BE98B-B86B-4095-BF52-9D23B2E0A752}"), 100);

			public static PropertyKey Project => new PropertyKey(new Guid("{39A7F922-477C-48DE-8BC8-B28441E342E3}"), 100);

			public static PropertyKey ProviderItemID => new PropertyKey(new Guid("{F21D9941-81F0-471A-ADEE-4E74B49217ED}"), 100);

			public static PropertyKey Rating => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 9);

			public static PropertyKey RatingText => new PropertyKey(new Guid("{90197CA7-FD8F-4E8C-9DA3-B57E1E609295}"), 100);

			public static PropertyKey Sensitivity => new PropertyKey(new Guid("{F8D3F6AC-4874-42CB-BE59-AB454B30716A}"), 100);

			public static PropertyKey SensitivityText => new PropertyKey(new Guid("{D0C7F054-3F72-4725-8527-129A577CB269}"), 100);

			public static PropertyKey SFGAOFlags => new PropertyKey(new Guid("{28636AA6-953D-11D2-B5D6-00C04FD918D0}"), 25);

			public static PropertyKey SharedWith => new PropertyKey(new Guid("{EF884C5B-2BFE-41BB-AAE5-76EEDF4F9902}"), 200);

			public static PropertyKey ShareUserRating => new PropertyKey(new Guid("{64440492-4C8B-11D1-8B70-080036B11A03}"), 12);

			public static PropertyKey SharingStatus => new PropertyKey(new Guid("{EF884C5B-2BFE-41BB-AAE5-76EEDF4F9902}"), 300);

			public static PropertyKey SimpleRating => new PropertyKey(new Guid("{A09F084E-AD41-489F-8076-AA5BE3082BCA}"), 100);

			public static PropertyKey Size => new PropertyKey(new Guid("{B725F130-47EF-101A-A5F1-02608C9EEBAC}"), 12);

			public static PropertyKey SoftwareUsed => new PropertyKey(new Guid("{14B81DA1-0135-4D31-96D9-6CBFC9671A99}"), 305);

			public static PropertyKey SourceItem => new PropertyKey(new Guid("{668CDFA5-7A1B-4323-AE4B-E527393A1D81}"), 100);

			public static PropertyKey StartDate => new PropertyKey(new Guid("{48FD6EC8-8A12-4CDF-A03E-4EC5A511EDDE}"), 100);

			public static PropertyKey Status => new PropertyKey(new Guid("{000214A1-0000-0000-C000-000000000046}"), 9);

			public static PropertyKey Subject => new PropertyKey(new Guid("{F29F85E0-4FF9-1068-AB91-08002B27B3D9}"), 3);

			public static PropertyKey Thumbnail => new PropertyKey(new Guid("{F29F85E0-4FF9-1068-AB91-08002B27B3D9}"), 17);

			public static PropertyKey ThumbnailCacheId => new PropertyKey(new Guid("{446D16B1-8DAD-4870-A748-402EA43D788C}"), 100);

			public static PropertyKey ThumbnailStream => new PropertyKey(new Guid("{F29F85E0-4FF9-1068-AB91-08002B27B3D9}"), 27);

			public static PropertyKey Title => new PropertyKey(new Guid("{F29F85E0-4FF9-1068-AB91-08002B27B3D9}"), 2);

			public static PropertyKey TotalFileSize => new PropertyKey(new Guid("{28636AA6-953D-11D2-B5D6-00C04FD918D0}"), 14);

			public static PropertyKey Trademarks => new PropertyKey(new Guid("{0CEF7D53-FA64-11D1-A203-0000F81FEDEE}"), 9);
		}

		public static ShellPropertyDescription GetPropertyDescription(PropertyKey propertyKey)
		{
			return ShellPropertyDescriptionsCache.Cache.GetPropertyDescription(propertyKey);
		}

		public static ShellPropertyDescription GetPropertyDescription(string canonicalName)
		{
			PropertyKey propkey;
			int num = PropertySystemNativeMethods.PSGetPropertyKeyFromName(canonicalName, out propkey);
			if (!CoreErrorHelper.Succeeded(num))
			{
				throw new ArgumentException(LocalizedMessages.ShellInvalidCanonicalName, Marshal.GetExceptionForHR(num));
			}
			return ShellPropertyDescriptionsCache.Cache.GetPropertyDescription(propkey);
		}
	}
}
