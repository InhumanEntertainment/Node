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

namespace Inhuman
{
    public partial class UIHeaderNode : UserControl
    {
        CompositeTransform Offset = new CompositeTransform();

        SolidColorBrush DefaultBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
        SolidColorBrush DeleteBrush = new SolidColorBrush(Color.FromArgb(255, 100, 0, 0));
        SolidColorBrush AddBrush = Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
        //SolidColorBrush AddBrush = new SolidColorBrush(Color.FromArgb(255, 0, 100, 0));

        FrameworkElement RootUIControl;
        public bool EditOnCreate = false;
    
        //===================================================================================================================================================//
        public UIHeaderNode()
        {
            InitializeComponent();
            LayoutRoot.RenderTransform = Offset;
            Deleted.Storyboard.Completed += new EventHandler(Deleted_Completed);
        }

        //===================================================================================================================================================//
        void Deleted_Completed(object sender, EventArgs e)
        {           
            if (RootUIControl != null)
            {
                Node node = (Node)(DataContext as Node);
                NodeController.CurrentPageNode.Nodes.Remove(node.Id);
                NodeController.UI.MainListBox.Items.Remove(RootUIControl);               
            }           
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
            // Find Root Control //
            if (NodeController.UI.MainListBox.Items.Contains(this))
                RootUIControl = (FrameworkElement)this;
            else
                RootUIControl = (Parent as FrameworkElement).Parent as FrameworkElement;

            StartPos = e.ManipulationOrigin;
            index = NodeController.UI.MainListBox.Items.IndexOf(RootUIControl);               
        }

        //===================================================================================================================================================//
        void UserControl_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            //Offset.X = Math.Max(0, e.ManipulationOrigin.X - StartPos.X);
            Offset.TranslateX = e.ManipulationOrigin.X - StartPos.X;
            //Offset.Y = e.ManipulationOrigin.Y - StartPos.Y;

            if (Offset.TranslateX < -160)
            {
                RootControl.Background = AddBrush;
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
                Node node;
                UserControl uinode;
                if (NodeController.CurrentPageNode is ProjectNode)
                {
                    // Create Node //
                    node = new TaskNode();
                    uinode = NodeController.GetUITaskNode();
                    node.Name = "Task";
                }
                else
                {
                    // Create Node //
                    node = new Node("Node");
                    uinode = new UINode();
                }				              

                // Get Position //
                NodeController.UI.MainListBox.UpdateLayout();
                NodeController.UI.MainListBox.Items.Insert(index, uinode);
                uinode.DataContext = node;

                NodeController.Data.Nodes.Add(node);
                NodeController.CurrentPageNode.Nodes.Insert(index, node.Id);

                Offset.TranslateX = 0;
				RootControl.Background = null;
            }
            else
            {
                Offset.TranslateX = 0;
				RootControl.Background = null;
                //Reset.Storyboard.Begin();

                //VisualStateManager.GoToState(this, "Reset", true);
            }
        }

        //===================================================================================================================================================//
        void MoveUp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	int index = NodeController.UI.MainListBox.Items.IndexOf(RootUIControl);

            if (index > 0)
            {
                // Move UI //
                object temp = NodeController.UI.MainListBox.Items[index];
                NodeController.UI.MainListBox.Items.RemoveAt(index);
                NodeController.UI.MainListBox.Items.Insert(index - 1, temp);

                // Move Data //
                string node = NodeController.CurrentPageNode.Nodes[index];
                NodeController.CurrentPageNode.Nodes.RemoveAt(index);
                NodeController.CurrentPageNode.Nodes.Insert(index - 1, node);
            }
        }
		
		//===================================================================================================================================================//
        void MoveDown_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int index = NodeController.UI.MainListBox.Items.IndexOf(RootUIControl);

            if (index < NodeController.UI.MainListBox.Items.Count - 1)
            {
                // Move UI //
                object temp = NodeController.UI.MainListBox.Items[index + 1];
                NodeController.UI.MainListBox.Items.RemoveAt(index + 1);
                NodeController.UI.MainListBox.Items.Insert(index, temp);

                // Move Data //
                string node = NodeController.CurrentPageNode.Nodes[index + 1];
                NodeController.CurrentPageNode.Nodes.RemoveAt(index + 1);
                NodeController.CurrentPageNode.Nodes.Insert(index, node);
            }
        }

        //===================================================================================================================================================//
        void Node_Loaded(object sender, RoutedEventArgs e)
        {
            if (EditOnCreate)
            {
                NameText.Focus();
            }
        }

        //===================================================================================================================================================//
        void Node_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
        	e.Handled = true;
        }
    }
}
