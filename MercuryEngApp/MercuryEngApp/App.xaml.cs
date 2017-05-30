using MercuryEngApp.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using UsbTcdLibrary;
using MercuryEngApp.Views;

namespace MercuryEngApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// The active channels
        /// </summary>
        private static ActiveChannels activeChannels;

        /// <summary>
        /// Gets or sets the active channels.
        /// </summary>
        /// <value>The active channels.</value>
        internal static ActiveChannels ActiveChannels
        {
            get
            {
                return activeChannels;
            }
            set
            {
                if (activeChannels != value)
                {
                    activeChannels = value;
                    switch (activeChannels)
                    {
                        case ActiveChannels.Channel1:
                            CurrentChannel = TCDHandles.Channel1;
                            break;
                        case ActiveChannels.Channel2:
                            CurrentChannel = TCDHandles.Channel2;
                            break;
                        case ActiveChannels.None:
                            CurrentChannel = TCDHandles.None;
                            break;
                        case ActiveChannels.NoTCD:
                            CurrentChannel = TCDHandles.None;
                            break;
                        case ActiveChannels.Both:
                            CurrentChannel = TCDHandles.Channel1;
                            break;
                    }

                }
            }
        }

        internal static TCDHandles CurrentChannel
        {
            get;
            set;
        }
        internal static MainWindow mainWindow;
        public App()
        {

        }

        public static MainViewModel MainViewModel { get; set; }

        public static string TCDLog { get; set; }

        public static string ApplicationLog { get; set; }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            MercuryEngApp.Views.SplashScreen splashScreen = new MercuryEngApp.Views.SplashScreen();
            mainWindow = new MainWindow();
            mainWindow.Show();
            splashScreen.Show();
            await Task.Delay(4500);
            mainWindow.Activate();
            splashScreen.Close();       
        }
    }
}
