using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Win32;

namespace SusunGambar;

public partial class MainWindow : Window
{
    private readonly DispatcherTimer _timer;
    private readonly Random _random = new();
    private BitmapSource? _sourceImage;
    private UniformGrid? _board;
    private List<Tile> _tiles = [];
    private int _gridSize = 3;
    private int? _selectedTile;
    private int _moves;
    private int _elapsedSeconds;
    private bool _isDragging;
    private bool _isDarkMode;
    private bool _isEnglish;
    private bool _isGameComplete;
    private int? _draggedTileIndex;
    private int _dragSource = -1;

    public MainWindow()
    {
        InitializeComponent();
        LanguageBox.SelectedIndex = 0;
        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += (_, _) =>
        {
            _elapsedSeconds++;
            UpdateTime();
        };
    }

    private void StartSample_Click(object sender, RoutedEventArgs e)
    {
        SetImage(CreateSampleImage());
    }

    private void ThemeButton_Click(object sender, RoutedEventArgs e)
    {
        _isDarkMode = !_isDarkMode;
        var dark = _isDarkMode;

        SetThemeBrush("WindowBackgroundBrush", dark ? Color.FromRgb(23, 25, 35) : Color.FromRgb(244, 245, 250));
        SetThemeBrush("SurfaceBrush", dark ? Color.FromRgb(34, 37, 50) : Colors.White);
        SetThemeBrush("BoardBackgroundBrush", dark ? Color.FromRgb(25, 28, 39) : Color.FromRgb(238, 240, 248));
        SetThemeBrush("TextPrimaryBrush", dark ? Color.FromRgb(241, 242, 248) : Color.FromRgb(32, 36, 58));
        SetThemeBrush("TextSecondaryBrush", dark ? Color.FromRgb(165, 169, 184) : Color.FromRgb(119, 125, 145));
        SetThemeBrush("DisabledTextBrush", dark ? Color.FromRgb(184, 188, 202) : Color.FromRgb(100, 106, 124));
        SetThemeBrush("TextBodyBrush", dark ? Color.FromRgb(208, 210, 220) : Color.FromRgb(85, 91, 112));
        SetThemeBrush("FooterTextBrush", dark ? Color.FromRgb(135, 140, 158) : Color.FromRgb(150, 155, 173));
        SetThemeBrush("SoftSurfaceBrush", dark ? Color.FromRgb(44, 48, 64) : Color.FromRgb(246, 247, 251));
        SetThemeBrush("SoftAccentBrush", dark ? Color.FromRgb(48, 52, 81) : Color.FromRgb(238, 240, 255));
        SetThemeBrush("AccentTextBrush", dark ? Color.FromRgb(174, 180, 255) : Color.FromRgb(77, 88, 216));
        SetThemeBrush("AccentBrush", dark ? Color.FromRgb(124, 131, 255) : Color.FromRgb(91, 103, 241));
        SetThemeBrush("SubtleBorderBrush", dark ? Color.FromRgb(58, 62, 80) : Color.FromRgb(226, 228, 237));
        SetThemeBrush("TileBorderBrush", dark ? Color.FromRgb(65, 70, 90) : Colors.White);
        ApplyLanguage();
    }

    private void SetThemeBrush(string key, Color color)
    {
        Resources[key] = new SolidColorBrush(color);
    }

    private void LanguageBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _isEnglish = LanguageBox.SelectedItem is ComboBoxItem { Tag: "en" };
        ApplyLanguage();
    }

    private void ApplyLanguage()
    {
        var english = _isEnglish;
        Title = english ? "Picture Puzzle" : "Susun Gambar";
        HeaderEyebrow.Text = english ? "PICTURE PUZZLE" : "PUZZLE GAMBAR";
        HeaderTitle.Text = english ? "Put the pieces together." : "Susun potongannya.";
        HeaderSubtitle.Text = english
            ? "Choose your favorite picture, then put it back together."
            : "Pilih gambar favoritmu, lalu susun kembali sampai utuh.";
        ThemeButton.Content = _isDarkMode
            ? (english ? "☀  Light mode" : "☀  Mode terang")
            : (english ? "🌙  Dark mode" : "🌙  Mode gelap");
        LanguageBox.ToolTip = english ? "Choose language" : "Pilih bahasa";
        EmptyTitle.Text = english ? "Ready to play?" : "Siap bermain?";
        EmptyDescription.Text = english
            ? "Start with the sample picture, or choose one of your own."
            : "Mulai dengan gambar contoh, atau pilih fotomu sendiri.";
        StartSampleButton.Content = english ? "Play with sample picture" : "Mulai dengan gambar contoh";
        SettingsTitle.Text = english ? "SETTINGS" : "PENGATURAN";
        ChooseImageButton.Content = english ? "Choose Picture" : "Pilih Gambar";
        DifficultyLabel.Text = english ? "Difficulty" : "Tingkat kesulitan";
        ((ComboBoxItem)DifficultyBox.Items[0]).Content = english ? "Easy — 3 × 3" : "Mudah — 3 × 3";
        ((ComboBoxItem)DifficultyBox.Items[1]).Content = english ? "Medium — 4 × 4" : "Sedang — 4 × 4";
        ((ComboBoxItem)DifficultyBox.Items[2]).Content = english ? "Hard — 5 × 5" : "Sulit — 5 × 5";
        ShuffleButton.Content = english ? "Shuffle pieces" : "Acak potongan";
        MovesLabel.Text = english ? "MOVES" : "LANGKAH";
        TimeLabel.Text = english ? "TIME" : "WAKTU";
        HowToTitle.Text = english ? "HOW TO PLAY" : "CARA BERMAIN";
        HowToText.Text = english
            ? "Click two neighboring pieces to swap them, or drag one piece onto a neighbor."
            : "Klik dua potongan yang berdampingan untuk menukar posisinya, atau tarik satu potongan ke tetangganya.";
        PreviewButton.Content = english ? "View full picture" : "Lihat gambar utuh";
        StatusText.Text = GetStatusText();
        FooterText.Text = english
            ? "Picture Puzzle  •  Play with your favorite pictures"
            : "Susun Gambar  •  Mainkan dengan gambar favoritmu";

        if (_board is not null)
        {
            foreach (var child in _board.Children)
            {
                if (child is Button tileButton)
                {
                    tileButton.ToolTip = english
                        ? "Click to select, then click a neighbor to swap; or drag onto a neighbor"
                        : "Klik untuk memilih, lalu klik potongan di sebelahnya; atau seret ke tetangganya";
                }
            }
        }
    }

    private string GetStatusText()
    {
        if (_sourceImage is null)
        {
            return _isEnglish ? "Choose a picture to get started." : "Pilih gambar untuk memulai.";
        }

        if (_isGameComplete)
        {
            return _isEnglish
                ? $"Solved in {_moves} moves!"
                : $"Selesai dalam {_moves} langkah!";
        }

        return _isEnglish
            ? $"{_gridSize} × {_gridSize} pieces — you can do it!"
            : $"{_gridSize} × {_gridSize} potongan — semangat!";
    }

    private void ChooseImage_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Title = _isEnglish ? "Choose a puzzle picture" : "Pilih gambar untuk puzzle",
            Filter = _isEnglish
                ? "Image files|*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.tif;*.tiff"
                : "File gambar|*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.tif;*.tiff",
            CheckFileExists = true
        };

        if (dialog.ShowDialog(this) != true)
        {
            return;
        }

        try
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(dialog.FileName);
            image.EndInit();
            image.Freeze();
            SetImage(image);
        }
        catch (Exception exception) when (exception is IOException or NotSupportedException or
                                           FormatException or InvalidOperationException)
        {
            MessageBox.Show(this,
                _isEnglish
                    ? $"The picture could not be opened.\n\n{exception.Message}"
                    : $"Gambar tidak dapat dibuka.\n\n{exception.Message}",
                _isEnglish ? "Unable to open picture" : "Gagal membuka gambar",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void SetImage(BitmapSource image)
    {
        var side = Math.Min(image.PixelWidth, image.PixelHeight);
        var crop = new CroppedBitmap(image, new Int32Rect(
            (image.PixelWidth - side) / 2,
            (image.PixelHeight - side) / 2,
            side,
            side));
        crop.Freeze();
        _sourceImage = crop;
        PreviewButton.IsEnabled = true;
        ShuffleButton.IsEnabled = true;
        EmptyState.Visibility = Visibility.Collapsed;
        NewGame();
    }

    private void Shuffle_Click(object sender, RoutedEventArgs e)
    {
        if (_sourceImage is not null)
        {
            NewGame();
        }
    }

    private void DifficultyBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DifficultyBox.SelectedItem is ComboBoxItem { Tag: string value } &&
            int.TryParse(value, out var size))
        {
            _gridSize = size;
            if (_sourceImage is not null)
            {
                NewGame();
            }
        }
    }

    private void Preview_Click(object sender, RoutedEventArgs e)
    {
        if (_sourceImage is not null)
        {
            new PreviewWindow(_sourceImage, _isDarkMode, _isEnglish, this).ShowDialog();
        }
    }

    private void NewGame()
    {
        if (_sourceImage is null)
        {
            return;
        }

        _timer.Stop();
        _elapsedSeconds = 0;
        _moves = 0;
        _selectedTile = null;
        _isGameComplete = false;
        _isDragging = false;
        _dragSource = -1;
        UpdateTime();
        MovesText.Text = "0";
        var tileCount = _gridSize * _gridSize;
        _tiles = Enumerable.Range(0, tileCount)
            .Select(CreateTile)
            .ToList();
        ShuffleTiles();
        RenderBoard();
        _timer.Start();
        StatusText.Text = GetStatusText();
    }

    private Tile CreateTile(int index)
    {
        var imageSize = _sourceImage!.PixelWidth;
        var row = index / _gridSize;
        var column = index % _gridSize;
        var left = column * imageSize / _gridSize;
        var top = row * imageSize / _gridSize;
        var right = (column + 1) * imageSize / _gridSize;
        var bottom = (row + 1) * imageSize / _gridSize;
        var bitmap = new CroppedBitmap(_sourceImage, new Int32Rect(
            left,
            top,
            right - left,
            bottom - top));
        bitmap.Freeze();
        return new Tile(index, bitmap);
    }

    private void ShuffleTiles()
    {
        for (var index = _tiles.Count - 1; index > 0; index--)
        {
            var other = _random.Next(index + 1);
            (_tiles[index], _tiles[other]) = (_tiles[other], _tiles[index]);
        }

        if (IsSolved())
        {
            (_tiles[0], _tiles[1]) = (_tiles[1], _tiles[0]);
        }
    }

    private void RenderBoard()
    {
        _board = new UniformGrid { Rows = _gridSize, Columns = _gridSize };
        foreach (var (tile, index) in _tiles.Select((value, index) => (value, index)))
        {
            _board.Children.Add(CreateTileButton(tile, index));
        }

        BoardHost.Children.Clear();
        BoardHost.Children.Add(_board);
    }

    private Button CreateTileButton(Tile tile, int index)
    {
        var button = new Button
        {
            Tag = index,
            Padding = new Thickness(0),
            Margin = new Thickness(2),
            BorderThickness = new Thickness(3),
            BorderBrush = (Brush)FindResource("TileBorderBrush"),
            Background = (Brush)FindResource("TileBorderBrush"),
            Cursor = Cursors.Hand,
            AllowDrop = true,
            ToolTip = _isEnglish
                ? "Click to select, then click a neighbor to swap; or drag onto a neighbor"
                : "Klik untuk memilih, lalu klik potongan di sebelahnya; atau seret ke tetangganya"
        };
        button.Content = new Image
        {
            Source = tile.Image,
            Stretch = Stretch.Fill,
            IsHitTestVisible = false
        };
        button.Click += Tile_Click;
        button.PreviewMouseLeftButtonDown += Tile_MouseDown;
        button.PreviewMouseMove += Tile_MouseMove;
        button.PreviewMouseLeftButtonUp += Tile_MouseUp;
        button.Drop += Tile_Drop;
        button.DragOver += Tile_DragOver;
        return button;
    }

    private void Tile_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button { Tag: int index } button)
        {
            return;
        }

        if (_draggedTileIndex == index)
        {
            _draggedTileIndex = null;
            return;
        }

        if (_selectedTile is null)
        {
            _selectedTile = index;
            button.BorderBrush = (Brush)FindResource("AccentBrush");
            return;
        }

        if (_selectedTile == index)
        {
            _selectedTile = null;
            button.BorderBrush = (Brush)FindResource("TileBorderBrush");
            return;
        }

        var first = _selectedTile.Value;
        if (GetAdjacentIndices(first).Contains(index))
        {
            SwapTiles(first, index);
            return;
        }

        if (_board?.Children[first] is Button previousButton)
        {
            previousButton.BorderBrush = (Brush)FindResource("TileBorderBrush");
        }

        _selectedTile = index;
        button.BorderBrush = (Brush)FindResource("AccentBrush");
    }

    private void Tile_MouseDown(object sender, MouseButtonEventArgs e)
    {
        _draggedTileIndex = null;
        if (sender is Button { Tag: int index } button)
        {
            _dragSource = index;
            _isDragging = true;
            button.CaptureMouse();
        }
    }

    private void Tile_MouseMove(object sender, MouseEventArgs e)
    {
        if (!_isDragging || e.LeftButton != MouseButtonState.Pressed || _dragSource < 0 ||
            sender is not Button button)
        {
            return;
        }

        _isDragging = false;
        button.ReleaseMouseCapture();
        var source = _dragSource;
        _dragSource = -1;
        _draggedTileIndex = source;
        DragDrop.DoDragDrop(button, source, DragDropEffects.Move);
    }

    private void Tile_MouseUp(object sender, MouseButtonEventArgs e)
    {
        _isDragging = false;
        _dragSource = -1;
        if (sender is Button button)
        {
            button.ReleaseMouseCapture();
        }
    }

    private void Tile_DragOver(object sender, DragEventArgs e)
    {
        var canMove = sender is FrameworkElement { Tag: int targetIndex } &&
                      e.Data.GetData(typeof(int)) is int source &&
                      source >= 0 && source < _tiles.Count &&
                      targetIndex != source &&
                      GetAdjacentIndices(source).Contains(targetIndex);
        e.Effects = canMove ? DragDropEffects.Move : DragDropEffects.None;
        e.Handled = true;
    }

    private void Tile_Drop(object sender, DragEventArgs e)
    {
        if (sender is FrameworkElement { Tag: int targetIndex } &&
            e.Data.GetData(typeof(int)) is int source &&
            source >= 0 && source < _tiles.Count &&
            targetIndex != source &&
            GetAdjacentIndices(source).Contains(targetIndex))
        {
            SwapTiles(source, targetIndex);
        }
    }

    private void SwapTiles(int first, int second)
    {
        if (!GetAdjacentIndices(first).Contains(second))
        {
            return;
        }

        (_tiles[first], _tiles[second]) = (_tiles[second], _tiles[first]);
        _selectedTile = null;
        _draggedTileIndex = null;
        _moves++;
        MovesText.Text = _moves.ToString();
        RenderBoard();
        CheckForWin();
    }

    private IEnumerable<int> GetAdjacentIndices(int index)
    {
        var row = index / _gridSize;
        var column = index % _gridSize;
        if (row > 0)
        {
            yield return index - _gridSize;
        }

        if (row < _gridSize - 1)
        {
            yield return index + _gridSize;
        }

        if (column > 0)
        {
            yield return index - 1;
        }

        if (column < _gridSize - 1)
        {
            yield return index + 1;
        }
    }

    private bool IsSolved()
    {
        return _tiles.Select((tile, index) => tile.CorrectIndex == index).All(correct => correct);
    }

    private void CheckForWin()
    {
        if (!IsSolved())
        {
            return;
        }

        _timer.Stop();
        _isGameComplete = true;
        StatusText.Text = GetStatusText();
        MessageBox.Show(this,
            _isEnglish
                ? $"Great job! You solved the picture in {_moves} moves and {TimeText.Text}."
                : $"Hebat! Kamu berhasil menyusun gambar dalam {_moves} langkah dan {TimeText.Text}.",
            _isEnglish ? "Puzzle complete" : "Puzzle selesai",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void UpdateTime()
    {
        TimeText.Text = TimeSpan.FromSeconds(_elapsedSeconds).ToString(@"mm\:ss");
    }

    private BitmapSource CreateSampleImage()
    {
        const int size = 900;
        var visual = new DrawingVisual();
        using (var context = visual.RenderOpen())
        {
            context.DrawRectangle(new LinearGradientBrush(
                    Color.FromRgb(255, 190, 132), Color.FromRgb(103, 128, 196),
                    new Point(0.5, 0), new Point(0.5, 1)),
                null, new Rect(0, 0, size, size));
            context.DrawEllipse(new SolidColorBrush(Color.FromRgb(255, 232, 157)),
                null, new Point(660, 240), 100, 100);
            context.DrawGeometry(new SolidColorBrush(Color.FromRgb(95, 116, 166)), null,
                Polygon((0, 600), (220, 320), (430, 600)));
            context.DrawGeometry(new SolidColorBrush(Color.FromRgb(71, 96, 142)), null,
                Polygon((300, 610), (590, 300), (900, 630)));
            context.DrawGeometry(new SolidColorBrush(Color.FromRgb(71, 135, 125)), null,
                Polygon((0, 640), (210, 510), (430, 660), (650, 490), (900, 660), (900, 900), (0, 900)));
            context.DrawRectangle(new SolidColorBrush(Color.FromRgb(245, 215, 173)),
                null, new Rect(295, 540, 300, 260));
            context.DrawGeometry(new SolidColorBrush(Color.FromRgb(183, 104, 87)), null,
                Polygon((260, 550), (445, 390), (630, 550)));
            context.DrawRectangle(new SolidColorBrush(Color.FromRgb(102, 75, 79)),
                null, new Rect(418, 650, 66, 150));
            context.DrawRectangle(new SolidColorBrush(Color.FromRgb(112, 165, 186)),
                null, new Rect(330, 595, 68, 72));
            context.DrawRectangle(new SolidColorBrush(Color.FromRgb(112, 165, 186)),
                null, new Rect(490, 595, 68, 72));
            DrawTree(context, 180, 700);
            DrawTree(context, 720, 690);
        }

        var bitmap = new RenderTargetBitmap(size, size, 96, 96, PixelFormats.Pbgra32);
        bitmap.Render(visual);
        bitmap.Freeze();
        return bitmap;
    }

    private static Geometry Polygon(params (double X, double Y)[] points)
    {
        var geometry = new StreamGeometry();
        using (var context = geometry.Open())
        {
            context.BeginFigure(new Point(points[0].X, points[0].Y), true, true);
            context.PolyLineTo(points.Skip(1).Select(point => new Point(point.X, point.Y)).ToArray(),
                true, false);
        }

        geometry.Freeze();
        return geometry;
    }

    private static void DrawTree(DrawingContext context, double x, double y)
    {
        context.DrawRectangle(new SolidColorBrush(Color.FromRgb(105, 78, 70)),
            null, new Rect(x - 12, y, 24, 110));
        context.DrawEllipse(new SolidColorBrush(Color.FromRgb(49, 103, 91)),
            null, new Point(x, y - 25), 72, 78);
    }

    private sealed record Tile(int CorrectIndex, BitmapSource Image);
}
