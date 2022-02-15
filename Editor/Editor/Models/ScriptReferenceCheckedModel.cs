using Oscetch.ScriptComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Models
{
    public class ScriptReferenceCheckedModel
    {
        public ScriptReference ScriptReference { get; }
        public string Name => ScriptReference.ToString();
        public bool IsSelected { get; set; }

        public ScriptReferenceCheckedModel(ScriptReference scriptReference, bool isSelected)
        {
            ScriptReference = scriptReference;
            IsSelected = isSelected;
        }
    }
}
