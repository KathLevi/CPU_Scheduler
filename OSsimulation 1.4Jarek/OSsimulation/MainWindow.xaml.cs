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

namespace OSsimulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

       
        public MainWindow()
        {
            InitializeComponent();
        }


        /**Button action that runs through first the FCFS algorithm
         * Then it runs through the SPN algorithm*/
        public async void OnClick(object sender, RoutedEventArgs e)
        {
            DictGen gen = new DictGen();

            //FCFS fcfs = new FCFS();
            //fcfs.Run(gen.makeDict(100));

            //RoundRobin RoundRob = new RoundRobin();
            //RoundRob.job_dict = gen.makeDict(100);
            //RoundRob.Run(20);

            MFQ mg = new MFQ();
            mg.distribute(gen.makeDict(20));



            
        }
    }
}
