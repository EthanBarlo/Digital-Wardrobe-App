using System;
using System.IO;
using Android.Graphics;
using MyDigitalWardrobe.Droid;
using MyDigitalWardrobe.Interfaces;
using MyDigitalWardrobe.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileService))]
namespace MyDigitalWardrobe.Droid
{
    // Code can be found here, its been modified slightly to allow for different users to have their own folders.
    // https://stackoverflow.com/questions/51038251/how-to-download-image-and-save-it-in-local-storage-using-xamarin-forms
    public class FileService : IFileService
    {
        public void SavePicture(string name, Stream data, string location)
        {
            // Creates the directorys for the image to be stored in
            // We are using the Current Users Email to create a unique folder, this way if someone were to switch users the images will still exist but they cant see them.
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            documentsPath = System.IO.Path.Combine(documentsPath, FireBaseService.CurrentUserInformation.User.Email, location);
            Directory.CreateDirectory(documentsPath);

            string filePath = System.IO.Path.Combine(documentsPath, name);

            byte[] bArray = new byte[data.Length];
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using (data)
                {
                    data.Read(bArray, 0, (int)data.Length);
                }
                int length = bArray.Length;
                fs.Write(bArray, 0, length);
            }
        }
        public string GetImagePath(string location = "temp", int? itemIdent = null, string name = null)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            path = System.IO.Path.Combine(path, FireBaseService.CurrentUserInformation.User.Email, location);
            if (itemIdent != null)
                path = System.IO.Path.Combine(path, itemIdent.ToString());
            if (name != null)
                path = System.IO.Path.Combine(path, $"{name}.jpg");
            return path;
        }
        public bool MoveImageToStore(string source, string destination, string imageName)
        {
            try
            {
                Directory.CreateDirectory(destination);
                destination = System.IO.Path.Combine(destination, imageName);
                // Image Compression using Android.Graphics
                using (FileStream fs1 = File.OpenRead(source))
                {
                    using var bitmap = BitmapFactory.DecodeStream(fs1);
                    using FileStream fs2 = new FileStream(destination, FileMode.OpenOrCreate);
                    bitmap.Compress(Bitmap.CompressFormat.Jpeg, 30, fs2);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}