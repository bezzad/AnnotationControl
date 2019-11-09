using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AnnotationControl
{
    public class TextCanvas : Canvas
    {
        public FlowDirection TextDirection { get; set; }
        public Brush Foreground { get; set; }
        public FontFamily FontFamily { get; set; }
        public double FontSize { get; set; }
        public TextAlignment TextAlign { get; set; }
        public string Text { get; set; }
        public double ScrollBarWidth { get; set; } = 23;
        //public Thickness Padding { get; set; }

        public TextCanvas()
        {
            ClipToBounds = true;
            Background = Brushes.Transparent;
        }


        protected override void OnRender(DrawingContext dc)
        {
            var ft = new FormattedText(Text, CultureInfo.CurrentCulture, TextDirection,
                new Typeface(FontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal),
                FontSize, Foreground, new NumberSubstitution(), VisualTreeHelper.GetDpi(this).PixelsPerDip)
            {
                LineHeight = FontSize,
                TextAlignment = TextAlign,
                MaxTextWidth = ActualWidth - ScrollBarWidth//- Padding.Left - Padding.Right
            };

            if (Parent is ScrollViewer container)
            {
                if (ft.Height > container.ActualHeight) // when scrollbar visible
                {
                    ft.MaxTextWidth = ActualWidth - ScrollBarWidth; //- Padding.Left - Padding.Right - ScrollBarWidth;
                }
                // Note set parent height from here, when the text height is less than parent height
                Height = Math.Max(ft.Height /* + Padding.Top + Padding.Bottom */, container.ActualHeight);
            }

            dc.DrawText(ft, new Point(0, 0));
        }
    }
}
