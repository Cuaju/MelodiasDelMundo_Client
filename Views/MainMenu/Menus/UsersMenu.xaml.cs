using MelodiasDelMundo_Client.Views.Product;
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

namespace MelodiasDelMundo_Client.Views.MainMenu.Menus
{
    /// <summary>
    /// Lógica de interacción para UsersMenu.xaml
    /// </summary>
    public partial class UsersMenu : Window
    {
        public UsersMenu()
        {
            InitializeComponent();
        }

        private void btSearchUser_Click(object sender, RoutedEventArgs e)
        {
            var deleteWindow = new SearchEmployee(this);
            deleteWindow.Show();
            this.Hide();
        }

        private void btDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            var deleteWindow = new DeleteEmployee(this);
            deleteWindow.Show();
            this.Hide(); 

        }

        private void btEditUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btRegisterUser_Click(object sender, RoutedEventArgs e)
        {
            var deleteWindow = new EmployeeForm(this);
            deleteWindow.Show();
            this.Hide();

        }


        private void btLogout_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MMenu mainMenu = new MMenu();
            mainMenu.Show();
            this.Close();
        }
    }
}
