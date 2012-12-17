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
        CompositeTransform Offset = new CompositeTransform();

        SolidColorBrush DefaultBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
        SolidColorBrush DeleteBrush = new SolidColorBrush(Color.FromArgb(255, 100, 0, 0));
        //SolidColorBrush AddBrush = Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
        SolidColorBrush AddBrush = new SolidColorBrush(Color.FromArgb(255, 0, 100, 0));
        
        //===================================================================================================================================================//
        public UINode()
        {
            InitializeComponent();
           
            Create.Storyboard.Begin();
            LayoutRoot.RenderTransform = Offset;

            Deleted.Storyboard.Completed += new EventHandler(Deleted_Completed);
            //Added.Storyboard.Completed += new EventHandler(Added_Completed);
            //Reset.Storyboard.Completed += new EventHandler(Reset_Completed);
        }

        //===================================================================================================================================================//
        void Added_Completed(object sender, EventArgs e)
        {
            

        }

        //===================================================================================================================================================//
        void Deleted_Completed(object sender, EventArgs e)
        {
            MainPage.Instance.MainListBox.Items.Remove(this);
            Streamline.Data.CurrentPageNode.Nodes.Remove((this.DataContext as Node).Id);
        }

        //===================================================================================================================================================//
        void Reset_Completed(object sender, EventArgs e)
        {
            //Reset.Storyboard.Stop();
            //VisualStateManager.GoToState(this, "Base", true);
            //LayoutRoot.RenderTransform = Offset;

        }
		
		//===================================================================================================================================================//
        public void FocusName()
		{
            NameText.SelectAll();	
		}

        //===================================================================================================================================================//
        void NameText_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
        	NameText.SelectAll();
        }

        //===================================================================================================================================================//
        void RemoveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Deleted.Storyboard.Begin();
        }

        //===================================================================================================================================================//
        Point StartPos;
        void UserControl_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            StartPos = e.ManipulationOrigin;
            index = MainPage.Instance.MainListBox.Items.IndexOf(this);               
        }

        //===================================================================================================================================================//
        void UserControl_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            //Offset.X = Math.Max(0, e.ManipulationOrigin.X - StartPos.X);
            Offset.TranslateX = e.ManipulationOrigin.X - StartPos.X;
            //Offset.Y = e.ManipulationOrigin.Y - StartPos.Y;

            if (Offset.TranslateX < -160)
            {
                RootControl.Background = DeleteBrush;
            }
            else if (Offset.TranslateX > 160)
            {
                RootControl.Background = AddBrush;
            }
            else
            {
                RootControl.Background = DefaultBrush;
            }
        }

        //===================================================================================================================================================//
        int index;
                
        void RootControl_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            if (Offset.TranslateX < -160)
            {
                Deleted.Storyboard.Begin();

                
            }
            else if (Offset.TranslateX > 160)
            {              
                //Added.Storyboard.Begin();

                // Create Node //
                Node node = new Node("Node");
                UINode uinode = new UINode();

                // Get Position //
                MainPage.Instance.MainListBox.UpdateLayout();
                MainPage.Instance.MainListBox.Items.Insert(index, uinode);
                uinode.DataContext = node;

                Streamline.Data.Nodes.Add(node);
                Streamline.Data.CurrentPageNode.Nodes.Insert(index, node.Id);

                Offset.TranslateX = 0;
                uinode.FocusName();
            }
            else
            {
                Offset.TranslateX = 0;
                //Reset.Storyboard.Begin();

                //VisualStateManager.GoToState(this, "Reset", true);
            }
        }
    }
}
