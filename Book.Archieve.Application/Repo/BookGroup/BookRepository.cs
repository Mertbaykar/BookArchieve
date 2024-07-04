using Book.Archieve.Application.Definition;
using Book.Archieve.Domain.DTO.Request.Book;
using Book.Archieve.Domain.DTO.Response.Book;
using Book.Archieve.Domain.Entity;
using Book.Archieve.Infrastructure.Context;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Book.Archieve.Application.Repo.BookGroup
{
    public class BookRepository : RepositoryBase, IBookRepository
    {

        private readonly IValidator<Domain.Entity.Book> bookValidator;
        private readonly IValidator<Domain.Entity.Author> authorValidator;

        public BookRepository(BookContext bookContext, IValidator<Domain.Entity.Book> bookValidator, IValidator<Author> authorValidator) : base(bookContext)
        {
            this.bookValidator = bookValidator;
            this.authorValidator = authorValidator;
        }

        private void ValidateByDB(Domain.Entity.Book book)
        {
            if (MasterDataDb.Book.Any(u => u.Name == book.Name.ToLower()))
                throw new Exception("Kitap zaten tanımlanmış");
        }

        private void ValidateAuthorByDB(Domain.Entity.Author author)
        {
            if (MasterDataDb.Author.Any(u => u.FirstName == author.FirstName.ToLower() || u.LastName == author.LastName.ToLower() && u.IsActive))
                throw new Exception("Yazar zaten tanımlanmış");
        }

        public Domain.Entity.Book Create(CreateBookRequest createBookRequest, int userId)
        {

            // Dosya kontrolü
            if (createBookRequest.CoverImageFile == null || createBookRequest.CoverImageFile.Length == 0)
                throw new Exception("Görsel yükleyiniz");

            if (createBookRequest.CoverImageFile.Length > FileDefinition.MaxFileSize)
                throw new Exception("Dosya boyutu 5 MB'ı geçemez");

            string fileExtension = Path.GetExtension(createBookRequest.CoverImageFile.FileName);

            // Dosya format kontrolü
            if (!FileDefinition.ImageFileExtensions.Contains(fileExtension))
                throw new Exception("Uygun formatta görsel yükleyiniz");

            Author? author = MasterDataDb.Author
                  .Where(a => a.Id == createBookRequest.AuthorId && a.IsActive).FirstOrDefault();
            if (author == null)
                throw new Exception("Yazar bulunamadı");

            // Dosyayı kaydetmek için klasör yolunu belirleyin
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), FileDefinition.BookUploadPath).Replace("bin\\Debug\\net8.0\\", "");

            // Klasörün var olup olmadığını kontrol edin, yoksa oluşturun
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = createBookRequest.Name + "_" + Guid.NewGuid() + fileExtension;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                createBookRequest.CoverImageFile.CopyTo(fileStream);
            }

            createBookRequest.CreatedById = userId;
            createBookRequest.CoverImagePath = Path.Combine(FileDefinition.BookUploadPath, uniqueFileName);

            Domain.Entity.Book book = Domain.Entity.Book.Create(createBookRequest);
            base.Validate(book, bookValidator);
            ValidateByDB(book);
            return base.Add(book);
        }

        public Author CreateAuthor(CreateAuthorRequest createAuthorRequest)
        {
            Domain.Entity.Author author = Domain.Entity.Author.Create(createAuthorRequest);
            base.Validate(author, authorValidator);
            ValidateAuthorByDB(author);
            return base.Add(author);
        }

        public Domain.Entity.Book Update(UpdateBookRequest updateBookRequest, int userId)
        {
            var book = MasterDataDb.Book.Where(b => b.Id == updateBookRequest.Id && b.IsActive && b.CreatedById == userId).FirstOrDefault();
            if (book == null)
                throw new Exception("Kitap bulunamadı");

            book.Update(updateBookRequest.Name, updateBookRequest.Summary, updateBookRequest.PublishYear, updateBookRequest.ShelfLocation);
            base.Validate(book, bookValidator);

            Author? author = MasterDataDb.Author.Where(a => a.Id == updateBookRequest.AuthorId && a.IsActive).FirstOrDefault();
            if (author == null)
                throw new Exception("Yazar bulunamadı");

            if (MasterDataDb.Book.Any(b => b.Id != updateBookRequest.Id && (b.IsActive && b.Name.ToLower() == updateBookRequest.Name.ToLower() || b.ShelfLocation.ToLower() == updateBookRequest.ShelfLocation.ToLower())))
                throw new Exception("Bu kitap zaten var");

            if (updateBookRequest.CoverImageFile != null)
            {
                if (updateBookRequest.CoverImageFile.Length > FileDefinition.MaxFileSize)
                    throw new Exception("Dosya boyutu 5 MB'ı geçemez");

                string fileExtension = Path.GetExtension(updateBookRequest.CoverImageFile.FileName);

                // Dosya format kontrolü
                if (!FileDefinition.ImageFileExtensions.Contains(fileExtension))
                    throw new Exception("Uygun formatta görsel yükleyiniz");

                // Dosyayı kaydetmek için klasör yolunu belirleyin
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), FileDefinition.BookUploadPath).Replace("bin\\Debug\\net8.0\\", "");

                // Klasörün var olup olmadığını kontrol edin, yoksa oluşturun
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string rawFileName = Path.GetFileNameWithoutExtension(updateBookRequest.CoverImageFile.FileName);

                string uniqueFileName = rawFileName + "_" + Guid.NewGuid() + fileExtension;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    updateBookRequest.CoverImageFile.CopyTo(fileStream);
                }

                book.ChangeCoverImage(Path.Combine(FileDefinition.BookUploadPath, uniqueFileName));
            }

            base.Save();
            return book;
        }

        public void Delete(int id, int userId)
        {
            var book = MasterDataDb.Book.Where(x => x.Id == id && x.IsActive && x.CreatedById == userId).FirstOrDefault();
            if (book == null)
                throw new Exception("Kitap bulunamadı");
            base.Delete(book);
        }

        public IEnumerable<ReadBookResponse> Search(SearchBookRequest searchBookRequest, int userId)
        {
            IQueryable<Domain.Entity.Book>? query = MasterDataDb.Book;

            if (searchBookRequest.CreatedById.HasValue)
                query = query.Where(b => b.CreatedById == searchBookRequest.CreatedById.Value);

            if (searchBookRequest.AuthorId.HasValue)
                query = query.Where(b => b.AuthorId == searchBookRequest.AuthorId.Value);

            if (searchBookRequest.PublishYear.HasValue)
                query = query.Where(b => b.PublishYear == searchBookRequest.PublishYear.Value);

            if (!string.IsNullOrWhiteSpace(searchBookRequest.Text))
            {
                query = query.Where(b =>
                b.Name.ToLower().Contains(searchBookRequest.Text)
                || b.Summary.ToLower().Contains(searchBookRequest.Text)
                || b.ShelfLocation.ToLower().Contains(searchBookRequest.Text)
                || (b.CreatedBy.FirstName.ToLower().Contains(searchBookRequest.Text)
                || b.CreatedBy.LastName.ToLower().Contains(searchBookRequest.Text))
                || (b.Author.FirstName.ToLower().Contains(searchBookRequest.Text)
                || b.Author.LastName.ToLower().Contains(searchBookRequest.Text)));
            }

            return query.Select(x => new ReadBookResponse
            {
                AuthorId = x.AuthorId,
                AuthorName = x.Author.FirstName + " " + x.Author.LastName,
                CoverImagePath = x.CoverImagePath,
                CreatedById = x.CreatedById,
                CreatedByName = x.CreatedBy.FirstName + " " + x.CreatedBy.LastName,
                CreatedDate = x.CreatedDate,
                Name = x.Name,
                PublishYear = x.PublishYear,
                ShelfLocation = x.ShelfLocation,
                Summary = x.Summary,
                Notes = x.Notes.Where(y => y.IsActive)
                           .Where(note =>
                            note.UserId == userId
                            ||
                            (note.IsShared &&
                            (
                            (note.User.ShareId == (int)NoteShareSetting.FriendsOnly &&
                                // aralarında arkadaşlık var mı
 MasterDataDb.FriendShip.Any(f => (f.UserId == note.UserId && f.FriendId == userId) || (f.UserId == userId && f.FriendId == note.UserId) && f.User.IsActive && f.Friend.IsActive)
 ) 
 || (note.User.ShareId == (int)NoteShareSetting.Public))
                                     )
                                     )
                                .Select(note => new ReadNoteResponse
                                {
                                    CreatedDate = note.CreatedDate,
                                    ShareId = note.User.ShareId,
                                    IsShared = note.IsShared,
                                    Text = note.Text,
                                    UserId = userId,
                                    UserName = note.User.FirstName + " " + note.User.LastName,
                                })
                                .ToList()
            }).ToList();
        }
    }

    public interface IBookRepository
    {
        Domain.Entity.Book Create(CreateBookRequest createBookRequest, int userId);
        Domain.Entity.Book Update(UpdateBookRequest updateBookRequest, int userId);
        void Delete(int id, int userId);
        Domain.Entity.Author CreateAuthor(CreateAuthorRequest createAuthorRequest);
        IEnumerable<ReadBookResponse> Search(SearchBookRequest searchBookRequest, int userId);

    }
}
