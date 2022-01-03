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
    public class LineSpanBenchmarks
    {

        public IniFile file;

        [GlobalSetup]
        public void Setup()
        {
            file = new("G:\\Clone Hero\\settings.ini");
        }

        [Benchmark]
        public IniFile ReadFileSpan()
        {
            ReadOnlySpan<char> text = File.ReadAllText(file.Path).AsSpan().Trim();

            int currentOffset = 0;
            IniSection? section = null;
            while (currentOffset < text.Length)
            {
                int readTo = GetNextLineOffset(text, currentOffset);

                ReadOnlySpan<char> line = text.Slice(currentOffset, readTo);

                if (line.StartsWith(";"))
                    continue;

                // Section Parsing
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    var sectionText = line.Slice(1, line.Length - 2);

                    section = file.CreateSection(sectionText.ToString());
                }
                else
                {
                    int equalsIndex = line.IndexOf('=');
                    if (equalsIndex == -1)
                        continue;

                    var key = line.Slice(0, equalsIndex).Trim();
                    var value = line.Slice(equalsIndex + 1, line.Length - equalsIndex - 1).Trim();

                    if (section == null)
                    {
                        file.AddField(key.ToString(), value.ToString());
                    }
                    else section.AddField(key.ToString(), value.ToString());
                }

                currentOffset = readTo + 1;
            }

            return file;
        }

        private int GetNextLineOffset(ReadOnlySpan<char> span, int offset)
        {
            for (int i = offset; i < span.Length; i++)
            {
                if (span[i] == '\n')
                {
                    return i;
                }
            }
            return span.Length - 1;
        }

    }
}
