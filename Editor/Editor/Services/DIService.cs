using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Services
{
    public class DIService
    {
        private readonly Dictionary<Type, object> _singletons = new();

        public DIService RegisterSingleton(object o)
        {
            _singletons[o.GetType()] = o;
            return this;
        }

        public T Resolve<T>()
        {
            var type = typeof(T);
            return (T)Resolve(type);
        }

        private object Resolve(Type type)
        {
            if (_singletons.TryGetValue(type, out var obj))
            {
                return obj;
            }

            var constructor = type.GetConstructors().FirstOrDefault();
            if (constructor == null)
            {
                return Activator.CreateInstance(type);
            }

            var parameters = constructor.GetParameters();
            var paramObjects = new object[parameters.Length];
            for(var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                paramObjects[i] = Resolve(parameter.ParameterType);
            }

            return constructor.Invoke(paramObjects);
        }
    }
}
