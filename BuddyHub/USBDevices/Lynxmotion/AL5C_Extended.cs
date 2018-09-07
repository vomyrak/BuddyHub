using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lynxmotion
{
    partial class AL5C
    {

        public float GetServo_F(Servo servoIn)
        {
            return ((float)servoIn.PulseWidth - (float)Servo.MIN_PULSE_WIDTH) / ((float)Servo.MAX_PULSE_WIDTH - (float)Servo.MIN_PULSE_WIDTH);
        }


        #region Incr/Decr Gripper
        public void IncreaseGripper_F()
        {
            float angle = GetServo_F(GripperServo);
            if (angle <= 0.95)
                setGripper_F(angle + 0.01f);
            else
                setGripper_PW(Servo.MAX_PULSE_WIDTH);
        }

        public void DecreaseGripper_F()
        {
            float angle = GetServo_F(GripperServo);
            if (angle >= 0.05)
                setGripper_F(angle - 0.01f);
            else
                setGripper_PW(Servo.MIN_PULSE_WIDTH);
        }
        #endregion


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

