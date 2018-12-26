using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AwesomeBooks.Model.DomainEntities.Core;
using AwesomeBooks.Model.Exceptions;
using AwesomeBooks.Model.Repositories;

namespace AwesomeBooks.Model.DomainServices
{
    public interface ICategoryGroupService
    {
        Task<PagedResult<CategoryGroupEntity>> GetPaged(int page, int pageSize);
        Task<CategoryGroupEntity> GetById(int id);
        Task<CategoryGroupEntity> GetByName(string name);
        Task Create(CategoryGroupEntity entity);
        Task Update(CategoryGroupEntity entity);
        Task<bool> DeleteById(int id);
    }

    public class CategoryGroupService : ICategoryGroupService
    {
        private readonly IRepository<CategoryGroupEntity> _categoryGroupRepository;
        private readonly IRepository<CategoryEntity> _categoryRepository;

        public CategoryGroupService(
            IRepository<CategoryGroupEntity> categoryGroupRepository,
            IRepository<CategoryEntity> categoryRepository)
        {
            _categoryGroupRepository = categoryGroupRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<PagedResult<CategoryGroupEntity>> GetPaged(int page, int pageSize)
        {   
            return await _categoryGroupRepository.ReadPaged(page, pageSize);
        }

        public async Task<CategoryGroupEntity> GetById(int id)
        {
            return await _categoryGroupRepository.ReadByIdAsync(id);
        }

        public async Task<CategoryGroupEntity> GetByName(string name)
        {
            name = name?.Trim();
            return await _categoryGroupRepository.Entities.FirstOrDefaultAsync(e => e.Name == name);
        }

        public async Task Create(CategoryGroupEntity entity)
        {
            // validate duplicate name
            var name = entity.Name?.Trim();
            var sameNameExists = _categoryGroupRepository.Entities.Any(e => e.Name == name);
            if (sameNameExists)
            {
                throw new EntityAlreadyExistsException($"CategoryGroup with same name: '{name}' already exists.");
            }
            await _categoryGroupRepository.CreateAsync(entity);
        }

        public async Task Update(CategoryGroupEntity entity)
        {
            // validate duplicate name
            var name = entity.Name?.Trim();
            var id = entity.Id;
            var sameNameExists = _categoryGroupRepository.Entities.Any(e => e.Name == name && e.Id != id);
            if (sameNameExists)
            {
                throw new EntityAlreadyExistsException($"CategoryGroup with same name: '{name}' already exists.");
            }
            await _categoryGroupRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteById(int id)
        {
            // validate no category is assigned to group
            var categoryGroup = await _categoryGroupRepository.TryReadByIdAsync(id, false);
            if (categoryGroup != null)
            {
                var anyCategoryAssigned = _categoryRepository.Entities.Any(e => e.CategoryGroupId == id);
                if (anyCategoryAssigned)
                {
                    throw new ValidationException($"There are one or more categories assigned to category group: '{categoryGroup.Name}'.");
                }
            }

            return await _categoryGroupRepository.DeleteByIdAsync(id);
        }
    }
}