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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MelodiasDelMundo_Client.UserControllers
{
    /// <summary>
    /// Lógica de interacción para FindSupplierItem.xaml
    /// </summary>
    public partial class FindSupplierItem : UserControl
    {
        public event EventHandler<SupplierFound> EditSupplier;
        public event EventHandler<SupplierFound> DeleteSupplier;

        public FindSupplierItem()
        {
            InitializeComponent();
        }

        private void btEditSupplier_Click(object sender, RoutedEventArgs e)
        {
            EditSupplier?.Invoke(this, DataContext as SupplierFound);
        }

        private void btDeleteSupplier_Click(object sender, RoutedEventArgs e)
        {
            DeleteSupplier?.Invoke(this, DataContext as SupplierFound);
        }
    }
}
