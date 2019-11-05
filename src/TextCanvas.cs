using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPF.Core
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
        public Thickness Padding { get; set; }

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
                MaxTextWidth = ActualWidth - Padding.Left - Padding.Right
            };

            if (ft.Height > ft.LineHeight)
            {
                ft.MaxTextWidth = ActualWidth - Padding.Left - Padding.Right - ScrollBarWidth;
                Height = ft.Height + Padding.Top + Padding.Bottom;
            }

            dc.DrawText(ft, new Point(Padding.Left, Padding.Top));

        }
    }
}
