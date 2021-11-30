using CsvHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS3_part1
{
    class Program
    {

        class Data
        {
            private double result;
            private int threadCount;
            private long t;

            public double Result { get => result; set => result = value; }
            public int ThreadCount { get => threadCount; set => threadCount = value; }
            public long Time { get => t; private set => t = value; }


            public Data(double result, int threadCount, long t)
            {
                this.result = result;
                this.threadCount = threadCount;
                this.t = t;
            }
        }


        static int N = 100000000, blockSize = 10 * 930714;


        static int InputCountThread()
        {
            int tc = 0;
            do
            {
                Console.WriteLine("Input thread count:");
                if (!int.TryParse(Console.ReadLine(), out tc) || tc <= 0)
                {
                    Console.WriteLine("Please, input an integer greater than zero!");
                }
            } while (tc <= 0);
            return tc;
        }

        static void Main(string[] args)
        {
            //int ths = InputCountThread();
            List<Data> dataTimeCopy = new List<Data>();
            foreach (int ths in new int[] { 1, 2, 4, 8,12, 16})
            {
                PiCalc calc = new PiCalc(N, blockSize, ths, new WinThread());
                Stopwatch watch = new Stopwatch();
                watch.Start();
                calc.StartCalculation();
                double result = calc.GetResult();
                watch.Stop();
                Console.WriteLine($"Result: {result};\nTime: {watch.ElapsedMilliseconds} ms\nThreads: {ths}");
                dataTimeCopy.Add(new Data(result, ths, watch.ElapsedMilliseconds));
            }

            using (var streamWriter = new StreamWriter(@"c:\dataOS3.csv"))
            using (var csv = new CsvWriter(streamWriter, System.Globalization.CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(dataTimeCopy);
                Console.WriteLine("Finish");
            }

            Console.ReadLine();
    }
    }
}
