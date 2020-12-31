using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface
{
    public class ByYearDataSelection
    {
        #region Folders

        public string RootPath { get; }
        public string AssetFolder { get; }
        public string ProviderFolder { get; }
        public string YearFolder { get; }

        #endregion

        #region Data

        public string Asset => AssetFolder;
        public string Provider => ProviderFolder;
        public int Year => int.Parse(YearFolder);

        #endregion

        public ByYearDataSelection(string rootPath, string assetFolder, string providerFolder, string yearFolder)
        {
            RootPath = rootPath;
            AssetFolder = assetFolder;
            ProviderFolder = providerFolder;
            YearFolder = yearFolder;
        }
    }
}