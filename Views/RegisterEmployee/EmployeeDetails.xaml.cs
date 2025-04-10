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
using System.Xml.Linq;
using MelodiasDelMundo_Client.ServiceReference1;
using MelodiasDelMundo_Client.Utils;

namespace MelodiasDelMundo_Client.Views.RegisterEmployee
{
    public partial class EmployeeDetails : UserControl
    {
        private NotificationDialog _notificationDialog;
        public EmployeeDetails()
        {
            InitializeComponent();
            _notificationDialog = new NotificationDialog();
        }

        public event Action<int> DeleteRequested;

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var employee = DataContext as EmployeeDataContract;
            if (employee == null) return;

            var result = MessageBox.Show($"¿Estás seguro de eliminar a {employee.name}?",
                                         "Confirmar Eliminación",
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var service = new UsersManagerClient();
                    int id = await Task.Run(() => service.GetIdEmployeeByUserName(employee.userName));
                    bool success = await Task.Run(() => service.DeleteEmployee(id));

                    if (success)
                    {
                        tbDetails.Visibility = Visibility.Collapsed;
                        TBCITY.Visibility = Visibility.Collapsed;
                        TBPHONE.Visibility = Visibility.Collapsed;
                        TBCITY.Visibility= Visibility.Collapsed;
                        TBNAME.Visibility = Visibility.Collapsed;
                        TBSURNAMES.Visibility = Visibility.Collapsed;
                        TBMAIL.Visibility = Visibility.Collapsed;   
                        btnDelete.Visibility = Visibility.Collapsed;
                        tbCity.Visibility = Visibility.Collapsed;
                        tbMail.Visibility = Visibility.Collapsed;
                        tbSurnames.Visibility = Visibility.Collapsed;
                        tbPhone.Visibility = Visibility.Collapsed;
                        tbCity.Visibility=Visibility.Collapsed;
                        _notificationDialog.ShowSuccessNotification("Empleado eliminado correctamente.");
                        DeleteRequested?.Invoke(id);
                    }
                    else
                    {
                        _notificationDialog.ShowErrorNotification("No se pudo eliminar el empleado.");
                    }
                }
                catch (Exception ex)
                {
                    _notificationDialog.ShowErrorNotification("Error al eliminar: " + ex.Message);
                }
            }
        }

    }
}

