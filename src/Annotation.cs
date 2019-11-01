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


        public Annotation()
        {
            BorderBrush = Brushes.Teal;
            BorderThickness = 2;
            _pen = new Pen(Brushes.Transparent, BorderThickness);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var arrowRadius = 5;
            var cornerRadius = 10;

            var a = new Point(0, cornerRadius);
            var b = new Point(cornerRadius, 0);
            var c = new Point(ActualWidth / 2 - arrowRadius, 0);
            var d = new Point(ActualWidth / 2, -cornerRadius);
            var e = new Point(ActualWidth / 2 + arrowRadius, 0);
            var f = new Point(ActualWidth - cornerRadius, 0);
            var g = new Point(ActualWidth, 10);
            var h = new Point(ActualWidth, ActualHeight - cornerRadius);
            var i = new Point(ActualWidth - cornerRadius, ActualHeight);
            var j = new Point(cornerRadius, ActualHeight);
            var k = new Point(0, ActualHeight - cornerRadius);


            if (_pen.Brush != BorderBrush)
                _pen.Brush = BorderBrush;
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
            drawingContext.DrawGeometry(BorderBrush, _pen, CreateArcGeometry(a, b, cornerRadius)); // 1
            drawingContext.DrawLine(_pen, b, c); //    2
            drawingContext.DrawLine(_pen, c, d); //    3
            drawingContext.DrawLine(_pen, d, e); //    4
            drawingContext.DrawLine(_pen, e, f); //    5
            drawingContext.DrawGeometry(BorderBrush, _pen, CreateArcGeometry(f, g, cornerRadius)); // 6
            drawingContext.DrawLine(_pen, g, h); //    7
            drawingContext.DrawGeometry(BorderBrush, _pen, CreateArcGeometry(h, i, cornerRadius)); // 8
            drawingContext.DrawLine(_pen, i, j); //     9
            drawingContext.DrawGeometry(BorderBrush, _pen, CreateArcGeometry(j, k, cornerRadius)); // 10
            drawingContext.DrawLine(_pen, k, a); //     11
        }


        protected Geometry CreateArcGeometry(Point startPos, Point endPos, double radius)
        {
            var arcSeg = new ArcSegment
            {
                Point = endPos,
                Size = new Size(radius, radius),
                IsLargeArc = false,
                SweepDirection = SweepDirection.Clockwise,
                RotationAngle = 0,
                IsStroked = true
            };

            var pthFigure = new PathFigure(startPos, new List<PathSegment> { arcSeg }, false) { IsFilled = false };
            var pthGeometry = new PathGeometry(new List<PathFigure> { pthFigure }, FillRule.EvenOdd, null);

            return pthGeometry;
        }
    }
}
