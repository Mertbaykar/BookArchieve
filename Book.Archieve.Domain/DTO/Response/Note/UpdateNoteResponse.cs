
namespace Book.Archieve.Domain.DTO.Response.Note
{
    public class UpdateNoteResponse
    {
        public string Text { get; set; }
        public bool IsShared { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }

        public int BookId { get; set; }
        public string BookName { get; set; }
    }
}
