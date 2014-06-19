using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ArmControlTools;

namespace newMacros
{
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("newMacros", true)]
    public partial class ToolboxControl : UserControl
    {

        public delegate void newMacroRequest(armCommand[] newTargets);
        public event newMacroRequest newMacroData;

        public ToolboxControl()
        {
            InitializeComponent();
        }

        public class armCommand
        {
            public double target;
            public armConstants.armActuatorID ID;

            /// <summary>
            /// Represent the movement of a SINGLE actuator. timeout is how long the motion is expected to take (plus some) before the motion should be finished. If it isnt the motion should "fail" as something likely went wrong.
            /// </summary>
            /// <param name="timeout"></param>
            /// <param name="jointVal"></param>
            /// <param name="actuatorID"></param>
            public armCommand( double jointVal, armConstants.armActuatorID actuatorID)
            {
                target = jointVal;
                ID = actuatorID;
            }
        }

        private void buttonClick(object sender, RoutedEventArgs e)
        {
            string[] info = ((Button)sender).Uid.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
            List<armCommand> commands = new List<armCommand>();
            foreach (string potentialCommand in info)
            {
                double val;
                if (double.TryParse(potentialCommand.Substring(potentialCommand.LastIndexOf(":") + 1) , out val))
                {
                    if (potentialCommand.StartsWith("SH:"))
                    {
                        commands.Add(new armCommand(val, armConstants.armActuatorID.shoulder));
                    }
                    else if (potentialCommand.StartsWith("G:"))
                    {
                        commands.Add(new armCommand(val, armConstants.armActuatorID.grip));
                    }
                    else if (potentialCommand.StartsWith("E:"))
                    {
                        commands.Add(new armCommand(val, armConstants.armActuatorID.elbow));
                    }
                    else if (potentialCommand.StartsWith("TT:"))
                    {
                        commands.Add(new armCommand(val, armConstants.armActuatorID.turnTable));
                    }
                }
            }
            if (commands.Count > 0 && newMacroData != null)
            {
                newMacroData(commands.ToArray());
            }
        }
    }
}
