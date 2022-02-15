using Editor.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Editor.UserControls
{
    /// <summary>
    /// Interaction logic for TextPropertyControl.xaml
    /// </summary>
    public partial class TextPropertyControl : UserControl
    {
        public TextPropertyControl()
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

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title), typeof(string), typeof(TextPropertyControl),
            new PropertyMetadata(OnTitleChanged));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            nameof(Description), typeof(string), typeof(TextPropertyControl),
            new PropertyMetadata(OnDescriptionChanged));

        public static readonly DependencyProperty PropertyValueProperty = DependencyProperty.Register(
            nameof(PropertyValue), typeof(string), typeof(TextPropertyControl),
            new PropertyMetadata(OnPropertyValueChanged));

        private static void OnPropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TextPropertyControl view)
            {
                return;
            }

            view.ValueTextBox.Text = e.NewValue as string ?? string.Empty;
        }

        private static void OnDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TextPropertyControl view)
            {
                return;
            }

            view.DescriptionTextBlock.Text = e.NewValue as string ?? string.Empty;
        }

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is not TextPropertyControl view)
            {
                return;
            }

            view.TitleTextBlock.Text = e.NewValue as string ?? string.Empty;
        }
    }
}
