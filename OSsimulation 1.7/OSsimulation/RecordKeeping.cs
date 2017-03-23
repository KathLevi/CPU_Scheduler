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
        public static Excel._Worksheet rr_sheet = my_book.Sheets[1];
        public static Excel._Worksheet fcfs_sheet = my_book.Sheets[2];
        public static Excel._Worksheet srt_sheet = my_book.Sheets[3];
        public static Excel._Worksheet spn_sheet = my_book.Sheets[4];
        public static Excel._Worksheet mfq_sheet = my_book.Sheets[5];

        public static void UpdateExcel_RR(int processNum, double totaltime,
            double turnaround, double wait, double response, double contextswitch,
            double processorUtil, double speedUp)
        {
            //add 2 to the process num to place in the correct position on the excel sheet
            int i = processNum + 2;
            //add in all of the correct values to the excel sheet
            rr_sheet.Cells[i, 1] = processNum;
            rr_sheet.Cells[i, 2] = totaltime;
            rr_sheet.Cells[i, 3] = turnaround;
            rr_sheet.Cells[i, 4] = wait;
            rr_sheet.Cells[i, 5] = response;
            rr_sheet.Cells[i, 6] = contextswitch;
            rr_sheet.Cells[i, 7] = processorUtil;
            rr_sheet.Cells[i, 8] = speedUp;

            //closing the excel sheet saves and closes the book and application, do we need this every update or only at the end
            //closeExcel();
        }
        public static void UpdateExcel_FCFS(int processNum, double totaltime,
            double turnaround, double wait, double response, double contextswitch,
            double processorUtil, double speedUp)
        {
            //add 2 to the process num to place in the correct position on the excel sheet
            int i = processNum + 2;
            //add in all of the correct values to the excel sheet
            fcfs_sheet.Cells[i, 1] = processNum;
            fcfs_sheet.Cells[i, 2] = totaltime;
            fcfs_sheet.Cells[i, 3] = turnaround;
            fcfs_sheet.Cells[i, 4] = wait;
            fcfs_sheet.Cells[i, 5] = response;
            fcfs_sheet.Cells[i, 6] = contextswitch;
            fcfs_sheet.Cells[i, 7] = processorUtil;
            fcfs_sheet.Cells[i, 8] = speedUp;
        }
        public static void UpdateExcel_SRT(int processNum, double totaltime,
            double turnaround, double wait, double response, double contextswitch,
            double processorUtil, double speedUp)
        {
            //add 2 to the process num to place in the correct position on the excel sheet
            int i = processNum + 2;
            //add in all of the correct values to the excel sheet
            srt_sheet.Cells[i, 1] = processNum;
            srt_sheet.Cells[i, 2] = totaltime;
            srt_sheet.Cells[i, 3] = turnaround;
            srt_sheet.Cells[i, 4] = wait;
            srt_sheet.Cells[i, 5] = response;
            srt_sheet.Cells[i, 6] = contextswitch;
            srt_sheet.Cells[i, 7] = processorUtil;
            srt_sheet.Cells[i, 8] = speedUp;
        }
        public static void UpdateExcel_SPN(int processNum, double totaltime,
            double turnaround, double wait, double response, double contextswitch,
            double processorUtil, double speedUp)
        {
            //add 2 to the process num to place in the correct position on the excel sheet
            int i = processNum + 2;
            //add in all of the correct values to the excel sheet
            spn_sheet.Cells[i, 1] = processNum;
            spn_sheet.Cells[i, 2] = totaltime;
            spn_sheet.Cells[i, 3] = turnaround;
            spn_sheet.Cells[i, 4] = wait;
            spn_sheet.Cells[i, 5] = response;
            spn_sheet.Cells[i, 6] = contextswitch;
            spn_sheet.Cells[i, 7] = processorUtil;
            spn_sheet.Cells[i, 8] = speedUp;
        }
        public static void UpdateExcel_MFQ(int processNum, double totaltime,
            double turnaround, double wait, double response, double contextswitch,
            double processorUtil, double speedUp)
        {
            //add 2 to the process num to place in the correct position on the excel sheet
            int i = processNum + 2;
            //add in all of the correct values to the excel sheet
            mfq_sheet.Cells[i, 1] = processNum;
            mfq_sheet.Cells[i, 2] = totaltime;
            mfq_sheet.Cells[i, 3] = turnaround;
            mfq_sheet.Cells[i, 4] = wait;
            mfq_sheet.Cells[i, 5] = response;
            mfq_sheet.Cells[i, 6] = contextswitch;
            mfq_sheet.Cells[i, 7] = processorUtil;
            mfq_sheet.Cells[i, 8] = speedUp;
        }

        public static void closeExcel()
        {
            try
            {
                my_book.Close(true);
                
            }
            finally
            {
                if (my_excel != null) { my_excel.Quit(); }
            }
        }

    }
}
