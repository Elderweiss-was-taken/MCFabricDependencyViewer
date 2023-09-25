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
    public partial class Mod
    {
        public static List<Mod> GiveEmptyDepList()
        {
            List<Mod> returnValue = new();

            return returnValue;
        }

        public Mod(string ID_, string path, List<Mod> dep_list)
        {
            this.ID = ID_;
            this.ModFileName = path;
            this.Dependencies = dep_list;

            this.Slaves = new();

            this.CanBeDisplayedAtRoot = true;

            this._Color = null;
            this._CollapseCount = 0;
        }

        #region Constructor dependant

        public string ID { get; set; }

        public string ModFileName { get; set; }

        public List<Mod> Dependencies { get; set; }

        #endregion

        #region Constructor independant
        public int DependencyCount_ForRealModsThatHaveAJar
        {
            get
            {
                int returnValue = 0;

                foreach (Mod item in this.Dependencies)
                {
                    if (item.HasJar)
                    {
                        returnValue++;
                    }
                }

                return returnValue;
            }
        }
        public int DependencyCount_Any
        {
            get
            {
                return this.Dependencies.Count;
            }
        }
        
        public List<Mod> Slaves { get; set; }

        public int SlaveCount
        {
            get
            {
                int returnValue = 0;

                foreach (Mod item in this.Slaves)
                {
                    if (item.HasJar)
                    {
                        returnValue++;
                    }
                }

                return returnValue;
            }
        }

        public bool IsTrulyIndependant
        {
            get
            {
                return HasZeroSlaves && HasZeroDependencies;
            }
        }

        public bool HasZeroSlaves
        {
            get
            {
                return this.SlaveCount == 0;
            }
        }
        public bool HasZeroDependencies
        {
            get
            {
                return this.DependencyCount_ForRealModsThatHaveAJar == 0;
            }
        }

        public bool CanBeDisplayedAtRoot { get; set; }

        public string Jar
        {
            get
            {
                return this.ModFileName;
            }
        }

        public bool HasJar
        {
            get
            {
                if (this.ModFileName == null)
                {
                    return false;
                }
                if (this.ModFileName == string.Empty)
                {
                    return false;
                }
                return true;
            }
        }

        public bool IsSelfLibrary
        {
            get
            {
                if (this.HasJar)
                {
                    return false;
                }
                return true;
            }
        }

        private System.ConsoleColor? _Color { get; set; }

        public System.ConsoleColor? Color
        {
            get
            {
                CollapseColor();
                return _Color;
            }
        }

        private int _CollapseCount { get; set; }

        public void CollapseColor()
        {
            if (_CollapseCount != 0)
            {
                //TODO very dangerous since it means if we remove or add dependancies/slaves, the color won't update
                return;
            }
            if (this.IsTrulyIndependant)
            {
                _Color = Mod.ConsoleColor.TrulyIndependant;
                _CollapseCount++;
                return;
            }
            else if (this.HasZeroSlaves)
            {
                _Color = Mod.ConsoleColor.ZeroSlaves;
                _CollapseCount++;
                return;
            }
            else if (this.HasZeroDependencies)
            {
                _Color = Mod.ConsoleColor.ZeroDependencies;
                _CollapseCount++;
                return;
            }
        }
        #endregion
    }
}

/// Static
namespace MCFabricDependencyViewer
{
    public partial class Mod
    {
        public static class ConsoleColor
        {
            #region Mod states
            public static System.ConsoleColor? TrulyIndependant
            {
                get
                {
                    return System.ConsoleColor.Yellow;
                }
            }
            public static System.ConsoleColor? ZeroSlaves
            {
                get
                {
                    return System.ConsoleColor.DarkCyan;
                }
            }
            public static System.ConsoleColor? ZeroDependencies
            {
                get
                {
                    return System.ConsoleColor.Red;
                }
            }
            #endregion
            #region Not a mod color, line colors
            public static System.ConsoleColor? Asterix
            {
                get
                {
                    return System.ConsoleColor.Green;
                }
            }
            public static System.ConsoleColor? ColorWhenRoot
            {
                get
                {
                    return System.ConsoleColor.Green;
                }
            }
            public static System.ConsoleColor? ColorWhenNonRoot
            {
                get
                {
                    return System.ConsoleColor.Magenta;
                }
            }
            #endregion
        }
    }
}