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
using System.Device.Location;
using Microsoft.Phone.Controls.Maps;

namespace Inhuman
{
    public partial class PropertiesPage : PhoneApplicationPage
    {
        public Node CurrentNode;
        public ObservableCollection<PageNode> Instances = new ObservableCollection<PageNode>();

        //===================================================================================================================================================//
        public PropertiesPage()
        {
            InitializeComponent();
			
			/*GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();			
			GeoCoordinate loc = watcher.Position.Location;
			
			if (loc.IsUnknown == true)
			{
				// Cannot retrieve the GPS position
				return;
			}
			
			MapControl.SetView(loc, 17);			
			MapLayer pushPinLayer = new MapLayer();
            MapControl.Children.Add(pushPinLayer);
			
			Pushpin p = new Pushpin();			
			p.Content = "YOU ARE HERE";
			p.Location = loc;
			
			pushPinLayer.AddChild(p, loc, PositionOrigin.BottomLeft);    
			*/
			
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
                
                // Find Instances //
                List<PageNode> references = NodeController.GetReferences(CurrentNode);
                Instances = new ObservableCollection<PageNode>(references);
                InstanceList.ItemsSource = Instances;
            }
        }

        //===================================================================================================================================================//
        void BackButton_Click(object sender, System.EventArgs e)
        {
            NavigationService.GoBack();
        }

        //===================================================================================================================================================//
        void DeleteButton_Click(object sender, System.EventArgs e)
        {
            Node node = (DataContext as Node);
            NodeController.DeleteNode(node, true);

            NavigationService.GoBack();
        }
    }
}
