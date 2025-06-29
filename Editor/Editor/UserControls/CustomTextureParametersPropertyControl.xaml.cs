using Ninject.Parameters;
using Oscetch.MonoGame.Textures.Enums;
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
    /// Interaction logic for CustomTextureParametersPropertyControl.xaml
    /// </summary>
    public partial class CustomTextureParametersPropertyControl : UserControl
    {
        public CustomTextureParametersPropertyControl()
        {
            InitializeComponent();
            FillColorPicker.ColorChanged += ColorPicker_ColorChanged;
            BorderColorPicker.ColorChanged += BorderColorPicker_ColorChanged;
        }

        private void BorderColorPicker_ColorChanged(object sender, RoutedEventArgs e)
        {
            PropertyBorderColor = BorderColorPicker.SelectedColor;
        }

        private void ColorPicker_ColorChanged(object sender, RoutedEventArgs e)
        {
            PropertyColor = FillColorPicker.SelectedColor;
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

        public int PropertyWidthValue
        {
            get => (int)GetValue(PropertyWidthValueProperty);
            set => SetValue(PropertyWidthValueProperty, value);
        }

        public int PropertyHeightValue
        {
            get => (int)GetValue(PropertyHeightValueProperty);
            set => SetValue(PropertyHeightValueProperty, value);
        }

        public ShapeType PropertyShapeType
        {
            get => (ShapeType)GetValue(PropertyShapeTypeProperty);
            set => SetValue(PropertyShapeTypeProperty, value);
        }

        public Color PropertyColor
        {
            get => (Color)GetValue(PropertyColorProperty);
            set => SetValue(PropertyColorProperty, value);
        }

        public int PropertyCornerRadius
        {
            get => (int)GetValue(PropertyCornerRadiusProperty);
            set => SetValue(PropertyCornerRadiusProperty, value);
        }

        public string ToControlSizeButtonText
        {
            get => (string)GetValue(ToControlSizeButtonTextProperty);
            set => SetValue(ToControlSizeButtonTextProperty, value);
        }

        public ICommand ToControlSizeButtonCommand
        {
            get => (ICommand)GetValue(ToControlSizeButtonCommandProperty);
            set => SetValue(ToControlSizeButtonCommandProperty, value);
        }

        public int PropertyBorderThickness
        {
            get => (int)GetValue(PropertyBorderThicknessProperty);
            set => SetValue(PropertyBorderThicknessProperty, value);
        }

        public Color PropertyBorderColor
        {
            get => (Color)GetValue(PropertyBorderColorProperty);
            set => SetValue(PropertyBorderColorProperty, value);
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title), typeof(string), typeof(CustomTextureParametersPropertyControl),
            new PropertyMetadata(OnTitleChanged));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            nameof(Description), typeof(string), typeof(CustomTextureParametersPropertyControl),
            new PropertyMetadata(OnDescriptionChanged));

        public static readonly DependencyProperty PropertyShapeTypeProperty = DependencyProperty.Register(
            nameof(PropertyShapeType), typeof(ShapeType), typeof(CustomTextureParametersPropertyControl));

        public static readonly DependencyProperty PropertyWidthValueProperty = DependencyProperty.Register(
            nameof(PropertyWidthValue), typeof(int), typeof(CustomTextureParametersPropertyControl));

        public static readonly DependencyProperty PropertyHeightValueProperty = DependencyProperty.Register(
            nameof(PropertyHeightValue), typeof(int), typeof(CustomTextureParametersPropertyControl));

        public static readonly DependencyProperty PropertyColorProperty = DependencyProperty.Register(
            nameof(PropertyColor), typeof(Color), typeof(CustomTextureParametersPropertyControl), 
            new PropertyMetadata(OnPropertyValueChanged));

        public static readonly DependencyProperty PropertyCornerRadiusProperty = DependencyProperty.Register(
            nameof(PropertyCornerRadius), typeof(int), typeof(CustomTextureParametersPropertyControl));

        public static readonly DependencyProperty ToControlSizeButtonTextProperty = DependencyProperty.Register(
            nameof(ToControlSizeButtonText), typeof(string), typeof(CustomTextureParametersPropertyControl));

        public static readonly DependencyProperty ToControlSizeButtonCommandProperty = DependencyProperty.Register(
            nameof(ToControlSizeButtonCommand), typeof(ICommand), typeof(CustomTextureParametersPropertyControl));

        public static readonly DependencyProperty PropertyBorderThicknessProperty = DependencyProperty.Register(
            nameof(PropertyBorderThickness), typeof(int), typeof(CustomTextureParametersPropertyControl));

        public static readonly DependencyProperty PropertyBorderColorProperty = DependencyProperty.Register(
            nameof(PropertyBorderColor), typeof(Color), typeof(CustomTextureParametersPropertyControl),
            new PropertyMetadata(OnPropertyBorderColorChanged));

        private static void OnPropertyBorderColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not CustomTextureParametersPropertyControl view)
            {
                return;
            }

            view.BorderColorPicker.SelectedColor = e.NewValue is Color newColor
                ? newColor
                : new Color();
        }

        private static void OnPropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not CustomTextureParametersPropertyControl view)
            {
                return;
            }

            view.FillColorPicker.SelectedColor = e.NewValue is Color newColor
                ? newColor
                : new Color();
        }

        private static void OnDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not CustomTextureParametersPropertyControl view)
            {
                return;
            }

            view.DescriptionTextBlock.Text = e.NewValue as string ?? string.Empty;
        }

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not CustomTextureParametersPropertyControl view)
            {
                return;
            }

            view.TitleTextBlock.Text = e.NewValue as string ?? string.Empty;
        }
    }
}
