using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace Inhuman
{
    public partial class PropertiesPage : PhoneApplicationPage
    {
        public Node CurrentNode;
        public ObservableCollection<Node> Instances = new ObservableCollection<Node>();

        //===================================================================================================================================================//
        public PropertiesPage()
        {
            InitializeComponent();
        }
		
		//===================================================================================================================================================//
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            IDictionary<string, string> queryStrings = this.NavigationContext.QueryString;

            // Load Picture //
            if (queryStrings.ContainsKey("Node"))
            {
                CurrentNode = NodeController.GetNode(queryStrings["Node"]) as Node;
                DataContext = CurrentNode;
                InstanceList.ItemsSource = Instances;

                Instances.Clear();
                
                // Find Instances //
                foreach (Node node in NodeController.Data.Nodes)
                {
                    if (node is PageNode)
                    {
                        foreach (string nodeId in (node as PageNode).Nodes)
                        {
                            //if (nodeId == CurrentNode.Id)
                            //{
                                Instances.Add(NodeController.GetNode(nodeId));
                            //}
                        }
                    }
                }
            }
        }
    }
}
