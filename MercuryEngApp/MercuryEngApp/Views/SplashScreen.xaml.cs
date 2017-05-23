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
using System.Windows.Shapes;
using Windows.Storage;

namespace MercuryEngApp.Views
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
/// <seealso cref="Windows.UI.Xaml.Controls.Page" />
    public partial class SplashScreen : Window
    {
         #region Constatnts

        /// <summary>
        /// The wait to display splash
        /// </summary>
        private const int WAIT_TO_DISPLAY_SPLASH = 4500;
        /// <summary>
        /// The na tech identifier
        /// </summary>
        private const int NA_TECH_ID = 2;
        /// <summary>
        /// The database folder name
        /// </summary>
        private const string DATABASE_FOLDER_NAME = "Database";
        /// <summary>
        /// The database file name
        /// </summary>
        private const string DATABASE_FILE_NAME = "Mercury.sqlite";

        private const int VALUE_8 = 8;
        private const int VALUE_1 = 1;

        #endregion
        /// <summary>
        /// Initializes a new instance of the <see cref="SpashScreen"/> class.
        /// </summary>
        public SplashScreen()
        {
            this.InitializeComponent();
            Loaded+=SplashScreenLoaded;
        }

        /// <summary>
        /// Spashes the screen loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void SplashScreenLoaded(object sender, RoutedEventArgs e)
        {
            //await Task.Delay(9000);  
            //this.Close();
        }

        ///// <summary>
        ///// Decides the home page.
        ///// </summary>
        //private void DecideHomePage()
        //{
        //    MainViewModel loginViewModel = new MainViewModel();
        //    MainWindow login = loginViewModel.GetMainWindowByUserID(NA_TECH_ID);
        //    if (login==null)
        //    {
               
        //    }
        //    else
        //    {
                
        //    }
        //}
       
        /// <summary>
        /// Copy the database
        /// </summary>
        /// <returns>Task</returns>
     
        /// <summary>
        /// Load the exam vessels for each exam procedure
        /// </summary>
       
    }
}
