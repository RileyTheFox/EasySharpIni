using System.Collections.Generic;
using System.Threading.Tasks;

using EasySharpIni.Models;

namespace EasySharpIni
{
    public class IniFile
    {
        private string _path;
        public string Path { get => _path; set => _path = value; }

        private readonly List<IniSection> _sections = new();
        public IReadOnlyList<IniSection> Sections => _sections;

        private readonly List<IniField> _fields = new();
        public IReadOnlyList<IniField> Fields => _fields;

        public IniFile(string path)
        {
            _path = path;
        }

        public IniFile Parse()
        {
            return IniIO.Parse(this);
        }

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        public async Task<IniFile> ParseAsync()
        {
            return await IniIO.ParseAsync(this);
        }
#endif

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

        public IniSection GetSection(string section)
        {
            foreach (IniSection loopSection in _sections)
            {
                if (loopSection.Name == section)
                    return loopSection;
            }

            return CreateSection(section);
        }

        public void RemoveSection(IniSection section)
        {
            _sections.Remove(section);
        }

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

        public override string ToString()
        {
            return IniIO.ExportToText(this, IniExportOptions.Default);
        }

    }
}