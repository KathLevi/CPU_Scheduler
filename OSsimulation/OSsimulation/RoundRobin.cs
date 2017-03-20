using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSsimulation
{
    class RoundRobin
    {
        /**container for all the proccesses that we are running.*/
        List<Process> roundr;

        /**constructor
        \param Process to be added to the List*/
        public RoundRobin(Process process)
        {
            roundr.Add(process);
        }

        /**runs through the algorithm until all processes are done.*/
        public void Run()
        {
            //runs the process for 20 ticks and updates time them puts it to the back of the List
            for(int i = roundr[0].time_to_run; i > 0; i-=1)
            {
                if (i == 20)
                    roundr[0].time_to_run -= i;
                    break;
            }
            //puts current process to the back of the queue to wait its turn again
            if (roundr[0].time_to_run > 0)
            {
                roundr.Add(roundr[0]);
                roundr.RemoveAt(0);
            }
            //if its done then remove it
            else
            {
                roundr.RemoveAt(0);
            }

        }
    }
}
