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
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;

namespace Inhuman
{
    public partial class PicturePage : PhoneApplicationPage
    {
        public CompositeTransform PictureTransform = new CompositeTransform();

        //===================================================================================================================================================//
        public PicturePage()
        {
            InitializeComponent();

            PictureControl.RenderTransform = PictureTransform;           
        }

        //===================================================================================================================================================//
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            IDictionary<string, string> queryStrings = this.NavigationContext.QueryString;

            // Load Picture //
            if (queryStrings.ContainsKey("Node"))
            {
                PictureNode node = NodeController.GetNode(queryStrings["Node"]) as PictureNode;
                DataContext = node;

                // Center Picture //

            }
        }

        //===================================================================================================================================================//
        Vector2 StartOffset;
        Vector2 StartPosition;
        float StartScaleOffset;
        float StartScale;

        Vector2 Pivot;

        int NumTouches = 0;
		void LayoutRoot_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            
        }

        //===================================================================================================================================================//
        void LayoutRoot_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            TouchCollection touches = TouchPanel.GetState();

            bool reset = false;
            if (NumTouches != touches.Count)
            {
                // Set Start Positions// 
                reset = true;

                StartOffset = new Vector2((float)PictureTransform.TranslateX, (float)PictureTransform.TranslateY);
                StartScaleOffset = (float)PictureTransform.ScaleX;
            }

            NumTouches = touches.Count;

            if (NumTouches == 1)
            {
                // Pan Only //
                if (reset)
                {
                    StartPosition = touches[0].Position;
                }

                float offsetX = touches[0].Position.X - StartPosition.X;
                float offsetY = touches[0].Position.Y - StartPosition.Y;

                PictureTransform.TranslateX = StartOffset.X + offsetX;
                PictureTransform.TranslateY = StartOffset.Y + offsetY;
            }
            else if (NumTouches == 2)
            {
                // Pan and Zoom //
                if (reset)
                {
                    Pivot = (touches[0].Position + touches[1].Position) * 0.5f - StartOffset;

                    StartPosition = (touches[0].Position + touches[1].Position) * 0.5f;
                    StartScale = (touches[0].Position - touches[1].Position).Length();
                }

                // Position //
                Vector2 offset = (touches[0].Position + touches[1].Position) * 0.5f - StartPosition;

                PictureTransform.TranslateX = StartOffset.X + offset.X;
                PictureTransform.TranslateY = StartOffset.Y + offset.Y;

                // Scale //
                float currentScale = (touches[0].Position - touches[1].Position).Length();
                float scale = currentScale / StartScale;

                PictureTransform.ScaleX = StartScaleOffset * scale;
                PictureTransform.ScaleY = StartScaleOffset * scale;

                PictureTransform.TranslateX -= (Pivot.X * (scale - 1));
                PictureTransform.TranslateY -= (Pivot.Y * (scale - 1));
            }
        }

        //===================================================================================================================================================//
        void LayoutRoot_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
        	
        }
    }
}
