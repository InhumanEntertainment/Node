using System;
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
using System.Diagnostics;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace Inhuman
{
    public partial class UIInstanceNode : UserControl
    {	
        //===================================================================================================================================================//
        public UIInstanceNode()
        {
            InitializeComponent();
        }

        //===================================================================================================================================================//
        void NameText_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
        	NameText.SelectAll();
        }

        //===================================================================================================================================================//
        void ActionButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string param = "Page=" + (DataContext as Node).Id;
            NodeController.UI.NavigationService.Navigate(new Uri("/MainPage.xaml?" + param, UriKind.Relative));
        }
    }
}
