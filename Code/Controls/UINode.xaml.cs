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
    [ContentProperty("LowerContent")]
    public partial class UINode : UserControl
    {
		// Button Content //
		public static readonly DependencyProperty ButtonContentProperty = DependencyProperty.Register("ButtonContent", typeof(object), typeof(UINode), new PropertyMetadata((object)null));
        public object ButtonContent
        {
            get { return (object)GetValue(ButtonContentProperty); }
            set { SetValue(ButtonContentProperty, value); }
        }
		
		// Icon Content //
		public static readonly DependencyProperty IconContentProperty = DependencyProperty.Register("IconContent", typeof(object), typeof(UINode), new PropertyMetadata((object)null));
        public object IconContent
        {
            get { return (object)GetValue(IconContentProperty); }
            set { SetValue(IconContentProperty, value); }
        }
		
		// LowerContent //
		public static readonly DependencyProperty LowerContentProperty = DependencyProperty.Register("LowerContent", typeof(object), typeof(UINode), new PropertyMetadata((object)null));
        public object LowerContent
        {
            get { return (object)GetValue(LowerContentProperty); }
            set { SetValue(LowerContentProperty, value); }
        }
		
		
		
        CompositeTransform Offset = new CompositeTransform();

        SolidColorBrush DefaultBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
        SolidColorBrush DeleteBrush = new SolidColorBrush(Color.FromArgb(255, 100, 0, 0));
        SolidColorBrush AddBrush = Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
        //SolidColorBrush AddBrush = new SolidColorBrush(Color.FromArgb(255, 0, 100, 0));

        FrameworkElement RootUIControl;
        public bool EditOnCreate = false;
		
        //===================================================================================================================================================//
        public UINode()
        {
            InitializeComponent();

            LayoutRoot.RenderTransform = Offset;

            Deleted.Storyboard.Completed += new EventHandler(Deleted_Completed);
            //Added.Storyboard.Completed += new EventHandler(Added_Completed);
            //Reset.Storyboard.Completed += new EventHandler(Reset_Completed);
        }
		
        //===================================================================================================================================================//
        public void PlayAnim()
        {
            Create.Storyboard.Begin();
        }

        //===================================================================================================================================================//
        void Deleted_Completed(object sender, EventArgs e)
        {           
                
        }

        public void Delete()
        {
            if (RootUIControl != null)
            {
                Node node = (Node)(DataContext as Node);
                NodeController.CurrentPageNode.Nodes.Remove(node.Id);
                NodeController.UI.MainListBox.Items.Remove(RootUIControl);

                if (node.Created == node.Updated)
                {
                    NodeController.DeleteNode(node, false);
                }

                Offset.TranslateX = 0;
            }       
        }

        //===================================================================================================================================================//
        void Reset_Completed(object sender, EventArgs e)
        {
            //Reset.Storyboard.Stop();
            //VisualStateManager.GoToState(this, "Base", true);
            //LayoutRoot.RenderTransform = Offset;

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

            var tx = e.ManipulationContainer.TransformToVisual(this);
            StartPos = tx.Transform(e.ManipulationOrigin);

            NodeIndex = NodeController.UI.MainListBox.Items.IndexOf(RootUIControl);

            NodeImage.Visibility = System.Windows.Visibility.Visible;
            TaskImage.Visibility = System.Windows.Visibility.Visible;
            PictureImage.Visibility = System.Windows.Visibility.Visible;
            AudioImage.Visibility = System.Windows.Visibility.Visible;
            RemoveImage.Visibility = System.Windows.Visibility.Visible;
            DeleteImage.Visibility = System.Windows.Visibility.Visible;
        }

        //===================================================================================================================================================//
        void UserControl_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            //Offset.X = Math.Max(0, e.ManipulationOrigin.X - StartPos.X);
            Offset.TranslateX = e.ManipulationOrigin.X - StartPos.X;
            //Offset.Y = e.ManipulationOrigin.Y - StartPos.Y;

            Offset.TranslateX = Math.Min(Offset.TranslateX, 320);
            Offset.TranslateX = Math.Max(Offset.TranslateX, -160);

            // Move Selected //
            int step = Offset.TranslateX >= 0 ? 40 : -40;
            int index = (int)((Offset.TranslateX + step) / 80);

            /*if (Offset.TranslateX >= 0)
            {
                NodeImage.Opacity = index == 1 ? 1 : 0.15f;
                TaskImage.Opacity = index == 2 ? 1 : 0.15f;
                PictureImage.Opacity = index == 3 ? 1 : 0.15f;
                AudioImage.Opacity = index == 4 ? 1 : 0.15f;
                RemoveImage.Opacity = 0;
                DeleteImage.Opacity = 0; 
            }
            else
            {
                NodeImage.Opacity = 0;
                TaskImage.Opacity = 0;
                PictureImage.Opacity = 0;
                AudioImage.Opacity = 0;
                RemoveImage.Opacity = index == -1 ? 1 : 0.15f;
                DeleteImage.Opacity = index <= -2 ? 1 : 0.15f;               
            }*/


            NodeImage.Margin = new Thickness(Math.Max(0, Offset.TranslateX - 80), 0, 0, 0);
            TaskImage.Margin = new Thickness(Math.Max(0, Offset.TranslateX - 160), 0, 0, 0);
            PictureImage.Margin = new Thickness(Math.Max(0, Offset.TranslateX - 240), 0, 0, 0);
            AudioImage.Margin = new Thickness(Math.Max(0, Offset.TranslateX - 320), 0, 0, 0);

            RemoveImage.Margin = new Thickness(0, 0, Math.Max(0, -Offset.TranslateX - 80), 0);
            DeleteImage.Margin = new Thickness(0, 0, Math.Max(0, -Offset.TranslateX - 160), 0);

            /*if (Offset.TranslateX < -160)
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
            }*/
        }

        //===================================================================================================================================================//
        int NodeIndex;
                
        void RootControl_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            Offset.TranslateX = e.ManipulationOrigin.X - StartPos.X;

            int step = Offset.TranslateX >= 0 ? 40 : -40;
            int index = (int)((Offset.TranslateX + step) / 80);

            if (index == -1)
            {
                //Deleted.Storyboard.Begin();
                Delete();
            }
            else if (index <= -2)
            {
                //Deleted.Storyboard.Begin();
                Delete();

                // Delete Node //
                //MessageBox.Show("Delete Node", "Are you sure you want to delete this node and all of it's instances?", MessageBoxButton.OKCancel);
            }
            else if (index > 0)
            {
                Node node;
                UserControl uinode;
                if (index == 2)
                {
                    // Create Node //
                    node = new TaskNode();
                    uinode = NodeController.GetUITaskNode();
                    node.Name = "Task";
                    (uinode as UITaskNode).NodeObject.EditOnCreate = true;
                }
                else if (index == 10)
                {
                    // Create Node //
                    node = new PageNode();
                    uinode = NodeController.GetUIPageNode();
                    node.Name = "Node";
                    (uinode as UIPageNode).NodeObject.EditOnCreate = true;
                }
                else
                {
                    // Create Node //
                    node = new PageNode();
                    uinode = NodeController.GetUIPageNode();
                    node.Name = "Node";
                    (uinode as UIPageNode).NodeObject.EditOnCreate = true;
                }

                // Get Position //
                NodeController.UI.MainListBox.UpdateLayout();
                NodeController.UI.MainListBox.Items.Insert(NodeIndex, uinode);
                uinode.DataContext = node;

                if (uinode is UIPageNode)
                {
                    (uinode as UIPageNode).Initialize();
                }

                NodeController.Data.Nodes.Add(node);
                NodeController.CurrentPageNode.Nodes.Insert(NodeIndex, node.Id);
            }
            else
            {
                //Reset.Storyboard.Begin();
                //VisualStateManager.GoToState(this, "Reset", true);
            }

            Offset.TranslateX = 0;
            
            NodeImage.Visibility = System.Windows.Visibility.Collapsed;
            TaskImage.Visibility = System.Windows.Visibility.Collapsed;
            PictureImage.Visibility = System.Windows.Visibility.Collapsed;
            AudioImage.Visibility = System.Windows.Visibility.Collapsed;
            RemoveImage.Visibility = System.Windows.Visibility.Collapsed;
            DeleteImage.Visibility = System.Windows.Visibility.Collapsed;
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
        void Node_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (EditOnCreate)
            {
                NameText.Focus();
                EditOnCreate = false;
            }
        }

        //===================================================================================================================================================//
        void Node_Tap(object sender, GestureEventArgs e)
        {
            e.Handled = true;
        }

        //===================================================================================================================================================//
        void DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (RootUIControl != null)
            {
                Node node = (Node)(DataContext as Node);
                NodeController.CurrentPageNode.Nodes.Remove(node.Id);
                NodeController.UI.MainListBox.Items.Remove(RootUIControl);
               
                NodeController.DeleteNode(node, true);
            }           
        }

        //===================================================================================================================================================//
        void PropertiesItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string param = "Node=" + (DataContext as Node).Id;
            NodeController.UI.NavigationService.Navigate(new Uri("/Pages/PropertiesPage.xaml?" + param, UriKind.Relative));
        }
    }
}
