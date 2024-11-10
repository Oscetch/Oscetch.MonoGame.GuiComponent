using Editor.Models;
using Oscetch.ScriptComponent.Interfaces;
using Oscetch.ScriptComponent;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Oscetch.MonoGame.GuiComponent.Interfaces;
using System.Diagnostics;
using System;
using System.Windows.Documents;
using System.Collections.Generic;

namespace Editor.Modals
{
    /// <summary>
    /// Interaction logic for ReferenceScriptTypeDialog.xaml
    /// </summary>
    public partial class ReferenceScriptTypeDialog : Window
    {
        public ObservableCollection<ScriptReferenceCheckedModel> ScriptReferenceCheckedModels { get; set; }

        public ReferenceScriptTypeDialog(string referencePath)
        {
            InitializeComponent();

            ScriptReferenceCheckedModels = [];
            var scriptInterface = typeof(IGuiScript<>);
            if (File.Exists(referencePath))
            {
                var assembly = Assembly.LoadFrom(referencePath);
                try
                {
                    var types = assembly.GetTypes();
                    var assignableTypes = assembly.GetTypes().Where(x => x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == scriptInterface)).ToList();

                    foreach (var type in assignableTypes)
                    {
                        var scriptReference = new ScriptReference(referencePath, type.FullName);
                        ScriptReferenceCheckedModels.Add(new ScriptReferenceCheckedModel(scriptReference, false));
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
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
