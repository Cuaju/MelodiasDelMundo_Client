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
    /// Lógica de interacción para FindSupplierPurchaseItem.xaml
    /// </summary>
    public partial class FindSupplierPurchaseItem : UserControl
    {
        public EventHandler<SupplierFound> selectSupplier;

        private bool isSelected = false;
        public FindSupplierPurchaseItem()
        {
            InitializeComponent();
        }

        private void btSelectSupplier_Click(object sender, RoutedEventArgs e)
        {
            if (!isSelected)
            {
                isSelected = true;
                btSelected.Background = Brushes.Blue;
                btSelected.Foreground = Brushes.Blue;
            }
            else
            {
                isSelected = false;
                btSelected.Background= Brushes.Gray;
                btSelected.Foreground= Brushes.Gray;
            }

            selectSupplier?.Invoke(this, DataContext as SupplierFound);
        }
    }
}
