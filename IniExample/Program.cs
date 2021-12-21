using System;
using EasySharpIni;
using EasySharpIni.Converters;
using EasySharpIni.Models;

namespace IniTester
{
    internal class Program
    {
        // This path doesn't exist
        static readonly IniFile iniFile = new IniFile("example.ini");

        public static IniField GlobalField = iniFile.GetField("global", "global field");
        public static IniField Version = iniFile.GetSection("info").GetField("version", "1.0.0");

        public static IniField SqlHost = iniFile.GetSection("sql").GetField("host", "127.0.0.1");
        public static IniField SqlUsername = iniFile.GetSection("sql").GetField("username", "admin");
        public static IniField SqlPassword = iniFile.GetSection("sql").GetField("password", "admin123");
        public static IniField SqlDatabase = iniFile.GetSection("sql").GetField("database", "example_database");

        public static IniField PrimitiveInt = iniFile.GetSection("primitive").GetField("int", "5");
        public static IniField PrimitiveDouble = iniFile.GetSection("primitive").GetField("double", "7.324");

        static void Main(string[] args)
        {
            // Iterate through global fields
            foreach (IniField field in iniFile.Fields)
            {
                // Convert IniField implicitly to a string
                Console.WriteLine($"{field.Key}: {field}");
            }
            // Iterate through sections
            foreach (IniSection iniSection in iniFile.Sections)
            {
                // Iterate through section fields
                foreach (IniField field in iniSection.Fields)
                {
                    Console.WriteLine($"[{iniSection.Name}] {field.Key}: {field.Get()}");
                }
            }

            // Get these fields as specific types
            int primInt = PrimitiveInt.Get(new IntConverter());
            double primDouble = PrimitiveDouble.Get(new DoubleConverter());

            // Write these fields using type converters, converts the type to a string
            PrimitiveInt.Set(12, new IntConverter());
            PrimitiveDouble.Set(54.3, new DoubleConverter());

            Console.WriteLine(PrimitiveInt.Get(new IntConverter()));
            Console.WriteLine(PrimitiveDouble.Get(new DoubleConverter()));

            // Write ini file to new location
            iniFile.Write(pathOverride: "newexample.ini");

            // Write ini file with some export options
            iniFile.Write(IniExportOptions.AlphabeticalFields | IniExportOptions.AlphabeticalSections, pathOverride: "newexample options.ini");
        }
    }
}
