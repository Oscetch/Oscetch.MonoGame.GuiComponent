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
using System.Windows.Shapes;

namespace Editor.Modals
{
    /// <summary>
    /// Interaction logic for StringDialog.xaml
    /// </summary>
    public partial class StringDialog : Window
    {
        public string Result => dialogTextBox.Text;

        public StringDialog(string windowTitle, string descriptionText, string defaultValue = "")
        {
            InitializeComponent();
            Title = windowTitle;
            descriptionTextBlock.Text = descriptionText;
            dialogTextBox.Text = defaultValue;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
