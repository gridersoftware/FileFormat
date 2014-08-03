using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileFormat.Languages
{
    abstract class Language
    {
        List<string> statements;

        public Language()
        {
            statements = new List<string>();
        }

        public string Name { get; set; }

        public Language Parent { get; set; }

        public string DefaultValue { get; set; }

        public bool HasDefaultValue
        {
            get
            {
                return DefaultValue == "";
            }
        }

        public string ReturnType { get; set; }

        public AccessModifiers AccessModifier { get; set; }

        public AccessModifiers SecondaryAccessModifier { get; set; }

        public Modifiers[] OtherModifiers { get; set; }

        public int Count { get; set; }

        /// <summary>
        /// Gets a value that determines if the language supports auto-implemented properties.
        /// </summary>
        public virtual bool SupportsAutoProperties { get; }

        /// <summary>
        /// Gets a value that determines if property accessors can be private.
        /// </summary>
        public virtual bool SupportsPrivateAccessors { get; }

        /// <summary>
        /// Gets a value that determines if the language is case-insensitive. When true, "VariableName" and "variablename" refer to the same variable.
        /// </summary>
        public virtual bool CaseInsensitiveLanguage { get; }

        /// <summary>
        /// Gets a keyword representing the current object. In C#, "this". In VB.Net, "Me".
        /// </summary>
        public virtual string ThisObject { get; }

        /// <summary>
        /// Gets a keyword representing the base object. In C#, "base". In VB.Net, "MyBase".
        /// </summary>
        public virtual string BaseObject { get; }

        public virtual string NewKeyword { get; }

        public virtual string GetNewObjectInstance(string type, Language[] arguments);

        public virtual string GetNewObjectInstance(Language type, Language[] arguments);

        public virtual string GetMethodCall(string method, Language[] arguments);

        public virtual string GetMethodCall(Language method, Language[] arguments);

        public virtual void AddMember(Language structure);

        public void AddMember(string statement)
        {
            statements.Add(statement);
        }

        public virtual void AddParameter(Language parameter);

        public virtual void AddInclude(string @namespace);

        public virtual string GetHeader();

        public virtual string GetModifierList();

        public virtual string GetModifier(Modifiers modifier);

        public virtual string[] Build();

        public virtual void AddComment(string comment);

        public virtual void AddXmlComment(string tag, string comment);

        public virtual void AddAssignment(string left, string right);

        public void AddLine(string line)
        {
            AddMember(line);
        }

        public virtual string GetCompare(string left, string right, CompareOperation operation);

        public virtual string GetArithmetic(string left, string right, BinaryOperation operation);

        public virtual string GetLogic(string left, string right, LogicOperation operation);

        public virtual string GetLogicalNot(string operand);

        public static string WrapParenthesis(string value)
        {
            return '(' + value + ')';
        }

        public void AddBlankLine()
        {
            AddLine("");
        }

        public virtual Language MakeFileStructure();

        public virtual Language MakeVariable(string name, string type, AccessModifiers modifier, bool constant = false);

        public virtual Language MakeVariable(string name, string type, AccessModifiers modifier, Modifiers[] modifierList);

        public virtual Language GetNewMember(StructureType type, string name, AccessModifiers modifier, string returnType);
    }
}
