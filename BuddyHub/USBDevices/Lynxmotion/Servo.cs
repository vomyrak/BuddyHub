﻿// Lynxmotion SSC32/AL5x Robotic Arm Library
//
// Copyright © Rémy Dispagne, 2014
// cramer at libertysurf.fr
//
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

namespace Lynxmotion
{
    /// <summary>
    /// Class definition of a servomotor
    /// </summary>
    public class Servo
    {
        /// <summary>
        /// Maximum value for pulse width
        /// </summary>
        public const short MAX_PULSE_WIDTH = 2500;
        /// <summary>
        /// Minimum value for pulse width
        /// </summary>
        public const short MIN_PULSE_WIDTH = 500;

        /// <summary>
        /// Maximum value for servo channel number
        /// </summary>
        public const byte MAX_CHANNEL = 31;
        /// <summary>
        /// Minimum value for servo channel number
        /// </summary>
        public const byte MIN_CHANNEL = 0;

        private short pulse_width = 1500;

        private short movement_speed = -1;

        internal bool pulse_width_changed = true;

        /// <summary>
        /// Initializes a servo class from a channel number and the initial pulse width
        /// </summary>
        /// <param name="channel">Channel number on which is connected the servo</param>
        /// <param name="initial_pulse_width">The initial pulse width</param>
        private void Initialize(byte channel, short initial_pulse_width)
        {
            if (channel < MIN_CHANNEL || channel > MAX_CHANNEL)
            {
                throw new ArgumentException("channel out of range !");
            }
            this.Channel = channel;
            this.pulse_width = initial_pulse_width;
        }

        /// <summary>
        /// Creates a servo class from a channel number
        /// </summary>
        /// <param name="channel">Channel number on which is connected the servo</param>
        public Servo(byte channel)
        {
            Initialize(channel, 0);
        }

        /// <summary>
        /// Creates a servo class from a channel number and the initial pulse width
        /// </summary>
        /// <param name="channel">Channel number on which is connected the servo</param>
        /// <param name="initial_pulse_width">The initial pulse width</param>
        public Servo(byte channel, short initial_pulse_width)
        {
            Initialize(channel, initial_pulse_width);
        }

        /// <summary>
        /// Sets or gets the channel/number of the servo on the driving board
        /// </summary>
        public byte Channel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current value of the servo pulse width in µs
        /// </summary>
        public short PulseWidth
        {
            get
            {
                return pulse_width;
            }
            set
            {
                setPulseWidth(value);
            }
        }

        /// <summary>
        /// Sets the servo pulse width
        /// </summary>
        /// <param name="pulse_width">Pulse width in µs</param>
        public void setPulseWidth(short pulse_width)
        {
            // Clip the pulse width to the correct range
            if (pulse_width < MIN_PULSE_WIDTH && pulse_width != 0)
            {
                pulse_width = MIN_PULSE_WIDTH;
            }
            else
            {
                if (pulse_width > MAX_PULSE_WIDTH)
                {
                    pulse_width = MAX_PULSE_WIDTH;
                }
            }

            // If the value to apply is different from the current one
            if (pulse_width != this.pulse_width)
            {
                // Flag it
                pulse_width_changed = true;

                // Apply the new value
                this.pulse_width = pulse_width;
            }
        }

        /// <summary>
        /// Convert servo class to string by returning the corresponding SSC32 command string
        /// </summary>
        /// <returns>The SSC32 command string</returns>
        public override string ToString()
        {
            string command = "#" + Channel + " P" + pulse_width;

            if (movement_speed >= 0)
            {
                command += " S" + movement_speed;
            }

            return command + " ";
        }
    }
}
