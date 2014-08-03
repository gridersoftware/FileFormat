# FileFormat Language

The FileFormat Language is a language for the FileFormat Utility. It is used to describe file formats using a C-like syntax. 
When run through the FileFormat utility, a class file is generated in the target language that can create, read, and write 
files in the format specified by the FileFormat file used.

## Syntax

```
#NAMESPACE GScript
#FORMATNAME CompiledScriptFile
#MAGICNUMBER string "GSC"
#ENCODING utf8

string $magic :: MAGICNUMBER
string $name

int32 $varcount 
struct[$varcount] Variable $variables
{
	byte $type
	int32 $len
	byte[$len] $value
}
```

This will yield

```
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace GScript
{
	public class CompiledScriptFile
	{
		private const Encoding ENCODING = Encoding.UTF8;
		public const String MAGICNUMBER = "GSC";

		public String Magic { get; private set; }
		public String Name { get; private set; }
		public Int32 Varcount { get; private set; }
		public Variable[] Variables { get; private set; }

		public struct Variable
		{
			public Byte Type { get; private set; }
			public Int32 Len { get; private set; }
			public Byte[] Value { get; private set; }

			public Variable(Byte type, Int32 len, Byte[] value)
			{
				Type = type;
				Len = len;
				if (value.Length != len) throw new ArgumentOutOfRangeException();
				Value = value;
			} 

			public Variable(BinaryReader reader)
			{
				try
				{
					Type = reader.ReadByte();
					Len = reader.ReadInt32();
					Value = reader.ReadBytes(Len);
				}	
				catch
				{
					throw;
				}
			} 

			public void Save(BinaryWriter writer)
			{
				try
				{
					writer.Write(Type);
					writer.Write(Len);
					for (int i = 0; i < Len; i++)
					{
						writer.Write(Value[i]);
					}
				}
				catch
				{
					throw;
				}
			}
		} 

		public CompiledScriptFile(String magic, String name, Int32 varcount, Variable[] variables)
		{
			if (magic != MAGICNUMBER) throw new ArgumentException();
			Magic = magic;
			Name = name;
			Varcount = varcount;
			if (variables.Length != Varcount) throw new ArgumentOutOfRangeException();
			Variables = variables;
		} 

		public CompiledScriptFile(string filename)
		{
			try
			{
				if (File.Exists(filename))
				{
					using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open), ENCODING))
					{
						Magic = reader.ReadString();
						Name = reader.ReadString();
						Varcount = reader.ReadInt32();
						Variables = new Variable[Varcount];

						for (int i = 0; i < Varcount; i++)
						{
							Variables[i] = new Variable(reader);
						} 

						reader.Close();
					}
				}
				else
				{
					throw new FileNotFoundException();
				}
			}
			catch
			{
				throw;
			}
		}

		public void SaveFile(string filename, FileMode filemode)
		{
			try
			{
				using (BinaryWriter writer = new BinaryWriter(File.Open(filename, filemode), ENCODING))
				{
					writer.Write(MAGICNUMBER);
					writer.Write(Name);
					writer.Write(Varcount);
					for (int i = 0; i < Varcount; i++)
					{
						Variables[i].Save(writer);
					} 

					writer.Close();
				}
			}
			catch
			{
				throw;
			}
		}
	}
}
```

## Language Features

FileFormat has a C-style syntax, with some minor differences. For example, file element names must be declared using a
dollar sign ($), as is done with variables in Perl or PHP. Semicolons are not used in FileFormat.

### File Elements

A FileFormat file consists primarily of a list of a file elements, one per line. These are given in the order in which 
they appear in the file. File elements can be thought of as variables, and when translated into the target language, 
are treated as properties in the format class. The names of file elements must follow the following rules:

* Element names must begin with a dollar sign ($). `$name` is valid, but `name` is not.
* The first character after the dollar sign must be a letter. `$x1` is valid but `$1x` is not.
* Element names must be entirely lowercase. Uppercase letters are not allowed. `$name` is valid, but `$Name` is not.
* With the exception of the starting dollar sign, element names can contain only lowercase letters and numbers. Special characters, including underscore, are not allowed. `$varcount` is valid, but `$var_count` is not.

All elements must be given a type, which can either be a basic data type, an array, or a compound type.

#### Basic Data Types

Every element of the file must be declared by data type.

|Keyword|.Net System Type|Description|Version|
|-------|----------------|-----------|-------|
|bool|Boolean|Represents a true/false value|1.0|
|byte|Byte|Represents an unsigned byte|1.0|
|sbyte|SByte|Represents a signed byte|1.0|
|uint16|UInt16|Represents a 16-bit unsigned integer|1.0|
|int16|Int16|Represents a 16-bit signed integer|1.0|
|uint32|UInt32|Represents a 32-bit unsigned integer|1.0|
|int32|Int32|Represents a 32-bit signed integer|1.0|
|uint64|UInt64|Represents a 64-bit unsigned integer|1.0|
|int64|Int64|Represents a 64-bit signed integer|1.0|
|float|Float|Represents a 32-bit floating-point number|1.0|
|double|Double|Represents a 64-bit floating-point number|1.0|
|decimal|Decimal|Represents a high-precision decimal number|1.0|
|char|Char|Represents a single character|1.0|
|string|String|Represents a character string|1.0|
|uid|Guid|Represents a globally unique identifier|1.1|

### Arrays
Any basic type can be turned into an array. This is done by placing pair of square brackets after the type which contains 
the number of elements. This number can either be a literal unsigned integer, or an element name, where the element is an 
int32. If an element is used, it must be declared before the array.

```
bool[5] $bits

int32 $strcount
string[$strcount] $strings
```

### Compound Types (Structs)
Compound types, declared by the keyword	`struct`, are similar to a structure in that it is a single element that contains 
multiple elements. Compound types must be declared as arrays, as there is no need for a struct if you're only going to 
have one. Compound types are useful for creating repeating structures, like database records.

The syntax for the compound type is as follows:
```
struct[(literal|$elementName)] TypeName $elementname
```

The parts of the declaration are:
* struct - Declares a compound type
* [(literal)|$elementname] - Follows the same rules for arrays. Use a literal unsigned integer, or the element name of an integral type.
* TypeName - Compound types must be given a name. They are translated into structures in the target language, and must be named. The name follows these rules:
  * Type names must contain only uppercase and lowercase letters and numbers.
  * Type names must start with a letter.
  * Type names cannot contain any whitespace or special characters, including underscore.
* $elementname - The name of the element that will contain the structure array. This name follows the same element rules as always.

The elements of a compound type must be defined between a pair of curly brackets ({ and }).

For example:
```
int32 $recordcount

struct[$recordcount] Record $records
{
	int32 $id
	string $name
	string $phonenumber
	string $emailaddress
	byte $age
}
```

The curley brackets **MUST** be on their own lines. For example:
```
/* This is correct */
struct[$recordcount] Record $records
{
	/* your code here */
}

/* This is incorrect */
struct[$recordcount] Record $records {
/* your code here */
}
```

### Trees
Tree structures can be created very easily using the tree keyword. Trees work the same way as compound types, except they 
utilize the TreeNode<> class in GSLib. As a result, you should add GSLib to your program references, and in your FileFormat 
file, `#INCLUDE GSLib.Collections.Trees`. If you fail to do the include, FileFormat will warn you that you are missing an 
include for that namespace.

Trees also act like arrays and structures in that a tree can have one or more root objects.
```
// Produces a tree with a single root
tree StructName $variablename
{
}

// Produces a tree with a single root. The "[1]" is not required.
tree[1] StructName $variablename
{
}

// Produces a tree with a fixed number of roots.
tree[5] StructName $variablename
{
}

// Produces a tree with a variable number of roots.
int32 $rootcount
tree[$rootcount] StructName $variablename
{
}
```

As with arrays and structs, the count must be an integer literal or Int32 element.

#### Example
The following code creates a tree whose value is a struct called Node. Node only contains an integer.
```
#NAMESPACE Examples
#FORMATNAME ExampleTree
#INCLUDE GSLib.Collections.Trees

int32 $rootcount

tree[$rootcount] Node $nodes
{
	int32 $value	
}
```

When run through FileFormat, the resulting code is:
```
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using GSLib.Collections.Trees;

namespace Examples
{
	public class ExampleTree
	{
		public Int32 Rootcount { get; private set; }
		public TreeNode<Node>[] Nodes { get; private set; }

		public struct Node
		{
			public Int32 Value { get; private set; }

			public Node(Int32 value)
			{
				this = new Node();
				Value = value;
			}

			public Node(BinaryReader reader)
			{
				this = new Node(); 
				try 
				{
					Value = reader.ReadInt32();
				}
				catch
				{
					throw;
				}
			}

			public void Save(BinaryWriter writer)
			{
				try
				{
					writer.Write(Value);
				}
				catch
				{
					throw;
				}
			}
		}

		public ExampleTree(Int32 rootcount, TreeNode<Node>[] nodes)
		{
			Rootcount = rootcount;
			if (nodes.Length != Rootcount) throw new ArgumentOutOfRangeException();
			Nodes = nodes;
		}

		public ExampleTree(string filename)
		{
			try
			{
				if (File.Exists(filename))
				{
					using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
					{
						Rootcount = reader.ReadInt32();
						Nodes = new TreeNode<Node>[Rootcount];

						for (int i = 0; i < Rootcount; i++)
						{
							ReadTreeNode(ref Nodes[i], reader);
						}

						reader.Close();
					}
				}
				else
				{
				throw new FileNotFoundException();
				}
			}
			catch
			{
			throw;
			}
		}

		public void SaveFile(string filename, FileMode filemode)
		{
			try
			{
				using (BinaryWriter writer = new BinaryWriter(File.Open(filename, filemode)))
				{
					writer.Write(Rootcount);
					for (int i = 0; i < Rootcount; i++)
					{
						SaveTreeNode(Nodes[i], writer);
					}

					writer.Close();
				}
			}
			catch
			{
				throw;
			}
		}

		private void SaveTreeNode(TreeNode<Node> _node, BinaryWriter writer)
		{
			writer.Write(_node.Count);
			_node.Value.Save(writer);

			TreeNode<Node>[] children = _node.ToArray();

			foreach (TreeNode<Node> child in children)
			{
				SaveTreeNode(child, writer);
			}
		}

		private void ReadTreeNode(ref TreeNode<Node> _node, BinaryReader reader)
		{
			int count = reader.ReadInt32();
			Node[] nodes = new Node[count];
			_node = new TreeNode<Node>(new Node(reader));

			for (int i = 0; i < count; i++)
			{
				TreeNode<Node> n = new TreeNode<Node>(nodes[i]);
				ReadTreeNode(ref _n, reader);
				_node.Add(n);
			}
		}
	}
}
```

## Directives

Directives start with a pound or hash (#) and are all uppercase. These are directives that **MUST** be used in every FileFormat code: #FORMATNAME and #NAMESPACE

### #FORMATNAME
This is one of the required directives. It has one argument: the name of your format. This will also become the name of your
class file, and the name of the class itself.
```
#FORMATNAME MyFileFormat
```

### #NAMESPACE
This is the other required directive. It also has one argument: the namespace in which your class will exist. Note: This directive 
will be ignored in cases where the target language does not support or require namespaces.
```
#NAMESPACE MyProgram
```

### #ENCODING
This is required if you are using `char` or `string` types. This defines the encoding format for characters and strings.
There are five possible values.

* utf7
* utf8
* unicode
* utf32
* ascii

The value `unicode` is the 16-bit Unicode format. If you're unsure what to use, it is recommended that you use utf8.
```
#ENCODING utf8
```

### #MAGICNUMBER
This defines a constant value that is placed at a specific location in the file. When using the #MAGICNUMBER directive, you define
its type and its value.

At this time, the only types allowed are single basic data types. Compound types and arrays are not allowed. The syntax is as follows:
```
#MAGICNUMBER type value
```

To assign which element is a magic number, use the compare operator (::) followed by the constant keyword `MAGICNUMBER`. For example:
```
#MAGICNUMBER string magic

string $magic :: MAGICNUMBER
```

### #CONST
This allows you to define constants. This directive works similarly to #MAGICNUMBER except that constants have user-defined names.
```
#CONST type name value
```

### #INCLUDE
This allows you to include referenced namespaces.
```
#INCLUDE namespace
```
You may need to use this if you are using the Trees structure.

## Operators
There is only one operator: the compare operator.

### The compare operator, represented by two colons (::), allows a comparison or assignment between a file element and another value.
The other value can be a literal, the value of another file element, or the value of a #CONST directive.

When reading a file, the compare operator will force a comparison between the value of the element on the left side of the operator
and the value on the right. If the two values do not match, an exception will be thrown by the class.

When writing a file, the compare operator acts as an assign operator, and the value on the right will be assigned to the element on
the left.

The object on the left MUST be an element. This operator is used when declaring the element.

## Constant Keywords
Constant keywords are keywords that can have a predefined value, and can be used with a Compare operator. 
There is only one constant keyword available: `MAGICNUMBER`.

### MAGICNUMBER
The `MAGICNUMBER` constant becomes available when using the #MAGICNUMBER directive. `MAGICNUMBER` is the value defined by that constant.

## Comments
Comments work the same way in FileFormat as they do in C-style languages. Double forward slashes (//) create single-line comments,
and a comment that starts with /* and end with */ is a multi-line comment.

```
// This is a single line comment.

/* This is a
multi-line comment. */

/* This is a multi-line comment written on one line. */
```