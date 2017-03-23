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
        Queue<Process> queue0 = new Queue<Process>();
        Queue<Process> queue1 = new Queue<Process>();
        Queue<Process> queue2 = new Queue<Process>();
        Queue<Process> queue3 = new Queue<Process>();

        public MFQ() { }

        public void distribute(SortedDictionary<int, Process> sorted_dict)
        {
            for (int i = sorted_dict.Count; i > 0; --i)
            {
                if (i % 2 == 0)
                {
                    queue1.Enqueue(sorted_dict[i]);
                    sorted_dict[i].time_enter_queue = i;
                }
                else if (i % 3 == 0)
                {
                    queue2.Enqueue(sorted_dict[i]);
                    sorted_dict[i].time_enter_queue = i;
                }
                else if (i % 4 == 0)
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

        void run(Queue<Process> thread_queue)
        {
            int runtime_cpu = 0;
            int runtime_total = 0;
            //goes through every process in the queue that this thread is running
            for (int i = thread_queue.Count; i > 0; --i)
            {
                while (thread_queue.Peek().Bursts.Any())
                {
                    if (thread_queue.Peek().Bursts.Peek().IO)
                    {
                        thread_queue.Peek().time_in_io += thread_queue.Peek().Bursts.Peek().Time;
                        thread_queue.Dequeue();
                    }
                    else
                    {
                        thread_queue.Peek().time_on_cpu += thread_queue.Peek().Bursts.Peek().Time;
                        runtime_cpu = thread_queue.Peek().Bursts.Peek().Time;

                        if (i == 0)
                        {
                            thread_queue.Peek().time_response = 0;
                        }
                        else
                        {
                            thread_queue.Peek().time_response = runtime_total;
                        }

                        runtime_total += thread_queue.Peek().Bursts.Peek().Time;

                        thread_queue.Peek().time_turnaround = runtime_cpu - thread_queue.Peek().time_wait;
                        thread_queue.Peek().Bursts.Dequeue();
                    }
                }
            }
        }
    }
}