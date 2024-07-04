using Book.Archieve.API.Controllers;
using Book.Archieve.API.Filter;
using Book.Archieve.Application.Definition;
using Book.Archieve.Application.UnitOfWork;
using Book.Archieve.Domain.DTO.Request.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Book.Archieve.Controllers
{

    [Authorize]
    public class BookController : BasicController
    {

        private readonly IBookUnitOfWork bookUnitOfWork;

        public BookController(IBookUnitOfWork bookUnitOfWork)
        {
            this.bookUnitOfWork = bookUnitOfWork;
        }

        [HttpPost]
        public IActionResult Create(CreateBookRequest createBookRequest)
        {
            var result = bookUnitOfWork.Create(createBookRequest, UserId);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Update(UpdateBookRequest updateBookRequest)
        {
            var result = bookUnitOfWork.Update(updateBookRequest, UserId);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            bookUnitOfWork.Delete(id, UserId);
            return Ok();
        }

        [HttpPost]
        public IActionResult Search(SearchBookRequest searchBookRequest)
        {
            var result = bookUnitOfWork.Search(searchBookRequest, UserId);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateAuthor(CreateAuthorRequest createAuthorRequest)
        {
            var author = bookUnitOfWork.CreateAuthor(createAuthorRequest);
            return Ok(author);
        }
    }
}
