using Core.Utilities.Results;
using StajTakip.Entities.ComplexTypes;
using StajTakip.Entities.DTOs;

namespace StajTakip.MVC.Helpers.Abstract
{
    public interface IFileHelper
    {
        /// <summary>
        /// Resim veya dosya yuklemek icin kullanilabilir.
        /// </summary>
        /// <param name="name">
        ///     Dosya ismi
        /// </param>
        /// <param name="formFile">
        ///     Client uzerinden alinan dosya
        /// </param>
        /// <param name="fileType">
        ///     Dosya tipi (0 = Image, 1 = File)
        /// </param>
        /// <param name="folderName">
        ///     Icinde bulundugu dosya ismi
        /// </param>
        /// <returns></returns>
        Task<IDataResult<ImageUploadDto>> UploadAsync(string name, IFormFile formFile, FileType fileType, string folderName = null);
        IDataResult<ImageDeletedDto> Delete(string fileName);
    }
}
