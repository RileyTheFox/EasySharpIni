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

        internal static IniFile ReadFile(IniFile file, string[] lines)
        {
            IniSection? section = null;
            foreach (string line in lines)
            {
                // Comments are not parsed
                if (line.StartsWith(";") || string.IsNullOrEmpty(line))
                    continue;

                // Section Parsing
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    string sectionText = line.Substring(1, line.Length - 1).Trim();

                    section = file.CreateSection(sectionText);
                }
                else
                {
                    int equalsIndex = line.IndexOf('=');
                    if (equalsIndex == -1)
                        continue;

                    string key = line.Substring(0, equalsIndex).Trim();
                    string value = line.Substring(equalsIndex + 1, line.Length - equalsIndex - 1).Trim();

                    if (section == null)
                    {
                        file.AddField(key, value);
                    }
                    else section.AddField(key, value);
                }
            }

            return file;
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
