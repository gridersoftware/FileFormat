# FileFormat

## Syntax and Setup

```
FileFormat.exe source destination [options]
```

The FileFormat utility runs in the Windows Command Prompt or the Linux terminal (from here on, simply called, "the console").

If running the FileFormat utility in Microsoft Windows, you can run the program from the console without changing anything. However, if you are running FileFormat in Linux, you will need to change the file permissions to allow the executable to run.

## Making the FileFormat utility executable in Linux

Before you can use the FileFormat utility in Linux, you must have the [Mono Framework](www.mono-project.com/Main_Page). You must also change the file permissions to make FileFormat.exe an executable.

```
# Changing file permissions for all users
david@david-VirtualBox ~/FileFormat $ ls -l
total 44
-rw--r--r-- 1 david david 39936 Feb 6 16:41 FileFormat.exe
-rw--r--r-- 1 david david 285   Dec 4 23:13 sample.ff
david@david-VirtualBox ~/FileFormat $ chmod a+x FileFormat.exe
david@david-VirtualBox ~/FileFormat $ ls -l
total 44
-rw-xr-xr-x 1 david david 39936 Feb 6 16:41 FileFormat.exe
-rw--r--r-- 1 david david 285   Dec 4 23:13 sample.ff
```

## Running FileFormat

FileFormat has two required arguments: the input file name, and the output file name. The input file must be written as a text file (in the FileFormat language), and should, but does not have to, have a *.ff suffix.

There are some optional arguments:

|Argument|Description|
|--------|-----------|
|/p|Optionally prints the file output to the console along with the standard debugging information.|
|/lang:|This option determines the language to output. By default, FileFormat outputs C# code, so this is optional. As of version 1.0, FileFormat **only** outputs to C#, but more languages are planned for the future. C# language option example: `/lang:cs`|

Language Options:

|Option|Language|Version|
|------|--------|-------|
|cs|C# 3.0|1.0+|
