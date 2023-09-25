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
    public class Global
    {
        #pragma warning disable CS8618
        public Global(string path_of_jar_mods)
        {
            this.mods = new();

            FolderHandler.Processing_the_folder_where_mods_are_located(this, path_of_jar_mods);
        }
        #pragma warning restore CS8618
        #region Main properties

        #pragma warning disable CS8618
        public void ResetCounter()
        {
            this.counter = new();
        }

        public Counterr counter;
        #pragma warning restore CS8618

        public Dictionary<string, Mod> mods { get; set; }
        #endregion

        #region unable to dynamically override all Dictionary methods and properties so I implement as I need them
        public void Add(string key, Mod value)
        {
            mods.Add(key, value);
        }
        public void Remove(string key)
        {
            mods.Remove(key);
        }
        public bool ContainsKey(string key)
        {
            return mods.ContainsKey(key);
        }
        public Dictionary<string, Mod>.KeyCollection Keys
        {
            get
            {
                return mods.Keys;
            }
        }
        public Dictionary<string, Mod>.ValueCollection Values
        {
            get
            {
                return mods.Values;
            }
        }
        public Mod this[string index]
        {
            get
            {
                return mods[index];
            }
            set
            {
                mods[index] = value;
            }
        }
        #endregion
    }
}