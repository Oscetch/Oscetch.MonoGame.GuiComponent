using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Editor.Models
{
    public class CompletionData : ICompletionData
    {
        private readonly CompletionItem _item;

        public ImageSource Image => null;

        public string Text { get; }

        public object Content => Text;

        public object Description { get; }

        public double Priority { get; }

        public CompletionData(CompletionItem item)
        {
            Text = $"{item.DisplayTextPrefix ?? string.Empty}{item.DisplayText}{item.DisplayTextSuffix ?? string.Empty}";
            Description = item.InlineDescription ?? string.Empty;
            _item = item;
        }

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Remove(completionSegment.Offset - 1, completionSegment.Length + 1);
            textArea.Document.Insert(_item.Span.Start, Text);
        }
    }
}
