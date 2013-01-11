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
using Microsoft.Phone.Controls;
using System.Diagnostics;
using System.Windows.Threading;

namespace Inhuman
{
    public partial class ArrangePage : PhoneApplicationPage
    {
        public List<FrameworkElement> Nodes = new List<FrameworkElement>();
        public List<double> NodeTargets = new List<double>();

        public FrameworkElement SelectedNode;
        public double Offset;
        public double StartPosition;
        public double CurrentPosition;
        public double NodeOffset;

        public bool Dragging = false;

        public TranslateTransform Xform = new TranslateTransform();
        public bool EditMode = true;
        public double ScrollSize;
        public double ScrollValue;
        public double ScrollStartValue;
        public double ScrollTarget;
        public double ScrollVelocity;
        public DispatcherTimer Timer;
        public int Sliding = 0;

        //===================================================================================================================================================//
        public ArrangePage()
        {
            InitializeComponent();
            Slider.RenderTransform = Xform;
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(10);
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Start();              
        }

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


            // Set Positions //
            ScrollValue = ScrollValue * 0.8f + ScrollTarget * 0.2f;
            Xform.Y = ScrollValue;

            // Seta Noe Targets //
            for (int i = 0; i < Nodes.Count; i++)
			{
			    if (Nodes[i] != SelectedNode)
                {
                    double pos = Canvas.GetTop(Nodes[i]) * 0.8f + NodeTargets[i] * 0.2f;
                    Canvas.SetTop(Nodes[i], pos);
                }
			}
            
        }

        //===================================================================================================================================================//
        void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Get Canvas Children //
            foreach (var node in NodeList.Children)
	        {
                Nodes.Add((FrameworkElement)node);
                NodeTargets.Add(0);
                Debug.WriteLine(node);
	        }

            // Get Scroll size //
            double size = 0;
            for (int i = 0; i < Nodes.Count; i++)
            {
                size += Nodes[i].ActualHeight;

            }
            ScrollSize = Math.Max(0, size - NodeList.ActualHeight);

            ArrangeNodes();
        }

        //===================================================================================================================================================//
        void NodeList_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            GeneralTransform tx = (e.OriginalSource as UIElement).TransformToVisual(this);
            StartPosition = tx.Transform(e.ManipulationOrigin).Y - ScrollValue;
            Dragging = true;

            if (EditMode)
            {
                if (!(e.OriginalSource is Canvas))
                {
                    SelectedNode = (FrameworkElement)GetRootUI(e.OriginalSource as DependencyObject);
                    Canvas.SetZIndex(SelectedNode, 1);
                    NodeOffset = e.ManipulationOrigin.Y;                   
                    Debug.WriteLine("Down: " + SelectedNode + " - " + StartPosition);                   
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
                
                if (next is UIPageNode || next is UITextNode)
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

            if (Dragging)
            {
                GeneralTransform tx = (e.OriginalSource as UIElement).TransformToVisual(this);
                CurrentPosition = tx.Transform(e.ManipulationOrigin).Y - ScrollValue;
                Debug.WriteLine("Start: " + StartPosition + ", Current: " + CurrentPosition);
                Offset = CurrentPosition - StartPosition;
                //StartPosition -= Offset;
                       
                if (EditMode)
                {
                    Canvas.SetTop(SelectedNode, CurrentPosition - NodeOffset);
                  
                    // Scroll if close to top or bottom //
                    if (CurrentPosition + ScrollValue < 100)
                        Sliding = 1;
                    else if (CurrentPosition + ScrollValue > 700)
                        Sliding = -1;
                    else
                        Sliding = 0;

                    ArrangeNodes();
                }
                else
                {
                    //Debug.WriteLine("Scroll: " + Offset + " - " + StartPosition);
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
        public void ArrangeNodes()
        {
            List<FrameworkElement> newNodes = new List<FrameworkElement>();
            List<double> newTargets = new List<double>();
            double YPos = 0;
            double emptySpace = 0;
            bool foundselected = false;
            
            // Add Selected Node //
            if (Dragging)
            {
                newNodes.Add(SelectedNode);
                newTargets.Add(0);                        
                emptySpace = CurrentPosition;
                Nodes.Remove(SelectedNode);
            }

            for (int i = 0; i < Nodes.Count; i++)
            {                
                if (Nodes[i] != SelectedNode)
                {
                    double nodeheight = Nodes[i].ActualHeight;

                    if (Dragging && nodeheight * 0.5f > emptySpace - NodeOffset)
                    {
                        YPos += SelectedNode.ActualHeight;
                        emptySpace = 10000;
                        newNodes.Add(Nodes[i]);
                        newTargets.Add(YPos); 
                        foundselected = true;             
                    }
                    else if (foundselected)
                    {
                        newNodes.Add(Nodes[i]);
                        newTargets.Add(YPos);                       
                    }
                    else
                    {
                        newNodes.Insert(i, Nodes[i]);
                        newTargets.Insert(i, YPos);                        
                    }

                    //Canvas.SetTop(Nodes[i], YPos);
                    YPos += Nodes[i].ActualHeight;
                    emptySpace -= nodeheight;
                }
            }

            //for (int i = 0; i < newNodes.Count; i++)
            ////{
            //    newTargets.Add(Canvas.GetTop(newNodes[i]));     
            //}

            Nodes = newNodes;
            NodeTargets = newTargets;
        }
    }
}


// Drag selected object
