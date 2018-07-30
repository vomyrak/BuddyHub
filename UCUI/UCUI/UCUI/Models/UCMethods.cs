using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace UCUI.Models
{
    class UCMethods
    {
        
        static public double GetWindowLeft(Window window)
        {
            if (window.WindowState == WindowState.Maximized)
            {
                var leftField = typeof(Window).GetField("_actualLeft", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                return (double)leftField.GetValue(window)+7.2;
            }
            else
                return window.Left;
        }

        static public double GetWindowTop(Window window)
        {
            if (window.WindowState == WindowState.Maximized)
            {
                var leftField = typeof(Window).GetField("_actualTop", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                return (double)leftField.GetValue(window)+7.2;
            }
            else
                return window.Top;

        }

        static public void PlayMySound()
        {
            using (SoundPlayer player = new SoundPlayer(AppDomain.CurrentDomain.BaseDirectory + "\\sounds\\AudioFeedback.wav"))
            {
                // Use PlaySync to load and then play the sound.
                // ... The program will pause until the sound is complete.
                player.Play();
            }


        }


        static public void SetPosition(Window win)
        {
            Matrix m = PresentationSource.FromVisual(System.Windows.Application.Current.MainWindow).CompositionTarget.TransformToDevice;
            double dx = m.M11;
            double dy = m.M22;
            var left = Convert.ToInt32((GetWindowLeft(win) + win.ActualWidth / 2) * dx);
            var top = Convert.ToInt32((GetWindowTop(win) + win.ActualHeight / 2) * dy);
            SetCursorPos(left, top);
        }

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

    }
}
