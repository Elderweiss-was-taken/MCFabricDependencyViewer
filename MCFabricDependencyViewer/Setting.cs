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
    public static class Setting
    {
        public static readonly bool keep_history = false;//false
        public static readonly bool disable_showing_line_numbers = false;//false

        #region dependencies
        public static readonly bool display_self_library = false;//false
        public static readonly bool never_show_self_library_as_root_item__in_final_display = true;//true
        #endregion

        public static bool WillWeEverUse_Mod_dot_CanBeDisplayedAtRoot
        {
            get
            {
                return display_self_library == true && never_show_self_library_as_root_item__in_final_display == true;
            }
        }

        #region blacklist
        #region blacklist settings
        public static readonly bool mod_blacklist_enabled = true;
        #endregion
        public static readonly List<string> blacklisted_mods = new()
        {
            "fabric",
            "fabricloader",
            "minecraft",
            "java"
        };
        public static bool IsModAllowed(string name)
        {
            if (mod_blacklist_enabled)
            {
                if (!blacklisted_mods.Contains(name))
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}