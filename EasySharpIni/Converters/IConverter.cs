namespace EasySharpIni.Converters
{
    public interface IConverter<T>
    {

        public string ToString(T arg);

        public bool Parse(string arg, out T result);

        public string GetDefaultName();

        public T GetDefaultValue();
    }

    public abstract class Converter<T> : IConverter<T>
    {

        /// <summary>
        /// Converts type <typeparamref name="T"/> <paramref name="arg"/> to a <see cref="string"/>.
        /// </summary>
        /// <param name="arg">Type to convert</param>
        /// <returns>Type <typeparamref name="T"/> converted to a <see cref="string"/>.</returns>
        public virtual string ToString(T arg)
        {
            return $"{arg?.ToString()}";
        }

        /// <summary>
        /// Parses <see cref="string"/> <paramref name=" arg"/> to <typeparamref name="T"/> <paramref name=" result"/>.
        /// <remarks>
        /// <para>
        /// <paramref name="result"/> will return <see cref="GetDefaultValue"/> if parsing failed.
        /// </para>
        /// </remarks>
        /// </summary>
        /// <param name="arg">The string to parse from</param>
        /// <param name="result">The parsed result</param>
        /// <returns><see langword="true"/> on success, <see langword="false"/> if parsing failed.</returns>
        public abstract bool Parse(string arg, out T result);

        /// <summary>
        /// The default name for this <see cref="Converter{T}"/>.
        /// </summary>
        public abstract string GetDefaultName();

        /// <summary>
        /// The default value for this <see cref="Converter{T}"/>.
        /// </summary>
        public abstract T GetDefaultValue();
    }
}
