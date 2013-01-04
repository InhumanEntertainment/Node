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
        //===================================================================================================================================================//
        public UITextNode()
        {
            InitializeComponent();
        }

        //===================================================================================================================================================//
        void TextDataBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            object item = NodeController.UI.MainListBox.SelectedIndex < NodeController.UI.MainListBox.Items.Count - 1 ? NodeController.UI.MainListBox.Items[NodeController.UI.MainListBox.SelectedIndex + 1] : NodeController.UI.MainListBox.Items[NodeController.UI.MainListBox.SelectedIndex];
            //NodeController.UI.MainListBox.ScrollIntoView(item);
            //NodeController.UI.MainListBox.ScrollIntoView(
        }

        void TextDataBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            var textbox = NodeObject.LowerContent as TextBox;
            textbox.SelectAll();
        }
    }
}
