using System;
using System.IO;
using System.Windows;
using System.Diagnostics;

namespace LittleBustersScriptModifier
{
    public partial class MainWindow : Window
    {
        private string _configFilePath = "config.txt";
        private string _backupFolderPath = "backup";
        private string _litbusFolderPath = "";

        public MainWindow()
        {
            InitializeComponent();
            LoadConfig();
            CheckLitbusFolder();
        }

        private void LoadConfig()
        {
            if (File.Exists(_configFilePath))
            {
                _litbusFolderPath = File.ReadAllText(_configFilePath);
                if (!Directory.Exists(_litbusFolderPath))
                {
                    _litbusFolderPath = "";
                }
            }
        }

        private void CheckLitbusFolder()
        {
            if (string.IsNullOrEmpty(_litbusFolderPath) || !Directory.Exists(_litbusFolderPath))
            {
                var result = MessageBox.Show("Please select the folder containing LITBUS_WIN32.exe", "Error", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    var dialog = new System.Windows.Forms.OpenFileDialog();
                    dialog.Title = "Select LITBUS_WIN32.exe";
                    dialog.Filter = "Executable files (*.exe)|*.exe";
                    var result2 = dialog.ShowDialog();
                    if (result2 == System.Windows.Forms.DialogResult.OK)
                    {
                        _litbusFolderPath = Path.GetDirectoryName(dialog.FileName);
                        File.WriteAllText(_configFilePath, _litbusFolderPath);
                    }
                }
            }
        }

        private void BackupScriptPak()
        {
            var filesPath = Path.Combine(_litbusFolderPath, "files");
            if (!Directory.Exists(filesPath))
            {
                MessageBox.Show("Could not find files folder in Little Busters! English Edition folder", "Error", MessageBoxButton.OK);
                return;
            }

            var scriptPakPath = Path.Combine(filesPath, "SCRIPT.PAK");
            if (!File.Exists(scriptPakPath))
            {
                MessageBox.Show("Could not find SCRIPT.PAK in files folder", "Error", MessageBoxButton.OK);
                return;
            }

            var backupPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _backupFolderPath);
            if (!Directory.Exists(backupPath))
            {
                Directory.CreateDirectory(backupPath);
            }

            var backupScriptPakPath = Path.Combine(backupPath, "SCRIPT.PAK");
            bool shouldCopyScriptPak = true;

            if (File.Exists(backupScriptPakPath))
            {
                var result = MessageBox.Show("A backup of SCRIPT.PAK already exists. Do you want to overwrite it?", "Warning", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                {
                    var result2 = MessageBox.Show("Do you want to extract the contents of the existing backup of SCRIPT.PAK?", "Warning", MessageBoxButton.YesNo);
                    if (result2 == MessageBoxResult.Yes)
                    {
                        shouldCopyScriptPak = false;
                    }
                    else
                    {
                        return;
                    }
                }
            }

            if (shouldCopyScriptPak)
            {
                File.Copy(scriptPakPath, backupScriptPakPath, true);
                MessageBox.Show("Backup created successfully", "Success", MessageBoxButton.OK);
            }

            var extractFolderPath = Path.Combine(backupPath, "extract");
            if (!Directory.Exists(extractFolderPath))
            {
                Directory.CreateDirectory(extractFolderPath);
            }

            var luckSystemExePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dependencies", "LuckSystem.exe");
            if (!File.Exists(luckSystemExePath))
            {
                MessageBox.Show("Could not find LuckSystem.exe in dependencies folder", "Error", MessageBoxButton.OK);
                return;
            }

            var arguments = $"pak extract -i \"{backupScriptPakPath}\" -o \"{Path.Combine(backupPath, "extract", "wow")}\" --all \"{Path.Combine(backupPath, "extract")}\"";

            var processStartInfo = new ProcessStartInfo(luckSystemExePath, arguments);

            processStartInfo.UseShellExecute = false;

            processStartInfo.CreateNoWindow = true;

            processStartInfo.RedirectStandardOutput = true;

            using (var process = new Process())
            {
                process.StartInfo = processStartInfo;
                process.Start();
                process.WaitForExit();

                var wowFilePath = Path.Combine(extractFolderPath, "wow");

                if (File.Exists(wowFilePath))
                {
                    File.Delete(wowFilePath);
                }
                RunLucaSystemTools();
                return;
            }
        }
        private void RunLucaSystemTools()
        {
            var backupPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _backupFolderPath);
            if (!Directory.Exists(backupPath))
            {
                MessageBox.Show("Could not find backup folder", "Error", MessageBoxButton.OK);
                return;
            }

            var lucaSystemToolsExePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dependencies", "LucaSystemTools.exe");
            if (!File.Exists(lucaSystemToolsExePath))
            {
                MessageBox.Show("Could not find LucaSystemTools.exe in dependencies folder", "Error", MessageBoxButton.OK);
                return;
            }

            var exportFolderPath = Path.Combine(backupPath, "extract");
            if (!Directory.Exists(exportFolderPath))
            {
                MessageBox.Show("Could not find extract folder in backup folder", "Error", MessageBoxButton.OK);
                return;
            }

            var convertFolderPath = Path.Combine(backupPath, "convert");
            if (!Directory.Exists(convertFolderPath))
            {
                Directory.CreateDirectory(convertFolderPath);
            }

            foreach (var file in Directory.GetFiles(exportFolderPath))
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);

                var argumentsForLucaSystemToolsExe = $"-t scr -m export -f \"{file}\" -o \"{Path.Combine(convertFolderPath, fileNameWithoutExtension)}\" -opcode LB_EN -json";

                var processStartInfo = new ProcessStartInfo(lucaSystemToolsExePath, argumentsForLucaSystemToolsExe);

                processStartInfo.UseShellExecute = false;

                processStartInfo.CreateNoWindow = true;

                processStartInfo.RedirectStandardOutput = true;

                using (var process = new Process())
                {
                    process.StartInfo = processStartInfo;
                    process.Start();
                    process.WaitForExit();

                    continue;
                }
            }
            StatusTextBlock.Text = "Convert Complete!";
        }

        private void BackupButton_Click(object sender, RoutedEventArgs e)
        {
            StatusTextBlock.Text = "Backup Complete!";
            BackupScriptPak();
        }

        private void ChangeDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Title = "Select LITBUS_WIN32.exe";
            dialog.Filter = "Executable files (*.exe)|*.exe";
            var result2 = dialog.ShowDialog();
            if (result2 == System.Windows.Forms.DialogResult.OK)
            {
                _litbusFolderPath = Path.GetDirectoryName(dialog.FileName);
                File.WriteAllText(_configFilePath, _litbusFolderPath);
                CheckLitbusFolder();
            }
        }

        private void RunLuckSystemAndLucaSystemTools()
        {
            var backupPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _backupFolderPath);
            if (!Directory.Exists(backupPath))
            {
                MessageBox.Show("Could not find backup folder", "Error", MessageBoxButton.OK);
                return;
            }

            var lucaSystemToolsExePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dependencies", "LucaSystemTools.exe");
            if (!File.Exists(lucaSystemToolsExePath))
            {
                MessageBox.Show("Could not find LucaSystemTools.exe in dependencies folder", "Error", MessageBoxButton.OK);
                return;
            }

            var luckSystemExePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dependencies", "LuckSystem.exe");
            if (!File.Exists(luckSystemExePath))
            {
                MessageBox.Show("Could not find LuckSystem.exe in dependencies folder", "Error", MessageBoxButton.OK);
                return;
            }

            var convertFolderPath = Path.Combine(backupPath, "convert");
            if (!Directory.Exists(convertFolderPath))
            {
                MessageBox.Show("Could not find convert folder in backup folder", "Error", MessageBoxButton.OK);
                return;
            }

            var workingFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "working");
            if (!Directory.Exists(workingFolderPath))
            {
                Directory.CreateDirectory(workingFolderPath);
            }

            foreach (var file in Directory.GetFiles(convertFolderPath))
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);

                var argumentsForLucaSystemToolsExe = $"-t scr -m import -f \"{file}\" -o \"{Path.Combine(workingFolderPath, fileNameWithoutExtension)}\" -opcode LB_EN -json";

                var processStartInfo = new ProcessStartInfo(lucaSystemToolsExePath, argumentsForLucaSystemToolsExe);

                processStartInfo.UseShellExecute = false;

                processStartInfo.CreateNoWindow = true;

                processStartInfo.RedirectStandardOutput = true;

                using (var process = new Process())
                {
                    process.StartInfo = processStartInfo;
                    process.Start();
                    process.WaitForExit();
                }
            }

            var scriptPakPathInFilesFolder = Path.Combine(_litbusFolderPath, "files", "SCRIPT.PAK");
            if (!File.Exists(scriptPakPathInFilesFolder))
            {
                MessageBox.Show("Could not find SCRIPT.PAK in files folder next to LITBUS_WIN32.exe", "Error", MessageBoxButton.OK);
                return;
            }

            var scriptPakOutputPathInFilesFolder = Path.Combine(_litbusFolderPath, "files", "SCRIPT_OUTPUT.PAK");

            foreach (var file in Directory.GetFiles(workingFolderPath))
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);

                var argumentsForLuckSystemExe = $"pak replace -s \"{scriptPakPathInFilesFolder}\" -o \"{scriptPakOutputPathInFilesFolder}\" -i \"{file}\" -n \"{fileNameWithoutExtension}\"";

                var processStartInfo = new ProcessStartInfo(luckSystemExePath, argumentsForLuckSystemExe);

                processStartInfo.UseShellExecute = false;

                processStartInfo.CreateNoWindow = true;

                processStartInfo.RedirectStandardOutput = true;

                using (var process = new Process())
                {
                    process.StartInfo = processStartInfo;
                    process.Start();
                    process.WaitForExit();
                }

                File.Delete(scriptPakPathInFilesFolder);

                File.Move(scriptPakOutputPathInFilesFolder, scriptPakPathInFilesFolder);
            }
        }
        private void RunLuckSystemAndLucaSystemToolsButton_Click(object sender, RoutedEventArgs e)
        {
            var result3 = MessageBox.Show("Are you sure you want to push your changes? This will modify SCRIPT.PAK.", "Warning", MessageBoxButton.YesNo);
            if (result3 == MessageBoxResult.No)
            {
                return;
            }
            StatusTextBlock.Text = "Import Complete!";
            RunLuckSystemAndLucaSystemTools();
        }

        private void RestoreBackup()
        {
            var result = MessageBox.Show("Are you sure you want to restore the backup of SCRIPT.PAK?", "Warning", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                return;
            }

            var backupPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _backupFolderPath);
            if (!Directory.Exists(backupPath))
            {
                MessageBox.Show("Could not find backup folder", "Error", MessageBoxButton.OK);
                return;
            }

            var backupScriptPakPath = Path.Combine(backupPath, "SCRIPT.PAK");
            if (!File.Exists(backupScriptPakPath))
            {
                MessageBox.Show("Could not find backup of SCRIPT.PAK in backup folder", "Error", MessageBoxButton.OK);
                return;
            }

            var filesPath = Path.Combine(_litbusFolderPath, "files");
            if (!Directory.Exists(filesPath))
            {
                MessageBox.Show("Could not find files folder in Little Busters! English Edition folder", "Error", MessageBoxButton.OK);
                return;
            }

            var scriptPakPathInFilesFolder = Path.Combine(filesPath, "SCRIPT.PAK");

            File.Copy(backupScriptPakPath, scriptPakPathInFilesFolder, true);

            MessageBox.Show("Backup restored successfully", "Success", MessageBoxButton.OK);
        }

        private void RestoreBackupButton_Click(object sender, RoutedEventArgs e)
        {
            RestoreBackup();
        }
    }
}