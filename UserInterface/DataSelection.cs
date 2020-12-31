/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System.Collections.Generic;
using System.Linq;

namespace UserInterface
{
    public class DataSelection
    {
        #region Folders

        public string RootPath { get; }
        public string AssetFolder { get; }
        public string ProviderFolder { get; }
        public string YearFolder { get; }
        public string MonthFolder { get; }
        public List<string> DayFolders { get; }

        #endregion

        #region Data

        public string Asset => AssetFolder;
        public string Provider => ProviderFolder;
        public int Year => int.Parse(YearFolder);
        public int Month => int.Parse(MonthFolder);
        public List<int> Days => DayFolders.Select(day => int.Parse(day.Replace(".bin", ""))).ToList();

        #endregion

        public DataSelection(string rootPath, string assetFolder, string providerFolder, string yearFolder,
            string monthFolder, List<string> dayFolders)
        {
            RootPath = rootPath;
            AssetFolder = assetFolder;
            ProviderFolder = providerFolder;
            YearFolder = yearFolder;
            MonthFolder = monthFolder;
            DayFolders = dayFolders;
        }

        public List<string> FilePaths
        {
            get
            {
                var rootFolder = $@"{RootPath}\{AssetFolder}\{ProviderFolder}\{YearFolder}\{MonthFolder}\";
                return DayFolders.Select(day => rootFolder + day).ToList();
            }
        }
    }
}