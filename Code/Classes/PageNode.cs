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
    public class PageNode : Node
    {
        private ObservableCollection<string> _nodes = new ObservableCollection<string>();
        public ObservableCollection<string> Nodes
        {
            get
            {
                return _nodes;
            }
            set
            {
                if (value != _nodes)
                {
                    _nodes = value;
                    NotifyPropertyChanged("Nodes");
                }
            }
        }

        //===================================================================================================================================================//
        public PageNode()
        {
            Nodes.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Nodes_Changed);
        }

        //===================================================================================================================================================//
        void Nodes_Changed(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (NodeController.DataLoaded)
                UpdateTime();
        }

        //===================================================================================================================================================//
        public void AddNode(Node node)
        {
            Nodes.Add(node.Id);
        }
    }
}
