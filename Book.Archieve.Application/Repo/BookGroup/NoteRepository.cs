using Book.Archieve.Application.Definition;
using Book.Archieve.Domain.DTO.Request;
using Book.Archieve.Domain.DTO.Request.Book;
using Book.Archieve.Domain.DTO.Request.Note;
using Book.Archieve.Domain.DTO.Response.Book;
using Book.Archieve.Domain.DTO.Response.Note;
using Book.Archieve.Domain.Entity;
using Book.Archieve.Infrastructure.Context;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Book.Archieve.Application.Repo.BookGroup
{
    public class NoteRepository : RepositoryBase, INoteRepository
    {

        private readonly IValidator<Domain.Entity.Note> noteValidator;

        public NoteRepository(BookContext bookContext, IValidator<Note> authorValidator) : base(bookContext)
        {
            this.noteValidator = authorValidator;
        }

        public CreateNoteResponse Create(CreateNoteRequest createNoteRequest, int userId)
        {
            var user = MasterDataDb.User.Where(u => u.Id == userId && u.IsActive).FirstOrDefault();
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı");
            var book = MasterDataDb.Book.Where(b => b.Id == createNoteRequest.BookId && b.IsActive).FirstOrDefault();
            if (book == null)
                throw new Exception("Kitap bulunamadı");

            Note note = Note.Create(createNoteRequest, userId);
            base.Validate(note, noteValidator);
            base.Add(note);

            return new CreateNoteResponse
            {
                BookId = book.Id,
                BookName = book.Name,
                CreatedDate = note.CreatedDate,
                IsShared = note.IsShared,
                Text = note.Text,
                UserId = userId,
                UserName = user.FullName
            };
        }

        public void Delete(int id, int userId)
        {
           var note = MasterDataDb.Note.Where(x => x.Id == id && x.IsActive && x.UserId == userId).FirstOrDefault();
            if (note == null)
                throw new Exception("Not bulunamadı");
            base.Delete(note);
        }

        public UpdateNoteResponse Update(UpdateNoteRequest updateNoteRequest, int userId)
        {
            var note = MasterDataDb.Note
                 .Where(n => n.Id == updateNoteRequest.Id && n.IsActive && n.UserId == userId)
                 .Include(n => n.Book)
                 .Include(n => n.User)
                 .FirstOrDefault();

            if (note == null)
                throw new Exception("Not bulunamadı");

            note.Update(updateNoteRequest);
            base.Validate(note, noteValidator);
            Save();

            return new UpdateNoteResponse
            {
                Text = note.Text,
                UserId = userId,
                UserName = note.User.FullName,
                BookId = note.BookId,
                BookName = note.Book.Name,
                IsShared = note.IsShared,
            };
        }
    }

    public interface INoteRepository
    {
        CreateNoteResponse Create(CreateNoteRequest createNoteRequest, int userId);
        UpdateNoteResponse Update(UpdateNoteRequest updateNoteRequest, int userId);
        void Delete(int id, int userId);

    }
}
