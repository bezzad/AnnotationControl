using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPF.Core
{
    public class Annotation : Decorator
    {
        private readonly Pen _pen;

        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(nameof(Padding), typeof(double), typeof(Annotation), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register(
            nameof(BorderThickness), typeof(double), typeof(Annotation), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register(
            nameof(BorderBrush), typeof(Brush), typeof(Annotation), new PropertyMetadata(default(Brush)));

        public Brush BorderBrush
        {
            get => (Brush)GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }

        public double BorderThickness
        {
            get => (double)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }
        public double Padding
        {
            get => (double)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }
        public Point BubblePeakPosition { get; set; }

        public Annotation()
        {
            BubblePeakPosition = new Point(ActualWidth / 2, 0);
            BorderBrush = new SolidColorBrush(Colors.Bisque) { Opacity = 0.97 };
            BorderThickness = 1;
            _pen = new Pen(Brushes.Teal, BorderThickness);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var arrowRadius = 8;
            var cornerRadius = 10;

            var a = new Point(0, cornerRadius);
            var b = new Point(cornerRadius, 0);
            var c = new Point(BubblePeakPosition.X - arrowRadius, 0);
            var d = new Point(BubblePeakPosition.X, -cornerRadius);
            var e = new Point(BubblePeakPosition.X + arrowRadius, 0);
            var f = new Point(ActualWidth - cornerRadius, 0);
            var g = new Point(ActualWidth, 10);
            var h = new Point(ActualWidth, ActualHeight - cornerRadius);
            var i = new Point(ActualWidth - cornerRadius, ActualHeight);
            var j = new Point(cornerRadius, ActualHeight);
            var k = new Point(0, ActualHeight - cornerRadius);

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
            var pathSegments = new List<PathSegment>
            {
                new ArcSegment(b, new Size(cornerRadius, cornerRadius), 0, false, SweepDirection.Clockwise, true),
                new LineSegment(c, true),
                new LineSegment(d, true),
                new LineSegment(e, true),
                new LineSegment(f, true),
                new ArcSegment(g, new Size(cornerRadius, cornerRadius), 0, false, SweepDirection.Clockwise, true),
                new LineSegment(h, true),
                new ArcSegment(i, new Size(cornerRadius, cornerRadius), 0, false, SweepDirection.Clockwise, true),
                new LineSegment(j, true),
                new ArcSegment(k, new Size(cornerRadius, cornerRadius), 0, false, SweepDirection.Clockwise, true),
                new LineSegment(a, true)
            };

            var pthFigure = new PathFigure(a, pathSegments, false) { IsFilled = true };
            var pthGeometry = new PathGeometry(new List<PathFigure> { pthFigure }, FillRule.Nonzero, null);
            drawingContext.DrawGeometry(BorderBrush, _pen, pthGeometry);
        }
    }
}
