using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using MelodiasDelMundo_Client.ServiceReference1;
using MelodiasDelMundo_Client.Utils;
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

        public void NavigateToWindow(Window newWindow, double? newWidth = null, double? newHeight = null)
        {
            newWindow.Show();

            if (newWidth != null && newHeight != null)
            {
                newWindow.Width = newWidth.Value;
                newWindow.Height = newHeight.Value;
                newWindow.SizeToContent = SizeToContent.Manual;
            }
            else
            {
                newWindow.SizeToContent = SizeToContent.WidthAndHeight;
            }

            newWindow.WindowState = WindowState.Normal;
            newWindow.WindowStyle = WindowStyle.SingleBorderWindow;

            this.Close();
        }

        public void NavigateToUserControll(UserControl control, double? newWidth = null, double? newHeight = null)
        {
            Content = null;

            var container = new Grid();
            container.Children.Add(control);
            Content = container;

            WindowState = WindowState.Normal;
            WindowStyle = WindowStyle.SingleBorderWindow;

            if (newWidth != null && newHeight != null)
            {
                Height = newHeight.Value;
                Width = newWidth.Value;
                SizeToContent = SizeToContent.Manual;
            }
            else
            {
                SizeToContent = SizeToContent.WidthAndHeight;
            }
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
                    NavigateToWindow(new Views.MainMenu.MMenu());
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
