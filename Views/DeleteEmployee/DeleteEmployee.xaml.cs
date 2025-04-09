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

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MelodiasDelMundo_Client.Views.RegisterEmployee
{
    public partial class DeleteEmployee : Window
    {
        private UsersManagerClient _service;
        private ObservableCollection<EmployeeDataContract> _employees;

        public DeleteEmployee()
        {
            InitializeComponent();
            _service = new UsersManagerClient();
            _employees = new ObservableCollection<EmployeeDataContract>();
            EmployeeList.ItemsSource = _employees;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string userName = tbName.Text;
                int id = await Task.Run(() => _service.GetIdEmployeeByUserName(userName));
                EmployeeDataContract employeeData = await Task.Run(() => _service.GetEmployeeDetailsWithoutPassword(id));

                // Guardar el ID como Tag (truco simple para tenerlo en el control)
                employeeData.userName = userName; // solo como referencia visual
                employeeData.GetType().GetProperty("IdFromServer")?.SetValue(employeeData, id);

                _employees.Clear();
                _employees.Add(employeeData);

                // Conectar evento DeleteRequested
                Dispatcher.InvokeAsync(() =>
                {
                    var container = EmployeeList.ItemContainerGenerator.ContainerFromItem(employeeData) as ContentPresenter;
                    if (container != null)
                    {
                        var employeeDetails = FindVisualChild<Views.DeleteEmployee.EmployeeDetails>(container);
                        if (employeeDetails != null)
                        {
                            employeeDetails.DeleteRequested += OnEmployeeDeleted;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el empleado: " + ex.Message);
            }
        }

        private void OnEmployeeDeleted(int idEmployee)
        {
            var employeeToRemove = _employees.FirstOrDefault(); // Solo hay uno
            if (employeeToRemove != null)
            {
                _employees.Remove(employeeToRemove);
            }
        }


        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T correctlyTyped)
                    return correctlyTyped;

                var result = FindVisualChild<T>(child);
                if (result != null)
                    return result;
            }
            return null;
        }
    }
}
