using Book.Archieve.Application.Repo.BookGroup;
using Book.Archieve.Application.Repo.UserGroup;
using Book.Archieve.Domain.DTO.Request;
using Book.Archieve.Domain.DTO.Request.Book;
using Book.Archieve.Domain.DTO.Request.Note;
using Book.Archieve.Domain.DTO.Response;
using Book.Archieve.Domain.DTO.Response.Book;
using Book.Archieve.Domain.DTO.Response.Note;
using Book.Archieve.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Archieve.Application.UnitOfWork
{
    public class BookUnitOfWork : IBookUnitOfWork
    {
        public IBookRepository BookRepository { get; private set; }
        public INoteRepository NoteRepository { get; private set; }

        public BookUnitOfWork(IBookRepository userRepository, INoteRepository noteRepository)
        {
            BookRepository = userRepository;
            NoteRepository = noteRepository;
        }

        public CreateBookResponse Create(CreateBookRequest createBookRequest, int userId)
        {
            var book = BookRepository.Create(createBookRequest, userId);
            return new CreateBookResponse
            {
                AuthorId = book.AuthorId,
                CoverImagePath = book.CoverImagePath,
                CreatedById = book.CreatedById,
                CreatedDate = book.CreatedDate,
                Name = book.Name,
                PublishYear = book.PublishYear,
                Summary = book.Summary,
                ShelfLocation = book.ShelfLocation,
            };
        }

        public Author CreateAuthor(CreateAuthorRequest createAuthorRequest)
        {
            return BookRepository.CreateAuthor(createAuthorRequest);
        }

        public UpdateBookResponse Update(UpdateBookRequest updateBookRequest, int userId)
        {
            var book = BookRepository.Update(updateBookRequest, userId);
            return new UpdateBookResponse
            {
                AuthorId = book.AuthorId,
                CoverImagePath = book.CoverImagePath,
                CreatedById = book.CreatedById,
                CreatedDate = book.CreatedDate,
                Name = book.Name,
                PublishYear = book.PublishYear,
                Summary = book.Summary,
                ShelfLocation = book.ShelfLocation,
            };
        }

        public IEnumerable<ReadBookResponse> Search(SearchBookRequest searchBookRequest, int userId)
        {
            return BookRepository.Search(searchBookRequest, userId);
        }

        public void Delete(int id, int userId)
        {
            BookRepository.Delete(id, userId);
        }

        public CreateNoteResponse CreateNote(CreateNoteRequest createNoteRequest, int userId)
        {
            return NoteRepository.Create(createNoteRequest, userId);
        }

        public UpdateNoteResponse UpdateNote(UpdateNoteRequest updateNoteRequest, int userId)
        {
            return NoteRepository.Update(updateNoteRequest, userId);
        }

        public void DeleteNote(int id, int userId)
        {
            NoteRepository.Delete(id, userId);
        }
    }

    public interface IBookUnitOfWork
    {
        IBookRepository BookRepository { get; }
        INoteRepository NoteRepository { get; }

        CreateBookResponse Create(CreateBookRequest createBookRequest, int userId);
        UpdateBookResponse Update(UpdateBookRequest updateBookRequest, int userId);
        void Delete(int id, int userId);

        Domain.Entity.Author CreateAuthor(CreateAuthorRequest createAuthorRequest);
        IEnumerable<ReadBookResponse> Search(SearchBookRequest searchBookRequest, int userId);

        CreateNoteResponse CreateNote(CreateNoteRequest createNoteRequest, int userId);
        UpdateNoteResponse UpdateNote(UpdateNoteRequest updateNoteRequest, int userId);
        void DeleteNote(int id, int userId);

    }
}
