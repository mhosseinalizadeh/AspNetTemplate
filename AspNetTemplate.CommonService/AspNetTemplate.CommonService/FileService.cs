using AspNetTemplate.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetTemplate.CommonService
{
    public class FileService : IFileService
    {
        private FileServiceData _fileServiceData;
        public FileService(FileServiceData fileServiceData)
        {
            _fileServiceData = fileServiceData;
        }

        public string GetNewExpenseFilePath()
        {
            return  _fileServiceData.ExpensePhotoPath + Guid.NewGuid().ToString();
        }
    }
}
