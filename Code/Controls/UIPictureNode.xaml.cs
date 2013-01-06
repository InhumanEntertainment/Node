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
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace Inhuman
{
    public partial class UIPictureNode : UserControl
    {
        //===================================================================================================================================================//
        public UIPictureNode()
        {
            InitializeComponent();
        }

        //===================================================================================================================================================//
        public void Initialize()
        {
            NodeObject.PlayAnim();
        }

        //===================================================================================================================================================//
        void NameText_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
        	//NameText.SelectAll();
        }
        
        //===================================================================================================================================================//
        void ActionButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Pages/PicturePage.xaml?Node=" + (DataContext as PictureNode).Id, UriKind.Relative));     
        }

        //===================================================================================================================================================//
        void NodeImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Pages/PicturePage.xaml?Node=" + (DataContext as PictureNode).Id, UriKind.Relative));     
        }
    }
}
