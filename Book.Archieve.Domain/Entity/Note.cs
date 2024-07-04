﻿using Book.Archieve.Domain.DTO.Request;
using Book.Archieve.Domain.DTO.Request.Book;
using Book.Archieve.Domain.DTO.Request.Note;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Archieve.Domain.Entity
{
    public class Note : EntityBase
    {

        private Note()
        {

        }

        public string Text { get; private set; }
        public bool IsShared { get; private set; } = true;
        public DateTime CreatedDate { get; private set; }

        public int UserId { get; private set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; private set; }

        public int BookId { get; private set; }
        [ForeignKey(nameof(BookId))]
        public Book Book { get; private set; }

        public static Note Create(CreateNoteRequest createNoteRequest, int userId)
        {
            Note note = new Note();
            note.Text = createNoteRequest.Text;
            note.BookId = createNoteRequest.BookId;
            note.UserId = userId;
            note.IsShared = createNoteRequest.IsShared;
            note.CreatedDate = DateTime.Now;
            return note;
        }

        public void Update(UpdateNoteRequest updateNoteRequest)
        {
            Text = updateNoteRequest.Text;
            IsShared = updateNoteRequest.IsShared;
        }
    }
}