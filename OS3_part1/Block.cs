using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS3_part1
{
    public class Block
    {
        IIterationProvider iterationProvider;

        public Block(IIterationProvider iterationProvider)
        {
            this.iterationProvider = iterationProvider;
        }

        double sum = 0;
        static double CalcIter(int iter, int accuracy)
        {
            var x_i = (iter + 0.5d) / (accuracy);
            return 4d / (1 + (x_i * x_i));
        }

        public void Calculate(int startIter, int endIter, int accuracy)
        {
            while (startIter <= endIter)
                sum += CalcIter(startIter++, accuracy);
        }

        public void StartCalculate()
        {
            if (iterationProvider == null)
                return;
            while (iterationProvider.StartNewIterationBlock(this)) ;
            Finished = true;
        }
        public bool Finished { get; private set; } = false;
        public double Result => sum;

    }
}
