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
        private IniFile file2;

        [GlobalSetup]
        public void Setup()
        {
            file = new("G:\\Clone Hero\\settings.ini");
            file2 = new("G:\\Clone Hero\\settings.ini");
        }

        [Benchmark]
        public IniFile ParseString()
        {
            return file.Parse();
        }

        [Benchmark]
        public IniFile ParseSpan()
        {
            return file2.ParseSpan();
        }

    }
}
