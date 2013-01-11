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
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace Inhuman
{
    public partial class HelpPage : PhoneApplicationPage
    {
        //===================================================================================================================================================//
        public HelpPage()
        {
            InitializeComponent();
        }

        //===================================================================================================================================================//
        void HomeImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NodeController.PreviousPage();
        }

        //===================================================================================================================================================//
        void PageTitle_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            PageTitle.SelectAll();
        }
    }
}
