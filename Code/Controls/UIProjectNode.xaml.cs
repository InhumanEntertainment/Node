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
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace Inhuman
{
    public partial class UIProjectNode : UIControl
    {
        //===================================================================================================================================================//
        public UIProjectNode()
        {
            InitializeComponent();             
        }

        //===================================================================================================================================================//
        public void Initialize()
        {
            (DataContext as ProjectNode).CalculateProgress();
        }

        //===================================================================================================================================================//
        public override void SetHitTest(bool value)
        {
            NodeObject.IsHitTestVisible = value;
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
