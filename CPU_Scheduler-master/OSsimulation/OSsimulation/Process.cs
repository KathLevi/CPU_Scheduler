using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSsimulation
{
    class Process
    {
        /**Total time for the process to run*/
        public int time_to_run = 0;
        /** Time that the process requires on the cpu*/
        public int time_on_cpu = 0;
        /**Total time that the process will take in i/o*/
        public int time_in_io = 0;
        /**Total time that the process is in the waiting queue*/
        public int time_in_queue = 0;

        /**Time the processor entered the queue*/
        public int time_enter_queue;
        /**Time that the process was created*/
        public int time_start;
        /**Time that the process terminates*/
        public int time_end;

        /**Constructor that assignes a random time to time_in_io and time_on_cpu
         * Then sets time_to_tun to the sum of those two vallues*/
        public Process()
        {
            //initializing random
            Random rand = new Random();
            //generating between 1 and 50000 for time to run i/o and time on cpu
            time_in_io = rand.Next(1, 50000);
            time_on_cpu = rand.Next(1, 50000);
            //Make sure that total time is not greater or less than time to run i/o and time on cpu combined
            time_to_run = time_in_io + time_on_cpu;
        }

    }
}
