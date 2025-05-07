using MelodiasDelMundo_Client.ServiceReference1;
using MelodiasDelMundo_Client.Utils;
using MelodiasDelMundo_Client.Views.MainMenu.Menus;
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

namespace MelodiasDelMundo_Client.Views.RegisterEmployee
{
    /// <summary>
    /// Lógica de interacción para EditEmployee.xaml
    /// </summary>
    public partial class EditEmployee : Window
    {
        private NotificationDialog _notificationDialog;
        private UsersManagerClient _service;
        public EditEmployee(Window ventanaAnterior)
        {
            _notificationDialog = new NotificationDialog();
            _service = new UsersManagerClient();
            InitializeComponent();
            CargarDatosEmpleado();
        }
        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            int idEmpleado = SesionEmpleado.Instancia.IdEmpleado;

            if (string.IsNullOrWhiteSpace(txtUserName.Text) ||
                string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtSurnames.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtMail.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text) ||
                string.IsNullOrWhiteSpace(txtZipCode.Text) ||
                string.IsNullOrWhiteSpace(txtCity.Text))
            {
                _notificationDialog.ShowWarningNotification("Todos los campos son obligatorios");
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtMail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                _notificationDialog.ShowWarningNotification("Correo electronico no valido");
                return;
            }

            if (!int.TryParse(txtPhone.Text, out int telefono))
            {
                _notificationDialog.ShowWarningNotification("El telefono solo debe contener numeros");
                return;
            }

            try
            {
                var datosActuales = _service.GetEmployeeDetailsWithoutPassword(idEmpleado);

                if (txtMail.Text != datosActuales.mail && _service.MailAlreadyExist(txtMail.Text))
                {
                    _notificationDialog.ShowErrorNotification("El correo ya esta registrado en otra cuenta");
                    return;
                }

                if (telefono != datosActuales.phone && _service.PhoneNumberExists(telefono))
                {
                    _notificationDialog.ShowErrorNotification("El numero de telefono ya esta registrado");
                    return;
                }

                if (txtUserName.Text != datosActuales.userName && _service.UserNameExist(txtUserName.Text))
                {
                    _notificationDialog.ShowErrorNotification("EL nombre de usuario ya esta en uso");
                    return;
                }

                EmployeeDataContract empleadoEditado = new EmployeeDataContract
                {
                    userName = txtUserName.Text,
                    name = txtName.Text,
                    surnames = txtSurnames.Text,
                    phone = telefono,
                    mail = txtMail.Text,
                    address = txtAddress.Text,
                    zipCode = txtZipCode.Text,
                    city = txtCity.Text
                };

                bool resultado = _service.EditEmployee(idEmpleado, empleadoEditado);

                if (resultado)
                {
                    _notificationDialog.ShowSuccessNotification("Datos actulizados correctamente");
                    this.Close();
                }
                else
                {
                    _notificationDialog.ShowErrorNotification("No se pudieron actualizar los datos");
                }
            }
            catch (Exception ex)
            {
                _notificationDialog.ShowErrorNotification("Error al guardar los cambios");
            }
        }
        private void BtnRegresar_Click(object sender, RoutedEventArgs e)
        {
            UsersMenu reportesMenu = new UsersMenu();
            reportesMenu.Show();
            this.Close();
        }


        private void CargarDatosEmpleado()
        {
            int idEmpleado = SesionEmpleado.Instancia.IdEmpleado;
            Console.WriteLine("el id es"+idEmpleado);
            try
            {


                EmployeeDataContract datos = _service.GetEmployeeDetailsWithoutPassword(idEmpleado);

                if (datos != null)
                {
                    txtUserName.Text = datos.userName;
                    txtName.Text = datos.name;
                    txtSurnames.Text = datos.surnames;
                    txtPhone.Text = datos.phone.ToString();
                    txtMail.Text = datos.mail;
                    txtAddress.Text = datos.address;
                    txtZipCode.Text = datos.zipCode;
                    txtCity.Text = datos.city;
                }
            }
            catch (Exception ex)
            {
                _notificationDialog.ShowErrorNotification("error al cargar los datos");
            }
        }

        
    }

}
    
