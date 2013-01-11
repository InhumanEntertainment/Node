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
    public partial class UIPictureNode : UIControl
    {
        //===================================================================================================================================================//
        public UIPictureNode()
        {
            InitializeComponent();
        }

        //===================================================================================================================================================//
        public override void Initialize(bool autoedit)
        {
            (Node as PictureNode).LoadBitmap();
        }

        //===================================================================================================================================================//
        public override void SetHitTest(bool value)
        {
            NodeObject.IsHitTestVisible = value;
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
