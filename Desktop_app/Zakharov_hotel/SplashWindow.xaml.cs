using System.Windows;
using Zakharov_hotel;

namespace Zakharov_hotel
{
    public partial class SplashWindow : Window
    {
        public SplashWindow()
        {
            InitializeComponent();
            SplashPlayer.Play();
        }

        private void SplashPlayer_MediaEnded(object sender, RoutedEventArgs e)
        { 
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}
