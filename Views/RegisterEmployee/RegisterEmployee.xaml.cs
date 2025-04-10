
using MelodiasDelMundo_Client.ServiceReference1;
using MelodiasDelMundo_Client.Utils;
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
using System.Windows.Shapes;
namespace MelodiasDelMundo_Client.Views.RegisterEmployee
{
    /// <summary>
    /// Lógica de interacción para EmployeeForm.xaml
    /// </summary>
    public partial class EmployeeForm : Window
    {
        private UsersManagerClient _service;
        private NotificationDialog _notificationDialog;
        private Window _ventanaAnterior;

        public EmployeeForm(Window ventanaAnterior)
        {
            InitializeComponent();
            _service = new UsersManagerClient();
            _ventanaAnterior = ventanaAnterior;
            _notificationDialog = new NotificationDialog();
        }

        private void BtnAcept_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }

            EmployeeDataContract employeeDataContract = new EmployeeDataContract()
            {
                name = tbName.Text,
                surnames = tbSurnames.Text,
                phone = int.Parse(tbPhoneNumber.Text),
                mail = tbMail.Text,
                city = tbCity.Text,
                zipCode = tbPostalCode.Text,
                userName = tbUserName.Text,
                password = pbPassword.Password
            };

            try
            {
                if (_service.MailAlreadyExist(tbMail.Text))
                {
                    _notificationDialog.ShowErrorNotification("El correo electrónico ya está en uso.");
                    return;
                }

                if (_service.UserNameExist(tbUserName.Text))
                {
                    _notificationDialog.ShowErrorNotification("El nombre de usuario ya está en uso.");
                    return;
                }
                if (_service.PhoneNumberExists(int.Parse(tbPhoneNumber.Text)))
                {
                    _notificationDialog.ShowErrorNotification("El telefono ya está en uso.");
                    return;
                }
                _service.AddEmployee(employeeDataContract);

                _notificationDialog.ShowSuccessNotification("Se agregó de forma correcta el empleado");

            }
            catch (EndpointNotFoundException ex)
            {
                Console.WriteLine(ex);
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

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(tbName.Text) ||
                string.IsNullOrWhiteSpace(tbSurnames.Text) ||
                string.IsNullOrWhiteSpace(tbPhoneNumber.Text) ||
                string.IsNullOrWhiteSpace(tbMail.Text) ||
                string.IsNullOrWhiteSpace(tbCity.Text) ||
                string.IsNullOrWhiteSpace(tbPostalCode.Text) ||
                string.IsNullOrWhiteSpace(tbUserName.Text) ||
                string.IsNullOrWhiteSpace(pbPassword.Password))
            {
                _notificationDialog.ShowErrorNotification("Todos los campos son obligatorios.");
                return false;
            }

            if (!int.TryParse(tbPhoneNumber.Text, out _))
            {
                _notificationDialog.ShowErrorNotification("El número de teléfono debe ser válido.");
                return false;
            }

            if (!IsValidEmail(tbMail.Text))
            {
                _notificationDialog.ShowErrorNotification("El formato del correo electrónico no es válido.");
                return false;
            }

            if (pbPassword.Password.Length < 6)
            {
                _notificationDialog.ShowErrorNotification("La contraseña debe tener al menos 6 caracteres.");
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            tbName.Text = "";
            tbSurnames.Text = "";
            tbPhoneNumber.Text = "";
            tbMail.Text = "";
            tbCity.Text = "";
            tbPostalCode.Text = "";
            tbUserName.Text = "";
            pbPassword.Password = "";
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            _ventanaAnterior.Show();
            this.Close();
        }
    }
}


