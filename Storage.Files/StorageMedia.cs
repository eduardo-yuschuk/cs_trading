/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using Storage.Shared;
using Configuration.Shared;
using System.IO;

namespace Storage.Files
{
    public class StorageMedia : IStorageMedia
    {
        private string _rootFolder;

        public StorageMedia()
        {
            _rootFolder = ConfigurationFactory.Configuration["FileStorageMediaRootFolder"];
        }

        public void Save(IStorageContext context, InformationUnitId informationUnit, IStorableData data)
        {
            string folder = string.Format(@"{0}\{1}\", _rootFolder, context.Name);
            Directory.CreateDirectory(folder);
            string path = string.Format(@"{0}\{1}", folder, informationUnit.Name);
            File.WriteAllBytes(path, data.GetBytes());
        }

        public IStorableData Read(IStorageContext context, InformationUnitId informationUnit)
        {
            string path = string.Format(@"{0}\{1}\{2}", _rootFolder, context.Name, informationUnit.Name);
            return new RawBytesStorableData(File.ReadAllBytes(path));
        }
    }
}