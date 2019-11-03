﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF.Core
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected Annotation Annotation { get; set; }


        public MainWindow()
        {
            InitializeComponent();

            Annotation = new Annotation()
            {
                Width = 300,
                Height = 200,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = 4
            };
            MainCanvas.Children.Add(Annotation);

            Annotation.Text = @"دکترها به بورلی هشدار داده بودند که لازم است همیشه یوجین را تحت نظر داشته باشد. دکترها می‌گفتند که اگر او گم شود، دیگر قادر نخواهد بود راه خانه‌اش را پیدا کند. ولی یک روز صبح در حالی که بورلی در حال لباس پوشیدن بود، یوجین از در جلویی بیرون رفت. یوجین عادت داشت از این اتاق به آن اتاق برود، برای همین مدتی طول کشید تا بورلی متوجه شود که یوجین آنجا نیست و هنگامی که فهمید به شدت نگران شد، بیرون دوید و با دقت خیابان را نگاه کرد اما نتوانست یوجین را ببیند. او به خانه همسایه‌ها رفت و محکم به شیشه پنجره‌شان زد. خانه‌های آنها شبیه خانه خودشان بود. آیا این امکان وجود داشت که یوجین گیج شده و داخل رفته باشد؟ بورلی به سمت در دوید و زنگ را زد تا اینکه کسی جواب داد. یوجین آنجا نبود. بورلی در حالی که اسم یوجین را فریاد می‌زد، دوباره به خیابان دوید. بورلی گریه‌اش گرفته بود. اگر یوجین به خیابان پر از اتومبیل رفته باشد چه؟ چطور می‌توانست به دیگران بگوید که کجا زندگی می‌کند؟ بورلی پانزده دقیقه بود که بیرون بود و همه جا را می‌گشت. او به سمت خانه دوید تا به پلیس زنگ بزند. 
وقتی بورلی در را باز کرد، دید که یوجین توی اتاق پذیرایی روبروی تلویزیون نشسته و شبکه هیستوری (تاریخ) را تماشا می‌کند. یوجین با دیدن اشک‌های بورلی گیج شده بود. او گفت که به خاطر نمی‌آورد که خانه را ترک کرده باشد، یادش نمی‌آمد که کجا بوده";

        }


        private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Annotation != null)
            {
                var posInView = e.GetPosition(MainCanvas);
                Annotation.Height = MainCanvas.ActualHeight / 2 - 25;
                Canvas.SetLeft(Annotation, posInView.X);
                Canvas.SetTop(Annotation, posInView.Y);
                Annotation.Visibility = Visibility.Visible;
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            Annotation.BubblePeakPosition = e.GetPosition(Annotation);
            Annotation.InvalidateVisual();
        }
    }
}
