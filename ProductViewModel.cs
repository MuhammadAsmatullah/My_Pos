using Microsoft.Extensions.DependencyInjection;
using My_Pos.Commands;
using My_Pos.Models;
using My_Pos.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace My_Pos.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private readonly ProductService _productService;
        private bool _isLoading = false;

        private ObservableCollection<Products> _products;
        public ObservableCollection<Products> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        private ObservableCollection<Products> _filteredFoodItems;
        public ObservableCollection<Products> FilteredFoodItems
        {
            get => _filteredFoodItems;
            set
            {
                _filteredFoodItems = value;
                OnPropertyChanged(nameof(FilteredFoodItems));
            }
        }

        private ObservableCollection<CategoryViewModel> _categories;
        public ObservableCollection<CategoryViewModel> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        private ObservableCollection<CartItem> _cartItems = new ObservableCollection<CartItem>();
        public ObservableCollection<CartItem> CartItems
        {
            get => _cartItems;
            set
            {
                _cartItems = value;
                OnPropertyChanged(nameof(CartItems));
            }
        }

        private decimal _subtotal;
        public decimal Subtotal
        {
            get => _subtotal;
            private set
            {
                if (_subtotal != value)
                {
                    _subtotal = value;
                    OnPropertyChanged(nameof(Subtotal));
                }
            }
        }

        private decimal _total;
        public decimal Total
        {
            get => _total;
            private set
            {
                if (_total != value)
                {
                    _total = value;
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        public ICommand FilterProductCommand { get; }
        public ICommand AddToCartCommand { get; }
        public ICommand IncreaseQuantityCommand { get; }
        public ICommand DecreaseQuantityCommand { get; }
        public ICommand RemoveFromCartCommand { get; }



        public ProductViewModel(ProductService productService)
        {
            _productService = productService;

            FilterProductCommand = new RelayCommand<object>(async (param) => await FilterProducts(param));
            AddToCartCommand = new RelayCommand<Products>(AddToCart);
            IncreaseQuantityCommand = new RelayCommand<CartItem>(IncreaseQuantity);
            DecreaseQuantityCommand = new RelayCommand<CartItem>(DecreaseQuantity);
            RemoveFromCartCommand = new RelayCommand<CartItem>(RemoveFromCart);



            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await LoadProducts();
            await LoadAndSetCategoriesAsync();
        }

        private async Task LoadProducts()
        {
            if (_isLoading) return;
            _isLoading = true;

            try
            {
                var result = await _productService.GetAllProductsAsync();
                Products = new ObservableCollection<Products>(result);
                FilteredFoodItems = Products;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading products: {ex.Message}");
            }
            finally
            {
                _isLoading = false;
            }
        }

        private async Task<List<Category>> LoadCategoriesAsync()
        {
            using var scope = App.ServiceProvider.CreateScope();
            var productService = scope.ServiceProvider.GetRequiredService<ProductService>();

            return await productService.GetAllCategoriesAsync();
        }

        private async Task LoadAndSetCategoriesAsync()
        {
            try
            {
                var categories = await LoadCategoriesAsync();
                var categoryViewModels = categories.Select(c => new CategoryViewModel(c)).ToList();
                Categories = new ObservableCollection<CategoryViewModel>(categoryViewModels);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading categories: {ex.Message}");
                Categories = new ObservableCollection<CategoryViewModel>(); // fallback empty collection
            }
        }

        public async Task LoadProductsByDescript(string descript)
        {
            if (_isLoading) return;
            _isLoading = true;

            try
            {
                var result = await _productService.GetAllProductsByDescriptAsync(descript);
                FilteredFoodItems = new ObservableCollection<Products>(result);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading filtered products: {ex.Message}");
            }
            finally
            {
                _isLoading = false;
            }
        }

        private async Task FilterProducts(object categoryId)
        {
            if (categoryId == null) return;

            string idStr = categoryId.ToString();
            if (!string.IsNullOrEmpty(idStr))
            {
                await LoadProductsByDescript(idStr);
            }
        }

        private void AddToCart(Products product)
        {
            if (product == null)
                return;

            var existingItem = CartItems.FirstOrDefault(c => c.Product.ProductID == product.ProductID);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                CartItems.Add(new CartItem { Product = product, Quantity = 1 });
            }

            UpdateTotals();
        }

        private void IncreaseQuantity(CartItem item)
        {
            if (item == null) return;

            item.Quantity++;
            UpdateTotals();
        }

        private void DecreaseQuantity(CartItem item)
        {
            if (item == null) return;

            if (item.Quantity > 1)
            {
                item.Quantity--;
            }
            else
            {
                CartItems.Remove(item);
            }

            UpdateTotals();
        }

        private void UpdateTotals()
        {
            Subtotal = CartItems.Sum(c => c.TotalPrice);
            Total = Subtotal; // Add tax/shipping logic here if needed
        }
        private void RemoveFromCart(CartItem item)
        {
            if (item == null) return;

            CartItems.Remove(item);
            UpdateTotals();
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
