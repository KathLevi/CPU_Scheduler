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
        public async void OnClick(object sender, RoutedEventArgs e)
        {
            FCFS fcfs = new FCFS(10000);
            Stopwatch stop = new Stopwatch();
            stop.Start();
            fcfs.Run();
            stop.Stop();
            string ellapsed = stop.Elapsed.TotalSeconds.ToString();
            Text_block.Text = ellapsed;

            await Task.Delay(2000);

            SPN spn = new SPN(10000);
            stop.Reset();
            stop.Start();
            spn.run();
            stop.Stop();
            ellapsed = stop.Elapsed.TotalSeconds.ToString();
            Text_block.Text = ellapsed;
        }
    }
}
