using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPF.Core
{
    public class AnnotationBox : ScrollViewer
    {
        public AnnotationBox()
        {
            TextViewer = new TextCanvas();
            Content = TextViewer;

            Padding = new Thickness(5, 5, 5, 5);
            BorderThickness = new Thickness(1);
            CornerRadius = 10;
            BubblePeakWidth = 16;
            BorderBrush = Brushes.Teal;
            BubblePeakPosition = new Point(CornerRadius + BubblePeakWidth, 0);
            Foreground = Brushes.Teal;
            Background = new SolidColorBrush(Colors.Bisque) { Opacity = 0.97 };
            base.Background = Brushes.Transparent;
            FontSize = 16;
            FontFamily = new FontFamily("Arial");
            TextViewer.TextDirection = FlowDirection.RightToLeft;
            TextViewer.TextAlign = TextAlignment.Justify;
        }


        private TextCanvas TextViewer { get; set; }

        public static readonly DependencyProperty BubblePeakWidthProperty = DependencyProperty.Register(nameof(BubblePeakWidth), typeof(double), typeof(Annotation), new PropertyMetadata(default(double)));
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(double), typeof(Annotation), new PropertyMetadata(default(double)));
        public static readonly DependencyProperty BubblePeakPositionProperty = DependencyProperty.Register(nameof(BubblePeakPosition), typeof(Point), typeof(Annotation), new PropertyMetadata(default(Point)));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(Annotation), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty TextAlignProperty = DependencyProperty.Register(nameof(TextAlign), typeof(TextAlignment), typeof(Annotation), new PropertyMetadata(default(TextAlignment)));


        public new Brush Background { get; set; }

        public TextAlignment TextAlign
        {
            get => (TextAlignment)GetValue(TextAlignProperty);
            set => SetValue(TextAlignProperty, value);
        }
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set
            {
                SetValue(TextProperty, value);
                TextViewer.Text = Text;
            }
        }

        public Point BubblePeakPosition
        {
            get => (Point)GetValue(BubblePeakPositionProperty);
            set => SetValue(BubblePeakPositionProperty, value);
        }
        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public double BubblePeakWidth
        {
            get => (double)GetValue(BubblePeakWidthProperty);
            set => SetValue(BubblePeakWidthProperty, value);
        }


        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.OldValue != e.NewValue)
            {
                var prop = TextViewer?.GetType().GetProperty(e.Property.Name);
                if (prop?.CanWrite == true && prop.CanRead == true)
                {
                    prop.SetValue(TextViewer, e.NewValue);
                }
            }
        }


        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            //
            //                  d
            //                 / \
            //     b____2____c/3 4\e___5___f
            //    (1                       6)
            //    a                         g
            //    |                         |
            //    |                         |
            //  11|                         |7
            //    |                         |
            //    |                         |
            //    k                         h
            //  10(j___________9___________i)8
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
        }
    }
}
