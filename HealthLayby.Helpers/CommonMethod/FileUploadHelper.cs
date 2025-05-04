using Microsoft.AspNetCore.Http;

namespace HealthLayby.Helpers.CommonMethod
{
    /// <summary>
    /// FileUploadHelper
    /// </summary>
    public static class FileUploadHelper
    {
        /// <summary>
        /// Uploads the file.
        /// </summary>
        /// <param name="base64">The base64.</param>
        /// <param name="extension">The extension.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string UploadFile(string base64, string extension, string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var fileName = Guid.NewGuid().ToString();

                while (true)
                {
                    if (File.Exists(Path.Combine(path, $"{fileName}{extension}")))
                    {
                        fileName = Guid.NewGuid().ToString();
                    }
                    else
                    {
                        break;
                    }
                }

                File.WriteAllBytes(Path.Combine(path, $"{fileName}.{extension}"), Convert.FromBase64String(base64));

                return $"{fileName}.{extension}";
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Uploads the file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="extension">The extension.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string UploadFile(IFormFile file, string extension, string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var fileName = Guid.NewGuid().ToString();
                string filePath = Path.Combine(path, $"{fileName}{extension}");

                while (true)
                {
                    if (File.Exists(filePath))
                    {
                        fileName = Guid.NewGuid().ToString();
                    }
                    else
                    {
                        break;
                    }
                }

                if (file.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                return $"{fileName}{extension}";
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="path">The path.</param>
        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
