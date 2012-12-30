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
    public partial class UIProjectNode : UserControl
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
        void OpenButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// Get Page node from url //
            PageNode page = (PageNode)DataContext;

            // Switch Pages //
            NodeController.Data.CurrentPage = page.Id;
            NodeController.LoadPage(page);
        }
    }
}
