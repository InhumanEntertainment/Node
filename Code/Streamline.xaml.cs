using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Inhuman
{
    public partial class Streamline : Application
    {
        public static Streamline Instance;
        public PhoneApplicationFrame RootFrame { get; private set; }

        //===================================================================================================================================================//
        public Streamline()
        {
            UnhandledException += Application_UnhandledException;
            InitializeComponent();
            InitializePhoneApplication();
            if (System.Diagnostics.Debugger.IsAttached)
            {
                Application.Current.Host.Settings.EnableFrameRateCounter = true;
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

            // Streamline //
            Instance = this;            
        }

        //===================================================================================================================================================//
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            NodeController.Data = StreamlineData.Load();

            if (NodeController.Data.Nodes.Count > 0)
            {
                NodeController.Data.CurrentPage = NodeController.Data.Nodes[0].Id;
            }
        }

        //===================================================================================================================================================//
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            NodeController.Data = StreamlineData.Load();           
        }

        //===================================================================================================================================================//
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            NodeController.Data.Save();
        }

        //===================================================================================================================================================//
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            NodeController.Data.Save();
        }

        //===================================================================================================================================================//
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        //===================================================================================================================================================//
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}