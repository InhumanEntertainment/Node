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
using System.Windows.Media.Imaging;

namespace Inhuman
{
    public partial class UILinkNode : UserControl
    {
        BitmapImage PageImage;
        BitmapImage NodeImage;
        BitmapImage PageActionImage;
        BitmapImage NodeActionImage;

        //===================================================================================================================================================//
        public UILinkNode()
        {
            InitializeComponent();

            PageImage = new BitmapImage(new Uri("/Node;component/Art/Page.png", UriKind.Relative));
            NodeImage = new BitmapImage(new Uri("/Node;component/Art/Task.png", UriKind.Relative));
            PageActionImage = new BitmapImage(new Uri("/Node;component/Art/Arrow.png", UriKind.Relative));
            NodeActionImage = new BitmapImage(new Uri("/Node;component/Art/Dot.png", UriKind.Relative));    
        }

        //===================================================================================================================================================//
        public void Initialize()
        {
            PageNode node = (DataContext as PageNode);
            
            Button actionButton = (NodeObject.ButtonContent as Button);
            Image actionImage = actionButton.Content as Image;
            Image typeImage = NodeObject.IconContent as Image;

            if (node.Nodes.Count > 0)
            {
                actionImage.Source = PageActionImage;
                typeImage.Source = PageImage;
                node.Info = "Page - " + node.Nodes.Count + " Nodes";
            }
            else
            {
                actionImage.Source = NodeActionImage;
                typeImage.Source = NodeImage;
                node.Info = "";
            }               
        }

        //===================================================================================================================================================//
        public void Edit()
        {
            NodeObject.NameText.Focus();
        }

        //===================================================================================================================================================//
        void OpenButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PageNode page = (PageNode)DataContext;

            string param = "Page=" + page.Id;
            NodeController.UI.NavigationService.Navigate(new Uri("/MainPage.xaml?" + param, UriKind.Relative));
        }
    }
}
