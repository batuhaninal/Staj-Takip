using Core.Utilities.Results;
using StajTakip.Entities.ComplexTypes;
using StajTakip.Entities.DTOs;
using StajTakip.MVC.Helpers.Abstract;

namespace StajTakip.MVC.Helpers.Concrete
{
    public class FIleHelper : IFileHelper
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _wwwroot;
        private readonly string _fileFolder = "files";
        private readonly string _imageFolder = "images";
        private const string internBookPicture = "internBooks";

        public FIleHelper(IWebHostEnvironment env)
        {
            _env = env;
            _wwwroot = env.WebRootPath;
        }

        public IDataResult<ImageDeletedDto> Delete(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<ImageUploadDto>> UploadAsync(string name, IFormFile formFile, FileType fileType, string folderName = null)
        {
            throw new NotImplementedException();
        }
    }
}
