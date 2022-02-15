using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Editor.UserControls
{
    /// <summary>
    /// Interaction logic for ColorPropertyControl.xaml
    /// </summary>
    public partial class ColorPropertyControl : UserControl
    {
        public ColorPropertyControl()
        {
            InitializeComponent();
            ColorPicker.ColorChanged += ColorPicker_ColorChanged;
        }

        public string Title 
        { 
            get => (string)GetValue(TitleProperty); 
            set => SetValue(TitleProperty, value); 
        }

        public string Description 
        { 
            get => (string)GetValue(DescriptionProperty); 
            set => SetValue(DescriptionProperty, value); 
        }

        public Color PropertyValue 
        { 
            get => (Color)GetValue(PropertyValueProperty); 
            set => SetValue(PropertyValueProperty, value); 
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title), typeof(string), typeof(ColorPropertyControl),
            new PropertyMetadata(OnTitleChanged));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            nameof(Description), typeof(string), typeof(ColorPropertyControl),
            new PropertyMetadata(OnDescriptionChanged));

        public static readonly DependencyProperty PropertyValueProperty = DependencyProperty.Register(
            nameof(PropertyValue), typeof(Color), typeof(ColorPropertyControl),
            new PropertyMetadata(OnPropertyValueChanged));

        private static void OnPropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ColorPropertyControl view)
            {
                return;
            }

            view.ColorPicker.SelectedColor = e.NewValue is Color newColor
                ? newColor
                : new Color();
        }

        private static void OnDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ColorPropertyControl view)
            {
                return;
            }

            view.DescriptionTextBlock.Text = e.NewValue as string ?? string.Empty;
        }

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is not ColorPropertyControl view)
            {
                return;
            }

            view.TitleTextBlock.Text = e.NewValue as string ?? string.Empty;
        }

        private void ColorPicker_ColorChanged(object sender, RoutedEventArgs e)
        {
            PropertyValue = ColorPicker.SelectedColor;
        }
    }
}
