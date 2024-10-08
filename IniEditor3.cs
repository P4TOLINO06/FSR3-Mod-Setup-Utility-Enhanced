using IniParser.Model;
using IniParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace FSR3ModSetupUtilityEnhanced
{
    internal class IniEditor3
    {
        public string Path { get; private set; }

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder retVal, int size, string filePath);

        public IniEditor3(string path)
        {
            Path = path;
        }

        public void ConfigIni3(Dictionary<string, string> keyValuePairs, string? section = null)
        {
            foreach (var kvp in keyValuePairs)
            {
                if (section != null)
                {
                    WritePrivateProfileString(section, kvp.Key, kvp.Value, Path);
                }
                else
                {
                    WritePrivateProfileString(null, kvp.Key, kvp.Value, Path);
                }
            }
        }

        public string Read(string section, string key)
        {
            var retVal = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", retVal, 255, Path);
            return retVal.ToString();
        }
    }
}