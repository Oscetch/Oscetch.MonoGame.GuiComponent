using System.Windows;

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
