using MelodiasDelMundo_Client.Views.MainMenu.Menus;
using MelodiasDelMundo_Client.Views.RegisterEmployee;
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

namespace MelodiasDelMundo_Client.Views.MainMenu
{
    /// <summary>
    /// Lógica de interacción para Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void btUsersManagement_Click(object sender, RoutedEventArgs e)
        {
            UsersMenu mainMenu = new UsersMenu();
            mainMenu.Show();
            this.Close();
        }

        private void btProductsManagement_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToWindow(new Menus.ProductsMenu());
        }

        private void btSalesManagement_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToWindow(new Menus.SalesMenu());
        }

        private void btSuppliersManagement_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToWindow(new Menus.SuppliersMenu());
        }

        private void btReports_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btLogout_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
