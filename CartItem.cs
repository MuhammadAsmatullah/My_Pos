using My_Pos.Models;
using System.ComponentModel;

namespace My_Pos
{
    public class CartItem : INotifyPropertyChanged
    {
        private int _quantity;

        public Products Product { get; set; } = null!;
        public string ProductName => Product.Descript ?? "Unnamed";

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                    OnPropertyChanged(nameof(TotalPrice));
                }
            }
        }

        public int TotalPrice => Product != null ? Product.PriceA * Quantity : 0;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
