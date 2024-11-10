using System.Windows;
using System.Windows.Controls;

namespace Editor.UserControls
{
    /// <summary>
    /// Interaction logic for Vector2PropertyControl.xaml
    /// </summary>
    public partial class Vector2PropertyControl : UserControl
    {
        public Vector2PropertyControl()
        {
            InitializeComponent();
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

        public float PropertyXValue 
        { 
            get => (float)GetValue(PropertyXValueProperty); 
            set => SetValue(PropertyXValueProperty, value); 
        }

        public float PropertyYValue 
        {
            get => (float)GetValue(PropertyYValueProperty); 
            set => SetValue(PropertyYValueProperty, value); 
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title), typeof(string), typeof(Vector2PropertyControl),
            new PropertyMetadata(OnTitleChanged));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            nameof(Description), typeof(string), typeof(Vector2PropertyControl),
            new PropertyMetadata(OnDescriptionChanged));

        public static readonly DependencyProperty PropertyXValueProperty = DependencyProperty.Register(
            nameof(PropertyXValue), typeof(float), typeof(Vector2PropertyControl));

        public static readonly DependencyProperty PropertyYValueProperty = DependencyProperty.Register(
            nameof(PropertyYValue), typeof(float), typeof(Vector2PropertyControl));

        private static void OnDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is not Vector2PropertyControl view)
            {
                return;
            }

            view.DescriptionTextBlock.Text = e.NewValue as string ?? string.Empty;
        }

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is not Vector2PropertyControl view)
            {
                return;
            }

            view.TitleTextBlock.Text = e.NewValue as string ?? string.Empty;
        }
    }
}
