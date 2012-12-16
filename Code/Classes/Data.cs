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
    [   System.Xml.Serialization.XmlInclude(typeof(PageNode)), System.Xml.Serialization.XmlInclude(typeof(LinkNode)), 
        System.Xml.Serialization.XmlInclude(typeof(WebNode)), System.Xml.Serialization.XmlInclude(typeof(ImageNode)),
        System.Xml.Serialization.XmlInclude(typeof(TaskNode))
    ]       
    public class StreamlineData
    {
        public static string Filename = "Streamline.xml";
        public ObservableCollection<Node> Nodes = new ObservableCollection<Node>();
        public string CurrentPage;

        [System.Xml.Serialization.XmlIgnore]
        public PageNode CurrentPageNode
        {
            get
            {
                return GetNode(CurrentPage) as PageNode;
            }
        }

        //===================================================================================================================================================//
        public StreamlineData()
        {
        }

        //===================================================================================================================================================//
        public Node GetNode(string id)
        {
            for (int x = 0; x < Nodes.Count; x++)
            {
                if (id == Nodes[x].Id)
                {
                    return Nodes[x];
                }
            }

            return null;
        }

        //===================================================================================================================================================//
        public static StreamlineData Load()
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            StreamlineData data;

            try
            {
                if (storage.FileExists(Filename))
                {
                    IsolatedStorageFileStream stream = storage.OpenFile(Filename, FileMode.Open);
                    XmlSerializer xml = new XmlSerializer(typeof(StreamlineData));
                    data = xml.Deserialize(stream) as StreamlineData;

                    // Debug XML //
                    byte[] buffer = new byte[32000];
                    stream.Position = 0;
                    stream.Read(buffer, 0, (int)stream.Length);
                    string stuff = System.Text.Encoding.UTF8.GetString(buffer, 0, (int)stream.Length);
                    Debug.WriteLine("=======================================================================================");
                    Debug.WriteLine("  Load XML");
                    Debug.WriteLine("=======================================================================================");
                    Debug.WriteLine(stuff);

                    stream.Close();
                    stream.Dispose();
                }
                else
                    data = new StreamlineData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
                data = new StreamlineData();
            }

            return data;
        }

        //===================================================================================================================================================//
        public void Save()
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream stream = storage.CreateFile(Filename);
            XmlSerializer xml = new XmlSerializer(GetType());
            xml.Serialize(stream, this);

            // Debug XML //
            byte[] buffer = new byte[32000];
            stream.Position = 0;
            stream.Read(buffer, 0, (int)stream.Length);
            string stuff = System.Text.Encoding.UTF8.GetString(buffer, 0, (int)stream.Length);
            Debug.WriteLine("=======================================================================================");
            Debug.WriteLine("  Save XML");
            Debug.WriteLine("=======================================================================================");
            Debug.WriteLine(stuff);


            stream.Close();
            stream.Dispose();
        }

        //===================================================================================================================================================//
        public void Reset()
        {
            Nodes.Clear();
            PageNode page = new PageNode() { Name = "Default" };
            CurrentPage = page.Id;
            Nodes.Add(page);
        }

        //===================================================================================================================================================//
        [System.Xml.Serialization.XmlIgnore]
        public Dictionary<string, string> ExportFilenames = new Dictionary<string, string>();

        public void ExportAll()
        {
            // Create Dictionary //
            ExportFilenames.Clear();

            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i] is PageNode)
                {
                    string filename = Nodes[i].Name;
                    int count = 1;

                    while (ExportFilenames.ContainsValue(filename + ".html"))
                    {
                        filename = Nodes[i].Name + "_" + count++;
                    }
                    ExportFilenames.Add(Nodes[i].Id, filename + ".html");
                }
            }


            // Export Pages //
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i] is PageNode)
                {
                    Export(Nodes[i] as PageNode);
                }
            }
        }
        
        //===================================================================================================================================================//
        public void Export(PageNode page)
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

                if (node is ImageNode)
                {
                    // Image //
                    buffer += "\t\t" + "<div class=\"Image\">";
                    buffer += "<img src=\"" + (node as ImageNode).Filename + "\">";
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
