//  BuddyHub Universal Controller
//
//  Created by Husheng Deng, 2018
//  https://github.com/vomyrak/BuddyHub

//  This library is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.

//  This library is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.

//  You should have received a copy of the GNU General Public License
//  along with this library.  If not, see <http://www.gnu.org/licenses/>.
//
//  All trademarks, service marks, trade names, product names are the property of their respective owners.

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