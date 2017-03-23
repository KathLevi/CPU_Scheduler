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
        List<Process> list = new List<Process>();
        List<Process> completed = new List<Process>();
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

                    list.Add(temp);

                    job_dict.Remove(time);
                }
            }

        }


        public void run()
        {
            //THE TIME VARIABLE IS THE GLOBAL CLOCK AND TOTAL TIME THE SYSTEM HAS BEEN ACCEPTING JOBS (TOTAL SERVICE TIME)
            int time = 0;
            int time_in_io = 0;
            int time_on_cpu = 0;


            do
            {
                if(list.Any() == false && job_dict.Any() == false)
                {
                    done = true;
                }

                if(list.Count == 0)
                {
                    AddProcessToQueue(time);
                    time++;
                }
                //Some process in the list to run
                else
                {
                    list.OrderByDescending(i => i.time_to_run_remain);
                    //Look at all processes and compare their time_to_run/time_to_run_remaining
                    if(list.First().Bursts.Count != 0)
                    {
                        
                        if (list.First().first_time)
                        {
                            list.First().time_response = time - list.First().time_enter_queue;
                            list.First().time_wait = time - list.First().time_enter_queue;
                            list.First().time_turnaround = list.First().time_to_run;
                            list.First().first_time = false;
                        }

                        for(int k = 0; k < list.First().Bursts.Count; k++)
                        {
                            if (list.First().Bursts.First().IO)
                            {
                                for (int j = 0; j < list.First().Bursts.First().Time; j++)
                                {
                                    AddProcessToQueue(time);
                                    time_in_io++;
                                    time++;
                                }
                                list.First().Bursts.Dequeue();
                            }
                            else
                            {
                                for (int j = 0; j < list.First().Bursts.First().Time; j++)
                                {
                                    time_on_cpu++;
                                    AddProcessToQueue(time);
                                    time++;
                                }
                                list.First().Bursts.Dequeue();
                            }
                        }
                        
                        completed.Add(list.First());
                        list.RemoveAt(0);
                    }
                                        
                }
                
            } while (!done);

            //Variables for averageing
            double total_service_time = time;
            double avg_response_time = 0;
            double avg_wait_time = 0;
            double avg_turnaround_time = 0;

            for (int i = 0; i < completed.Count; i++)
            {
                avg_response_time += completed[i].time_response;
                avg_turnaround_time += completed[i].time_turnaround;
                avg_wait_time += completed[i].time_wait;
            }

            avg_response_time /= completed.Count;
            avg_turnaround_time /= completed.Count;
            avg_wait_time /= completed.Count;

            System.Windows.MessageBox.Show(string.Format("Jobs Completed: {0} in {1} cycles", completed.Count, total_service_time));
            System.Windows.MessageBox.Show(string.Format("Average Wait: {0}", avg_wait_time));
            System.Windows.MessageBox.Show(string.Format("Average TT: {0}", avg_turnaround_time));
            System.Windows.MessageBox.Show(string.Format("Average Response: {0}", avg_response_time));

        }
    }
}
