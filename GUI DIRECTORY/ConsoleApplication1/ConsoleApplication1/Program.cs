using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            /*string format = "000";
            int test = 9000;
            Console.WriteLine(test.ToString(format));
            Console.Read();*/

            string test = "ARM_WR_U:050_R:100_L:000";
            int testInt = -999;
            if (int.TryParse(test.Substring(9, 3), out testInt))
            {
                
            }
            Console.WriteLine("RESULT: " + testInt);
            Console.Read();
        }
    }
}
