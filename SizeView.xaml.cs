using System.Linq;
using System.Windows;
using System.Windows.Threading;
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

            this.Loaded += (s, e) =>
            {
                var productView = Application.Current.Windows
                    .OfType<ProductView>()
                    .FirstOrDefault();

                Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new System.Action(() =>
                {
                    this.Width = 400;
                    this.Height = 300;

                    if (productView != null)
                    {
                        var centerPoint = productView.GetCenterOfProductColumn();

                        double left = centerPoint.X - (this.Width / 2);
                        double top = centerPoint.Y - (this.Height / 2);

                        // Clamp to screen bounds
                        left = Math.Max(0, Math.Min(left, SystemParameters.WorkArea.Width - this.Width));
                        top = Math.Max(0, Math.Min(top, SystemParameters.WorkArea.Height - this.Height));

                        this.WindowStartupLocation = WindowStartupLocation.Manual;
                        this.Left = left;
                        this.Top = top;
                    }
                    else
                    {
                        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    }
                }));
            };
        }

        private void ShowCrustViewAndClose()
        {
            var crustView = new CrustView
            {
                Width = 400,
                Height = 300
            };

            var productView = Application.Current.Windows
                .OfType<ProductView>()
                .FirstOrDefault();

            if (productView != null)
            {
                var centerPoint = productView.GetCenterOfProductColumn();

                double left = centerPoint.X - (crustView.Width / 2);
                double top = centerPoint.Y - (crustView.Height / 2);

                left = Math.Max(0, Math.Min(left, SystemParameters.WorkArea.Width - crustView.Width));
                top = Math.Max(0, Math.Min(top, SystemParameters.WorkArea.Height - crustView.Height));

                crustView.WindowStartupLocation = WindowStartupLocation.Manual;
                crustView.Left = left;
                crustView.Top = top;
            }
            else
            {
                crustView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }

            crustView.Show();
            this.Close();
        }
    }
}
