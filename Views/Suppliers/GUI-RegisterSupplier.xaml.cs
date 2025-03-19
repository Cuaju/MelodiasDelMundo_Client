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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MelodiasDelMundo_Client.Views.Suppliers
{
    /// <summary>
    /// Lógica de interacción para GUI_RegisterSupplier.xaml
    /// </summary>
    public partial class GUI_RegisterSupplier : UserControl
    {
        private SuppliersManagerClient _service;
        public GUI_RegisterSupplier()
        {
            InitializeComponent();
            _service = new SuppliersManagerClient();
        }

        private void btRegister_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(tbName.Text) || string.IsNullOrWhiteSpace(tbAdrress.Text) ||
                string.IsNullOrWhiteSpace(tbCity.Text) || string.IsNullOrWhiteSpace(tbCountry.Text) ||
                string.IsNullOrWhiteSpace(tbPostalCode.Text) || string.IsNullOrWhiteSpace(tbPhone.Text) ||
                string.IsNullOrWhiteSpace(tbEmail.Text))
            {
                var dialog = new NotificationDialog();
                dialog.ShowWarningNotification("Missing fields");
                return;
            }

            if (_service.IsSupplierEmailTaken(tbEmail.Text))
            {
                var dialog = new NotificationDialog();
                dialog.ShowErrorNotification("This email is currently associated to another supplier");
                return;
            }
            if (_service.IsSupplierNameTaken(tbName.Text))
            {
                var dialog = new NotificationDialog();
                dialog.ShowErrorNotification("This supplier already exists");
                return;
            }

            var supplier = new SupplierDTO()
            {
                Name = tbName.Text,
                Address = tbAdrress.Text,
                City = tbCity.Text,
                Country = tbCountry.Text,
                PostalCode = tbPostalCode.Text,
                Phone = tbPhone.Text,
                Email = tbEmail.Text,
            };

            try
            {
                if(_service.RegisterSupplier(supplier))
                {
                    var dialog = new NotificationDialog();
                    dialog.ShowSuccessNotification("Supplier registered succesfully");
                    return;
                }
            }
            catch (EndpointNotFoundException ex)
            {
                var dialog = new NotificationDialog();
                dialog.ShowErrorNotification("Cannot conect to the server");
                return;
            }
            catch (CommunicationException ex)
            {
                var dialog = new NotificationDialog();
                dialog.ShowErrorNotification("Cannot establish conecciont to the server");
                return;
            }
            catch (TimeoutException ex)
            {
                var dialog = new NotificationDialog();
                dialog.ShowErrorNotification("Server took too long to respond try again lates");
                return;
            }
            catch (Exception ex)
            {
                var dialog = new NotificationDialog();
                dialog.ShowErrorNotification("Unknown error, try again later");
                return;
            }

        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
