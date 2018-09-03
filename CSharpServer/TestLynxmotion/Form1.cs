// Lynxmotion SSC32/AL5x Robotic Arm Sample Applications
//
// Copyright © Rémy Dispagne, 2014
// All rights reserved. 3-BSD License:
//
//   Redistribution and use in source and binary forms, with or without
//   modification, are permitted provided that the following conditions are met:
//
//      * Redistributions of source code must retain the above copyright
//        notice, this list of conditions and the following disclaimer.
//
//      * Redistributions in binary form must reproduce the above copyright
//        notice, this list of conditions and the following disclaimer in the
//        documentation and/or other materials provided with the distribution.
//
//      * Neither the name of the Lynxmotion SS32/AL5x Robotic Arm Library authors nor the
//        names of its contributors may be used to endorse or promote products
//        derived from this software without specific prior written permission.
//
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//  ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER BE LIABLE FOR ANY
//  DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//  (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//  ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//

using System;
using System.Windows.Forms;
using Lynxmotion;

namespace TestLynxmotion
{
    public partial class Form1 : Form
    {
        AL5C al5c;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SSC32ENumerationResult[] SSC32s = AL5C.EnumerateConnectedSSC32(9600);

            if (SSC32s.Length > 0)
            {
                al5c = new AL5C(SSC32s[0].PortName);
            }
            else
            {
                MessageBox.Show("Unable to find any SSC32 board on this computer...\r\nClosing.");
                Application.Exit();
            }

            trackBar1.Minimum = Servo.MIN_PULSE_WIDTH;
            trackBar1.Maximum = Servo.MAX_PULSE_WIDTH;
            trackBar1.Value = 1500;
            trackBar1_Scroll(trackBar1, e);
            trackBar2.Minimum = Servo.MIN_PULSE_WIDTH;
            trackBar2.Maximum = Servo.MAX_PULSE_WIDTH;
            trackBar2.Value = 1500;
            trackBar2_Scroll(trackBar2, e);
            trackBar3.Minimum = Servo.MIN_PULSE_WIDTH;
            trackBar3.Maximum = Servo.MAX_PULSE_WIDTH;
            trackBar3.Value = 1500;
            trackBar3_Scroll(trackBar3, e);
            trackBar4.Minimum = Servo.MIN_PULSE_WIDTH;
            trackBar4.Maximum = Servo.MAX_PULSE_WIDTH;
            trackBar4.Value = 1500;
            trackBar4_Scroll(trackBar4, e);
            trackBar5.Minimum = Servo.MIN_PULSE_WIDTH;
            trackBar5.Maximum = Servo.MAX_PULSE_WIDTH;
            trackBar5.Value = 1500;
            trackBar5_Scroll(trackBar5, e);

            al5c.RelaxAllServos();

            numericUpDown1.Value = al5c.MotionTime;

            timer1.Enabled = true;
        }

        void ScrollCheck()
        {
            if (checkBox1.Checked == true)
            {
                al5c.updateServos();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            al5c.updateServos();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            al5c.RelaxAllServos();
            al5c.updateServos();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            al5c.setShoulderBase_PW((short)((TrackBar)sender).Value);
            label2.Text = "Base : PW=" + (short)((TrackBar)sender).Value;
            ScrollCheck();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            al5c.setShoulderBase_PW(0);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            al5c.setShoulder_PW((short)((TrackBar)sender).Value);
            label3.Text = "Shoudler : PW=" + (short)((TrackBar)sender).Value;
            ScrollCheck();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            al5c.setShoulder_PW(0);
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            al5c.setElbow_PW((short)((TrackBar)sender).Value);
            label4.Text = "Elbow : PW=" + (short)((TrackBar)sender).Value;
            ScrollCheck();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            al5c.setElbow_PW(0);
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            al5c.setWrist_PW((short)((TrackBar)sender).Value);
            label5.Text = "Wrist : PW=" + (short)((TrackBar)sender).Value;
            ScrollCheck();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            al5c.setWrist_PW(0);
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            al5c.setGripper_PW((short)((TrackBar)sender).Value);
            label6.Text = "Gripper : PW=" + (short)((TrackBar)sender).Value;
            ScrollCheck();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            al5c.setGripper_PW(0);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
            
            try
            {
                al5c.RelaxAllServos();
                al5c.updateServos();
            }
            catch (Exception exc)
            {
            }

            try
            {
                al5c.Dispose();
            }
            catch (Exception exc)
            {
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            al5c.MotionTime = (ushort)numericUpDown1.Value;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            checkBox2.Checked = al5c.IsMovementFinished();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // Save current motion time
            ushort mTime = al5c.MotionTime;
            // Do the action on 2seconds
            al5c.MotionTime = 2000;
            // Go to idle/parking position
            al5c.GoToIdlePosition(true);
            al5c.updateServos();
            // Revert the motion time back
            al5c.MotionTime = mTime;
        }
    }
}
