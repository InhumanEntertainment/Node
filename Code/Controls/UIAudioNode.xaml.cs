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
using System.Windows.Media.Imaging;

namespace Inhuman
{
    public enum AudioMode {Play, Stop, Record}

    public partial class UIAudioNode : UIControl
    {
        BitmapImage PlayImage;
        BitmapImage StopImage;
        BitmapImage RecordImage;
        
        //===================================================================================================================================================//
        public UIAudioNode()
        {
            InitializeComponent();

            PlayImage = new BitmapImage(new Uri("/Node;component/Art/Play.png", UriKind.Relative));
            StopImage = new BitmapImage(new Uri("/Node;component/Art/Stop.png", UriKind.Relative));
            RecordImage = new BitmapImage(new Uri("/Node;component/Art/Record.png", UriKind.Relative));            
        }

        //===================================================================================================================================================//
        public override void Initialize(bool autoedit)
        {
            (Node as AudioNode).LoadSound();
            if ((Node as AudioNode).Filename == null)
                SetButtonText(AudioMode.Record);
        }

        //===================================================================================================================================================//
        public void SetButtonText(AudioMode mode)
        {
            var button = (NodeObject.ButtonContent as Button);
            Image image = button.Content as Image;

            if (mode == AudioMode.Play)
                image.Source = PlayImage;
            else if (mode == AudioMode.Stop)
                image.Source = StopImage;
            else if (mode == AudioMode.Record)
                image.Source = RecordImage;          
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
                    SetButtonText(AudioMode.Stop);

                    node.UpdateTime();
                }
                else
                {
                    node.StopRecording();
                    SetButtonText(AudioMode.Play);
                }
            }
            else
            {
                node.Play();
            }
        }

        //===================================================================================================================================================//
        public override void SetHitTest(bool value)
        {
            NodeObject.IsHitTestVisible = value;
        }
    }
}
