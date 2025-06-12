using My_Pos.Models; // Access Products class
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Linq;
using My_Pos.Commands;  // For RelayCommand<T>

namespace My_Pos.ViewModels
{
    public class POSViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Products> Products { get; set; }
        public ObservableCollection<Products> FilteredProducts { get; set; }
        public ObservableCollection<CartItem> CartItems { get; set; }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    FilterProducts(_searchText);
                }
            }
        }

        public ICommand AddToCartCommand { get; }

        public POSViewModel()
        {
            Products = new ObservableCollection<Products>();
            FilteredProducts = new ObservableCollection<Products>();
            CartItems = new ObservableCollection<CartItem>();

           // LoadProducts();

            AddToCartCommand = new RelayCommand<Products>(AddToCart);
        }

      /*  private void LoadProducts()
        {
            Products.Add(new Products { ProductID = 1, Descript = "Burger", PriceA = 5 });
            Products.Add(new Products { ProductID = 2, Descript = "Pizza", PriceA = 8 });
            //Products.Add(new Products { ProductID = 3, Descript = "Drinks", PriceA = 3 });

            FilterProducts(string.Empty);
        }
      */
        private void FilterProducts(string search)
        {
            FilteredProducts.Clear();
            foreach (var product in Products)
            {
                if (string.IsNullOrEmpty(search) || product.Descript?.ToLower().Contains(search.ToLower()) == true)
                {
                    FilteredProducts.Add(product);
                }
            }
        }

        private void AddToCart(Products product)
        {
            if (product == null) return;

            var existingItem = CartItems.FirstOrDefault(ci => ci.Product.ProductID == product.ProductID);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                CartItems.Add(new CartItem { Product = product, Quantity = 1 });
            }

            OnPropertyChanged(nameof(CartItems));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
