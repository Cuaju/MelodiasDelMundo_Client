using MelodiasDelMundo_Client.Views.MainMenu.Menus;
using MelodiasDelMundo_Client.Views.Product;
using MelodiasDelMundo_Client.Views.RegisterEmployee;
using MelodiasDelMundo_Client.Views.Sale;
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
    public partial class MMenu : Window
    {
        public MMenu()
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
            GUI_ProductManagement productManagementWindow = new GUI_ProductManagement();
            productManagementWindow.Show();
            this.Close();
        }

        private void btSalesManagement_Click(object sender, RoutedEventArgs e)
        {
            GUI_SalesManagement saleManagementWindow = new GUI_SalesManagement();
            saleManagementWindow.Show();
            this.Close();
        }


        private void btSuppliersManagement_Click(object sender, RoutedEventArgs e)
        {
            SuppliersMenu mainMenu = new SuppliersMenu();
            mainMenu.Show();
            this.Close();
        }

        private void btReports_Click(object sender, RoutedEventArgs e)
        {
            GUI_ReportesMenu reportesMenu= new GUI_ReportesMenu();
            reportesMenu.Show();
            this.Close();
        }

        private void btLogout_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }





    }
}
