﻿using Editor.Extensions;
using Editor.Models;
using ICSharpCode.AvalonEdit.CodeCompletion;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Oscetch.ScriptComponent;
using Oscetch.ScriptComponent.Compiler;
using Oscetch.ScriptComponent.Compiler.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Editor.Modals
{
    /// <summary>
    /// Interaction logic for ModalScriptWindow.xaml
    /// </summary>
    public partial class ModalScriptWindow : Window
    {
        private readonly ProjectSettings _settings;
        private Document _document;
        private CompletionWindow _completionWindow;

        public ModalScriptWindow(string templatePath)
        {
            InitializeComponent();
            _settings = ProjectSettings.GetSettings();
            if (!Directory.Exists(_settings.ScriptsDir))
            {
                Directory.CreateDirectory(_settings.ScriptsDir);
            }

            if (File.Exists(templatePath))
            {
                var text = File.ReadAllText(templatePath);
                codeControl.Text = text;
            }

            _document = OscetchCompiler.CreateDocument("TestAssembly", [GetType().Assembly]);

            codeControl.TextArea.TextEntering += TextArea_TextEntering;
            codeControl.TextArea.TextEntered += TextArea_TextEntered;
        }

        private async void TextArea_TextEntered(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var sourceText = SourceText.From(codeControl.Text);
            _document = _document.WithText(sourceText);
            var completionService = CompletionService.GetService(_document);
            if(completionService == null)
            {
                return;
            }

            Microsoft.CodeAnalysis.Completion.CompletionList result = null;
            try
            {
                if (!completionService.ShouldTriggerCompletion(sourceText, codeControl.CaretOffset,
                    CompletionTrigger.CreateInsertionTrigger(e.Text[0])))
                {
                    return;
                }
                result = await completionService.GetCompletionsAsync(_document, codeControl.CaretOffset);
            }
            catch { }
            if(result.ItemsList == null)
            {
                return;
            }

            _completionWindow = new CompletionWindow(codeControl.TextArea);
            var completionData = _completionWindow.CompletionList.CompletionData;
            foreach(var item in result.ItemsList)
            {
                completionData.Add(new CompletionData(item));
            }
            _completionWindow.Show();
            _completionWindow.Closed += (sender, args) =>
            {
                _completionWindow = null;
            };
        }

        private void TextArea_TextEntering(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && _completionWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]))
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    _completionWindow.CompletionList.RequestInsertion(e);
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            IList<SyntaxTree> syntaxTrees;
            try
            {
                syntaxTrees = GetScriptSyntaxTrees(out var skippedScripts);
                foreach(var skippedScript in skippedScripts)
                {
                    File.Delete(skippedScript);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            var stringDialog = new StringDialog("Set script name", "Enter script name:", 
                syntaxTrees.FirstOrDefault()?.GetClassNames()?.FirstOrDefault() ?? string.Empty);
            var dialogResult = stringDialog.ShowDialog() ?? false;
            if (!dialogResult)
            {
                return;
            }

            var newScriptPath = Path.Join(_settings.ScriptsDir, $"{stringDialog.Result}.cs");
            File.WriteAllText(newScriptPath, codeControl.Text);

            var referencePath = ProjectSettings.GetSettings().GameDllPath;
            var initialAssembly = Assembly.LoadFrom(referencePath);
            var assemblyList = initialAssembly.GetReferencedAssembliesAtPath(referencePath);
            var referencedAssemblies = assemblyList.ToMetadata();
            var result = OscetchCompiler.Compile(_settings.AssemblyName, syntaxTrees, referencedAssemblies, 
                out var dllPath, out _);
            if (!result)
            {
                MessageBox.Show("Error compiling");
                return;
            }

            File.Copy(dllPath, _settings.OutputPath, true);
            File.Delete(dllPath);
        }

        private List<SyntaxTree> GetScriptSyntaxTrees(out IList<string> skippedScripts)
        {
            var newClassNames = new List<string>();
            var syntaxTreeList = new List<SyntaxTree>();
            skippedScripts = [];

            var scintillaSyntaxTree = CSharpSyntaxTree.ParseText(codeControl.Text);
            syntaxTreeList.Add(scintillaSyntaxTree);

            newClassNames = [.. scintillaSyntaxTree.GetClassNames()];

            foreach(var scriptFile in Directory.GetFiles(_settings.ScriptsDir))
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(scriptFile));
                if (syntaxTree.GetClassNames().Any(x => newClassNames.Contains(x)))
                {
                    var result = MessageBox.Show($"The script {scriptFile} contains class names identical to the script you're trying to save.\n" +
                        $"Do whish to replace {scriptFile} with the new script to avoid errors?", "Duplicate file found", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        skippedScripts.Add(Path.GetFileName(scriptFile));
                        continue;
                    }
                }
                syntaxTreeList.Add(syntaxTree);
            }

            return syntaxTreeList;
        }
    }
}
