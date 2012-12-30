using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace Inhuman
{
	public partial class UIThumbnail : UserControl
	{
		//===================================================================================================================================================//
        public UIThumbnail()
		{
			InitializeComponent();
		}

		//===================================================================================================================================================//
        void LayoutRoot_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			Node node = (DataContext as PictureNode);
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Pages/PicturePage.xaml?Node=" + node.Id, UriKind.Relative)); 
		}
	}
}