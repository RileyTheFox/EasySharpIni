namespace EasySharpIni.Models
{
    public class IniSection
    {

        private readonly IniFile _file;

        private string _name = string.Empty;
        public string Name => _name;

        private readonly List<IniField> _fields = new();
        public IReadOnlyList<IniField> Fields => _fields;

        internal IniSection(string name, IniFile file)
        {
            _name = name;
            _file = file;
        }

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

        public IniField GetField(string key, string defaultValue = "")
        {
            foreach (IniField loopField in _fields)
            {
                if (loopField.Key == key)
                    return loopField;
            }

            return AddField(key, defaultValue: defaultValue);
        }

        public void RemoveField(IniField key)
        {
            _fields.Remove(key);
        }

    }
}
