using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser;
using IniParser.Model;

namespace FSR3ModSetupUtilityEnhanced
{
    internal class IniEditor2
    {
        public static void ConfigIni2(string key, string value, string path, string? section = null)
        {
            try
            {
                var parser = new FileIniDataParser();
                IniData data;

                if (File.Exists(path))
                {
                    data = parser.ReadFile(path);
                }
                else
                {
                    data = new IniData();
                }

                if (section != null)
                {
                    if (!data.Sections.ContainsSection(section))
                    {
                        data.Sections.AddSection(section);
                    }
                    data[section][key] = value;
                }
                else
                {
                    data.Global[key] = value;
                }

                parser.WriteFile(path, data);
            }
            catch (Exception ex)
            {            
            }
        }
    }  
}
