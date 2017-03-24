using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace OSsimulation
{
    //Multiple Feedback Queue
    class MFQ
    {
        Mutex mutex = new Mutex();
        /**Queues that will get values from the main process dictionary*/
        Queue<Process> queue0 = new Queue<Process>();
        Queue<Process> queue1 = new Queue<Process>();
        Queue<Process> queue2 = new Queue<Process>();
        Queue<Process> queue3 = new Queue<Process>();

        List<Process> completed = new List<Process>();


        /**Default constructor*/
        public MFQ() { }

        /**Distributes all the processes created in the sorted dictionary into 4 seperate fcfs queues
         * \param SortedDictionary<int, Process> that will be split into the seperate queues*/
        public void distribute(SortedDictionary<int, Process> sorted_dict)
        {
            for (int i = sorted_dict.Count; i > 0; --i)
            {
                /*Starting with i % 4 so that it will recieve values as well*/
                if (i % 4 == 0)
                {
                    queue1.Enqueue(sorted_dict[i]);
                    sorted_dict[i].time_enter_queue = i;
                }
                else if (i % 3 == 0)
                {
                    queue2.Enqueue(sorted_dict[i]);
                    sorted_dict[i].time_enter_queue = i;
                }
                else if (i % 2 == 0)
                {
                    queue3.Enqueue(sorted_dict[i]);
                    sorted_dict[i].time_enter_queue = i;
                }
                else
                {
                    queue0.Enqueue(sorted_dict[i]);
                    sorted_dict[i].time_enter_queue = i;
                }
            }
            System.Windows.MessageBox.Show("Split all values");
        }

        /**Initialize and run 4 threads, each thread gets one of the queues to simulate multiple processors*/
        public void thread_run()
        {
            Thread thread0 = new Thread(() => run(queue0));
            Thread thread1 = new Thread(() => run(queue1));
            Thread thread2 = new Thread(() => run(queue2));
            Thread thread3 = new Thread(() => run(queue3));

            thread0.Start();
            thread1.Start();
            thread2.Start();
            thread3.Start();

            thread0.Join();
            thread1.Join();
            thread2.Join();
            thread3.Join();
        }

        /**Function that runs through the queue, each thread runs this.
         * \param Queue<Process> the queue that the thread will be running
               runs as fcfs for all queues*/
        void run(Queue<Process> thread_queue)
        {
            double cpu_utilization = 0;
            int runtime_total = 0;
            //goes through every process in the queue that this thread is running
            for (int i = 0; i < thread_queue.Count; ++i)
            {
                thread_queue.Peek().time_wait = runtime_total;
                thread_queue.Peek().time_response = runtime_total;

                while (thread_queue.Peek().Bursts.Any())
                {
                    //IO burst
                    if (thread_queue.Peek().Bursts.Peek().IO)
                    {
                        thread_queue.Peek().time_in_io += thread_queue.Peek().Bursts.Peek().Time;
                        thread_queue.Peek().Bursts.Dequeue();
                    }
                    //cpu burst
                    else
                    {
                        thread_queue.Peek().time_on_cpu += thread_queue.Peek().Bursts.Peek().Time;

                        if (i == 0)
                        {
                            thread_queue.Peek().time_response = 0;
                        }
                        else
                        {
                            thread_queue.Peek().time_response = runtime_total;
                        }
                        thread_queue.Peek().Bursts.Dequeue();
                        cpu_utilization += thread_queue.Peek().time_on_cpu;
                    }

                }
                //When the process is done add the times up and we get our new time for the runtime
                //since the process is done we can go to the next one
                runtime_total += thread_queue.Peek().time_on_cpu;
                runtime_total += thread_queue.Peek().time_in_io;
                //sets turnaround time to total time the cpu has been running minus the time the process waited
                thread_queue.Peek().time_turnaround = runtime_total - thread_queue.Peek().time_wait;
            }

            double total_service_time = runtime_total;
            double avg_response_time = 0;
            double avg_wait_time = 0;
            double avg_turnaround_time = 0;

            for (int i = 0; i < thread_queue.Count; ++i)
            {
                avg_response_time += thread_queue.Peek().time_response;
                avg_turnaround_time += thread_queue.Peek().time_turnaround;
                avg_wait_time += thread_queue.Peek().time_wait;
            }

            avg_wait_time /= thread_queue.Count;
            avg_response_time /= thread_queue.Count;
            avg_turnaround_time /= thread_queue.Count;
            cpu_utilization = (thread_queue.Peek().time_on_cpu) / (thread_queue.Peek().time_to_run);
            mutex.WaitOne();
            try
            {
                RecordKeeping.UpdateExcel_MFQ(thread_queue.Peek().PID, thread_queue.Peek().time_to_run,
                thread_queue.Peek().time_turnaround, thread_queue.Peek().time_wait, thread_queue.Peek().time_response,
                0, cpu_utilization, 0);
                while (thread_queue.Count != 0)
                {
                    completed.Add(thread_queue.Dequeue());
                }
                System.Windows.MessageBox.Show(completed.Count.ToString());
            }
            finally
            {
                mutex.ReleaseMutex();
            }

        }
    }
}