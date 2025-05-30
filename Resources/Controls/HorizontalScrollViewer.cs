﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CSTracker.Resources.Controls
{
    public class HorizontalScrollViewer : ScrollViewer
    {
        private Point _scrollMousePoint;
        private double _hOff = 1;

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            if(ScrollableWidth > 0)
            {
                Cursor = Cursors.Hand;
            } else
            {
                Cursor = Cursors.Arrow;
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (ScrollableWidth > 0)
            {
                _scrollMousePoint = e.GetPosition(this);
                _hOff = HorizontalOffset;
                CaptureMouse();
            }
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            if (IsMouseCaptured)
            {
                var newOffset = _hOff + (_scrollMousePoint.X - e.GetPosition(this).X);
                ScrollToHorizontalOffset(newOffset);
            }
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);

            ReleaseMouseCapture();
        }
    }
}
