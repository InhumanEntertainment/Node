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
using System.Diagnostics;

namespace Inhuman
{
    public partial class UIAudioNode : UserControl
    {
        //===================================================================================================================================================//
        public UIAudioNode()
        {
            InitializeComponent();           
        }

        //===================================================================================================================================================//
        public void SetButtonText(string text)
        {
            var button = (NodePresenter.ButtonContent as Button);
            button.Content = text;
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
                    SetButtonText("Finish");
                }
                else
                {
                    node.StopRecording();
                    SetButtonText("Play");
                }
            }
            else
            {
                node.Play();
            }
        }
    }
}
