using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCUI.Models
{
    class ControlSource
    {
        private static List<ControlOption> _options;


        static ControlSource()
        {
            _options = new List<ControlOption>();
            _options.Add(new ControlOption
            {
                buttonVisible = new bool[] { true, true, true, true, true, true, true, true, true },
                number = 0,
                textBoxVisible = false,
                name = "Macro",
                description="Record and replay mouse and keyboard inputs"
            });
            _options.Add(new ControlOption
            {
                buttonVisible = new bool[] { false, true, false, true, true, true, false, true, false },
                number = 1,
                textBoxVisible = false,
                name = "Robotic arm",
                description="Control the movement of a Lynxmotion AL5D robotic arm"
            });
            _options.Add(new ControlOption
            {
                buttonVisible = new bool[] { false, false, false, false, true, true, false, false, false },
                number = 3,
                textBoxVisible = true,
                name = "Text-to-Speech",
                description = "Use text to speech software"
            });
            _options.Add(new ControlOption
            {
                buttonVisible = new bool[] { true, true, true, true, false, true, true, false, true },
                number = 6,
                textBoxVisible = false,
                name = "Cooking Pan",
                description="Interact with a recipe software"
            });
            _options.Add(new ControlOption
            {
                buttonVisible = new bool[] { false, false, false, false, false, false, false, false, false },
                number = 6,
                textBoxVisible = false,
                name = "None"
            });


        }

        public static List<ControlOption> Options
        {
            get
            {
                return _options;
            }
        }
    }


}
