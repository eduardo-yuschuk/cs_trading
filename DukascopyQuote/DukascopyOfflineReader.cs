/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace DukascopyQuote
{
    /// <summary>
    /// Lector de archivos de texto.
    /// </summary>
    public class DukascopyOfflineReader
    {
        /// <summary>
        /// Retorna una lista con todas las líneas de texto contenidas en el archivo cuyo path se recibe por parámetro.
        /// </summary>
        /// <param name="filepath">El path del archivo a leer.</param>
        /// <returns>Una lista con las líneas de texto contenidas en el archivo.</returns>
        public static List<string> GetHistoricalPrices(string filepath)
        {
            return File.ReadAllLines(filepath).ToList();
        }

        /// <summary>
        /// Invoca la función por cada línea de texto encontrada en el archivo.
        /// </summary>
        /// <param name="filepath">El path del archivo a leer.</param>
        /// <param name="func">La función a ejecutar con cada línea.</param>
        public static void GetHistoricalPrices(string filepath, Func<string, bool> func)
        {
            using (StreamReader reader = File.OpenText(filepath))
            {
                // skip header
                string line = reader.ReadLine();
                line = reader.ReadLine();
                while (line != null && func(line))
                {
                }
            }
        }
    }
}