using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using EasySharpIni;
using EasySharpIni.Models;

namespace IniBenchmarks
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class LineStringBenchmarks
    {

        private IniFile file;
        private string[] lines;

        [GlobalSetup]
        public void Setup()
        {
            file = new("G:\\Clone Hero\\settings.ini");
            lines = File.ReadAllLines(file.Path);
        }

        [Benchmark]
        public IniFile ReadFile()
        {
            IniSection? section = null;
            foreach (string line in lines)
            {
                var trimLine = line.Trim();
                // Comments are not parsed
                if (trimLine.StartsWith(";") || string.IsNullOrEmpty(trimLine))
                    continue;

                // Section Parsing
                if (trimLine.StartsWith("[") && trimLine.EndsWith("]"))
                {
                    string sectionText = trimLine.Substring(1, trimLine.Length - 2).Trim();

                    section = file.CreateSection(sectionText);
                }
                else
                {
                    int equalsIndex = trimLine.IndexOf('=');
                    if (equalsIndex == -1)
                        continue;

                    string key = trimLine.Substring(0, equalsIndex).Trim();
                    string value = trimLine.Substring(equalsIndex + 1, trimLine.Length - equalsIndex - 1).Trim();

                    if (section == null)
                    {
                        file.AddField(key, value);
                    }
                    else section.AddField(key, value);
                }
            }

            return file;
        }

        [Benchmark]
        public IniFile ReadFileSpan()
        {
            IniSection? section = null;
            foreach (string line in lines)
            {
                var span = line.AsSpan().Trim();

                // Comments are not parsed
                if (span.StartsWith(";") || span.IsEmpty)
                    continue;

                // Section Parsing
                if (span.StartsWith("[") && span.EndsWith("]"))
                {
                    var sectionText = span.Slice(1, span.Length - 2);

                    section = file.CreateSection(sectionText.ToString());
                }
                else
                {
                    int equalsIndex = span.IndexOf('=');
                    if (equalsIndex == -1)
                        continue;

                    var key = span.Slice(0, equalsIndex).Trim();
                    var value = span.Slice(equalsIndex + 1, span.Length - equalsIndex - 1).Trim();

                    if (section == null)
                    {
                        file.AddField(key.ToString(), value.ToString());
                    }
                    else section.AddField(key.ToString(), value.ToString());
                }
            }

            return file;
        }
    }
}
