using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface
{
    public class IngestionDataSelection
    {
        #region Folders

        public string RootPath { get; }
        public string AssetFolder { get; }
        public string ProviderFolder { get; }

        #endregion

        #region Data

        public string Asset => AssetFolder;
        public string Provider => ProviderFolder;

        #endregion

        public IngestionDataSelection(string rootPath, string assetFolder, string providerFolder)
        {
            RootPath = rootPath;
            AssetFolder = assetFolder;
            ProviderFolder = providerFolder;
        }
    }
}
