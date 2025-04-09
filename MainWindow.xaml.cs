using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
using MelodiasDelMundo_Client.Utils;
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
        private NotificationDialog _notificationDialog;

        public MainWindow()
        {
            InitializeComponent();
            _service = new UsersManagerClient();
            _notificationDialog = new NotificationDialog();
        }

        private void BtLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = tbUsername.Text;
            string password = tbPassword.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                _notificationDialog.ShowErrorNotification("Todos los campos son obligatorios.");
                return;
            }

            try
            {
                if (_service.LogIn(username, password))
                {
                    DeleteEmployee mainMenu = new DeleteEmployee();
                    mainMenu.Show();
                    this.Close();
                }
                else
                {
                    _notificationDialog.ShowErrorNotification("Revisa de nuevo los datos ingresados.");
                }
            }
            catch (FaultException ex)
            {
                _notificationDialog.ShowErrorNotification("Error de servicio: " + ex.Message);
            }
            catch (CommunicationException ex)
            {
                _notificationDialog.ShowErrorNotification("Error de comunicación con el servidor: " + ex.Message);
            }
            catch (TimeoutException ex)
            {
                _notificationDialog.ShowErrorNotification("Tiempo de espera agotado al intentar conectar con el servidor: " + ex.Message);
            }
            catch (Exception ex)
            {
                _notificationDialog.ShowErrorNotification("Error inesperado: " + ex.Message);
            }
        }
    }
}
