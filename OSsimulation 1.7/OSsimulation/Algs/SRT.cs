using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSsimulation
{
    //This one is the only premptive one
    class SRT
    {

        Queue<Process> process_queue = new Queue<Process>();
        Queue<Process> IO_queue = new Queue<Process>();
        public SortedDictionary<int, Process> job_dict = new SortedDictionary<int, Process>();
        List<Process> completed = new List<Process>();
        bool done = false;

        public SRT() { }

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
                    for(int i = 0; i < job_dict[time].Bursts.Count; i++)
                    {
                        job_dict[time].time_to_run += job_dict[time].Bursts.ElementAt(i).Time;
                    }
                    job_dict[time].time_to_run_remain = job_dict[time].time_to_run;
                    process_queue.Enqueue(job_dict[time]);
                    job_dict.Remove(time);
                }
            }

        }

        public void CheckIOQueue()
        {
            if (IO_queue.Count != 0 && IO_queue.First().Bursts.Count != 0)
            {
                IO_queue.First().Bursts.First().Time--;
                IO_queue.First().time_to_run_remain--;

                if (IO_queue.First().Bursts.First().Time == 0)
                {
                    IO_queue.First().Bursts.Dequeue();
                    process_queue.Enqueue(IO_queue.Dequeue());
                }
            }
        }

        public void run()
        {
            int time = 0;

            do
            {

                process_queue.OrderByDescending(i => i.time_to_run_remain);

                if(IO_queue.Any() == false && process_queue.Any() == false && job_dict.Any() == false)
                {
                    done = true;
                }

                //No processes to run yet
                if (process_queue.Count == 0)
                {
                    //Check if we need to add one
                    AddProcessToQueue(time);
                    CheckIOQueue();
                    time++;
                }
                else
                {


                    if(process_queue.Peek().Bursts.Count != 0)
                    {
                        process_queue.Peek().counter = time;

                        //IO BURST
                        #region
                        if (process_queue.Peek().Bursts.First().IO)
                        {
                            if (process_queue.Peek().first_time)
                            {
                                process_queue.Peek().time_wait = time - process_queue.Peek().time_enter_queue;
                                process_queue.Peek().first_time = false;
                            }
                            //Pops off top and adds to bottom of IO queue
                            Process temp = process_queue.Dequeue();
                            temp.time_in_io += temp.Bursts.First().Time;
                            temp.context_switch_time += 3;
                            IO_queue.Enqueue(temp);

                        }
                        #endregion

                        //CPU BURST
                        #region
                        else
                        {
                            if (process_queue.First().Bursts.Any())
                            {
                                for(int i = 0; i < process_queue.First().Bursts.First().Time; i++)
                                {
                                    process_queue.First().time_on_cpu++;
                                    AddProcessToQueue(time);
                                    CheckIOQueue();
                                    time++;
                                }
                                process_queue.Peek().time_to_run_remain -= process_queue.Peek().Bursts.First().Time;
                                process_queue.Peek().Bursts.Dequeue();
                            }
                            
                        }
                        #endregion

                    }
                    else
                    {
                        Process temp = process_queue.Dequeue();
                        temp.time_wait += temp.counter;
                        temp.counter = time;
                        temp.context_switch_time += 3;
                        completed.Add(temp);
                    }
                }
                
                
            }
            while (!done);

            double total_service_time = time;
            double avg_response_time = 0;
            double avg_wait_time = 0;
            double avg_turnaround_time = 0;
            double avg_context_time = 0;

            for (int i = 0; i < completed.Count; i++)
            {
                avg_response_time += completed[i].time_response;
                avg_turnaround_time += completed[i].time_wait;
                avg_turnaround_time += completed[i].time_to_run;
                avg_wait_time += completed[i].time_wait;
                avg_context_time += completed[i].context_switch_time;
                RecordKeeping.UpdateExcel_SRT(completed[i].PID, completed[i].time_to_run, completed[i].time_turnaround, completed[i].time_wait
                    , completed[i].time_response, completed[i].context_switch_time, 0, 0);
            }
            avg_context_time /= completed.Count;
            avg_wait_time /= completed.Count;
            avg_response_time /= completed.Count;
            avg_turnaround_time /= completed.Count;

            //System.Windows.MessageBox.Show(string.Format("Jobs Completed: {0} in {1} cycles at 1ms a cycle", completed.Count, total_service_time));
            //System.Windows.MessageBox.Show(string.Format("Average Wait: {0}", avg_wait_time));
            //System.Windows.MessageBox.Show(string.Format("Average TT: {0}ms", avg_turnaround_time));
            //System.Windows.MessageBox.Show(string.Format("Average Response: {0}ms", avg_response_time));
            //System.Windows.MessageBox.Show(string.Format("CPU Utilization is: 100%"));
            //System.Windows.MessageBox.Show(string.Format("Average Context Switch Time: {0}ms", avg_context_time));
        }
    }
}
