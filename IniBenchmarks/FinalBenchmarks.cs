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
    public class FinalBenchmarks
    {

        private IniFile file;
        private IniFile file2;
        private IniFile file3;

        [GlobalSetup]
        public void Setup()
        {
            file = new("F:\\Downloads\\php.ini");
            file2 = new("F:\\Downloads\\php.ini");
            file3 = new("F:\\Downloads\\php.ini");
        }

        [Benchmark]
        public IniFile ParseString()
        {
            string[] lines = File.ReadAllLines(file.Path);

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
                    string sectionText = trimLine[1..^1].Trim();

                    section = file.CreateSection(sectionText);
                }
                else
                {
                    int equalsIndex = trimLine.IndexOf('=');
                    if (equalsIndex == -1)
                        continue;

                    string key = trimLine[..equalsIndex].Trim();
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
        public IniFile ParseSpanExtensions()
        {
            ReadOnlySpan<char> text = File.ReadAllText(file2.Path).AsSpan().Trim();

            IniSection? section = null;

            ReadOnlySpan<char> remaining = text;
            while (ReadLine(ref remaining, out ReadOnlySpan<char> line))
            {
                if (line.StartsWith(";"))
                    continue;

                // Section Parsing
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    var sectionText = line[1..^1];

                    section = file2.CreateSection(sectionText.ToString());
                }
                else
                {
                    int equalsIndex = line.IndexOf('=');
                    if (equalsIndex == -1)
                        continue;

                    var key = line[..equalsIndex].Trim();
                    var value = line.Slice(equalsIndex + 1, line.Length - equalsIndex - 1).Trim();

                    if (section == null)
                    {
                        file2.AddField(key.ToString(), value.ToString());
                    }
                    else section.AddField(key.ToString(), value.ToString());
                }
            }

            return file2;
        }

        [Benchmark]
        public IniFile ParseSpanManualImpl()
        {
            ReadOnlySpan<char> text = File.ReadAllText(file3.Path).AsSpan().Trim();

            IniSection? section = null;

            ReadOnlySpan<char> remaining = text;
            while (ReadLine(ref remaining, out ReadOnlySpan<char> line))
            {
                // Ignores blank lines (not really blank but the carriage returns and newlines are stripped)
                if (line.Length == 0)
                    continue;

                // Ignore comments
                if (line[0] == ';')
                    continue;

                // Section Parsing
                if (line[0] == '[' && line[^1] == ']')
                {
                    var sectionText = line[1..^1];

                    section = file3.CreateSection(sectionText.ToString());
                }
                else
                {
                    int equalsIndex = line.IndexOf('=');
                    if (equalsIndex == -1)
                        continue;

                    var key = line[..equalsIndex].Trim();
                    var value = line.Slice(equalsIndex + 1, line.Length - equalsIndex - 1).Trim();

                    if (section == null)
                    {
                        file3.AddField(key.ToString(), value.ToString());
                    }
                    else section.AddField(key.ToString(), value.ToString());
                }
            }

            return file;
        }

        private static bool ReadLine(ref ReadOnlySpan<char> span, out ReadOnlySpan<char> line)
        {
            for (int i = 0; i < span.Length; i++)
            {
                if (span[i] == '\n')
                {
                    line = span[..i];
                    span = span.Slice(i + 1, span.Length - i - 1);
                    return true;
                }
                if (span[i] == '\r' && span[i + 1] == '\n')
                {
                    line = span[..i];
                    span = span.Slice(i + 2, span.Length - i - 2);
                    return true;
                }
            }
            line = span;
            return false;
        }
    }
}
