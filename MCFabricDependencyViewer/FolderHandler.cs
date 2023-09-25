using ANSIConsole;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;
using System.Xml.Linq;

using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MCFabricDependencyViewer
{
    public static class FolderHandler
    {
        public static void Processing_the_folder_where_mods_are_located(Global g, string folderPath)
        {
            IEnumerable[] jarFiles = Directory.GetFiles(folderPath, "*.jar", SearchOption.TopDirectoryOnly);

            #pragma warning disable IDE0220
            foreach (string jarFile in jarFiles)
            {
                ProcessJarFile(g, jarFile);
            }
            #pragma warning restore IDE0220
        }

        private static void ProcessJarFile(Global g, string jarFilePath)
        {
            string[] file_formats_URHHHHH = new string[] { "fabric.mod.json", "quilt.mod.json" };

            try
            {
                using (var jarStream = File.OpenRead(jarFilePath))
                using (var zipArchive = new System.IO.Compression.ZipArchive(jarStream))
                {
                    byte modLoaderIndex = 0;

                    if (!successfully_processed_fabric_jar(zipArchive.GetEntry(file_formats_URHHHHH[(int)modLoaderIndex])))
                    {
                        modLoaderIndex++;
                        if (!successfully_processed_quilt_jar(zipArchive.GetEntry(file_formats_URHHHHH[(int)modLoaderIndex])))
                        {
                            modLoaderIndex++;

                            #region exception - mod is not fabric or quilt
                            StringBuilder retVal = new();

                            #region content
                            retVal.AppendLine();
                            retVal.AppendLine();
                            retVal.AppendLine();

                            retVal.AppendLine($"The mod {Path.GetFileName(jarFilePath)} does not contain any of the following :");

                            retVal.AppendLine();

                            foreach (string s in file_formats_URHHHHH)
                            {
                                retVal.AppendLine($" - {s}");
                            }

                            retVal.AppendLine();

                            retVal.AppendLine("This program only supports Fabric and quilt");

                            retVal.AppendLine();
                            retVal.AppendLine();
                            retVal.AppendLine();
                            #endregion

                            throw new Exception(retVal.ToString());
                            #endregion
                        }
                    }

                    bool successfully_processed_fabric_jar(ZipArchiveEntry? ModJsonEntry)
                    {
                        bool value = ModJsonEntry != null;

                        if (value)
                        {
                            #pragma warning disable CS8602
                            using (var fabricModJsonStream = ModJsonEntry.Open())
                            using (var reader = new StreamReader(fabricModJsonStream))
                            {
                                Base_process(reader, modLoaderIndex);
                            }
                            #pragma warning restore CS8602
                        }

                        return value;
                    }

                    bool successfully_processed_quilt_jar(ZipArchiveEntry? ModJsonEntry)
                    {
                        bool value = ModJsonEntry != null;

                        if (ModJsonEntry != null)
                        {
                            using (var quiltModJsonStream = ModJsonEntry.Open())
                            using (var reader = new StreamReader(quiltModJsonStream))
                            {
                                Base_process(reader, modLoaderIndex);
                            }
                        }

                        return value;
                    }

                    void Base_process(StreamReader? r_r_reader, byte _modLoaderIndex)
                    {
                        #pragma warning disable CS8602
                        var ModJson = r_r_reader.ReadToEnd();
                        #pragma warning restore CS8602
                        var ModInfo = JObject.Parse(ModJson);

                        var __path = Path.GetFileName(jarFilePath);

                        if (_modLoaderIndex == 0)
                        {
                            ProcessFabricModInfo(g, ModInfo, __path);
                        }
                        if (_modLoaderIndex == 1)
                        {
                            ProcessQuiltModInfo(g, ModInfo, __path);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing {Path.GetFileName(jarFilePath)}: {ex.Message}");
            }
        }

        private static void BaseProcessModInfo(Global g, string modID, string modFileName, JObject dependencies)
        {
            main_code();

            void main_code()
            {
                try
                {
                    Mod _m;
                    _m = g[modID];

                    Root_because_created_by_a_dependency_____now_need_to_update_the_jar___________and_dependencies_____________();

                    void Root_because_created_by_a_dependency_____now_need_to_update_the_jar___________and_dependencies_____________()
                    {
                        _m.ModFileName = modFileName;

                        _m.Dependencies = return_mod_dependencies_____________________________________________________________________________();

                        List<Mod> return_mod_dependencies_____________________________________________________________________________()
                        {
                            return Base_return_mod_dependencies();
                        }
                    }
                }
                catch (Exception)
                {
                    not_Root_Yet_________________________________________________________________________________________________();
                }

                void not_Root_Yet_________________________________________________________________________________________________()
                {
                    Base_not_Root_Yet__________________________________();

                    void Base_not_Root_Yet__________________________________()
                    {
                        Mod ttmp = new(modID, modFileName, return_mod_dependencies_____________________________________________________________________________());

                        g.Add(modID, ttmp);
                    }

                    List<Mod> return_mod_dependencies_____________________________________________________________________________()
                    {
                        return Base_return_mod_dependencies();
                    }
                }
            }

            List<Mod> Base_return_mod_dependencies()
            {
                List<Mod> tmp_value = new();

                forEach_dependency___________________________________________________________________________________();

                void forEach_dependency___________________________________________________________________________________()
                {
                    foreach (JProperty item in dependencies.Properties())
                    {
                        if (Setting.IsModAllowed(item.Name))
                        {
                            Base_forEach_dependency__________________________________________________________________();

                            void Base_forEach_dependency__________________________________________________________________()
                            {
                                try
                                {
                                    Mod _m;
                                    _m = g[item.Name];

                                    dependency_does_exist____________________________________________________________();

                                    void dependency_does_exist____________________________________________________________()
                                    {
                                        tmp_value.Add(_m);
                                    }
                                }
                                catch (Exception)
                                {
                                    dependency_does_not_exist____________________________________________________________();

                                    void dependency_does_not_exist____________________________________________________________()
                                    {
                                        Mod ttttmp = new(item.Name, string.Empty, Mod.GiveEmptyDepList());

                                        g.Add(item.Name, ttttmp);

                                        tmp_value.Add(g[item.Name]);
                                    }
                                }
                            }
                        }
                    }
                }

                return tmp_value;
            }
        }

        private static void ProcessQuiltModInfo(Global g, JObject jsonModInfo, string modFileName)
        {
            #pragma warning disable CS8602
            const string hello_im_quilt = "quilt_loader";

            string[] val = { "id", "depends", "versions" };

            JToken? cell = jsonModInfo.Root[hello_im_quilt][val[0]];

            if (cell.ToString() is string modID)
            {
                if (!Setting.IsModAllowed(modID))
                {
                    return;
                }

                if (!string.IsNullOrWhiteSpace(modID))
                {
                    JToken? cell2 = jsonModInfo[hello_im_quilt][val[1]];

                    if (quiltSpecialCode() is JObject dependencies)
                    {
                        BaseProcessModInfo(g, modID, modFileName, dependencies);
                    }
                    else
                    {
                        g.Add(modID, new(modID, modFileName, Mod.GiveEmptyDepList()));
                    }

                    ///Converts X into Y
                    /// X is the json object that reading a Quilt mod gives us
                    /// Y is the same json object that we receive in reading Fabric mods
                    JToken? quiltSpecialCode()
                    {
                        JObject tmpJObject = new()
                        {
                            [val[1]] = new JObject { }
                        };

                        if (cell2 is JArray dependsArray)
                        {
                            foreach (JToken item in dependsArray)
                            {
                                if (item is JObject obj && obj.ContainsKey(val[0]) && obj.ContainsKey(val[2]))
                                {

                                    string id = obj[val[0]].ToString();
                                    string versions = obj[val[2]].ToString();

                                    tmpJObject[val[1]][id] = versions;
                                }
                            }
                        }
                        return tmpJObject;
                    }
                }
            }
            #pragma warning restore CS8602
        }

        private static void ProcessFabricModInfo(Global g, JObject jsonModInfo, string modFileName)
        {
            string[] val = { "id", "depends" };

            int ccc = 0;

            #pragma warning disable CS8602
            JToken? cell = jsonModInfo[val[ccc]];

            if (cell.ToString() is string modID)
            {
                if (!Setting.IsModAllowed(modID))
                {
                    return;
                }

                if (!string.IsNullOrWhiteSpace(modID))
                {
                    ccc++;

                    JToken? cell2 = jsonModInfo[val[ccc]];

                    if (cell2 is JObject dependencies)
                    {
                        BaseProcessModInfo(g, modID, modFileName, dependencies);
                    }
                    else
                    {
                        g.Add(modID, new(modID, modFileName, Mod.GiveEmptyDepList()));
                    }
                }
            }
            #pragma warning restore CS8602
        }
    }
}