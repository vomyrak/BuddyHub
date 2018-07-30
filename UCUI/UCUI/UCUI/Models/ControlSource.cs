using System;
using System.Collections.Generic;
using System.IO;
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
                textBoxVisible = false,
                name = "Macro",
                description="Record and replay mouse and keyboard inputs",
                imageName="\\images\\macro.png"

            });
            _options.Add(new ControlOption
            {
                buttonVisible = new bool[] { false, true, false, true, false, true, false, true, false },
                textBoxVisible = false,
                name = "Robotic arm",
                description="Control the movement of a Lynxmotion AL5D robotic arm",
                imageName= "\\images\\arm.png"

            });
            _options.Add(new ControlOption
            {
                buttonVisible = new bool[] { false, false, false, false, true, true, false, false, false },
                textBoxVisible = true,
                name = "Text-to-Speech",
                description = "Use text to speech software",
                imageName= "\\images\\tts.png"
            });
            _options.Add(new ControlOption
            {
                buttonVisible = new bool[] { true, true, true, true, false, true, true, false, true },
                textBoxVisible = false,
                name = "Cooking Pan",
                description="Interact with a recipe software",
                imageName= "\\images\\pan.png"
            });


            foreach(ControlOption curOption in _options)
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + curOption.imageName))
                {
                    curOption.actualUri = new Uri(AppDomain.CurrentDomain.BaseDirectory + curOption.imageName, UriKind.RelativeOrAbsolute);
                }
            }


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
