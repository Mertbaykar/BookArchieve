
namespace Book.Archieve.Domain.DTO.Request.Note
{
    public class CreateNoteRequest
    {
        public string Text { get; set; }
        public bool IsShared { get; set; }
        public int BookId { get; set; }
    }
}
