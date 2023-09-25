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
    public partial class Counterr
    {
        #pragma warning disable CS8618
        public Counterr()
        {
            this.root    = new();
            this.notRoot = new();

            #pragma warning disable CS8625
            this.mod = null;
            #pragma warning restore CS8625
            this._Depth = -1;
        }
        #pragma warning restore CS8618

        #pragma warning disable CS8618
        public Mod mod { get; set; }
        #pragma warning restore CS8618
        public void SetProperties(Global g, string modID, int depth)
        {
            this.mod = g[modID];
            this._Depth = depth;
        }

        private int _Depth = -1;
        public int Depth
        {
            get
            {
                return this._Depth;
            }
        }

        public bool IsRoot
        {
            get
            {
                return Depth == 0;
            }
        }

        public ConsoleColor? Color_
        {
            get
            {
                return (IsRoot ? Mod.ConsoleColor.ColorWhenRoot : Mod.ConsoleColor.ColorWhenNonRoot);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Collapse
        {
            get
            {
                int retVal;

                if (this.IsRoot)
                {
                    if (this.mod.HasJar)//If the mod is root AND has a jar :
                    {
                        notRoot.Reset();//Then we stop numbering on other levels.

                        retVal = returnValue(root);
                        root++;       // 2.) Then we increment it. In that order. Return first, then increment second.
                        return retVal;// 1.) We return it.
                    }
                    else//If the mod is root AND does not have a jar :
                    {
                        notRoot.Reset();//Then we stop numbering on other levels.
                                                 // 2.) Then we DONT increment it. In that order. Return first, then DONT increment second.
                        return returnValue(root);// 1.) We return it.
                    }
                }

                retVal = returnValue(notRoot);//If the mod is not root then 1.) We return it 2.) We increment it.
                notRoot++;

                return retVal;

                static int returnValue(int val)//static is here to show that the method isn't using any instance at all
                {
                    return int.Parse(val.ToString());
                }
            }
        }

        public rootC    root;
        public notRootC notRoot;
    }
}

namespace MCFabricDependencyViewer
{
    public abstract class baseCounter
    {
        internal int ____value;

        public int Value
        {
            get { return ____value; }
        }
        public void Reset()
        {
            ____value = 1;
        }
        public override string ToString()
        {
            return ____value.ToString();
        }
    }

    public class rootC : baseCounter
    {
        internal rootC()
        {
            ____value = 1;
        }
        public static implicit operator int(rootC obj)
        {
            return obj.____value;
        }
        public static rootC operator ++(rootC obj)
        {
            obj.____value++;
            return obj;
        }
    }

    public class notRootC : baseCounter
    {
        internal notRootC()
        {
            ____value = 1;
        }
        public static implicit operator int(notRootC obj)
        {
            return obj.____value;
        }
        public static notRootC operator ++(notRootC obj)
        {
            obj.____value++;
            return obj;
        }
    }
}