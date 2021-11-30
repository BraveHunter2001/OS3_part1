using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS3_part1
{

    public interface IIterationProvider
    {

        bool StartNewIterationBlock(Block block);
    }
    public class PiCalc: IIterationProvider
    {
		int totalIterations, blockSize, currentIteration = 0, operationBlockCount;
		List<Block> blocks = new List<Block>();
		IThread thread;

		public PiCalc(int totalIterations, int blockSize, int operationBlockCount,
			IThread thread)
		{
			this.totalIterations = totalIterations;
			this.blockSize = blockSize - 1;
			this.operationBlockCount = operationBlockCount;
			this.thread = thread;
			for (int i = 0; i < operationBlockCount; i++)
				blocks.Add(new Block(this));
		}


		public void StartCalculation()
		{
			for (int i = 0; i < operationBlockCount; i++)
			{
			
				var j = i;
				thread.StartNewThread(blocks[j].StartCalculate);
			}
		}
		public bool StartNewIterationBlock(Block block)
		{
			int currentStart, currentEnd;
			lock (this)
			{
				int currentBlock = Math.Min(blockSize, totalIterations - currentIteration);
				if (currentBlock <= 0)
					return false;
				currentStart = currentIteration;
				currentIteration += currentBlock + 1;
				currentEnd = currentIteration - 1;
			}
			block.Calculate(currentStart, currentEnd, totalIterations + 1);
			return true;
		}

		public double GetResult()
		{
			double result = 0;
			foreach (var op in blocks)
			{
				while (!op.Finished)
					;
				result += op.Result;
			}
			thread.CloseAllThreads();
			return result / (totalIterations + 1);
		}

	}
}
