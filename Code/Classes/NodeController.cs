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

        
	    //===================================================================================================================================================//
        public static void Initialize(StreamlineData data, MainPage ui)
        {
            Data = data;
            UI = ui;
        }

        //===================================================================================================================================================//
        public static void Reset()
        {
            PageHistory.Clear();

            Data.Reset();
            LoadPage(CurrentPageNode);
        }

        //===================================================================================================================================================//
        public static void PreviousPage()
        {
            if (PageHistory.Count > 1)
            {
                PageHistory.Pop();
                LoadPage(PageHistory.Pop());
                PrintHistory();
            }
        }

        //===================================================================================================================================================//
        public static void CreatePage()
        {
            // Create Link Node //
            //LinkNode link = new LinkNode();            
            //Data.Nodes.Add(link);
            //link.Url = page.Id;
            //uilink.DataContext = GetNode(link.Url);
            //uilink.Tag = link;

            // Create New Page //
            PageNode page = new PageNode();
            page.Name = "Page";
            Data.Nodes.Add(page);
            CurrentPageNode.AddNode(page);

            UILinkNode uilink = new UILinkNode();
            UI.MainListBox.Items.Add(uilink);
            uilink.DataContext = page;            
        }

        //===================================================================================================================================================//
        public static void CreateProject()
        {
            // Create New Page //
            ProjectNode page = new ProjectNode();
            page.Name = "Project";
            Data.Nodes.Add(page);
            CurrentPageNode.AddNode(page);

            UIProjectNode uinode = new UIProjectNode();
            UI.MainListBox.Items.Add(uinode);
            uinode.DataContext = page;

            uinode.Initialize();
        }

        //===================================================================================================================================================//
        public static void CreateGallery()
        {
            // Create New Page //
            GalleryNode page = new GalleryNode();
            page.Name = "Gallery";
            Data.Nodes.Add(page);
            CurrentPageNode.AddNode(page);

            UIGalleryNode uinode = new UIGalleryNode();
            UI.MainListBox.Items.Add(uinode);
            uinode.DataContext = page;

            uinode.Initialize();
        }

        //===================================================================================================================================================//
        public static void CreateNode()
        {
            Node node = new Node("New Node");
            UINode uinode = new UINode();
            UI.MainListBox.Items.Add(uinode);
            uinode.DataContext = node;

            Data.Nodes.Add(node);
            CurrentPageNode.AddNode(node);
        }

        //===================================================================================================================================================//
        public static UITaskNode CreateTask()
        {
            TaskNode node = new TaskNode();
            node.Name = "Task";
            UITaskNode uinode = new UITaskNode();
            UI.MainListBox.Items.Add(uinode);
            uinode.DataContext = node;

            Data.Nodes.Add(node);
            CurrentPageNode.AddNode(node);

            return uinode;
        }

        //===================================================================================================================================================//
        public static UINode CreateLink()
        {
            WebNode node = new WebNode() { Name = "Web Link" };
            UINode uinode = new UINode();
            UI.MainListBox.Items.Add(uinode);
            uinode.DataContext = node;

            Data.Nodes.Add(node);
            CurrentPageNode.AddNode(node);

            return uinode;
        }

        //===================================================================================================================================================//
        public static UIAudioNode CreateAudio()
        {
            AudioNode node = new AudioNode();
            node.Name = "Audio";
            UIAudioNode uinode = new UIAudioNode();
            UI.MainListBox.Items.Add(uinode);
            uinode.DataContext = node;
            uinode.SetButtonText("Record");

            Data.Nodes.Add(node);
            CurrentPageNode.AddNode(node);

            return uinode;
        }

        //===================================================================================================================================================//
        public static UITextNode CreateText()
        {
            TextNode node = new TextNode();
            node.Name = "Text";
            UITextNode uinode = new UITextNode();
            UI.MainListBox.Items.Add(uinode);
            uinode.DataContext = node;

            Data.Nodes.Add(node);
            CurrentPageNode.AddNode(node);

            return uinode;
        }

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
                UI.MainListBox.Items.Clear();
                UI.ScrollView.ScrollToVerticalOffset(0);
                UI.DataContext = page;
                if (PageHistory.Count == 0 || (PageHistory.Count > 0 && PageHistory.Peek().Id != page.Id))
                {
                    PageHistory.Push(page);
                    PrintHistory();                   
                }
                Data.CurrentPage = page.Id;

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
        public static void CreateUINode(Node node)
        {
            if (node is ProjectNode)
            {
                UIProjectNode uinode = new UIProjectNode();
                UI.MainListBox.Items.Add(uinode);
                uinode.DataContext = node;
                uinode.Initialize();
            }
            else if (node is GalleryNode)
            {
                UIGalleryNode uinode = new UIGalleryNode();
                UI.MainListBox.Items.Add(uinode);
                uinode.DataContext = node;
                uinode.Initialize();
            } 
            else if (node is PageNode)
            {
                UILinkNode uilink = new UILinkNode();
                UI.MainListBox.Items.Add(uilink);
                uilink.DataContext = node;
            }           
            else if (node is PictureNode)
            {
                UIPictureNode uinode = new UIPictureNode();
                (node as PictureNode).LoadBitmap();
                UI.MainListBox.Items.Add(uinode);
                uinode.DataContext = node;
            }
            else if (node is TaskNode)
            {
                UITaskNode uinode = new UITaskNode();
                UI.MainListBox.Items.Add(uinode);
                uinode.DataContext = node;
            }
            else if (node is AudioNode)
            {
                UIAudioNode uilink = new UIAudioNode();
                (node as AudioNode).LoadSound();
                UI.MainListBox.Items.Add(uilink);
                uilink.DataContext = node;
            }
            else if (node is TextNode)
            {
                UITextNode uinode = new UITextNode();
                UI.MainListBox.Items.Add(uinode);
                uinode.DataContext = node;
            }
            else
            {
                UINode uinode = new UINode();
                UI.MainListBox.Items.Add(uinode);
                uinode.DataContext = node;
            }
        }

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
                    //BackgroundImage = new Uri("Background.png", UriKind.Relative),
                    BackContent = NodeController.CurrentPageNode.Nodes.Count + " Nodes"

                };
                ShellTile.Create(new Uri("/MainPage.xaml?" + tileParameter, UriKind.Relative), secondaryTile); // Pass tileParameter as QueryString 
            }
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
                        PictureNode node = new PictureNode();
                        UIPictureNode uinode = new UIPictureNode();
                        node.Bitmap = bmp;
                        node.Name = "Picture";
                        node.Filename = node.Id + ".jpg";
                        UI.MainListBox.Items.Add(uinode);
                        uinode.DataContext = node;

                        Data.Nodes.Add(node);
                        CurrentPageNode.AddNode(node);

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

                if (node is LinkNode)
                {
                    string linkFilename = ExportFilenames[(node as LinkNode).Url];
                    buffer += "\t" + "<a href=\"" + linkFilename + "\">" + "\n";

                    name = GetNode((node as LinkNode).Url).Name;
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

                if (node is LinkNode)
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
