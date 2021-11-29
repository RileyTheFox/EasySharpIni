using System.Text;
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

        internal static async Task<IniFile> ParseAsync(IniFile file)
        {
            if (!File.Exists(file.Path))
                return file;

            return ReadFile(file, await File.ReadAllLinesAsync(file.Path));
        }

        internal static void Write(IniFile file, IniExportOptions options)
        {
            File.WriteAllText(file.Path, ExportToText(file, options));
        }

        internal static async Task WriteAsync(IniFile file, IniExportOptions options)
        {
            await File.WriteAllTextAsync(file.Path, ExportToText(file, options));
        }

        private static IniFile ReadFile(IniFile file, string[] lines)
        {
            IniSection? section = null;
            foreach (string line in lines)
            {
                // Comments are not parsed
                if (line.StartsWith(";") || string.IsNullOrEmpty(line))
                    continue;

                // Section Parsing
                if (line.StartsWith('[') && line.EndsWith(']'))
                {
                    string sectionText = line[1..(line.Length - 1)].Trim();

                    section = file.CreateSection(sectionText);
                }
                else
                {
                    int equalsIndex = line.IndexOf('=');
                    if (equalsIndex == -1)
                        continue;

                    string key = line[0..equalsIndex].Trim();
                    string value = line[(equalsIndex + 1)..line.Length].Trim();

                    if (section == null)
                    {
                        file.AddField(key, value);
                    }
                    else section.AddField(key, value);
                }
            }

            return file;
        }

        public static string ExportToText(IniFile file, IniExportOptions options)
        {
            StringBuilder output = new StringBuilder();
            string keyValueSeparator = options.HasFlag(IniExportOptions.KeyValueWhitespace) ? " = " : "=";

            bool alphabeticalFields = options.HasFlag(IniExportOptions.AlphabeticalFields);
            bool alphabeticalSections = options.HasFlag(IniExportOptions.AlphabeticalSections);

            // Output fields that are not in a section.
            if (alphabeticalFields)
            {
                SortedSet<IniField> sorted = new(file.Fields, new IniFieldKeyComparer());

                foreach (IniField field in sorted)
                {
                    output.AppendLine($"{field.Key}{keyValueSeparator}{field.Get()}");
                }
            }
            else
            {
                foreach (IniField field in file.Fields)
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
                SortedSet<IniSection> sorted = new(file.Sections, new IniSectionComparer());

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
                foreach (IniSection section in file.Sections)
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
