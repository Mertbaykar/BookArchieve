using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Archieve.Domain.DTO.Request.Book
{
    public class SearchBookRequest
    {
        public string? Text { get; set; }
        public int? AuthorId { get; set; }
        public int? CreatedById { get; set; }
        public int? PublishYear { get; set; }
    }
}
