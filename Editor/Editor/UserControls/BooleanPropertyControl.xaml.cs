using System.Windows;
using System.Windows.Controls;

namespace Editor.UserControls
{
    /// <summary>
    /// Interaction logic for BooleanPropertyControl.xaml
    /// </summary>
    public partial class BooleanPropertyControl : UserControl
    {
        public BooleanPropertyControl()
        {
            InitializeComponent();
            ValueCheckBox.Checked += ValueCheckBox_CheckedChanged;
            ValueCheckBox.Unchecked += ValueCheckBox_CheckedChanged;
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

        public bool PropertyValue
        {
            get => (bool)GetValue(PropertyValueProperty);
            set => SetValue(PropertyValueProperty, value);
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title), typeof(string), typeof(BooleanPropertyControl),
            new PropertyMetadata(OnTitleChanged));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            nameof(Description), typeof(string), typeof(BooleanPropertyControl),
            new PropertyMetadata(OnDescriptionChanged));

        public static readonly DependencyProperty PropertyValueProperty = DependencyProperty.Register(
            nameof(PropertyValue), typeof(bool), typeof(BooleanPropertyControl),
            new PropertyMetadata(OnPropertyValueChanged));

        private static void OnPropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not BooleanPropertyControl view)
            {
                return;
            }

            view.ValueCheckBox.IsChecked = e.NewValue is bool isChecked 
                ? isChecked 
                : false;
        }

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is not BooleanPropertyControl view)
            {
                return;
            }

            view.TitleTextBlock.Text = e.NewValue as string ?? string.Empty;
        }

        private static void OnDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not BooleanPropertyControl view)
            {
                return;
            }

            view.DescriptionTextBlock.Text = e.NewValue as string ?? string.Empty;
        }

        private void ValueCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            PropertyValue = ValueCheckBox.IsChecked ?? false;
        }
    }
}
