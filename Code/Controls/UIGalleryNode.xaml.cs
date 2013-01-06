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
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace Inhuman
{
    public partial class UIGalleryNode : UserControl
    {
        public int NumberOfPictures;
        ObservableCollection<UIThumbnail> Thumbnails;

        //===================================================================================================================================================//
        public UIGalleryNode()
        {
            InitializeComponent();

            Thumbnails = new ObservableCollection<UIThumbnail>();
			
        }

        //===================================================================================================================================================//
        public void Initialize()
        {
            ListBox lower = (ListBox)NodeObject.LowerContent;
            lower.ItemsSource = Thumbnails;

            GalleryNode gallery = (DataContext as GalleryNode);
            gallery.Nodes.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Nodes_CollectionChanged);
            LoadThumbnails();

            NodeObject.PlayAnim();
        }

        //===================================================================================================================================================//
        void Nodes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            LoadThumbnails();
        }

        //===================================================================================================================================================//
        public void LoadThumbnails()
        {
            GalleryNode gallery = (DataContext as GalleryNode);

            if (NodeController.Data != null)
            {
                Thumbnails.Clear();

                if (gallery.Nodes.Count > 0)
                {
                    int numPics = 0;

                    for (int i = 0; i < gallery.Nodes.Count; i++)
                    {
                        Node node = NodeController.GetNode(gallery.Nodes[i]);
                        
                        if (node is PictureNode)
                        {
                            PictureNode picNode = (node as PictureNode);

                            // Create Thumbnail Item //
                            numPics++;

                            UIThumbnail thumb = new UIThumbnail();
                            thumb.DataContext = node;
                            Thumbnails.Add(thumb);

                            if (picNode.Bitmap == null)
                            {
                                picNode.LoadBitmap();
                            }
                        }
                    }

                    NumberOfPictures = numPics;
                    gallery.Info = "Gallery - " + NumberOfPictures + " Pictures";
                }
                else
                {
                    NumberOfPictures = 0;
                    gallery.Info = "Gallery - No Pictures";
                }
            }
        }

        //===================================================================================================================================================//
        void OpenButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PageNode page = (PageNode)DataContext;

            string param = "Page=" + page.Id;
            NodeController.UI.NavigationService.Navigate(new Uri("/MainPage.xaml?" + param, UriKind.Relative));
        }
    }
}
