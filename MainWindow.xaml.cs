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
using MelodiasDelMundo_Client.ServiceReference1;
using MelodiasDelMundo_Client.Views;
using MelodiasDelMundo_Client.Views.RegisterEmployee;

namespace MelodiasDelMundo_Client
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UsersManagerClient _service;
        public MainWindow()
        {
            InitializeComponent();
            _service=new UsersManagerClient();
        }

        private void btLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = tbUsername.Text;
            string password = tbPassword.Text;

            if (_service.LogIn(username, password))
            {
                EmployeeForm mainMenu = new EmployeeForm();
                mainMenu.Show();  
                this.Close();


            }
            else 
            {
                MessageBox.Show("tas menso wey");
            }
        }
    }
}
