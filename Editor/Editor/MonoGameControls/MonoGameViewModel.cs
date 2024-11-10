using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Editor.MonoGameControls
{
    public interface IMonoGameViewModel : IDisposable
    {
        IGraphicsDeviceService GraphicsDeviceService { get; set; }

        void Initialize();
        void LoadContent();
        void UnloadContent();
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
        void OnActivated(object sender, EventArgs args);
        void OnDeactivated(object sender, EventArgs args);
        void OnExiting(object sender, EventArgs args); 
        void OnMouseUp(IInputElement reference, MouseButtonEventArgs e);
        void OnMouseWheel(IInputElement reference, MouseWheelEventArgs e);
        void OnMouseDown(IInputElement reference, MouseButtonEventArgs e);
        void OnMouseMove(IInputElement reference, MouseEventArgs e);

        void SizeChanged(object sender, SizeChangedEventArgs args);
    }

    public class MonoGameViewModel : ViewModel, IMonoGameViewModel
    {
        public MonoGameViewModel()
        {
        }

        public void Dispose()
        {
            Content?.Dispose();
            GC.SuppressFinalize(this);
        }

        public IGraphicsDeviceService GraphicsDeviceService { get; set; }
        protected GraphicsDevice GraphicsDevice => GraphicsDeviceService?.GraphicsDevice;
        protected MonoGameServiceProvider Services { get; private set; }
        protected ContentManager Content { get; set; }

        public virtual void Initialize()
        {
            Services = new MonoGameServiceProvider();
            Services.AddService(GraphicsDeviceService);
            Content = new ContentManager(Services) { RootDirectory = Settings.GetSettings().ContentPath };
        }

        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }
        public virtual void OnActivated(object sender, EventArgs args) { }
        public virtual void OnDeactivated(object sender, EventArgs args) { }
        public virtual void OnExiting(object sender, EventArgs args) { }
        public virtual void SizeChanged(object sender, SizeChangedEventArgs args) { }

        public virtual void OnMouseUp(IInputElement reference, MouseButtonEventArgs e)
        {
        }

        public virtual void OnMouseWheel(IInputElement reference, MouseWheelEventArgs e)
        {
        }

        public virtual void OnMouseDown(IInputElement reference, MouseButtonEventArgs e)
        {
        }

        public virtual void OnMouseMove(IInputElement reference, MouseEventArgs e)
        {
        }
    }
}
