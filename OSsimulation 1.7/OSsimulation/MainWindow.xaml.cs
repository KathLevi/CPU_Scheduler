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
            int num = Convert.ToInt32(NumTxt.Text);
            SPN spn = new SPN();
            spn.job_dict = gen.makeDict(num);
            spn.run();

            //MFQ mfq = new MFQ();
            //mfq.distribute(gen.makeDict(100));
            //mfq.thread_run();

            FCFS fcfs = new FCFS();
            fcfs.Run(gen.makeDict(num));

            RoundRobin RoundRob = new RoundRobin();
            RoundRob.job_dict = gen.makeDict(num);
            RoundRob.Run(20);
            RecordKeeping.closeExcel();
        }
    }
}
