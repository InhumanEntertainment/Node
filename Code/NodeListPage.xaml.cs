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
    public partial class NodeListPage : PhoneApplicationPage
    {
        public ObservableCollection<Node> FilteredNodes = new ObservableCollection<Node>();

        //===================================================================================================================================================//
        public NodeListPage()
        {
            InitializeComponent();
        }
		
		//===================================================================================================================================================//
        void PhoneApplicationPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
        	// Load Nodes into list //
			for (int i = 0; i < NodeController.Data.Nodes.Count; i++)
			{
                if (NodeController.Data.Nodes[i] is PageNode)
	            {
                    ListNode node = new ListNode();
                    node.Name = NodeController.Data.Nodes[i].Name;
                    node.Id = NodeController.Data.Nodes[i].Id;
                    node.Type = "Page";

                    FilteredNodes.Add(NodeController.Data.Nodes[i]);
                    Debug.WriteLine(node.Name);
	            }

			}

            // Other Nodes //
            for (int i = 0; i < NodeController.Data.Nodes.Count; i++)
            {
                if (!(NodeController.Data.Nodes[i] is PageNode))
                {
                    ListNode node = new ListNode();
                    node.Name = NodeController.Data.Nodes[i].Name;
                    node.Id = NodeController.Data.Nodes[i].Id;
                    node.Type = "Other";

                    FilteredNodes.Add(NodeController.Data.Nodes[i]);
                    Debug.WriteLine(node.Name);
                }
            }

            NodeList.ItemsSource = FilteredNodes;
        }

        //===================================================================================================================================================//
        void CancelButton_Click(object sender, System.EventArgs e)
        {
        	NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        //===================================================================================================================================================//
        void OKButton_Click(object sender, System.EventArgs e)
        {         
            //NodeController.CreateUINode(NodeList.SelectedItem as Node);
            NodeController.DuplicateNode(NodeList.SelectedItem as Node);

            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }



    public class ListNode : Node
    {
        private string _type;
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (value != _type)
                {
                    _type = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }

    }
}