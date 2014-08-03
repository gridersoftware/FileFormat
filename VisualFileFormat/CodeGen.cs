﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace VisualFileFormat
{
    class CodeGen
    {
        List<string> output;
        frmMain wnd;

        #region ParseState

        CompoundType root;
        CompoundType current = null;
        CompoundType newest;

        public struct Constant
        {
            public TypeCode typecode;
            public string value;
        }

        Dictionary<string, Constant> constants;
        List<string> includes;

        string @namespace;

        bool ffinit = false;    // determines if the #FORMATNAME directive has been used
        bool nsinit = false;    // determines if the #NAMESPACE directive has been used
        bool xdirect = false;   // determines if the code has exited the directive phase (eXited DIRECTives);

        int lineNumber = 0;

        /// <summary>
        /// Gets a value determining if the required directives have been used
        /// </summary>
        bool Initialized { get { return (ffinit & nsinit); } }

        enum StructState
        {
            StructDeclare,
            StructOpen
        }

        Stack<StructState> structState = new Stack<StructState>();

        #endregion

        /// <summary>
        /// Creates a new instance of CodeGen.
        /// </summary>
        public CodeGen(string inputFile, string outputFile, frmMain window)
        {
            wnd = window; 

            // Create a new string list and add dependancies
            output = new List<string>();
            
            includes = new List<string>(new string[] { "System", "System.IO", "System.Text", "System.Collections.Generic" });
            
            // Setup constants
            constants = new Dictionary<string, Constant>();

            // Parse file and output result
            try
            {
                ParseFile(inputFile);
            }
            catch (FileNotFoundException)
            {
                wnd.PrintError("File \"" + inputFile + "\" not found", 0);
                return;
            }
            catch (Exception ex)
            {
                wnd.PrintError("Unhandled exception: " + ex.Message, lineNumber);
                Console.WriteLine(ex.StackTrace);
                return;
            }

            // Setup namespaces
            foreach (string s in includes)
            {
                output.Add("using " + s + ";");
            }

            output.Add("");

            output.Add("namespace " + @namespace);
            output.Add("{");

            // Add class code
            output.AddRange(root.GetStructCode(1, constants));

            // Close namespace
            output.Add("}");

            if (CompileFlags.PrintOutput) PrintFile(output.ToArray());
            File.WriteAllLines(outputFile, output.ToArray());
        }

        private void PrintFile(string[] lines)
        {
            foreach (string line in lines)
            {
                Console.WriteLine(line);
            }
        }

        private void ParseFile(string file)
        {
            string[] lines;

            if (!File.Exists(file)) throw new FileNotFoundException();
            lines = File.ReadAllLines(file);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                line = line.Trim();
                string[] parts = line.Split(' ');

                lineNumber = i + 1;

                if (line != "")
                {
                    if (line.StartsWith("#")) ParseDirective(line);
                    else if ((!Regex.IsMatch(line, "(^//.*)|(^/\\*.*\\*/$)"))   // single-line comment
                            && (!Regex.IsMatch(line, "^(/\\*).*"))              // start multi-line comment
                            && (!Regex.IsMatch(line, ".*(\\*/)$")))             // end multi-line comment
                    {
                        // otherwise, check if the #FORMATNAME directive has been used
                        if (!Initialized)
                            Console.WriteLine("Syntax error: expected \"#FORMATNAME\". Line {0}", i + 1);
                        else
                            ParseLine(line);
                    }
                }
            }
        }

        void ParseDirective(string line)
        {
            string[] parts = line.Split(' ');

            if (!xdirect)
            {
                if (Regex.IsMatch(line, "#FORMATNAME [A-Za-z]+[a-z0-9]{0,}"))     // #FORMATNAME directive
                {
                    root = new CompoundType(parts[1], parts[1], 1, null);
                    current = root;

                    // check if this directive has been used. if not, initialize. otherwise, print an error
                    if (!ffinit) ffinit = true;
                    else wnd.PrintError("#FORMATNAME directive can only be used once.", lineNumber);
                }
                else if (Regex.IsMatch(line, "#INCLUDE (([_A-Za-z][\\w]+)\\.?)*([_A-Za-z][\\w]+)$"))
                {
                    includes.Add(parts[1]);
                }
                else if (Regex.IsMatch(line, "#NAMESPACE @?(([_A-Za-z][\\w]+)\\.?)*([_A-Za-z][\\w]+)$"))  // #NAMESPACE directive
                {
                    // TODO: Fix this
                    @namespace = parts[1];
                    if (!nsinit) nsinit = true;
                    else wnd.PrintError("#NAMESPACE directive can only be used once.", lineNumber);
                }
                else if (Regex.IsMatch(line, "#CONST (bool|s?byte|u?int(16|32|64)|float|double|decimal|string|char) @?(([_A-Za-z][\\w]+)\\.?)*([_A-Za-z][\\w]+)$ .*"))
                {
                    string[] substring;
                    string val = "";

                    if (parts[1] == "string")
                    {
                        substring = Regex.Split(line, "(\")");
                        if (substring.Length == 4)
                            val = substring[2] + substring[3] + substring[4];
                        else
                            wnd.PrintError("Invalid string value.", lineNumber);
                    }
                    else if (parts[1] == "char")
                    {
                        substring = Regex.Split(line, "(\')");
                        if (substring.Length == 4)
                            val = substring[2] + substring[3] + substring[4];
                        else
                            wnd.PrintError("Invalid char value.", lineNumber);
                    }
                    else
                    {
                        val = parts[3];
                    }

                    constants.Add(parts[2], new Constant() { typecode = Variable.GetTypeCode(parts[1]), value = val });
                }
                else if (Regex.IsMatch(line, "#MAGICNUMBER (bool|s?byte|u?int(16|32|64)|float|double|decimal|string|char) .*")) // #MAGICNUMBER directive
                {
                    string[] substring;
                    string val = "";

                    if (parts[1] == "string")
                    {
                        substring = Regex.Split(line, "(\")");
                        if (substring.Length == 5)
                            val = substring[1] + substring[2] + substring[3];
                        else
                            wnd.PrintError("Invalid string value.", lineNumber);
                    }
                    else if (parts[1] == "char")
                    {
                        substring = Regex.Split(line, "(\')");
                        if (substring.Length == 5)
                            val = substring[1] + substring[2] + substring[3];
                        else
                            wnd.PrintError("Invalid char value.", lineNumber);
                    }
                    else
                    {
                        val = parts[2];
                    }

                    constants.Add("MAGICNUMBER", new Constant() { typecode = Variable.GetTypeCode(parts[1]), value = val });

                    // assign to magic number
                }
                else if (Regex.IsMatch(line, "#ENCODING (utf7|utf8|utf32|unicode|ascii)")) // #ENCODING directive
                {
                    constants.Add("ENCODING", new Constant() { typecode = TypeCode.String, value = parts[1] });
                }
            }
        }

        void ParseLine(string line)
        {
            xdirect = true;    // set that there are no more directives
            string[] parts = line.Split(' ');

            if (Regex.IsMatch(line, "(bool|s?byte|u?int(16|32|64)|float|double|decimal|string|char) \\$[a-z]*[a-z0-9]{0,} :: (MAGICNUMBER|\\$[a-z]*[a-z0-9]{0,})")) // single variable with compare
            {
                // setup aliases for the parts
                string type = parts[0];
                string name = parts[1];
                string cmpname = parts[3];

                CreateVariable(name, type, cmpname);
            }
            else if (Regex.IsMatch(line, "(bool|s?byte|u?int(16|32|64)|float|double|decimal|string|char) \\$[a-z]*[a-z0-9]{0,}"))   // single variable (no compare)
            {
                CreateVariable(parts[1], parts[0]);
            }
            else if (Regex.IsMatch(line, "(bool|s?byte|u?int(16|32|64)|float|double|decimal|string|char)\\[([0-9]*|(\\$[a-z]*[a-z0-9]{0,}))] \\$[a-z]*[a-z0-9]{0,}")) // array variable
            {
                string[] substrings = Regex.Split(line, "\\[|\\]| ");
                
                string type = substrings[0];
                string countVar = substrings[1];
                string name = substrings[3];
                int count;

                // if the element count is a variable
                if (countVar.StartsWith("$"))
                    CreateArray(name, type, countVar);
                else
                {
                    if (int.TryParse(substrings[1], out count))
                        CreateArray(name, type, count);
                    else
                        wnd.PrintError("Value \"" + countVar + "\" is not a variable or int32 constant.", lineNumber);
                }
            }
            else if (Regex.IsMatch(line, "tree [A-Za-z]*[a-z0-9]{0,} \\$[a-z]*[a-z0-9]{0,}"))
            {
                string structName = parts[1];
                string name = parts[2];

                newest = CreateTree(name, structName);
                structState.Push(StructState.StructDeclare);
            }
            else if (Regex.IsMatch(line, "tree\\[([0-9]*|(\\$[a-z]*[a-z0-9]{0,}))] [A-Za-z]*[a-z0-9]{0,} \\$[a-z]*[a-z0-9]{0,}"))
            {
                if (!includes.Contains("GSLib.Collections.Trees")) wnd.PrintWarning("You should #INCLUDE GSLib.Collections.Trees to use the Tree<> class.", lineNumber);

                string[] substrings = Regex.Split(line, "\\[|\\]| ");

                string countVal = substrings[1];
                string structName = substrings[3];
                string name = substrings[4];
                int count;

                if ((structState.Count > 0) && (structState.Peek() == StructState.StructDeclare)) wnd.PrintError("\'{\' Expected.", lineNumber);

                // if the element count is a variable
                if (countVal.StartsWith("$"))
                    newest = CreateTree(name, structName, countVal);
                else
                {
                    if (int.TryParse(countVal, out count))
                        newest = CreateTree(name, structName, count);
                    else
                        wnd.PrintError("Value \"" + countVal + "\" is not a variable or int32 constant.", lineNumber);
                }

                structState.Push(StructState.StructDeclare);
            }
            else if (Regex.IsMatch(line, "struct\\[([0-9]*|(\\$[a-z]*[a-z0-9]{0,}))] [A-Za-z]*[a-z0-9]{0,} \\$[a-z]*[a-z0-9]{0,}"))  // struct
            {
                string[] substrings = Regex.Split(line, "\\[|\\]| ");

                string countVal = substrings[1];
                string structName = substrings[3];
                string name = substrings[4];
                int count;

                if ((structState.Count > 0) && (structState.Peek() == StructState.StructDeclare)) wnd.PrintError("\'{\' Expected.", lineNumber);

                // if the element count is a variable
                if (countVal.StartsWith("$"))
                    newest = CreateStruct(name, structName, countVal);
                else
                {
                    if (int.TryParse(countVal, out count))
                        newest = CreateStruct(name, structName, count);
                    else
                        wnd.PrintError("Value \"" + countVal + "\" is not a variable or int32 constant.", lineNumber);
                }

                structState.Push(StructState.StructDeclare);
            }
            else if (line == "{") // struct block opening
            {
                if ((structState.Count > 0) && (structState.Peek() == StructState.StructOpen)) wnd.PrintError("\'}\' Expected.", lineNumber);
                else if (structState.Count == 0) wnd.PrintError("Unexpected \'{\'.", lineNumber);

                current = newest;
                structState.Push(StructState.StructOpen);
            }
            else if (line == "}") // struct block closing
            {
                if ((structState.Count > 0) && (structState.Peek() == StructState.StructDeclare)) wnd.PrintError("\'{\' Expected.", lineNumber);
                else if (structState.Count == 0) wnd.PrintError("Unexpected \'}\'.", lineNumber);

                if (current.Parent != null) current = current.Parent;

                if (structState.Count >= 2)
                {
                    structState.Pop();
                    structState.Pop();
                }
            }
            else
            {
                wnd.PrintError("Syntax error.", lineNumber);
            }
        }

        void PrintException(Exception ex, int line)
        {
            wnd.PrintError("Internal - " + ex.Message, line);
        }

        bool CheckForGlobalName(string name)
        {
            CompoundType head = root;

            if (head.ContainsName(name))
            {
                wnd.PrintError("Variable or struct with the name \"" + name + "\" already exists.", lineNumber);
                return true;
            }
            return false;
        }

        bool TryParseLocalName(string name, out Variable v)
        {
            v = null;

            try
            {
                v = current.Variables[name];
                if (v != null) return true;
                else return false;
            }
            catch
            {
                return false;
            }

        }

        bool TryParseTypeCode(string s, out TypeCode type)
        {
            type = Variable.GetTypeCode(s);
            if (type == TypeCode.Empty)
            {
                wnd.PrintError("Invalid type \"" + s + "\".", lineNumber);
                return false;
            }
            else if ((type == TypeCode.String) | (type == TypeCode.Char))
            {
                // if the user used a string or char without defining encoding, print a warning
                if (!constants.ContainsKey("ENCODING")) 
                    wnd.PrintWarning("#ENCODING directive not used. No Encoding set for string and char values.", lineNumber);
            }
            return true;
        }

        void CreateVariable(string name, string type)
        {
            TypeCode t;
            Variable v;

            // Check for problems
            if (CheckForGlobalName(name) | !TryParseTypeCode(type, out t)) return;

            // try to create variable. If this generates an error, there's a major problem somewhere
            try
            {
                v = new Variable(name, t);
                current.Variables.Add(v);
            }
            catch (Exception ex)
            {
                wnd.PrintError("@CodeGen.CreateVariable(" + name + ", " + type + ") - " + ex.Message, lineNumber);
            }
        }

        void CreateVariable(string name, string type, string cmpName)
        {
            TypeCode t;
            Variable v;
            string cmp;

            if (CheckForGlobalName(name) | !TryParseTypeCode(type, out t)) return;

            if (cmpName == "MAGICNUMBER")
                cmp = cmpName;
            else if (CheckForGlobalName(cmpName))
                cmp = cmpName;
            else
                return;

            try
            {
                v = new Variable(name, t);
                v.CompareWith = cmp;
                current.Variables.Add(v);
            }
            catch (Exception ex)
            {
                wnd.PrintError("@CodeGen.CreateVariable(" + name + ", " + type + "," + cmpName + ") - " + ex.Message, lineNumber);
            }
        }

        void CreateArray(string name, string type, int count)
        {
            TypeCode t;
            Variable v;

            // Check for problems. If something isn't right, exit.
            if (CheckForGlobalName(name) | !TryParseTypeCode(type, out t)) return;
            if (count <= 1)
            {
                wnd.PrintError("Cannot create an array with less than 2 elements.", lineNumber);
                return;
            }

            // Try to create variable. If this generates an error, there's major problem somewhere.
            try
            {
                v = new Variable(name, count, t);
                current.Variables.Add(v);
            }
            catch (Exception ex)
            {
                wnd.PrintError("@CodeGen.CreateArray(" + name + "," + count.ToString() + "," + type + ") - " + ex.Message, lineNumber);
            }
        }

        void CreateArray(string name, string type, string countVar)
        {
            TypeCode t;
            Variable v, cv;

            // Check for problems. If something isn't right, exit.
            if (CheckForGlobalName(name) | !TryParseTypeCode(type, out t)) return;
            if (!TryParseLocalName(countVar, out cv)) return;
            if (cv.TypeCode != TypeCode.Int32)
            {
                wnd.PrintError("Count variable must be an int32.", lineNumber);
                return;
            }

            try
            {
                v = new Variable(name, cv, t);
                current.Variables.Add(v);
            }
            catch (Exception ex)
            {
                wnd.PrintError("@CodeGen.CreateArray(" + name + "," + countVar + "," + type + ") - " + ex.Message, lineNumber);
            }
        }

        CompoundType CreateStruct(string name, string structName, int count)
        {
            CompoundType c = null;

            if (CheckForGlobalName(name)) return null;
            if (count <= 1)
            {
                wnd.PrintError("Cannot create an array with less than 2 elements.", lineNumber);
                return null;
            }

            try
            {
                c = new CompoundType(name, structName, count, current);
                current.Structs.Add(c);
            }
            catch (Exception ex)
            {
                wnd.PrintError("@CodeGen.CreateStruct(" + name + "," + structName + "," + count.ToString() + ") - " + ex.Message, lineNumber);
            }

            return c;
        }

        CompoundType CreateStruct(string name, string structName, string countVar)
        {
            Variable cv;
            CompoundType c = null;

            if (CheckForGlobalName(name)) return null;
            if (!TryParseLocalName(countVar, out cv)) return null;

            try
            {
                c = new CompoundType(name, structName, cv, current);
                current.Structs.Add(c);
            }
            catch (Exception ex)
            {
                wnd.PrintError("@CodeGen.CreateStruct(" + name + "," + structName + "," + countVar + ") - " + ex.Message, lineNumber);
            }

            return c;
        }

        CompoundType CreateTree(string name, string typeName)
        {
            CompoundType c = null;

            if (CheckForGlobalName(name)) return null;
            try
            {
                c = CompoundType.CreateTree(name, typeName, 1, current);
                current.Structs.Add(c);
            }
            catch (Exception ex)
            {
                wnd.PrintError("@CodeGen.CreateTree(" + name + "," + typeName + ") - " + ex.Message, lineNumber);
                return null;
            }

            return c;
        }

        CompoundType CreateTree(string name, string typeName, int count)
        {
            CompoundType c = null;

            if (CheckForGlobalName(name)) return null;
            if (count < 1)
            {
                wnd.PrintError("Cannot create a tree with less than 1 root node.", lineNumber);
                return null;
            }

            try
            {
                c = CompoundType.CreateTree(name, typeName, count, current);
                current.Structs.Add(c);
            }
            catch (Exception ex)
            {
                wnd.PrintError("@CodeGen.CreateTree(" + name + "," + typeName + "," + count.ToString() + ") - " + ex.Message, lineNumber);
            }

            return c;
        }

        CompoundType CreateTree(string name, string typeName, string countVar)
        {
            Variable cv;
            CompoundType c = null;

            if (CheckForGlobalName(name)) return null;
            if (!TryParseLocalName(countVar, out cv)) return null;

            try
            {
                c = CompoundType.CreateTree(name, typeName, cv, current);
                current.Structs.Add(c);
            }
            catch (Exception ex)
            {
                wnd.PrintError("@CodeGen.CreateTree(" + name + "," + typeName + "," + countVar + ") - " + ex.Message, lineNumber);
            }

            return c;
        }
    }
}
