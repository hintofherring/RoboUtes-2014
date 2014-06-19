using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XboxController;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace UsingLibrary {
    class Program {
        static void Main(string[] args) {
            Program p = new Program();

            Console.Read();
        }

        public Program() {
            XboxController.XboxController controller = new XboxController.XboxController();

            // Buttons
            controller.ButtonAPressed += ButtonAPressed;
            controller.ButtonAReleased += ButtonAReleased;
            controller.ButtonBPressed += ButtonBPressed;
            controller.ButtonBReleased += ButtonBReleased;
            controller.ButtonXPressed += ButtonXPressed;
            controller.ButtonXReleased += ButtonXReleased;
            controller.ButtonYPressed += ButtonYPressed;
            controller.ButtonYReleased += ButtonYReleased;
            controller.ButtonStartPressed += ButtonStartPressed;
            controller.ButtonStartReleased += ButtonStartReleased;
            controller.ButtonBackPressed += ButtonBackPressed;
            controller.ButtonBackReleased += ButtonBackReleased;
            controller.ButtonLeftShoulderPressed += ButtonLeftShoulderPressed;
            controller.ButtonLeftShoulderReleased += ButtonLeftShoulderReleased;
            controller.ButtonRightShoulderPressed += ButtonRightShoulderPressed;
            controller.ButtonRightShoulderReleased += ButtonRightShoulderReleased;
            controller.ThumbStickLeftPressed += ThumbStickLeftPressed;
            controller.ThumbStickLeftReleased += ThumbStickLeftReleased;
            controller.ThumbStickRightPressed += ThumbStickRightPressed;
            controller.ThumbStickRightReleased += ThumbStickRightReleased;

            // DPad
            controller.DPadUpPressed += DPadUpPressed;
            controller.DPadUpReleased += DPadUpReleased;
            controller.DPadDownPressed += DPadDownPressed;
            controller.DPadDownReleased += DPadDownReleased;
            controller.DPadLeftPressed += DPadLeftPressed;
            controller.DPadLeftReleased += DPadLeftReleased;
            controller.DPadRightPressed += DPadRightPressed;
            controller.DPadRightReleased += DPadRightReleased;

            // Triggers
            controller.TriggerLeft += TriggerLeft;
            controller.TriggerRight += TriggerRight;

            // ThumbSticks
            controller.ThumbStickLeft += ThumbStickLeft;
            controller.ThumbStickRight += ThumbStickRight;
        }


        /************************ BUTTONS *************************/


        public void ButtonAPressed(object sender, EventArgs e) {
            Console.WriteLine("Button A pressed");
        }
        public void ButtonAReleased(object sender, EventArgs e) {
            Console.WriteLine("Button A released");
        }

        public void ButtonBPressed(object sender, EventArgs e) {
            Console.WriteLine("Button B pressed");
        }
        public void ButtonBReleased(object sender, EventArgs e) {
            Console.WriteLine("Button B released");
        }

        public void ButtonXPressed(object sender, EventArgs e) {
            Console.WriteLine("Button X pressed");
        }
        public void ButtonXReleased(object sender, EventArgs e) {
            Console.WriteLine("Button X released");
        }

        public void ButtonYPressed(object sender, EventArgs e) {
            Console.WriteLine("Button Y pressed");
        }
        public void ButtonYReleased(object sender, EventArgs e) {
            Console.WriteLine("Button Y released");
        }

        public void ButtonStartPressed(object sender, EventArgs e) {
            Console.WriteLine("Button Start pressed");
        }
        public void ButtonStartReleased(object sender, EventArgs e) {
            Console.WriteLine("Button Start released");
        }

        public void ButtonBackPressed(object sender, EventArgs e) {
            Console.WriteLine("Button Back pressed");
        }
        public void ButtonBackReleased(object sender, EventArgs e) {
            Console.WriteLine("Button Back released");
        }

        public void ButtonLeftShoulderPressed(object sender, EventArgs e) {
            Console.WriteLine("Left shoulder pressed");
        }
        public void ButtonLeftShoulderReleased(object sender, EventArgs e) {
            Console.WriteLine("Left shoulder released");
        }

        public void ButtonRightShoulderPressed(object sender, EventArgs e) {
            Console.WriteLine("Right shoulder pressed");
        }
        public void ButtonRightShoulderReleased(object sender, EventArgs e) {
            Console.WriteLine("Right shoulder released");
        }

        public void ThumbStickLeftPressed(object sender, EventArgs e) {
            Console.WriteLine("Left ThumbStickLeft pressed");
        }
        public void ThumbStickLeftReleased(object sender, EventArgs e) {
            Console.WriteLine("Left ThumbStickLeft released");
        }

        public void ThumbStickRightPressed(object sender, EventArgs e) {
            Console.WriteLine("Right ThumbStickLeft pressed");
        }
        public void ThumbStickRightReleased(object sender, EventArgs e) {
            Console.WriteLine("Right ThumbStickLeft released");
        }


        /*********************** DPAD ******************/

        public void DPadUpPressed(object sender, EventArgs e) {
            Console.WriteLine("DPad Up pressed");
        }
        public void DPadUpReleased(object sender, EventArgs e) {
            Console.WriteLine("DPad Up released");
        }

        public void DPadDownPressed(object sender, EventArgs e) {
            Console.WriteLine("DPad Down pressed");
        }
        public void DPadDownReleased(object sender, EventArgs e) {
            Console.WriteLine("DPad Down released");
        }

        public void DPadLeftPressed(object sender, EventArgs e) {
            Console.WriteLine("DPad Left pressed");
        }
        public void DPadLeftReleased(object sender, EventArgs e) {
            Console.WriteLine("DPad Left released");
        }

        public void DPadRightPressed(object sender, EventArgs e) {
            Console.WriteLine("DPad Right pressed");
        }
        public void DPadRightReleased(object sender, EventArgs e) {
            Console.WriteLine("DPad Right released");
        }


        /*********************** TRIGGERS *****************/


        public void TriggerLeft(object sender, EventArgs e) {
            XboxEventArgs args = (XboxEventArgs)e;
            Console.WriteLine(RoundFloat(args.GetTriggerLeft()));
        }

        public void TriggerRight(object sender, EventArgs e) {
            XboxEventArgs args = (XboxEventArgs)e;
            Console.WriteLine(RoundFloat(args.GetTriggerRight()));
        }


        /************************ THUMBSTICKS ***********************/


        public void ThumbStickLeft(object sender, EventArgs e) {
            XboxEventArgs args = (XboxEventArgs)e;
            Tuple<float, float> vector = args.GetThumbStickLeft();
            Console.WriteLine("X = " + vector.Item1);
            Console.WriteLine("Y = " + vector.Item2);
        }

        public void ThumbStickRight(object sender, EventArgs e) {
            XboxEventArgs args = (XboxEventArgs)e;
            Tuple<float, float> vector = args.GetThumbStickRight();
            Console.WriteLine("X = " + vector.Item1);
            Console.WriteLine("Y = " + vector.Item2);
        }


        /// <summary>
        /// Used to truncate a float to one decimal place.
        /// </summary>
        private float RoundFloat(float f) {
            int accuracy = 1000;
            int i = (int)(f * accuracy);
            f = i;
            return f / accuracy;
        }
    }
}
