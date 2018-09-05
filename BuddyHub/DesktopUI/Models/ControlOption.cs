using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCUI.Models
{
    class ControlOption
    {
        public bool[] buttonVisible { get; set; }
        public bool textBoxVisible { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string imageName { get; set; }
        public Uri actualUri { get; set; }
        public string[] buttonLabels { get; set; }
        public string[] buttonImages { get; set; }
        public Uri[] buttonUris { get; set; }
    }
}
