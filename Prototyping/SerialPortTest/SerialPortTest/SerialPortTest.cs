using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace SerialPortTest
{
    class SerialPortTest
    {
        static void Main(string[] args)
        {
            SerialPort Port = new SerialPort("COM1");

            //Port.Writetim

            Console.WriteLine("Available Ports:");
            foreach (string s in SerialPort.GetPortNames())
            {
                Console.WriteLine("   {0}", s);
            }
//            Port.PortName = "COM1";
            Port.Open();

            while (true)
            {
                Console.WriteLine("aus");
                Port.RtsEnable = false;
                //Port.DtrEnable = false;
                Console.ReadLine();
                Console.WriteLine("an");
                Port.RtsEnable = true;
                //Port.DtrEnable = true;
                Console.ReadLine();
            }
            //Port.DtrEnable = true;

            Console.ReadLine();
        }
    }
}
