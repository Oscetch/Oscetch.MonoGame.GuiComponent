using Ninject.Modules;

namespace Editor.ViewModels
{
    public class ViewModelConfiguration : NinjectModule
    {
        public override void Load()
        {
            Bind<MainWindowViewModel>().ToSelf().InSingletonScope();
            Bind<EditorViewModel>().ToSelf().InSingletonScope();
            Bind<TopLeftViewModel>().ToSelf().InSingletonScope();
            Bind<ControlPropertiesViewModel>().ToSelf().InSingletonScope();
            Bind<EditTextViewModel>().ToSelf().InSingletonScope();
            Bind<ControlTreeViewModel>().ToSelf().InSingletonScope();
        }
    }
}
