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
using System.Windows.Shapes;
using Microsoft.Phone.Tasks;

namespace Inhuman
{
    public partial class UILinkNode : UIControl
    {
        //===================================================================================================================================================//
        public UILinkNode()
        {
            InitializeComponent();
        }

        //===================================================================================================================================================//
        public void Initialize()
        {
        }

        //===================================================================================================================================================//
        public override void SetHitTest(bool value)
        {
            NodeObject.IsHitTestVisible = value;
        }

        //===================================================================================================================================================//
        void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            LinkNode node = Node as LinkNode;

            if (node.Url != "")
            {
                WebBrowserTask task = new WebBrowserTask();
                task.Uri = new Uri(node.Url, UriKind.Absolute);
                task.Show();               
            }
        }
    }
}
