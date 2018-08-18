using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lynxmotion
{
    public partial class AL5D
    {
        public float GetServo_F(Servo servoIn)
        {
            return ((float)servoIn.PulseWidth - (float)Servo.MIN_PULSE_WIDTH) / ((float)Servo.MAX_PULSE_WIDTH - (float)Servo.MIN_PULSE_WIDTH);
        }
        public short GetServo_PW(Servo servoIn)
        {
            return servoIn.PulseWidth;
        }

        public double GetServo_Angle(Servo servoIn)
        {
            return ((double)servoIn.PulseWidth - (double)Servo.MIN_PULSE_WIDTH) * 180 / (double)(Servo.MAX_PULSE_WIDTH - (double)Servo.MIN_PULSE_WIDTH);
        }

        public void IncreaseGripper_F()
        {
            float angle = GetServo_F(GripperServo);
            if (angle <= 0.95)
                setGripper_F(angle + 0.01f);
            else
                setGripper_PW(Servo.MAX_PULSE_WIDTH);
            updateServos();
        }

        public void DecreaseGripper_F()
        {
            float angle = GetServo_F(GripperServo);
            if (angle >= 0.05)
                setGripper_F(angle - 0.01f);
            else
                setGripper_PW(Servo.MIN_PULSE_WIDTH);
            updateServos();
        }



        #region Incr/Decr Shoulder
        public void IncreaseShoulder_F()
        {
            float angle = GetServo_F(ShoulderServo);
            if (angle <= 0.95)
                setShoulder_F(angle + 0.01f);
            else
                setShoulder_PW(Servo.MAX_PULSE_WIDTH);
        }

        public void DecreaseShoulder_F()
        {
            float angle = GetServo_F(ShoulderServo);
            if (angle >= 0.05)
                setShoulder_F(angle - 0.01f);
            else
                setShoulder_PW(Servo.MIN_PULSE_WIDTH);
        }
        #endregion


        #region Incr/Decr ShoulderBase
        public void IncreaseShoulderBase_F()
        {
            float angle = GetServo_F(ShoulderBaseServo);
            if (angle <= 0.95)
                setShoulderBase_F(angle + 0.01f);
            else
                setShoulderBase_PW(Servo.MAX_PULSE_WIDTH);
        }

        public void DecreaseShoulderBase_F()
        {
            float angle = GetServo_F(ShoulderBaseServo);
            if (angle >= 0.05)
                setShoulderBase_F(angle - 0.01f);
            else
                setShoulderBase_PW(Servo.MIN_PULSE_WIDTH);
        }
        #endregion


        #region Incr/Decr Elbow
        public void IncreaseElbow_F()
        {
            float angle = GetServo_F(ElbowServo);
            if (angle <= 0.95)
                setElbow_F(angle + 0.01f);
            else
                setElbow_PW(Servo.MAX_PULSE_WIDTH);
        }

        public void DecreaseElbow_F()
        {
            float angle = GetServo_F(ElbowServo);
            if (angle >= 0.05)
                setElbow_F(angle - 0.01f);
            else
                setElbow_PW(Servo.MIN_PULSE_WIDTH);
        }
        #endregion

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

        public void IncreaseGrip()
        {
            IncreaseGripper_F();
            updateServos();
        }

        public void DecreaseGrip()
        {
            DecreaseGripper_F();
            updateServos();
        }

        public void IncreaseWrist_F()
        {
            float angle = GetServo_F(WristServo);
            if (angle <= 0.95)
                setWrist_F(angle + 0.01f);
            else
                setWrist_PW(Servo.MAX_PULSE_WIDTH);
        }

        public void DecreaseWrist_F()
        {
            float angle = GetServo_F(WristServo);
            if (angle >= 0.05)
                setWrist_F(angle - 0.01f);
            else
                setWrist_PW(Servo.MIN_PULSE_WIDTH);
        }

        public void MoveForward()
        {
            IncreaseShoulder_F();
            IncreaseElbow_F();
            updateServos();
        }

        public void MoveBackward()
        {
            DecreaseShoulder_F();
            DecreaseElbow_F();
            updateServos();
        }


        public void TurnRight()
        {
            IncreaseShoulderBase_F();
            updateServos();
        }

        public void TurnLeft()
        {
            DecreaseShoulderBase_F();
            updateServos();
        }


        public void TiltUp()
        {
            IncreaseElbow_F();
            IncreaseWrist_F();
            updateServos();
        }

        public void TiltDown()
        {

            DecreaseElbow_F();
            DecreaseWrist_F();
            updateServos();
        }
    }
}
