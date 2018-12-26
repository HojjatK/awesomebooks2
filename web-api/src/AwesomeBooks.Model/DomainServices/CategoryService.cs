using AwesomeBooks.Model.DomainEntities.Core;
using AwesomeBooks.Model.Exceptions;
using AwesomeBooks.Model.Repositories;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeBooks.Model.DomainServices
{
    public interface ICategoryService
    {
        Task<PagedResult<CategoryEntity>> GetPaged(int page, int pageSize, bool loadRelations = false);
        Task<CategoryEntity> GetById(int id);
        Task<CategoryEntity> GetByName(string groupName, string name);
        Task Create(CategoryEntity entity);
        Task Update(CategoryEntity entity);
        Task<bool> DeleteById(int id);
    }

    public class CategoryService : ICategoryService
    {
        private readonly IRepository<CategoryEntity> _categoryRepository;
        private readonly IRepository<BookEntity> _bookRepository;

        public CategoryService(
            IRepository<CategoryEntity> categoryRepository,
            IRepository<BookEntity> bookRepository)
        {
            _categoryRepository = categoryRepository;
            _bookRepository = bookRepository;
        }

        public async Task<PagedResult<CategoryEntity>> GetPaged(int page, int pageSize, bool loadRelations = false)
        {
            return await _categoryRepository.ReadPaged(page, pageSize, loadRelations);
        }

        public async Task<CategoryEntity> GetById(int id)
        {
            return await _categoryRepository.ReadByIdAsync(id);
        }

        public async Task<CategoryEntity> GetByName(string groupName, string name)
        {
            name = name?.Trim();
            groupName = groupName?.Trim();
            return await _categoryRepository.Entities
                .FirstOrDefaultAsync(e => e.Name == name && e.CategoryGroup.Name == groupName);
        }

        public async Task Create(CategoryEntity entity)
        {
            // validate duplicate name
            var name = entity.Name?.Trim();
            var groupId = entity.CategoryGroupId;
            var sameNameExists = _categoryRepository.Entities.Any(e => e.Name == name && e.CategoryGroupId == groupId);
            if (sameNameExists)
            {
                throw new EntityAlreadyExistsException($"Category with same name: '{name}' already exists.");
            }
            await _categoryRepository.CreateAsync(entity);
        }

        public async Task Update(CategoryEntity entity)
        {
            // validate duplicate name
            var name = entity.Name?.Trim();
            var id = entity.Id;
            var groupId = entity.CategoryGroupId;
            var sameNameExists = _categoryRepository.Entities.Any(e => e.Name == name && e.CategoryGroupId == groupId && e.Id != id);
            if (sameNameExists)
            {
                throw new EntityAlreadyExistsException($"Category with same name: '{name}' already exists.");
            }
            await _categoryRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteById(int id)
        {
            // validate no book is assigned to group
            var category = await _categoryRepository.TryReadByIdAsync(id, false);
            if (category != null)
            {
                var anyBookAssigned = _bookRepository.Entities.Any(e => e.CategoryId == id);
                if (anyBookAssigned)
                {
                    throw new ValidationException($"There are one or more books assigned to category: '{category.Name}'.");
                }
            }

            return await _categoryRepository.DeleteByIdAsync(id);
        }
    }
}
