using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileFormat.Languages
{
    enum AccessModifiers
    {
        Public,
        Private,
        Protected,
        Internal,
        None
    }

    enum Modifiers
    {
        Abstract,
        Async,
        Const,
        Event,
        Extern,
        New,
        Override,
        Partial,
        ReadOnly,
        Sealed,
        Static,
        Unsafe,
        Virtual,
        Volatile
    }

    enum CompareOperation
    {
        EqualTo,
        GreaterThan,
        LessThan,
        NotEqualTo,
        GreaterThanOrEqualTo,
        LessThanOrEqualTo
    }

    enum BinaryOperation
    {
        Addition,
        Subtraction,
        Multiplication,
        Division,
        Modulus
    }

    enum LogicOperation
    {
        And,
        Or,
        Not,
        Xor,
        AndAlso,
        OrAlso
    }

    enum StructureType
    {
        Class,
        Structure,
        AutoProperty,
        Property,
        GetAccessor,
        SetAccessor,
        Variable,
        CatchStatement,
        ElseStatement,
        Enumeration,
        FinallyStatement,
        IfStatement,
        Interface,
        Method,
        SwitchStatement,
        TryStatement,
        UsingStatement,
        Namespace,
        File,
        Constructor
    }
}
