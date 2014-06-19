using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            string test = "G:80.9,SH:50,E:45";
            string[] output = test.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in output)
            {
                Console.WriteLine(double.Parse(s.Substring(s.LastIndexOf(":")+1))+100.5);
            }
            Console.Read();
        }
    }
}
