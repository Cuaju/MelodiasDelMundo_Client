using MelodiasDelMundo_Client.ServiceReference1;
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
        public EmployeeForm()
        {
            InitializeComponent();
            _service = new UsersManagerClient();
        }

        private void Btnaccept(object sender, RoutedEventArgs e)
        {
            EmployeeDataContract employeeDataContract = new EmployeeDataContract();
            employeeDataContract.name= tbName.Text;
            employeeDataContract.surnames= tbSurnames.Text;
            string phone= tbPhoneNumber.Text;
            int number = int.Parse(phone);
            employeeDataContract.phone= number;
            employeeDataContract.mail = tbMail.Text;
            employeeDataContract.city = tbCity.Text;
            employeeDataContract.zipCode= tbPostalCode.Text;
            employeeDataContract.userName= tbUserName.Text;
            employeeDataContract.password= pbPassword.Password;

            try
            {
                _service.AddEmployee(employeeDataContract);
            }
            catch (EndpointNotFoundException ex)
            {
                Console.WriteLine(ex);
            }catch(CommunicationException ex)
            {
                Console.WriteLine(ex.Message);
            }



        }

        private void BtnCancel(object sender, RoutedEventArgs e)
        {


        }
    }
}
