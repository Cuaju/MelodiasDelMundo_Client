﻿using MelodiasDelMundo_Client.Views.Report;
using MelodiasDelMundo_Client.Views.Sale;
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
    /// Lógica de interacción para GUI_ReportesMenu.xaml
    /// </summary>
    public partial class GUI_ReportesMenu : Window
    {
        public GUI_ReportesMenu()
        {
            InitializeComponent();
        }

        private void BtReporteVentasProducto_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new GUI_ReporteVentasProducto();
            ventana.Show();
            this.Close();
        }

        private void BtReporteVentasCategoria_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new GUI_ReporteVentasCategoria();
            ventana.Show();
            this.Close();
        }

        private void BtReporteInventario_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtReporteGanancias_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new ReportSales();
            ventana.Show();
            this.Close();
        }

        private void StackPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var ventana = new MMenu();
            ventana.Show();
            this.Close();
        }
    }
}