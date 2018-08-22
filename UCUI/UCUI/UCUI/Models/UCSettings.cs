<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UCUI.Models 
{
    class UCSettings : INotifyPropertyChanged
    {

        //KeyBinds stored in string instead of Key so it can be more easily read from text file (No backwards conversion needed). Alternatively it could be read binary mode.
        static private string[] keyBinds = new string[10];    
        static public void SetKey(string keyIn, int i)
        {
            if (keyIn != keyBinds[i])
            {
                keyBinds[i] = keyIn;
            }
        }
        static public string GetKey(int i)
        {
            if (keyBinds[i] != null)
                return keyBinds[i];
            else return "null";
        }

        private  bool isShake; //Increased visual feedback animation for shaker buttons
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

        private bool isCenter; //Mouse centering
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

        private bool isHover; //CLicking on hover
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

        private bool isSound; //Audio Feedback
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

        private bool isOpen; //If any view (i.e. SettingsView or HelpView) is visible, this is true
        public bool IsOpen
        {
            get { return isOpen; }
            set
            {
                if (value != isOpen)
                {
                    isOpen = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isFull; //Fullscreen
        public bool IsFull
        {
            get { return isFull; }
            set
            {
                if (value != isFull)
                {
                    isFull = value;
                    OnPropertyChanged();
                    if (isFull)
                    {
                        App.Current.MainWindow.WindowState = WindowState.Normal;
                        App.Current.MainWindow.WindowStyle = WindowStyle.ToolWindow;
                        App.Current.MainWindow.ResizeMode = ResizeMode.NoResize;
                        App.Current.MainWindow.WindowState = WindowState.Maximized;
                        
                    }
                    else
                    {
                        App.Current.MainWindow.WindowState = WindowState.Normal;
                        App.Current.MainWindow.ResizeMode = ResizeMode.CanResize;
                        App.Current.MainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                    }
                }
            }
        }

        private string message; //Displayed on TitleBlock in MainWindow
        public string Message
        {
            get { return message; }
            set
            {
                if (value != message)
                {
                    message = value;
                    OnPropertyChanged();
                }
            }
        }

        private string buttonKey; //ToString() of the current key down in the main window. Compared with e.Key is compared with keybinds[i] to determine its value. Fires the pressdown animation.
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

        #region AutoTabbing
        public static int AutoTabTime;
        public static bool IsAuto;

        static public void AutoTab()
        {

            while (IsAuto)
            {
                Thread.Sleep(AutoTabTime);
                Application.Current.Dispatcher.Invoke((Action)delegate {
                    UCMethods.NextTab();
                });
                
            }
            if(Thread.CurrentThread.IsBackground)Thread.CurrentThread.Abort();

        }
        #endregion

    }
}
=======
﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UCUI.Models 
{
    class UCSettings : INotifyPropertyChanged
    {
        static private string[] keyBinds = new string[9];

       
        static public void SetKey(string keyIn, int i)
        {
            if (keyIn != keyBinds[i])
            {
                keyBinds[i] = keyIn;
            }
        }

        static public string GetKey(int i)
        {
            return keyBinds[i];
        }

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
>>>>>>> rachel
