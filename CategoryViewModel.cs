using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

public class CategoryViewModel : INotifyPropertyChanged
{
    public int CategoryID { get; set; }
    public string Descript { get; set; }

    private ImageSource _image;
    public ImageSource Image
    {
        get => _image;
        set
        {
            if (_image != value)
            {
                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }
    }

    public CategoryViewModel(Category category)
    {
        CategoryID = category.CategoryID;
        Descript = category.Descript;

        try
        {
            Image = ConvertToImageSource(category.Image); // category.Image is byte[] from DB
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading image for CategoryID {CategoryID}: {ex.Message}");
            Image = null;
        }
    }

    private ImageSource ConvertToImageSource(byte[] imageData)
    {
        if (imageData == null || imageData.Length == 0)
            return null;

        try
        {
            using var ms = new MemoryStream(imageData);
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = ms;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze();
            return bitmap;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Image conversion failed: {ex.Message}");
            return null;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
