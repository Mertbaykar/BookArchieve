using Book.Archieve.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Book.Archieve.Domain.DTO.Response.Book
{
    public class ReadBookResponse
    {
        public string Name { get;  set; }
        public string Summary { get;  set; }
        public int PublishYear { get;  set; }
        /// <summary>
        /// Raf yeri bilgisi
        /// </summary>
        public string ShelfLocation { get;  set; }
        public DateTime CreatedDate { get;  set; }
        /// <summary>
        /// Server Path, canlıda network paylaşımlı bir IIS dizini veya azure blob storage tarzı bir şey kullanılabilir. Gösterim amaçlı server'a kaydediliyor
        /// </summary>
        public string CoverImagePath { get;  set; }

        public int AuthorId { get;  set; }
        public string AuthorName { get;  set; }

        public int CreatedById { get;  set; }
        public string CreatedByName { get;  set; }

        public List<ReadNoteResponse> Notes { get; set; } = new();
    }

    public class ReadNoteResponse
    {
        public string Text { get; set; }
        public bool IsShared { get; set; }
        public int ShareId { get; set; }
        //[JsonIgnore]
        //public NoteShareSetting ShareSetting => Enum.GetValues<NoteShareSetting>().First(x => (int)x == ShareId);
        public DateTime CreatedDate { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
