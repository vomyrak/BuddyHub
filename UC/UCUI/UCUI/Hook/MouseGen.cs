<<<<<<< HEAD
<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace UCUI.Hook
{
    class MouseGen
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        private Cursor cursor;
        private Point location;
        private Size size;

        public void DoMouseClick(uint x, uint y)
        {
            Cursor.Position = new System.Drawing.Point(Convert.ToInt32(x), Convert.ToInt32(y));
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x, y, 0, 0);
        }

        private void MoveCursor(int xIn, int yIn)
        {
            // Set the Current cursor, move the cursor's Position,
            // and set its clipping rectangle to the form. 

            this.cursor = new Cursor(Cursor.Current.Handle);
            Cursor.Position = new Point(xIn, yIn);
        }
    }
=======
=======
>>>>>>> prototype-vk
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace UCUI.Hook
{
    class MouseGen
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        private Cursor cursor;
        private Point location;
        private Size size;

        public void DoMouseClick(uint x, uint y)
        {
            Cursor.Position = new System.Drawing.Point(Convert.ToInt32(x), Convert.ToInt32(y));
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x, y, 0, 0);
        }

        private void MoveCursor(int xIn, int yIn)
        {
            // Set the Current cursor, move the cursor's Position,
            // and set its clipping rectangle to the form. 

            this.cursor = new Cursor(Cursor.Current.Handle);
            Cursor.Position = new Point(xIn, yIn);
        }
    }
<<<<<<< HEAD
>>>>>>> 706da80e0f05b1e7c3e37c9c3d22963d4460622a
=======
>>>>>>> prototype-vk
}