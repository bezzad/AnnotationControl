using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var url = "https://taaghche.com";
            OpenUrl(url);
        }

        public void OpenUrl(string url)
        {
            url = url.Replace("&", "^&");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}")
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
            else
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = true,
                    CreateNoWindow = true
                });
            }
        }
    }
}
