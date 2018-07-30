<<<<<<< HEAD
<<<<<<< HEAD
﻿using System;
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
                number = 0,
                name = "Macro"
            });
            _options.Add(new ControlOption
            {
                number = 1,
                name = "Robotic arm"
            });
            _options.Add(new ControlOption
            {
                number = 3,
                name = "Text-to-Speech"
            });
            _options.Add(new ControlOption
            {
                number = 6,
                name = "Cooking Pan"
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
=======
=======
>>>>>>> prototype-vk
﻿using System;
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
                number = 0,
                name = "Macro"
            });
            _options.Add(new ControlOption
            {
                number = 1,
                name = "Robotic arm"
            });
            _options.Add(new ControlOption
            {
                number = 3,
                name = "Text-to-Speech"
            });
            _options.Add(new ControlOption
            {
                number = 6,
                name = "Cooking Pan"
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
<<<<<<< HEAD
>>>>>>> 706da80e0f05b1e7c3e37c9c3d22963d4460622a
=======
>>>>>>> prototype-vk
