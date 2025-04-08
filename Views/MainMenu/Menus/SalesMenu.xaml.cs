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
using System.Windows.Shapes;

namespace MelodiasDelMundo_Client.Views.MainMenu.Menus
{
    /// <summary>
    /// Lógica de interacción para SalesMenu.xaml
    /// </summary>
    public partial class SalesMenu : Window
    {
        public SalesMenu()
        {
            InitializeComponent();
        }

        private void btSearchSales_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btDeleteSales_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btEditSales_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btRegisterSales_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btLogout_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToUserControll(new MainMenu2());
        }
    }
}
