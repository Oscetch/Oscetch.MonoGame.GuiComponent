using Oscetch.ScriptComponent;

namespace Editor.Models
{
    public class ScriptReferenceCheckedModel(ScriptReference scriptReference, bool isSelected)
    {
        public ScriptReference ScriptReference { get; } = scriptReference;
        public string Name => ScriptReference.ToString();
        public bool IsSelected { get; set; } = isSelected;
    }
}
