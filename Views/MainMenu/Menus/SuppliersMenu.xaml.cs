using MelodiasDelMundo_Client.Views.Report;
using MelodiasDelMundo_Client.Views.Suppliers;
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

namespace MelodiasDelMundo_Client.Views.MainMenu.Menus
{
    /// <summary>
    /// Lógica de interacción para SuppliersMenu.xaml
    /// </summary>
    public partial class SuppliersMenu : Window
    {
        public SuppliersMenu()
        {
            InitializeComponent();
        }
        public void NavigateToUserControll(UserControl control, double? newWidth = null, double? newHeight = null)
        {
            this.Content = null;

            var container = new Grid();
            container.Children.Add(control);
            this.Content = container;

            this.WindowState = WindowState.Normal;
            this.WindowStyle = WindowStyle.SingleBorderWindow;
            if (newWidth.HasValue && newHeight.HasValue)
            {
                this.Width = newWidth.Value;
                this.Height = newHeight.Value;
                this.SizeToContent = SizeToContent.Manual;
            }
            else
            {
                this.SizeToContent = SizeToContent.WidthAndHeight;
            }
        }

        private void btSearchSupplier_Click(object sender, RoutedEventArgs e)
        {
            NavigateToUserControll(new GUI_RegisterPurchase());
        }

        private void btDeleteSupplier_Click(object sender, RoutedEventArgs e)
        {
            NavigateToUserControll(new GUI_SelectSupplier());
        }

        private void btEditSupplier_Click(object sender, RoutedEventArgs e)
        {
            
            NavigateToUserControll(new GUI_SelectSupplier());
        }

        private void btRegisterSupplier_Click(object sender, RoutedEventArgs e)
        {
            NavigateToUserControll(new GUI_RegisterSupplier());
        }

        private void btLogout_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var ventana = new MainMenu.MMenu ();
            ventana.Show();
            this.Close();
        }
    }
}
