using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using EasySharpIni;

namespace IniBenchmarks
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class FinalBenchmarks
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
        public IniFile ParseString()
        {
            return IniIO.ReadFile(new IniFile("G:\\Clone Hero\\settings.ini"), lines);
        }

        [Benchmark]
        public IniFile ParseSpan()
        {
            return IniIO.ReadFileSpan(new IniFile("G:\\Clone Hero\\settings.ini"));
        }

    }
}
