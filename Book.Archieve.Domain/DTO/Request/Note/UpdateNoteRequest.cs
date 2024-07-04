
namespace Book.Archieve.Domain.DTO.Request
{
    public class UpdateNoteRequest
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsShared { get; set; }
    }
}
