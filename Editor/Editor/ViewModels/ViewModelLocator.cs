using Editor.Services;

namespace Editor.ViewModels
{
    public class ViewModelLocator
    {
        public static MainWindowViewModel MainWindowViewModel => NinjectKernel.Get<MainWindowViewModel>();
        public static EditorViewModel EditorViewModel => NinjectKernel.Get<EditorViewModel>();
        public static TopLeftViewModel TopLeftViewModel => NinjectKernel.Get<TopLeftViewModel>();
        public static LeftPanelViewModel LeftPanelViewModel => NinjectKernel.Get<LeftPanelViewModel>();
        public static ControlPropertiesViewModel ControlPropertiesViewModel => NinjectKernel.Get<ControlPropertiesViewModel>();
    }
}
