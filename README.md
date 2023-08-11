# INSTALLER REPACK
A "simple" installer C# project template for making installers

## Requirements
- .NET Framework 4.7.2
- Visual Studio 2017 or higher
- .NET desktop development Workload installed on Visual Studio
## HOW TO USE?
You can refer to a video tutorial here if you want to

[Download Video](https://cdn.discordapp.com/attachments/883753960828198958/1139533769129328700/HowToUseInstallerRepack.mp4)

[YouTube Link](https://youtu.be/y18NYiFvlFA)

### Changing the installer config
- Open up `Installer-Repack/MainForm.cs` and expand the Setup Config region, 
- You'll see a bunch of options you can change there along with the comments for more information
for each config
- Replace `Installer-Repack/Resources/InstallationArchive.zip` with your own installation archive

### Changing the uninstaller config
Open up `Uninstaller/MainForm.cs` and just copy and paste the config you have on `Installer-Repack/MainForm.cs`

### Finishing touches
- Rebuild Uninstaller
- Replace `Installer-Repack/Resources/Uninstaller.exe` with the built Uninstaller.exe
- Replace `Installer-Repack/Resources/Icon.ico` with your own installer icon
- Each replacement must have the same file name unless if you know how to tinker around with resource files

### Final step
- Rebuild Installer-Repack (If you only build it the resources might not refresh)
