using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using My_Pos.Models;
using My_Pos.DbContexts;
using My_Pos.Commands;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace My_Pos.ViewModels
{
    public class CrustViewModel : INotifyPropertyChanged
    {
        private readonly SqlServerDbContext _dbContext;

        public ObservableCollection<CrustModel> Crust { get; set; }
        private CrustModel? _selectedCrust;

        public CrustModel? SelectedCrust
        {
            get => _selectedCrust;
            set
            {
                if (_selectedCrust != value)
                {
                    _selectedCrust = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SelectCrustCommand { get; }

        public CrustViewModel(SqlServerDbContext dbContext)
        {
            _dbContext = dbContext;

            var activeCrusts = _dbContext.Crust
                .Where(c => c.ISACTIVE)
                .ToList();

            Crust = new ObservableCollection<CrustModel>(activeCrusts);

            SelectCrustCommand = new RelayCommand(param =>
            {
                if (param is CrustModel selected)
                {
                    SelectedCrust = selected;
                    MessageBox.Show($"Selected Crust: {selected.NAME}");

                    // Close the CrustView window
                    Application.Current.Windows
                        .OfType<Window>()
                        .FirstOrDefault(w => w is Views.CrustView)
                        ?.Close();
                }
            });
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
