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

namespace Inhuman
{
    public partial class UITextNode : UserControl
    {
        public UITextNode()
        {
            InitializeComponent();
        }

        private void NameText_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
        	//NameText.SelectAll();
        }

        void TextDataBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
        	

        }
    }
}
