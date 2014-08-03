using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileFormat
{
    class TypeHandler
    {
        public static void AssignToObject(string value, ref object obj, TypeCode type, int lineNum)
        {
            switch (type)
            {
                case TypeCode.Boolean:
                    if (value == "true") obj = true;
                    else if (value == "false") obj = false;
                    else Program.PrintError("Invalid boolean value. Must be true or false.", lineNum);
                    break;

                case TypeCode.Byte:
                    {
                        byte result;

                        if (byte.TryParse(value, out result)) obj = result;
                        else Program.PrintError("Invalid byte value. Must be an integer between 0 and 255.", lineNum);
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
                            Program.PrintError("Char must be wrapped in single-quotes.", lineNum);
                        }
                        // strip quotes, etc.

                        char result;

                        if (char.TryParse(v, out result)) obj = result;
                        else Program.PrintError("Invalid char value.", lineNum);
                        break;
                    }

                case TypeCode.Decimal:
                    {
                        decimal result;

                        if (decimal.TryParse(value, out result)) obj = result;
                        else Program.PrintError("Invalid decimal value.", lineNum);
                        break;
                    }

                case TypeCode.Double:
                    {
                        double result;

                        if (double.TryParse(value, out result)) obj = result;
                        else Program.PrintError("Invalid decimal value.", lineNum);
                        break;
                    }

                case TypeCode.Int16:
                    {
                        Int16 result;

                        if (Int16.TryParse(value, out result)) obj = result;
                        else Program.PrintError("Invalid int16 value.", lineNum);
                        break;
                    }

                case TypeCode.Int32:
                    {
                        Int32 result;

                        if (Int32.TryParse(value, out result)) obj = result;
                        else Program.PrintError("Invalid int32 value.", lineNum);
                        break;
                    }

                case TypeCode.Int64:
                    {
                        Int64 result;

                        if (Int64.TryParse(value, out result)) obj = result;
                        else Program.PrintError("Invalid int64 value.", lineNum);
                        break;
                    }

                case TypeCode.SByte:
                    {
                        sbyte result;

                        if (sbyte.TryParse(value, out result)) obj = result;
                        else Program.PrintError("Invalid sbyte value.", lineNum);
                        break;
                    }

                case TypeCode.Single:
                    {
                        Single result;

                        if (Single.TryParse(value, out result)) obj = result;
                        else Program.PrintError("Invalid single value.", lineNum);
                        break;
                    }

                case TypeCode.String:
                    {
                        // write special parsing code (strip quotes, etc.)
                        if (value.StartsWith("\"") & value.EndsWith("\""))
                        {
                            string v = value.Trim('\"');
                            ReplaceEscapes(value);
                            obj = v;
                        }
                        else
                        {
                            Program.PrintError("Strings must be wrapped in double-quotes.", lineNum);
                        }
                        break;
                    }

                case TypeCode.UInt16:
                    {
                        UInt16 result;

                        if (UInt16.TryParse(value, out result)) obj = result;
                        else Program.PrintError("Invalid uint16 value.", lineNum);
                        break;
                    }

                case TypeCode.UInt32:
                    {
                        UInt32 result;

                        if (UInt32.TryParse(value, out result)) obj = result;
                        else Program.PrintError("Invalid uint32 value.", lineNum);
                        break;
                    }

                case TypeCode.UInt64:
                    {
                        UInt64 result;

                        if (UInt64.TryParse(value, out result)) obj = result;
                        else Program.PrintError("Invalid uint64 value.", lineNum);
                        break;
                    }
            }
        }

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
    }
}
