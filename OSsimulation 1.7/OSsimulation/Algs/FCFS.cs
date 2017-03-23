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
        public SortedDictionary<int, Process> fcfs = new SortedDictionary<int, Process>();


        /** Constructor
        \param number of processes to add to the list*/
        public FCFS() { }

        /**Loops through the process that is first in the list till it is done*/
        public async void Run(SortedDictionary<int, Process> fcfs)
        {
            //Variable for storing current CPU total runtime, essentially our clock
            int runtime_total = 0;
            int IO_time = 0;
            int CPU_time = 0;

            /* Loop through processes in queue starting at the back of the queue where the shortest process will be next*/
            for (int j = 0; j < fcfs.Count; j++)
            {
                fcfs.ElementAt(j).Value.time_wait = runtime_total;
                fcfs.ElementAt(j).Value.time_response = runtime_total;
                //Burst iteration region
                #region
                //Iterate through each burst not just the time for the process
                while (fcfs.ElementAt(j).Value.Bursts.Any())
                {
                    //IO Case
                    if (fcfs.ElementAt(j).Value.Bursts.Peek().IO)
                    {
                        //Add to IO Burst count
                        fcfs.ElementAt(j).Value.time_in_io += fcfs.ElementAt(j).Value.Bursts.Peek().Time;
                        IO_time += fcfs.ElementAt(j).Value.Bursts.Peek().Time;
                        fcfs.ElementAt(j).Value.Bursts.Dequeue();
                        
                    }
                    //CPU case
                    else
                    {
                        //For recording individual process runtime and total CPU runtime
                        fcfs.ElementAt(j).Value.time_on_cpu += fcfs.ElementAt(j).Value.Bursts.Peek().Time;
                        CPU_time += fcfs.ElementAt(j).Value.Bursts.Peek().Time;

                        if (j == 0)
                        {
                            //First Process Case, runtime is at zero
                            fcfs.ElementAt(0).Value.time_response = 0;
                        }
                        else
                        {
                            //Response time for this process, at the runtime timestamp
                            fcfs.ElementAt(j).Value.time_response = runtime_total;
                        }
                        fcfs.ElementAt(j).Value.Bursts.Dequeue();


                    }
                }
                #endregion

                //When the process is done add the times up and we get our new time for the runtime
                //since the process is done we can go to the next one
                runtime_total += fcfs.ElementAt(j).Value.time_on_cpu;
                runtime_total += fcfs.ElementAt(j).Value.time_in_io;
                //Sets turnaround time to total time the CPU has been running minus the time the processes waited
                fcfs.ElementAt(j).Value.time_turnaround = runtime_total - fcfs.ElementAt(j).Value.time_wait;
                
                //Averages wait time, turnaround time, response time, does not do anything with the data
                // RecordKeeping.Average(fcfs,"FCFS");

            }
            //Variables for averageing
            double total_service_time = runtime_total;
            double avg_response_time = 0;
            double avg_wait_time = 0;
            double avg_turnaround_time = 0;
            double cpu_util = ((double)CPU_time / total_service_time)*100;

            for (int i = 0; i < fcfs.Count; i++)
            {
                avg_response_time += fcfs.ElementAt(i).Value.time_response;
                avg_turnaround_time += fcfs.ElementAt(i).Value.time_turnaround;
                avg_wait_time += fcfs.ElementAt(i).Value.time_wait;
            }
            
            avg_wait_time /= fcfs.Count;
            avg_response_time /= fcfs.Count;
            avg_turnaround_time /= fcfs.Count;

            System.Windows.MessageBox.Show(string.Format("Jobs Completed: {0} in {1} cycles", fcfs.Count, total_service_time));
            System.Windows.MessageBox.Show(string.Format("Average Wait: {0}", avg_wait_time));
            System.Windows.MessageBox.Show(string.Format("Average TT: {0}", avg_turnaround_time));
            System.Windows.MessageBox.Show(string.Format("Average Response: {0}", avg_response_time));
            System.Windows.MessageBox.Show(string.Format("CPU Utilization is: {0}%", cpu_util));
            System.Windows.MessageBox.Show(string.Format("Average Context Switch Time: 0ms because nothing ever switches"));
        }

    }
}