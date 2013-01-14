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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Media;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using System.IO;
using System.Diagnostics;
using Microsoft.Phone.Shell;
using Microsoft.Phone.UserData;
using System.Windows.Threading;

namespace Inhuman
{
    public partial class MainPage : PhoneApplicationPage
    {
        public static BitmapImage BackBitmap = new BitmapImage(new Uri("/Node;component/Art/Back.png", UriKind.Relative));
        public static BitmapImage HomeBitmap = new BitmapImage(new Uri("/Node;component/Art/Home.png", UriKind.Relative));
        public string CurrentPage;
        public DateTime LoadTime;
        
        //===================================================================================================================================================//
        public MainPage()
        {
            InitializeComponent();

            // Slider Setup //
            Slider.RenderTransform = Xform;
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(10);
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Start(); 
        }

        //===================================================================================================================================================//
        void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            TimeSpan time = DateTime.Now - LoadTime;
            Debug.WriteLine("Loaded in " + time.TotalSeconds.ToString("N2"));
			
			//NavigationService.Navigate(new Uri("/Pages/ArrangePage.xaml", UriKind.Relative));
            MeasureNodes();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NodeList.Children.Clear();                
        }

        //===================================================================================================================================================//
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadTime = DateTime.Now;

            base.OnNavigatedTo(e);
            IDictionary<string, string> queryStrings = this.NavigationContext.QueryString;
            NodeController.UI = this;    

            // Image Share //
            if (queryStrings.ContainsKey("FileId"))
            {
                MediaLibrary library = new MediaLibrary();
                Picture picture = library.GetPictureFromToken(queryStrings["FileId"]);

                BitmapImage bitmap = new BitmapImage();
                bitmap.CreateOptions = BitmapCreateOptions.None;
                bitmap.SetSource(picture.GetImage());

                WriteableBitmap picLibraryImage = new WriteableBitmap(bitmap);

                // Create ImageNode and Add it to Page //
            }

            // Load Page //
            if (queryStrings.ContainsKey("Page"))
            {
                CurrentPage = queryStrings["Page"];
                NodeController.LoadPage(NodeController.GetNode(CurrentPage) as PageNode);
            }
            else if (CurrentPage == null)
            {
                if (NodeController.Data.Nodes.Count > 0)
                {
                    CurrentPage = NodeController.Data.Nodes[0].Id;
                    NodeController.LoadPage(NodeController.GetNode(CurrentPage) as PageNode);
                }                
            }
            else
            {
                NodeController.LoadPage(NodeController.GetNode(CurrentPage) as PageNode);
            }
        }
        #region Buttons
        //===================================================================================================================================================//
        // NODES //
        //===================================================================================================================================================//
        void AddNodeButton_Click(object sender, EventArgs e)
        {
            NodeController.CreateNewUINode<UIPageNode>(NodeController.CreateNewNode<PageNode>("Node"), true);
        }

        //===================================================================================================================================================//
        void WebMenu_Click(object sender, System.EventArgs e)
        {
            NodeController.CreateLink();
        }

        //===================================================================================================================================================//
        void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //e.Cancel = true;
            //NodeController.PreviousPage();           
        }

        //===================================================================================================================================================//
        void AddExistingButton_Click(object sender, System.EventArgs e)
        {
            NavigationService.Navigate(new Uri("/NodeListPage.xaml", UriKind.Relative));
        }

        //===================================================================================================================================================//
        void ExportMenu_Click(object sender, System.EventArgs e)
        {
            NodeController.ExportAll();
        }

        //===================================================================================================================================================//
        void AudioMenu_Click(object sender, System.EventArgs e)
        {
            NodeController.CreateNewUINode<UIAudioNode>(NodeController.CreateNewNode<AudioNode>("Audio"), true);
        }

        //===================================================================================================================================================//
        void LinkMenu_Click(object sender, System.EventArgs e)
        {
            NodeController.CreateNewUINode<UILinkNode>(NodeController.CreateNewNode<LinkNode>("Link"), true);
        }

        //===================================================================================================================================================//
        void TextMenu_Click(object sender, System.EventArgs e)
        {
            NodeController.CreateNewUINode<UITextNode>(NodeController.CreateNewNode<TextNode>("Text"), true);
        }

        //===================================================================================================================================================//
        void PinMenu_Click(object sender, System.EventArgs e)
        {
            NodeController.PinPage();
        }

        //===================================================================================================================================================//
        void PictureMenu_Click(object sender, System.EventArgs e)
        {
            NodeController.CreatePicture();
        } 

        //===================================================================================================================================================//
        void PropertiesMenu_Click(object sender, System.EventArgs e)
        {
            string param = "Node =" + NodeController.Data.CurrentPage;

            NavigationService.Navigate(new Uri("/Pages/PropertiesPage.xaml?" + param, UriKind.Relative));
        }

        //===================================================================================================================================================//
        void ProjectMenu_Click(object sender, System.EventArgs e)
        {
            NodeController.CreateNewUINode<UIProjectNode>(NodeController.CreateNewNode<ProjectNode>("Project"), true);
        }

        //===================================================================================================================================================//
        void GalleryMenu_Click(object sender, System.EventArgs e)
        {
            NodeController.CreateNewUINode<UIGalleryNode>(NodeController.CreateNewNode<GalleryNode>("Gallery"), true);
        }

        //===================================================================================================================================================//
        void ResetMenu_Click(object sender, System.EventArgs e)
        {
        	NodeController.Reset();
        }

        //===================================================================================================================================================//
        void HeaderMenu_Click(object sender, System.EventArgs e)
        {
            NodeController.CreateNewUINode<UIHeaderNode>(NodeController.CreateNewNode<HeaderNode>("Header"), true);
        }

        //===================================================================================================================================================//
        void AddPictureButton_Click(object sender, System.EventArgs e)
        {
            NodeController.CreatePicture();
        }

        //===================================================================================================================================================//
        void AddTaskButton_Click(object sender, System.EventArgs e)
        {
            NodeController.CreateNewUINode<UITaskNode>(NodeController.CreateNewNode<TaskNode>("Task"), true);
        }

        //===================================================================================================================================================//
        void HomeImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NodeController.PreviousPage();

            /*if (NodeController.CurrentPageNode != NodeController.Data.Nodes[0])
            {
                PageNode page = NodeController.Data.Nodes[0] as PageNode;
                NodeController.LoadPage(page);
            }*/
        }

        //===================================================================================================================================================//
        void PageTitle_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
        	PageTitle.SelectAll();
        }

        //===================================================================================================================================================//
        void SyncMenu_Click(object sender, System.EventArgs e)
        {
        	NavigationService.Navigate(new Uri("/Pages/SyncPage.xaml", UriKind.Relative));
        }

        //===================================================================================================================================================//
        void MainListBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            /*if (NodeController.CurrentPageNode is ProjectNode)
            {
                NodeController.CreateTask();
            }
            else
            {
                NodeController.CreatePage();
            } */
        }

        //===================================================================================================================================================//
        void HelpMenu_Click(object sender, System.EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/HelpPage.xaml", UriKind.Relative));
        }
        #endregion

        #region Edit Mode

        public List<UIControl> UINodes = new List<UIControl>();
        public List<double> NodeTargets = new List<double>();

        public UIControl SelectedNode;
        public double Offset;
        public double StartPosition;
        public double CurrentPosition;
        public double NodeOffset;

        public bool Dragging = false;

        public TranslateTransform Xform = new TranslateTransform();
        public bool EditMode = false;
        public double ScrollSize;
        public double ScrollValue;
        public double ScrollStartValue;
        public double ScrollTarget;
        public double ScrollVelocity;
        public DispatcherTimer Timer;
        public int Sliding = 0;

        //===================================================================================================================================================//
        void Timer_Tick(object sender, EventArgs e)
        {
            if (EditMode)
            {
                // Scroll if close to top or bottom //
                if (Sliding == 1)
                {
                    ScrollTarget += 50;
                    //CurrentPosition += 10;
                    //Canvas.SetTop(SelectedNode, CurrentPosition - NodeOffset);
                    ArrangeNodes();
                    
                }
                else if (Sliding == -1)
                {
                    ScrollTarget -= 50;
                    //CurrentPosition -= 10;
                    //Canvas.SetTop(SelectedNode, CurrentPosition - NodeOffset);
                    
                    ArrangeNodes();
                }

                ScrollTarget = Math.Min(ScrollTarget, 0);
                ScrollTarget = Math.Max(ScrollTarget, -ScrollSize);          
            }

            // Set Node Targets //
            for (int i = 0; i < UINodes.Count; i++)
            {
                if (UINodes[i] != SelectedNode)
                {
                    double pos = Canvas.GetTop(UINodes[i]) * 0.8f + NodeTargets[i] * 0.2f;
                    Canvas.SetTop(UINodes[i], pos);
                }
            }

            // Set Positions //
            ScrollValue = ScrollValue * 0.8f + ScrollTarget * 0.2f;
            Xform.Y = ScrollValue;
        }

        //===================================================================================================================================================//
        void NodeList_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            if (EditMode)
            {
                GeneralTransform tx = (e.OriginalSource as UIElement).TransformToVisual(NodeList);
                StartPosition = tx.Transform(e.ManipulationOrigin).Y - ScrollValue;
                Dragging = true;
            }
            else
            {
                GeneralTransform tx = (e.OriginalSource as UIElement).TransformToVisual(this);
                StartPosition = tx.Transform(e.ManipulationOrigin).Y - ScrollValue;
                Dragging = true;
            }

            Debug.WriteLine("Drag Start: " + e.OriginalSource);    

            if (EditMode)
            {
                if (!(e.OriginalSource is Canvas))
                {
                    SelectedNode = GetRootUI(e.OriginalSource as UIElement) as UIControl;
                    Canvas.SetZIndex(SelectedNode, 1);
                    NodeOffset = e.ManipulationOrigin.Y;                   
                    //Debug.WriteLine("Down: " + SelectedNode + " - " + StartPosition); 
                    Debug.WriteLine("Selected Node = " + SelectedNode);                    
                } 
            }
            else
            {
                ScrollStartValue = ScrollValue;
            }
            
        }

        //===================================================================================================================================================//
        public DependencyObject GetRootUI(DependencyObject element)
        {
            DependencyObject next = element;
            for (int i = 0; i < 20; i++)
            {
                next = VisualTreeHelper.GetParent(next);

                if (next is UIControl)
                {
                    return next;
                }
            }

            return null;
        }

        //===================================================================================================================================================//
        void NodeList_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            //Debug.WriteLine("Container: " + e.ManipulationContainer);
            //e.Handled = true;
            if (Dragging)
            {
                if (EditMode)
                {
                    GeneralTransform tx = (e.OriginalSource as UIElement).TransformToVisual(NodeList);
                    CurrentPosition = tx.Transform(e.ManipulationOrigin).Y - ScrollValue;
                }
                else
                {
                    GeneralTransform tx = (e.OriginalSource as UIElement).TransformToVisual(this);
                    CurrentPosition = tx.Transform(e.ManipulationOrigin).Y - ScrollValue;
                }
                
                Debug.WriteLine("Start: " + StartPosition + ", Current: " + CurrentPosition + ", Scroll: " + ScrollValue + ", ScrollSize: " + ScrollSize);
                Debug.WriteLine("Drag Delta: " + e.OriginalSource);    

                Offset = CurrentPosition - StartPosition;
                //StartPosition -= Offset;

                
                       
                if (EditMode && SelectedNode != null)
                {
                    Canvas.SetTop(SelectedNode, CurrentPosition - NodeOffset);
                  
                    // Scroll if close to top or bottom //
                    if (CurrentPosition + ScrollValue < 100)
                        Sliding = 1;
                    else if (CurrentPosition + ScrollValue > 540)
                        Sliding = -1;
                    else
                        Sliding = 0;

                    MeasureNodes();
                }
                else
                {
                    Debug.WriteLine("Scroll: " + Offset + " - " + StartPosition);
                    ScrollTarget = ScrollStartValue + Offset;
                    ScrollValue = ScrollTarget;
                    Xform.Y = ScrollValue;
                }
            }
        }

        //===================================================================================================================================================//
        void NodeList_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            if (Dragging)
            {
                Dragging = false;
                Sliding = 0;
                    
                if (EditMode)
                {
                    if (SelectedNode != null)
                        Canvas.SetZIndex(SelectedNode, 0);
                    
                    SelectedNode = null;
                    ArrangeNodes();
                }
                else
                {
                    //Timer.Stop();
                    ScrollTarget = Math.Min(ScrollTarget, 0);
                    ScrollTarget = Math.Max(ScrollTarget, -ScrollSize);
                }
            }
        }

        //===================================================================================================================================================//
        public void MeasureNodes()
        {
            Debug.WriteLine("=====================================================\nMeasure:");

            // Get Canvas Children //
            /*UINodes.Clear();
            foreach (var node in NodeList.Children)
            {
                UINodes.Add((FrameworkElement)node);
                NodeTargets.Add(0);
                Debug.WriteLine("Measure: " + node);
            }*/

            // Get Scroll size //
            double size = 0;
            for (int i = 0; i < UINodes.Count; i++)
            {
                size += UINodes[i].ActualHeight;
            }
            ScrollSize = Math.Max(0, size - NodeList.ActualHeight);
            Debug.WriteLine("Scroll Size = " + ScrollSize);

            ArrangeNodes();
        }
        
        //===================================================================================================================================================//
        public void ArrangeNodes()
        {
            Debug.WriteLine("=====================================================\nArrange: " + UINodes.Count + " Nodes");

            List<double> newTargets = new List<double>();
            List<UIControl> newUINodes = new List<UIControl>();


            double YPos = 0;
            double emptySpace = 0;
            bool foundselected = false;
            newUINodes.Clear();
            
            // Add Selected Node //
            if (Dragging && SelectedNode != null)
            {
                newUINodes.Add(SelectedNode);
                newTargets.Add(0);                        
                emptySpace = CurrentPosition;
                UINodes.Remove(SelectedNode as UIControl);
            }

            for (int i = 0; i < UINodes.Count; i++)
            {                
                if (UINodes[i] != SelectedNode)
                {
                    double nodeheight = UINodes[i].ActualHeight;

                    if (Dragging && SelectedNode != null && nodeheight * 0.5f > emptySpace - NodeOffset)
                    {
                        YPos += SelectedNode.ActualHeight;
                        emptySpace = 10000;
                        foundselected = true;

                        newUINodes.Add(UINodes[i]);
                        newTargets.Add(YPos);                                  
                    }
                    else if (foundselected)
                    {
                        newUINodes.Add(UINodes[i]);
                        newTargets.Add(YPos);
                    }
                    else
                    {
                        newUINodes.Insert(i, UINodes[i]);
                        newTargets.Insert(i, YPos);
                    }

                    //Canvas.SetTop(UINodes[i], YPos);
                    Debug.WriteLine("Set Top: " + YPos + ", " + Canvas.GetLeft(UINodes[i]));
                    YPos += UINodes[i].ActualHeight;
                    emptySpace -= nodeheight;
                }
            }

            //for (int i = 0; i < newNodes.Count; i++)
            ////{
            //    newTargets.Add(Canvas.GetTop(newNodes[i]));     
            //}
            UINodes = newUINodes;
            NodeTargets = newTargets;
            //MainListBox.Items.Clear();
        }

        //===================================================================================================================================================//
        void EditToggle_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
			EditMode = true;
			
        	// Disable Hittest on all node objects //
            for (int i = 0; i < UINodes.Count; i++)
            {
                Debug.WriteLine(UINodes[i].Node.Name);
                UINodes[i].SetHitTest(false);
            }
			
			
			
        }

        //===================================================================================================================================================//
        void EditToggle_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
			EditMode = false;
			
        	// Enable Hittest on all node objects //
            for (int i = 0; i < UINodes.Count; i++)
            {
                Debug.WriteLine(UINodes[i].Node.Name);
                UINodes[i].SetHitTest(true);
            }
			
			
        }

        #endregion
    }
}