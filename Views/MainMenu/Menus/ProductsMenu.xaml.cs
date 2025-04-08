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
    /// Lógica de interacción para ProductsMenu.xaml
    /// </summary>
    public partial class ProductsMenu : Window
    {
        public ProductsMenu()
        {
            InitializeComponent();
        }

        private void btSearchProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btDeleteProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btEditProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btRegisterProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btLogout_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToUserControll(new MainMenu2());
        }
    }
}
