/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using TaLib.Extension;

namespace Simulation.Shared
{
    public interface IIndicatorsRegistry
    {
        void AddSerie(string name, TaResult serie, bool normalize);
    }
}