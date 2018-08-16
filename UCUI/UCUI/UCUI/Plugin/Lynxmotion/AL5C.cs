// Lynxmotion SSC32/AL5x Robotic Arm Library
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
using System.ComponentModel;
using System.ComponentModel.Composition;
using CSharpServer;

namespace Lynxmotion
{

    [Export(typeof(CSharpServer.IDevice))]
    /// <summary>
    /// Class definition of an AL5C robot arm driven by a SSC32 board
    /// <remarks>This requires the whole arm has been mounted and wired following the Lynxmotion manual. See <see cref="http://www.lynxmotion.com/images/html/build142.htm"/> for mounting details.</remarks>
    /// </summary>
    public partial class AL5C : SSC32, IDevice
    {

        public AL5C()
        {        }


        public string GetSerialPort() {
            SSC32ENumerationResult[] SSC32s = AL5C.EnumerateConnectedSSC32(9600);
            if (SSC32s.Length > 0)
            {
                return SSC32s[0].PortName;
            }
            return "";
        }
        /// <summary>
        /// Allocate each servo to its corresponding channel
        /// </summary>
        /// <param name="shoulderBaseServoChannel">Shoulder base servo channel</param>
        /// <param name="shoulderServoChannel">Shoulder servo channel</param>
        /// <param name="elbowServoChannel">Elbow servo channel</param>
        /// <param name="wristServoChannel">Wrist servo channel</param>
        /// <param name="gripperServoChannel">Gripper servo channel</param>
        /// <param name="wristRotateServoChannel">Wrist rotate servo channel</param>
        private void Initialize(byte shoulderBaseServoChannel, byte shoulderServoChannel, byte elbowServoChannel,
            byte wristServoChannel, byte gripperServoChannel, byte wristRotateServoChannel)
        {
            ShoulderBaseServo = servos[shoulderBaseServoChannel];
            ShoulderServo = servos[shoulderServoChannel];
            ElbowServo = servos[elbowServoChannel];
            WristServo = servos[wristServoChannel];
            GripperServo = servos[gripperServoChannel];
            WristRotateServo = servos[wristRotateServoChannel];
        }

        
        /// <summary>
        /// Creates an AL5C robot arm
        /// </summary>
        /// <param name="portName">Port name to access the AL5C arm (i.e. the SSC32 board)</param>
        public AL5C(string portName)
            : base(portName, 9600, 6)
        {
            Initialize(0, 1, 2, 3, 4, 5);
        }

        /// <summary>
        /// Creates an AL5C robot arm by specifying a custom servo channel allocation
        /// </summary>
        /// <param name="portName">Port name to access the AL5C arm (i.e. the SSC32 board)</param>
        /// <param name="shoulderBaseServoChannel">Shoulder base servo channel</param>
        /// <param name="shoulderServoChannel">Shoulder servo channel</param>
        /// <param name="elbowServoChannel">Elbow servo channel</param>
        /// <param name="wristServoChannel">Wrist servo channel</param>
        /// <param name="gripperServoChannel">Gripper servo channel</param>
        /// <param name="wristRotateServoChannel">Wrist rotate servo channel</param>
        public AL5C(string portName, byte shoulderBaseServoChannel, byte shoulderServoChannel, byte elbowServoChannel,
            byte wristServoChannel, byte gripperServoChannel, byte wristRotateServoChannel)
            : base(portName, 9600, 6)
        {
            Initialize(shoulderBaseServoChannel, shoulderServoChannel, elbowServoChannel, wristServoChannel, gripperServoChannel, wristRotateServoChannel);
        }

        #region Servos definitions
        /// <summary>
        /// Sets or gets the gripper servo
        /// </summary>
        public Servo GripperServo
        {
            get;
            set;
        }
        /// <summary>
        /// Sets or gets the wrist rotate servo
        /// </summary>
        public Servo WristRotateServo
        {
            get;
            set;
        }
        /// <summary>
        /// Sets or gets the wrist servo
        /// </summary>
        public Servo WristServo
        {
            get;
            set;
        }
        /// <summary>
        /// Sets or gets the elbow servo
        /// </summary>
        public Servo ElbowServo
        {
            get;
            set;
        }
        /// <summary>
        /// Sets or gets the shoulder servo
        /// </summary>
        public Servo ShoulderServo
        {
            get;
            set;
        }
        /// <summary>
        /// Sets or gets the shoulder base servo
        /// </summary>
        public Servo ShoulderBaseServo
        {
            get;
            set;
        }
        #endregion

        #region Pulse width handled movements
        /// <summary>
        /// Sets the shoulder base pulse width
        /// </summary>
        /// <param name="pulse_width">Pulse width in µs</param>
        public void setShoulderBase_PW(short pulse_width)
        {
            ShoulderBaseServo.setPulseWidth(pulse_width);
        }
        /// <summary>
        /// Sets the shoulder pulse width
        /// </summary>
        /// <param name="pulse_width">Pulse width in µs</param>
        public void setShoulder_PW(short pulse_width)
        {
            ShoulderServo.setPulseWidth(pulse_width);
        }
        /// <summary>
        /// Sets the elbow pulse width
        /// </summary>
        /// <param name="pulse_width">Pulse width in µs</param>
        public void setElbow_PW(short pulse_width)
        {
            ElbowServo.setPulseWidth(pulse_width);
        }
        /// <summary>
        /// Sets the wrist pulse width
        /// </summary>
        /// <param name="pulse_width">Pulse width in µs</param>
        public void setWrist_PW(short pulse_width)
        {
            WristServo.setPulseWidth(pulse_width);
        }
        /// <summary>
        /// Sets the wrist rotate pulse width
        /// </summary>
        /// <param name="pulse_width">Pulse width in µs</param>
        public void setWristRotate_PW(short pulse_width)
        {
            WristRotateServo.setPulseWidth(pulse_width);
        }
        /// <summary>
        /// Sets the gripper pulse width
        /// </summary>
        /// <param name="pulse_width">Pulse width in µs</param>
        public void setGripper_PW(short pulse_width)
        {
            GripperServo.setPulseWidth(pulse_width);
        }
        #endregion


        #region Angle handled movements (angle from 0 to 180 degrees)
        /// <summary>
        /// Sets the shoulder base angle
        /// </summary>
        /// <param name="angle">Angle in degrees (0 corresponds to the position calibrated when following the Lynxmotion mounting instructions)</param>
        public void setShoulderBase_Angle(double angle)
        {
            ShoulderBaseServo.setPulseWidth((short)(angle * (Servo.MAX_PULSE_WIDTH - Servo.MIN_PULSE_WIDTH) / 180 + Servo.MIN_PULSE_WIDTH));
        }
        /// <summary>
        /// Sets the shoulder angle
        /// </summary>
        /// <param name="angle">Angle in degrees (0 corresponds to the position calibrated when following the Lynxmotion mounting instructions)</param>
        public void setShoulder_Angle(double angle)
        {
            ShoulderServo.setPulseWidth((short)(angle * (Servo.MAX_PULSE_WIDTH - Servo.MIN_PULSE_WIDTH) / 180 + Servo.MIN_PULSE_WIDTH));
        }
        /// <summary>
        /// Sets the elbow angle
        /// </summary>
        /// <param name="angle">Angle in degrees (0 corresponds to the position calibrated when following the Lynxmotion mounting instructions)</param>
        public void setElbow_Angle(double angle)
        {
            ElbowServo.setPulseWidth((short)(angle * (Servo.MAX_PULSE_WIDTH - Servo.MIN_PULSE_WIDTH) / 180 + Servo.MIN_PULSE_WIDTH));
        }
        /// <summary>
        /// Sets the wrist angle
        /// </summary>
        /// <param name="angle">Angle in degrees (0 corresponds to the position calibrated when following the Lynxmotion mounting instructions)</param>
        public void setWrist_Angle(double angle)
        {
            WristServo.setPulseWidth((short)(angle * (Servo.MAX_PULSE_WIDTH - Servo.MIN_PULSE_WIDTH) / 180 + Servo.MIN_PULSE_WIDTH));
        }
        /// <summary>
        /// Sets the wrist rotate angle
        /// </summary>
        /// <param name="angle">Angle in degrees (0 corresponds to the position calibrated when following the Lynxmotion mounting instructions)</param>
        public void setWristRotate_Angle(double angle)
        {
            WristRotateServo.setPulseWidth((short)(angle * (Servo.MAX_PULSE_WIDTH - Servo.MIN_PULSE_WIDTH) / 180 + Servo.MIN_PULSE_WIDTH));
        }
        /// <summary>
        /// Sets the gripper angle
        /// </summary>
        /// <param name="angle">Angle in degrees (0 corresponds to the position calibrated when following the Lynxmotion mounting instructions)</param>
        public void setGripper_Angle(double angle)
        {
            GripperServo.setPulseWidth((short)(angle * (Servo.MAX_PULSE_WIDTH - Servo.MIN_PULSE_WIDTH) / 180 + Servo.MIN_PULSE_WIDTH));
        }
        #endregion


        #region Float handled movements (float from 0 to 1)
        /// <summary>
        /// Sets the shoulder base angle
        /// </summary>
        /// <param name="value">Value from 0.0 to 1.0, with 0.0 corresponding to MIN_PULSE_WIDTH and 1.0 to MAX_PULSE_WIDTH</param>
        public void setShoulderBase_F(float value)
        {
            ShoulderBaseServo.setPulseWidth((short)(value * (Servo.MAX_PULSE_WIDTH - Servo.MIN_PULSE_WIDTH) + Servo.MIN_PULSE_WIDTH));
        }
        /// <summary>
        /// Sets the shoulder angle
        /// </summary>
        /// <param name="value">Value from 0.0 to 1.0, with 0.0 corresponding to MIN_PULSE_WIDTH and 1.0 to MAX_PULSE_WIDTH</param>
        public void setShoulder_F(float value)
        {
            ShoulderServo.setPulseWidth((short)(value * (Servo.MAX_PULSE_WIDTH - Servo.MIN_PULSE_WIDTH) + Servo.MIN_PULSE_WIDTH));
        }
        /// <summary>
        /// Sets the elbow angle
        /// </summary>
        /// <param name="value">Value from 0.0 to 1.0, with 0.0 corresponding to MIN_PULSE_WIDTH and 1.0 to MAX_PULSE_WIDTH</param>
        public void setElbow_F(float value)
        {
            ElbowServo.setPulseWidth((short)(value * (Servo.MAX_PULSE_WIDTH - Servo.MIN_PULSE_WIDTH) + Servo.MIN_PULSE_WIDTH));
        }
        /// <summary>
        /// Sets the wrist angle
        /// </summary>
        /// <param name="value">Value from 0.0 to 1.0, with 0.0 corresponding to MIN_PULSE_WIDTH and 1.0 to MAX_PULSE_WIDTH</param>
        public void setWrist_F(float value)
        {
            WristServo.setPulseWidth((short)(value * (Servo.MAX_PULSE_WIDTH - Servo.MIN_PULSE_WIDTH) + Servo.MIN_PULSE_WIDTH));
        }
        /// <summary>
        /// Sets the wrist rotate angle
        /// </summary>
        /// <param name="value">Value from 0.0 to 1.0, with 0.0 corresponding to MIN_PULSE_WIDTH and 1.0 to MAX_PULSE_WIDTH</param>
        public void setWristRotate_F(float value)
        {
            WristRotateServo.setPulseWidth((short)(value * (Servo.MAX_PULSE_WIDTH - Servo.MIN_PULSE_WIDTH) + Servo.MIN_PULSE_WIDTH));
        }
        /// <summary>
        /// Sets the gripper angle
        /// </summary>
        /// <param name="value">Value from 0.0 to 1.0, with 0.0 corresponding to MIN_PULSE_WIDTH and 1.0 to MAX_PULSE_WIDTH</param>
        public void setGripper_F(float value)
        {
            GripperServo.setPulseWidth((short)(value * (Servo.MAX_PULSE_WIDTH - Servo.MIN_PULSE_WIDTH) + Servo.MIN_PULSE_WIDTH));
        }
        #endregion

        /// <summary>
        /// Stops all the servos to a "idle" no driving position
        /// </summary>
        public void RelaxAllServos()
        {
            for (int i = 0; i < servos.Length; i++)
            {
                servos[i].setPulseWidth(0);
            }
        }

        /// <summary>
        /// Moves the arm to the default idle position (requires that the arm is calibrated as described in the AL5C manual)
        /// </summary>
        /// <param name="MoveShoulderBase">If False, does not move the base</param>
        public void GoToIdlePosition(bool MoveShoulderBase)
        {
            if (MoveShoulderBase == true)
            {
                setShoulderBase_F(0.5f);
            }
            setShoulder_PW(1983);
            setElbow_PW(2106);
            setWrist_PW(1112);
            
        }
        public object ConnectDevice(string serialPort)
        {
            
            return new AL5C(serialPort);
        }
    }
}
