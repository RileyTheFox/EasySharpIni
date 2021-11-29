namespace EasySharpIni
{
    [Flags]
    public enum IniExportOptions
    {
        /// <summary>
        /// Puts whitespace between the delimiter ('=') and the key and value.
        /// </summary>
        KeyValueWhitespace = 1,
        /// <summary>
        /// Generates a blank new line in between sections.
        /// </summary>
        NewLineAfterSection = 2,
        /// <summary>
        /// Exports the sections in alphabetical order.
        /// </summary>
        AlphabeticalSections = 4,
        /// <summary>
        /// Exports the fields in alphabetical order.
        /// </summary>
        AlphabeticalFields = 8,
        Default = KeyValueWhitespace | NewLineAfterSection
    }
}