using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSsimulation
{
    class Process
    {
        public int time_to_run = 0;
        public int time_on_cpu = 0;
        public int time_in_io = 0;
        public int time_in_queue = 0;

        public Process()
        {
            //initializing random
            Random rand = new Random();
            //generating between 1 and 500 for time to run i/o
            time_in_io = rand.Next(1, 50000);
            //generating between 1 and 500 for time to run on cpu
            time_on_cpu = rand.Next(1, 50000);
            //between 1 and 1000 for total time
            time_to_run = rand.Next(1, 100000);
        }

    }
}
