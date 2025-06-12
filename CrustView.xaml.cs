using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using My_Pos.ViewModels;

namespace My_Pos.Views
{
    public partial class CrustView : Window
    {
        public CrustView()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider?.GetRequiredService<CrustViewModel>();
        }
    }
}
