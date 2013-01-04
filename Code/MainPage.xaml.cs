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

namespace Inhuman
{
    public partial class MainPage : PhoneApplicationPage
    {
        public static BitmapImage BackBitmap = new BitmapImage(new Uri("/Node;component/Art/Back.png", UriKind.Relative));
        public static BitmapImage HomeBitmap = new BitmapImage(new Uri("/Node;component/Art/Home.png", UriKind.Relative));

        public string CurrentPage;
        
        //===================================================================================================================================================//
        public MainPage()
        {
            InitializeComponent();    
        }

        //===================================================================================================================================================//
        void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
        }

        //===================================================================================================================================================//
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
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

        //===================================================================================================================================================//
        // NODES //
        //===================================================================================================================================================//
        void AddNodeButton_Click(object sender, EventArgs e)
        {
            NodeController.CreatePage();

            /*for (int i = 0; i < NodeController.Data.Nodes.Count; i++)
            {
                NodeController.Data.Nodes[i].Created = DateTime.Now - TimeSpan.FromDays(1) + TimeSpan.FromMinutes(i);
                NodeController.Data.Nodes[i].Updated = DateTime.Now - TimeSpan.FromDays(1) + TimeSpan.FromMinutes(i);
            }*/
        }

        //===================================================================================================================================================//
        void AddPageButton_Click(object sender, System.EventArgs e)
        {
            NodeController.CreatePage();
        }

        //===================================================================================================================================================//
        void WebMenu_Click(object sender, System.EventArgs e)
        {
            NodeController.CreateLink();
        }

        //===================================================================================================================================================//
        void TaskMenu_Click(object sender, System.EventArgs e)
        {
            NodeController.CreateTask();
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
            NodeController.CreateAudio();
        }

        //===================================================================================================================================================//
        void TextMenu_Click(object sender, System.EventArgs e)
        {
            NodeController.CreateText();
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
        void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainListBox.SelectedIndex == -1)
                return;

            //NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + MainListBox.SelectedIndex, UriKind.Relative));
            if (MainListBox.SelectedItem is UINode)
            {
                if ((MainListBox.SelectedItem as UINode).DataContext is WebNode)
                {
                    ((MainListBox.SelectedItem as UINode).DataContext as WebNode).Open();
                }
            }

            MainListBox.SelectedIndex = -1;
        }

        //===================================================================================================================================================//
        void ProjectMenu_Click(object sender, System.EventArgs e)
        {
            NodeController.CreateProject();
        }

        //===================================================================================================================================================//
        void GalleryMenu_Click(object sender, System.EventArgs e)
        {
        	NodeController.CreateGallery();
        }

        //===================================================================================================================================================//
        void ResetMenu_Click(object sender, System.EventArgs e)
        {
        	NodeController.Reset();
        }

        //===================================================================================================================================================//
        void HeaderMenu_Click(object sender, System.EventArgs e)
        {
        	NodeController.CreateHeader();
        }

        //===================================================================================================================================================//
        void AddPictureButton_Click(object sender, System.EventArgs e)
        {
            NodeController.CreatePicture();
        }

        //===================================================================================================================================================//
        void AddTaskButton_Click(object sender, System.EventArgs e)
        {
            NodeController.CreateTask();
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
    }
}