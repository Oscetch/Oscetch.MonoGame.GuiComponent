using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using Microsoft.CodeAnalysis.Completion;
using System;
using System.Windows.Media;

namespace Editor.Models
{
    public class CompletionData(CompletionItem item) : ICompletionData
    {
        private readonly CompletionItem _item = item;

        public ImageSource Image => null;

        public string Text { get; } = $"{item.DisplayTextPrefix ?? string.Empty}{item.DisplayText}{item.DisplayTextSuffix ?? string.Empty}";

        public object Content => Text;

        public object Description { get; } = item.InlineDescription ?? string.Empty;

        public double Priority { get; }

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Remove(completionSegment.Offset - 1, completionSegment.Length + 1);
            textArea.Document.Insert(_item.Span.Start, Text);
        }
    }
}
