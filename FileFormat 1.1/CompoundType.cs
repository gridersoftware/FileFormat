using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileFormat.Languages;

namespace FileFormat
{
    class CompoundType<T> : Variable 
        where T : Language
    {
        VariableCollection variables;
        CompoundTypeCollection<T> structs;

        public static CompoundType<T> CreateTree(string name, string type, int count, CompoundType<T> parent)
        {
            CompoundType<T> tree = new CompoundType<T>(name, type, count, parent);
            tree.IsTree = true;
            return tree;
        }

        public static CompoundType<T> CreateTree(string name, string type, Variable count, CompoundType<T> parent)
        {
            CompoundType<T> tree = new CompoundType<T>(name, type, count, parent);
            tree.IsTree = true;
            return tree;
        }

        /// <summary>
        /// Creates a new CompoundType instance.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <param name="structName">Structure name</param>
        /// <param name="count">Number of elements in the array.</param>
        /// <param name="parent">Parent structure.</param>
        public CompoundType(string name, string structName, int count, CompoundType<T> parent)
            : base(name, count, TypeCode.Object) 
        {
            variables = new VariableCollection();
            structs = new CompoundTypeCollection<T>();
            Parent = parent;
            StructName = structName;
            GenericType = null;
            IsTree = false;
        }

        /// <summary>
        /// Creates a new CompoundType instance.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <param name="structName">Structure name.</param>
        /// <param name="countref">Variable containing the number of elements in the array.</param>
        /// <param name="parent">Parent structure.</param>
        public CompoundType(string name, string structName, Variable countref, CompoundType<T> parent)
            : base(name, countref, TypeCode.Object)
        {
            variables = new VariableCollection();
            structs = new CompoundTypeCollection<T>();
            Parent = parent;
            StructName = structName;
            GenericType = null;
            IsTree = false;
        }

        /// <summary>
        /// Gets the variables contained by the structure.
        /// </summary>
        public VariableCollection Variables { get { return variables; } }

        /// <summary>
        /// Gets the structs contained of structures.
        /// </summary>
        public CompoundTypeCollection<T> Structs { get { return structs; } }

        /// <summary>
        /// Gets the parent structure.
        /// </summary>
        public CompoundType<T> Parent { get; private set; }

        /// <summary>
        /// Name of the structure type.
        /// </summary>
        public string StructName { get; private set; }

        /// <summary>
        /// Gets the value determining if the object represents a GSLib.Collections.Trees.Tree.
        /// </summary>
        public bool IsTree { get; private set; }

        /// <summary>
        /// Gets the value determining the type if the object represents a generic, such as a Tree.
        /// </summary>
        public CompoundType<T> GenericType { get; private set; }

        /// <summary>
        /// Gets the argument representation of the structure.
        /// </summary>
        /// <returns>Returns a string representing the structure in C# format.</returns>
        public override string GetArgumentCode()
        {
            if (IsTree)
            {
                if (Count == 1)
                    return "TreeNode<" + StructName + "> " + LowercaseName;
                else
                    return "TreeNode<" + StructName + ">[] " + LowercaseName;
            }

            return StructName + "[] " + LowercaseName;
        }

        public override string GetPropertyCode()
        {
            if (IsTree)
            {
                if (Count == 1)
                    return "public TreeNode<" + StructName + "> " + UppercaseName + " { get; private set; }";
                else
                    return "public TreeNode<" + StructName + ">[] " + UppercaseName + " { get; private set; }";
            }

            return "public " + StructName + "[] " + UppercaseName + " { get; private set; }";
        }

        public CompoundType<T>[] GetTrees()
        {
            List<CompoundType<T>> trees = new List<CompoundType<T>>();

            if (IsTree) trees.Add(this);
            foreach (CompoundType<T> c in structs)
            {
                trees.AddRange(c.GetTrees());
            }

            return trees.ToArray();
        }

        /// <summary>
        /// Gets the C# code representing this struct object.
        /// </summary>
        /// <param name="encoding">Encoding string to use./param>
        /// <returns>Returns the code associated with the given structure.</returns>
        public void GetStructCode(Dictionary<string, Constant> constants, T lang)
        {
            T root = lang;
            T current = root;
            T newest;

            if (Parent == null)
                newest = (T)lang.GetNewMember(StructureType.Class, StructName, AccessModifiers.Public, "");
            else
                newest = (T)lang.GetNewMember(StructureType.Structure, StructName, AccessModifiers.Public, "");

            current.AddMember(newest);
            current = newest;

            // Constants
            if (Parent == null)
            {
                Language constant;
                if (constants.ContainsKey("ENCODING"))
                {
                    constant = current.GetNewMember(StructureType.Variable, "ENCODING", AccessModifiers.Private, "Encoding");
                    constant.DefaultValue = Program.GetEncodingString(constants["ENCODING"].value);

                    current.AddMember(constant);
                }

                if (constants.ContainsKey("MAGICNUMBER"))
                {
                    constant = current.GetNewMember(StructureType.Variable, "MAGICNUMBER", AccessModifiers.Public, constants["MAGICNUMBER"].typecode.ToString());
                    constant.DefaultValue = constants["MAGICNUMBER"].value;

                    current.AddMember(constant);
                }

                foreach (string key in constants.Keys)
                {
                    if ((key != "ENCODING") & (key != "MAGICNUMBER"))
                    {
                        constant.Name = key;
                        constant.DefaultValue = constants[key].value;
                        constant.ReturnType = constants[key].typecode.ToString(); 
                        current.AddMember(constant);
                    }
                }

                if (constants.Count > 0) current.AddBlankLine();
            }

            // Add properties
            foreach (Variable v in variables)
            {
                v.GetPropertyCode(current);
            }
            foreach (CompoundType<T> c in structs)
            {
                c.GetPropertyCode(current);
            }

            current.AddBlankLine();
            //code.Add("");

            // Add structs
            foreach (CompoundType<T> s in structs)
            {
                s.GetStructCode(constants, current);
            }

            GetCreateConstructorCode(tabCount + 1, ref code);

            if (Parent == null)
            {
                GetFromFileConstructorCode(tabCount + 1, constants, ref code);
                code.Add("");
                SaveFileCode(tabCount + 1, ref code, constants);
            }
            else
            {
                GetFromFileConstructorCode(tabCount + 1, ref code);
                code.Add("");
                SaveCode(tabCount + 1, ref code);
            }

            if (Parent == null)
            {
                CompoundType<T>[] trees = GetTrees();
                foreach (CompoundType<T> tree in trees)
                {
                    code.Add("");
                    tree.SaveTree(ref code, tabCount + 1);
                    code.Add("");
                    tree.ReadTree(ref code, tabCount + 1);
                }
            }

            Add("}", tabCount, ref code);
            code.Add("");

            return code.ToArray();
        }

        /// <summary>
        /// Gets the constructor that builds a struct from a file. Specifically for structs.
        /// </summary>
        /// <param name="tabCount"></param>
        /// <param name="code"></param>
        //private void GetFromFileConstructorCode(int tabCount, ref List<string> code)
        private void GetFromFileConstructorCode(T lang)
        {
            T constructor = (T)lang.GetNewMember(StructureType.Constructor, StructName, AccessModifiers.Public, "");
            string constructorCall = constructor.GetMethodCall(constructor, 
            constructor.AddAssignment(lang.ThisObject, lang.NewKeyword + " " lang.GetMethodCall(constructor, 
                new Language[] { lang.GetNewMember(StructureType.Variable, "reader", AccessModifiers.None, "BinaryReader")}));

            //Add("public " + StructName + "(BinaryReader reader)", tabCount, ref code);
            //Add("{", tabCount, ref code);

            //Add("this = new " + StructName + "();", tabCount + 1, ref code);    // initialize with default constructor first

            

            //Add("try", tabCount + 1, ref code);
            //Add("{", tabCount + 1, ref code);

            //ReadVariables(ref code, tabCount + 2);
            //Add("}", tabCount + 1, ref code);
            //Add("catch", tabCount + 1, ref code);
            //Add("{", tabCount + 1, ref code);
            //Add("throw;", tabCount + 2, ref code);
            //Add("}", tabCount + 1, ref code);
            //Add("}", tabCount, ref code);
        }

        /// <summary>
        /// Gets the constructor that builds the struct from a file. Specifically for classes.
        /// </summary>
        /// <returns></returns>
        private void GetFromFileConstructorCode(int tabCount, Dictionary<string, Constant> constants, ref List<string> code)
        {

            /* Constructor declaration */
            Add("public " + StructName + "(string filename)", tabCount, ref code);
            Add("{", tabCount, ref code);
            
            // try block
            Add("try", tabCount + 1, ref code);
            Add("{", tabCount + 1, ref code);
            Add("if (File.Exists(filename))", tabCount + 2, ref code);
            Add("{", tabCount + 2, ref code);

            if (constants.ContainsKey("ENCODING"))
                Add("using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open), ENCODING))", tabCount + 3, ref code);
            else 
                Add("using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))", tabCount + 3, ref code);

            Add("{", tabCount + 3, ref code);

            ReadVariables(ref code, tabCount + 4);

            code.Add("");
            Add("reader.Close();", tabCount + 4, ref code);

            Add("}", tabCount + 3, ref code);
            Add("}", tabCount + 2, ref code);
            Add("else", tabCount + 2, ref code);
            Add("{", tabCount + 2, ref code);
            Add("throw new FileNotFoundException();", tabCount + 3, ref code);
            Add("}", tabCount + 2, ref code);

            Add("}", tabCount + 1, ref code);
            Add("catch", tabCount + 1, ref code);
            Add("{", tabCount + 1, ref code);
            Add("throw;", tabCount + 2, ref code);
            Add("}", tabCount + 1, ref code);
            Add("}", tabCount, ref code);
        }

        private void ReadVariables(ref List<string> code, int tabCount)
        {
            /* Assign variables section */
            StringBuilder sb;

            foreach (Variable v in Variables)
            {
                sb = new StringBuilder("for (int i = 0; i < ");

                if (v.Count == 1)
                    Add(v.UppercaseName + " = " + Variable.GetReadType(v.TypeCode), tabCount, ref code);
                else if (v.TypeCode == System.TypeCode.Byte)
                {
                    sb = new StringBuilder(v.UppercaseName + " = reader.ReadBytes(");
                    if (v.Count > 0) sb.Append(v.Count.ToString());
                    else sb.Append(v.CountRef.UppercaseName);
                    sb.Append(");");
                    Add(sb.ToString(), tabCount, ref code);
                }
                else
                {
                    if (v.CountRef == null) sb.Append(v.Count.ToString() + "; i++)");
                    else sb.Append(v.CountRef.UppercaseName + "; i++)");
                    Add(sb.ToString(), tabCount, ref code);
                    Add("{", tabCount, ref code);
                    Add(v.UppercaseName + "[i] = " + Variable.GetReadType(v.TypeCode), tabCount + 1, ref code);
                    Add("}", tabCount, ref code);
                }
            }

            /* Assign structs section */
            foreach (CompoundType<T> c in structs)
            {
                if (!c.IsTree)
                {
                    if (c.CountRef != null)
                        Add(c.UppercaseName + " = new " + c.StructName + "[" + c.CountRef.UppercaseName + "];", tabCount, ref code);
                    else
                        Add(c.UppercaseName + " = new " + c.StructName + "[" + c.Count + "];", tabCount, ref code);
                }
                else
                {
                    if (c.Count == 1)
                    {
                        Add(c.UppercaseName + " = new TreeNode<" + c.StructName + ">();", tabCount, ref code);
                    }
                    else
                    {
                        if (c.CountRef != null)
                            Add(c.UppercaseName + " = new TreeNode<" + c.StructName + ">[" + c.CountRef.UppercaseName + "];", tabCount, ref code);
                        else
                            Add(c.UppercaseName + " = new TreeNode<" + c.StructName + ">[" + c.Count + "];", tabCount, ref code);
                    }
                }
            }

            if (structs.Count > 0)
                code.Add("");

            foreach (CompoundType<T> c in structs)
            {
                if (c.IsTree & c.Count == 1)
                {
                    Add("ReadTreeNode(ref " + c.UppercaseName + ", reader);", tabCount, ref code);
                }
                else
                {
                    if (c.CountRef != null)
                        Add("for (int i = 0; i < " + c.CountRef.UppercaseName + "; i++)", tabCount, ref code);
                    else
                        Add("for (int i = 0; i < " + c.Count.ToString() + "; i++)", tabCount, ref code);

                    Add("{", tabCount, ref code);

                    if (!c.IsTree)
                        Add(c.UppercaseName + "[i] = new " + c.StructName + "(reader);", tabCount + 1, ref code);
                    else
                        Add("ReadTreeNode(ref " + c.UppercaseName + "[i], reader);", tabCount + 1, ref code);

                    Add("}", tabCount, ref code);
                }
            }
        }

        private void WriteVariables(ref List<string> code, int tabCount)
        {
            StringBuilder sb;

            foreach (Variable v in Variables)
            {
                sb = new StringBuilder("for (int i = 0; i < ");

                if (v.Count == 1)
                {
                    if (v.CompareWith == null)
                        Add("writer.Write(" + v.UppercaseName + ");", tabCount, ref code);
                    else
                        Add("writer.Write(" + v.CompareWith + ");", tabCount, ref code);
                }
                else
                {
                    if (v.CountRef == null) sb.Append(v.Count.ToString() + "; i++)");
                    else sb.Append(v.CountRef.UppercaseName + "; i++)");
                    Add(sb.ToString(), tabCount, ref code);
                    Add("{", tabCount, ref code);
                    Add("writer.Write(" + v.UppercaseName + "[i]);", tabCount + 1, ref code);
                    Add("}", tabCount, ref code);
                }
            }

            foreach (CompoundType<T> s in structs)
            {
                if (s.IsTree & s.Count == 1)
                {
                    Add("SaveTreeNode(" + s.UppercaseName + ", writer);", tabCount, ref code);
                }
                else
                {
                    sb = new StringBuilder("for (int i = 0; i < ");
                    if (s.CountRef == null) sb.Append(s.Count.ToString() + "; i++)");
                    else sb.Append(s.CountRef.UppercaseName + "; i++)");
                    Add(sb.ToString(), tabCount, ref code);
                    Add("{", tabCount, ref code);

                    if (s.IsTree)
                        Add("SaveTreeNode(" + s.UppercaseName + "[i], writer);", tabCount + 1, ref code);
                    else
                        Add(s.UppercaseName + "[i].Save(writer);", tabCount + 1, ref code);

                    Add("}", tabCount, ref code);
                }
            }
        }

        private void GetCreateConstructorCode(T lang)
        {
            T constructor = (T)lang.GetNewMember(StructureType.Method, StructName, AccessModifiers.Public, "");

            if (Parent != null) constructor.AddAssignment(lang.ThisObject, lang.GetNewObjectInstance(lang, new T[0]));

            foreach (Variable v in variables)
            {
                T parameter = (T)lang.GetNewMember(StructureType.Variable, v.LowercaseName, AccessModifiers.None, v.TypeCode.ToString());
                constructor.AddParameter(parameter);
                constructor.AddAssignment(v.UppercaseName, v.LowercaseName);
            }

            lang.AddMember(constructor);

            if (Parent != null) Add("this = new " + StructName + "();", tabCount + 1, ref code);    // initialize with default constructor first

            /* Create assignments */
            foreach (Variable v in variables)
            {
                if ((v.CompareWith != null) && (v.CompareWith != ""))
                    Add("if (" + v.LowercaseName + " != " + v.CompareWith + ") throw new ArgumentException();", tabCount + 1, ref code);

                if (v.Count > 1)
                    Add("if (" + v.LowercaseName + " != " + v.Count.ToString() + ") throw new ArgumentOutOfRangeException();", tabCount + 1, ref code);
                else if (v.Count < 1)
                    Add("if (" + v.LowercaseName + ".Length != " + v.CountRef.LowercaseName + ") throw new ArgumentOutOfRangeException();", tabCount + 1, ref code);

                Add(v.UppercaseName + " = " + v.LowercaseName + ";", tabCount + 1, ref code);
            }

            /* Add structs */
            foreach (CompoundType c in structs)
            {
                // check for invalid size
                if (!CompileFlags.ImplicitCounts)
                {
                    if (c.CountRef == null)
                    {
                        if (c.Count != 1) Add("if (" + c.LowercaseName + ".Length != " + c.Count + ") throw new ArgumentOutOfRangeException();", tabCount + 1, ref code);
                    }
                    else
                    {
                        Add("if (" + c.LowercaseName + ".Length != " + c.CountRef.UppercaseName + ") throw new ArgumentOutOfRangeException();", tabCount + 1, ref code);
                    }
                }

                Add(c.UppercaseName + " = " + c.LowercaseName + ";", tabCount + 1, ref code);
            }
            
            /* Finish first function */
            Add("}", tabCount, ref code);
            code.Add("");
        }

        private void SaveFileCode(int tabCount, ref List<string> code, Dictionary<string, Constant> constants)
        {
            T lang;
            T saveFileMethod 
            //Add("public void SaveFile(string filename, FileMode filemode)", tabCount, ref code);
            //Add("{", tabCount, ref code);
            //Add("try", tabCount + 1, ref code);
            //Add("{", tabCount + 1, ref code);

            //if (constants.ContainsKey("ENCODING"))
            //    Add("using (BinaryWriter writer = new BinaryWriter(File.Open(filename, filemode), ENCODING))", tabCount + 2, ref code);
            //else
            //    Add("using (BinaryWriter writer = new BinaryWriter(File.Open(filename, filemode)))", tabCount + 2, ref code);

            //Add("{", tabCount + 2, ref code);

            //WriteVariables(ref code, tabCount + 3);

            //code.Add("");
            //Add("writer.Close();", tabCount + 3, ref code);

            //Add("}", tabCount + 2, ref code);
            //Add("}", tabCount + 1, ref code);
            //Add("catch", tabCount + 1, ref code);
            //Add("{", tabCount + 1, ref code);
            //Add("throw;", tabCount + 2, ref code);
            //Add("}", tabCount + 1, ref code);
            //Add("}", tabCount, ref code);
        }

        private void SaveCode(int tabCount, ref List<string> code)
        {
            Add("public void Save(BinaryWriter writer)", tabCount, ref code);
            Add("{", tabCount, ref code);
            Add("try", tabCount + 1, ref code);
            Add("{", tabCount + 1, ref code);

            WriteVariables(ref code, tabCount + 2);
            Add("}", tabCount + 1, ref code);
            Add("catch", tabCount + 1, ref code);
            Add("{", tabCount + 1, ref code);
            Add("throw;", tabCount + 2, ref code);
            Add("}", tabCount + 1, ref code);
            Add("}", tabCount, ref code);
        }

        public bool ContainsName(string name)
        {
            return ((Name == name) | (structs.ContainsName(name)) | (variables.ContainsName(name)));
        }

        public CompoundType<T> GetHeadNode()
        {
            CompoundType n = this;
            while (n.Parent != null) n = n.Parent;
            return n;
        }

        private void SaveTree(ref List<string> code, int tabCount)
        {
            Add("private void SaveTreeNode(TreeNode<" + StructName + "> _" + StructName.ToLower() + ", BinaryWriter writer)", tabCount, ref code);
            Add("{", tabCount, ref code);

            Add("writer.Write(_" + StructName.ToLower() + ".Count);", tabCount + 1, ref code);
            Add("_" + StructName.ToLower() + ".Value.Save(writer);", tabCount + 1, ref code);
            
            code.Add("");

            Add("TreeNode<" + StructName + ">[] children = _" + StructName.ToLower() + ".ToArray();", tabCount + 1, ref code);
            code.Add("");

            Add("foreach (TreeNode<" + StructName + "> child in children)", tabCount + 1, ref code);
            Add("{", tabCount + 1, ref code);
            Add("SaveTreeNode(child, writer);", tabCount + 2, ref code);
            Add("}", tabCount + 1, ref code);

            Add("}", tabCount, ref code);
        }

        private void ReadTree(ref List<string> code, int tabCount)
        {
            Add("private void ReadTreeNode(ref TreeNode<" + StructName + "> _" + StructName.ToLower() + ", BinaryReader reader)", tabCount, ref code);
            Add("{", tabCount, ref code);

            Add("int count = reader.ReadInt32();", tabCount + 1, ref code);
            Add(StructName + "[] " + LowercaseName + " = new " + StructName + "[count];", tabCount + 1, ref code);
            Add("_" + StructName.ToLower() + " = new TreeNode<" + StructName + ">(new " + StructName + "(reader));", tabCount + 1, ref code);
            
            code.Add("");

            Add("for (int i = 0; i < count; i++)", tabCount + 1, ref code);
            Add("{", tabCount + 1, ref code);
            Add("TreeNode<" + StructName + "> " + LowercaseName[0] + " = new TreeNode<" + StructName + ">(" + LowercaseName + "[i]);", tabCount + 2, ref code);
            //Add("_" + StructName.ToLower() + ".Add(" + LowercaseName + "[i]);", tabCount + 2, ref code);
            Add("ReadTreeNode(ref _" + LowercaseName[0] +", reader);", tabCount + 2, ref code);
            Add("_" + StructName.ToLower() + ".Add(" + LowercaseName[0] + ");", tabCount + 2, ref code);

            Add("}", tabCount + 1, ref code);

            Add("}", tabCount, ref code);
        }
    }
}
