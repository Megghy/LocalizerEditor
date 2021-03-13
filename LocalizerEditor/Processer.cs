using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyControl.Controls;
using Ionic.Zip;
using Newtonsoft.Json.Linq;
using static LocalizerEditor.Data;

namespace LocalizerEditor
{
    class Processer
    {
        public static List<LocpackData> ReadAllLocpack()
        {
            /*return await Task.Run(() => {
                
            });*/
            List<LocpackData> list = new List<LocpackData>();
            string path = Properties.Settings.Default.FileLocation;
            if (path == null || path == string.Empty)
            {
                Growl.Warning("未指定Localizer汉化包存放位置.");
                return list;
            }
            var files = Directory.GetFiles(path, "*.locpack");
            if (files.Any())
            {
                foreach (var filename in files)
                {
                    try  //开始读取单个文件
                    {
                        var jsons = new Dictionary<string, JObject>();
                        var zip = ZipFile.Read(filename);
                        if (!zip.ContainsEntry("Package.json"))
                        {
                            Growl.Info($"未在文件 {filename} 中找到 Package.json, 文件可能损坏或非Localizer汉化包.");
                            continue;
                        }
                        var json = JObject.Parse(new StreamReader(zip["Package.json"].OpenReader()).ReadToEnd());
                        jsons.Add("Package", json);
                        var locpackdata = new LocpackData(json);
                        long count = 0;
                        foreach (string zipfile in json["FileList"].ToArray())
                        {
                            try
                            {
                                if (zip.ContainsEntry(zipfile + ".json"))
                                {
                                    var tempjson = JObject.Parse(new StreamReader(zip[zipfile + ".json"].OpenReader()).ReadToEnd());
                                    jsons.Add(zipfile, tempjson);
                                    switch (zipfile)
                                    {
                                        case "Package":
                                            break;
                                        case "LdstrFile":
                                            foreach (var ldstrentries in tempjson["LdstrEntries"].Children())
                                            {
                                                count += ldstrentries.Children().Count();
                                            }
                                            break;
                                        default:
                                            count += tempjson.Count;
                                            break;
                                    }
                                }
                            }
                            catch (Exception zipex) { Growl.Error(zipex.Message); }
                        }
                        locpackdata.EntriesCount = count;
                        locpackdata.Data = jsons;
                        list.Add(locpackdata);
                    }
                    catch (Exception ex) { Growl.Error(ex.Message); }
                }
            }
            return list;
        }
    }
}
