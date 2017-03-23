using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSsimulation
{
    //NON PREEMPTIVE
    class SPN2
    {
        //Dictionary of all the jobs for this sim. Sorted on the time they will enter the queue
        public SortedDictionary<int, Process> job_dict = new SortedDictionary<int, Process>();

        //sortedlist of processes that can be run by the system. Get added to from job_dict upon hitting the time_enter_queue.
        List<Process> SPN_list = new List<Process>();
        bool done = false;


        public SPN2() { }

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
                    Process temp = job_dict[time];
                    int ExecutionTime = 0;

                    for (int k = 0; k < temp.Bursts.Count; k++)
                    {
                        ExecutionTime += temp.Bursts.ElementAt(k).Time;
                    }
                    temp.time_to_run = ExecutionTime;
                    SPN_list.Add(temp);

                    SPN_list.OrderBy(o => o.time_to_run).ToList();

                    job_dict.Remove(time);
                }
            }

        }

        public void run()
        {
           //THE TIME VARIABLE IS THE GLOBAL CLOCK AND TOTAL TIME THE SYSTEM HAS BEEN ACCEPTING JOBS (TOTAL SERVICE TIME)
            int time = 0;

            do
            {
                if(SPN_list.Any() == false && job_dict.Any() == false)
                {
                    done = true;
                }

                if(SPN_list.Count == 0)
                {
                    AddProcessToQueue(time);
                    time++;
                }
                //Some process in the list to run
                else
                {
                    AddProcessToQueue(time);
                    time++;
                }
                
            } while (!done);
        }
    }
}
