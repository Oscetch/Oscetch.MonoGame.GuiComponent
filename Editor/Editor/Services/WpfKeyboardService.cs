using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Editor.Services
{
    public class WpfKeyboardService
    {
        private static readonly Key[] _allKeys = Enum.GetValues(typeof(Key))
            .OfType<Key>()
            .Where(x => x != Key.None)
            .ToArray();

        private readonly List<Key> _previousKeys = new List<Key>();
        private readonly List<Key> _currentKeys = new List<Key>();

        public void Update()
        {
            _previousKeys.Clear();
            _previousKeys.AddRange(_currentKeys);
            _currentKeys.Clear();
            for(var i = 0; i < _allKeys.Length; i++)
            {
                var key = _allKeys[i];
                if (Keyboard.IsKeyDown(key))
                {
                    _currentKeys.Add(key);
                }
            }
        }

        public bool IsKeyDown(Key key)
        {
            return _currentKeys.Contains(key);
        }

        public bool IsKeyClicked(Key key)
        {
            return _currentKeys.Contains(key) && !_previousKeys.Contains(key);
        }
    }
}
