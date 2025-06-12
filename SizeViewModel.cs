using My_Pos.Models;
using My_Pos.DbContexts;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using My_Pos.Commands;

namespace My_Pos.ViewModels
{
    public class SizeViewModel : INotifyPropertyChanged
    {
        private readonly SqlServerDbContext _context;

        public ObservableCollection<SizeModel> Sizes { get; set; } = new ObservableCollection<SizeModel>();

        private SizeModel? _selectedSize;
        public SizeModel? SelectedSize
        {
            get => _selectedSize;
            set
            {
                _selectedSize = value;
                OnPropertyChanged(nameof(SelectedSize));

                if (_selectedSize != null)
                {
                    OnSizeSelected?.Invoke();
                }
            }
        }

        public ICommand SelectSizeCommand { get; }

        public Action? OnSizeSelected { get; set; }

        public SizeViewModel(SqlServerDbContext context)
        {
            _context = context;
            LoadSizes();
            SelectSizeCommand = new RelayCommand(SelectSize);
        }

        private void SelectSize(object parameter)
        {
            if (parameter is SizeModel size)
            {
                SelectedSize = size;
            }
        }

        private void LoadSizes()
        {
            try
            {
                var activeSizes = _context.Sizes.Where(s => s.ISACTIVE).ToList();
                Sizes = new ObservableCollection<SizeModel>(activeSizes);
                OnPropertyChanged(nameof(Sizes));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Failed to load sizes: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
