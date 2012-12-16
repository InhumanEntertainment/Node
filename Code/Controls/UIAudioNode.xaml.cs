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
using Microsoft.Xna.Framework.Audio;

namespace Inhuman
{
    public partial class UIAudioNode : UserControl
    {
        public string ActionText
        {
            set
            {
                ActionButton.Content = value;
            }
        }

        public UIAudioNode()
        {
            InitializeComponent();
            ActionButton.Content = "Play";
        }

        //===================================================================================================================================================//
        void ActionButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AudioNode node = (DataContext as AudioNode);
            Microphone mic = Microphone.Default;

            if (node.Sound == null)
            {
                if (mic.State == MicrophoneState.Stopped)
                {
                    node.Record();
                    ActionButton.Content = "Finish";
                }
                else
                {
                    node.StopRecording();
                    ActionButton.Content = "Play";
                }
            }
            else
            {
                node.Play();
            }        	
        }

        //===================================================================================================================================================//
        void NameText_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
        	NameText.SelectAll();
        }
    }
}
