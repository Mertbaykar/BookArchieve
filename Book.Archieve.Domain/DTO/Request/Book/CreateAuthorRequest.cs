
namespace Book.Archieve.Domain.DTO.Request.Book
{
    public class CreateAuthorRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => FirstName + " " + LastName;
    }
}
