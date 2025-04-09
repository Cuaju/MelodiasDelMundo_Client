using MelodiasDelMundo_Client.ServiceReference1;
using MelodiasDelMundo_Client.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Lógica de interacción para DeleteEmployee.xaml
    /// </summary>
    public partial class DeleteEmployee : Window
    {
        private UsersManagerClient _service;
        private NotificationDialog _notificationDialog;
        private ObservableCollection<EmployeeDataContract> _employees;

        public DeleteEmployee()
        {
            InitializeComponent();
            _service = new UsersManagerClient();
            _notificationDialog = new NotificationDialog();

            _employees = new ObservableCollection<EmployeeDataContract>();
            EmployeeList.ItemsSource = _employees;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Accede a tbName.Text en el hilo de la UI
                string userName = tbName.Text;

                // Luego haz las llamadas al servidor en segundo plano
              //  int id = await Task.Run(() => _service.GetIdEmployeeByUserName(userName));
                EmployeeDataContract employeeData = await Task.Run(() => _service.GetEmployeeDetailsWithoutPassword(2));
                Console.WriteLine("empleado en cliente "+employeeData.userName);

                // Actualiza la colección observable
                _employees.Clear();
                _employees.Add(employeeData);
            }
            catch (Exception ex)
            {
                _notificationDialog.ShowErrorNotification("Error al obtener el empleado: " + ex.Message);
            }
        }


    }


}
