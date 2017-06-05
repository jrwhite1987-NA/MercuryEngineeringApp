using MercuryEngApp.ViewModels;
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

namespace MercuryEngApp
{
    /// <summary>
    /// Interaction logic for CalibrationTabUserControl.xaml
    /// </summary>
    public partial class CalibrationUserControl : UserControl
    {
        CalibrationViewModel calViewModel;
        public CalibrationUserControl()
        {
            calViewModel = new CalibrationViewModel();
            InitializeComponent();
            this.DataContext = calViewModel;
        }

        private void OverrideCalChecked(object sender, RoutedEventArgs e)
        {
            
        }

        private void OverrideCalibrationClick(object sender, RoutedEventArgs e)
        {

        }

        private void StartMeasurement1Click(object sender, RoutedEventArgs e)
        {

        }

        private void StartMeasurement2Click(object sender, RoutedEventArgs e)
        {

        }

        private void ApplyMeasurementsClick(object sender, RoutedEventArgs e)
        {

        }

        private void ConsistencyCheckStartClick(object sender, RoutedEventArgs e)
        {

        }

        private void ConsistencyCheckStopClick(object sender, RoutedEventArgs e)
        {

        }

        private void SafetyStartClick(object sender, RoutedEventArgs e)
        {

        }

        private void SafetyStopClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
