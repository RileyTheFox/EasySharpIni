using System.Collections.Generic;

namespace EasySharpIni.Models
{
    public class IniSection
    {

        private readonly IniFile _file;

        private string _name = string.Empty;
        /// <summary>
        /// The name of this <see cref="IniSection"/>.
        /// </summary>
        public string Name => _name;

        private readonly List<IniField> _fields = new();
        /// <summary>
        /// A Read-Only list of the fields in this <see cref="IniSection"/>.
        /// </summary>
        public IReadOnlyList<IniField> Fields => _fields;

        internal IniSection(string name, IniFile file)
        {
            _name = name;
            _file = file;
        }

        /// <summary>
        /// Adds a field to the <see cref="IniSection"/> with an optional value and default value. 
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
                    return _fields[i] = new(key, _file, value, string.IsNullOrEmpty(defaultValue) ? _fields[i].DefaultValue : defaultValue);
            }

            IniField field = new(key, _file, value, defaultValue);

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
        /// Removes a global field from this <see cref="IniSection"/>
        /// </summary>
        /// <param name="key"></param>
        public void RemoveField(IniField key)
        {
            _fields.Remove(key);
        }

    }
}
