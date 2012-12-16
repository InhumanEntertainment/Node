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

namespace Inhuman
{
    public partial class UINode : UserControl
    {
        TranslateTransform Offset = new TranslateTransform();

        SolidColorBrush DefaultRoot = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
        //SolidColorBrush DeleteRoot = new SolidColorBrush(Color.FromArgb(255, 150, 0, 0));
        SolidColorBrush DeleteRoot = Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;

        //===================================================================================================================================================//
        public UINode()
        {
            InitializeComponent();

            LayoutRoot.RenderTransform = Offset;
        }

        //===================================================================================================================================================//
        void NameText_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
        	NameText.SelectAll();
        }

        //===================================================================================================================================================//
        void RemoveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainPage.Instance.MainListBox.Items.Remove(this);
            Streamline.Data.CurrentPageNode.Nodes.Remove((this.DataContext as Node).Id);
        }

        //===================================================================================================================================================//
        Point StartPos;
        void UserControl_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            StartPos = e.ManipulationOrigin;
        }

        //===================================================================================================================================================//
        void UserControl_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            Offset.X = Math.Max(0, e.ManipulationOrigin.X - StartPos.X);
            //Offset.Y = e.ManipulationOrigin.Y - StartPos.Y;

            if (Offset.X > 200)
            {
                RootControl.Background = DeleteRoot;
            }
            else
            {
                RootControl.Background = DefaultRoot;
            }
        }

        //===================================================================================================================================================//
        void RootControl_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            if (Offset.X > 200)
            {
                MainPage.Instance.MainListBox.Items.Remove(this);
                Streamline.Data.CurrentPageNode.Nodes.Remove((this.DataContext as Node).Id);
            }
            else
            {
                Offset.X = 0;
            }
        }
    }
}
