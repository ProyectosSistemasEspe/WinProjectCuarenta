using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;

namespace WinProjectCuarentaV2
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

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            WindowsCuarenta w = new WindowsCuarenta(true,"",4);
            this.Close();
            w.Show();
        }
        private void btn2PlayersGame_Click(object sender, RoutedEventArgs e)
        {
            WindowsCuarenta w = new WindowsCuarenta(true, "",2);
            this.Close();
            w.Show();
        }
        private void btnJoinGame_Click(object sender, RoutedEventArgs e)
        {
            JoinGameWindow j = new JoinGameWindow(false);
            j.Show();
            this.Close();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

       
    }
}
