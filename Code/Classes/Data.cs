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
        System.Xml.Serialization.XmlInclude(typeof(WebNode)), System.Xml.Serialization.XmlInclude(typeof(PictureNode)),
        System.Xml.Serialization.XmlInclude(typeof(TaskNode)), System.Xml.Serialization.XmlInclude(typeof(AudioNode)),
        System.Xml.Serialization.XmlInclude(typeof(TextNode)), System.Xml.Serialization.XmlInclude(typeof(ProjectNode)),
        System.Xml.Serialization.XmlInclude(typeof(GalleryNode))
    ]       
    public class StreamlineData
    {
        public static string Filename = "Streamline.xml";
        public ObservableCollection<Node> Nodes = new ObservableCollection<Node>();
        public string CurrentPage;

        //===================================================================================================================================================//
        public StreamlineData()
        {
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
    }
}
