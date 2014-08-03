using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileFormat.Languages;

namespace FileFormat
{
    class Variable
    {
        /// <summary>
        /// Gets the name of the variable.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the number of elements in the variable, if defined by a constant. If Count is greater than 1, the variable is an array. 
        /// If Count is less than 1, the number of elements is defined by the value of the variable set in CountRef.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets the variable that defines the number of elements in the variable. If CountRef is null, the number of elements is a constant and
        /// is defined in Count.
        /// </summary>
        public Variable CountRef { get; private set; }

        /// <summary>
        /// Gets the type of the variable.
        /// </summary>
        public TypeCode @TypeCode { get; private set; }

        /// <summary>
        /// Gets or sets the name of the variable or constant with which to compare to or assign from.
        /// </summary>
        public string CompareWith { get; set; }

        /// <summary>
        /// Creates a new Variable instance with a single element.
        /// </summary>
        /// <param name="name">Name of variable.</param>
        /// <param name="type">Type of variable.</param>
        public Variable(string name, TypeCode type)
        {
            if (name == null) throw new ArgumentNullException();
            if (name == "") throw new ArgumentException();

            Name = name;
            Count = 1;
            CountRef = null;
            @TypeCode = type;
        }

        /// <summary>
        /// Creates a new multi-element Variable instance with a constant size.
        /// </summary>
        /// <param name="name">Name of the variable.</param>
        /// <param name="count">Number of elements in the variable.</param>
        /// <param name="type">Type of the variable.</param>
        /// <exception cref="ArgumentNullException">Thrown if name is null.</exception>
        /// <exception cref="ArgumentException">Thrown if name is an empty string.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if count is less than 1.</exception>
        public Variable(string name, int count, TypeCode type)
        {
            if (name == null) throw new ArgumentNullException();
            if (name == "") throw new ArgumentException();
            if (count < 1) throw new ArgumentOutOfRangeException();

            Name = name;
            Count = count;
            CountRef = null;
            @TypeCode = type;
        }

        /// <summary>
        /// Creates a new multi-element Variable instance with a variable size.
        /// </summary>
        /// <param name="name">Name of the variable.</param>
        /// <param name="countref">Variable determining the number of elements in the variable.</param>
        /// <param name="type">Type of the variable.</param>
        /// <exception cref="ArgumentNullException">Thrown if name or countref is null.</exception>
        /// <exception cref="ArgumentException">Thrown if name is an empty string.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if countref's TypeCode is not TypeCode.Int32.</exception>
        public Variable(string name, Variable countref, TypeCode type)
        {
            if (name == null) throw new ArgumentNullException();
            if (name == "") throw new ArgumentException();
            if (countref == null) throw new ArgumentNullException();
            if (countref.TypeCode != System.TypeCode.Int32) throw new ArgumentOutOfRangeException();

            Name = name;
            Count = 0;
            CountRef = countref;
            @TypeCode = type;
        }

        /// <summary>
        /// Gets the System.TypeCode associated with the given type name.
        /// </summary>
        /// <param name="typecode">Name of a type.</param>
        /// <returns>Returns the TypeCode associated with the given type name. If an unknown type is given, TypeCode.Empty is returned.</returns>
        public static TypeCode GetTypeCode(string typecode)
        {
            switch (typecode)
            {
                case "bool": return TypeCode.Boolean;
                case "sbyte": return TypeCode.SByte;
                case "byte": return TypeCode.Byte;
                case "uint16": return TypeCode.UInt16;
                case "uint32": return TypeCode.UInt32;
                case "uint64": return TypeCode.UInt64;
                case "int16": return TypeCode.Int16;
                case "int32": return TypeCode.Int32;
                case "int64": return TypeCode.Int64;
                case "float": return TypeCode.Single;
                case "double": return TypeCode.Double;
                case "decimal": return TypeCode.Decimal;
                case "string": return TypeCode.String;
                case "char": return TypeCode.Char;
                default: return TypeCode.Empty;
            }
        }

        public static string GetReadType(TypeCode t)
        {
            return "reader.Read" + t.ToString() + "();";
        }

        /// <summary>
        /// Gets the C# code that represents the Variable as an auto-initialized property.
        /// </summary>
        /// <returns>Returns a string containing C# code for the property.</returns>
        public virtual T GetPropertyCode<T>(T current)
            where T : Language
        {
            if (current.SupportsAutoProperties & current.SupportsPrivateAccessors)
            {
                T v = (T)current.GetNewMember(StructureType.AutoProperty, UppercaseName, AccessModifiers.Public, TypeCode.ToString());
            }
            else
            {
                T backing = (T)current.GetNewMember(StructureType.Variable, '_' + LowercaseName 
            }
            T v = (T)current.GetNewMember(StructureType.AutoProperty, UppercaseName, AccessModifiers.Public, @TypeCode.ToString());
            v.Count = Count;
            v.PrivateSet = true;
            current.AddMember(v);
            return v;
        }

        /// <summary>
        /// Gets the C# code fragment that represents the Variable as a function argument.
        /// </summary>
        /// <returns>Returns a string containing C# code for the function argument.</returns>
        public virtual string GetArgumentCode()
        {
            if (Count == 1)
                return @TypeCode.ToString() + " " + LowercaseName;
            else
                return @TypeCode.ToString() + "[] " + LowercaseName;
        }

        /// <summary>
        /// Gets the name of the variable without the dollar sign, with an uppercase name.
        /// </summary>
        public string UppercaseName
        {
            get
            {
                return GetPropertyName(Name);
            }
        }

        /// <summary>
        /// Gets the name of the variable without the dollar sign and all lowercase characters.
        /// </summary>
        public string LowercaseName
        {
            get
            {
                return Name.TrimStart('$');
            }
        }

        /// <summary>
        /// Returns the name 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetPropertyName(string name)
        {
            char[] n = name.TrimStart('$').ToCharArray();
            n[0] = char.ToUpper(n[0]);
            return new string(n);
        }
    }
}
