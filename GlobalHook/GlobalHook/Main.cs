// WSUROP 2018 Universal Controller Source Code
//
// Programme Entry

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalHook;
using System.Windows.Forms;

namespace TheHook
{
    public class Program
    {
        static void Main(string[] arg)
        {
            Application.Run(new myHook());
        }
    }
}
