using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SusunGambar;

public partial class PreviewWindow : Window
{
    public PreviewWindow(BitmapSource image, bool isDarkMode, bool isEnglish, Window owner)
    {
        InitializeComponent();
        Owner = owner;
        FullImage.Source = image;

        var dark = isDarkMode;
        SetBrush("PreviewBackgroundBrush",
            dark ? Color.FromRgb(23, 25, 35) : Color.FromRgb(244, 245, 250));
        SetBrush("PreviewSurfaceBrush",
            dark ? Color.FromRgb(34, 37, 50) : Colors.White);
        SetBrush("PreviewMutedBrush",
            dark ? Color.FromRgb(165, 169, 184) : Color.FromRgb(119, 125, 145));
        SetBrush("PreviewBorderBrush",
            dark ? Color.FromRgb(58, 62, 80) : Color.FromRgb(226, 228, 237));
        Title = isEnglish ? "Full Picture" : "Gambar utuh";
        PreviewTitle.Text = isEnglish ? "FULL PICTURE" : "GAMBAR UTUH";
        PreviewDescription.Text = isEnglish
            ? "Use this picture as a reference."
            : "Gunakan gambar ini sebagai petunjuk.";
        CloseButton.Content = isEnglish ? "Close" : "Tutup";

        var workArea = SystemParameters.WorkArea;
        Width = Math.Min(640, workArea.Width * 0.8);
        Height = Math.Min(700, workArea.Height * 0.85);
    }

    private void SetBrush(string key, Color color)
    {
        Resources[key] = new SolidColorBrush(color);
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
