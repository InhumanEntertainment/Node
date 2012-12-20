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

namespace Inhuman
{
    public partial class UILinkNode : UserControl
    {
        //===================================================================================================================================================//
        public UILinkNode()
        {
            InitializeComponent();
        }

        //===================================================================================================================================================//
        void OpenButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// Get Page node from url //
            PageNode page = (PageNode)DataContext;// (PageNode)Streamline.Data.GetNode(((PageNode)DataContext).Url);

            // Switch Pages //
            Streamline.Data.CurrentPage = page.Id;
            MainPage.Instance.LoadPageNode(page);
        }

        //===================================================================================================================================================//
        void NameText_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
        	NameText.SelectAll();
        }
    }
}
