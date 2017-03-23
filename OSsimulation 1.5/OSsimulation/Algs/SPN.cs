using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSsimulation
{
    class SPN
    {
        /**List that holds all the processes*/
        List<Process> shortest_process_next = new List<Process>();
        public SortedDictionary<int, Process> job_dict = new SortedDictionary<int, Process>();

        /**Constructor that creates processes and adds them to the List
         * \param num: number of process to add to the List*/
        public SPN(int num)
        {

            for (int j = 0; j < num; ++j)
            {
                Process process = new Process();

                /*Make sure that the List isnt empty*/
                if (shortest_process_next.Count() == 0)
                {
                    shortest_process_next.Add(process);
                }
                else
                {
                    /*Check if the new process has less time than the process and if it does then insert there*/
                    for (int i = shortest_process_next.Count() - 1; i > -1; --i)
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

        public void AddProcessToQueue(int time)
        {
            //Dont want to do anything if there is nothing in the job dictionary
            if (job_dict.Any() != false)
            {
                //We need to know the key of the next process in the dictionary
                int timeofnextprocess = job_dict.First().Key;
                //CPU clock hits time to add next process into queue
                if (time == timeofnextprocess)
                {
                    //add the process and then remove it from the incoming jobs
                    shortest_process_next.Add(job_dict.ElementAt(time).Value);
                    job_dict.Remove(time);
                }
            }
        }

        /**Runs through the processes in the queue
         * Randomly decides when the process has to go into i/o or cpu*/
        public void run()
        {
            //Loop through List starting at back where shortest process will be next;
            for (int j = shortest_process_next.Count(); j > 0; --j)
            {
                for (int i = shortest_process_next[0].time_to_run; i > 0; --i)
                {
                    //just loop till its done cause no interrupts happen with this algorithm.
                    //Need to add burst times for cpu and i/o
                }
                shortest_process_next.RemoveAt(0);
            }
        }
    }
}