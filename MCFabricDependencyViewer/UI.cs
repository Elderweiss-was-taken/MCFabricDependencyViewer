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
using System.Reflection;

namespace MCFabricDependencyViewer
{
    public static partial class UI
    {
        public static B_and_F GetBody(string path, int program_shown)
        {
            Global global = new(path);

            Prepare(global);

            switch (program_shown)
            {
                default://default is case 0:
                    return DisplayDependencyTree0(global);
                case 1:
                    return DisplayDependencyTree1(global);
                case 2:
                    return DisplayDependencyTree2(global);
            }
        }
        private static void Base_DisplayDependencyTree0(Global global, string modID, int depth, ref List<ANSIString> body)
        {
            if (!Setting.WillWeEverUse_Mod_dot_CanBeDisplayedAtRoot)
            {
                AddToList(ref body, global, modID, depth);
            }
            else
            {
                if (depth == 0)
                {
                    if (global[modID].CanBeDisplayedAtRoot)
                    {
                        AddToList(ref body, global, modID, depth);
                    }
                }
                else
                {
                    AddToList(ref body, global, modID, depth);
                }
            }

            void AddToList(ref List<ANSIString> l, Global g, in string m, in int d)
            {
                g.counter.SetProperties(g, m, d);

                ANSIString line = Menu.Methods.Print_Line(g);
                l.Add(line);
            }
        }
        private static B_and_F DisplayDependencyTree0(Global global)
        {
            B_and_F Main()
            {
                List<ANSIString> body = new();

                var list = global.Keys.ToList();
                list.Sort();

                foreach (var modID in list)
                {
                    DisplayModDependencyTree0(modID, 0);
                }

                void DisplayModDependencyTree0(string modID, int depth)
                {
                    Base_DisplayDependencyTree0(global, modID, depth, ref body);

                    if (global.ContainsKey(modID))
                    {
                        foreach (var dependency in global[modID].Dependencies)
                        {
                            DisplayModDependencyTree0(dependency.ID, depth + 1);
                        }
                    }
                }

                return new B_and_F(body);
            }

            B_and_F returnValue = Main();

            returnValue.Footer.Add(new("footer"));

            return returnValue;
        }
    }
}

namespace MCFabricDependencyViewer
{
    public static partial class UI
    {
        /// <summary>
        /// REMOVE => display_self_library == FALSE
        /// TAG    => display_self_library == TRUE  && never_show_self_library_as_root_item__in_final_display == TRUE
        /// 
        /// No mod will be removed if "display_self_library" is TRUE
        /// No tag will be used if Setting.WillWeEverUse_Mod_dot_CanBeDisplayedAtRoot is false
        /// </summary>
        private static void RemoveSelfDep_TagSelfDep(Global global)
        {
            List<string> modsToDelete = new();

            foreach (string modID in global.Keys)
            {
                bool IsSelfLibrary = global[modID].IsSelfLibrary;

                bool Keep_the_mod = false;

                #region SL
                if (!Setting.display_self_library)
                {
                    if (!IsSelfLibrary)
                    {
                        Keep_the_mod = true;
                    }
                }
                else
                {
                    if (Setting.never_show_self_library_as_root_item__in_final_display)
                    {
                        if (IsSelfLibrary)
                        {
                            Keep_the_mod = true;
                            global[modID].CanBeDisplayedAtRoot = false;
                        }
                        else
                        {
                            Keep_the_mod = true;
                        }
                    }
                    else
                    {
                        Keep_the_mod = true;
                    }
                }

                if (!Keep_the_mod)
                {
                    modsToDelete.Add(modID);
                }
                #endregion
            }
            foreach (Mod mod in global.mods.Values)
            {
                int tmp = mod.Dependencies.Count;
                List<int> l = new();
                for (int i = 0; i < tmp; i++)
                {
                    l.Add(i);
                }
                l.Reverse();
                foreach (int i in l)
                {
                    if (modsToDelete.Contains(mod.Dependencies[i].ID))
                    {
                        mod.Dependencies.RemoveAt(i);
                    }
                }
                tmp = mod.Slaves.Count;
                l = new();
                for (int i = 0; i < tmp; i++)
                {
                    l.Add(i);
                }
                l.Reverse();
                foreach (int i in l)
                {
                    if (modsToDelete.Contains(mod.Slaves[i].ID))
                    {
                        mod.Slaves.RemoveAt(i);
                    }
                }
            }
            foreach (string modID in modsToDelete)
            {
                global.Remove(modID);
                
            }
        }
        private static void SetSlavery(Global global)
        {
            RemoveSelfDep_TagSelfDep(global);

            int i;
            int j;

            List<string> l = new(global.Keys);

            int total_number_of_mods = l.Count;
            Menu.PrintProperties = new(total_number_of_mods);

            for (i = 0; i < total_number_of_mods; i++)
            {
                for (j = 0; j < global[l[i]].DependencyCount_Any; j++)
                {
                    if (global[l[i]].Dependencies[j].HasJar)
                    {
                        global[l[i]].Dependencies[j].Slaves.Add(global[l[i]]);
                    }
                }
            }
        }
        private static void Base_Prepare(Global global)
        {
            SetSlavery(global);

            global.ResetCounter();

            foreach (var modID in global.Keys)
            {
                DisplayModDependencyTree0(modID, 0);
            }

            void DisplayModDependencyTree0(string modID, int depth)
            {
                global.counter.SetProperties(global, modID, depth);

                string depthSpaces_plus_modID = Menu.Methods.PrintS_middle____depthSpaces_and_modID_and_tab(global[modID], modID, depth);
                Menu.PrintProperties.Add_longest_depthSpaces_plus_modID(depthSpaces_plus_modID);

                string jar = Menu.Methods.PrintS_right_____jars(global[modID], global[modID].Jar);
                Menu.PrintProperties.Add_longest_jar(jar);

                if (global.ContainsKey(modID))
                {
                    foreach (var dependency in global[modID].Dependencies)
                    {
                        DisplayModDependencyTree0(dependency.ID, depth + 1);
                    }
                }
            }
        }
        private static void Prepare(Global global)
        {
            Base_Prepare(global);

            global.ResetCounter();
        }
    }
}

namespace MCFabricDependencyViewer
{
    public static partial class UI
    {
        private static int _User_selection = -1;
        public static int User_selection
        {
            get
            {
                return _User_selection;
            }
            set
            {
                _User_selection = value;
            }
        }
        public static void WriteLines(B_and_F arg)
        {
            string tmp = "Displaying : ";

            #region not important
            if (Setting.keep_history)
            {
                Backspace();
            }
            else
            {
                Clear();

                WriteLine(Menu.HeaderBase);
            }
            #endregion

            WriteLine(string.Empty);
            WriteLine(GetHeader(tmp));
            WriteLine(string.Empty);
            WriteLine(arg.Body);
            WriteLine(string.Empty);
            WriteLine(arg.Footer);
            WriteLine(string.Empty);
        }
        private static ANSIString GetHeader(string text)
        {
            return new($"{text}{Menu.Instance[UI.User_selection].Underlined()}");
        }
        #region Write and stuff
        private static void Write(params object[] args)
        {
            foreach (var arg in args)
            {
                Console.Write(arg);
            }
        }
        public static void WriteLine(params object[] args)
        {
            foreach (var arg in args)
            {
                Console.WriteLine(arg);
            }
        }
        public static void WriteLine(List<ANSIString> list)
        {
            foreach (ANSIString s in list)
            {
                WriteLine(s);
            }
        }
        private static void Clear()
        {
            Console.Clear();
        }
        public static void Backspace()
        {
            Write("\b \b");
        }
        #endregion
        private static B_and_F DisplayDependencyTree1(Global global)
        {
            throw new NotImplementedException();
        }
        private static B_and_F DisplayDependencyTree2(Global global)
        {
            throw new NotImplementedException();
        }
        public static int Get_user_selection()
        {
            bool decline_keyPress = true;

            do
            {
                UI._User_selection = -1;

                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();

                if (char.IsDigit(consoleKeyInfo.KeyChar))
                {
                    UI._User_selection = Convert.ToInt32(consoleKeyInfo.KeyChar.ToString());

                    if (Menu.Contains(UI._User_selection))
                    {
                        decline_keyPress = false;
                    }
                }
                if (decline_keyPress == true)
                {
                    UI.Backspace();
                }

            } while (decline_keyPress);

            return UI._User_selection;
        }
    }
}