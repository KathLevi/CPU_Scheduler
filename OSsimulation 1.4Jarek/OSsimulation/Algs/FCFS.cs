using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Diagnostics;

namespace OSsimulation
{
    //Rework this entire class to have load sharing and work with threads
    class FCFS
    { 
        /**List of processes that will be ran through algorithm*/
        //currently not using
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
                fcfs.ElementAt(j).Value.time_wait = runtime_CPU;
                
                
                //Iterate through each burst not just the time for the process
                while (fcfs.ElementAt(j).Value.Bursts.Any())
                {
                    //IO Case
                    if (fcfs.ElementAt(j).Value.Bursts.Peek().IO)
                    {
                        //Add to IO Burst count
                        fcfs.ElementAt(j).Value.time_in_io += fcfs.ElementAt(j).Value.Bursts.Peek().Time;
                        fcfs.ElementAt(j).Value.Bursts.Dequeue();
                    }
                    //CPU case
                    else
                    {
                        //For recording individual process runtime and total CPU runtime
                        fcfs.ElementAt(j).Value.time_on_cpu += fcfs.ElementAt(j).Value.Bursts.Peek().Time;
                        runtime_CPU = fcfs.ElementAt(j).Value.Bursts.Peek().Time;

                        if(j == 0)
                        {
                            //First Process Case, runtime is at zero
                            fcfs.ElementAt(0).Value.time_response = 0;
                        }
                        else
                        {
                            //Response time for this process, at the runtime timestamp
                            fcfs.ElementAt(j).Value.time_response = runtime_total;
                        }

                        //Add whatever burst just happened to total runtime
                        runtime_total += fcfs.ElementAt(j).Value.Bursts.Peek().Time;

                        //Sets turnaround time to total time the CPU has been running
                        fcfs.ElementAt(j).Value.time_turnaround = runtime_CPU - fcfs.ElementAt(j).Value.time_wait;
                        fcfs.ElementAt(j).Value.Bursts.Dequeue();
                    }
                }


               //Averages wait time, turnaround time, response time, does not do anything with the data
               // RecordKeeping.Average(fcfs,"FCFS");
               
            }
            
        }

    }
}
