using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSsimulation
{
    class SPN
    {
        List<Process> shortest_process_next = new List<Process>();

        public SPN(int num)
        {
            for (int j = 0; j < num; ++j)
            {
                Process process = new Process();
                if (shortest_process_next.Count() == 0)
                {
                    shortest_process_next.Add(process);
                }
                else
                {
                    for (int i = shortest_process_next.Count() -1 ; i > -1; --i)
                    {
                        if (process.time_to_run < shortest_process_next[i].time_to_run)
                        {
                            shortest_process_next.Insert(i, process);
                            break;
                        }
                        else if (i == shortest_process_next.Count - 1)
                        {
                            shortest_process_next.Add(process);
                            break;
                        }
                    }
                }
            }
            
        }

        public void run()
        {
            for (int j = shortest_process_next.Count(); j > 0; --j)
            {
                for (int i = shortest_process_next[0].time_to_run; i > 0; --i)
                {
                    //just loop till its done cause no interrupts happen with this algorithm.
                }
                shortest_process_next.RemoveAt(0);
            }
        }
    }
}
