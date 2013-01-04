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
    public class NodeAlphaCompare : IComparer<Node>
    {
        public int Compare(Node x, Node y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }

    public class NodeUpdatedCompare : IComparer<Node>
    {
        public int Compare(Node x, Node y)
        {
            return y.Updated.CompareTo(x.Updated);
        }
    }

    public class NodeCreatedCompare : IComparer<Node>
    {
        public int Compare(Node x, Node y)
        {
            return y.Created.CompareTo(x.Created);
        }
    }

    public class NodeMatchCompare : IComparer<ListNode>
    {
        public int Compare(ListNode x, ListNode y)
        {
            return y.Rank.CompareTo(x.Rank);
        }
    }

    public enum FilterType { All, Pages, Pictures, Audio, Tasks }
    public enum SortType { LastUpdated, Alphabetical }

    public partial class NodeListPage : PhoneApplicationPage
    {
        public List<Node> FilteredNodes = new List<Node>();
        public List<ListNode> SortedNodes = new List<ListNode>();
        public ObservableCollection<ListNode> MatchNodes = new ObservableCollection<ListNode>();
        public string Search = "";

        IsolatedStorageSettings Storage;

        int SortIndex = 2;
        int FilterIndex = 1;
        bool Loaded = false;

        //===================================================================================================================================================//
        public NodeListPage()
        {
            InitializeComponent();
            Storage = IsolatedStorageSettings.ApplicationSettings;            
        }

        //===================================================================================================================================================//
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }
        
        //===================================================================================================================================================//
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (Storage.Contains("SortMode"))
                Storage["SortMode"] = SortPicker.SelectedIndex;
            else
                Storage.Add("SortMode", SortPicker.SelectedIndex);

            if (Storage.Contains("FilterMode"))
                Storage["FilterMode"] = FilterPicker.SelectedIndex;
            else
                Storage.Add("FilterMode", FilterPicker.SelectedIndex);

            Storage.Save();
        }
		
		//===================================================================================================================================================//
        void PhoneApplicationPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Storage.Contains("SortMode"))
                SortIndex = (int)Storage["SortMode"];                
            if (Storage.Contains("FilterMode"))
                FilterIndex = (int)Storage["FilterMode"];               

            SortPicker.SelectedIndex = SortIndex;
            FilterPicker.SelectedIndex = FilterIndex;

            Loaded = true;
            UpdateList();
        }

        //===================================================================================================================================================//
        public Dictionary<int, Type> NodeDict = new Dictionary<int, Type> { { -1, typeof(Node) }, { 0, typeof(Node) }, { 1, typeof(PageNode) }, { 2, typeof(PictureNode) }, { 3, typeof(Node) }, { 4, typeof(TaskNode) } };
        public void UpdateList()
        {
            if (Loaded)
            {
                // Filter //
                int i = FilterPicker.SelectedIndex;

                if (i == 0)
                    FilterNodes<Node>();
                else if (i == 1)
                    FilterNodes<PageNode>();
                else if (i == 2)
                    FilterNodes<PictureNode>();
                else if (i == 3)
                    FilterNodes<AudioNode>();
                else if (i == 4)
                    FilterNodes<TaskNode>();
                else if (i == -1)
                    FilterNodes<Node>();

                // Sort //
                i = SortPicker.SelectedIndex;

                if (i == 0)
                {
                    NodeUpdatedCompare nc = new NodeUpdatedCompare();
                    FilteredNodes.Sort(nc);
                }
                else if (i == 1)
                {
                    NodeCreatedCompare nc = new NodeCreatedCompare();
                    FilteredNodes.Sort(nc);
                }
                else if (i == 2)
                {
                    NodeAlphaCompare nc = new NodeAlphaCompare();
                    FilteredNodes.Sort(nc);
                }

                CreateNodes();

                if (Search != "Search..." && Search != "")
                    SortedNodes.Sort(new NodeMatchCompare());

                MatchNodes = new ObservableCollection<ListNode>(SortedNodes);

                NodeList.ItemsSource = MatchNodes;
            }
        }

        float MaxRank = 0;

        //===================================================================================================================================================//
        public void CreateNodes()
        {
            Block matchBlock = new Block(Search);
            matchBlock.Update();

            SortedNodes.Clear();
            MaxRank = 0.001f;
            for (int i = 0; i < FilteredNodes.Count; i++)
            {
                // Ranking //
                float rank = 0.001f;

                /*if (Search != "Search..." && Search != "")
                {
                    Block nodeBlock = new Block(FilteredNodes[i].Name);
                    nodeBlock.Update();
                    rank = Block.Compare(nodeBlock, matchBlock);
                    if (rank > MaxRank)
                        MaxRank = rank;
                }   */            
                
                //if (rank > 0)
                //{
                if (FilteredNodes[i].Name.ToLower().Contains(Search) || Search == "Search..." || Search == "")
                {                               
                    ListNode node = new ListNode();
                    node.Name = FilteredNodes[i].Name;
                    node.Id = FilteredNodes[i].Id;
                    node.Info = FilteredNodes[i].Info;
                    node.Rank = rank;

                    SortedNodes.Add(node);
                }                
            }

            /*float scale = 1 / MaxRank;
            for (int i = 0; i < SortedNodes.Count; i++)
            {
                SortedNodes[i].Rank *= scale;
            }*/
        }

        //===================================================================================================================================================//
        public void FilterNodes<T>()
        {
            FilteredNodes.Clear();
            for (int i = 0; i < NodeController.Data.Nodes.Count; i++)
            {
                //if (NodeController.Data.Nodes[i] is T && NodeController.Data.Nodes[i].Name.ToLower().Contains(Search.ToLower()))
                if (NodeController.Data.Nodes[i] is T)
                {
                    // Not Self //
                    if (NodeController.Data.Nodes[i] == NodeController.CurrentPageNode)
                        continue;	                    
                        
                    // Not on Page Already //
                    bool onPage = false;
                    foreach (string node in NodeController.CurrentPageNode.Nodes)
                    {
                        if (NodeController.Data.Nodes[i].Id == node)
                        {
                            onPage = true;
                            break;
                        }
                    }

                    if (onPage)
                        continue;

                    FilteredNodes.Add(NodeController.Data.Nodes[i]);
                }
            }
        }

        //===================================================================================================================================================//
        void CancelButton_Click(object sender, System.EventArgs e)
        {
            NavigationService.GoBack();
        }

        //===================================================================================================================================================//
        void OKButton_Click(object sender, System.EventArgs e)
        {         
            //NodeController.CreateUINode(NodeList.SelectedItem as Node);
            NodeController.DuplicateNode(NodeList.SelectedItem as Node);

            NavigationService.GoBack();
        }

        //===================================================================================================================================================//
        void FilterPicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (FilterPicker.SelectedItem != null)
            {
                Debug.WriteLine(FilterPicker.SelectedItem);
            }

            UpdateList();
        }

        //===================================================================================================================================================//
        void SortPicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (SortPicker.SelectedItem != null)
            {
                Debug.WriteLine(SortPicker.SelectedItem);
            }

            UpdateList();
        }

        //===================================================================================================================================================//
        void SearchText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Search = SearchText.Text;
            UpdateList();       
        }

        //===================================================================================================================================================//
        void SearchText_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
        	SearchText.SelectAll();
        }

        //===================================================================================================================================================//
        void NodeList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        	NodeController.DuplicateNode(NodeList.SelectedItem as Node);
            NavigationService.GoBack();
        }
		
		//===================================================================================================================================================//
        void DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ListNode listNode = (sender as FrameworkElement).DataContext as ListNode;
            MatchNodes.Remove(listNode);

            Node node = NodeController.GetNode(listNode.Id);

            NodeController.DeleteNode(node, true);        
        }

        //===================================================================================================================================================//
        void PropertiesItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ListNode listnode = (sender as FrameworkElement).DataContext as ListNode;
            Node node = NodeController.GetNode(listnode.Id);
            string param = "Node=" + node.Id;
            NodeController.UI.NavigationService.Navigate(new Uri("/Pages/PropertiesPage.xaml?" + param, UriKind.Relative));
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

        private float _rank;
        public float Rank
        {
            get
            {
                return _rank;
            }
            set
            {
                if (value != _rank)
                {
                    _rank = value;
                    NotifyPropertyChanged("Rank");
                }
            }
        }

    }
}