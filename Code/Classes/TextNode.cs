﻿using System;
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

namespace Inhuman
{
    public class TextNode : Node
    {
        private string _text = "Enter text here...";
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (value != _text)
                {
                    _text = value;
                    NotifyPropertyChanged("Text");
                    UpdateTime();
                }
            }
        }
    }
}
