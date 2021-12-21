using EasySharpIni.Converters;

namespace EasySharpIni.Models
{
    public class IniField
    {

        private readonly IniFile _file;

        private readonly IniSection? _section;

        private string _key;
        /// <summary>
        /// The Key name of the field.
        /// </summary>
        public string Key => _key;

        private string _rawValue;

        private readonly string _defaultValue;
        /// <summary>
        /// The Default value of the field. 
        /// </summary>
        public string DefaultValue => _defaultValue;

        /// <summary>
        /// Creates a new <see cref="IniField"/> with a key and <see cref="IniFile"/> reference.
        /// </summary>
        /// <param name="key">The key of this field.</param>
        /// <param name="file">The <see cref="IniFile"/> this field is contained in.</param>
        /// <param name="value">The value to assign this field.</param>
        /// <param name="defaultValue">The value this field should take by default if no value is assigned.</param>
        /// <param name="section">The section this field belongs to.</param>
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

        /// <summary>
        /// Returns the raw string value of the field.
        /// </summary>
        /// <returns></returns>
        public string Get()
        {
            return _rawValue;
        }

        /// <summary>
        /// Converts the value of the field to type <typeparamref name="T"/> using the supplied <see cref="IConverter{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="converter">The converter to use.</param>
        /// <returns>The value of this field, as type <typeparamref name="T"/>.</returns>
        public T Get<T>(IConverter<T> converter)
        {
            bool success = converter.Parse(_rawValue, out T result);
            if (!success)
                return converter.GetDefaultValue();

            return result;
        }

        /// <summary>
        /// Sets the raw string value of the field.
        /// </summary>
        /// <param name="value"></param>
        public void Set(string value)
        {
            _rawValue = value;
        }

        /// <summary>
        /// Converts the <typeparamref name="T"/> value to a string and sets it to the value of the field.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value to set.</param>
        /// <param name="converter">The converter to convert use.</param>
        public void Set<T>(T value, IConverter<T> converter)
        {
            _rawValue = converter.ToString(value);
        }

        /// <summary>
        /// Returns the raw string value of the field.
        /// <para>Same as <see cref="Get"/></para>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Get();
        }

        public static implicit operator string(IniField field) => field.ToString();
    }
}
