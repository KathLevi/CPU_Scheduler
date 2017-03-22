using System;
using System.Collections.Generic;
using System.Data.OleDb;
using Excel = Microsoft.Office.Interop.Excel;

namespace OSsimulation
{
    class RecordKeeping
    {
        public static Excel.Application my_excel = new Excel.Application();
        public static Excel.Workbook my_book = my_excel.Workbooks.Open(Environment.CurrentDirectory + "\\ProcessRecords.xlsx", 0, false);
        public static Excel._Worksheet process_sheet = my_book.Sheets[1];

        public static void UpdateExcel(int processNum, double totaltime, 
            double turnaround, double wait, double response, double contextswitch, 
            double processorUtil, double speedUp)
        {
            //add 2 to the process num to place in the correct position on the excel sheet
            int i = processNum + 2;
            //add in all of the correct values to the excel sheet
            process_sheet.Cells[i, 1] = processNum;
            process_sheet.Cells[i, 2] = totaltime;
            process_sheet.Cells[i, 3] = turnaround;
            process_sheet.Cells[i, 4] = wait;
            process_sheet.Cells[i, 5] = response;
            process_sheet.Cells[i, 6] = contextswitch;
            process_sheet.Cells[i, 7] = processorUtil;
            process_sheet.Cells[i, 8] = speedUp;

            //closing the excel sheet saves and closes the book and application, do we need this every update or only at the end
            //closeExcel();
        }

        public static void closeExcel()
        {
            try
            {
                my_book.Save();
                my_book.Close();
            }
            finally
            {
                if (my_excel != null) { my_excel.Quit(); }
            }
        }
        public static void Average(List<Process> the_processes,string sim_type)
        {
            //averages for our final output
            double throughput = 0;
            double avg_totaltime = 0; //col2
            double avg_turnaround = 0; //col3
            double avg_wait = 0; //col4
            double avg_response = 0; //col5
            double avg_contextSwitch = 0; //col6
            double avg_utilization = 0; //col7
            double avg_speedUp = 0; //col8

            Excel.Range used = process_sheet.UsedRange;
            foreach (Excel.Range row in used.Rows)
            {
                //find the total values for each of the above variables
                //throughput++;
                avg_totaltime += process_sheet.Cells[row.Row, 2].Value2;
                avg_turnaround += process_sheet.Cells[row.Row, 3].Value2;
                avg_wait += process_sheet.Cells[row.Row, 4].Value2;
                avg_response += process_sheet.Cells[row.Row, 5].Value2;
                avg_contextSwitch += process_sheet.Cells[row.Row, 6].Value2;
                avg_utilization += process_sheet.Cells[row.Row, 7].Value2;
                avg_speedUp += process_sheet.Cells[row.Row, 8].Value2;
            }

            //divide the totals by the number of processes for the final results
            int num = the_processes.Count;
            throughput = throughput / num;
            avg_totaltime = avg_totaltime / num;
            avg_turnaround = avg_turnaround / num;
            avg_wait = avg_wait / num;
            avg_response = avg_response / num;
            avg_contextSwitch = avg_contextSwitch / num;
            avg_utilization = avg_utilization / num;
            avg_speedUp = avg_speedUp / num;

            /*foreach(Process p in the_processes)
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
            */
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
