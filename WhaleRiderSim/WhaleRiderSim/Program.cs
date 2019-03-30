using System;
#if SURFACE
using Microsoft.Surface.Core;
#endif
using Microsoft.Xna.Framework;

namespace WhaleRiderSim
{
#if WINDOWS || XBOX || SURFACE
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (WhaleRidingSim game = new WhaleRidingSim())
            {
                game.Run();

            }
        }

#if SURFACE
        // Hold on to the game window.
        static GameWindow Window;
        
        /// <summary>
        /// Sets the window style for the specified HWND to None.
        /// </summary>
        /// <param name="hWnd">the handle of the window</param>
        internal static void RemoveBorder(IntPtr hWnd)
        {
            Form form = (Form)Form.FromHandle(hWnd);
            form.FormBorderStyle = FormBorderStyle.None;
        }

        /// <summary>
        /// Registers event handlers and sets the initial position of the game window.
        /// </summary>
        /// <param name="window">the game window</param>
        internal static void PositionWindow(GameWindow window)
        {
            if (window == null)
                throw new ArgumentNullException("window");

            if (Window != null)
            {
                Window.ClientSizeChanged -= new EventHandler(OnSetWindowPosition);
                Window.ScreenDeviceNameChanged -= new EventHandler(OnSetWindowPosition);
            }

            Window = window;

            Window.ClientSizeChanged += new EventHandler(OnSetWindowPosition);
            Window.ScreenDeviceNameChanged += new EventHandler(OnSetWindowPosition);

            UpdateWindowPosition();
        }

        /// <summary>
        /// When the ScreenDeviceChanges or the ClientSizeChanges update the Windows Position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnSetWindowPosition(object sender, EventArgs e)
        {
            UpdateWindowPosition();
        }

        /// <summary>
        /// Use the Desktop bounds to update the the position of the Window correctly.
        /// </summary>
        private static void UpdateWindowPosition()
        {
            IntPtr hWnd = Window.Handle;
            Form form = (Form)Form.FromHandle(hWnd);
            form.SetDesktopLocation(InteractiveSurface.DefaultInteractiveSurface.Left - (Window.ClientBounds.Left - form.DesktopBounds.Left),
                                    InteractiveSurface.DefaultInteractiveSurface.Top - (Window.ClientBounds.Top - form.DesktopBounds.Top));
        }
#endif
    }
#endif
}

