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
    public partial class Menu
    {
        #region Constructor
        public static void Init()
        {
            _Instance = new Menu();//TODO Learn about record type and the init (init is like get and set)
        }
        public Menu()
        {
            this._Options         = this.__Options;
            this._Options_Indexes = this.__Options_Indexes;
            this._HeaderBase     = this.__HeaderBase;

            _Instance = this;
        }
        #endregion
    }
}

namespace MCFabricDependencyViewer
{
    public partial class Menu
    {
        #pragma warning disable CS8618
        public static PrintProp PrintProperties { get; set; }
        #pragma warning restore CS8618
        public class PrintProp
        {
            #region other
            private int _total_number_of_mods;

            private List<int> _longest_depthSpaces_and_modID;
            private List<int> _longest_jar  ;
            public int Number_of_digits
            {
                get
                {
                    return this._total_number_of_mods.ToString().Length;
                }
            }
            public int Total_number_of_mods
            {
                get
                {
                    return this._total_number_of_mods;
                }
            }
            public void Add_longest_depthSpaces_plus_modID(string depthSpaces_plus_modID)
            {
                _longest_depthSpaces_and_modID.Add(depthSpaces_plus_modID.Length);
            }
            public void Add_longest_jar(string jar)
            {
                _longest_jar.Add(jar.Length);
            }
            public int CharCount_longest_depthSpaces_and_modID
            {
                get
                {
                    return this._longest_depthSpaces_and_modID.Max();
                }
            }
            public int CharCount_longest_jar
            {
                get
                {
                    return this._longest_jar.Max();
                }
            }
            #endregion
            #region Constructor
            public PrintProp(int total_number_of_mods)
            {
                this._total_number_of_mods = total_number_of_mods;

                this._longest_depthSpaces_and_modID = new();
                this._longest_jar                   = new();

                PrintProperties = this;
            }
            #endregion
        }
    }
}

namespace MCFabricDependencyViewer
{
    public partial class Menu
    {
        #region Private members
        private readonly string[] _Options;
        private readonly int[] _Options_Indexes;
        private readonly ANSIString _HeaderBase;

        private string[] __Options
        {
            get
            {
                //return new string[] { "Display All", "Standalones only", "dependant mods" };
                return new string[] { "Display All", "Work in Progress", "Work in Progress" };
            }
        }
        private int[] __Options_Indexes
        {
            get
            {
                return Enumerable.Range(0, _Options.Length).ToArray();
            }
        }
        private ANSIString __HeaderBase
        {
            get
            {
                return new ANSIString($"0 for {this[0]}, 1 for {this[1]}, 2 for {this[2]}");
            }
        }

        private bool _Contains(int value)
        {
            return _Options_Indexes.Contains(value);
        }
        #endregion
        #region Public instance members
        public string this[int index]
        {
            get
            {
                return _Options[index];
            }
        }
        #endregion
        #region Public static members
        #region Instance
        #pragma warning disable CS8625
        public static Menu _Instance = null;
        #pragma warning restore CS8625
        public static Menu Instance
        {
            get
            {
                if (_Instance == null) throw new InvalidOperationException("ばか");
                return _Instance;
            }
        }
        #endregion
        public static string[] Options
        {
            get
            {
                return Instance._Options;
            }
        }
        public static int[] Options_Indexes
        {
            get
            {
                return Instance._Options_Indexes;
            }
        }
        public static ANSIString HeaderBase
        {
            get
            {
                return Instance._HeaderBase;
            }
        }
        public static bool Contains(int value)
        {
            return Instance._Contains(value);
        }
        #endregion
    }
}