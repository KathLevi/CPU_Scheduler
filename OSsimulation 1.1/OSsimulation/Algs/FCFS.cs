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
        public SortedDictionary<int,Process> fcfs = new SortedDictionary<int,Process>();


        /** Constructor
        \param number of processes to add to the list*/
        public FCFS(){ }          

        /**Loops through the process that is first in the list till it is done*/
        public async void Run(SortedDictionary<int,Process> fcfs)
        {
            //Variable for storing current CPU total runtime, essentially our clock
            int runtime_total = 0;
            int runtime_CPU = 0;
            
            /* Loop through processes in queue starting at the back of the queue where the shortest process will be next*/
            for (int j = 0; j < fcfs.Count; j++)
            {
                fcfs[j].time_wait = runtime_CPU;
                
                //Iterate through each burst not just the time for the process
                while (fcfs[j].Bursts.Any())
                {
                    //Just loop through till time hits zero, doesnt really have to do anything while running
                    //Need to add burst times for when process is on cpu and in i/o

                    //IO Case
                    if (fcfs[j].Bursts.Peek().IO)
                    {
                        //Add to IO Burst count
                        fcfs[j].time_in_io += fcfs[j].Bursts.Peek().Time;
                        fcfs[j].Bursts.Dequeue();
                    }
                    //CPU case
                    else
                    {
                        //For recording individual process runtime and total CPU runtime
                        fcfs[j].time_on_cpu += fcfs[j].Bursts.Peek().Time;
                        runtime_CPU = fcfs[j].Bursts.Peek().Time;

                        if(j == 0)
                        {
                            //First Process Case, runtime is at zero
                            fcfs[0].time_response = 0;
                        }
                        else
                        {
                            //Response time for this process, at the runtime timestamp
                            fcfs[j].time_response = runtime_total;
                        }

                        //Add whatever burst just happened to total runtime
                        runtime_total += fcfs[j].Bursts.Peek().Time;

                        //Sets turnaround time to total time the CPU has been running
                        fcfs[j].time_turnaround = runtime_CPU - fcfs[j].time_wait;
                        fcfs[j].Bursts.Dequeue();
                    }
                }


               //Averages wait time, turnaround time, response time, does not do anything with the data
               // RecordKeeping.Average(fcfs,"FCFS");
               
            }
            
        }

    }
}
