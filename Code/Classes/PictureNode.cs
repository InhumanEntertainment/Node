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
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Inhuman
{
    public class PictureNode : Node
    {
        private BitmapImage _bitmap;

        [System.Xml.Serialization.XmlIgnore]
        public BitmapImage Bitmap
        {
            get
            {
                return _bitmap;
            }
            set
            {
                if (value != _bitmap)
                {
                    _bitmap = value;
                    NotifyPropertyChanged("Bitmap");
                }
            }
        }

        string _Filename;
        public string Filename
        {
            get
            {
                return _Filename;
            }
            set
            {
                if (value != _Filename)
                {
                    _Filename = value;
                    NotifyPropertyChanged("Filename");
                }
            }
        }

        //===================================================================================================================================================//
        public PictureNode()
        {            
        }

        //===================================================================================================================================================//
        public void LoadBitmap()
        {
            if (Bitmap == null && Filename != "")
            {
                IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();

                if (storage.FileExists(Filename))
                {
                    // Open file //
                    IsolatedStorageFileStream stream = storage.OpenFile(Filename, FileMode.Open);

                    //Stream to Bitmap//
                    BitmapImage bmp = new BitmapImage();
                    bmp.SetSource(stream);

                    Bitmap = bmp;
					
                    Info = "Picture - " + bmp.PixelWidth + "x" + bmp.PixelHeight;
                }
            }
        }
    }
}
