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
using MelodiasDelMundo_Client.Utils;

namespace MelodiasDelMundo_Client.Views.DeleteEmployee
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
                        _notificationDialog.ShowInfoNotification("Empleado eliminado de manera correcta");
                        DeleteRequested?.Invoke(id);
                        tbName.Text = "";
                        tbTelefono.Text = "";
                        tbCorreo.Text = "";
                        tbApellidos.Text = "";

                    }
                    else
                    {
                        _notificationDialog.ShowInfoNotification("No se pudo eliminar el empleado");
                        
                    }
                }
                catch (Exception ex)
                {
                    _notificationDialog.ShowInfoNotification("No se pudo eliminar el empleado");
                }
            }
        }
    
    }
}
