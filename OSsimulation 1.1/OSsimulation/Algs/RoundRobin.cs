﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace OSsimulation
{
    class RoundRobin
    {
        /**container for all the proccesses that we are running.*/
        Queue<Process> roundr = new Queue<Process>();
        List<Process> completed = new List<Process>();
        public SortedDictionary<int, Process> job_dict = new SortedDictionary<int, Process>();
        bool done = false;

        /**constructor
        \param Process to be added to the List*/
        public RoundRobin()
        {        
        }

        public void AddProcessToQueue(int time) {
            //Dont want to do anything if there is nothing in the job dictionary
            if (job_dict.Any() != false)
            {
                //We need to know the key of the next process in the dictionary
                int timeofnextprocess = job_dict.First().Key;
                //CPU clock hits time to add next process into queue
                if (time == timeofnextprocess)
                {
                    //add the process and then remove it from the incoming jobs
                    roundr.Enqueue(job_dict.ElementAt(time).Value);
                    job_dict.Remove(time);
                }
            }
            
        }

        /**runs through the algorithm until all processes are added to queue*/
        public void Run(int time_quantum)
        {
            //THE TIME VARIABLE IS THE GLOBAL CLOCK AND TOTAL TIME THE SYSTEM HAS BEEN ACCEPTING JOBS (TOTAL SERVICE TIME)
            int time = 0;

            do
            {
                #region
                //We want to stop running if there are no more processes to add or processes to run
                if (roundr.Any() == false && job_dict.Any() == false)
                {
                    done = true;
                }
                //No processes to run yet
                if (roundr.Count == 0)
                {

                    //Check if we need to add one
                    AddProcessToQueue(time);
                    time++;
                }
                #endregion
                //We have at least one process in the queue
                else
                {
                    //THIS AREA OF CODE IS WHERE A PROCESS IS RUNNING
                    //HOW DO WE RECORD STATS IF WE ARE GOING TO SEE THE SAME PROCESS AGAIN?
                    //NEED SOME KIND OF "RemoveProcess" METHOD FOR REMOVAL AND RECORDING
                    //NEED SOME WAY TO RECORD TIME  
                    //We want it to run as long as there are processes in the queue
                    //Burst Handling of top process
                    #region
                    //How should be be handling IO?

                    if (roundr.Peek().Bursts.First().IO)
                    {
                        //Pops off top and adds to bottom if IO
                        Process temp = roundr.Dequeue();
                        temp.time_in_io += temp.Bursts.First().Time;
                        temp.Bursts.Dequeue();
                        roundr.Enqueue(temp);
                    }
                    #endregion
                    #region
                    //CPU Burst Handling
                    else
                    {
                        //Just need to pop off the burst and add that time to the global time
                        if (roundr.Peek().Bursts.First().Time > time_quantum)
                        {
                            for (int k = 0; k < roundr.Peek().Bursts.First().Time; k++)
                            {
                                roundr.First().time_on_cpu++;
                                AddProcessToQueue(time);
                                time++;
                            }
                            
                        }
                        else
                        {
                            //need while loop to go until there is no more time in the quantum
                            int remain = time_quantum;
                            do
                            {
                                for (int k = 0; k < roundr.Peek().Bursts.First().Time; k++)
                                {
                                    roundr.First().time_on_cpu++;
                                    AddProcessToQueue(time);
                                    remain--;
                                    time++;
                                }
                                //Need to pop off that last one since theres another burst we can do
                                if (roundr.Peek().Bursts.Any())
                                {
                                    roundr.Peek().Bursts.Dequeue();
                                }
                                //No more bursts for the process
                                else
                                {
                                    //Add to completed processes
                                    roundr.Peek().time_to_run = time - roundr.Peek().time_enter_queue;
                                    
                                    completed.Add(roundr.Dequeue());

                                }
                            } while (remain > 0);

                            Process temp = roundr.Dequeue();
                            roundr.Enqueue(temp);
                        }
                    }
                    #endregion

                }


            } while (!done);

        }
    }
}
