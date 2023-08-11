using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using IWshRuntimeLibrary;
using Ionic.Zip;

namespace Aseprite_Repack
{
    public partial class MainForm : Form
    {
        #region Setup Config

        // The original name of the program
        public const string programName = "Godot";

        // The company/creator name of the program
        const string companyName = "Godot Foundation";

        // The version of the program
        const string appVersion = "3.5.2-stable";

        // The link for the program
        const string programLink = "https://godotengine.org";

        // The window title of the setup
        const string installerName = "Godot Setup";

        // This is what will be shown default on installation path
        const string defaultDestinationPath = "C:\\Program Files (x86)\\Godot";

        // This path is relative to the destination path
        const string exePath = "Godot_v3.5.2-stable_win64.exe";

        // A special GUID specific for the program (you can use Guid.NewGuid() too)
        const string programGUID = "{5c023135-e620-402b-b2ae-17c670e5f843}";

        // Format: { <Extension>, <FileDescription> }
        static Dictionary<string, string> associations = new Dictionary<string, string>()
        {
            { ".godot", "Godot Project File" }
        };

        // Manually put the extracted size of your program (in bytes)
        long archiveSize = 76582340;

        // Put your archive on InstallerResource.resx
        static byte[] archiveBytes = InstallerResource.InstallationArchive;

        // Put your installer icon on InstallerResource.resx
        // Also change the project properties so the output .exe installer have the same icon
        static System.Drawing.Icon installerIcon = InstallerResource.Icon;

        #endregion

        #region Variables
        InstallSection section = InstallSection.Home;
        Panel[] sections;

        string destinationPath;
        List<string> installedPaths = new List<string>();
        List<AssociationCheckBox> assocBoxes = new List<AssociationCheckBox>();

        long freeSpace = 0;
        bool passDialog;

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("Shell32.dll")]
        private static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);
        #endregion

        #region Form
        public MainForm()
        {
            InitializeComponent();
            sections = new[]{
                welcomePanel,
                pathPanel,
                optionsPanel,
                installationPanel,
                finishPanel
            };
            ApplyConfigs();
            Shown += MainForm_Shown;
        }

        public void ApplyConfigs()
        {
            MoveTab(InstallSection.Home);

            // Installer Window
            Text = installerName;
            Icon = installerIcon;

            // Installer Labels
            titleLabel.ChangeTextAsProgram();
            descriptionLabel.ChangeTextAsProgram();
            pathDescLabel.ChangeTextAsProgram();
            installHeaderDesc.ChangeTextAsProgram();
            optionDescLabel.ChangeTextAsProgram();
            finishTitleLabel.ChangeTextAsProgram();
            finishDescLabel.ChangeTextAsProgram();
            finishLaunchBox.ChangeTextAsProgram();

            requiredSpaceLabel.Text = "Required free space: " + BytesToString(archiveSize);

            // Options Associations
            foreach(var assoc in associations)
            {
                var box = new AssociationCheckBox();
                box.Prepare(assoc.Key, programName, assoc.Value);
                box.Checked = true;
                box.Size = new System.Drawing.Size(400, 17);
                associationPanel.Controls.Add(box);
                assocBoxes.Add(box);
            }

            destinationPathBox.Text = defaultDestinationPath;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            SetWindowLong(Handle, (int)NativeWindowLong.GWL_STYLE,
                (int)(
                NativeWindowStyle.WS_OVERLAPPED |
                NativeWindowStyle.WS_MINIMIZEBOX |
                NativeWindowStyle.WS_GROUP |
                NativeWindowStyle.WS_SYSMENU |
                NativeWindowStyle.WS_DLGFRAME |
                NativeWindowStyle.WS_CAPTION |
                NativeWindowStyle.WS_CLIPSIBLINGS |
                NativeWindowStyle.WS_VISIBLE |
                NativeWindowStyle.WS_BORDER
                )
            );
        }

        private void browsePathBox_Click(object sender, EventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.Title = $"Select the folder where you want to install {programName}";
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                destinationPathBox.Text = dialog.FileName;
        }

        private void destinationPathBox_TextChanged(object sender, EventArgs e)
        {
            destinationPath = destinationPathBox.Text;

            if (!string.IsNullOrEmpty(destinationPath))
                RefreshFreeSpace();
        }
        
        private void LeftestButton_Click(object sender, EventArgs e)
        {
            if (section == InstallSection.Path)
            {
                LeftestButton.Visible = false;
                MoveTab(InstallSection.Home);
            }
            else
            {
                MoveTab(((int)section) - 1);
            }
        }

        private void LeftButton_Click(object sender, EventArgs e)
        {
            if (section == InstallSection.Options)
            {
                LeftestButton.Enabled = false;
                LeftButton.Enabled = false;
                MoveTab(InstallSection.Installation);
                StartInstallation();
            }
            else if (section == InstallSection.Finish)
            {
                passDialog = true;
                if(finishLaunchBox.Checked)
                {
                    string executablePath = Path.Combine(destinationPath, exePath);
                    Process.Start(executablePath);
                }
                Close();
            }
            else if (section == InstallSection.Home)
            {
                LeftestButton.Visible = true;
                RefreshFreeSpace();
                MoveTab(InstallSection.Path);
            }
            else
            {
                MoveTab(((int)section) + 1);
            }
        }

        private void RightButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!passDialog)
            {
                e.Cancel = true;
                if (MessageBox.Show("Are you sure you want to abort the installation?", installerName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    passDialog = true;
                    Close();
                }
            }
        }
        #endregion

        #region Installation
        public void StartInstallation()
        {
            Task.Run(InstallationTask);
        }

        public Task InstallationTask()
        {
        
            try
            {
                if (!Directory.Exists(destinationPath))
                    Directory.CreateDirectory(destinationPath);
            }
            catch (Exception e) { }
            using (var memoryStream = new MemoryStream(archiveBytes))
            {
                try
                {
                    using (ZipFile zip = ZipFile.Read(memoryStream))
                    {
                        var entries = new List<ZipEntry>(zip.Entries);
                        for (int i = 0; i < entries.Count; i++)
                        {
                            ZipEntry e = entries[i];
                            string filePath = Path.Combine(destinationPath, e.FileName).Replace("/", "\\");
                            float prog = (((float)i) / ((float)entries.Count) * 100);
                            installedPaths.Add(e.FileName);
                            Invoke(new Action(() => installationProgLabel.Text = $"Installing: {filePath}"));
                            Invoke(new Action(() => installationbar.Value = prog > 100 ? 100 : (int)prog));
                        tryagain:
                            try
                            {
                                e.Extract(destinationPath, ExtractExistingFileAction.OverwriteSilently);
                            }
                            catch (Exception ex)
                            {
                                var result = MessageBox.Show($"Unable to install file {filePath}: {ex.Message}", "Continue?", MessageBoxButtons.AbortRetryIgnore);
                                if (result == DialogResult.Retry)
                                {
                                    i--;
                                    goto tryagain;
                                }
                                else if (result == DialogResult.Abort)
                                {
                                    Invoke(new Action(() => EndInstallation("Installation cancelled.", $"The installation of {programName} was cancelled by the user", true)));
                                    return Task.CompletedTask;
                                }
                            }
                        }
                    }
                    Invoke(new Action(() =>
                    {
                        string executablePath = Path.Combine(destinationPath, exePath);
                        if (desktopIconBox.Checked) CreateDesktopShortcut();
                        if (startMenuIconBox.Checked) CreateStartMenuShortcut();
                        foreach(var box in assocBoxes)
                            box.TryAssignAssoc(executablePath);
                        CreateUninstaller();
                        System.IO.File.WriteAllText(Path.Combine(destinationPath, "Uninstall.inf"), string.Join("\n", installedPaths));
                        System.IO.File.WriteAllBytes(Path.Combine(destinationPath, "Uninstall.exe"), InstallerResource.Uninstaller);
                        EndInstallation();
                    }));
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Unable to read installation archive : {e.Message}", installerName);
                    Invoke(new Action(() => EndInstallation("Installation failed.", "We are sorry, the archive is corrupted or isn't recognized: " + e.Message, true)));
                }
            }
            return Task.CompletedTask;
        }
        #endregion

        #region Utilities
        private void CreateDesktopShortcut()
        {
            string filePath = Path.Combine(destinationPath, exePath);
            object shDesktop = (object)"Desktop";
            WshShell shell = new WshShell();
            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + $"\\{programName}.lnk";
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.Description = $"Opens {programName}";
            // shortcut.Hotkey = "Ctrl+Shift+N";
            shortcut.TargetPath = filePath;
            shortcut.Save();
        }

        private void CreateStartMenuShortcut()
        {
            string pathToExe = Path.Combine(destinationPath, exePath);
            string roamingStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
            string commonStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);
            string appStartMenuPath = Path.Combine(roamingStartMenuPath, "Programs", companyName);
            string appStartMenuPath2 = Path.Combine(commonStartMenuPath, "Programs", companyName);

            if (!Directory.Exists(appStartMenuPath))
                Directory.CreateDirectory(appStartMenuPath);
            if (!Directory.Exists(appStartMenuPath2))
                Directory.CreateDirectory(appStartMenuPath2);

            string[] shortcutLocations = new string[] {
                Path.Combine(appStartMenuPath, programName + ".lnk"),
                Path.Combine(appStartMenuPath2, programName + ".lnk")
            };
            foreach (var location in shortcutLocations)
            {
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(location);

                shortcut.Description = "Opens up " + programName;
                //shortcut.IconLocation = @"C:\Program Files (x86)\TestApp\TestApp.ico"; //uncomment to set the icon of the shortcut
                shortcut.TargetPath = pathToExe;
                shortcut.Save();
            }
            CreateStartMenuShortcutUninstaller();
        }

        private void CreateStartMenuShortcutUninstaller()
        {
            string pathToExe = Path.Combine(destinationPath, "Uninstall.exe");
            string roamingStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
            string commonStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);
            string appStartMenuPath = Path.Combine(roamingStartMenuPath, "Programs", companyName);
            string appStartMenuPath2 = Path.Combine(commonStartMenuPath, "Programs", companyName);

            if (!Directory.Exists(appStartMenuPath))
                Directory.CreateDirectory(appStartMenuPath);
            if (!Directory.Exists(appStartMenuPath2))
                Directory.CreateDirectory(appStartMenuPath2);

            string[] shortcutLocations = new string[] {
                Path.Combine(appStartMenuPath, $"Uninstall {programName}" + ".lnk"),
                Path.Combine(appStartMenuPath2, $"Uninstall {programName}" + ".lnk")
            };
            foreach (var location in shortcutLocations)
            {
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(location);

                shortcut.Description = $"Removes {programName} from your computer";
                //shortcut.IconLocation = @"C:\Program Files (x86)\TestApp\TestApp.ico"; //uncomment to set the icon of the shortcut
                shortcut.TargetPath = pathToExe;
                shortcut.Save();
            }
        }

        private void CreateUninstaller()
        {
            using (RegistryKey parent = Registry.LocalMachine.OpenSubKey(
                         @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", true))
            {
                if (parent == null)
                {
                    //throw new Exception("Uninstall registry key not found.");
                }
                try
                {
                    RegistryKey key = null;

                    try
                    {
                        string guidText = programGUID;
                        key = parent.OpenSubKey(guidText, true) ??
                              parent.CreateSubKey(guidText);

                        if (key == null)
                        {
                            throw new Exception(String.Format("Unable to create uninstaller '{0}\\{1}'", @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", guidText));
                        }

                        string appExe = Path.Combine(destinationPath, exePath);
                        string uninsExe = Path.Combine(destinationPath, "Uninstall.exe");

                        key.SetValue("DisplayName", programName);
                        key.SetValue("ApplicationVersion", appVersion);
                        key.SetValue("Publisher", companyName);
                        key.SetValue("DisplayIcon", appExe);
                        key.SetValue("DisplayVersion", appVersion);
                        key.SetValue("URLInfoAbout", programLink);
                        key.SetValue("InstallDate", DateTime.Now.ToString("yyyyMMdd"));
                        key.SetValue("UninstallString", uninsExe);
                    }
                    finally
                    {
                        if (key != null)
                        {
                            key.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    /*throw new Exception(
                        "An error occurred writing uninstall information to the registry. The service is fully installed but can only be uninstalled manually through the command line.",
                        ex);*/
                }
            }
        }

        public void EndInstallation(string title = null, string desc = null, bool failed = false)
        {
            if (!string.IsNullOrWhiteSpace(title)) finishTitleLabel.Text = title;
            if (!string.IsNullOrWhiteSpace(desc)) finishDescLabel.Text = desc;
            LeftestButton.Visible = false;
            RightButton.Visible = false;
            LeftButton.Enabled = true;
            LeftButton.Text = "Finish";
            if (failed)
            {
                finishLaunchBox.Checked = false;
                finishLaunchBox.Visible = false;
            }
            MoveTab(InstallSection.Finish);
        }

        public void MoveTab(InstallSection section) => MoveTab((int)section);
        public void MoveTab(int index)
        {
            section = (InstallSection)index;
            for (int i = 0; i < sections.Length; i++)
                sections[i].Visible = index == i;
        }
        public void RefreshFreeSpace()
        {
            var root = Path.GetPathRoot(destinationPath);
            try
            {
                DriveInfo dDrive = new DriveInfo(root);
                if (dDrive.IsReady)
                {
                    freeSpace = dDrive.AvailableFreeSpace;
                    freeSpaceLabel.Text = "Available free space: " + BytesToString(freeSpace);
                }
                else
                {
                    freeSpace = 0;
                    freeSpaceLabel.Text = "Available free space: Drive doesn't exist";
                }
            }
            catch
            {
                freeSpace = 0;
                freeSpaceLabel.Text = "Available free space: Invalid path";
            }

            CheckIfEligible();
        }

        public void CheckIfEligible()
        {
            LeftButton.Enabled = archiveSize < freeSpace;
        }

        static string BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return $"{(Math.Sign(byteCount) * num).ToString()} {suf[place]}";
        }
        #endregion
    }

    #region Association
    public class FileAssociation
    {
        public string Extension { get; set; }
        public string ProgId { get; set; }
        public string FileTypeDescription { get; set; }
        public string ExecutableFilePath { get; set; }

        public FileAssociation() { }
        public FileAssociation(string ext, string id, string desc, string path)
        {
            Extension = ext;
            ProgId = id;
            FileTypeDescription = desc;
            ExecutableFilePath = path;
        }
    }

    public class FileAssociations
    {
        // Needed so that Explorer windows get refreshed after the registry is updated
        [DllImport("Shell32.dll")]
        private static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);

        public FileAssociation info;
        private const int SHCNE_ASSOCCHANGED = 0x8000000;
        private const int SHCNF_FLUSH = 0x1000;

        public FileAssociations() { }
        public FileAssociations(FileAssociation info) => this.info = info;
        public FileAssociations(string ext, string id, string desc, string path) => info = new FileAssociation(ext, id, desc, path);

        public void EnsureAssociationsSet()
        {
            EnsureAssociationsSet(info);
        }

        public static void EnsureAssociationsSet(params FileAssociation[] associations)
        {
            bool madeChanges = false;
            foreach (var association in associations)
            {
                madeChanges |= SetAssociation(
                    association.Extension,
                    association.ProgId,
                    association.FileTypeDescription,
                    association.ExecutableFilePath);
            }

            if (madeChanges)
            {
                SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_FLUSH, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public static bool SetAssociation(string extension, string progId, string fileTypeDescription, string applicationFilePath)
        {
            bool madeChanges = false;
            madeChanges |= SetKeyDefaultValue(@"Software\Classes\" + extension, progId);
            madeChanges |= SetKeyDefaultValue(@"Software\Classes\" + progId, fileTypeDescription);
            madeChanges |= SetKeyDefaultValue($@"Software\Classes\{progId}\shell\open\command", "\"" + applicationFilePath + "\" \"%1\"");
            return madeChanges;
        }

        private static bool SetKeyDefaultValue(string keyPath, string value)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(keyPath))
            {
                if (key.GetValue(null) as string != value)
                {
                    key.SetValue(null, value);
                    return true;
                }
            }

            return false;
        }
    }

    public class AssociationCheckBox : CheckBox
    {
        public string extension;
        public string progName;
        public string progDescription;

        public void Prepare(string ext, string name, string desc)
        {
            extension = ext;
            progName = name;
            progDescription = desc;
            Text = $"Open {ext} files with {name}";
        }

        public void TryAssignAssoc(string exePath)
        {
            if(Checked)
                FileAssociations.SetAssociation(extension, progName, progDescription, exePath);
        }
    }
    #endregion

    #region Enums
    public enum InstallSection
    {
        Home = 0,
        Path = 1,
        Options = 2,
        Installation = 3,
        Finish = 4,
        Cancel = 5
    }

    [Flags]
    public enum NativeWindowStyle : uint
    {
        WS_OVERLAPPED = 0x00000000,
        WS_POPUP = 0x80000000,
        WS_CHILD = 0x40000000,
        WS_MINIMIZE = 0x20000000,
        WS_VISIBLE = 0x10000000,
        WS_DISABLED = 0x08000000,
        WS_CLIPSIBLINGS = 0x04000000,
        WS_CLIPCHILDREN = 0x02000000,
        WS_MAXIMIZE = 0x01000000,
        WS_CAPTION = 0x00C00000,
        WS_BORDER = 0x00800000,
        WS_DLGFRAME = 0x00400000,
        WS_VSCROLL = 0x00200000,
        WS_HSCROLL = 0x00100000,
        WS_SYSMENU = 0x00080000,
        WS_THICKFRAME = 0x00040000,
        WS_GROUP = 0x00020000,
        WS_TABSTOP = 0x00010000,
        WS_MINIMIZEBOX = 0x00020000,
        WS_MAXIMIZEBOX = 0x00010000
    }

    public enum NativeWindowLong
    {
        GWL_EXSTYLE = -20,
        GWL_HINSTANCE = -6,
        GWL_HWNDPARENT = -8,
        GWL_ID = -12,
        GWL_STYLE = -16,
        GWL_USERDATA = -21,
        GWL_WNDPROC = -4
    }
    #endregion
}
