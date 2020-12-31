/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;

namespace SeriesVerification.Automation.Program
{
    class Program
    {
        static void Main(string[] args)
        {
            Robot robot = new Robot();
            robot.Start();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}