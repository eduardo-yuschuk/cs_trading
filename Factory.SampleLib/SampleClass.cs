/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using Factory.SampleLib.Shared;

namespace Factory.SampleLib
{
    public class SampleClass : ISampleInterface
    {
        public int SampleMethod(int a, int b)
        {
            return a + b;
        }
    }
}