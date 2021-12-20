# EasySharpIni
A library designed to easily read, parse and write .ini files written in C#. EasySharpIni supports .NET 5-6, .NET Standard 2.1, .NET Framework 4.6, 4.7.2 and 4.8. It is also compatible with Unity. You can download the latest release on [nuget](https://www.nuget.org/packages/EasySharpIni/).

# Overview
This will go over the basics of how to use EasySharpIni, such as:
- Reading and parsing an ini file.
- Using sections and fields (Getting and Setting values). 
- Writing an ini file.

This will also cover how to automatically convert field values to different data types using the `IConverter<T>` interface.
## Parsing
To parse an ini file, you must create an IniFile object, specify the file path, and call the parse method:

    var myFile = new IniFile("example.ini").Parse();
You can also parse the file asynchronously using `.ParseAsync()`
> NOTE: `.ParseAsync()` is only available on .NET Standard 2.1 or greater and .NET 5 or greater.

If the file or path do not exist, the object will still be created but will contain no data.

## Getting Sections and Fields
### Sections
After parsing to an `IniFile`, you can get a section by using the following:

    IniSection mySection = myFile.GetSection("mySectionName");
    
The `GetSection();` method will return the field with the matching name. If the section cannot be found, it is created. If it is found, the IniSection is returned.

### Fields
There are 2 types of fields in the `IniFile`, **Global** fields and **Local** fields. Local fields are contained within sections, and Global fields are contained within the file itself (they have no section/an empty section)

After parsing to an `IniFile`, you can get a global field by using the following:

    IniField myField = myFile.GetField("myGlobalKey", "default value");
    
The `GetField()` method will return the field with the matching key. If the field cannot be found, it is created and given the value of the `defaultValue` parameter (this parameter is optional, but defaults to an empty string). If it is found, the IniField is returned.

To get a field within a section (local field), call `GetField();`, but on the `IniSection` object instead. For example:

    IniField mySectionField = mySection.GetField("myLocalKey", "local default value");

## Retrieving Field values
Once you have an `IniField` object, you can parse the value of the field using:

    string myValue = myField.Get();

This will return the value of the field as a string, which is the raw value that was taken from the file.

However, EasySharpIni provides an easy way to get values as other types, using the `Converter<T>` interface. This library includes, by default, ways to convert all the various number types from strings in the `IniField`.
For example, to get the value of an `IniField` as an integer:

    int myConvertedValue = myField.Get(new IntConverter());
This will convert the string value to an integer using the `Converter<T>` implementation `IntConverter`. If the conversion fails by whatever means, the default value of the converter (in the case of an integer, 0) is returned instead.
You can use this to parse various different data types quickly and easily, and have the parsing be done outside of your code.

## Setting Field values
You can set the value of a field by using

    myField.Set("my new value");
Alternatively, you can once again use the `Converter<T>` interface to convert the data type back to a string:

    myField.Set(153.4f, new FloatConverter());

## Writing the IniFile to disk
After parsing the file, reading and setting values, you may want to save these changes back to the disk.
To do this, using your `IniFile` object, call:

    myFile.Write();
You can also write the file asynchronously using `WriteAsync()`
> NOTE: `.WriteAsync()` is only available on .NET Standard 2.1 or greater and .NET 5 or greater.

You can also pass export options to the `Write()` function. These are used to change the formatting of the exported file, such as alphabetical sorting, empty lines in between sections etc.

    myFile.Write(IniExportOptions.AlphabeticalSections | IniExportOptions.AlphabeticalFields);
This also works asynchronously.

You can also specify a `pathOverride` parameter when Writing. The `IniFile` object stores the path of the file it was read from in order to write back to it. However in case you do not want to overwrite the existing file, you can instead choose to write to a different file by overriding the path when writing.

## Creating your own Converter
You may have data types that you would like to quickly convert from a string to that type, without doing the conversion in the code you are accessing the field.

To start, create a class that inherits the abstract `Converter<T>`class. Instead of T, write the data type you are wanting to convert, for example `double`. By default, 3 methods will be overridden. These are `Parse()`, `GetDefaultName()` and `GetDefaultValue()`. A 4th method can also be overridden, `ToString()`, but is not necessary. This is required if your data type cannot be converted back to a string using the normal `ToString();` method, and requires manual serialisation.

`GetDefaultName()` - Here you specify the name of this converter, usually just the type it is converting e.g "Double". This can be used in logging purposes to identify what Converter encountered an error.
`GetDefaultValue()` - In case parsing is not successful, you should return this value instead. For number types this may be 0 or another number such as -1, for strings it could be an empty string.
`Parse(string arg, out T result)` - In this function you write your code to convert `arg`, which is the value of the field into type `T` as the result.

# Help
If you require help, you can create an issue on the GitHub repository, or message me on Discord `@Riley The Fox#3621` and please state your question in your first message (don't just message me saying "hi" first, I am unlikely to respond. Be upfront about your question)