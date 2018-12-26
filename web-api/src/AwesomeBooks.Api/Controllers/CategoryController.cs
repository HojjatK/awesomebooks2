using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AwesomeBooks.Contracts;
using System.Net;
using AwesomeBooks.Model.DomainServices;
using AwesomeBooks.Model.DomainEntities.Core;
using AwesomeBooks.Api.Extensions;

namespace AwesomeBooks.Api.Controllers
{
    /// <summary>
    /// Category Controller.
    /// </summary>
    [Produces("application/json")]
    [Route(RoutePrefix)]
    public class CategoryController : Controller
    {
        private const string RoutePrefix = "api/v1/category";
        private ICategoryService CategoryService { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CategoryController(
            ICategoryService categoryService)
        {
            CategoryService = categoryService;
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns>List of Categories</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Category>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var entities = await CategoryService.GetPaged(1, 5000, loadRelations: true);
            var contracts = entities.Results.Select(entity => new Category
            {
                Id = entity.Id,
                Uid = entity.Uid,
                Name = entity.Name,
                Description = entity.Description,
                CategoryGroupId = entity.CategoryGroupId,
                CategoryGroupName = entity.CategoryGroup?.Name,
            });
            return this.OkActionResult(contracts);
        }

        /// <summary>
        /// Get category by Id.
        /// </summary>
        /// <param name="id">Category Id.</param>
        /// <returns>Category</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Category), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await CategoryService.GetById(id);
            var contract = new Category
            {
                Id = entity.Id,
                Uid = entity.Uid,
                Name = entity.Name,
                Description = entity.Description,
                CategoryGroupId = entity.CategoryGroupId,
                CategoryGroupName = entity.CategoryGroup?.Name,
            };
            return this.OkActionResult(contract);
        }

        /// <summary>
        /// Creates a new Category.
        /// </summary>
        /// <param name="category">New Category.</param>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.PreconditionFailed)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Post([FromBody]CreateCategory category)
        {
            var entity = new CategoryEntity
            {
                Name = category.Name,
                Description = category.Description,
                CategoryGroupId = category.CategoryGroupId
            };
            await CategoryService.Create(entity);
            return this.CreatedActionResult($"{RoutePrefix}/{entity.Id}");
        }

        /// <summary>
        /// Update an existing Category.
        /// </summary>
        /// <param name="id">Category Id.</param>
        /// <param name="category">Updating Category.</param>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody]UpdateCategory category)
        {
            if (id != category.Id)
            {
                throw new InvalidOperationException("Category identity is invalid");
            }

            var entity = await CategoryService.GetById(id);
            entity.Name = category.Name;
            entity.CategoryGroupId = category.CategoryGroupId;
            entity.Description = category.Description;
            await CategoryService.Update(entity);
            return this.OkActionResult();
        }

        /// <summary>
        /// Deletes an existing category.
        /// </summary>
        /// <param name="id">Category Id.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            await CategoryService.DeleteById(id);
            return this.ActionResult(HttpStatusCode.NoContent);
        }
    }
}