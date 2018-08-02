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
            string[] filenames = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\ControlOptions");

            for (int i = 0; i < filenames.Length; i++)
            {
                string[] lines = System.IO.File.ReadAllLines(filenames[i]);
                string[] boolWords = lines[0].Split(' ');
                bool[] _buttonVisible = new bool[9];
                string[] _buttonLabels = lines[5].Split(' ');
                for (int j = 0; j < 9; j++)
                {
                    _buttonVisible[j] = boolWords[j] == "true";
                }
                _options.Add(new ControlOption
                {
                    buttonVisible = _buttonVisible,
                    textBoxVisible = lines[1] == "true",
                    name = lines[2],
                    description = lines[3],
                    imageName = lines[4],
                    buttonLabels = _buttonLabels

                });

            }

            
            foreach (ControlOption curOption in _options)
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
