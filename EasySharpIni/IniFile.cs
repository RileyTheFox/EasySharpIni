using System.Collections.Generic;
using System.Threading.Tasks;

using EasySharpIni.Models;

namespace EasySharpIni
{
    public class IniFile
    {
        private string _path;
        /// <summary>
        /// The current path to read and write the ini file to.
        /// </summary>
        public string Path { get => _path; set => _path = value; }

        private readonly List<IniSection> _sections = new();
        /// <summary>
        /// A Read-Only list of the sections in this <see cref="IniFile"/>.
        /// </summary>
        public IReadOnlyList<IniSection> Sections => _sections;

        private readonly List<IniField> _fields = new();
        /// <summary>
        /// A Read-Only list of the global fields in this <see cref="IniFile"/>.
        /// </summary>
        public IReadOnlyList<IniField> Fields => _fields;

        /// <summary>
        /// Initialises an empty <see cref="IniFile"/> object with a file path.
        /// </summary>
        /// <param name="path">The file path to read from.</param>
        public IniFile(string path)
        {
            _path = path;
        }

        /// <summary>
        /// Parse the ini file.
        /// </summary>
        /// <returns>an <see cref="IniFile"/> containing section and field data.</returns>
        public IniFile Parse()
        {
            return IniIO.Parse(this);
        }

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        /// <summary>
        /// Parse the ini file asynchronously.
        /// </summary>
        /// <returns>an <see cref="IniFile"/> containing section and field data.</returns>
        public async Task<IniFile> ParseAsync()
        {
            return await IniIO.ParseAsync(this);
        }
#endif

        /// <summary>
        /// Write the IniFile to a file on disk.
        /// </summary>
        /// <param name="options">The export options to format the ini file.</param>
        /// <param name="pathOverride">The alternate path to save to.</param>
        public void Write(IniExportOptions options = IniExportOptions.Default, string pathOverride = "")
        {
            string oldPath = _path;

            if (!string.IsNullOrEmpty(pathOverride))
            {
                _path = pathOverride;
            }
            IniIO.Write(this, options);
            _path = oldPath;
        }

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        /// <summary>
        /// Write the IniFile to a file on disk asynchronously.
        /// </summary>
        /// <param name="options">The export options to format the ini file.</param>
        /// <param name="pathOverride">The alternate path to save to.</param>
        public async Task WriteAsync(IniExportOptions options = IniExportOptions.Default, string pathOverride = "")
        {
            string oldPath = _path;

            if (!string.IsNullOrEmpty(pathOverride))
            {
                _path = pathOverride;
            }
            await IniIO.WriteAsync(this, options);
            _path = oldPath;
        }
#endif

        /// <summary>
        /// Adds a new section to the <see cref="IniFile"/>.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public IniSection CreateSection(string sectionName, IniSection? section = null)
        {
            for (int i = 0; i < _sections.Count; i++)
            {
                IniSection loopSection = _sections[i];

                if (loopSection.Name == sectionName)
                {
                    return loopSection;
                }
            }

            if (section == null)
                section = new(sectionName, this);

            _sections.Add(section);
            return section;
        }

        /// <summary>
        /// Retrieves a section by name.
        /// </summary>
        /// <param name="section">The name of the section to retrieve.</param>
        /// <returns>The <see cref="IniSection"/> matching the name <paramref name="section"/>,
        /// or creates a new <see cref="IniSection"/> using <paramref name="section"/> if it does not exist.</returns>
        public IniSection GetSection(string section)
        {
            foreach (IniSection loopSection in _sections)
            {
                if (loopSection.Name == section)
                    return loopSection;
            }

            return CreateSection(section);
        }

        /// <summary>
        /// Removes a section from the <see cref="IniFile"/>.
        /// </summary>
        /// <param name="section"></param>
        public void RemoveSection(IniSection section)
        {
            _sections.Remove(section);
        }

        /// <summary>
        /// Adds a global field to the <see cref="IniFile"/> with an optional value and default value. 
        /// <br>If a field with the same key already exists, it is replaced.</br>
        /// </summary>
        /// <param name="key">The key of this field.</param>
        /// <param name="value">The string value to assign this field.</param>
        /// <param name="defaultValue">The default/fallback value for this field if no value is set. Defaults to an empty string.</param>
        /// <returns>The new <see cref="IniField"/>.</returns>
        public IniField AddField(string key, string? value = null, string defaultValue = "")
        {
            for (int i = 0; i < _fields.Count; i++)
            {
                IniField loopField = _fields[i];
                if (loopField.Key == key)
                    return _fields[i] = new(key, this, value, string.IsNullOrEmpty(defaultValue) ? _fields[i].DefaultValue : defaultValue);
            }

            IniField field = new(key, this, value, defaultValue);

            _fields.Add(field);
            return field;
        }

        /// <summary>
        /// Retrieves a field by name.
        /// <br>If the key does not exist, then it is created and assigned the default value of <paramref name="defaultValue"/>.</br>
        /// </summary>
        /// <param name="section">The name of the field to retrieve.</param>
        /// <returns>The <see cref="IniField"/> matching the key <paramref name="key"/>,
        /// or creates a new <see cref="IniField"/> using <paramref name="key"/> if it does not exist.</returns>
        public IniField GetField(string key, string defaultValue = "")
        {
            foreach (IniField loopField in _fields)
            {
                if (loopField.Key == key)
                    return loopField;
            }

            return AddField(key, defaultValue: defaultValue);
        }

        /// <summary>
        /// Removes a global field from this <see cref="IniFile"/>
        /// </summary>
        /// <param name="key"></param>
        public void RemoveField(IniField key)
        {
            _fields.Remove(key);
        }

        /// <summary>
        /// Exports the <see cref="IniFile"/> to a string.
        /// </summary>
        /// <returns>A string representation of the <see cref="IniFile"/>.</returns>
        public override string ToString()
        {
            return IniIO.ExportToText(this, IniExportOptions.Default);
        }

    }
}