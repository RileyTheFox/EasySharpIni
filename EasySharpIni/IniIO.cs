using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using EasySharpIni.Models;

namespace EasySharpIni
{
    public static class IniIO
    {

        internal static IniFile Parse(IniFile file)
        {
            if (!File.Exists(file.Path))
                return file;

            return ReadFile(file, File.ReadAllLines(file.Path));
        }

        internal static IniFile ParseSpan(IniFile file)
        {
            if (!File.Exists(file.Path))
                return file;

            return ReadFileSpan(file);
        }
        internal static void Write(IniFile file, IniExportOptions options)
        {
            File.WriteAllText(file.Path, ExportToText(file, options));
        }

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        internal static async Task<IniFile> ParseAsync(IniFile file)
        {
            if (!File.Exists(file.Path))
                return file;

            return ReadFile(file, await File.ReadAllLinesAsync(file.Path));
        }

        internal static async Task WriteAsync(IniFile file, IniExportOptions options)
        {
            await File.WriteAllTextAsync(file.Path, ExportToText(file, options));
        }
#endif

        public static IniFile ReadFileSpan(IniFile file)
        {
            try
            {
                ReadOnlySpan<char> text = File.ReadAllText(file.Path).AsSpan().Trim();

                IniSection? section = null;

                ReadOnlySpan<char> remaining = text;
                while (ReadLine(ref remaining, out ReadOnlySpan<char> line))
                {
                    if (line.StartsWith(";"))
                        continue;

                    // Section Parsing
                    if (line.StartsWith("[") && line.EndsWith("]"))
                    {
                        var sectionText = line.Slice(1, line.Length - 2);

                        section = file.CreateSection(sectionText.ToString());
                    }
                    else
                    {
                        int equalsIndex = line.IndexOf('=');
                        if (equalsIndex == -1)
                            continue;

                        var key = line.Slice(0, equalsIndex).Trim();
                        var value = line.Slice(equalsIndex + 1, line.Length - equalsIndex - 1).Trim();

                        if (section == null)
                        {
                            file.AddField(key.ToString(), value.ToString());
                        }
                        else section.AddField(key.ToString(), value.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }
            return file;
        }

        public static IniFile ReadFile(IniFile file, string[] lines)
        {
            IniSection? section = null;
            foreach (string line in lines)
            {
                var trimLine = line.Trim();
                // Comments are not parsed
                if (trimLine.StartsWith(";") || string.IsNullOrEmpty(trimLine))
                    continue;

                // Section Parsing
                if (trimLine.StartsWith("[") && trimLine.EndsWith("]"))
                {
                    string sectionText = trimLine.Substring(1, trimLine.Length - 2).Trim();

                    section = file.CreateSection(sectionText);
                }
                else
                {
                    int equalsIndex = trimLine.IndexOf('=');
                    if (equalsIndex == -1)
                        continue;

                    string key = trimLine.Substring(0, equalsIndex).Trim();
                    string value = trimLine.Substring(equalsIndex + 1, trimLine.Length - equalsIndex - 1).Trim();

                    if (section == null)
                    {
                        file.AddField(key, value);
                    }
                    else section.AddField(key, value);
                }
            }

            return file;
        }

        private static bool ReadLine(ref ReadOnlySpan<char> span, out ReadOnlySpan<char> line)
        {
            for (int i = 0; i < span.Length; i++)
            {
                if (span[i] == '\n')
                {
                    line = span.Slice(0, i);
                    span = span.Slice(i + 1, span.Length - i - 1);
                    return true;
                }
                if (span[i] == '\r' && span[i + 1] == '\n')
                {
                    line = span.Slice(0, i);
                    span = span.Slice(i + 2, span.Length - i - 2);
                    return true;
                }
            }
            line = span;
            return false;
        }

        /// <summary>
        /// Exports <paramref name="iniFile"/> to a <see cref="string"/> using the <see cref="IniExportOptions"/> <paramref name="options"/>. 
        /// </summary>
        /// <param name="iniFile">The <see cref="IniFile"/> to export.</param>
        /// <param name="options">The options to use</param>
        /// <returns><paramref name="iniFile"/> represented as a string.</returns>
        public static string ExportToText(IniFile iniFile, IniExportOptions options)
        {
            StringBuilder output = new StringBuilder();
            string keyValueSeparator = options.HasFlag(IniExportOptions.KeyValueWhitespace) ? " = " : "=";

            bool alphabeticalFields = options.HasFlag(IniExportOptions.AlphabeticalFields);
            bool alphabeticalSections = options.HasFlag(IniExportOptions.AlphabeticalSections);

            // Output fields that are not in a section.
            if (alphabeticalFields)
            {
                SortedSet<IniField> sorted = new(iniFile.Fields, new IniFieldKeyComparer());

                foreach (IniField field in sorted)
                {
                    output.AppendLine($"{field.Key}{keyValueSeparator}{field.Get()}");
                }
            }
            else
            {
                foreach (IniField field in iniFile.Fields)
                {
                    output.AppendLine($"{field.Key}{keyValueSeparator}{field.Get()}");
                }
            }
            // Newline in between sections.
            if (options.HasFlag(IniExportOptions.NewLineAfterSection) && output.Length != 0)
                output.AppendLine();

            // Output fields that are in sections
            if (alphabeticalSections)
            {
                SortedSet<IniSection> sorted = new(iniFile.Sections, new IniSectionComparer());

                foreach (IniSection section in sorted)
                {
                    output.AppendLine($"[{section.Name}]");
                    if (alphabeticalFields)
                    {
                        SortedSet<IniField> sortedFields = new(section.Fields, new IniFieldKeyComparer());

                        foreach (IniField field in sortedFields)
                        {
                            output.AppendLine($"{field.Key}{keyValueSeparator}{field.Get()}");
                        }
                    }
                    else
                    {
                        foreach (IniField field in section.Fields)
                        {
                            output.AppendLine($"{field.Key}{keyValueSeparator}{field.Get()}");
                        }
                    }
                    // Newline in between sections.
                    if (options.HasFlag(IniExportOptions.NewLineAfterSection))
                        output.AppendLine();
                }
            }
            else
            {
                foreach (IniSection section in iniFile.Sections)
                {
                    output.AppendLine($"[{section.Name}]");
                    if (alphabeticalFields)
                    {
                        SortedSet<IniField> sorted = new(section.Fields, new IniFieldKeyComparer());

                        foreach (IniField field in sorted)
                        {
                            output.AppendLine($"{field.Key}{keyValueSeparator}{field.Get()}");
                        }
                    }
                    else
                    {
                        foreach (IniField field in section.Fields)
                        {
                            output.AppendLine($"{field.Key}{keyValueSeparator}{field.Get()}");
                        }
                    }
                    // Newline in between sections.
                    if (options.HasFlag(IniExportOptions.NewLineAfterSection))
                        output.AppendLine();
                }
            }
            return output.ToString().Trim();
        }
    }

    internal class IniFieldKeyComparer : IComparer<IniField>
    {
        public int Compare(IniField? x, IniField? y)
        {
            if (x is null || y is null)
                return 0;
            else return x.Key.CompareTo(y.Key);
        }
    }

    internal class IniSectionComparer : IComparer<IniSection>
    {
        public int Compare(IniSection? x, IniSection? y)
        {
            if (x is null || y is null)
                return 0;
            else return x.Name.CompareTo(y.Name);
        }
    }
}
