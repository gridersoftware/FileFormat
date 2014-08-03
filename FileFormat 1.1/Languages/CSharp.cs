using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileFormat.Languages
{
    class CSharp : Language 
    {
        List<CSharp> structures;
        List<string> statements;
        List<CSharp> parameters;
        bool isConstant;
        string defaultValue;

        public override bool SupportsAutoProperties
        {
            get
            {
                return true;
            }
        }

        public override bool SupportsPrivateAccessors
        {
            get
            {
                return true;
            }
        }

        public override bool CaseInsensitiveLanguage
        {
            get
            {
                return false;
            }
        }

        public int Count { get; set; }

        public CSharp Parent
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            set;
        }

        public bool HasDefaultValue
        {
            get;
            set;
        }

        public string DefaultValue
        {
            get
            {
                if (Type == StructureType.Variable) return defaultValue;
                else return "";
            }
            set
            {
                if (Type == StructureType.Variable)
                {
                    if (value == null) throw new ArgumentNullException();
                    defaultValue = value;
                }
                else
                {
                    defaultValue = "";
                }
            }
        }

        public string ReturnType
        {
            get;
            set;
        }

        public string Inherits
        {
            get;
            set;
        }

        public StructureType Type
        {
            get;
            set;
        }

        public AccessModifiers AccessModifier
        {
            get;
            set;
        }

        public Modifiers[] OtherModifiers
        {
            get;
            set;
        }

        public AccessModifiers SecondaryAccessModifier
        {
            get;
            set;
        }

        public string ThisObject
        {
            get
            {
                return "this";
            }
        }

        public string BaseObject
        {
            get
            {
                return "base";
            }
        }

        public string NewKeyword
        {
            get
            {
                return "new";
            }
        }

        public void AddParameter(CSharp parameter)
        {
            if (parameter.Type != StructureType.Variable) throw new ArgumentException("parameter.Type must be StructureType.Variable.");
            if (parameter.AccessModifier != AccessModifiers.None) throw new ArgumentException("parameter.AccessModifier must be Modifers.None.");
            if (parameter.ReturnType == "") throw new ArgumentException("parameter.ReturnType cannot be an empty string.");

            parameters.Add(parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        /// <remarks>If an argument has an empty Name property, the DefaultValue property is used as the argument value.
        /// Otherwise, the argument Name is used. When using a string or char literal, please ensure that quotes are present
        /// inside the DefaultValue string.</remarks>
        public string GetNewInstanceOfObject(CSharp type, CSharp[] arguments)
        {
            if (type == null) throw new ArgumentNullException();
            if ((type.Type != StructureType.Structure) & (type.Type != StructureType.Class)) throw new ArgumentOutOfRangeException();
            if (arguments == null) throw new ArgumentNullException();
            if (type.Name == "") throw new ArgumentException();

            return GetNewObjectInstance(type.Name, arguments);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        /// <remarks>If an argument has an empty Name property, the DefaultValue property is used as the argument value.
        /// Otherwise, the argument Name is used. When using a string or char literal, please ensure that quotes are present
        /// inside the DefaultValue string.</remarks>
        public string GetNewObjectInstance(string type, CSharp[] arguments)
        {
            if (type == null) throw new ArgumentNullException();
            if (type == "") throw new ArgumentException();
            if (arguments == null) throw new ArgumentNullException();

            return string.Concat(NewKeyword, ' ', GetMethodCall(type, arguments));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        /// <remarks>If an argument has an empty Name property, the DefaultValue property is used as the argument value.
        /// Otherwise, the argument Name is used. When using a string or char literal, please ensure that quotes are present
        /// inside the DefaultValue string.</remarks>
        public string GetMethodCall(CSharp method, CSharp[] arguments)
        {
            if (method == null) throw new ArgumentNullException();
            if (arguments == null) throw new ArgumentNullException();
            if (method.Name == "") throw new ArgumentException();
            if (method.Type != StructureType.Method) throw new ArgumentOutOfRangeException();

            return GetMethodCall(method.Name, arguments);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        /// <remarks>If an argument has an empty Name property, the DefaultValue property is used as the argument value.
        /// Otherwise, the argument Name is used. When using a string or char literal, please ensure that quotes are present
        /// inside the DefaultValue string.</remarks>
        public string GetMethodCall(string method, CSharp[] arguments)
        {
            if (method == null) throw new ArgumentNullException();
            if (arguments == null) throw new ArgumentNullException();

            StringBuilder s = new StringBuilder();

            s.Append(method);
            s.Append('(');

            for (int i = 0; i < arguments.Length; i++)
            {
                if (arguments[i] == null) throw new ArgumentNullException();

                if (arguments[i].Name != "") s.Append(arguments[i].Name);
                else s.Append(arguments[i].DefaultValue);

                if (i < arguments.Length - 1) s.Append(", ");
            }

            s.Append(')');

            return s.ToString();
        }

        public void AddMember(CSharp structure)
        {
            structure.Parent = this;
            structures.Add(structure);
        }

        public void AddMember(string statement)
        {
            statements.Add(statement);
        }

        public static CSharp GetNewMember(StructureType type, string name, AccessModifiers accessModifier, Modifiers[] modifiers, string returnType)
        {
            return new CSharp(type, name, accessModifier, modifiers, returnType);
        }

        public CSharp(StructureType type, string name, AccessModifiers accessModifier, Modifiers[] modifiers, string returnType)
        {
            structures = new List<CSharp>();
            statements = new List<string>();
            parameters = new List<CSharp>();

            Type = type;
            Name = name;
            AccessModifier = accessModifier;
            OtherModifiers = modifiers;
            ReturnType = returnType;
            Count = 1;
        }

        public static CSharp MakeVariable(string name, string type, AccessModifiers modifier, bool constant = false)
        {
            Func<Modifiers[]> getMod = delegate
            { 
                if (constant) return new Modifiers[] { Modifiers.Const }; 
                else return new Modifiers[0];
            };

            CSharp var = new CSharp(StructureType.Variable, name, modifier, getMod(), type);

            return var;
        }

        public static CSharp MakeVariable(string name, string type, AccessModifiers modifier, Modifiers[] modifiers)
        {
            CSharp var = new CSharp(StructureType.Variable, name, modifier, modifiers, type);

            return var;
        }

        public static CSharp MakeFileStructure()
        {
            CSharp var = new CSharp(StructureType.File, "", AccessModifiers.Private, new Modifiers[0], "");
            return var;
        }

        public string[] Build()
        {
            List<string> output = new List<string>();

            output.Add(GetHeader());
            if ((Type != StructureType.AutoProperty) && (Type != StructureType.File))
            {
                output.Add("{");
            }

            output.AddRange(statements);
            foreach (CSharp s in structures)
            {
                output.AddRange(s.Build());
            }

            if ((Type != StructureType.AutoProperty) && (Type != StructureType.File))
            {
                output.Add("}");
            }

            return output.ToArray();
        }

        private string GetHeader()
        {
            StringBuilder header = new StringBuilder();

            switch (Type)
            {
                case StructureType.Class:
                    header.Append(GetAccessModifier());
                    header.Append(" class ");
                    header.Append(Name);
                    return header.ToString();

                case StructureType.Structure:
                    header.Append(GetAccessModifier());
                    header.Append(" struct ");
                    header.Append(Name);
                    return header.ToString();

                case StructureType.AutoProperty:
                case StructureType.Property:
                case StructureType.Variable:
                    header.Append(GetAccessModifier());
                    if (OtherModifiers.Contains(Modifiers.Const)) header.Append(" const");
                    header.Append(" ");
                    header.Append(ReturnType);
                    if (Count != 1) header.Append("[]");
                    header.Append(" ");
                    header.Append(Name);
                    if ((Type == StructureType.Variable | Count != 1) && (HasDefaultValue)) header.Append(" = " + defaultValue);
                    if (Type == StructureType.AutoProperty) 
                    {
                        header.Append("{ get; ");
                        if (SecondaryAccessModifier != AccessModifiers.None)
                        {
                            header.Append(GetSecondaryAccessModifier());
                            header.Append("");
                        }
                        header.Append("set; }");
                    }
                    return header.ToString();

                case StructureType.CatchStatement:
                    return "catch (" + ReturnType + " " + Name + ")";

                case StructureType.ElseStatement:
                    return "else";

                case StructureType.Enumeration:
                    header.Append(GetAccessModifier());
                    header.Append(" enum ");
                    header.Append(Name);
                    return header.ToString();

                case StructureType.FinallyStatement:
                    return "finally";

                case StructureType.IfStatement:
                    return "if (" + ReturnType + ")";

                case StructureType.Interface:
                    header.Append(GetAccessModifier());
                    header.Append(" interface ");
                    header.Append(Name);
                    return header.ToString();

                case StructureType.Method:
                    header.Append(GetAccessModifier());
                    header.Append(ReturnType);
                    header.Append(Name);
                    header.Append("(");
                    for (int i = 0; i < parameters.Count; i++)
                    {
                        CSharp p = parameters[i];
                        header.Append(p.GetHeader());
                        if (i < parameters.Count - 1) header.Append(", ");
                    }
                    header.Append(")");
                    return header.ToString();

                case StructureType.SwitchStatement:
                    header.Append("switch (");
                    header.Append(Name);
                    header.Append(")");
                    return header.ToString();

                case StructureType.TryStatement:
                    return "try";

                case StructureType.UsingStatement:
                    return "using (" + ReturnType + ")";

                case StructureType.Namespace:
                    return "namespace " + Name;

                default:
                    return "";
            }
        }

        private string _GetAccessModifier(AccessModifiers modifier)
        {
            switch (modifier)
            {
                case AccessModifiers.Private:
                    return "private";

                case AccessModifiers.Protected:
                    return "protected";

                case AccessModifiers.Public:
                    return "public";

                case AccessModifiers.Internal:
                    return "internal";

                default:
                    return "";
            }
        }

        private string GetAccessModifier()
        {
            return _GetAccessModifier(AccessModifier);
        }

        private string GetSecondaryAccessModifier()
        {
            return _GetAccessModifier(SecondaryAccessModifier);
        }

        private string GetModifierList()
        {
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < OtherModifiers.Length; i++)
            {
                s.Append(GetModifier(OtherModifiers[i]));
                if (i + 1 < OtherModifiers.Length)
                    s.Append(" ");
            }

            return s.ToString();
        }

        private string GetModifier(Modifiers modifier)
        {
            switch (modifier)
            {
                case Languages.Modifiers.Abstract:
                    return "abstract";

                case Languages.Modifiers.Async:
                    return "async";

                case Languages.Modifiers.Const:
                    return "const";

                case Languages.Modifiers.Event:
                    return "event";

                case Languages.Modifiers.Extern:
                    return "extern";

                case Languages.Modifiers.New:
                    return "new";

                case Languages.Modifiers.Override:
                    return "override";

                case Languages.Modifiers.Partial:
                    return "partial";

                case Languages.Modifiers.ReadOnly:
                    return "readonly";

                case Languages.Modifiers.Sealed:
                    return "sealed";

                case Languages.Modifiers.Static:
                    return "static";

                case Languages.Modifiers.Unsafe:
                    return "unsafe";

                case Languages.Modifiers.Virtual:
                    return "virtual";

                case Languages.Modifiers.Volatile:
                    return "volatile";

                default: return "";
            }
        }

        public void AddBlankLine()
        {
            AddMember("");
        }

        public void AddComment(string comment)
        {
            AddMember("// " + comment);
        }

        public void AddInclude(string @namespace)
        {
            AddMember("using " + @namespace);
        }

        private void AddLine(string line)
        {
            AddMember(line + ";");
        }

        public void AddAssignment(string left, string right)
        {
            AddLine(left + " = " + right);
        }

        public string GetCompare(string left, string right, CompareOperation operation)
        {
            switch (operation)
            {
                case CompareOperation.EqualTo:
                    return (left + " == " + right);

                case CompareOperation.GreaterThan:
                    return (left + " > " + right);

                case CompareOperation.GreaterThanOrEqualTo:
                    return (left + " >= " + right);

                case CompareOperation.LessThan:
                    return (left + " < " + right);

                case CompareOperation.LessThanOrEqualTo:
                    return (left + " <= " + right);

                case CompareOperation.NotEqualTo:
                    return (left + " != " + right);

                default:
                    return "";
            }
        }

        public string GetArithmetic(string left, string right, BinaryOperation operation)
        {
            switch (operation)
            {
                case BinaryOperation.Addition:
                    return (left + " + " + right);

                case BinaryOperation.Division:
                    return (left + " / " + right);

                case BinaryOperation.Modulus:
                    return (left + " % " + right);

                case BinaryOperation.Multiplication:
                    return (left + " * " + right);

                case BinaryOperation.Subtraction:
                    return (left + " - " + right);

                default:
                    return "";
            }
        }

        public string GetLogic(string left, string right, LogicOperation operation)
        {
            switch (operation)
            {
                case LogicOperation.And:
                    return (left + " & " + right);

                case LogicOperation.AndAlso:
                    return (left + " && " + right);

                case LogicOperation.Or:
                    return (left + " | " + right);

                case LogicOperation.OrAlso:
                    return (left + " || " + right);

                default:
                    return "";
            }
        }

        public string GetLogicalNot(string operand)
        {
            return "!" + operand;
        }

        public string WrapParenthesis(string value)
        {
            return "(" + value + ")";
        }

        public new void AddMember(CSharp structure)
        {
            structures.Add(structure);
        }

        public new void AddParameter(CSharp parameter)
        {
            parameters.Add(parameter);
        }

        
    }
}
