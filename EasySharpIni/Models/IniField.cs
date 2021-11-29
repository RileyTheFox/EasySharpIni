using EasySharpIni.Converters;

namespace EasySharpIni.Models
{
    public class IniField
    {

        private readonly IniFile _file;

        private readonly IniSection? _section;

        private string _key;
        public string Key => _key;

        private string _rawValue;

        private readonly string _defaultValue;
        public string DefaultValue => _defaultValue;

        public IniField(string key, IniFile file, string? value = null, string defaultValue = "", string section = "")
        {
            _key = key;
            _defaultValue = defaultValue;
            _file = file;

            if (!string.IsNullOrEmpty(section))
                _section = file.GetSection(section);

            if (value == null)
                _rawValue = _defaultValue;
            else
                _rawValue = value;
        }

        public string Get()
        {
            return _rawValue;
        }

        public T Get<T>(IConverter<T> converter)
        {
            bool success = converter.Parse(_rawValue, out T result);
            if (!success)
                return converter.GetDefaultValue();

            return result;
        }

        public void Set(string value)
        {
            _rawValue = value;
        }

        public void Set<T>(T value, IConverter<T> converter)
        {
            _rawValue = converter.ToString(value);
        }

        public override string ToString()
        {
            return Get();
        }
    }
}
