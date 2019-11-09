using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AnnotationControl
{
    public sealed class AnnotationBox : Decorator
    {
        public AnnotationBox(string text, FlowDirection dir)
        {
            _scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
                Margin = new Thickness(3)
            };

            _textViewer = new TextCanvas(_scrollViewer)
            {
                TextDirection = dir,
                TextAlign = TextAlignment.Justify
            };
            
            _scrollViewer.Content = _textViewer;

            CornerRadius = 8;
            BubblePeakWidth = 16;
            BubblePeakHeight = 10;
            BubblePeakPosition = new Point(CornerRadius + BubblePeakWidth, 0);
            Padding = new Thickness(5, 5, 5, 5);
            BorderThickness = new Thickness(1);
            BorderBrush = Brushes.Teal;
            Foreground = Brushes.Teal;
            Background = new SolidColorBrush(Colors.Bisque) { Opacity = 0.97 };
            FontSize = 16;
            FontFamily = new FontFamily("Arial");
            Text = text;

            Child = _scrollViewer;
        }


        private readonly TextCanvas _textViewer;
        private readonly ScrollViewer _scrollViewer;
        public double CornerRadius { get; set; }
        public double BubblePeakWidth { get; set; }
        public double BubblePeakHeight { get; set; }
        public Point BubblePeakPosition { get; set; }
        public Brush BorderBrush { get; set; }
        public Brush Background { get; set; }
        public Thickness BorderThickness { get; set; }
        public Thickness Padding
        {
            get => _scrollViewer.Padding;
            set
            {
                _textViewer.Padding = new Thickness(
                    value.Left + _scrollViewer.Margin.Left, 
                    value.Top + _scrollViewer.Margin.Top,
                    value.Right + _scrollViewer.Margin.Right,
                    value.Bottom + _scrollViewer.Margin.Bottom);

                _scrollViewer.Padding = value;
            }
        }

        public Brush Foreground
        {
            get => _textViewer.Foreground;
            set => _textViewer.Foreground = value;
        }
        public TextAlignment TextAlign
        {
            get => _textViewer.TextAlign;
            set => _textViewer.TextAlign = value;
        }
        public string Text
        {
            get => _textViewer.Text;
            set => _textViewer.Text = value;
        }
        public FontFamily FontFamily
        {
            get => _textViewer.FontFamily;
            set => _textViewer.FontFamily = value;
        }
        public double FontSize
        {
            get => _textViewer.FontSize;
            set => _textViewer.FontSize = value;
        }

        //protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        //{
        //    base.OnPropertyChanged(e);

        //    if (e.OldValue != e.NewValue)
        //    {
        //        var prop = _textViewer?.GetType().GetProperty(e.Property.Name);
        //        if (prop?.CanWrite == true && prop.CanRead)
        //        {
        //            prop.SetValue(_textViewer, e.NewValue);
        //        }
        //    }
        //}


        protected override void OnRender(DrawingContext dc)
        {
            //                        Width
            //            <------------------------->  
            //    ^                     d = BubblePeakPosition
            //    |                    / \
            //    |        b____2____c/3 4\e___5___f
            // H  |       (1                       6)
            // E  |       a   TTTTTTTTTTTTTTTTTT    g
            // I  |       |   TT              TT    |
            // G  |       |   TT     TEXT     TT    |
            // H  |     11|   TT              TT    |7
            // T  |       |   TT              TT    |
            //    |       |   TTTTTTTTTTTTTTTTTT    |
            //    |       k                         h
            //   _|_    10(j___________9___________i)8
            //
            var a = new Point(0, CornerRadius);
            var b = new Point(CornerRadius, 0);
            var c = new Point(BubblePeakPosition.X - BubblePeakWidth / 2, 0);
            var d = new Point(BubblePeakPosition.X, -CornerRadius);
            var e = new Point(BubblePeakPosition.X + BubblePeakWidth / 2, 0);
            var f = new Point(ActualWidth - CornerRadius, 0);
            var g = new Point(ActualWidth, 10);
            var h = new Point(ActualWidth, ActualHeight - CornerRadius);
            var i = new Point(ActualWidth - CornerRadius, ActualHeight);
            var j = new Point(CornerRadius, ActualHeight);
            var k = new Point(0, ActualHeight - CornerRadius);

            var pathSegments = new List<PathSegment>
            {
                new ArcSegment(b, new Size(CornerRadius, CornerRadius), 0, false, SweepDirection.Clockwise, true),
                new LineSegment(c, true),
                new LineSegment(d, true),
                new LineSegment(e, true),
                new LineSegment(f, true),
                new ArcSegment(g, new Size(CornerRadius, CornerRadius), 0, false, SweepDirection.Clockwise, true),
                new LineSegment(h, true),
                new ArcSegment(i, new Size(CornerRadius, CornerRadius), 0, false, SweepDirection.Clockwise, true),
                new LineSegment(j, true),
                new ArcSegment(k, new Size(CornerRadius, CornerRadius), 0, false, SweepDirection.Clockwise, true),
                new LineSegment(a, true)
            };

            var pthFigure = new PathFigure(a, pathSegments, false) { IsFilled = true };
            //var transform = BubblePeakPosition.Y > 0 ? new ScaleTransform(1, -1, ActualWidth / 2, ActualHeight / 2) : null; // rotate around x axis 
            var pthGeometry = new PathGeometry(new List<PathFigure> { pthFigure }, FillRule.EvenOdd, null);
            dc.DrawGeometry(Background, new Pen(BorderBrush, BorderThickness.Right), pthGeometry);
            _textViewer.InvalidateVisual();
        }



        private class TextCanvas : Canvas
        {
            public TextCanvas(ScrollViewer parent)
            {
                Container = parent;
                ClipToBounds = true;
                Background = Brushes.Transparent;
            }


            public FlowDirection TextDirection { get; set; }
            public Brush Foreground { get; set; }
            public FontFamily FontFamily { get; set; }
            public double FontSize { get; set; }
            public TextAlignment TextAlign { get; set; }
            public string Text { get; set; }
            public double ScrollBarWidth { get; set; } = 12;
            public Thickness Padding { get; set; }
            public ScrollViewer Container { get; set; }


            protected override void OnRender(DrawingContext dc)
            {
                if (Container?.ActualWidth > 0)
                {
                    var ft = new FormattedText(Text, CultureInfo.CurrentCulture, TextDirection,
                        new Typeface(FontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal),
                        FontSize, Foreground, new NumberSubstitution(), VisualTreeHelper.GetDpi(this).PixelsPerDip)
                    {
                        LineHeight = FontSize,
                        TextAlignment = TextAlign,
                        MaxTextWidth = Container.ActualWidth - Padding.Left - Padding.Right
                    };

                    if (ft.Height > Container.ActualHeight - Padding.Top - Padding.Bottom) // when scrollbar visible
                    {
                        ft.MaxTextWidth = Container.ActualWidth - Padding.Left - Padding.Right - ScrollBarWidth;
                    }
                    // Note set parent height from here, when the text height is less than parent height
                    Height = Math.Max(ft.Height + Padding.Top + Padding.Bottom, Container.ActualHeight);

                    dc.DrawText(ft, new Point(0, 0));
                }
            }
        }
    }
}
