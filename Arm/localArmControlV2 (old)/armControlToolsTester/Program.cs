using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ArmControlTools;
using XboxController;

namespace armControlToolsTester
{
    class Program
    {
        static void Main(string[] args)
        {
            XboxController.XboxController xboxController = new XboxController.XboxController();
            armInputManager testManager = armInputManager.getInstance(xboxController);
            testManager.targetElbowChanged+=testManager_targetElbowChanged;
            testManager.targetShoulderChanged += testManager_targetShoulderChanged;
            testManager.targetTurnTableChanged += testManager_targetTurnTableChanged;
        }

        static void testManager_targetTurnTableChanged(double newAngle)
        {
            Console.WriteLine("TurnTable Updated: " + newAngle);
        }

        static void testManager_targetShoulderChanged(double newAngle)
        {
            Console.WriteLine("Shoulder Updated: " + newAngle);
        }

        private static void testManager_targetElbowChanged(double newAngle)
        {
            Console.WriteLine("Elbow Updated: " + newAngle);
        }
    }
}
