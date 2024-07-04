using Book.Archieve.API.Controllers;
using Book.Archieve.API.Filter;
using Book.Archieve.Application.Definition;
using Book.Archieve.Application.UnitOfWork;
using Book.Archieve.Domain.DTO.Request;
using Book.Archieve.Domain.DTO.Request.Book;
using Book.Archieve.Domain.DTO.Request.Note;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Book.Archieve.Controllers
{

    [Authorize]
    public class NoteController : BasicController
    {

        private readonly IBookUnitOfWork bookUnitOfWork;

        public NoteController(IBookUnitOfWork bookUnitOfWork)
        {
            this.bookUnitOfWork = bookUnitOfWork;
        }

        [HttpPost]
        public IActionResult Create(CreateNoteRequest createNoteRequest)
        {
            var result = bookUnitOfWork.CreateNote(createNoteRequest, UserId);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Update(UpdateNoteRequest updateNoteRequest)
        {
            var result = bookUnitOfWork.UpdateNote(updateNoteRequest, UserId);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            bookUnitOfWork.DeleteNote(id, UserId);
            return Ok();
        }

    }
}
