using Oscetch.ScriptComponent;

namespace Editor.Models
{
    public class ScriptValueParameterModel(ScriptReference reference, ScriptValueParameter scriptValueParameter)
    {
        private readonly ScriptReference _reference = reference;
        private ScriptValueParameter _parameter = scriptValueParameter;

        public string Name { get => _parameter.Name; }
        public string Value
        {
            get => _parameter.Value;
            set
            {
                _reference.Params.Remove(_parameter);
                _parameter = new ScriptValueParameter(_parameter.Name, value);
                _reference.Params.Add(_parameter);
            }
        }
    }
}
