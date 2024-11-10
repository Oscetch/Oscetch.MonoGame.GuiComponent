using System.Windows;
using System.Windows.Controls;

namespace Editor.UserControls
{
    /// <summary>
    /// Interaction logic for FloatPropertyControl.xaml
    /// </summary>
    public partial class FloatPropertyControl : UserControl
    {
        public FloatPropertyControl()
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

        public float PropertyValue 
        { 
            get => (float)GetValue(PropertyValueProperty); 
            set => SetValue(PropertyValueProperty, value); 
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title), typeof(string), typeof(FloatPropertyControl), new PropertyMetadata());

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            nameof(Description), typeof(string), typeof(FloatPropertyControl));

        public static readonly DependencyProperty PropertyValueProperty = DependencyProperty.Register(
            nameof(PropertyValue), typeof(float), typeof(FloatPropertyControl), new PropertyMetadata(0f), ValidationCallback);

        private static bool ValidationCallback(object value)
        {
            return value is float;
        }
    }
}
