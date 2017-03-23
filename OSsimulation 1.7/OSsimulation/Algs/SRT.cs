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
        SRT() { }

        /**List of all processes that will be running through this program
         * these will be added to a list that contains processes that are
         * trying to run through the algorithm currently. If the new processes 
         * that is added has a shorter total_runtime than the one currently running
         * then it will premempt the one running, otherwise it will just be added
         * to the List*/
        Queue<Process> queue = new Queue<Process>();

        /**list that is running in the algorithm that can be premempted*/
        List<Process> list = new List<Process>();

        /**List for completed processes*/
        List<Process> complete_list = new List<Process>();

        /**adds from the dictionary to the queue
         * \param SortedDictionary<int, process> the dictionary holding out processes*/
        public void addToQueue(SortedDictionary<int, Process> sorted_dict)
        {
            for (int i = 0; i < sorted_dict.Count; ++i)
            {
                queue.Enqueue(sorted_dict[i]);
            }
        }

        public void add_list()
        {
            if (queue.Count != 0)
            {
                if (list.Count == 0)
                    list.Add(queue.Dequeue());
                else
                {
                    for (int i = 0; i < list.Count; ++i)
                    {
                        if (queue.Peek().time_to_run < list[i].time_to_run)
                        {
                            list.Insert(i, queue.Dequeue());
                        }
                        else
                        {
                            list.Add(queue.Dequeue());
                        }
                    }
                }
            }
        }

        //going to run it just like fcfs bu after every burst add new process from queue and check srt to do premeption before next burst
        public void run()
        {
            while (queue.Count != 0 && list.Count != 0)
            {
                add_list();
                
                               
            }
        }


    }
}
