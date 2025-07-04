using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Editor.UserControls
{
    /// <summary>
    /// Interaction logic for EditTextControl.xaml
    /// </summary>
    public partial class EditTextControl : UserControl
    {
        public EditTextControl()
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

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title), typeof(string), typeof(EditTextControl),
            new PropertyMetadata(OnTitleChanged));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            nameof(Description), typeof(string), typeof(EditTextControl),
            new PropertyMetadata(OnDescriptionChanged));

        private static void OnDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not EditTextControl view)
            {
                return;
            }

            view.DescriptionTextBlock.Text = e.NewValue as string ?? string.Empty;
        }

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not EditTextControl view)
            {
                return;
            }

            view.TitleTextBlock.Text = e.NewValue as string ?? string.Empty;
        }
    }
}
