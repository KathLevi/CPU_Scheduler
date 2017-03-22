using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSsimulation
{
    class SRT
    {
        List<Process> srt = new List<Process>();

        public SRT(int num_processes)
        {
            for(int i = 0; i < num_processes; ++i)
            {
                Process process = new Process();
                if (srt.Count == 0)
                    srt.Add(process);
                else
                {
                    for(int j = srt.Count; j > -1; --j)
                    {
                        if (process.time_to_run <= srt[j].time_to_run)
                        {
                            srt.Insert(j, process);
                        }
                        else if (j == srt.Count)
                            srt.Add(process);
                    }
                }
            }
        }
    }
}
