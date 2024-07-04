using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Archieve.Application.Definition
{
    public class FileDefinition
    {
        // Dosya boyut kontrolü (5 MB)
        public static readonly long MaxFileSize = 5 * 1024 * 1024;
        public static readonly string BookUploadPath = Path.Combine("Book", "CoverImage");

        public static readonly List<string> ImageFileExtensions = [
            ".jpg",
            ".jpeg",
            ".png",
            ];
    }
}
