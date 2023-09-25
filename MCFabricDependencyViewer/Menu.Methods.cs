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
    public partial class Menu
    {
        public static class Methods
        {
            #region very important
            public const string separator_between_modID_and_jar        = "    ";
            public const string separator_between_lineNumber_and_modID = " ";
            public static string depthSpaces(int depth)
            {
                return new(' ', depth * 2);
            }
            public static string write_spaces(int number_of_spaces)
            {
                return new(' ', number_of_spaces);
            }
            #endregion
            private enum PrintType
            {
                String,
                ANSIString
            }
            #region Format
            #region old code
            //private static ANSIString GetString(string s, Mod m)
            //{
            //    var tmp_color = GiveColor(m);

            //    if (tmp_color == null)
            //    {
            //        return new(s);
            //    }
            //    return s.Color((ConsoleColor)tmp_color);
            //}
            //private static ConsoleColor? GiveColor(Mod mod)
            //{
            //    ConsoleColor? returnValue = null;

            //    if (mod.IsTrulyIndependant)
            //    {
            //        returnValue = ConsoleColor.Yellow;
            //    }
            //    else if (mod.HasZeroSlaves)
            //    {
            //        returnValue = ConsoleColor.DarkCyan;
            //    }
            //    else if (mod.HasZeroDependencies)
            //    {
            //        returnValue = ConsoleColor.Red;
            //    }

            //    return returnValue;
            //}
            #endregion
            private static ANSIString GetString(string s, Mod mod)
            {
                return GetString(s, mod.Color);
            }
            private static ANSIString GetString(string s, ConsoleColor? color)
            {
                if (color == null)
                {
                    return new(s);
                }
                return s.Color((ConsoleColor)color);
            }
            #endregion
            #region Give line number
            private static object oPrintToFormat(PrintType type, Global global, in int line_number)
            {
                int how_many_padding_LeadingZeroes;

                if (0 >= line_number) throw new Exception("");

                char[] tmpCharArray;

                const string space_to_display_if_mod_isnt_root = "  ";
                string spacer_between_lineNumber_and_modName;

                if (global.counter.IsRoot)
                {
                    spacer_between_lineNumber_and_modName = space_to_display_if_mod_isnt_root;

                    how_many_padding_LeadingZeroes        = Menu.PrintProperties.Number_of_digits;
                }
                else
                {
                    spacer_between_lineNumber_and_modName = string.Empty;

                    how_many_padding_LeadingZeroes        = Menu.PrintProperties.Number_of_digits + space_to_display_if_mod_isnt_root.Count();
                }

                tmpCharArray = line_number.ToString($"D{how_many_padding_LeadingZeroes}").ToCharArray();
                
                string returnValue;

                for (int i = 0; i < tmpCharArray.Count(); i++)//this loop will remove all leading zeros but not other important zeros in case the number is 40 or 200
                {
                    if (tmpCharArray[i] == '0')
                    {
                        tmpCharArray[i] = ' ';
                    }
                    else
                    {
                        returnValue = new(tmpCharArray);

                        return (type == PrintType.String ? sFormatReturn(returnValue) : aFormatReturn(returnValue));
                    }
                }

                returnValue = new(tmpCharArray);

                return (type == PrintType.String ? sFormatReturn(returnValue) : aFormatReturn(returnValue));

                ANSIString aFormatReturn(string str1)
                {
                    string str2 = spacer_between_lineNumber_and_modName;

                    return new($"{GetString(str1, global.counter.Color_)}{str2}");
                }

                string sFormatReturn(string str1)
                {
                    string str2 = spacer_between_lineNumber_and_modName;

                    return new($"{str1}{str2}");
                }
            }
            private static string sPrintToFormat(Global global, in int line_number)
            {
                return (string)oPrintToFormat(PrintType.String, global, in line_number);
            }
            private static ANSIString aPrintToFormat(Global global, in int line_number)
            {
                return (ANSIString)oPrintToFormat(PrintType.ANSIString, global, in line_number);
            }
            private static object oPrint_asterix_ToFormat(PrintType type, Global global)
            {
                char[] tmpCharArray;

                const string space_to_display_if_mod_isnt_root = "  ";
                string spacer_between_lineNumber_and_modName;

                if (global.counter.IsRoot)
                {
                    spacer_between_lineNumber_and_modName = space_to_display_if_mod_isnt_root;
                }
                else
                {
                    spacer_between_lineNumber_and_modName = string.Empty;
                }

                tmpCharArray = Menu.PrintProperties.Total_number_of_mods.ToString().ToCharArray();
                
                string returnValue;
                for (int i = 0; i < tmpCharArray.Count(); i++) tmpCharArray[i] = '*';
                        
                returnValue = new(new string(tmpCharArray));

                return (type == PrintType.String ? sFormatReturn(returnValue) : aFormatReturn(returnValue));

                ANSIString aFormatReturn(string str1)
                {
                    string str2 = spacer_between_lineNumber_and_modName;

                    return new($"{GetString(str1, Mod.ConsoleColor.Asterix)}{str2}");
                }

                string sFormatReturn(string str1)
                {
                    string str2 = spacer_between_lineNumber_and_modName;

                    return new($"{str1}{str2}");
                }
            }
            private static string sPrint_asterix_ToFormat(Global global)
            {
                return (string)oPrint_asterix_ToFormat(PrintType.String, global);
            }
            private static ANSIString aPrint_asterix_ToFormat(Global global)
            {
                return (ANSIString)oPrint_asterix_ToFormat(PrintType.ANSIString, global);
            }
            #endregion
            #region Print small section - Base methods
            private static object Print_left______numbers(PrintType type, Global global)
            {
                int line_number = -1;

                int has_line_number_collapsed = 0;

                if (Setting.disable_showing_line_numbers)
                {
                    _ = give_line_number();
                    return (type == PrintType.ANSIString ? new ANSIString(string.Empty) : string.Empty );
                }
                else
                {
                    bool  firstCondition = !global.counter.mod.HasJar;
                    bool secondCondition =  global.counter.IsRoot;

                    bool c = Setting.display_self_library == true &&
                             Setting.never_show_self_library_as_root_item__in_final_display == false &&
                             firstCondition &&
                             secondCondition;

                    /***/  string s1 = new((string)give_line_number_formatted(PrintType.String, global, c, give_line_number()));
                    const  string s2 = separator_between_lineNumber_and_modID;
                    StringBuilder sb = new();
                    sb.Append(s1);
                    sb.Append(s2);
                    string sConcat = sb.ToString();

                    if (type == PrintType.String)
                    {
                        return sConcat;
                    }

                    ANSIString    a1 = (ANSIString)give_line_number_formatted(PrintType.ANSIString, global, c, give_line_number());
                    ANSIString    a2 = new(s2);
                    StringBuilder ab = new();
                    ab.Append(a1);
                    ab.Append(a2);

                    return new ANSIString(ab.ToString());

                    static object give_line_number_formatted(PrintType type_, Global global_, bool condition, in int line_number_)
                    {
                        if (condition)
                        {
                            if (type_ == PrintType.String)
                            {
                                return sPrint_asterix_ToFormat(global_);
                            }
                            if (type_ == PrintType.ANSIString)
                            {
                                return aPrint_asterix_ToFormat(global_);
                            }
                            throw new Exception("");
                        }
                        else
                        {
                            if (type_ == PrintType.String)
                            {
                                return sPrintToFormat(global_, in line_number_);
                            }
                            if (type_ == PrintType.ANSIString)
                            {
                                return aPrintToFormat(global_, in line_number_);
                            }
                            throw new Exception("");
                        }
                    }
                }

                int give_line_number()
                {
                    has_line_number_collapsed++;

                    if (has_line_number_collapsed == 1)
                    {
                        line_number = global.counter.Collapse;
                    }
                    return line_number;
                }
            }
            private static object Print_middle____depthSpaces_and_modID_and_tab(PrintType type, Mod mod, string modID, int depth)
            {
                string        s1 = new(depthSpaces(depth));
                string        s2 = new(modID);
                StringBuilder sb = new();
                sb.Append(s1);
                sb.Append(s2);
                string sConcat = sb.ToString();

                if (type == PrintType.String)
                {
                    return sConcat;
                }

                ANSIString    a1 =       new(s1);
                ANSIString    a2 = GetString(s2, mod);
                StringBuilder ab = new();
                ab.Append(a1);
                ab.Append(a2);

                string tabs;

                int left = Menu.PrintProperties.CharCount_longest_depthSpaces_and_modID;
                int right = sConcat.Length;
                int result = left - right;

                tabs = write_spaces(result);
                ab.Append($"{tabs}{separator_between_modID_and_jar}");

                return new ANSIString(ab.ToString());
            }
            private static object Print_right_____jars_and_tab(PrintType type, Mod mod, string jar)
            {
                string        s1 = new(jar);
                string        s2 = string.Empty;
                StringBuilder sb = new();
                sb.Append(s1);
                sb.Append(s2);
                string sConcat = sb.ToString();

                if (type == PrintType.String)
                {
                    return sConcat;
                }

                ANSIString    a1 = GetString(s1, mod);
                ANSIString    a2 = new(s2);
                StringBuilder ab = new();
                ab.Append(a1);
                ab.Append(a2);

                string tabs;

                int left = Menu.PrintProperties.CharCount_longest_jar;
                int right = sConcat.Length;
                int result = left - right;

                tabs = write_spaces(result);
                ab.Append($"{tabs}");

                return new ANSIString(ab.ToString());
            }
            #endregion
            #region Print small section - End methods
            public static ANSIString PrintA_left______numbers(Global global)
            {
                return (ANSIString)Print_left______numbers(PrintType.ANSIString, global);
            }
            public static ANSIString PrintA_middle____depthSpaces_and_modID_and_tab(Mod mod, string modID, int depth)
            {
                return (ANSIString)Print_middle____depthSpaces_and_modID_and_tab(PrintType.ANSIString, mod, modID, depth);
            }
            public static ANSIString PrintA_right_____jars(Mod mod, string jar)
            {
                return (ANSIString)Print_right_____jars_and_tab(PrintType.ANSIString, mod, jar);
            }
            public static string PrintS_left______numbers(Global global)
            {
                return (string)Print_left______numbers(PrintType.String, global);
            }
            public static string PrintS_middle____depthSpaces_and_modID_and_tab(Mod mod, string modID, int depth)
            {
                return (string)Print_middle____depthSpaces_and_modID_and_tab(PrintType.String, mod, modID, depth);
            }
            public static string PrintS_right_____jars(Mod mod, string jar)
            {
                return (string)Print_right_____jars_and_tab(PrintType.String, mod, jar);
            }
            #endregion
            public static ANSIString Print_Line(Global global)
            {
                StringBuilder sb = new();

                Mod mod = global[global.counter.mod.ID];
                
                #region left
                ANSIString left   = PrintA_left______numbers                       (global);
                #endregion

                #region middle
                string modID = new(mod.ID);
                int    depth = global.counter.Depth;

                ANSIString middle = PrintA_middle____depthSpaces_and_modID_and_tab (mod, modID, depth);
                #endregion

                #region right
                string jar = new(mod.Jar);

                ANSIString right  = PrintA_right_____jars                          (mod, jar);
                #endregion

                sb.Append(left);
                sb.Append(middle);
                sb.Append(right);

                return new(sb.ToString());
            }
        }
    }
}