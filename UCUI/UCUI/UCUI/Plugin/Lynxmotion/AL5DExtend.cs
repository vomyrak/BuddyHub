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
            return (GripperServo.PulseWidth - Servo.MIN_PULSE_WIDTH) / (Servo.MAX_PULSE_WIDTH - Servo.MIN_PULSE_WIDTH);
        }

        public short GetGripper_PW()
        {
            return GripperServo.PulseWidth;
        }

        public double GetGripper_Angle()
        {
            return (GripperServo.PulseWidth - Servo.MIN_PULSE_WIDTH) * 180 / (Servo.MAX_PULSE_WIDTH - Servo.MIN_PULSE_WIDTH);
        }

        public void IncreaseGripper_F()
        {
            float angle = GetGripper_F();
            if (angle <= 0.95)
                setGripper_F(angle + 0.05f);
            updateServos();
        }

        public void DecreaseGripper_F()
        {
            float angle = GetGripper_F();
            if (angle >= 0.05)
                setGripper_F(angle - 0.05f);
            updateServos();
        }
    }
}
