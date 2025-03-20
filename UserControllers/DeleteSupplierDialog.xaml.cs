using MelodiasDelMundo_Client.Classes;
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
using System.Xml.Linq;

namespace MelodiasDelMundo_Client.UserControllers
{
    /// <summary>
    /// Lógica de interacción para DeleteSupplierDialog.xaml
    /// </summary>
    public partial class DeleteSupplierDialog : Window
    {
        public SupplierFound supplier;
        public DeleteSupplierDialog(SupplierFound supplier)
        {
            InitializeComponent();
            this.supplier = supplier;
            SetInfoFields(supplier);

        }

        public void SetInfoFields(SupplierFound suplierInfo)
        {
            tbInfoSupplier.Text =
                " Name: "+
            suplierInfo.Name +
            "\n Address: " +
            suplierInfo.Address +
            "\n City: " +
            suplierInfo.City +
            "\n Country: " +
            suplierInfo.Country +
            "\n Postal Code: " +
            suplierInfo.PostalCode +
            "\n Phone: " +
            suplierInfo.Phone +
            "\n Email: " +
            suplierInfo.Email;
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
