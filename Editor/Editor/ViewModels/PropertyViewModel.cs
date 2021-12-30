using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.ViewModels
{
    public abstract class PropertyViewModel<T> : ViewModel
    {
        private string _title;
        private string _description;
        private T _propertyValue;

        public string Title 
        { 
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public T PropertyValue
        {
            get => _propertyValue;
            set
            {
                _propertyValue = value;
                OnChangedAction?.Invoke(value);
                OnPropertyChanged();
            }
        }

        public Action<T> OnChangedAction { get; set; }
    }
}
