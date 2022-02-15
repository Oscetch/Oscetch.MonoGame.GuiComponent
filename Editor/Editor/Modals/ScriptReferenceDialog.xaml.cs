using Editor.Models;
using Oscetch.ScriptComponent;
using Oscetch.ScriptComponent.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for ListDialog.xaml
    /// </summary>
    public partial class ScriptReferenceDialog : Window
    {
        private readonly Settings _settings;

        public ObservableCollection<ScriptReferenceCheckedModel> ScriptReferenceCheckedModels { get; set; }

        public ScriptReferenceDialog(List<ScriptReference> currentReferences)
        {
            _settings = Settings.GetSettings();
            InitializeComponent();

            Title = "Script References";

            ScriptReferenceCheckedModels = new ObservableCollection<ScriptReferenceCheckedModel>();
            foreach(var currentReference in currentReferences)
            {
                ScriptReferenceCheckedModels.Add(new ScriptReferenceCheckedModel(currentReference, true));
            }

            if (File.Exists(_settings.OutputPath))
            {
                var assembly = Assembly.Load(File.ReadAllBytes(_settings.OutputPath));
                var scriptInterface = typeof(IScript);
                var types = assembly.GetTypes()
                    .Where(x => scriptInterface.IsAssignableFrom(x));

                foreach (var type in types) 
                {
                    var scriptReference = new ScriptReference(_settings.OutputPath, type.FullName);
                    if(currentReferences.Any(x => x == scriptReference))
                    {
                        continue;
                    }
                    ScriptReferenceCheckedModels.Add(new ScriptReferenceCheckedModel(scriptReference, false));
                }
            }

            DataContext = this;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
