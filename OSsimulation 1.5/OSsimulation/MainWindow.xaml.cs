using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;

//WHEN THE PROGRAM IS DONE RUNNING WE NEED TO REACTIVATE THE RUN, ADD, SUB?
//SET RESULTS BUTTON TO ENABLED WHEN THE PROGRAM IS DONE --> ResultsBtn.IsEnabled = true;
namespace OSsimulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //UPDATE EXCEL USING: RecordKeeping.UpdateExcel_**(processNum, totalTime, turnaround, wait, response, contextSwitch, processorUtil, speedUp);
       
        public MainWindow()
        {
            InitializeComponent();
            
        }

        //create COM objects for everything referenced
        public static Excel.Application my_excel = new Excel.Application();
        public static Excel.Workbook my_book = my_excel.Workbooks.Open(Environment.CurrentDirectory + "\\ProcessRecords.xlsx", 0, false);
        public static Excel._Worksheet process_sheet = my_book.Sheets[1];
        public static Excel.Range my_range = process_sheet.UsedRange;

        //PUT THE ON CLICK TO GET MORE DATA HERE
        //AUTOMATICALLY POPULATE DATAGRID WITH INFORMATION AS WE RECEIVE IT SUCH AS AVEAGE TIMES
        /*public static void Average()
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

            Excel.Range used = rr_sheet.UsedRange;
            foreach (Excel.Range row in used.Rows)
            {
                //find the total values for each of the above variables
                //throughput++;
                avg_totaltime += rr_sheet.Cells[row.Row, 2].Value2;
                avg_turnaround += rr_sheet.Cells[row.Row, 3].Value2;
                avg_wait += rr_sheet.Cells[row.Row, 4].Value2;
                avg_response += rr_sheet.Cells[row.Row, 5].Value2;
                avg_contextSwitch += rr_sheet.Cells[row.Row, 6].Value2;
                avg_utilization += rr_sheet.Cells[row.Row, 7].Value2;
                avg_speedUp += rr_sheet.Cells[row.Row, 8].Value2;
            }

            //divide the totals by the number of processes for the final results
            int num = 10;
            throughput = throughput / num;
            avg_totaltime = avg_totaltime / num;
            avg_turnaround = avg_turnaround / num;
            avg_wait = avg_wait / num;
            avg_response = avg_response / num;
            avg_contextSwitch = avg_contextSwitch / num;
            avg_utilization = avg_utilization / num;
            avg_speedUp = avg_speedUp / num;

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
           
        }*/
        private void CloseExcel()
        {
            my_book.Close(true, null, null);
            my_excel.Quit();
        }

        private void ResultsBtn_Click(object sender, RoutedEventArgs e)
        {
            //open the excel spreadsheet that contains the rest of the data
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            //increase the value of NumTxt by 1
            NumTxt.Text = (Convert.ToInt32(NumTxt.Text) + 1).ToString();
        }

        private void SubBtn_Click(object sender, RoutedEventArgs e)
        {
            //decrease the value of NumTxt by 1
            NumTxt.Text = (Convert.ToInt32(NumTxt.Text) - 1).ToString();
        }

        private void RunBtn_Click(object sender, RoutedEventArgs e)
        {
            // button becomes inactive when clicked
            RunBtn.IsEnabled = false;
            AddBtn.IsEnabled = false;
            SubBtn.IsEnabled = false;

            DictGen gen = new DictGen();

            SPN2 spn = new SPN2();
            spn.job_dict = gen.makeDict(15);
            spn.run();

            //FCFS fcfs = new FCFS();
            //fcfs.Run(gen.makeDict(100));

            //RoundRobin RoundRob = new RoundRobin();
            //RoundRob.job_dict = gen.makeDict(100);
            //RoundRob.Run(20);
        }
    }
}
