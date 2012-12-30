using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Threading;
using System.Diagnostics;
using Microsoft.Devices;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Inhuman
{
    public class ProjectNode : PageNode
    {
        float _progress;
        public float Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                _progress = value;
                NotifyPropertyChanged("Progress");
            }
        }

        public int NumberOfTasks;

        //===================================================================================================================================================//
        public ProjectNode() 
        {
            Nodes.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Nodes_CollectionChanged);
            CalculateProgress();
        }

        //===================================================================================================================================================//
        void Nodes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CalculateProgress();
        }

        //===================================================================================================================================================//
        public void CalculateProgress()
        {
            if (NodeController.Data != null)
            {
                if (Nodes.Count > 0)
                {
                    int numTasks = 0;
                    float totalProgress = 0;

                    for (int i = 0; i < Nodes.Count; i++)
                    {
                        Node node = NodeController.GetNode(Nodes[i]);

                        if (node is TaskNode)
                        {
                            totalProgress += (node as TaskNode).Completed ? 1 : 0;
                            numTasks++;
                        }
                    }

                    NumberOfTasks = numTasks;
                    Progress = totalProgress / numTasks;
                    Info = NumberOfTasks + " Tasks - " + (Progress * 100f).ToString("N0") + "%";
                }
                else
                {
                    NumberOfTasks = 0;
                    Progress = 0;
                    Info = "No Tasks";
                }               
            }           
        }
    }
}
