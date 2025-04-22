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
    /// Lógica de interacción para FindProductItem.xaml
    /// </summary>
    public partial class FindProductItem : UserControl
    {
        public EventHandler<ProductFound> selectProduct;
        public bool isSelected;
        public FindProductItem()
        {
            InitializeComponent();
        }

        private void btSelectProduct_Click(object sender, RoutedEventArgs e)
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
                btSelected.Background = Brushes.Gray;
                btSelected.Foreground = Brushes.Gray;
            }

            selectProduct?.Invoke(this, DataContext as ProductFound);
        }
    }
}
