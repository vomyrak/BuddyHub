// WSUROP 2018 Universal Controller Source Code
//
// Source code for various test functions

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace TheHook
{
    class Test
    {
        static System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        static bool exitFlag = false;
        static int interruptCounter = 0;
        static Random rand = new Random();
        private static int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
        private static int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;
        static mouseGen generator = new mouseGen();

        private static void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            myTimer.Stop();
            interruptCounter += 1;
            if (interruptCounter < 10)
            {
                int randX = rand.Next(0, screenWidth - 1);
                int randY = rand.Next(0, screenHeight - 1);
                Console.Write("Written Coordinates (" + randX + ", " + randY + ")\n");
                generator.DoMouseClick(Convert.ToUInt32(randX), Convert.ToUInt32(randY));
                myTimer.Enabled = true;
            }
            else exitFlag = true;
        }

        public void mouseTest()
        {
            myTimer.Tick += new EventHandler(TimerEventProcessor);
            myTimer.Interval = 5000;
            myTimer.Start();
            while(exitFlag == false)
            {
                Application.DoEvents();
            }
        }
    }
}
