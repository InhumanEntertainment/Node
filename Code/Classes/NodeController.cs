using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.Windows.Threading;

namespace Inhuman
{
    public static class NodeController
    {
        static StreamlineData _Data;
        static MainPage _UI;

        public static StreamlineData Data
        {
            get
            {
                return _Data;
            }
            set
            {
                _Data = value;
            }
        }
        public static MainPage UI
        {
            get
            {
                return _UI;
            }
            set
            {
                _UI = value;
            }
        }
        public static PageNode Page;
        public static Stack<PageNode> PageHistory = new Stack<PageNode>();
        public static Dictionary<string, string> ExportFilenames = new Dictionary<string, string>();

        public static PageNode CurrentPageNode
        {
            get
            {
                return GetNode(Data.CurrentPage) as PageNode;
            }
        }

        public static Skydrive Skydrive;
        public static bool DataLoaded = false;

        public static PageNode HelpPage;
        public static List<Node> HelpNodes = new List<Node>();

        // Node Buffers //
        public static int NodeBufferSize = 0;

        public static int UIPageCount = 0;
        public static int UITaskCount = 0;
        public static int UITextCount = 0;
        public static int UIHeaderCount = 0;
        public static int UIPictureCount = 0;
        public static int UIAudioCount = 0;
        public static int UIProjectCount = 0;
        public static int UIGalleryCount = 0;

        public static List<UIPageNode> UIPageNodes = new List<UIPageNode>();
        public static List<UITaskNode> UITaskNodes = new List<UITaskNode>();
        public static List<UITextNode> UITextNodes = new List<UITextNode>();
        public static List<UIPictureNode> UIPictureNodes = new List<UIPictureNode>();
        public static List<UIAudioNode> UIAudioNodes = new List<UIAudioNode>();
        public static List<UIProjectNode> UIProjectNodes = new List<UIProjectNode>();
        public static List<UIGalleryNode> UIGalleryNodes = new List<UIGalleryNode>();
        public static List<UIHeaderNode> UIHeaderNodes = new List<UIHeaderNode>();

        //===================================================================================================================================================//
        public static void ResetNodeBuffers()
        {
            UIPageCount = 0;
            UITaskCount = 0;
            UITextCount = 0;
            UIHeaderCount = 0;
            UIPictureCount = 0;
            UIAudioCount = 0;
            UIProjectCount = 0;
            UIGalleryCount = 0;
        }
        
        //===================================================================================================================================================//
        public static void LoadUINodes()
        {
            ResetNodeBuffers();

            for (int i = 0; i < NodeBufferSize; i++)
            {
                UIPageNodes.Add(new UIPageNode());
                UITaskNodes.Add(new UITaskNode());
                UIPictureNodes.Add(new UIPictureNode());
                UIAudioNodes.Add(new UIAudioNode());
                UIProjectNodes.Add(new UIProjectNode());
                UIGalleryNodes.Add(new UIGalleryNode());
                UIHeaderNodes.Add(new UIHeaderNode());
            }
        }
        
        //===================================================================================================================================================//
        public static UIPageNode GetUIPageNode()
        {
            UIPageNode node;

            if (UIPageCount < UIPageNodes.Count)
                node = UIPageNodes[UIPageCount];
            else
            {
                node = new UIPageNode();
                UIPageNodes.Add(node);
            }

            UIPageCount++;
            return node;
        }
        public static UITaskNode GetUITaskNode()
        {
            UITaskNode node;
            if (UITaskCount < UITaskNodes.Count)
                node = UITaskNodes[UITaskCount];
            else
            {
                node = new UITaskNode();
                UITaskNodes.Add(node);
            }

            UITaskCount++;                      
            return node;
        }
        public static UIPictureNode GetUIPictureNode()
        {
            UIPictureNode node;
            if (UIPictureCount < UIPictureNodes.Count)
                node = UIPictureNodes[UIPictureCount];
            else
            {
                node = new UIPictureNode();
                UIPictureNodes.Add(node);
            }

            UIPictureCount++;
            return node;
        }
        public static UIAudioNode GetUIAudioNode()
        {
            UIAudioNode node;
            if (UIAudioCount < UIAudioNodes.Count)
                node = UIAudioNodes[UIAudioCount];
            else
            {
                node = new UIAudioNode();
                UIAudioNodes.Add(node);
            }

            UIAudioCount++;
            return node;
        }
        public static UIProjectNode GetUIProjectNode()
        {
            UIProjectNode node;
            if (UIProjectCount < UIProjectNodes.Count)
                node = UIProjectNodes[UIProjectCount];
            else
            {
                node = new UIProjectNode();
                UIProjectNodes.Add(node);
            }

            UIProjectCount++;
            return node;
        }
        public static UIGalleryNode GetUIGalleryNode()
        {
            UIGalleryNode node;
            if (UIGalleryCount < UIGalleryNodes.Count)
                node = UIGalleryNodes[UIGalleryCount];
            else
            {
                node = new UIGalleryNode();
                UIGalleryNodes.Add(node);
            }

            UIGalleryCount++;
            return node;
        }
        public static UIHeaderNode GetUIHeaderNode()
        {
            UIHeaderNode node;
            if (UIHeaderCount < UIHeaderNodes.Count)
                node = UIHeaderNodes[UIHeaderCount];
            else
            {
                node = new UIHeaderNode();
                UIHeaderNodes.Add(node);
            }

            UIHeaderCount++;
            return node;
        }
        public static UITextNode GetUITextNode()
        {
            UITextNode node;
            if (UITextCount < UITextNodes.Count)
                node = UITextNodes[UITextCount];
            else
            {
                node = new UITextNode();
                UITextNodes.Add(node);
            }

            UITextCount++;
            return node;
        }

        //===================================================================================================================================================//
        public static void Initialize(StreamlineData data, MainPage ui)
        {
            Data = data;
            UI = ui;           
        }

        //===================================================================================================================================================//
        public static void Sync()
        {
            Data.Save();
            Skydrive.UploadAll();
        }

        //===================================================================================================================================================//
        public static void Reset()
        {
            PageHistory.Clear();

            Data.Reset();
            LoadPage(CurrentPageNode);
        }

        //===================================================================================================================================================//
        public static void DeleteNode(Node node, bool force)
        {
            List<PageNode> references = GetReferences(node);

            if (force)
	        {
                // Remove from each page //
                foreach (PageNode page in references)
                {
                    page.Nodes.Remove(node.Id);
                }

                DeletePin(node);
                Data.Nodes.Remove(node);
                Debug.WriteLine("Deleted Node with " + references.Count + " References");
                // Delete Attachment //
	        }
            else if (references.Count == 0)
            {
                DeletePin(node);
                Data.Nodes.Remove(node);
                Debug.WriteLine("Deleted Node with 0 References");
            }
        }       
        
        //===================================================================================================================================================//
        public static List<PageNode> GetReferences(Node node)
        {
            List<PageNode> references = new List<PageNode>();

            // Each Page //
            for (int i = 0; i < Data.Nodes.Count; i++)
            {
                if (Data.Nodes[i] is PageNode)
                {
                    PageNode page = Data.Nodes[i] as PageNode;

                    foreach (string item in page.Nodes)
                    {
                        if (item == node.Id)
                        {
                            references.Add(page);
                        }
                    }
                }
            }

            return references;
        }

        //===================================================================================================================================================//
        public static void PreviousPage()
        {
            if (UI.NavigationService.CanGoBack)
                UI.NavigationService.GoBack();
            else if (UI.CurrentPage != Data.Nodes[0].Id)
            {
                string param = "Page=" + Data.Nodes[0].Id;
                UI.NavigationService.RemoveBackEntry();
                UI.NavigationService.RemoveBackEntry();
                UI.NavigationService.Navigate(new Uri("/MainPage.xaml?" + param, UriKind.Relative));              
            }
        }

        //===================================================================================================================================================//
        public static void CreateLink()
        {
            CreateNewUINode<UINode>(CreateNewNode<WebNode>("Link"), true);
        }


        

        //===================================================================================================================================================//
        /*public static void CreateContact()
        {
            AddressChooserTask addressTask = new AddressChooserTask();
            addressTask.Completed += new EventHandler<AddressResult>(addressTask_Completed);
            addressTask.Show();
        }

        static void addressTask_Completed(object sender, AddressResult e)
        {
            Debug.WriteLine(e.DisplayName);
            Contacts contacts = new Contacts();
            contacts.SearchCompleted += new EventHandler<Microsoft.Phone.UserData.ContactsSearchEventArgs>(contacts_SearchCompleted);
            contacts.SearchAsync(e.DisplayName, Microsoft.Phone.UserData.FilterKind.None, null);
        }

        static void contacts_SearchCompleted(object sender, Microsoft.Phone.UserData.ContactsSearchEventArgs e)
        {
            foreach (var contact in e.Results)
            {
                Debug.WriteLine(contact.DisplayName);
                Stream stream = contact.GetPicture();
                if (stream != null)
                {
                    BitmapImage bmp = new BitmapImage();
                    bmp.SetSource(stream);
                }
            }
        }
        */


        //===================================================================================================================================================//
        public static void PrintHistory()
        {
            string buffer = "";

            foreach (var item in PageHistory)
            {
                buffer += " > " + item.Name;
            }
            Debug.WriteLine(buffer);
        }

        //===================================================================================================================================================//
        public static void LoadPage(PageNode page)
        {
            if (page != null)
            {
                Debug.WriteLine("Load Page: " + page.Name);

                // Start Button //
                if (page == Data.Nodes[0])
                {
                    UI.HomeImage.Source = MainPage.HomeBitmap;

                    if (page.Nodes.Count == 0)
                    {
                        UI.StartImage.Visibility = Visibility.Visible;
                        page.Nodes.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Nodes_CollectionChanged);
                    }
                    else
                        UI.StartImage.Visibility = Visibility.Collapsed;
                }
                else
                {
                    UI.HomeImage.Source = MainPage.BackBitmap;
                    UI.StartImage.Visibility = Visibility.Collapsed;
                }

                UI.DataContext = page;
                Data.CurrentPage = page.Id;
                NodeController.ResetNodeBuffers();
                UI.UINodes.Clear();
                UI.NodeList.Children.Clear();

                for (int i = 0; i < page.Nodes.Count; i++)
                {
                    for (int x = 0; x < Data.Nodes.Count; x++)
                    {
                        if (page.Nodes[i] == Data.Nodes[x].Id)
                        {
                            CreateUINode(Data.Nodes[x]);
                        }
                    }
                }
            }
        }

        //===================================================================================================================================================//
        static void Nodes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UI.StartImage.Visibility = Visibility.Collapsed;
            (UI.DataContext as PageNode).Nodes.CollectionChanged -= new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Nodes_CollectionChanged);
        }

#region Node Creation
        //===================================================================================================================================================//
        public static Node CreateNewNode<NodeType>(string name) where NodeType : new()
        {
            Node node = new NodeType() as Node;
            node.Name = name;
            Data.Nodes.Add(node);
            CurrentPageNode.AddNode(node);

            return node;
        }

        //===================================================================================================================================================//
        public static UIControl CreateNewUINode<T>(Node node, bool edit) where T : new()
        {
            //UIControl uinode = GetUIPageNode();
            UIControl uinode = new T() as UIControl;
            UI.NodeList.Children.Add(uinode);
            UI.UINodes.Add(uinode);
            uinode.DataContext = node;
            uinode.Initialize(edit);
            UI.MeasureNodes();

            return uinode;
        }

        //===================================================================================================================================================//
        public static void CreateUINode(Node node)
        {
            Debug.WriteLine("Create Node: " + node.Name);

            if (node is ProjectNode)
            {
                CreateNewUINode<UIProjectNode>(node, false);
            }
            else if (node is GalleryNode)
            {
                CreateNewUINode<UIGalleryNode>(node, false);
            } 
            else if (node is PageNode)
            {
                CreateNewUINode<UIPageNode>(node, false);
            }
            else if (node is HeaderNode)
            {
                CreateNewUINode<UIHeaderNode>(node, false);
            }
            else if (node is PictureNode)
            {
                CreateNewUINode<UIPictureNode>(node, false);
            }
            else if (node is TaskNode)
            {
                CreateNewUINode<UITaskNode>(node, false);
            }
            else if (node is AudioNode)
            {
                CreateNewUINode<UIAudioNode>(node, false);
            }
            else if (node is TextNode)
            {
                CreateNewUINode<UITextNode>(node, false);
            }
        }

#endregion

        #region Pins
        //===================================================================================================================================================//
        public static void PinPage()
        {
            string tileParameter = "Page=" + NodeController.CurrentPageNode.Id;
            ShellTile tile = CheckIfTileExist(tileParameter);// Check if Tile's title has been used 
            if (tile == null)
            {
                StandardTileData secondaryTile = new StandardTileData
                {
                    Title = NodeController.CurrentPageNode.Name,
                    BackgroundImage = new Uri("LiveTile.png", UriKind.Relative),
                    //BackContent = NodeController.CurrentPageNode.Nodes.Count + " Nodes"
                };
                ShellTile.Create(new Uri("/MainPage.xaml?" + tileParameter, UriKind.Relative), secondaryTile); // Pass tileParameter as QueryString 
            }
        }

        //===================================================================================================================================================//
        public static void UpdatePin(Node node)
        {
            ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(node.Id));
            if (tile != null)
            {
                StandardTileData tiledata = new StandardTileData
                {
                    Title = NodeController.CurrentPageNode.Name,
                    BackgroundImage = new Uri("LiveTile.png", UriKind.Relative),
                };

                tile.Update(tiledata);
			}           
        }

        //===================================================================================================================================================//
        public static void DeletePin(Node node)
        {
            ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(node.Id));
            if (tile != null)
                tile.Delete();
        }

        //===================================================================================================================================================//
        public static void DuplicateNode(Node node)
        {
            CurrentPageNode.Nodes.Add(node.Id);
        }

        //===================================================================================================================================================//
        static ShellTile CheckIfTileExist(string tileUri)
        {
            ShellTile shellTile = ShellTile.ActiveTiles.FirstOrDefault(tile => tile.NavigationUri.ToString().Contains(tileUri));

            return shellTile;
        }

#endregion

        //===================================================================================================================================================//
        public static void CreatePicture()
        {
            PhotoChooserTask task = new PhotoChooserTask();
            task.Completed += new EventHandler<PhotoResult>(PhotoTask_Completed);
            task.ShowCamera = true;
            task.Show();
        }

        //===================================================================================================================================================//
        static void PhotoTask_Completed(object sender, PhotoResult e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    if (e.TaskResult == TaskResult.OK)
                    {
                        BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                        bmp.SetSource(e.ChosenPhoto);

                        WriteableBitmap writableBitmap = new WriteableBitmap(bmp);

                        // Add Image Node //                        
                        PictureNode node = CreateNewNode<PictureNode>("Picture") as PictureNode;
                        node.Bitmap = bmp;
                        node.Filename = node.Id + ".jpg";
                        CreateNewUINode<UIPictureNode>(node, false);
                        
                        // Save Image // 
                        IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
                        IsolatedStorageFileStream stream = storage.CreateFile(node.Filename);
                        Extensions.SaveJpeg(writableBitmap, stream, bmp.PixelWidth, bmp.PixelHeight, 0, 70);
                        //float aspect = (float)bmp.PixelWidth / (float)bmp.PixelHeight;
                        //int width = bmp.PixelWidth < 200 ? bmp.PixelWidth : 200;
                        //Extensions.SaveJpeg(writableBitmap, stream, width, (int)(width / aspect), 0, 70);

                        stream.Close();
                        stream.Dispose();
                    }
                });
        }

        //===================================================================================================================================================//
        public static Node GetNode(string id)
        {
            for (int x = 0; x < Data.Nodes.Count; x++)
            {
                if (id == Data.Nodes[x].Id)
                {
                    return Data.Nodes[x];
                }
            }

            return null;
        }

        //===================================================================================================================================================//        
        public static void ExportAll()
        {
            // Create Dictionary //
            ExportFilenames.Clear();

            for (int i = 0; i < Data.Nodes.Count; i++)
            {
                if (Data.Nodes[i] is PageNode)
                {
                    string filename = Data.Nodes[i].Name;
                    int count = 1;

                    while (ExportFilenames.ContainsValue(filename + ".html"))
                    {
                        filename = Data.Nodes[i].Name + "_" + count++;
                    }
                    ExportFilenames.Add(Data.Nodes[i].Id, filename + ".html");
                }
            }


            // Export Pages //
            for (int i = 0; i < Data.Nodes.Count; i++)
            {
                if (Data.Nodes[i] is PageNode)
                {
                    Export(Data.Nodes[i] as PageNode);
                }
            }
        }
 
        //===================================================================================================================================================//
        public static void Export(PageNode page)
        {
            string filename = ExportFilenames[page.Id];
            string buffer = "";


            /*<html xmlns="http://www.w3.org/1999/xhtml">
            <head>
            <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
            <title>Untitled Document</title>
            <link href="Style.css" rel="stylesheet" type="text/css" />
            </head>*/


            // Html //
            buffer += "<!DOCTYPE html PUBLIC \" -//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">" + "\n";
            buffer += "<html xmlns=\"http://www.w3.org/1999/xhtml\">" + "\n";
            buffer += "<head>" + "\n";
            buffer += "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />" + "\n";
            buffer += "<meta name=\"viewport\" content=\"width=device-width; initial-scale=0.96; maximum-scale=2.0; user-scalable=1;\">";

            buffer += "<title>" + page.Name + "</title>" + "\n";
            buffer += "<link href=\"Style.css\" rel=\"stylesheet\" type=\"text/css\" />" + "\n";
            buffer += "</head>" + "\n";
            buffer += "<body>" + "\n";

            // Main //
            buffer += "<div id=\"Main\">" + "\n";

            // Centered //
            buffer += "<div id=\"Centered\">" + "\n";

            // Header //
            buffer += "\t" + "<div id=\"Header\">\n";
            buffer += "\t\t" + "<div id=\"PageName\">";
            buffer += page.Name;
            buffer += "</div>" + "\n";
            buffer += "\t" + "</div>" + "\n";

            for (int i = 0; i < page.Nodes.Count; i++)
            {
                Node node = GetNode(page.Nodes[i]);
                string name = node.Name;

                if (node is PageNode)
                {
                    string linkFilename = ExportFilenames[(node as PageNode).Id];
                    buffer += "\t" + "<a href=\"" + linkFilename + "\">" + "\n";

                    name = GetNode((node as PageNode).Id).Name;
                }

                buffer += "\t" + "<div class=\"Node\">" + "\n";

                // Icon //
                buffer += "\t\t" + "<div class=\"Icon\"></div>" + "\n";

                // Arrow //
                buffer += "\t\t" + "<div class=\"Arrow\"></div>" + "\n";




                // Name //
                buffer += "\t\t" + "<div class=\"Name\">";
                buffer += name;
                buffer += "</div>" + "\n";

                // Info //
                //buffer += "\t\t" + "<div class=\"Info\">";
                //buffer += node.Id;
                //buffer += "</div>" + "\n";

                if (node is TextNode)
                {
                    // Image //
                    buffer += "\t\t" + "<div class=\"Text\">";
                    buffer += (node as TextNode).Text;
                    buffer += "</div>" + "\n";
                }

                if (node is PictureNode)
                {
                    // Image //
                    buffer += "\t\t" + "<div class=\"Image\">";
                    buffer += "<img src=\"" + (node as PictureNode).Filename + "\">";
                    buffer += "</div>" + "\n";
                }

                buffer += "\t" + "</div>" + "\n";

                if (node is PageNode)
                    buffer += "\t" + "</a>" + "\n";

            }

            // Footer //
            buffer += "\t" + "<div id=\"Footer\"></div>" + "\n";

            buffer += "</div>" + "\n";
            buffer += "</div>" + "\n";

            buffer += "</body>" + "\n";
            buffer += "</html>" + "\n";



            Debug.WriteLine(buffer);
            Clipboard.SetText(buffer);

            // Write File //
            /*StreamWriter stream = File.CreateText(filename);
            stream.WriteLine(buffer);
            stream.Close();*/

            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream stream = storage.CreateFile(filename);

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(buffer);
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
            stream.Dispose();
        }
    }
}
