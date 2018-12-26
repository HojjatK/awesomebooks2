using AwesomeBooks.Model.DomainEntities.Core;
using AwesomeBooks.Model.Exceptions;
using AwesomeBooks.Model.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeBooks.Model.DomainServices
{
    public interface IBookService
    {
        Task<PagedResult<BookEntity>> GetPaged(int page, int pageSize);
        Task<BookEntity> GetById(int id);
        Task<BookEntity> GetByName(string name, int publishYear, string authors);
        Task Create(BookEntity entity);
        Task Update(BookEntity entity);
        Task<bool> DeleteById(int id);
    }

    public class BookService : IBookService
    {
        private readonly IRepository<BookEntity> _bookRepository;

        public BookService(
            IRepository<BookEntity> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<PagedResult<BookEntity>> GetPaged(int page, int pageSize)
        {
            return await _bookRepository.ReadPaged(page, pageSize);
        }

        public async Task<BookEntity> GetById(int id)
        {
            return await _bookRepository.ReadByIdAsync(id);
        }

        public async Task<BookEntity> GetByName(string name, int publishYear, string authors)
        {
            name = name?.Trim();
            return await _bookRepository.Entities
                .FirstOrDefaultAsync(e => e.Name == name && e.PublishYear == publishYear && e.Authors == authors);
        }

        public async Task Create(BookEntity entity)
        {
            // validate duplicate name
            var name = entity.Name?.Trim();
            var year = entity.PublishYear;
            var authors = entity.Authors;
            var sameNameExists = _bookRepository.Entities.Any(e => e.Name == name && e.PublishYear == year && e.Authors == authors);
            if (sameNameExists)
            {
                throw new EntityAlreadyExistsException($"Book with same (name:'{name}', publish year:{year}, authors: {authors}) already exists.");
            }
            await _bookRepository.CreateAsync(entity);
        }

        public async Task Update(BookEntity entity)
        {
            // validate duplicate name
            var name = entity.Name?.Trim();
            var id = entity.Id;
            var year = entity.PublishYear;
            var authors = entity.Authors;
            var sameNameExists = _bookRepository.Entities.Any(e => e.Name == name && e.PublishYear == year && e.Authors == authors && e.Id != id);
            if (sameNameExists)
            {
                throw new EntityAlreadyExistsException($"Book with same (name:'{name}', publish year:{year}, authors: {authors}) already exists.");
            }
            await _bookRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteById(int id)
        {
            return await _bookRepository.DeleteByIdAsync(id);
        }
    }
}