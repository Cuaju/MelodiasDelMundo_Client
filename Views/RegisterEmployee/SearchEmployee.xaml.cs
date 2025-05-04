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
using System.Xml.Linq;

namespace MelodiasDelMundo_Client.Views.RegisterEmployee
{
    /// <summary>
    /// Lógica de interacción para SearchEmployee.xaml
    /// </summary>
    public partial class SearchEmployee : Window
    {
        private UsersManagerClient _service;
        private ObservableCollection<EmployeeDataContract> _employees;
        private NotificationDialog _notificationDialog;
        private Window _ventanaAnterior;
        public SearchEmployee(Window ventanaAnterior)
        {
            InitializeComponent();
            _service = new UsersManagerClient();
            _employees = new ObservableCollection<EmployeeDataContract>();
            EmployeeList.ItemsSource = _employees;
            _notificationDialog = new NotificationDialog();
            _ventanaAnterior = ventanaAnterior;

        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _ventanaAnterior.Show();
            this.Close();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string userName = tbName.Text.Trim();

                if (string.IsNullOrEmpty(userName))
                {
                    _notificationDialog.ShowErrorNotification("Por favor, ingrese un nombre de usuario.");
                    return;
                }

                int id = await Task.Run(() => _service.GetIdEmployeeByUserName(userName));
                if (id <= 0)
                {
                    _notificationDialog.ShowErrorNotification("No se encontró un empleado con ese nombre de usuario.");
                    return;
                }

                EmployeeDataContract employeeData = await Task.Run(() => _service.GetEmployeeDetailsWithoutPassword(id));

                if (employeeData == null)
                {
                    _notificationDialog.ShowErrorNotification("No se pudieron obtener los detalles del empleado.");
                    return;
                }

                employeeData.userName = userName;
                employeeData.GetType().GetProperty("IdFromServer")?.SetValue(employeeData, id);

                _employees.Clear();
                _employees.Add(employeeData);

                Dispatcher.InvokeAsync(() =>
                {
                    var container = EmployeeList.ItemContainerGenerator.ContainerFromItem(employeeData) as ContentPresenter;
                    if (container != null)
                    {
                        var employeeDetails = FindVisualChild<Views.RegisterEmployee.Employee>(container);
                    }
                });
            }
            catch (Exception ex)
            {
                _notificationDialog.ShowErrorNotification("Error al obtener el empleado: " + ex.Message);
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

