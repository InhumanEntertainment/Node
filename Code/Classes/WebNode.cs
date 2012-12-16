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
using System.Collections;
using System.Collections.Generic;
using Microsoft.Phone.Tasks;

namespace Inhuman
{
    public class WebNode : Node
    {
        public string Url = "http://www.inhumanize.com";

        //===================================================================================================================================================//
        public void Open()
        {
            WebBrowserTask task = new WebBrowserTask();
            task.Uri = new Uri(Url, UriKind.Absolute);
            task.Show();
        }
    }
}
