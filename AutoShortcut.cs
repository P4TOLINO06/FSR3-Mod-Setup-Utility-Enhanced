using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO = System.IO;
using IWshRuntimeLibrary;

namespace FSR3ModSetupUtilityEnhanced
{
    internal class AutoShortcut
    {
        public static void AShortcut(string pathExe, string nameShortcut, string dx12, string nameMessagebox = null)
        {
            if (MessageBox.Show($"{nameMessagebox}","Create Shortcut",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (IO.File.Exists(pathExe))
                {
                    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string shortcutPath = IO.Path.Combine(desktopPath, nameShortcut + ".lnk");

                    WshShell shell = new WshShell();
                    IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                    shortcut.TargetPath = pathExe;
                    shortcut.Arguments = dx12;
                    shortcut.Save();

                    MessageBox.Show("Shortcut successfully created on the Desktop, run the game through the shortcut for the mod to function properly.","Shortcut successfully created", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"\"{pathExe}\" not found, please create a shortcut manually.","Shortcut Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
