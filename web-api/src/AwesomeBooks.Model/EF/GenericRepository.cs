using AwesomeBooks.Model.DomainEntities;
using AwesomeBooks.Model.Exceptions;
using AwesomeBooks.Model.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeBooks.Model.EF
{
    public class GenericRepository<T> : IRepository<T>
        where T : class, IAggregateRoot
    {
        public GenericRepository(EntityContext dbContext)
        {
            DbContext = dbContext ?? throw new NullReferenceException(nameof(EntityContext));
        }

        private EntityContext DbContext { get; }

        public DbSet<T> Entities => DbContext.Set<T>();

        public async Task CreateAsync(T entity)
        {
            DbContext.Add(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task<T> ReadByIdAsync(int id, bool loadRelations = true)
        {
            var entity = await TryReadByIdAsync(id, loadRelations);
            if (entity == null)
            {
                throw new EntityNotFoundException(nameof(T));
            }
            return entity;
        }

        public async Task<T> TryReadByIdAsync(int id, bool loadRelations = true)
        {
            var entity = await DbContext.FindAsync<T>(id);
            if (entity == null)
            {
                return null;
            }

            if (loadRelations)
            {
                foreach (var navigation in DbContext.Entry(entity).Navigations)
                {
                    await navigation.LoadAsync();
                }
            }
            return entity;
        }

        public async Task<PagedResult<T>> ReadPaged(int page, int pageSize, bool loadRelations = false)
        {
            var query = Entities.Where(e => true);
            return await ReadPagedResult(query, page, pageSize, loadRelations);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            DbContext.Update(entity);
            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!(await EntityExistsAsync(entity.Id)))
                {
                    throw new EntityNotFoundException($"{nameof(T)} with id:{entity.Id} not found.", e);
                }
                throw;
            }
        }

        public virtual async Task<bool> DeleteByIdAsync(int id)
        {
            var entity = await TryReadByIdAsync(id);
            if (entity == null)
            {
                return false;
            }
            DbContext.Remove(entity);
            await DbContext.SaveChangesAsync();
            return true;
        }

        private async Task<bool> EntityExistsAsync(int id)
        {
            return await DbContext.Set<T>().AnyAsync(e => e.Id == id);
        }

        private async Task<PagedResult<T>> ReadPagedResult(IQueryable<T> query, int page, int pageSize, bool loadRelations = false)
        {
            var result = new PagedResult<T>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = result.PageCount == 0 ? 0 : (result.PageCount - 1) * pageSize;
            result.Results = await query.Skip(skip).Take(pageSize).ToListAsync();
            if (loadRelations)
            {
                foreach(var entity in result.Results)
                {   
                    foreach (var navigation in DbContext.Entry(entity).Navigations)
                    {
                        await navigation.LoadAsync();
                    }
                }
            }
            return result;
        }
    }
}