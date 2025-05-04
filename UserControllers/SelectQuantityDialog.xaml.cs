using MelodiasDelMundo_Client.Classes;
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
    /// Lógica de interacción para SelectQuantityDialog.xaml
    /// </summary>
    public partial class SelectQuantityDialog : Window
    {
        public ProductFound productSelected { get; set; } = new ProductFound();
        public decimal _totalCost { get; set; }

        public int _quantity { get; set; }

        public SelectQuantityDialog(ProductFound product)
        {
            InitializeComponent();
            productSelected = product;
            lbProductName.Content = product.ProductName;
            lbPrice.Content = product.PurchasePrice;
        }

        public SelectQuantityDialog()
        {
            InitializeComponent();
        }

        private void btAccept_Click(object sender, RoutedEventArgs e)
        {
            if (cbQuantity.SelectedItem is ComboBoxItem selectedItem)
            {
                string value = selectedItem.Content.ToString();
                if (value != null)
                {
                    if (int.TryParse(value, out int quantity))
                    {
                        _totalCost = quantity * productSelected.PurchasePrice;
                        DialogResult = true;
                        Close();
                    }
                }
                else
                {
                    var notification = new NotificationDialog();
                    notification.ShowWarningNotification("Select a valid quantity");
                }
            }
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void cbQuantity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbQuantity.SelectedItem is ComboBoxItem selectedItem)
            {
                string value = selectedItem.Content.ToString();
                int quantity = int.Parse(value);
                _quantity = quantity;
                _totalCost = quantity * productSelected.PurchasePrice;
                lbTotalCost.Content = _totalCost.ToString();
            }
        }
    }
}
