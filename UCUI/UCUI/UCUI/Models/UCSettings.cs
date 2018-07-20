using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UCUI.Models 
{
    class UCSettings : INotifyPropertyChanged
    {
        private bool isShake;
        public bool IsShake
        {
            get { return isShake;}
            set
            {
                if(value!=isShake)
                {
                    isShake = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
