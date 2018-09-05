using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using static UCUI.Models.InputInfo;

namespace UCUI.Models
{
    class UCMethods
    {

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("User32.dll", SetLastError = true)]
        private static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);

        static public double GetWindowLeft(Window window)
        {
            if (window.WindowState == WindowState.Maximized)
            {
                var leftField = typeof(Window).GetField("_actualLeft", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                return (double)leftField.GetValue(window);
            }
            else
                return window.Left;
        }

        static public double GetWindowTop(Window window)
        {
            if (window.WindowState == WindowState.Maximized)
            {
                var leftField = typeof(Window).GetField("_actualTop", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                return (double)leftField.GetValue(window);
            }
            else
                return window.Top;

        }

        static public void PlayMySound()
        {
            using (SoundPlayer player = new SoundPlayer(AppDomain.CurrentDomain.BaseDirectory + "\\sounds\\AudioFeedback.wav"))
            {
                player.Play();
            }


        }

        static public void NextTab()
        {
            TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);

            // Gets the element with keyboard focus.
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            // Change keyboard focus.
            if (elementWithFocus != null) elementWithFocus.MoveFocus(request);
            
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

        public static void SendKeyPress(KeyCode keyCode) //stolen from https://stackoverflow.com/questions/12761169/send-keys-through-sendinput-in-user32-dll
        {
            INPUT input = new INPUT
            {
                Type = 1
            };
            input.Data.Keyboard = new KEYBDINPUT()
            {
                Vk = (ushort)keyCode,
                Scan = 0,
                Flags = 0,
                Time = 0,
                ExtraInfo = IntPtr.Zero,
            };

            INPUT input2 = new INPUT
            {
                Type = 1
            };
            input2.Data.Keyboard = new KEYBDINPUT()
            {
                Vk = (ushort)keyCode,
                Scan = 0,
                Flags = 2,
                Time = 0,
                ExtraInfo = IntPtr.Zero
            };
            INPUT[] inputs = new INPUT[] { input, input2 };
            if (SendInput(2, inputs, Marshal.SizeOf(typeof(INPUT))) == 0)
                throw new Exception();
        }

    }
}
