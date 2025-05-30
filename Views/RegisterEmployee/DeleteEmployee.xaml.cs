﻿using MelodiasDelMundo_Client.ServiceReference1;
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
    public partial class DeleteEmployee : Window
    {
        private UsersManagerClient _service;
        private ObservableCollection<EmployeeDataContract> _employees;
        private Window _ventanaAnterior;
        private NotificationDialog _notificationDialog;
        public DeleteEmployee(Window ventanaAnterior)
        {
            InitializeComponent();
            _service = new UsersManagerClient();
            _employees = new ObservableCollection<EmployeeDataContract>();
            EmployeeList.ItemsSource = _employees;
            _ventanaAnterior = ventanaAnterior;
            _notificationDialog = new NotificationDialog();

        }
        private void BtnRegresar_Click(object sender, RoutedEventArgs e)
        {
            _ventanaAnterior.Show(); 
            this.Close();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string userName = tbName.Text.Trim();

                if (string.IsNullOrWhiteSpace(userName))
                {
                    _notificationDialog.ShowWarningNotification("Por favor, ingresa el nombre del empleado.");
                    return;
                }

                int id = await Task.Run(() => _service.GetIdEmployeeByUserName(userName));

                if (id <= 0)
                {
                    _notificationDialog.ShowWarningNotification("Empleado no encontrado con ese nombre.");
                    return;
                }

                EmployeeDataContract employeeData = await Task.Run(() => _service.GetEmployeeDetailsWithoutPassword(id));
                if (employeeData == null)
                {
                    _notificationDialog.ShowWarningNotification("No se pudieron obtener los detalles del empleado.");
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
                        var employeeDetails = FindVisualChild<Views.RegisterEmployee.EmployeeDetails>(container);
                        if (employeeDetails != null)
                        {
                            employeeDetails.DeleteRequested += OnEmployeeDeleted;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                _notificationDialog.ShowErrorNotification("Error al obtener el empleado: ");
            }
        }


        private void OnEmployeeDeleted(int idEmployee)
        {
            var employeeToRemove = _employees.FirstOrDefault(); 
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
