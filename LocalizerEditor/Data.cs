using System;
using System.Collections.Generic;
using HandyControl.Controls;
using Newtonsoft.Json.Linq;

namespace LocalizerEditor
{
    class Data
    {
        public class LocpackData
        {
            public LocpackData(JObject json)
            {
                try
                {
                    DisplayName = json.Value<string>("Name");
                    InternalName = json.Value<string>("ModName");
                    Author = json.Value<string>("Author");
                    Description = json.Value<string>("Description");
                    Version = json.Value<string>("Version");
                    ModVersion = json.Value<string>("ModVersion");
                    Language = json.Value<string>("Language");
                }
                catch (Exception ex) { Growl.Warning($"读取包数据时发生错误.\n{ex.Message}"); }
            }
            public string DisplayName { get; set; }
            public string InternalName { get; set; }
            public string Author { get; set; }
            public string Description { get; set; }
            public string Version { get; set; }
            public string ModVersion { get; set; }
            public string Language { get; set; }
            public long EntriesCount { get; set; }
            public Dictionary<string, JObject> Data = new Dictionary<string, JObject>();
            Dictionary<string, List<LocalizeEntry>> _Entries;
            public Dictionary<string, List<LocalizeEntry>> Entries
            {
                get
                {
                    if (_Entries == null)
                    {
                        _Entries = new Dictionary<string, List<LocalizeEntry>>();
                        foreach (var data in Data)
                        {
                            if (data.Key == "Package") continue;
                            var tempdata = new List<LocalizeEntry>();
                            switch (data.Key)
                            {
                                case "BasicBuffFile":
                                    foreach (var entry in data.Value["Buffs"].Children())
                                    {
                                        var tempentry = new LocalizeEntry(((JProperty)entry).Name, "", "", "", "", true);
                                        foreach (JObject _entry in entry.Children())
                                        {
                                            tempentry.OriginName = _entry["Name"]["Origin"].ToString();
                                            tempentry.TranslateName = _entry["Name"]["Translation"].ToString();
                                            tempentry.OriginDescription = _entry["Description"]["Origin"].ToString();
                                            tempentry.TranslateDescription = _entry["Description"]["Origin"].ToString();
                                        }
                                        tempdata.Add(tempentry);
                                    }
                                    break;
                                case "BasicItemFile":
                                    foreach (var entry in data.Value["Items"].Children())
                                    {
                                        var tempentry = new LocalizeEntry(((JProperty)entry).Name, "", "", "", "", true);
                                        foreach (JObject _entry in entry.Children())
                                        {
                                            tempentry.OriginName = _entry["Name"]["Origin"].ToString();
                                            tempentry.TranslateName = _entry["Name"]["Translation"].ToString();
                                            tempentry.OriginDescription = _entry["Tooltip"]["Origin"].ToString();
                                            tempentry.TranslateDescription = _entry["Tooltip"]["Origin"].ToString();
                                        }
                                        tempdata.Add(tempentry);
                                    }
                                    break;
                            }
                            _Entries.Add(data.Key, tempdata);
                        }
                    }
                    return _Entries;
                }
                set { }
            }
            public List<LocalizeEntry> BuffFile
            {
                get
                {
                    if (Entries.ContainsKey("BasicBuffFile")) return Entries["BasicBuffFile"]; return null;
                }
                set { }
            }
            public LdstrFile Ldstr;
        }
        public struct LocalizeEntry
        {
            public LocalizeEntry(string internalname, string name, string translatename, string description, string translatedescription, bool havedescription)
            {
                InternalName = internalname;
                OriginName = name;
                TranslateName = translatename;
                OriginDescription = description;
                TranslateDescription = translatedescription;
                HaveDescription = havedescription;
            }
            public string InternalName { get; set; }
            public string OriginName { get; set; }
            public string TranslateName { get; set; }
            public string OriginDescription { get; set; }
            public string TranslateDescription { get; set; }
            public bool HaveDescription { get; set; }
        }
        public class LdstrFile
        {
            public LdstrFile()
            {

            }

        }
    }
}
