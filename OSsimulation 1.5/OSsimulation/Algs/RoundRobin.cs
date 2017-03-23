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
        Queue<Process> IO = new Queue<Process>();
        public SortedDictionary<int, Process> job_dict = new SortedDictionary<int, Process>();
        bool done = false;

        /**constructor
        \param Process to be added to the List*/
        public RoundRobin() { }

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
                    roundr.Enqueue(job_dict[time]);
                    job_dict.Remove(time);
                }
            }

        }
        public void CheckIOQueue()
        {
            if (IO.Count != 0 && IO.First().Bursts.Count != 0)
            {
                IO.First().Bursts.First().Time--;
                if (IO.First().Bursts.First().Time == 0)
                {
                    IO.First().Bursts.Dequeue();
                    roundr.Enqueue(IO.Dequeue());
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

                //We want to stop running if there are no more processes to add or processes to run
                if (roundr.Any() == false && job_dict.Any() == false && IO.Any() == false)
                {
                    done = true;
                }

                //No processes to run yet
                if (roundr.Count == 0)
                {
                    //Check if we need to add one
                    AddProcessToQueue(time);
                    CheckIOQueue();
                    time++;
                }

                //We have at least one process in the queue
                else
                {
                    //THIS AREA OF CODE IS WHERE A PROCESS IS RUNNING  
                    //We want it to run as long as there are processes in the queue
                    //Burst Handling of top process

                    //How should be be handling IO?
                    if (roundr.Peek().Bursts.Count != 0)
                    {

                        System.Windows.MessageBox.Show((roundr.Peek().Bursts.First().IO.ToString()));

                        if (roundr.Peek().Bursts.First().IO)
                        {
                            if (roundr.Peek().first_time)
                            {
                                roundr.Peek().time_wait = time - roundr.Peek().time_enter_queue;
                                roundr.Peek().first_time = false;
                            }
                            //Pops off top and adds to bottom of IO queue
                            Process temp = roundr.Dequeue();
                            temp.time_in_io += temp.Bursts.First().Time;
                            IO.Enqueue(temp);

                        }

                        //CPU Burst Handling
                        else
                        {

                            if (roundr.Peek().first_time)
                            {
                                roundr.Peek().time_wait = time - roundr.Peek().time_enter_queue;
                                roundr.Peek().first_time = false;
                            }
                            //Just need to pop off the burst and add that time to the global time
                            if (roundr.Peek().Bursts.First().Time > time_quantum)
                            {
                                for (int k = 0; k < roundr.Peek().Bursts.First().Time; k++)
                                {
                                    roundr.First().time_on_cpu++;
                                    AddProcessToQueue(time);
                                    CheckIOQueue();
                                    time++;
                                }

                            }
                            else
                            {
                                //need while loop to go until there is no more time in the quantum
                                int remain = time_quantum;
                                do
                                {

                                    //Need to pop off that last one since theres another burst we can do
                                    if (roundr.Peek().Bursts.Any())
                                    {
                                        for (int k = 0; k < roundr.Peek().Bursts.First().Time; k++)
                                        {
                                            roundr.First().time_on_cpu++;
                                            AddProcessToQueue(time);
                                            CheckIOQueue();
                                            remain--;
                                            time++;
                                        }

                                        roundr.Peek().Bursts.Dequeue();
                                    }
                                    else
                                    {
                                        remain = 0;
                                    }
                                    //No more bursts for the process

                                } while (remain > 0);

                                Process temp = roundr.Dequeue();
                                roundr.Enqueue(temp);
                            }
                        }

                    }
                    else
                    {
                        //Add to completed processes
                        roundr.Peek().time_to_run = time - roundr.Peek().time_enter_queue;
                        completed.Add(roundr.Dequeue());

                    }
                }

            } while (!done);


            System.Windows.MessageBox.Show("Done");
        }
    }
}