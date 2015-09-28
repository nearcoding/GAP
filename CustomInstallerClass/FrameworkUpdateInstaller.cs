using Microsoft.Win32;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace CustomInstallerClass
{
    [RunInstaller(true)]
    public partial class FrameworkUpdateInstaller : System.Configuration.Install.Installer
    {
        public FrameworkUpdateInstaller()
        {
            InitializeComponent();
            string folderName = string.Empty;

            folderName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            folderName = folderName + "\\GAP";
            if (Directory.Exists(folderName))
                Directory.Delete(folderName);

            Directory.CreateDirectory(folderName);
        }
        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);
            // get the .NET runtime string, and add the ngen exe at the end.
            string runtimeStr = RuntimeEnvironment.GetRuntimeDirectory();
            string ngenStr = Path.Combine(runtimeStr, "ngen.exe");

            Process process = new Process();
            process.StartInfo.FileName = ngenStr;

            string assemblyPath = Context.Parameters["assemblypath"];
            assemblyPath = Path.GetDirectoryName(assemblyPath);
            assemblyPath = assemblyPath + "\\" + "GAP.exe";
            System.Windows.Forms.MessageBox.Show(assemblyPath);
            process.StartInfo.Arguments = "install \"" + assemblyPath + "\"";

            process.Start();
            process.WaitForExit();

        }
        protected override void OnBeforeInstall(IDictionary savedState)
        {
            base.OnBeforeInstall(savedState);

            //here we need to check if the key exists           
            string keyToFind = string.Empty;
            if (Environment.Is64BitOperatingSystem)
                keyToFind = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Updates\Microsoft .NET Framework 4 Extended\KB2468871";
            else
                keyToFind = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Updates\Microsoft .NET Framework 4 Extended\KB2468871";
            //Registry registryKey = Registry.LocalMachine.OpenSubKey(keyToFind,"ThisVersionInstalled");
            if (Registry.GetValue(keyToFind, "ThisVersionInstalled", "Y") == null)
            {
                //MessageBox.Show("A mandatory requirement is missing in the system. That update is included in this setup package and will be installed now", "Data Log", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //var tempExeName = Path.Combine(Directory.GetCurrentDirectory(), "UpdateInstaller.exe");
                //using (FileStream stream = new FileStream(tempExeName, FileMode.Create, FileAccess.Write))
                //{
                //    byte[] fileToWrite = Properties.Resources.GetExecutable();
                //    stream.Write(fileToWrite, 0, fileToWrite.Length);
                //}
                //if (File.Exists(tempExeName))
                //{
                //    Process.Start(tempExeName);
                //}
            }

        }
    }//end class
}//end namespace
