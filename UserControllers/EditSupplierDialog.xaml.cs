using MelodiasDelMundo_Client.Classes;
using MelodiasDelMundo_Client.ServiceReference1;
using MelodiasDelMundo_Client.Utils;
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

namespace MelodiasDelMundo_Client.UserControllers
{
    /// <summary>
    /// Lógica de interacción para EditSupplierDialog.xaml
    /// </summary>
    public partial class EditSupplierDialog : Window
    {
        private SuppliersManagerClient _service;
        public SupplierFound oldSupplier { get; private set; }

        public SupplierDTO editedSupplier { get; private set; }

        public EditSupplierDialog(SupplierFound supplier)
        {
            InitializeComponent();
            _service = new SuppliersManagerClient();
            Title = "Editing :" + $"{supplier.Name}";
            oldSupplier = supplier;
            SetInfoFields(supplier);
        }

        public EditSupplierDialog()
        {
            InitializeComponent();

        }

        public void SetInfoFields(SupplierFound suplierInfo)
        {
            tbName.Text = suplierInfo.Name;
            tbAdrress.Text = suplierInfo.Address;
            tbCity.Text = suplierInfo.City;
            tbCountry.Text = suplierInfo.Country;
            tbPostalCode.Text = suplierInfo.PostalCode;
            tbPhone.Text = suplierInfo.Phone;   
            tbEmail.Text = suplierInfo.Email;
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
            //try { 
            //if (_service.IsSupplierEmailTaken(tbEmail.Text))
            //{
            //    var dialog = new NotificationDialog();
            //    dialog.ShowErrorNotification("This email is currently associated to another supplier");
            //    return;
            //}
            //if (_service.IsSupplierNameTaken(tbName.Text))
            //{
            //    var dialog = new NotificationDialog();
            //    dialog.ShowErrorNotification("This supplier already exists");
            //    return;
            //}

            var supplier = new SupplierDTO()
            {
                SupplierId =oldSupplier.SupplierId, 
                Name = tbName.Text,
                Address = tbAdrress.Text,
                City = tbCity.Text,
                Country = tbCountry.Text,
                PostalCode = tbPostalCode.Text,
                Phone = tbPhone.Text,
                Email = tbEmail.Text,
            };

            editedSupplier = supplier;
            DialogResult = true;
            Close();
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
