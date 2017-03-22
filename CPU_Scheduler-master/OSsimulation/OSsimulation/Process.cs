﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSsimulation
{
    //Burst class so we can have a list of the bursts for a given process that are IO/CPU based
    class Burst
    {
        //If IO is true then the burst is IO
        public bool IO { get; set; }
        //Time of the given burst
        public int Time { get; set; }


        /**Constructor that assigns given time and io status to the burst object */
        public Burst(bool io, int time)
        {
            this.IO = io;
            this.Time = time;
        }
    }
    class Process
    {
        /**List of Bursts */
        public List<Burst> Bursts = new List<Burst>();

        //Do we need these?

        /**Total time for the process to run*/
        public int time_to_run = 0;
        /** Time that the process requires on the cpu*/
        public int time_on_cpu = 0;
        /**Total time that the process will take in i/o*/
        public int time_in_io = 0;
        /**Total time that the process is in the waiting queue*/
        public int time_in_queue = 0;
        /**Time for the process to enter the queue*/
        public int time_enter_queue = 0;

        /**Turnaround time for given process */
        public double time_turnaround = 0;
        /**Response time for given process */
        public double time_response = 0;
        /** Wait time for given process */
        public double time_wait = 0;



        /**Constructor that assignes a random time to time_in_io and time_on_cpu
         * Then sets time_to_turn to the sum of those two values*/
        public Process()
        {
            //initializing random
            Random rand = new Random();


            //NEED TO SORT PROCESS IN QUEUES ON THIS 
            this.time_enter_queue = rand.Next(1, 100000);

            //Number of bursts to add to the process, randomly generated
            int num_bursts = rand.Next(1, 20);

            //Create and add bursts 
            for (int i = 0; i < num_bursts; i++)
            {
                //randomized burst time
                int burst_time = rand.Next(1, 60);

                //Flip a coin to see if the burst will be IO or not, if it is 2 then it will be IO
                if (rand.Next(2) == 2)
                {
                    Burst tempB = new Burst(true, burst_time);
                    Bursts.Add(tempB);
                }
                //Add CPU burst
                else
                {
                    Burst tempB = new Burst(false, burst_time);
                    Bursts.Add(tempB);
                }
            }


            /*OLD CODE
            
            //generating between 1 and 50000 for time to run i/o and time on cpu
            time_in_io = rand.Next(1, 50000);
            time_on_cpu = rand.Next(1, 50000);
            //Make sure that total time is not greater or less than time to run i/o and time on cpu combined
            time_to_run = time_in_io + time_on_cpu;
            */
        }

    }

}
