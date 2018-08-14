using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lynxmotion
{
    public partial class AL5D
    {
        public float GetGripper_F()
        {
            return ((float)GripperServo.PulseWidth - (float)Servo.MIN_PULSE_WIDTH) / ((float)Servo.MAX_PULSE_WIDTH - (float)Servo.MIN_PULSE_WIDTH);
        }

        public short GetGripper_PW()
        {
            return GripperServo.PulseWidth;
        }

        public double GetGripper_Angle()
        {
            return ((double)GripperServo.PulseWidth - (double)Servo.MIN_PULSE_WIDTH) * 180 / (double)(Servo.MAX_PULSE_WIDTH - (double)Servo.MIN_PULSE_WIDTH);
        }

        public void IncreaseGripper_F()
        {
            float angle = GetGripper_F();
            if (angle <= 0.95)
                setGripper_F(angle + 0.1f);
            else
                setGripper_PW(Servo.MAX_PULSE_WIDTH);
            updateServos();
        }

        public void DecreaseGripper_F()
        {
            float angle = GetGripper_F();
            if (angle >= 0.05)
                setGripper_F(angle - 0.1f);
            else
                setGripper_PW(Servo.MIN_PULSE_WIDTH);
            updateServos();
        }

        public void SetAllServosToMin()
        {
            setElbow_PW(Servo.MIN_PULSE_WIDTH);
            setGripper_PW(Servo.MIN_PULSE_WIDTH);
            setShoulderBase_PW(Servo.MIN_PULSE_WIDTH);
            setShoulder_PW(Servo.MIN_PULSE_WIDTH);
            setWristRotate_PW(Servo.MIN_PULSE_WIDTH);
            setWrist_PW(Servo.MIN_PULSE_WIDTH);
        }
        public void SetAllServosToMax()
        {
            setElbow_PW(Servo.MAX_PULSE_WIDTH);
            setGripper_PW(Servo.MAX_PULSE_WIDTH);
            setShoulderBase_PW(Servo.MAX_PULSE_WIDTH);
            setShoulder_PW(Servo.MAX_PULSE_WIDTH);
            setWristRotate_PW(Servo.MAX_PULSE_WIDTH);
            setWrist_PW(Servo.MAX_PULSE_WIDTH);
        }
    }
}
