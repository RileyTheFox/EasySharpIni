using System;

namespace EasySharpIni
{
    /// <summary>
    /// Export flags for manipulating how an <see cref="IniFile"/> should be exported to a string.
    /// </summary>
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
        /// <summary>
        /// The default export options.
        /// <para>Includes <see cref="KeyValueWhitespace"/> and <see cref="NewLineAfterSection"/></para>
        /// </summary>
        Default = KeyValueWhitespace | NewLineAfterSection
    }
}