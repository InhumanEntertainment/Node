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
        public Stack<PageNode> PageHistory = new Stack<PageNode>();

        //===================================================================================================================================================//
        public MainPage()
        {
            Instance = this;
            InitializeComponent();

            DataContext = null;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        //===================================================================================================================================================//
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {            
            
        }

        //===================================================================================================================================================//
        public void LoadPageNode(PageNode page)
        {
            if (page != null)
            {
                MainListBox.Items.Clear();
                ScrollView.ScrollToVerticalOffset(0);
                DataContext = page;
                PageHistory.Push(page);
                Streamline.Data.CurrentPage = page.Id;

                for (int i = 0; i < page.Nodes.Count; i++)
                {
                    for (int x = 0; x < Streamline.Data.Nodes.Count; x++)
                    {
                        if (page.Nodes[i] == Streamline.Data.Nodes[x].Id)
                        {
                            Node node = Streamline.Data.Nodes[x];
                               
                            if (Streamline.Data.Nodes[x] is LinkNode)
                            {
                                UILinkNode uilink = new UILinkNode();
                                MainListBox.Items.Add(uilink);
                                uilink.DataContext = Streamline.Data.GetNode((node as LinkNode).Url);
                            }
                            else if (Streamline.Data.Nodes[x] is ImageNode)
                            {
                                UIImageNode uilink = new UIImageNode();
                                (node as ImageNode).LoadBitmap();
                                MainListBox.Items.Add(uilink);
                                uilink.DataContext = node;
                            }
                            else if (Streamline.Data.Nodes[x] is TaskNode)
                            {
                                UITaskNode uinode = new UITaskNode();
                                MainListBox.Items.Add(uinode);
                                uinode.DataContext = node;
                            }
                            else if (Streamline.Data.Nodes[x] is AudioNode)
                            {
                                UIAudioNode uilink = new UIAudioNode();
                                (node as AudioNode).LoadSound();
                                MainListBox.Items.Add(uilink);
                                uilink.DataContext = node;
                            }
                            else if (Streamline.Data.Nodes[x] is TextNode)
                            {
                                UITextNode uinode = new UITextNode();
                                MainListBox.Items.Add(uinode);
                                uinode.DataContext = node;
                            }
                            else
                            {
                                UINode uinode = new UINode();
                                MainListBox.Items.Add(uinode);
                                uinode.DataContext = node;
                            }
                        }
                    }
                }
            }
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
                LoadPageNode(Streamline.Data.GetNode(page) as PageNode);
            }
            else
            {
                LoadPageNode(Streamline.Data.CurrentPageNode);
            }
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
        void AddNodeButton_Click(object sender, EventArgs e)
        {
			Node node = new Node("New Node");
			UINode uinode = new UINode();
			MainListBox.Items.Add(uinode);
			uinode.DataContext = node;

            Streamline.Data.Nodes.Add(node);
            Streamline.Data.CurrentPageNode.AddNode(node);
        }

        //===================================================================================================================================================//
        void RestButton_Click(object sender, System.EventArgs e)
        {
            PageHistory.Clear();

            Streamline.Data.Reset();
            LoadPageNode(Streamline.Data.CurrentPageNode);
        }

        //===================================================================================================================================================//
        void AddPageButton_Click(object sender, System.EventArgs e)
        {
            // Create Link Node //
            LinkNode link = new LinkNode();
            UILinkNode uilink = new UILinkNode();
            MainListBox.Items.Add(uilink);
            uilink.DataContext = link;

            Streamline.Data.Nodes.Add(link);
            Streamline.Data.CurrentPageNode.AddNode(link);

            // Create New Page //
        	PageNode page = new PageNode();
			page.Name = "New Page";
            Streamline.Data.Nodes.Add(page);

            link.Url = page.Id;
            uilink.DataContext = page;      
        }

        //===================================================================================================================================================//
        void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (PageHistory.Count > 1)
            {
                PageHistory.Pop();
                LoadPageNode(PageHistory.Pop());
            }
            e.Cancel = true;
        }

        //===================================================================================================================================================//
        void ExportMenu_Click(object sender, System.EventArgs e)
        {
            Streamline.Data.ExportAll();
        }

        //===================================================================================================================================================//
        void AddExistingButton_Click(object sender, System.EventArgs e)
        {
            NavigationService.Navigate(new Uri("/NodeListPage.xaml", UriKind.Relative));
        }

        //===================================================================================================================================================//
        void PictureMenu_Click(object sender, System.EventArgs e)
        {
        	PhotoChooserTask task = new PhotoChooserTask();
            task.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
            task.ShowCamera = true;
            task.Show();
        } 
		
		//===================================================================================================================================================//
        void photoChooserTask_Completed(object sender, PhotoResult e)
		{
			if (e.TaskResult == TaskResult.OK)
			{                
				BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
				bmp.SetSource(e.ChosenPhoto);

                WriteableBitmap writableBitmap = new WriteableBitmap(bmp);

                // Add Image Node //
                ImageNode node = new ImageNode();
                UIImageNode uinode = new UIImageNode();
                node.Bitmap = bmp;
                node.Name = "Picture";
                node.Filename = node.Id + ".jpg";
                MainListBox.Items.Add(uinode);
                uinode.DataContext = node;

                Streamline.Data.Nodes.Add(node);
                Streamline.Data.CurrentPageNode.AddNode(node);

                // Save Image // 
                IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
                IsolatedStorageFileStream stream = storage.CreateFile(node.Filename);
                Extensions.SaveJpeg(writableBitmap, stream, bmp.PixelWidth, bmp.PixelHeight, 0, 90);

                stream.Close();
                stream.Dispose();
			}
		}

        //===================================================================================================================================================//
        void WebMenu_Click(object sender, System.EventArgs e)
        {
            WebNode node = new WebNode() { Name="Web Link" };
            UINode uinode = new UINode();
            MainListBox.Items.Add(uinode);
            uinode.DataContext = node;

            Streamline.Data.Nodes.Add(node);
            Streamline.Data.CurrentPageNode.AddNode(node);
        }

        //===================================================================================================================================================//
        void TaskMenu_Click(object sender, System.EventArgs e)
        {
            TaskNode node = new TaskNode();
            node.Name = "Task";
            UITaskNode uinode = new UITaskNode();
            MainListBox.Items.Add(uinode);
            uinode.DataContext = node;

            Streamline.Data.Nodes.Add(node);
            Streamline.Data.CurrentPageNode.AddNode(node);
        }

        //===================================================================================================================================================//
        void AudioMenu_Click(object sender, System.EventArgs e)
        {
            AudioNode node = new AudioNode();
            node.Name = "Audio";
            UIAudioNode uinode = new UIAudioNode();
            MainListBox.Items.Add(uinode);
            uinode.DataContext = node;
            uinode.ActionText = "Record";

            Streamline.Data.Nodes.Add(node);
            Streamline.Data.CurrentPageNode.AddNode(node);
        }

        //===================================================================================================================================================//
        void TextMenu_Click(object sender, System.EventArgs e)
        {
            TextNode node = new TextNode();
            node.Name = "Text";
            UITextNode uinode = new UITextNode();
            MainListBox.Items.Add(uinode);
            uinode.DataContext = node;

            Streamline.Data.Nodes.Add(node);
            Streamline.Data.CurrentPageNode.AddNode(node);
        }

        //===================================================================================================================================================//
        void PinMenu_Click(object sender, System.EventArgs e)
        {
            string tileParameter = "Page=" + Streamline.Data.CurrentPageNode.Id;
            ShellTile tile = CheckIfTileExist(tileParameter);// Check if Tile's title has been used 
            if (tile == null) 
            { 
                StandardTileData secondaryTile = new StandardTileData 
                {
                    Title = Streamline.Data.CurrentPageNode.Name, 
                    //BackgroundImage = new Uri("Background.png", UriKind.Relative),
                    BackContent = Streamline.Data.CurrentPageNode.Nodes.Count + " Nodes"
  
                }; 
                ShellTile.Create(new Uri("/MainPage.xaml?" + tileParameter, UriKind.Relative), secondaryTile); // Pass tileParameter as QueryString 
            } 
        }

        //===================================================================================================================================================//
        ShellTile CheckIfTileExist(string tileUri) 
        { 
            ShellTile shellTile = ShellTile.ActiveTiles.FirstOrDefault( tile => tile.NavigationUri.ToString().Contains(tileUri)); 

            return shellTile; 
        }
    }
}