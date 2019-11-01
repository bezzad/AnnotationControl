using System.Windows;
using System.Windows.Input;

namespace WPF.Core
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Note.BubblePeakPosition = e.GetPosition(Note);
            Note.InvalidateVisual();
        }
    }
}
