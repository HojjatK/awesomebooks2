using AwesomeBooks.Model.DomainEntities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeBooks.Model.Repositories
{
    public interface IRepository<T> where T : class, IAggregateRoot
    {
        DbSet<T> Entities { get; }
        Task CreateAsync(T entity);
        Task<T> ReadByIdAsync(int id, bool loadRelations = true);
        Task<T> TryReadByIdAsync(int id, bool loadRelations = true);
        Task<PagedResult<T>> ReadPaged(int page, int pageSize, bool loadRelations = false);
        Task UpdateAsync(T entity);
        Task<bool> DeleteByIdAsync(int id);
    }
}
