/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;

namespace FinancialConfiguration.Program
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"DataRoot: {Configuration.Instace.DataRoot}");
        }
    }
}