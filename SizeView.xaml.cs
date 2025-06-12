using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using My_Pos.ViewModels;

namespace My_Pos.Views
{
    public partial class SizeView : Window
    {
        private readonly SizeViewModel _viewModel;

        public SizeView()
        {
            InitializeComponent();

            _viewModel = App.ServiceProvider.GetRequiredService<SizeViewModel>();
            DataContext = _viewModel;

            _viewModel.OnSizeSelected = ShowCrustViewAndClose;
        }

        private void ShowCrustViewAndClose()
        {
            var crustView = new CrustView();
            crustView.Show();
            Application.Current.MainWindow = crustView;
            this.Close();
        }
    }
}
