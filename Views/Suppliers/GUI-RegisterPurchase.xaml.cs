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
    /// Lógica de interacción para GUI_RegisterPurchase.xaml
    /// </summary>
    public partial class GUI_RegisterPurchase : UserControl
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<SupplierFound> _suppliersFound;
        private ObservableCollection<ProductFound> _productFound;

        private SuppliersManagerClient _service;
        private ProductsManagerClient _productsManager;

        bool isSupplierSelected = false;
        bool isProductSelected = false;

        private SupplierDTO _supplierSelected { get; set; } = new SupplierDTO();
        private ProductDataContract _productSelected { get; set; } = new ProductDataContract();

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public GUI_RegisterPurchase()
        {
            InitializeComponent();
            _service = new SuppliersManagerClient();
            _productsManager = new ProductsManagerClient();
            DataContext = this;
            SuppliersFound = new ObservableCollection<SupplierFound>();
            ProductsFound = new ObservableCollection<ProductFound>();
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

        public ObservableCollection<ProductFound> ProductsFound
        {
            get => _productFound;
            set
            {
                _productFound = value;
                OnPropertyChanged(nameof(ProductsFound));
            }
        }

        private void FindProductItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is FindProductItem findProductItem)
            {
                findProductItem.selectProduct += OnSelectProduct;
            }
        }

        private void OnSelectProduct (object sender, ProductFound e)
        {
            if (!isProductSelected)
            {
                isProductSelected = true;
                _productSelected.ProductId = e.ProductID;
                _productSelected.ProductName = e.ProductName;
                _productSelected.ProductCode = e.ProductCode;
                _productSelected.Description = e.Description;
                _productSelected.PurchasePrice = e.PurchasePrice;
                _productSelected.SalePrice = e.SalePrice;
                _productSelected.Category = e.Category;
                _productSelected.Brand = e.Brand;
                _productSelected.Model = e.Model;
                _productSelected.Stock = e.Stock;
                _productSelected.Photo = e.Photo;
                _productSelected.Status = e.Status;
                _productSelected.HasSales = e.HasSales;
            }
            else
            {
                isProductSelected = false;
                _productSelected = new ProductDataContract();
            }
        }

        private void FindSupplierPurchaseItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is FindSupplierPurchaseItem findSupplierItem)
            {
                findSupplierItem.selectSupplier += OnSelectSupplier;
            }
        }

        private void OnSelectSupplier (object sender, SupplierFound e)
        {
            if (!isSupplierSelected)
            {
                isSupplierSelected = true;
                _supplierSelected.SupplierId = e.SupplierId;
            }
            else
            {
                isSupplierSelected = false;
                _supplierSelected = new SupplierDTO();
            }
        }

        private void btSearchSupplier_Click(object sender, RoutedEventArgs e)
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

        private void btSearchProduct_Click(object sender, RoutedEventArgs e)
        {
            ProductsFound.Clear();
            string search = tbSearchProduct.Text;
            if (!string.IsNullOrEmpty(search))
            {
                try
                {
                    ProductDataContract[] productsFound = _productsManager.GetProductsSearch(search);

                    foreach (ProductDataContract productFound in productsFound)
                    {
                        ProductsFound.Add(new ProductFound(productFound));
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

        private void btRegisterPurchase_Click(object sender, RoutedEventArgs e)
        {
            if (_supplierSelected.SupplierId != 0 || _productSelected.ProductId != 0)
            {


                if (e != null)
                {
                    var editDialog = new SelectQuantityDialog(new ProductFound(_productSelected))
                    {
                        Owner = Window.GetWindow(this)
                    };

                    bool? dialogResult = editDialog.ShowDialog();

                    if (dialogResult != true)
                    {
                        return;
                    }

                    PurchaseDTO purchaseDTO = new PurchaseDTO();
                    purchaseDTO.SupplierId = _supplierSelected.SupplierId;
                    purchaseDTO.ProductId = _productSelected.ProductId;
                    purchaseDTO.TotalCost = editDialog._totalCost;
                    purchaseDTO.PurchaseDate = DateTime.Now;

                    _productSelected.Stock += editDialog._quantity;

                    try
                    {
                        bool purchaseRegister = _service.RegisterPurchase(purchaseDTO);
                        bool stockUpdate = _productsManager.EditProduct(_productSelected);
                        if (purchaseRegister)
                        {
                            ProductsFound.Clear();
                            SuppliersFound.Clear();
                            var notificationDialog = new NotificationDialog();
                            notificationDialog.ShowSuccessNotification("EL nuevo stock de " + _productSelected.ProductName + " es de " + _productSelected.Stock + " items");
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
            else
            {
                var dialog = new NotificationDialog();
                dialog.ShowWarningNotification("Debes seleccionar un producto y un proveedor para continuar");
            }
        }
    }
}
