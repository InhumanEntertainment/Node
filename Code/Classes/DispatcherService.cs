using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Xna.Framework;


namespace Inhuman
{
    public class DispatcherService : IApplicationService
    {
        private DispatcherTimer DispatchTimer;

        //===================================================================================================================================================//
        public DispatcherService()
        {
            DispatchTimer = new DispatcherTimer();
            DispatchTimer.Interval = TimeSpan.FromMilliseconds(50);           
            DispatchTimer.Tick += DispatcherTimer_Tick;
            FrameworkDispatcher.Update();
        }

        //===================================================================================================================================================//
        void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            FrameworkDispatcher.Update();
        }

        //===================================================================================================================================================//
        void IApplicationService.StartService(ApplicationServiceContext context)
        {
            DispatchTimer.Start();
        }

        //===================================================================================================================================================//
        void IApplicationService.StopService()
        {
            DispatchTimer.Stop();
        }
    }
}