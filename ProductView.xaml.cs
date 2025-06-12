using Microsoft.Extensions.DependencyInjection;
using My_Pos.Models;
using My_Pos.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace My_Pos.Views
{
    public partial class ProductView : Window
    {
        public ProductView()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider?.GetRequiredService<ProductViewModel>();
        }

        private void ProductItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is Products product)
            {
                // If the clicked product is a pizza (e.g., ReportCatId == 2)
                if (product.ReportCatId == 2)
                {
                    // Use DI to create and show the SizeView
                    var sizeWindow = App.ServiceProvider?.GetRequiredService<SizeView>();
                    sizeWindow?.ShowDialog();
                    return;
                }

                // Add non-pizza products directly to the cart
                if (DataContext is ProductViewModel viewModel &&
                    viewModel.AddToCartCommand.CanExecute(product))
                {
                    viewModel.AddToCartCommand.Execute(product);
                }
            }
        }
    }
}
