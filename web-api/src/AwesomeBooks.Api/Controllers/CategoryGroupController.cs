using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AwesomeBooks.Contracts;
using AwesomeBooks.Model.DomainServices;
using AwesomeBooks.Model.DomainEntities.Core;
using AwesomeBooks.Api.Extensions;

namespace AwesomeBooks.Api.Controllers
{
    /// <summary>
    /// Category Group Controller.
    /// </summary>
    [Produces("application/json")]
    [Route(RoutePrefix)]
    public class CategoryGroupController : ControllerBase
    {
        private const string RoutePrefix = "api/v1/category-group"; 
        private ICategoryGroupService CategoryGroupService { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CategoryGroupController(
            ICategoryGroupService categoryGroupService)
        {
            CategoryGroupService = categoryGroupService;
        }

        /// <summary>
        /// Get all Category Groups.
        /// </summary>
        /// <returns>List of Category Groups.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryGroup>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var entities = await CategoryGroupService.GetPaged(1, 5000);
            var contracts = entities.Results.Select(entity => new CategoryGroup
            {
                Id = entity.Id,
                Uid = entity.Uid,
                Name = entity.Name,
                Description = entity.Description
           });
           return this.OkActionResult(contracts);
        }

        /// <summary>
        /// Get Category Group by Id.
        /// </summary>
        /// <param name="id">Category Group Id.</param>
        /// <returns>Category Group.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryGroup), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await CategoryGroupService.GetById(id);
            var contract = new CategoryGroup
            {
                Id = entity.Id,
                Uid = entity.Uid,
                Name = entity.Name,
                Description = entity.Description
            };
            return this.OkActionResult(contract);
        }
        
        /// <summary>
        /// Creates a new Category Group.
        /// </summary>
        /// <param name="categoryGroup">New Category Group.</param>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.PreconditionFailed)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Post([FromBody]CreateCategoryGroup categoryGroup)
        {
            var entity = new CategoryGroupEntity
            {
                Name = categoryGroup.Name,
                Description = categoryGroup.Description
            };
            await CategoryGroupService.Create(entity);
            return this.CreatedActionResult($"{RoutePrefix}/{entity.Id}");
        }
        
        /// <summary>
        /// Updates an existing Category Group.
        /// </summary>
        /// <param name="id">Category Group Id.</param>
        /// <param name="categoryGroup">Updating Category Group.</param>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody]UpdateCategoryGroup categoryGroup)
        {   
            var entity = await CategoryGroupService.GetById(categoryGroup.Id);
            entity.Name = categoryGroup.Name;
            entity.Description = categoryGroup.Description;
            await CategoryGroupService.Update(entity);
            return this.OkActionResult();
        }
        
        /// <summary>
        /// Delete an existing Category Group.
        /// </summary>
        /// <param name="id">Category Group Id.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            await CategoryGroupService.DeleteById(id);
            return this.ActionResult(HttpStatusCode.NoContent);
        }
    }
}