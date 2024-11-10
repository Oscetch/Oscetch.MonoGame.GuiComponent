using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Editor.UserControls
{
    /// <summary>
    /// Interaction logic for ButtonPropertyControl.xaml
    /// </summary>
    public partial class ButtonPropertyControl : UserControl
    {
        public ButtonPropertyControl()
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

        public string ButtonText
        {
            get => (string)GetValue(ButtonTextProperty);
            set => SetValue(ButtonTextProperty, value);
        }

        public ICommand ButtonCommand
        {
            get => (ICommand)GetValue(ButtonCommandProperty);
            set => SetValue(ButtonCommandProperty, value);
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title), typeof(string), typeof(ButtonPropertyControl));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            nameof(Description), typeof(string), typeof(ButtonPropertyControl));

        public static readonly DependencyProperty ButtonTextProperty = DependencyProperty.Register(
            nameof(ButtonText), typeof(string), typeof(ButtonPropertyControl));

        public static readonly DependencyProperty ButtonCommandProperty = DependencyProperty.Register(
            nameof(ButtonCommand), typeof(ICommand), typeof(ButtonPropertyControl));
    }
}
