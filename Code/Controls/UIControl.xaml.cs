using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace Inhuman
{
	public partial class UIControl : UserControl
	{
        public Node Node
        {
            get
            {
                if (DataContext is Node)
                    return (DataContext as Node);
                else
                    return null;               
            }
        }

        //===================================================================================================================================================//
        public UIControl()
		{

		}

        //===================================================================================================================================================//
        public virtual void Initialize(bool autoedit)
        {

        }
        
        //===================================================================================================================================================//
        public virtual void SetHitTest(bool value)
        {

        }
	}
}