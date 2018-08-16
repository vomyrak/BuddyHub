using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace Lynxmotion
{
    public class ArmControl
    {
        public static void IncreaseGrip(AL5C al5c)
        {
            al5c.IncreaseGripper_F();
            al5c.updateServos();
        }

        public static void DecreaseGrip(AL5C al5c)
        {
            al5c.DecreaseGripper_F();
            al5c.updateServos();
        }


        public static void MoveForward(AL5C al5c)
        {
            al5c.IncreaseShoulder_F();
            al5c.IncreaseElbow_F();
            al5c.updateServos();
        }

        public static void MoveBackward(AL5C al5c)
        {
            al5c.DecreaseShoulder_F();
            al5c.DecreaseElbow_F();
            al5c.updateServos();
        }


        public static void TurnRight(AL5C al5c)
        {
            al5c.IncreaseShoulderBase_F();
            al5c.updateServos();
        }

        public static void TurnLeft(AL5C al5c)
        {
            al5c.DecreaseShoulderBase_F();
            al5c.updateServos();
        }


        public static void TiltUp(AL5C al5c)
        {         
            al5c.IncreaseElbow_F();
            al5c.updateServos();
        }

        public static void TiltDown(AL5C al5c)
        {
            
            al5c.DecreaseElbow_F();
            al5c.updateServos();
        }

    }
}
