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
using Core.Constants;

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

        public static string ApplicationLog
        {
            get
            {
                return mainWindow.mainViewModel.ApplicationLog;
            }
            set
            {
                mainWindow.mainViewModel.ApplicationLog = value+'\n';
            }
        }

        public static string TCDLog
        {
            get
            {
                return mainWindow.mainViewModel.TCDLog;
            }
            set
            {
                mainWindow.mainViewModel.TCDLog = value+'\n';
            }
        }

        private async void ApplicationStartup(object sender, StartupEventArgs e)
        {
            MercuryEngApp.Views.SplashScreen splashScreen = new MercuryEngApp.Views.SplashScreen();
            mainWindow = new MainWindow();
            mainWindow.Show();
            splashScreen.Show();
            await Task.Delay(Constants.VALUE_4500);
            mainWindow.Activate();
            splashScreen.Close();       
        }
    }
}
