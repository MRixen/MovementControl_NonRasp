using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WindowsFormsApplication6;

namespace Graph
{
    /// <summary>
    /// Interaction logic for ChartTypeLine.xaml
    /// </summary>
    public partial class ChartTypeLine : Window
    {
        //List<KeyValuePair<int, int>> testList;
        ObservableCollection<KeyValuePair<int, int>> testList, testListTemp;
        LineSeries mySeries;
        int counter;

        public ChartTypeLine(Object context, int sampleTimeFactor)
        {
            InitializeComponent();




            counter = 0;
        }

        private void initCharts()
        {
            testList = new ObservableCollection<KeyValuePair<int, int>>();
            testListTemp = new ObservableCollection<KeyValuePair<int, int>>();
            mySeries = new LineSeries();
            mcChart.Series.Add(mySeries);
            mySeries.IndependentValueBinding = new Binding("Key");
            mySeries.DependentValueBinding = new Binding("Value");
            mySeries.ItemsSource = testList;
            mySeries.Title = "X-Value";
        }


        private void Window_Closing(object sender, ContextMenuEventArgs e)
        {
            //formBaseContext.setCheckboxUnchecked_X = CheckState.Unchecked;
            //if (notifyIcon != null) notifyIcon.Dispose();

            //chartTimer.Stop();
            //chartTimer.Dispose();
            //chartsExitEventHandler();
        }

        public void addLineChartData(int key, int value)
        {
            if (testList.Count() >= 15)
            {
                testList.RemoveAt(0);
            }
            testList.Add(new KeyValuePair<int, int>(key, value));
            Debug.WriteLine(testList.Count());
        }


    }
}
