using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSsimulation
{
    class RecordKeeping
    {

        public static void Average(List<Process> the_processes,string sim_type)
        {
            double avg_turnaround = 0;
            double avg_response = 0;
            double avg_wait = 0;
            double servicetime = 0;
            foreach(Process p in the_processes)
            {
                avg_response += p.time_response;
                avg_turnaround += p.time_turnaround;
                avg_wait += p.time_wait;
                servicetime += p.time_on_cpu;

            }


            //What do yall want to do with this data now?
            avg_response /= the_processes.Count;
            avg_turnaround /= the_processes.Count;
            avg_wait /= the_processes.Count;
            //servicetime
            //SendTest(sim_type, avg_turnaround, avg_wait, avg_response, servicetime);
        }

        public static void SendTest(string sim_type, double turnaround, double wait, double response,double service)
        {
            try
            {
                string ConnectionPath = "Provider=sqloledb; Data Source = CS1; Initial Catalog = SIM_DB; Integrated Security = SSPI;";
                OleDbConnection MyConnection = new OleDbConnection(ConnectionPath);
                MyConnection.Open();
                string Command = "INSERT INTO Simulation (ALG_type, AVG_Turnaround, AVG_Response, AVG_Wait, ServiceTime) VALUES (@Alg_Type, @AVG_Turnaround, @AVG_Response, @AVG_Wait, @ServiceTime);";
                OleDbTransaction trans = MyConnection.BeginTransaction();
                OleDbCommand cmd = new OleDbCommand(Command, MyConnection, trans);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ALG_type",sim_type);
                cmd.Parameters.AddWithValue("@AVG_Turnaround", turnaround);
                cmd.Parameters.AddWithValue("@AVG_Response", response);
                cmd.Parameters.AddWithValue("@AVG_Wait", wait);
                cmd.Parameters.AddWithValue("@ServiceTime", service);
                cmd.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Oops error: " + ex.Message + ex.StackTrace);
            }
        }

    }
}
