using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Live;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.Diagnostics;
using System.IO;

namespace Inhuman
{
    public class Skydrive
    {
        public LiveAuthClient Auth;
        public LiveConnectClient Client;
        public List<string> Scopes = new List<string>() { "wl.signin", "wl.offline_access", "wl.basic", "wl.skydrive_update"  };
        public string NodeFolder; 

        public bool isConnected
        {
            get
            {
                if (Auth != null && Auth.Session != null)
	                return true;
                else
                    return false;
            }
        }
        public bool NodeFolderExists = false;
        public List<string> SyncedFiles;

        //=====================================================================================================================================================//
        public Skydrive()
        {
            Connect();
        }

        //=====================================================================================================================================================//
        public void Connect()
        {            
            Auth = new LiveAuthClient("00000000400E9840");
            Auth.InitializeCompleted +=new EventHandler<LoginCompletedEventArgs>(Initialize_Completed);
            Auth.InitializeAsync(Scopes);
        }

        //=====================================================================================================================================================//
        void Initialize_Completed(object sender, LoginCompletedEventArgs e)
        {
 	        if (e.Error == null)
            {
                if (e.Status == LiveConnectSessionStatus.Connected)
                {
                    Client = new LiveConnectClient(e.Session);
                    //Client.GetCompleted += new EventHandler<LiveOperationCompletedEventArgs>(Download_Completed);
                    Client.UploadCompleted += new EventHandler<LiveOperationCompletedEventArgs>(Upload_Completed);                      
                    CheckFolderExists("me/skydrive/files");               
                }
                else if (e.Status == LiveConnectSessionStatus.Unknown)
                {
                    Auth.LoginCompleted += new EventHandler<LoginCompletedEventArgs>(Login_Completed);
                    Auth.LoginAsync(Scopes);
                }                              
            }
            else
            {
                MessageBox.Show(e.Error.Message);
            }
        }

        //=====================================================================================================================================================//
        void Login_Completed(object sender, LoginCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Status == LiveConnectSessionStatus.Connected)
                {
                    Client = new LiveConnectClient(e.Session);
                    //Client.GetCompleted += new EventHandler<LiveOperationCompletedEventArgs>(Download_Completed);
                    Client.UploadCompleted += new EventHandler<LiveOperationCompletedEventArgs>(Upload_Completed);                               
                }                             
            }
            else
            {
                MessageBox.Show(e.Error.Message);
            }
        }

        //=====================================================================================================================================================//
        public void CheckFolderExists(string folder)
        {
            if (isConnected)
	        {
		         Client.GetAsync(folder);
                 Client.GetCompleted += new EventHandler<LiveOperationCompletedEventArgs>(CheckFolderExists_Completed);
	        }           
        }
        void CheckFolderExists_Completed(object sender, LiveOperationCompletedEventArgs e)
        {
            Client.GetCompleted -= new EventHandler<LiveOperationCompletedEventArgs>(CheckFolderExists_Completed);

            if (e.Error == null)
            {
                List<object> files = (List<object>)e.Result["data"];

                foreach (var file in files)
                {
                    IDictionary<string, object> dict = file as IDictionary<string, object>;
                    string name = (string)dict["name"];

                    if (name == "Node")
                    {
                        NodeFolder = (string)dict["id"];
                        GetFileList();
                    }
                }
            }
        }

        //=====================================================================================================================================================//
        public void GetFileList()
        {
            if (isConnected)
            {
                Client.GetAsync(NodeFolder + "/files");
                Client.GetCompleted += new EventHandler<LiveOperationCompletedEventArgs>(GetFileList_Completed);
            }
        }
        void GetFileList_Completed(object sender, LiveOperationCompletedEventArgs e)
        {
            Client.GetCompleted -= new EventHandler<LiveOperationCompletedEventArgs>(GetFileList_Completed);
            SyncedFiles = new List<string>();

            if (e.Error == null)
            {
                List<object> files = (List<object>)e.Result["data"];

                foreach (var file in files)
                {
                    IDictionary<string, object> dict = file as IDictionary<string, object>;
                    string name = (string)dict["name"];

                    if (name != "__ApplicationSettings")
                    {
                        SyncedFiles.Add(name);
                    }
                }
                Debug.WriteLine("Got Skydrive File List");
            }
        }

        //=====================================================================================================================================================//
        public void CreateFolder(string path, string name)
        {
            try
            {
                var folderData = new Dictionary<string, object>();
                folderData.Add("name", name);

                Client.PostCompleted += new EventHandler<LiveOperationCompletedEventArgs>(CreateFolder_Completed);
                Client.PostAsync("me/skydrive/" + path, folderData);
            }
            catch (LiveConnectException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        void CreateFolder_Completed(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                MessageBox.Show("Folder Created");
            }
            else
            {
                MessageBox.Show(e.Error.Message);
            }            
        }

        //=====================================================================================================================================================//
        public void UploadFile(string filename)
        {
            if (isConnected)
            {              
                try
                {
                    if (NodeFolder != "")
                    {
                        IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();

                        if (storage.FileExists(filename))
                        {
                            IsolatedStorageFileStream stream = storage.OpenFile(filename, FileMode.Open);
                            Client.UploadAsync(NodeFolder, filename, stream, OverwriteOption.Overwrite);
                        }
                        else
                            MessageBox.Show("File Doesn't Exist");
                    }                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        void Upload_Completed(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Debug.WriteLine("Uploaded: " + (string)e.Result["name"]);
            }                                
        }

        //=====================================================================================================================================================//
        void Download_Completed(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string result = e.RawResult;

                List<object> fileList = e.Result["data"] as List<object>;
                foreach (var file in fileList)
                {
                    IDictionary<string, object> File = file as IDictionary<string, object>;
                    MessageBox.Show(File["name"] as string);
                }
            }
            else
            {
                MessageBox.Show(e.Error.Message);
            }
        }

        //=====================================================================================================================================================//
        public void UploadAll()
        {
            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();

            string[] files = store.GetFileNames();

            foreach (string file in files)
            {
                if (file != "__ApplicationSettings")
                {
                    string extension = System.IO.Path.GetExtension(file);

                    if (SyncedFiles == null || !SyncedFiles.Contains(file) || extension == ".xml")
                    {
                        Debug.WriteLine("Uploading: " + file);
                        UploadFile(file);
                    }
                    else 
                    {
                        //Debug.WriteLine("Skipped: " + file);                        
                    }
                }                
            }
        }

    }
}
