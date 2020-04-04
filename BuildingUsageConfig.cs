using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace BuildingUsage
{
    /// <summary>
    /// define global (i.e. for this mod but not game specific) configuration properties
    /// </summary>
    public class BuildingUsageConfig
    {
        // convention for the config file name seems to be the mod name + "Config.xml"
        private const string ConfigFileName = "BuildingUsageConfig.xml";

        /// <summary>
        /// associate a usage type as a string with a check box status for writing/reading config file
        /// using a string for the usage type in the file allows removing a usage type from the enum without impacting the ability to read an existing config file
        /// </summary>
        public class FileConfigEntry
        {
            public string usageType;
            public bool checkBox;

            // cannot use default constructor
            private FileConfigEntry() { }

            // define new constructor
            public FileConfigEntry(string usageType, bool checkBox)
            {
                this.usageType = usageType;
                this.checkBox = checkBox;
            }
        }

        /// <summary>
        /// a list of config entries, for writing/reading config file
        /// cannot use Dictionary because a Dictionary cannot be serialized
        /// </summary>
        public class FileConfigEntryList : List<FileConfigEntry> { }

        /// <summary>
        /// internal list of config entries
        /// </summary>
        private class ConfigEntryList : Dictionary<UsagePanel.UsageType, bool> { }

        private static ConfigEntryList _configEntries = null;

        /// <summary>
        /// save to the config file
        /// </summary>
        public static void Save()
        {
            try
            {
                // create a config entry for each usage type
                FileConfigEntryList fileConfigEntries = new FileConfigEntryList();
                foreach (UsagePanel.UsageType usageType in Enum.GetValues(typeof(UsagePanel.UsageType)))
                {
                    // ignore None and UseLogic usage types
                    if (usageType != UsagePanel.UsageType.None && usageType != UsagePanel.UsageType.UseLogic1 && usageType != UsagePanel.UsageType.UseLogic2)
                    {
                        // get the usage panel that has the usage type
                        UsagePanel usagePanel = null;
                        if (BuildingUsage.workersUsagePanel.HasUsageType(usageType))
                        {
                            usagePanel = BuildingUsage.workersUsagePanel;
                        }
                        else if (BuildingUsage.visitorsUsagePanel.HasUsageType(usageType))
                        {
                            usagePanel = BuildingUsage.visitorsUsagePanel;
                        }
                        else if (BuildingUsage.storageUsagePanel.HasUsageType(usageType))
                        {
                            usagePanel = BuildingUsage.storageUsagePanel;
                        }
                        else if (BuildingUsage.vehiclesUsagePanel.HasUsageType(usageType))
                        {
                            usagePanel = BuildingUsage.vehiclesUsagePanel;
                        }
                        else
                        {
                            // usage panel not found for usage type (possibly because DLC is not installed)
                            // use most recent check box setting
                            fileConfigEntries.Add(new FileConfigEntry(usageType.ToString(), GetCheckBoxSetting(usageType)));
                        }

                        // add a new config entry with the usage type and its corresponding checkbox status
                        if (usagePanel != null)
                        {
                            fileConfigEntries.Add(new FileConfigEntry(usageType.ToString(), usagePanel.IsCheckBoxChecked(usageType)));
                        }
                    }
                }

                // save the config entries to the config file
                using (StreamWriter streamWriter = new StreamWriter(ConfigFileName))
                {
                    XmlSerializerNamespaces noNamespaces = new XmlSerializerNamespaces();
                    noNamespaces.Add("", "");
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(FileConfigEntryList));
                    xmlSerializer.Serialize(streamWriter, fileConfigEntries, noNamespaces);
                }

                // save file config entries to internal config entries
                ConvertFileConfigEntries(fileConfigEntries);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        /// <summary>
        /// return the check box setting for the usage type from the most recently saved config entries or from the config file
        /// </summary>
        public static bool GetCheckBoxSetting(UsagePanel.UsageType usageType)
        {
            try
            {
                // check if already have config entries
                if (_configEntries == null)
                {
                    // check if the config file exists
                    if (File.Exists(ConfigFileName))
                    {
                        // read the config entries from the config file
                        using (StreamReader streamReader = new StreamReader(ConfigFileName))
                        {
                            XmlSerializer xmlSerializer = new XmlSerializer(typeof(FileConfigEntryList));
                            FileConfigEntryList fileConfigEntries = (FileConfigEntryList)xmlSerializer.Deserialize(streamReader);
                            ConvertFileConfigEntries(fileConfigEntries);
                        }
                    }
                    else
                    {
                        // no config file, return default value of true
                        return true;
                    }
                }

                // find the config entry for the usage type and return the configured check box status
                if (_configEntries.TryGetValue(usageType, out bool checkBox))
                {
                    return checkBox;
                }

                // usage type not found in the config entries, return default value of true
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                return true;
            }
        }

        /// <summary>
        /// convert a list of file config entries to the internal list of config entries
        /// </summary>
        private static void ConvertFileConfigEntries(FileConfigEntryList fileConfigEntries)
        {
            // initialize the internal list
            _configEntries = new ConfigEntryList();

            // do each file config entry
            foreach (FileConfigEntry fileConfigEntry in fileConfigEntries)
            {
                // ignore any file config entry with a usage type that cannot be parsed back to the enum
                // this allows removing a usage type from the enum without impacting the ability to read an existing config file
                try
                {
                    _configEntries.Add((UsagePanel.UsageType)Enum.Parse(typeof(UsagePanel.UsageType), fileConfigEntry.usageType), fileConfigEntry.checkBox);
                }
                catch
                {
                }
            }
        }

    }
}
