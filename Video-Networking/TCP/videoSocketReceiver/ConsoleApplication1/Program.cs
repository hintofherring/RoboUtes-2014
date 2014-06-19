using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1 {
    class Program {
        static void Main(string[] args) {
            string test = "yyyyySIZE:1234567890zyyyyyz";
            byte[] receiveBytes = Encoding.UTF8.GetBytes(test);
            List<byte[]> bigBuffer = new List<byte[]>();
            bigBuffer.Add(receiveBytes.Skip(9 + 11).Take(receiveBytes.Length - 9 + 11).ToArray());
            //Console.WriteLine(test.Substring(test.LastIndexOf("S")+1));
            Console.WriteLine(Encoding.UTF8.GetString(bigBuffer[0]));
            Console.Read();
        }
    }
}
