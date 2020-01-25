using DataAccess;
using ModelEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDataItemManager
{
    class BizFileRepository
    {
        public FileRepositoryDTO ToFileRepository(FileRepository fileRepository)
        {
            var result = new FileRepositoryDTO();
            result.FileName = fileRepository.FileName;
            result.FileExtension = fileRepository.FileExtention.Replace(".", "");
            result.Content = fileRepository.Content;
            return result;
        }
        public FileRepository ToFileRepository(MyIdeaDataDBEntities letterModel, FileRepositoryDTO attechedFile)
        {
            //if (attechedFile.ID != 0)
            //    throw new Exception("file id is not zero");
            FileRepository fileRepository = null;
            if (attechedFile.ID == 0)
                fileRepository = new FileRepository();
            else
                fileRepository = letterModel.FileRepository.First(x => x.ID == attechedFile.ID);
            fileRepository.FileName = attechedFile.FileName;
            fileRepository.FileExtention = attechedFile.FileExtension.Replace(".", "");
            fileRepository.Content = attechedFile.Content;
            if (attechedFile.ID == 0)
                letterModel.FileRepository.Add(fileRepository);
            return fileRepository;
        }
    }
}
