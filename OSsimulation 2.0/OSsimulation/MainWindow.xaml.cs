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

        private void CloseExcel()
        {
            my_book.Close(true, null, null);
            my_excel.Quit();
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

            Run_Algs();

            RunBtn.IsEnabled = true;
            AddBtn.IsEnabled = true;
            SubBtn.IsEnabled = true;
        }

        private void Run_Algs()
        {
            //run algorithms
            DictGen gen = new DictGen();
            int num = Convert.ToInt32(NumTxt.Text);

            SRT srt = new SRT();
            srt.job_dict = gen.makeDict(num);
            srt.run();

            SPN spn = new SPN();
            spn.job_dict = gen.makeDict(num);
            spn.run();

            MFQ mfq = new MFQ();
            mfq.distribute(gen.makeDict(num));
            mfq.thread_run();

            FCFS fcfs = new FCFS();
            fcfs.Run(gen.makeDict(num));

            RoundRobin RoundRob = new RoundRobin();
            RoundRob.job_dict = gen.makeDict(num);
            RoundRob.Run(20);

            //Update data averages
            updateupdate(num);
            
            //close and save the excel file
            RecordKeeping.closeExcel();
        }

        private void updateupdate(int num)
        {
            //update algorithm averages
            rr_tt.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[2, 2] as Excel.Range).Value)) / num;
            rr_ta.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[2, 3] as Excel.Range).Value)) / num;
            rr_wait.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[2, 4] as Excel.Range).Value)) / num;
            rr_resp.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[2, 4] as Excel.Range).Value)) / num;
            rr_cs.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[2, 6] as Excel.Range).Value)) / num;
            rr_pu.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[2, 7] as Excel.Range).Value)) / num;
            rr_su.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[2, 8] as Excel.Range).Value)) / num;

            fcfs_tt.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[3, 2] as Excel.Range).Value)) / num;
            fcfs_ta.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[3, 3] as Excel.Range).Value)) / num;
            fcfs_wait.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[3, 4] as Excel.Range).Value)) / num;
            fcfs_resp.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[3, 5] as Excel.Range).Value)) / num;
            fcfs_cs.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[3, 6] as Excel.Range).Value)) / num;
            fcfs_pu.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[3, 7] as Excel.Range).Value)) / num;
            fcfs_su.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[3, 8] as Excel.Range).Value)) / num;

            srt_tt.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[4, 2] as Excel.Range).Value)) / num;
            srt_ta.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[4, 3] as Excel.Range).Value)) / num;
            srt_wait.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[4, 4] as Excel.Range).Value)) / num;
            srt_resp.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[4, 5] as Excel.Range).Value)) / num;
            srt_cs.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[4, 6] as Excel.Range).Value)) / num;
            srt_pu.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[4, 7] as Excel.Range).Value)) / num;
            srt_su.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[4, 8] as Excel.Range).Value)) / num;

            spn_tt.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[5, 2] as Excel.Range).Value)) / num;
            spn_ta.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[5, 3] as Excel.Range).Value)) / num;
            spn_wait.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[5, 4] as Excel.Range).Value)) / num;
            spn_resp.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[5, 5] as Excel.Range).Value)) / num;
            spn_cs.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[5, 6] as Excel.Range).Value)) / num;
            spn_pu.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[5, 7] as Excel.Range).Value)) / num;
            spn_su.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[5, 8] as Excel.Range).Value)) / num;

            mfq_tt.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[6, 2] as Excel.Range).Value)) / num;
            mfq_ta.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[6, 3] as Excel.Range).Value)) / num;
            mfq_wait.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[6, 4] as Excel.Range).Value)) / num;
            mfq_resp.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[6, 5] as Excel.Range).Value)) / num;
            mfq_cs.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[6, 6] as Excel.Range).Value)) / num;
            mfq_pu.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[6, 7] as Excel.Range).Value)) / num;
            mfq_su.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[6, 8] as Excel.Range).Value)) / num;

            //update total averages
            tt.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[7, 2] as Excel.Range).Value))/num;
            ta.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[7, 3] as Excel.Range).Value))/num;
            wait.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[7, 4] as Excel.Range).Value)) / num;
            resp.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[7, 5] as Excel.Range).Value)) / num;
            cs.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[7, 6] as Excel.Range).Value)) / num;
            pu.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[7, 7] as Excel.Range).Value)) / num;
            su.Content = (Convert.ToInt32((RecordKeeping.avg_sheet.Cells[7, 8] as Excel.Range).Value)) / num;
        }

    }
}
