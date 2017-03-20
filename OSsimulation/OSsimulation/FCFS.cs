using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace OSsimulation
{
    class FCFS
    { 
        /**List of processes that will be ran through algorithm*/
        public List<Process> fcfs = new List<Process>();

        /** Constructor
        \param number of processes to add to the list*/
        public FCFS(int num)
        {
            for(int i = 0; i < num; ++i)
            {
                Process process = new Process();
                fcfs.Add(process);
            }
        }

        /**Loops through the process that is first in the list till it is done*/
        public async void Run()
        {
            for (int j = fcfs.Count(); j > 0; --j)
            {
                for (int i = fcfs[0].time_to_run; i > 0; --i)
                {
                    //Just loop through till time hits zero, doesnt really have to do anything while running
                }
                fcfs.RemoveAt(0);
            }
        }

    }
}
