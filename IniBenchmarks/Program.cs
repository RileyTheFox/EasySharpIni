using System;
using BenchmarkDotNet.Running;
using EasySharpIni;
using EasySharpIni.Converters;
using EasySharpIni.Models;

namespace IniBenchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*Console.WriteLine("SPAN IMPLEMENTATION:");
            IniFile ini = new IniFile("G:\\Clone Hero\\settings.ini").ParseSpan();
            foreach (IniSection iniSection in ini.Sections)
            {
                Console.WriteLine($"[{iniSection.Name}]");
                foreach (IniField iniField in iniSection.Fields)
                {
                    Console.WriteLine($"{iniField.Key} = {iniField}");
                }
                Console.WriteLine();
            }*/
            //BenchmarkRunner.Run<LineStringBenchmarks>();
            //BenchmarkRunner.Run<LineSpanBenchmarks>();
            BenchmarkRunner.Run<FinalBenchmarks>();
        }
    }
}