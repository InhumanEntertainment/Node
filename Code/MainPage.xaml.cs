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

namespace Inhuman
{
    public partial class MainPage : PhoneApplicationPage
    {
        public static MainPage Instance;
        
        //===================================================================================================================================================//
        public MainPage()
        {
            Instance = this;
            InitializeComponent();
            NodeController.UI = this;

            DataContext = null;
        }     

        //===================================================================================================================================================//
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            IDictionary<string, string> queryStrings = this.NavigationContext.QueryString;

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

            // Pinned Pages //
            if (queryStrings.ContainsKey("Page"))
            {
                string page = queryStrings["Page"];
                NodeController.LoadPage(NodeController.GetNode(page) as PageNode);
            }
            else
            {
                NodeController.LoadPage(NodeController.CurrentPageNode);
            }
        }




        //===================================================================================================================================================//
        // NODES //
        //===================================================================================================================================================//
        void AddNodeButton_Click(object sender, EventArgs e)
        {
            NodeController.CreateNode();
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
        void ResetButton_Click(object sender, System.EventArgs e)
        {
            NodeController.Reset();
        }

        //===================================================================================================================================================//
        void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            NodeController.PreviousPage();           
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
    }
}