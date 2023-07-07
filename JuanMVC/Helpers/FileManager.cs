namespace JuanMVC.Helpers
{
    public class FileManager
    {
        public static string Save(IFormFile file, string rootPath, string folder)
        {
            var newPath = Guid.NewGuid().ToString() + (file.FileName.Length <= 64 ? file.FileName : file.FileName.Substring(file.FileName.Length - 64));

            var path = Path.Combine(rootPath, folder, newPath);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return newPath;

        }

        public static void Delete(string rootPath , string folder,string fileNAme)
        {
            var path = Path.Combine(rootPath, folder, fileNAme);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

        }


    }
}
