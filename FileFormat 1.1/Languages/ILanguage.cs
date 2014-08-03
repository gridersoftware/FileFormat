using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileFormat.Languages
{
    interface ILanguage
    {
        string Name { get; set; }
        ILanguage Parent { get; set; }
        string DefaultValue { get; set; }
        bool HasDefaultValue { get; set; }
        bool Constant { get; set; }
        string ReturnType { get; set; }
        Modifiers Modifier { get; set; }
        int Count { get; set; }
        /// <summary>
        /// Gets or sets a value that determines if an AutoProperty has a private set.
        /// </summary>
        bool PrivateSet { get; set; }

        /// <summary>
        /// Gets a string representing the "this" object.
        /// </summary>
        string ThisObject { get; }

        /// <summary>
        /// Gets a string representing the "base" object.
        /// </summary>
        string BaseObject { get; }

        string NewKeyword { get; }

        string GetNewObjectInstance(string type, ILanguage[] arguments);
        string GetNewObjectInstance(ILanguage type, ILanguage[] arguments);
        string GetMethodCall(string type, ILanguage[] arguments);
        string GetMethodCall(ILanguage method, ILanguage[] arguments);

        void AddMember(ILanguage structure);
        void AddMember(string statement);
        void AddParameter(ILanguage parameter);
        void AddInclude(string @namespace);
        string GetHeader();
        string GetModifier();
        string[] Build();
        void AddComment(string comment);
        void AddXmlComment(string tag, string comment);
        void AddAssignment(string left, string right);
        void AddLine(string line);
        string GetCompare(string left, string right, CompareOperation operation);
        string GetArithmetic(string left, string right, BinaryOperation operation);
        string GetLogic(string left, string right, LogicOperation operation);
        string GetLogicalNot(string operand);
        string WrapParenthesis(string value);
        void AddBlankLine();
        ILanguage MakeFileStructure();
        ILanguage MakeVariable(string name, string type, Modifiers modifier, bool constant = false);
        ILanguage GetNewMember(StructureType type, string name, Modifiers modifier, string returnType);
    }

    
}
