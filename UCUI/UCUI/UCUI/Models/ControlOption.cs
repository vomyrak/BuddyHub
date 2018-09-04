using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCUI.Models
{
    class ControlOption
    {
        public bool[] buttonVisible { get; set; } //which buttons are visible
        public bool textBoxVisible { get; set; } //is the Textbox visible?
        public string name { get; set; }
        public string description { get; set; } //Description that goes into the listbox
        public string imageName { get; set; } //filename of the URI below
        public Uri actualUri { get; set; } //Image URI representing option int the listbox
        public string[] buttonLabels { get; set; } //text that goes under button images
        public string[] buttonImages { get; set; } //the filenames of all image files
        public Uri[] buttonUris { get; set; } //The image URI array of all visible buttons
    }
}
