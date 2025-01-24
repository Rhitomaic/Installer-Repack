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
            saveBuildDialog.FileName = Project.InstallerName;
            if (saveBuildDialog.ShowDialog() == DialogResult.OK)
            {
                CopyDirectory("OriginalProject", "Cache/Project", true);

                long size = 0;
                foreach (var item in Project.Items)
                {
                    if (item.IsFolder)
                        size += DirSize(new DirectoryInfo(Path.Combine(CurrentDirectory, item.RelativePath)));
                    else
                        size += new FileInfo(Path.Combine(CurrentDirectory, item.RelativePath)).Length;
                }

                var configData = GenerateConfigDataFromProject((int)size);

                var content = File.ReadAllText("Cache/Project/Installer-Repack/MainForm.cs");
                content = content.Replace("{CONFIG_DATA}", configData);
                File.WriteAllText("Cache/Project/Installer-Repack/MainForm.cs", content);

                content = File.ReadAllText("Cache/Project/Uninstaller/MainForm.cs");
                content = content.Replace("{CONFIG_DATA}", configData);
                File.WriteAllText("Cache/Project/Uninstaller/MainForm.cs", content);

                BuildUninstaller("Cache/Project/Installer-Repack/Resources/Uninstaller.exe");
                Build(saveBuildDialog.FileName);

                Directory.Delete("Cache/Project", true);
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

        public void BuildUninstaller(string outputName)
        {
            string csprojPath = "Cache/Project/Uninstaller/Uninstaller.csproj";

            if (!File.Exists(csprojPath))
            {
                Console.WriteLine("Project file not found.");
                return;
            }

            // Read and parse the .csproj file
            XDocument csproj = XDocument.Load(csprojPath);

            // Extract source files
            IEnumerable<string> sourceFiles = csproj
                .Descendants("Compile")
                .Select(e => e.Attribute("Include")?.Value)
                .Where(f => f != null);

            // Extract references
            IEnumerable<string> references = csproj
                .Descendants("Reference")
                .Select(e => e.Attribute("Include")?.Value)
                .Where(r => r != null);

            // Setup compiler parameters
            CompilerParameters parameters = new CompilerParameters
            {
                GenerateExecutable = true,
                OutputAssembly = outputName, // Replace with your output name
                IncludeDebugInformation = true,
            };

            // Add referenced assemblies
            foreach (string reference in references)
            {
                // Resolve reference paths if needed
                string resolvedPath = ResolveReference(reference);
                if (resolvedPath != null)
                {
                    parameters.ReferencedAssemblies.Add(resolvedPath);
                }
            }

            // Read source code from files
            List<string> sourceCode = new List<string>();
            foreach (string file in sourceFiles)
            {
                string filePath = Path.Combine(Path.GetDirectoryName(csprojPath) ?? string.Empty, file);
                if (File.Exists(filePath))
                {
                    sourceCode.Add(File.ReadAllText(filePath));
                }
            }

            Console.WriteLine("Uninstaller Sources: " + sourceCode.Count);

            AppDomain compileDomain = AppDomain.CreateDomain("CompileDomain");

            try
            {
                // Create an instance of the compiler worker in the new domain
                var worker = (CompilerWorker)compileDomain.CreateInstanceAndUnwrap(
                    Assembly.GetExecutingAssembly().FullName,
                    typeof(CompilerWorker).FullName);

                // Compile the assembly in the worker
                bool success = worker.CompileAssembly(parameters, sourceCode.ToArray());
                if (success)
                {
                    Console.WriteLine("Compilation succeeded in another domain.");
                }
                else
                {
                    Console.WriteLine("Compilation failed in another domain.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                // Unload the compile domain
                AppDomain.Unload(compileDomain);
                Console.WriteLine("Compile domain unloaded.");
            }
        }

        public void Build(string outputName)
        {
            long size = 0;
            foreach (var item in Project.Items)
            {
                if (item.IsFolder)
                    size += DirSize(new DirectoryInfo(Path.Combine(CurrentDirectory, item.RelativePath)));
                else
                    size += new FileInfo(Path.Combine(CurrentDirectory, item.RelativePath)).Length;
            }

            string csprojPath = "Cache/Project/Installer-Repack/Installer-Repack.csproj";

            if (!File.Exists(csprojPath))
            {
                Console.WriteLine("Project file not found.");
                return;
            }

            // Read and parse the .csproj file
            XDocument csproj = XDocument.Load(csprojPath);

            // Extract source files
            IEnumerable<string> sourceFiles = csproj
                .Descendants("Compile")
                .Select(e => e.Attribute("Include")?.Value)
                .Where(f => f != null);

            // Extract references
            IEnumerable<string> references = csproj
                .Descendants("Reference")
                .Select(e => e.Attribute("Include")?.Value)
                .Where(r => r != null);

            // Setup compiler parameters
            CompilerParameters parameters = new CompilerParameters
            {
                GenerateExecutable = true,
                OutputAssembly = outputName, // Replace with your output name
                IncludeDebugInformation = true,
            };

            // Add referenced assemblies
            foreach (string reference in references)
            {
                // Resolve reference paths if needed
                string resolvedPath = ResolveReference(reference);
                if (resolvedPath != null)
                {
                    parameters.ReferencedAssemblies.Add(resolvedPath);
                }
            }

            // Read source code from files
            List<string> sourceCode = new List<string>();
            foreach (string file in sourceFiles)
            {
                string filePath = Path.Combine(Path.GetDirectoryName(csprojPath) ?? string.Empty, file);
                if (File.Exists(filePath))
                {
                    sourceCode.Add(File.ReadAllText(filePath));
                }
            }

            Console.WriteLine("Installer Sources: " + sourceCode.Count);
            AppDomain compileDomain = AppDomain.CreateDomain("CompileDomain");

            try
            {
                // Create an instance of the compiler worker in the new domain
                var worker = (CompilerWorker)compileDomain.CreateInstanceAndUnwrap(
                    Assembly.GetExecutingAssembly().FullName,
                    typeof(CompilerWorker).FullName);

                // Compile the assembly in the worker
                bool success = worker.CompileAssembly(parameters, sourceCode.ToArray());
                if (success)
                {
                    Console.WriteLine("Compilation succeeded in another domain.");
                }
                else
                {
                    Console.WriteLine("Compilation failed in another domain.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                // Unload the compile domain
                AppDomain.Unload(compileDomain);
                Console.WriteLine("Compile domain unloaded.");
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

        public class CompilerWorker : MarshalByRefObject
        {
            public bool CompileAssembly(CompilerParameters parameters, string[] sourceCode)
            {
                using (CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    CompilerResults result = provider.CompileAssemblyFromSource(parameters, sourceCode);

                    if (result.Errors.HasErrors)
                    {
                        Console.WriteLine("Compilation failed:");
                        foreach (CompilerError error in result.Errors)
                        {
                            Console.WriteLine($"  {error}");
                        }
                        return false;
                    }
                    else
                    {
                        Console.WriteLine("Compilation succeeded!");
                        return true;
                    }
                }
            }
        }

        private static string ResolveReference(string referenceName)
        {
            // Implement logic to resolve reference paths based on your project's requirements.
            // This might include checking the GAC, NuGet packages, or specific paths.
            return $"{referenceName}.dll"; // Example: returning DLL name directly.
        }

        public string GenerateConfigDataFromProject(int archiveSize)
        {
            var builder = new StringBuilder();
            builder.AppendLine("\t\tpublic const string programName = \"" + JsonConvert.SerializeObject(Project.ProgramName) + "\";");
            builder.AppendLine("\t\tconst string companyName = \"" + JsonConvert.SerializeObject(Project.CompanyName) + "\";");
            builder.AppendLine("\t\tconst string appVersion = \"" + JsonConvert.SerializeObject(Project.Version) + "\";");
            builder.AppendLine("\t\tconst string programLink = \"" + JsonConvert.SerializeObject(Project.Link) + "\";");

            builder.AppendLine("\t\tconst string installerName = \"" + JsonConvert.SerializeObject(Project.InstallerName) + "\";");
            builder.AppendLine("\t\tconst string defaultDestinationPath = \"" + JsonConvert.SerializeObject(Project.DefaultDirectory) + "\";");
            builder.AppendLine("\t\tconst string exePath = \"" + JsonConvert.SerializeObject(Project.ExecutablePath) + "\";");
            builder.AppendLine("\t\tconst string programGUID = \"" + JsonConvert.SerializeObject(Project.Guid) + "\";");

            // Associations
            builder.AppendLine("\t\tstatic Dictionary<string, string> associations = new Dictionary<string, string>()\r\n\t\t{");
            foreach (var association in Project.FileAssociations)
            {
                builder.AppendLine("\t\t\t{ \"" + association.Extension + "\", \"" + JsonConvert.SerializeObject(association.Description) + "\" },");
            }
            builder.AppendLine("\t\t};");

            builder.AppendLine("\t\tconst long archiveSize = " + archiveSize + ";");
            return builder.ToString();
        }

        public string ResolveDirectoryPath(string path)
        {
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

                foreach (var item in Project.Items)
                    AddItem(item);
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
                Project.InstallerIcon = GetRelativePath(openIconDialog.FileName, CurrentDirectory);
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
