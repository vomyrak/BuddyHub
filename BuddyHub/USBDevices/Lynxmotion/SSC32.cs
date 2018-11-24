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
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System.Text.RegularExpressions;

namespace Lynxmotion
{
    /// <summary>
    /// Class which represents a result of a SSC32 connected enumeration
    /// </summary>
    public class SSC32ENumerationResult
    {

        /// <summary>
        /// Port name to use to connect to the SSC32
        /// </summary>
        public string PortName
        {
            get;
            private set;
        }

        /// <summary>
        /// Baudrate to use to connect to the SSC32
        /// </summary>
        public int BaudRate
        {
            get;
            private set;
        }

        /// <summary>
        /// Version read from the SSC32 during the enumeration
        /// </summary>
        public string Version
        {
            get;
            private set;
        }

        /// <summary>
        /// Enumeration result constructor
        /// </summary>
        /// <param name="portName">Port name found</param>
        /// <param name="baudRate">Baudrate used</param>
        /// <param name="version">Version of the SSC32 found</param>
        public SSC32ENumerationResult(string portName, int baudRate, string version)
        {
            this.PortName = portName;
            this.BaudRate = baudRate;
            this.Version = version;
        }
    }

    /// <summary>
    /// Class which represents a SSC32 servo driver board
    /// For manuals and more information go to <see cref="http://www.lynxmotion.com/images/html/build136.htm"/>
    /// </summary>
    public class SSC32:IDisposable
    {
        public const int STARTUP_STRING_MAX_LENGTH = 100;

        /// <summary>
        /// Inputs and Outputs of a SS32 board
        /// </summary>
        public enum SSC32Inputs
        {
            IO_A = 0,
            IO_B,
            IO_C,
            IO_D,
        }

        /// <summary>
        /// Function to enumerate SSC32 currently connected to the computer
        /// </summary>
        /// <param name="baudRate">Baudrate to connect with</param>
        /// <returns>List of found SSC32</returns>
        public static SSC32ENumerationResult[] EnumerateConnectedSSC32(int baudRate)
        {
            List<SSC32ENumerationResult> ret = new List<SSC32ENumerationResult>();
            SSC32 current_ssc32 = new SSC32(1);

            Console.WriteLine("Enumerating connected SSC32...");

            // Try every single COM port on the machine
            foreach (string port_name in SerialPort.GetPortNames())
            {
                try
                {
                    Console.WriteLine("\tTrying to connect to " + port_name + " at " + baudRate + "bps");

                    // Try to connect to a ssc32 board on that port
                    current_ssc32.Connect(port_name, baudRate);

                    Console.WriteLine("\t\tConnected !");

                    string version = current_ssc32.getSoftwareVersion();

                    ret.Add(new SSC32ENumerationResult(port_name, baudRate, version));
                }
                catch (Exception exc)
                {
                    Console.WriteLine("\t\tException occured : " + exc.Message);
                }

                try
                {
                    current_ssc32.Disconnect();
                }
                catch (Exception exc)
                {

                }

            }
            try
            {
                current_ssc32.Dispose();
            }
            catch (Exception exc)
            {

            }

            return ret.ToArray();
        }

        private SerialPort s_port;

        protected Servo[] servos;

        #region Initialization and constructors
        /// <summary>
        /// Initializes a SSC32 object
        /// </summary>
        /// <param name="servo_channels">List of the servo channels to use</param>
        public SSC32(params int[] servo_channels)
        {
            Initialize(servo_channels);
        }

        /// <summary>
        /// Initializes a SSC32 object
        /// </summary>
        /// <param name="servo_channels">List of the servo channels to use</param>
        private void Initialize(params int[] servo_channels)
        {
            if (servo_channels.Length < Servo.MIN_CHANNEL || servo_channels.Length > Servo.MAX_CHANNEL)
            {
                throw new ArgumentException("Argument count out of range !");
            }

            // Create as many servos as necessary
            servos = new Servo[servo_channels.Length];
            for (int i = 0; i < servo_channels.Length; i++)
            {
                servos[i] = new Servo((byte)servo_channels[i]);
            }

            // Initialize the flags that indicate if the dummy analog read has been done
            firstAnalogReadDone = new bool[Enum.GetNames(typeof(SSC32Inputs)).Length];
            for (int i = 0; i < firstAnalogReadDone.Length; i++)
            {
                firstAnalogReadDone[i] = false;
            }

            MotionTime = 100;
        }

        /// <summary>
        /// Instantiates a new SSC32 board
        /// </summary>
        /// <param name="portName">COM port name (COM1, COM2...)</param>
        /// <param name="baudRate">Baudrate for the communication</param>
        /// <param name="number_of_servos">Number of servos channels to use</param>
        public SSC32(string portName, int baudRate, int number_of_servos)
        {
            int[] servos_channels = new int[number_of_servos];
            for (int i = 0; i < servos_channels.Length; i++)
            {
                servos_channels[i] = i;
            }
            Initialize(servos_channels);

            Connect(portName, baudRate);

            //updateServos();
        }
#endregion

        #region Connect/Disconnect
        /// <summary>
        /// Connect to the SSC32 board
        /// </summary>
        /// <param name="portName">COM port name (COM1, COM2...)</param>
        /// <param name="baudRate">Baudrate for the communication</param>
        /// <returns>True if it succeeds to connect</returns>
        public bool Connect(string portName, int baudRate)
        {
            bool ret = false;
            // Connect to the given port at the given speed
            s_port = new SerialPort(portName, baudRate);
            // Set theline separator
            s_port.NewLine = "\r";
            // Sets the timeouts
            s_port.ReadTimeout = 1000;

            // Try to open the port
            s_port.Open();

            // Empty the input buffer
            s_port.ReadExisting();

            try
            {
                if (getSoftwareVersion().Contains("SSC32") == true)
                {
                    ret = true;
                }
            }
            catch (TimeoutException t_e)
            {
                Disconnect();
                throw new TimeoutException("Unable to find the SSC32 device, no response received...");
            }
            catch (Exception exc)
            {
                Disconnect();
                throw new Exception("Unable to find the SSC32 device : " + exc.Message);
            }

            return ret;
        }

        /// <summary>
        /// Disconnect from the SSC32 card and the associated COM port
        /// </summary>
        public void Disconnect()
        {
            if (CheckForComPortOpen() == true)
            {
                s_port.Close();
                s_port.Dispose();
            }
        }
        #endregion

        #region SS32 Commands
        /// <summary>
        /// Gets the software version of the SSC32
        /// </summary>
        /// <returns>Software version</returns>
        public string getSoftwareVersion()
        {
            string answer = "";

            // Ask for the version number string
            sendCommand("VER ");

            // Read the SSC32 answer
            answer = readAnswer();

            // Return the answer
            return answer;
        }

        /// <summary>
        /// Stops immediately a servo at its current position
        /// </summary>
        /// <param name="servo_channel">Servo channel to stop</param>
        public void Stop(int servo_channel)
        {
            sendCommand("STOP " + servo_channel);
        }

        /// <summary>
        /// Updates physically the position of the servos (sends the proper commands to the SSC32)
        /// </summary>
        public void updateServos()
        {
            string command = "";

            // Check all servos...
            for (int i = 0; i < servos.Length; i++)
            {
                // ... to see if pulse width changed
                if (servos[i].pulse_width_changed == true)
                {
                    // It did, so add it to the command string
                    command += servos[i].ToString();

                    servos[i].pulse_width_changed = false;
                }
            }

            // If we have something to update
            if (command != "")
            {
                // Send the command to update the servos
                sendCommand(command + " T" + MotionTime);
            }
        }

        /// <summary>
        /// Checks if the movement is finished or not
        /// </summary>
        /// <returns>False if the movement is not finished, True if it's finished.</returns>
        public bool IsMovementFinished()
        {
            bool ret = false;

            sendCommand("Q");

            char response = readChar();
            if (response == '.')
            {
                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// Query pulse width of a given servo
        /// </summary>
        /// <param name="servo_channel">Servo to get the pulse width</param>
        /// <returns>Pulse width in µs</returns>
        public int queryPulseWidth(byte servo_channel)
        {
            sendCommand("QP " + servo_channel);

            int response = (int)readChar() * 10;

            return response;
        }

        /// <summary>
        /// Waits for any movement to be finished
        /// </summary>
        public void WaitForMovementCompletion()
        {
            while (IsMovementFinished() == false)
            {
                Thread.Sleep(10);
            }
        }

        #region Analog Inputs

        bool[] firstAnalogReadDone;    // Indicates whether the first dummy analog read has been done or not for each input

        /// <summary>
        /// Reads the analog voltage present on the given SS32 input
        /// <remarks>The first time a V* command is used, the pin will be converted to analog without pullup. The result of this first read will not return valid data.</remarks>
        /// </summary>
        /// <param name="analog_input">Analog input to read</param>
        /// <returns>The voltage image read from the input</returns>
        public byte readAnalogInput(SSC32Inputs analog_input)
        {
            byte ret = 0;

            // Create the command following what's asked
            char command = 'A';
            command += (char)analog_input;

            // Send the command
            sendCommand("V" + command);

            // Read the result
            char answer = readChar();
            ret = (byte)answer;

            // If the dummy read has not been done
            if (firstAnalogReadDone[(int)analog_input] == false)
            {
                // Tick this as done
                firstAnalogReadDone[(int)analog_input] = true;

                // Ask the value again
                ret = readAnalogInput(analog_input);
            }

            return ret;
        }
        #endregion

        #region Digital Inputs/Outputs

        /// <summary>
        /// Sets the value of a digital output
        /// </summary>
        /// <param name="output_channel">Output channel to set</param>
        /// <param name="value">Value to set</param>
        public void setDigitalOutput(byte output_channel, bool value)
        {
            if (output_channel < Servo.MIN_CHANNEL && output_channel > Servo.MAX_CHANNEL)
            {
                throw new ArgumentOutOfRangeException("output_channel is out of range !");
            }
            sendCommand("#" + output_channel + (value == true ? "H" : "L"));
        }

        public void setBankByteOutput(byte bank_number, byte value)
        {
            if (bank_number < Servo.MIN_CHANNEL / 8 && bank_number > Servo.MAX_CHANNEL / 8)
            {
                throw new ArgumentOutOfRangeException("bank_number is out of range !");
            }
            sendCommand("#" + bank_number + ":" + value);
        }

        /// <summary>
        /// Reads a digital input value
        /// </summary>
        /// <param name="digital_input">Digital input to test</param>
        /// <param name="latched_request">Asks for a latched input data. See <see cref="http://www.lynxmotion.com/images/html/build136.htm"/> for more details</param>
        /// <returns>True if voltage level is high, false for low</returns>
        public bool readDigitalInput(SSC32Inputs digital_input, bool latched_request)
        {
            bool ret = false;

            // Create the command following what's asked
            char command = 'A';
            command += (char)digital_input;

            string l_request = "";
            if (latched_request == true)
            {
                l_request = "L";
            }

            // Send the command
            sendCommand(command + l_request);

            // The input is no longer an analog input (if it was...), so we'll have to make a dummy read next time
            firstAnalogReadDone[(int)digital_input] = false;

            // Read the result
            char answer = readChar();
            ret = (answer == '1');

            return ret;
        }
        #endregion

        #region Registers Read/Write
        public void writeRegister(byte register_number, int value)
        {
            sendCommand("R" + register_number + "=" + value);
        }

        public string readRegister(byte register_number)
        {
            string answer = "";

            // Ask for the version number string
            sendCommand("R" + register_number);

            // Read the SSC32 answer
            answer = readAnswer();

            // Return the answer
            return answer;
        }

        public void setRegistersToDefault()
        {
            // This command shouldnt be called during a movement, so wait for it to finish...
            WaitForMovementCompletion();

            // Save the current ReadTimeout
            int savedTimeout = s_port.ReadTimeout;
            // Since this operation may last more than one second, change the ReadTimeout to allow this
            s_port.ReadTimeout = 5000;

            // Call the command
            sendCommand("RDFLT");

            // Wait for the answer of the SSC32
            readAnswer();

            // Revert back the ReadTimeout
            s_port.ReadTimeout = savedTimeout;
        }

        [Flags]
        public enum R0RegisterMask
        {
            StartupStringEnable = 0x1,
            TXDelayPacingEnable = 0x2,
            InitialPulseOffsetEnable = 0x4,
            InitialPulseWidthEnable = 0x8,
            GlobalDisable = 0x8000
        }

        public void enableFunction(R0RegisterMask function,bool enable)
        {
            string R0_str = readRegister(0);
            int R0_int = int.Parse(R0_str);
            if (enable == true)
            {
                R0_int |= (int)function;
            }
            else
            {
                R0_int &= ~(int)function;
            }
            writeRegister(0, R0_int);
        }
        #endregion

        #region Startup strings

        /// <summary>
            /// Reads the startup string from the SSC32
            /// </summary>
            /// <returns>The startup string</returns>
            public string getStartupString()
            {
                string answer = "";

                // Ask for the startup string
                sendCommand("SS");

                // Read the SSC32 answer
                answer = readAnswer();

                if (answer != "" && answer.Length >= 2)
                {
                    // Remove the first and the last character (which are both ")
                    answer = answer.Substring(1, answer.Length - 2);
                }

                // Return the answer
                return answer;
            }

            /// <summary>
            /// Deletes the startup string from the SSC32
            /// </summary>
            public void deleteStartupString()
            {
                deleteNLastStartupStringCharacters(255);
            }

            /// <summary>
            /// Deletes the last N characters located at the end of the startup string
            /// </summary>
            /// <param name="n_chars_to_delete">Number of characters to remove</param>
            public void deleteNLastStartupStringCharacters(byte n_chars_to_delete)
            {
                sendCommand("SSDEL " + n_chars_to_delete);
            }


            /// <summary>
            /// Checks if a given text is a correct startup string
            /// </summary>
            /// <param name="startup_string_text">Startup string to check</param>
            /// <returns>True if it's a correct one, if not, throws an exception</returns>
            protected bool CheckStartupString(string startup_string_text)
            {
                bool ret = false;
                Regex regex = new Regex("R[0-9]{1,2}=");

                // Lynxmotion says : The following commands should not be used in a startup string: EER, EEW, R=, SSCAT, SSDEL.
                string text_check = startup_string_text.ToUpper().Replace(" ", ""); // Make a copy of the text to write, put it to upper case and remove spaces
                if (text_check.Contains("EER") || text_check.Contains("EEW") || regex.IsMatch(text_check) || text_check.Contains("SSCAT") || text_check.Contains("SSDEL"))
                {
                    throw new ArgumentException("The following commands should not be used in a startup string: EER, EEW, R=, SSCAT, SSDEL.");
                }
                else
                {
                    ret = true;
                }

                return ret;
            }

            /// <summary>
            /// Appends text to the current startup string on the SS32
            /// </summary>
            /// <param name="text_to_append">Text to append to the current startup string</param>
            public void appendStartupString(string text_to_append)
            {
                if (text_to_append.Length <= STARTUP_STRING_MAX_LENGTH)
                {
                    if (CheckStartupString(text_to_append) == true)
                    {
                        sendCommand("SSCAT " + text_to_append);
                    }
                } else {
                    throw new ArgumentException("The given string is too long ! " + STARTUP_STRING_MAX_LENGTH + " characters max !");
                }
            }

            /// <summary>
            /// Sets the startup string (i.e. deletes the current one and replaces it by the new one)
            /// </summary>
            /// <param name="startupString">The startup string</param>
            public void setStartupString(string startupString)
            {
                if (startupString.Length <= STARTUP_STRING_MAX_LENGTH)
                {
                    if (CheckStartupString(startupString) == true)
                    {
                        deleteStartupString();
                        appendStartupString(startupString);
                    }
                } else {
                    throw new ArgumentException("The given string is too long ! " + STARTUP_STRING_MAX_LENGTH + " characters max !");
                }
            }
            #endregion

        #endregion

        #region Send/receive commands
        /// <summary>
        /// Sends a command to the SSC32 board
        /// </summary>
        /// <param name="command">Command string to be sent <remarks>The end string is automatically completed with a carriage return</remarks></param>
        protected void sendCommand(string command)
        {
            if (CheckForComPortOpen() == true)
            {
                lock (s_port)
                {
                    // Send the given command to the SSC32
                    s_port.WriteLine(command);
                }
            }
        }

        /// <summary>
        /// Reads the answer which is currently stored in the input buffer
        /// </summary>
        /// <returns>Answer from the input buffer</returns>
        protected string readExistingAnswer()
        {
            return s_port.ReadExisting();
        }

        /// <summary>
        /// Waits for an answer and reads it
        /// </summary>
        /// <returns>The answer received</returns>
        protected string readAnswer()
        {
            return s_port.ReadLine();
        }

        /// <summary>
        /// Waits for one character to be received from the SSC32
        /// </summary>
        /// <returns>The char read</returns>
        protected char readChar()
        {
            return (char)s_port.ReadChar();
        }
        #endregion

        /// <summary>
        /// Time in mS for the entire move, affects all channels
        /// </summary>
        public UInt16 MotionTime
        {
            get;
            set;
        }

        /// <summary>
        /// Checks if the comm port is open and generates an exception if not
        /// </summary>
        /// <returns>True if the comm port is open</returns>
        private bool CheckForComPortOpen()
        {
            bool ret = false;

            if (s_port != null)
            {
                if (s_port.IsOpen == false)
                {
                    throw new System.IO.IOException("Unable to perform this action because the COM port is not currently opened");
                }
                else
                {
                    ret = true;
                }
            }

            return ret;
        }

        /// <summary>
        /// Dispose function
        /// </summary>
        public void Dispose()
        {
            Disconnect();

            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing) { }
    }
}
