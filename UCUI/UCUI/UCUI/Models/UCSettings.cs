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
        private  bool isShake;
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

        private bool isCenter;
        public bool IsCenter
        {
            get { return isCenter; }
            set
            {
                if(value!=isCenter)
                {
                    isCenter = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isHover;
        public bool IsHover
        {
            get { return isHover; }
            set
            {
                if(value!=isHover)
                {
                    isHover = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isSound;
        public bool IsSound
        {
            get { return isSound; }
            set
            {
                if(value!=isSound)
                {
                    isSound = value;
                    OnPropertyChanged();
                }
            }
        }

        private string buttonKey;
        public string ButtonKey
        {
            get { return buttonKey; }
            set
            {
                if (value != buttonKey)
                {
                    buttonKey = value;
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
