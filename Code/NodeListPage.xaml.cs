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
        public ObservableCollection<ListNode> FilteredNodes = new ObservableCollection<ListNode>();

        //===================================================================================================================================================//
        public NodeListPage()
        {
            InitializeComponent();
        }
		
		//===================================================================================================================================================//
        void PhoneApplicationPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
        	// Load Nodes into list //
			for (int i = 0; i < Streamline.Data.Nodes.Count; i++)
			{
                if (Streamline.Data.Nodes[i] is PageNode)
	            {
                    ListNode node = new ListNode();
                    node.Name = Streamline.Data.Nodes[i].Name;
                    node.Id = Streamline.Data.Nodes[i].Id;
                    node.Type = "Page";

		            FilteredNodes.Add(node);
                    Debug.WriteLine(node.Name);
	            }
			}

            NodeList.ItemsSource = FilteredNodes;
        }

        //===================================================================================================================================================//
        void CancelButton_Click(object sender, System.EventArgs e)
        {
        	
        }

        //===================================================================================================================================================//
        void OKButton_Click(object sender, System.EventArgs e)
        {
            if (NodeList.SelectedItem != null)
	        {		
                // Create Link Node //
                LinkNode link = new LinkNode();
                UILinkNode uilink = new UILinkNode();
                MainPage.Instance.MainListBox.Items.Add(uilink);
                uilink.DataContext = link;

                Streamline.Data.Nodes.Add(link);
                Streamline.Data.CurrentPageNode.AddNode(link);

                link.Url = (NodeList.SelectedItem as ListNode).Id;
                PageNode page = Streamline.Data.GetNode(link.Url) as PageNode;
                uilink.DataContext = page;     
	        }

            Debug.WriteLine(NodeList.SelectedIndex);

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