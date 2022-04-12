using System.IO;

namespace MyDigitalWardrobe.Interfaces
{
    public interface IFileService
    {
        void SavePicture(string name, Stream data, string location = "temp");
        string GetImagePath(string location = "temp", int? itemIdent = null, string name = null);
        bool MoveImageToStore(string source, string destination, string imageName);
    }
}
