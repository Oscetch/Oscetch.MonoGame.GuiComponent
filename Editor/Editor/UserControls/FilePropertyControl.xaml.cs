using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace Editor.UserControls
{
    /// <summary>
    /// Interaction logic for FilePropertyControl.xaml
    /// </summary>
    public partial class FilePropertyControl : UserControl
    {
        public FilePropertyControl()
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

        public string PropertyValue 
        { 
            get => (string)GetValue(PropertyValueProperty); 
            set => SetValue(PropertyValueProperty, value); 
        }

        public string FileFilter 
        { 
            get => (string)GetValue(FileFilterProperty); 
            set => SetValue(FileFilterProperty, value);
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title), typeof(string), typeof(FilePropertyControl),
            new PropertyMetadata(OnTitleChanged));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            nameof(Description), typeof(string), typeof(FilePropertyControl),
            new PropertyMetadata(OnDescriptionChanged));

        public static readonly DependencyProperty PropertyValueProperty = DependencyProperty.Register(
            nameof(PropertyValue), typeof(string), typeof(FilePropertyControl),
            new PropertyMetadata(OnPropertyValueChanged));

        public static readonly DependencyProperty FileFilterProperty =
            DependencyProperty.Register(nameof(FileFilter), typeof(string), typeof(FilePropertyControl));

        private static void OnPropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is not FilePropertyControl view)
            {
                return;
            }

            view.FileTextBox.Text = e.NewValue as string ?? string.Empty;
        }

        private static void OnDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is not FilePropertyControl view)
            {
                return;
            }

            view.DescriptionTextBlock.Text = e.NewValue as string ?? string.Empty;
        }

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is not FilePropertyControl view)
            {
                return;
            }

            view.TitleTextBlock.Text = e.NewValue as string ?? string.Empty;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog 
            {
                Filter = FileFilter
            };

            var result = fileDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                PropertyValue = fileDialog.FileName;
            }
        }

        private void ClearFileButton_Click(object sender, RoutedEventArgs e)
        {
            PropertyValue = null;
        }
    }
}
