using Microsoft.AspNetCore.Http;

namespace Book.Archieve.Domain.DTO.Request.Book
{
    public class UpdateBookRequest
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        /// <summary>
        /// Raf yeri bilgisi
        /// </summary>
        public string ShelfLocation { get; set; }
        public int PublishYear { get; set; }
        public IFormFile? CoverImageFile { get; set; }
        public int AuthorId { get; set; }
    }
}
