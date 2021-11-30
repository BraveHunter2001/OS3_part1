using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OS3_part1
{

    public interface IThread
    {
         uint StartNewThread(Action action);
         void CloseAllThreads();

    }

    class WinThread : IThread
    {
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
      
        private unsafe static extern uint CreateThread(uint* lpThreadAttributes,
            uint dwStackSize,
            ThreadStart lpStartAddress,
            uint* lpParameter,
            uint dwCreationFlags,
            out uint lpThreadId);


        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(uint hObject);


        List<uint> handles = new List<uint>();
        const int stackSize = 1024;
        public unsafe uint StartNewThread(Action action)
        {

            uint a = 0;
            uint* lpThrAtt = &a;
            uint i = 0;
            uint* lpParam = &i;
            uint lpThreadID = 0;
            uint stackSize = 0;

            uint thandle = CreateThread(null,
                stackSize,
                () => action(),
                lpParam, 0, out lpThreadID);
            if (thandle == 0)
                throw new Exception("Unable to create thread");
            handles.Add(thandle);
            return thandle;

        }


        public void CloseAllThreads()
        {
            foreach (var h in handles)
                CloseHandle(h);
            handles.Clear();
        }
    }

}

