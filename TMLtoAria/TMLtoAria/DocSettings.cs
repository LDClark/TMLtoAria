using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace TMLtoAria
{
    public class DocSettings
    {
        public String HostName { get; private set; } 
        public String Port { get; private set; } 
        public String DocKey { get; private set; } 
        public String ImportDir { get; private set; } 

        public static DocSettings ReadSettings(String settingsFilePath)
        {
            DocSettings docSettings = new DocSettings();
            if (File.Exists(settingsFilePath))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(settingsFilePath))
                    {
                        int i = 0;
                        while (!reader.EndOfStream)
                        {
                            string settingLine = reader.ReadLine();
                            
                            if (i == 0)
                                docSettings.HostName = RemoveWhitespace(settingLine.Split(':')[1]);
                            if (i == 1)
                                docSettings.Port = RemoveWhitespace(settingLine.Split(':')[1]);
                            if (i == 2)
                                docSettings.DocKey = RemoveWhitespace(settingLine.Split(':')[1]);
                            if (i == 3)
                                docSettings.ImportDir = RemoveWhitespace(settingLine.Split(':')[1]);
                            i++;
                        }
                        reader.Close();
                    }
                }
                catch (Exception)
                {
                    throw new ApplicationException("Error in reading TMLtoAria.setting, please check the file before using this script");
                }
            }
            else
            {
                throw new ApplicationException("Cannot locate TMLtoAria.setting, please check the file before using this script");
            }
            return docSettings;
        }

        public static string RemoveWhitespace(string line)
        {
            return string.Join("", line.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
