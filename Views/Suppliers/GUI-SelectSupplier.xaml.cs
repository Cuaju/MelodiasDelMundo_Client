using MelodiasDelMundo_Client.Classes;
using MelodiasDelMundo_Client.ServiceReference1;
using MelodiasDelMundo_Client.UserControllers;
using MelodiasDelMundo_Client.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MelodiasDelMundo_Client.Views.Suppliers
{
    /// <summary>
    /// Lógica de interacción para GUI_SelectSupplier.xaml
    /// </summary>
    public partial class GUI_SelectSupplier : UserControl
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<SupplierFound> _suppliersFound;

        private SuppliersManagerClient _service;


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public GUI_SelectSupplier()
        {
            InitializeComponent();
            _service = new SuppliersManagerClient();
            DataContext = this;
            SuppliersFound = new ObservableCollection<SupplierFound>();
        }

        public ObservableCollection<SupplierFound> SuppliersFound
        {
            get => _suppliersFound;
            set
            {
                _suppliersFound = value;
                OnPropertyChanged(nameof(SuppliersFound));
            }
        }

        private void FindSupplierItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is FindSupplierItem findSupplierItem)
            {
                findSupplierItem.EditSupplier += OnEditSupplier;
                findSupplierItem.DeleteSupplier += OnDeleteSupplier;
            }

        }

        private async void OnEditSupplier(object sender, SupplierFound e)
        {
            if ( e != null)
            {
                var editDialog = new EditSupplierDialog(e)
                {
                    Owner = Window.GetWindow(this)
                };

                bool? dialogResult = editDialog.ShowDialog();

                if( dialogResult != true)
                {
                    return;
                }

                SupplierDTO editedInfoSupplier = editDialog.editedSupplier;
                Console.WriteLine(editedInfoSupplier.Email);
                
                try
                {
                    Console.WriteLine(editedInfoSupplier.SupplierId);
                    bool editionResult = _service.EditSupplier(editedInfoSupplier);
                    Console.WriteLine(editionResult);
                    if (editionResult)
                    {
                        SuppliersFound.Remove(e);
                        var notificationDialog = new NotificationDialog();
                        notificationDialog.ShowSuccessNotification(editedInfoSupplier.Name + " information succesfully edited");
                        return;
                    }
                    else
                    {
                        var notificationDialog = new NotificationDialog();
                        notificationDialog.ShowErrorNotification("An error ocurred while editing the user try again later");

                        return ;
                    }
                }
                catch (EndpointNotFoundException ex)
                {
                    var notificationDialog = new NotificationDialog();
                    notificationDialog.ShowErrorNotification("Cannot conect to the server");
                    Console.WriteLine("----"+ ex.Message + "----");
                    Console.WriteLine(ex.StackTrace);
                    return;
                }
                catch (CommunicationException ex)
                {
                    var notificationDialog = new NotificationDialog();
                    notificationDialog.ShowErrorNotification("Cannot establish conecciont to the server");
                    return;
                }
                catch (TimeoutException ex)
                {
                    var notificationDialog = new NotificationDialog();
                    notificationDialog.ShowErrorNotification("Server took too long to respond try again lates");
                    return;
                }
                catch (Exception ex)
                {
                    var notificationDialog = new NotificationDialog();
                    notificationDialog.ShowErrorNotification("Unknown error, try again later");
                    return;
                }
            }

            var dialog = new NotificationDialog();
            dialog.ShowErrorNotification("Unknown error");
        }

        private async void OnDeleteSupplier(object sender, SupplierFound e)
        {
            if (e != null)
            {
                var editDialog = new DeleteSupplierDialog(e)
                {
                    Owner = Window.GetWindow(this)
                };

                bool? dialogResult = editDialog.ShowDialog();

                if (dialogResult != true)
                {
                    return;
                }

                try
                {
                    bool result = _service.DeleteSupplier(e.SupplierId);

                    if (result)
                    {
                        var notificationDialog = new NotificationDialog();
                        notificationDialog.ShowSuccessNotification("Supplier " + e.Name + " deleted succesfully");
                        SuppliersFound.Remove(e);
                        return;
                    }
                    else
                    {
                        var notificationDialog = new NotificationDialog();
                        notificationDialog.ShowErrorNotification("An error ocurred while editing the user try again later");
                        return;
                    }
                }
                catch (EndpointNotFoundException ex)
                {
                    var notificationDialog = new NotificationDialog();
                    notificationDialog.ShowErrorNotification("Cannot conect to the server");
                    Console.WriteLine("----" + ex.Message + "----");
                    Console.WriteLine(ex.StackTrace);
                    return;
                }
                catch (CommunicationException ex)
                {
                    var notificationDialog = new NotificationDialog();
                    notificationDialog.ShowErrorNotification("Cannot establish conecciont to the server");
                    return;
                }
                catch (TimeoutException ex)
                {
                    var notificationDialog = new NotificationDialog();
                    notificationDialog.ShowErrorNotification("Server took too long to respond try again lates");
                    return;
                }
                catch (Exception ex)
                {
                    var notificationDialog = new NotificationDialog();
                    notificationDialog.ShowErrorNotification("Unknown error, try again later");
                    return;
                }

            }
            var dialog = new NotificationDialog();
            dialog.ShowErrorNotification("Unknown error");

        }

        private void btGoBack_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btSearch_Click(object sender, RoutedEventArgs e)
        {
            SuppliersFound.Clear();
            string search = tbSearchSupplier.Text;
            if (!string.IsNullOrEmpty(search))
            {
                try
                {
                    SupplierDTO[] suppliersFound = _service.GetSuppliers(search);

                    foreach (SupplierDTO supplierFound in suppliersFound)
                    {
                        Console.WriteLine(supplierFound.Name + supplierFound.SupplierId);
                        SuppliersFound.Add(new SupplierFound(supplierFound));
                    }
                }
                catch (EndpointNotFoundException ex)
                {
                    var dialog = new NotificationDialog();
                    dialog.ShowErrorNotification("Cannot conect to the server");
                    return;
                }
                catch (CommunicationException ex)
                {
                    var dialog = new NotificationDialog();
                    dialog.ShowErrorNotification("Cannot establish conecciont to the server");
                    return;
                }
                catch (TimeoutException ex)
                {
                    var dialog = new NotificationDialog();
                    dialog.ShowErrorNotification("Server took too long to respond try again lates");
                    return;
                }
                catch (Exception ex)
                {
                    var dialog = new NotificationDialog();
                    dialog.ShowErrorNotification("Unknown error 1, try again later");
                    return;
                }

            }
            tbSearchSupplier.Text = "";
        }
    }
}
