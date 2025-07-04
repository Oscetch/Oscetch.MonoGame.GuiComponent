using Ninject;
using Ninject.Modules;

namespace Editor.Services
{
    public class NinjectKernel
    {
        private static StandardKernel _kernel;

        public static T Get<T>() => _kernel.Get<T>();

        public static void Init(params INinjectModule[] modules)
        {
            _kernel ??= new StandardKernel(modules);
        }
    }
}
