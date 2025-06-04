using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.CodeDom.Compiler;
using System.Reflection;

using Microsoft.CSharp;
using Newtonsoft.Json;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Xml.Linq;
using System.Diagnostics;
using Ionic.Zip;

namespace Creator
{
    public partial class MainForm : Form
    {
        public static string CurrentPath { get; set; }
        public static string CurrentDirectory { get; set; }

        public static InstallerProject Project = new InstallerProject();

        public MainForm(string[] args)
        {
            InitializeComponent();

            foreach (var arg in args)
            {
                if (File.Exists(arg) && Path.GetExtension(arg) == ".irp")
                    LoadProject(arg);
            }
        }

        private void itGenerateBtn_Click(object sender, EventArgs e)
        {
            itGuidBox.Text = $"{{{Guid.NewGuid().ToString()}}}";
        }

        private void buildBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CurrentPath))
                saveBuildDialog.InitialDirectory = CurrentDirectory;
            saveBuildDialog.FileName = Project.InstallerName + Project.Version;
            if (saveBuildDialog.ShowDialog() == DialogResult.OK)
            {
                var dialog = new LoadingForm();
                dialog.Show();

                Task.Run(async () => {
                    Invoke(new Action(() => { 
                        Enabled = false;
                        dialog?.SetStatus("Copying original project to cache...");
                    }));

                    try
                    {
                        CopyDirectory("OriginalProject", "Cache/Project", true);
                    }
                    catch (Exception ex)
                    {
                        Invoke(new Action(() => {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Enabled = true;
                            dialog?.Close();
                        }));
                        return;
                    }

                    Invoke(new Action(() => {
                        dialog?.SetProgress(0.05f);
                        dialog?.SetStatus("Counting archive size...");
                    }));
                    long size = 0;
                    try
                    {
                        foreach (var item in Project.Items)
                        {
                            if (item.IsFolder)
                                size += DirSize(new DirectoryInfo(Path.Combine(CurrentDirectory, item.RelativePath)));
                            else
                                size += new FileInfo(Path.Combine(CurrentDirectory, item.RelativePath)).Length;
                        }
                    }
                    catch (Exception ex)
                    {
                        Invoke(new Action(() => {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Enabled = true;
                            dialog?.Close();
                        }));
                        return;
                    }

                    Invoke(new Action(() => {
                        dialog?.SetProgress(0.1f);
                        dialog?.SetStatus("Modifying C# sources...");
                    }));
                    try
                    {
                        var configData = GenerateConfigDataFromProject((int)size);

                        var content = File.ReadAllText("Cache/Project/Installer-Repack/MainForm.cs");
                        content = content.Replace("{CONFIG_DATA}", configData);
                        File.WriteAllText("Cache/Project/Installer-Repack/MainForm.cs", content);

                        content = File.ReadAllText("Cache/Project/Uninstaller/MainForm.cs");
                        content = content.Replace("{CONFIG_DATA}", configData);
                        File.WriteAllText("Cache/Project/Uninstaller/MainForm.cs", content);

                        Invoke(new Action(() => {
                            dialog?.SetProgress(0.15f);
                            dialog?.SetStatus("Packaging files...");
                        }));

                        Console.WriteLine("Packaging files...");
                        if (File.Exists("Cache/Archive.zip")) File.Delete("Cache/Archive.zip");
                        using (var zip = new ZipFile("Cache/Archive.zip"))
                        {
                            int i = 0;
                            foreach (var item in Project.Items)
                            {
                                Invoke(new Action(() => {
                                    dialog?.SetProgress(0.15f, i, Project.Items.Count, 0.35f);
                                    dialog?.SetStatus("Packaging: " + item.RelativePath);
                                }));

                                var targetPath = Path.Combine(CurrentDirectory, item.RelativePath);
                                if (item.IsFolder)
                                    zip.AddDirectory(targetPath, Path.GetFileName(item.RelativePath));
                                else
                                    zip.AddFile(targetPath, "/");

                                await Task.Delay(50);

                                i++;
                            }

                            Invoke(new Action(() => {
                                dialog?.SetProgress(0.5f);
                                dialog?.SetStatus("Saving archive...");
                            }));
                            zip.Save();
                        }

                        Invoke(new Action(() => {
                            dialog?.SetProgress(0.55f);
                            dialog?.SetStatus("Copying archive as a resource...");
                        }));

                        await Task.Delay(200);

                        var archiveCopyTarget = "Cache/Project/Installer-Repack/Resources/InstallationArchive.zip";
                        if (File.Exists(archiveCopyTarget)) File.Delete(archiveCopyTarget);
                        File.Copy("Cache/Archive.zip", archiveCopyTarget);

                        var iconPath = Path.Combine(CurrentDirectory, Project.InstallerIcon);
                        if (File.Exists(iconPath))
                        {
                            Invoke(new Action(() => {
                                dialog?.SetProgress(0.6f);
                                dialog?.SetStatus("Copying installer icon as a resource...");
                            }));

                            var iconCopyTarget = "Cache/Project/Installer-Repack/Resources/Icon.ico";
                            if (File.Exists(iconCopyTarget)) File.Delete(iconCopyTarget);
                            File.Copy(iconPath, iconCopyTarget);
                            await Task.Delay(100);
                        }

                        Invoke(new Action(() => {
                            dialog?.SetProgress(0.65f);
                            dialog?.SetStatus("Building uninstaller...");
                        }));
                        BuildProject("Cache/Project/Uninstaller/Uninstaller.csproj");

                        Invoke(new Action(() => {
                            dialog?.SetProgress(0.75f);
                            dialog?.SetStatus("Copying uninstaller as a resource...");
                        }));

                        var uninstallerCopyTarget = "Cache/Project/Installer-Repack/Resources/Uninstaller.exe";
                        if (File.Exists(uninstallerCopyTarget)) File.Delete(uninstallerCopyTarget);
                        File.Copy("Cache/Project/Uninstaller/bin/Release/Uninstaller.exe", uninstallerCopyTarget);

                        Invoke(new Action(() => {
                            dialog?.SetProgress(0.85f);
                            dialog?.SetStatus("Building installer...");
                        }));
                        Console.WriteLine("Building Installer...");
                        BuildProject("Cache/Project/Installer-Repack/Installer-Repack.csproj");

                        Invoke(new Action(() => {
                            dialog?.SetProgress(0.9f);
                            dialog?.SetStatus("Copying result to target path...");
                        }));
                        Console.WriteLine("Copying result to target path...");
                        if (File.Exists(saveBuildDialog.FileName)) File.Delete(saveBuildDialog.FileName);
                        File.Copy("Cache/Project/Installer-Repack/bin/Release/Installer-Repack.exe", saveBuildDialog.FileName);

                        Invoke(new Action(() => {
                            dialog?.SetProgress(1f);
                            dialog?.SetStatus("Clearing cache...");
                        }));
                        Directory.Delete("Cache", true);

                        Invoke(new Action(() => {
                            Enabled = true;
                            dialog?.Close();

                            if (MessageBox.Show("Build finished! Do you want to run the installer now?", "Information", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                if (File.Exists(saveBuildDialog.FileName))
                                    Process.Start(saveBuildDialog.FileName);
                                else
                                    MessageBox.Show("Oh actually, we can't find the installer...", "Confusion");
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        Invoke(new Action(() => {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Enabled = true;
                            dialog?.Close();
                        }));
                        return;
                    }
                });
            }
        }
        static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                if (File.Exists(targetFilePath)) File.Delete(targetFilePath);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }

        public void BuildProject(string csprojPath)
        {
            if (!File.Exists(csprojPath))
            {
                throw new FileNotFoundException($"The specified project file was not found: {csprojPath}");
            }

            var msbuildPath = GetMSBuildPathUsingVsWhere();
            if (string.IsNullOrEmpty(msbuildPath))
            {
                throw new InvalidOperationException("MSBuild.exe could not be located. Ensure Visual Studio is installed.");
            }

            string arguments = $"\"{csprojPath}\" /p:Configuration=Release";

            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = msbuildPath,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    File.WriteAllText("Build.log", output);
                    throw new InvalidOperationException($"MSBuild failed with the following error:\n{error}\nand output of:\n{output}");
                }

                Console.WriteLine("MSBuild Output:");
                Console.WriteLine(output);
            }
        }

        private static string GetMSBuildPathUsingVsWhere()
        {
            string vswherePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                "Microsoft Visual Studio", "Installer", "vswhere.exe");

            if (!File.Exists(vswherePath))
            {
                throw new FileNotFoundException($"vswhere.exe not found at: {vswherePath}");
            }

            // Command to find MSBuild using vswhere
            string arguments = "-latest -requires Microsoft.Component.MSBuild -find MSBuild\\**\\Bin\\MSBuild.exe";

            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = vswherePath,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                process.Start();

                // Read the standard output and error
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    // Output should contain the MSBuild path
                    return output.Trim();
                }
                else
                {
                    throw new InvalidOperationException(
                        $"vswhere execution failed with error: {error}\nand output: {output}");
                }
            }
        }

        public static long DirSize(DirectoryInfo d)
        {
            long size = 0;
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += DirSize(di);
            }
            return size;
        }

        public string GenerateConfigDataFromProject(int archiveSize)
        {
            var builder = new StringBuilder();
            builder.AppendLine("\t\tpublic const string programName = " + JsonConvert.SerializeObject(Project.ProgramName) + ";");
            builder.AppendLine("\t\tconst string companyName = " + JsonConvert.SerializeObject(Project.CompanyName) + ";");
            builder.AppendLine("\t\tconst string appVersion = " + JsonConvert.SerializeObject(Project.Version) + ";");
            builder.AppendLine("\t\tconst string programLink = " + JsonConvert.SerializeObject(Project.Link) + ";");

            builder.AppendLine("\t\tconst string installerName = " + JsonConvert.SerializeObject(Project.InstallerName) + ";");
            builder.AppendLine("\t\tstatic string defaultDestinationPath = " + JsonConvert.SerializeObject(Project.DefaultDirectory) + ";");
            builder.AppendLine("\t\tconst string exePath = " + JsonConvert.SerializeObject(Project.ExecutablePath) + ";");
            builder.AppendLine("\t\tconst string programGUID = " + JsonConvert.SerializeObject(Project.Guid) + ";");

            // File Associations
            builder.AppendLine("\t\tstatic Dictionary<string, string> associations = new Dictionary<string, string>()\r\n\t\t{");
            foreach (var association in Project.FileAssociations)
            {
                builder.AppendLine("\t\t\t{ \"" + association.Extension + "\", " + JsonConvert.SerializeObject(association.Description) + " },");
            }
            builder.AppendLine("\t\t};");

            // Url Associations
            builder.AppendLine("\t\tstatic string[] urlProtocols = \r\n\t\t{");
            foreach (var association in Project.UrlAssociations)
            {
                builder.AppendLine("\t\t\t\"" + association + "\",");
            }
            builder.AppendLine("\t\t};");

            builder.AppendLine("\t\tconst long archiveSize = " + archiveSize + ";");
            return builder.ToString();
        }

        public string ResolveDirectoryPath(string path)
        {
            path = path.Replace("%UserDocuments%", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            path = path.Replace("%UserProfile%", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            path = path.Replace("%LocalAppData%", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            path = path.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            path = path.Replace("%ProgramFiles%", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
            path = path.Replace("%ProgramFilesX86%", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86));
            return path;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CurrentPath))
            {
                saveInstallerDialog.FileName = Project.ProgramName + "Installer";
                if (saveInstallerDialog.ShowDialog() == DialogResult.OK)
                {
                    Refresh();

                    string previousPath = CurrentPath;
                    string previousDirectory = CurrentDirectory;
                    try
                    {
                        CurrentPath = saveInstallerDialog.FileName;
                        CurrentDirectory = Path.GetDirectoryName(saveInstallerDialog.FileName);

                        var json = JsonConvert.SerializeObject(Project, Formatting.Indented);
                        File.WriteAllText(saveInstallerDialog.FileName, json);
                    }
                    catch (Exception ex)
                    {
                        CurrentPath = previousPath;
                        CurrentDirectory = previousDirectory;
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                Refresh();
                var json = JsonConvert.SerializeObject(Project, Formatting.Indented);
                File.WriteAllText(CurrentPath, json);
            }
        }

        private void loadBtn_Click(object sender, EventArgs e)
        {
            openInstallerDialog.FileName = Project.ProgramName + "Installer";
            if (openInstallerDialog.ShowDialog() == DialogResult.OK)
            {
                LoadProject(openInstallerDialog.FileName);
            }
        }

        public void LoadProject(string path)
        {
            var json = File.ReadAllText(path);
            Project = JsonConvert.DeserializeObject<InstallerProject>(json);

            CurrentPath = path;
            CurrentDirectory = Path.GetDirectoryName(path);
            Refresh(true);
        }

        public void Refresh(bool fromProject = false)
        {
            if (fromProject)
            {
                fFilesList.Items.Clear();
                fileTypesList.Items.Clear();
                urlProtocolList.Items.Clear();

                Project.ConvertItemsAsRelative(CurrentDirectory);

                gProgramNameBox.Text = Project.ProgramName;
                gCompanyNameBox.Text = Project.CompanyName;
                gVersionBox.Text = Project.Version;
                gLinkBox.Text = Project.Link;

                itDefaultDirBox.Text = Project.DefaultDirectory;
                itExecPathBox.Text = Project.ExecutablePath;
                itGuidBox.Text = Project.Guid;

                irNameBox.Text = Project.InstallerName;
                irIconBox.Text = Project.InstallerIcon;

                var iconPath = Path.Combine(CurrentDirectory, Project.InstallerIcon);
                if (File.Exists(iconPath))
                    irIconBox.Image = new Icon(iconPath).ToBitmap();

                foreach (var item in Project.Items)
                    AddItem(item);

                foreach (var item in Project.FileAssociations)
                    AddFileTypeItem(item);

                foreach (var item in Project.UrlAssociations)
                    AddUrlProtocolItem(item);
            }
            else
            {
                Project.ConvertItemsAsRelative(CurrentDirectory);

                Project.ProgramName = gProgramNameBox.Text;
                Project.CompanyName = gCompanyNameBox.Text;
                Project.Version = gVersionBox.Text;
                Project.Link = gLinkBox.Text;

                Project.DefaultDirectory = itDefaultDirBox.Text;
                Project.ExecutablePath = itExecPathBox.Text;
                Project.Guid = itGuidBox.Text;

                Project.InstallerName = irNameBox.Text;
                Project.InstallerIcon = irIconBox.Text;

                foreach (ListViewItem item in fFilesList.Items)
                {
                    var installerItem = item.Tag as InstallerItem;
                    installerItem.RelativePath = item.SubItems[1].Text;
                    installerItem.IsFolder = item.SubItems[2].Text == "Folder";
                }
            }
        }

        public ListViewItem AddItem(InstallerItem item)
        {
            var listItem = new ListViewItem(
                new string[] { 
                    Path.GetFileName(item.RelativePath),
                    item.RelativePath,
                    item.IsFolder ? "Folder" : "File"
                }
            );

            listItem.Tag = item;
            fFilesList.Items.Add(listItem);
            return listItem;
        }

        private void irIconBox_Click(object sender, EventArgs e)
        {
            if (openIconDialog.ShowDialog() == DialogResult.OK)
            {
                var icon = new Icon(openIconDialog.FileName);
                irIconBox.Image = icon.ToBitmap();
                Project.InstallerIcon = GetRelativePath(openIconDialog.FileName, CurrentDirectory);
                irIconBox.Text = Project.InstallerIcon;
            }
        }

        private void fAddFilesBtn_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog() { 
                Title = "Select files you want to add to the installer...",
                Filter = "All files (*.*)|*.*",
                Multiselect = true,
                RestoreDirectory = true
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var fileName in dialog.FileNames)
                {
                    var item = new InstallerItem(GetRelativePath(fileName, CurrentDirectory), false);
                    if (!Project.Items.Exists(val => val.RelativePath == item.RelativePath))
                    {
                        Project.Items.Add(item);
                        AddItem(item);
                    }
                }
            }
        }

        private void fAddFoldersBtn_Click(object sender, EventArgs e)
        {
            var dialog = new CommonOpenFileDialog()
            {
                Title = "Select folders you want to add to the installer...",
                IsFolderPicker = true,
                Multiselect = true
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                foreach (var folderName in dialog.FileNames)
                {
                    var item = new InstallerItem(GetRelativePath(folderName, CurrentDirectory), true);
                    if (!Project.Items.Exists(val => val.RelativePath == item.RelativePath))
                    {
                        Project.Items.Add(item);
                        AddItem(item);
                    }
                }
            }
        }

        public static string GetRelativePath(string path, string folder)
        {
            if (folder == null || path == null)
                return path;

            if (path == folder)
                return ".";

            if (path.Length < 2 || path[1] != ':')
                return path;

            Uri pathUri = new Uri(path);

            // Folders must end in a slash
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
                folder += Path.DirectorySeparatorChar;

            Uri folderUri = new Uri(folder);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedItems = new List<ListViewItem>(fFilesList.SelectedItems.Cast<ListViewItem>());
            foreach (ListViewItem item in selectedItems)
            {
                Project.Items.Remove(item.Tag as InstallerItem);
                fFilesList.Items.Remove(item);
            }
        }

        private void fileTypeAddBtn_Click(object sender, EventArgs e)
        {
            if (Project.FileAssociations.Exists(val => val.Extension == fileTypeExtBox.Text))
                return;

            var assoc = new InstallerFileAssoc(fileTypeExtBox.Text, fileTypeDescBox.Text);
            Project.FileAssociations.Add(assoc);
            AddFileTypeItem(assoc);
        }

        public void AddFileTypeItem(InstallerFileAssoc assoc)
        {
            var item = new ListViewItem(new string[] {
                assoc.Extension,
                assoc.Description
            })
            {
                Tag = assoc
            };
            fileTypesList.Items.Add(item);
        }

        private void urlProtocolAddBtn_Click(object sender, EventArgs e)
        {
            if (Project.UrlAssociations.Contains(urlAssocTextBox.Text))
                return;

            Project.UrlAssociations.Add(urlAssocTextBox.Text);
            AddUrlProtocolItem(urlAssocTextBox.Text);
        }

        public void AddUrlProtocolItem(string item)
        {
            urlProtocolList.Items.Add(item);
        }

        private void removeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var items = new List<ListViewItem>(fileTypesList.SelectedItems.Cast<ListViewItem>());
            foreach (var item in items)
            {
                Project.FileAssociations.Remove(item.Tag as InstallerFileAssoc);
                fileTypesList.Items.Remove(item);
            }
        }

        private void removeToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            var items = new List<string>(urlProtocolList.SelectedItems.Cast<string>());
            foreach (var item in items)
            {
                Project.UrlAssociations.Remove(item);
                urlProtocolList.Items.Remove(item);
            }
        }
    }

    public class InstallerProject
    {
        public string ProgramName { get; set; }
        public string CompanyName { get; set; }
        public string Version { get; set; }
        public string Link { get; set; }

        public string DefaultDirectory { get; set; }
        public string ExecutablePath { get; set; }
        public string Guid { get; set; }

        public string InstallerName { get; set; }
        public string InstallerIcon { get; set; }

        public List<InstallerItem> Items { get; set; } = new List<InstallerItem>();

        public List<string> UrlAssociations { get; set; } = new List<string>();

        public List<InstallerFileAssoc> FileAssociations { get; set; } = new List<InstallerFileAssoc>();

        public void ConvertItemsAsRelative(string folder)
        {
            if (Items == null)
                Items = new List<InstallerItem>();

            foreach (var item in Items)
                item.RelativePath = MainForm.GetRelativePath(item.RelativePath, folder);
        }
    }

    public class InstallerFileAssoc
    {
        public string Extension { get; set; }
        public string Description { get; set; }

        public InstallerFileAssoc() { }
        public InstallerFileAssoc(string extension, string description) { Extension = extension; Description = description; }
    }

    public class InstallerItem
    {
        public string RelativePath { get; set; }
        public bool IsFolder { get; set; }

        public InstallerItem() { }
        public InstallerItem(string relativePath, bool isFolder) { RelativePath = relativePath; IsFolder = isFolder; }
    }
}
