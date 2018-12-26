using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AwesomeBooks.Contracts;
using System.Net;
using AwesomeBooks.Model.DomainServices;
using AwesomeBooks.Model.DomainEntities.Core;
using AwesomeBooks.Api.Extensions;
using System;

namespace AwesomeBooks.Api.Controllers
{
    /// <summary>
    /// Books Controller.
    /// </summary>
    [Produces("application/json")]
    [Route(RoutePrefix)]
    public class BookController : Controller
    {
        private const string RoutePrefix = "api/v1/book";
        private IBookService BookService { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public BookController(IBookService bookService)
        {
            BookService = bookService;
        }

        /// <summary>
        /// Get all books
        /// </summary>
        /// <returns>List of all books.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Book>), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var entities = await BookService.GetPaged(1, 5000);
            var contracts = entities.Results.Select(entity => new Book
            {
                Id = entity.Id,
                Uid = entity.Uid,
                Name = entity.Name,
                PublishYear = entity.PublishYear,
                Authors = entity.Authors,
                Rating = entity.Rating,
                ImageUri = entity.ImageUri,
                AmazonUri = entity.AmazonUri,
                ContentType = entity.ContentType,
                ContentUri = entity.ContentUri,
                Reflection = entity.Reflection,
                CategoryId = entity.CategoryId,
                CategoryName = entity.Category?.Name
            });
            return this.OkActionResult(contracts);
        }

        /// <summary>
        /// Get book by Id.
        /// </summary>
        /// <param name="id">Book Id</param>
        /// <returns>Book</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Book), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await BookService.GetById(id);
            var contract = new Book
            {
                Id = entity.Id,
                Uid = entity.Uid,
                Name = entity.Name,
                PublishYear = entity.PublishYear,
                Authors = entity.Authors,
                Rating = entity.Rating,
                ImageUri = entity.ImageUri,
                AmazonUri = entity.AmazonUri,
                ContentType = entity.ContentType,
                ContentUri = entity.ContentUri,
                Reflection = entity.Reflection,
                CategoryId = entity.CategoryId,
                CategoryName = entity.Category?.Name
            };
            return this.OkActionResult(contract);
        }

        /// <summary>
        /// Creates a new book.
        /// </summary>
        /// <param name="book">New Book</param>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.PreconditionFailed)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Post([FromBody]CreateBook book)
        {
            var entity = new BookEntity
            {
                Name = book.Name,
                PublishYear = book.PublishYear,
                Authors = book.Authors,
                ImageUri = book.ImageUri,
                AmazonUri = book.AmazonUri,
                ContentType = book.ContentType,
                ContentUri = book.ContentUri,
                Reflection = book.Reflection,
                CategoryId = book.CategoryId
            };
            await BookService.Create(entity);
            return this.CreatedActionResult($"{RoutePrefix}/{entity.Id}");
        }

        /// <summary>
        /// Updates an existing book
        /// </summary>
        /// <param name="id">Book Id.</param>
        /// <param name="book">Updating book.</param>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody]UpdateBook book)
        {
            if (id != book.Id)
            {
                throw new InvalidOperationException("Book identity is invalid.");
            }
            var entity = await BookService.GetById(id);
            entity.Name = book.Name;
            entity.PublishYear = book.PublishYear;
            entity.Authors = book.Authors;
            entity.ImageUri = book.ImageUri;
            entity.AmazonUri = book.AmazonUri;
            entity.ContentType = book.ContentType;
            entity.ContentUri = book.ContentUri;
            entity.Reflection = book.Reflection;
            entity.CategoryId = book.CategoryId;
            await BookService.Update(entity);
            return this.OkActionResult();
        }


        /// <summary>
        /// Deletes an existing book
        /// </summary>
        /// <param name="id">Book Id.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            await BookService.DeleteById(id);
            return this.ActionResult(HttpStatusCode.NoContent);
        }
    }
}