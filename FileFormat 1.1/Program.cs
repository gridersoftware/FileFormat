using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace FileFormat
{
    class Program
    {
        static bool errorsOccured = false;

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                PrintHelp();
                return;
            }
            string inputFile = args[0];
            string outputFile = args[1];
          
            // set flag defaults
            CompileFlags.ImplicitCounts = false;
            CompileFlags.Lang = CompileFlags.OutputLanguage.CSharp;
            CompileFlags.PrintOutput = false;

            if (args.Length >= 3)
            {
                for (int i = 2; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "/ic":
                            CompileFlags.ImplicitCounts = true;
                            break;

                        case "/lang:cs":
                            CompileFlags.Lang = CompileFlags.OutputLanguage.CSharp; 
                            break;

                        case "/lang:vb":
                            CompileFlags.Lang = CompileFlags.OutputLanguage.VisualBasic;
                            break;

                        case "/p":
                            CompileFlags.PrintOutput = true;
                            break;

                        default:
                            PrintHelp();
                            return;
                    }
                }
            }

            if (CompileFlags.Lang == CompileFlags.OutputLanguage.CSharp)
            {
                CodeGen<Languages.CSharp> code = new CodeGen<Languages.CSharp>(inputFile, outputFile, Languages.CSharp.MakeFileStructure();
            }

            if (errorsOccured) Console.WriteLine("Code Generation failed.");
            else Console.WriteLine("Code Generation Succeeded.");

#if DEBUG
            Console.ReadLine();
#endif
        }

        private static void PrintHelp()
        {
            Console.WriteLine("ff INPUTFILE OUTPUTFILE [/ic] [/lang:cs | /lang:vb]");
            Console.WriteLine("");
            Console.WriteLine("INPUTFILE - FileFormat file to read.");
            Console.WriteLine("OUTPUTFILE - Filename to write to.");
            Console.WriteLine("/p - Determines whether to print the output to the console.");
            //Console.WriteLine("/ic - Implicitly determine values for array lengths based on array inputs.");
            Console.WriteLine("/lang - Determines which language to output. By default,\n\tFileFormat outputs C# code, so this is optional.");
            Console.WriteLine("\t:cs - Output C# code.");
            //Console.WriteLine("\t:vb - Output Visual Basic.Net code. This option is not\n\timplemented yet.");
        }

        public static void ValidateValueByType(string value, TypeCode type, int lineNum)
        {
            switch (type)
            {
                case TypeCode.Boolean:
                    if ((value != "true") && (value != "false")) PrintError("Invalid boolean value. Must be true or false.", lineNum);
                    break;

                case TypeCode.Byte:
                    {
                        byte result;

                        if (!(byte.TryParse(value, out result))) PrintError("Invalid byte value. Must be an integer between 0 and 255.", lineNum);
                        break;
                    }

                case TypeCode.Char:
                    {
                        string v = "";
                        if (value.StartsWith("\'") && value.EndsWith("\'"))
                        {
                            v = value.Trim('\'');
                            v = ReplaceEscapes(v);
                        }
                        else
                        {
                            PrintError("Char must be wrapped in single-quotes.", lineNum);
                        }
                        // strip quotes, etc.

                        char result;

                        if (!char.TryParse(v, out result)) PrintError("Invalid char value.", lineNum);
                        break;
                    }

                case TypeCode.Decimal:
                    {
                        decimal result;

                        if (!(decimal.TryParse(value, out result))) PrintError("Invalid decimal value.", lineNum);
                        break;
                    }

                case TypeCode.Double:
                    {
                        double result;

                        if (!(double.TryParse(value, out result))) PrintError("Invalid decimal value.", lineNum);
                        break;
                    }

                case TypeCode.Int16:
                    {
                        Int16 result;

                        if (!(Int16.TryParse(value, out result))) PrintError("Invalid int16 value.", lineNum);
                        break;
                    }

                case TypeCode.Int32:
                    {
                        Int32 result;

                        if (!(Int32.TryParse(value, out result))) PrintError("Invalid int32 value.", lineNum);
                        break;
                    }

                case TypeCode.Int64:
                    {
                        Int64 result;

                        if (!(Int64.TryParse(value, out result))) PrintError("Invalid int64 value.", lineNum);
                        break;
                    }

                case TypeCode.SByte:
                    {
                        sbyte result;

                        if (!(sbyte.TryParse(value, out result))) PrintError("Invalid sbyte value.", lineNum);
                        break;
                    }

                case TypeCode.Single:
                    {
                        Single result;

                        if (!(Single.TryParse(value, out result))) PrintError("Invalid single value.", lineNum);
                        break;
                    }

                case TypeCode.String:
                    {
                        FixString(value, lineNum);
                        break;
                    }

                case TypeCode.UInt16:
                    {
                        UInt16 result;

                        if (!(UInt16.TryParse(value, out result))) PrintError("Invalid uint16 value.", lineNum);
                        break;
                    }

                case TypeCode.UInt32:
                    {
                        UInt32 result;

                        if (!(UInt32.TryParse(value, out result))) PrintError("Invalid uint32 value.", lineNum);
                        break;
                    }

                case TypeCode.UInt64:
                    {
                        UInt64 result;

                        if (!(UInt64.TryParse(value, out result))) PrintError("Invalid uint64 value.", lineNum);
                        break;
                    }
            }
        }

        /// <summary>
        /// Prints an error to standard output.
        /// </summary>
        /// <param name="errmsg">Error message to print.</param>
        /// <param name="lineNumber">Line number associated with error.</param>
        public static void PrintError(string errmsg, int lineNumber)
        {
            errorsOccured = true;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: {0}. Line {1}", errmsg, lineNumber);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// Prints a warning to standard output.
        /// </summary>
        /// <param name="warningmsg">Warning message to print.</param>
        /// <param name="lineNumber">Line number associated with warning.</param>
        public static void PrintWarning(string warningmsg, int lineNumber)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Warning: {0}. Line {1}", warningmsg, lineNumber);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// Replaces string representations of escape sequences with true escape sequences.
        /// </summary>
        /// <param name="s">String containing escape sequences.</param>
        /// <returns>Returns a string containing the replaced sequences.</returns>
        static string ReplaceEscapes(string s)
        {
            string result = s;

            result = result.Replace("\\\\", "\\");
            result = result.Replace("\\a", "\a");
            result = result.Replace("\\b", "\b");
            result = result.Replace("\\f", "\f");
            result = result.Replace("\\n", "\n");
            result = result.Replace("\\r", "\r");
            result = result.Replace("\\t", "\t");
            result = result.Replace("\\v", "\\v");
            result = result.Replace("\\0", "\0");

            return result;
        }

        /// <summary>
        /// Removes wrapping quotes of a string and replaces escape sequences.
        /// </summary>
        /// <param name="s">String to fix.</param>
        /// <param name="lineNum">Line number associated with string.</param>
        /// <returns>Returns fixed string.</returns>
        public static void FixString(string s, int lineNum)
        {
            if (!(s.StartsWith("\"") & s.EndsWith("\"")))
                PrintError("Strings must be wrapped in double-quotes.", lineNum);
        }

        public static string GetEncodingString(string ffEncoding)
        {
            switch (ffEncoding)
            {
                case "utf7": return "Encoding.UTF7";
                case "utf8": return "Encoding.UTF8";
                case "utf32": return "Encoding.UTF32";
                case "unicode": return "Encoding.Unicode";
                case "ascii": return "Encoding.ASCII";
                default: return "";
            }
        }
    }
}
