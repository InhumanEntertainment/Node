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
    public class MatchNode : PageNode
    {
        [Flags]
        public enum Types
        {
            Text = 1,
            Picture = 2,
            Audio = 4,
            Task = 8,
            Link = 16,
            Page = 32
        }

        //===================================================================================================================================================//
        public List<TagNode> Tags = new List<TagNode>();

        //===================================================================================================================================================//
        public void GetMatches()
        {

        }
    }
}
