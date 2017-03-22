using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Diagnostics;

namespace OSsimulation
{
    class FCFS
    { 
        /**List of processes that will be ran through algorithm*/
        public List<Process> fcfs = new List<Process>();

        Stopwatch Stopwatch = new Stopwatch();

        /** Constructor
        \param number of processes to add to the list*/
        public FCFS(int num)
        {
            Stopwatch.Start();
            for(int i = 0; i < num; ++i)
            {
                Process process = new Process();
                //Set each processes enter queue time upon creation and addition to the queue
                process.time_enter_queue = Stopwatch.Elapsed.Seconds;

                fcfs.Add(process);

            }
        }

        /**Loops through the process that is first in the list till it is done*/
        public async void Run()
        {
            for (int j = fcfs.Count(); j > 0; --j)
            {
                fcfs[0].time_start = Stopwatch.Elapsed.Seconds;
                for (int i = fcfs[0].time_to_run; i > 0; --i)
                {
                    //Just loop through till time hits zero, doesnt really have to do anything while running
                }
                
                //For math before popped off queue
                fcfs[0].time_end = Stopwatch.Elapsed.Seconds;
                //This is where you record stuff
                //Add some function

                fcfs.RemoveAt(0);
               
            }
            Stopwatch.Stop();
        }

    }
}
