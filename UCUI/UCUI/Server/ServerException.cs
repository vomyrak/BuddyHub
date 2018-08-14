using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppServer
{
    [Serializable]
    class InvalidDeviceException : Exception
    {
        public InvalidDeviceException() { }

        public InvalidDeviceException(string message)
            : base(message) { }

        public InvalidDeviceException(string message, int errorCode) { }

        public InvalidDeviceException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    [Serializable]
    class InvalidActionException : Exception
    {
        public InvalidActionException() { }

        public InvalidActionException(string message)
            : base(message) { }

        public InvalidActionException(string message, int errorCode) { }

        public InvalidActionException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    [Serializable]
    class InvalidMethodException : Exception
    {
        public InvalidMethodException() { }

        public InvalidMethodException(string message)
            : base(message) { }

        public InvalidMethodException(string message, int errorCode) { }

        public InvalidMethodException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}