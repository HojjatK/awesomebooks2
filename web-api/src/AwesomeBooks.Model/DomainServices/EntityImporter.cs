using AwesomeBooks.Model.DomainEntities.Core;
using AwesomeBooks.Model.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeBooks.Model.DomainServices
{
    public class ImportResult
    {
        public int SucceedCount { get; set; }
        public int FailedCount { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }

    public interface IEntityImporter
    {
        Task<ImportResult> ImportCategoryGroups(IList<CategoryGroupRecord> records);
        Task<ImportResult> ImportCategories(IList<CategoryRecord> records);
        Task<ImportResult> ImportBooks(IList<BookRecord> records);

        Task<string> ExportCategoryGroups();
        Task<string> ExportCategories();
        Task<string> ExportBooks();
    }

    public class EntityImporter : IEntityImporter
    {
        private ICategoryGroupService CategoryGroupService { get; }
        private ICategoryService CategoryService { get; }
        private IBookService BookService { get; }
        private ICsvRecordReader CsvRecordReader { get; }

        public EntityImporter(
            ICategoryGroupService categoryGroupService,
            ICategoryService categoryService,
            IBookService bookService,
            ICsvRecordReader csvRecordReader)
        {
            CategoryGroupService = categoryGroupService;
            CategoryService = categoryService;
            BookService = bookService;
            CsvRecordReader = csvRecordReader;
        }

        public async Task<ImportResult> ImportCategoryGroups(IList<CategoryGroupRecord> records)
        {
            var result = new ImportResult();
            int counter = 0;
            foreach (var record in records)
            {
                counter++;
                if (!CategoryGroupRecordValid(record, out var errorMsg))
                {
                    result.ErrorMessages.Add($"[{counter}]: {errorMsg}");
                    continue;
                }

                var categoryGroup = await CategoryGroupService.GetByName(record.Name);
                if (categoryGroup != null)
                {
                    result.ErrorMessages.Add($"[{counter}]: CategoryGroup with name:'{record.Name}' already exists.");
                    continue;
                }

                categoryGroup = new CategoryGroupEntity
                {
                    Name = record.Name,
                    Description = record.Description
                };
                await CategoryGroupService.Create(categoryGroup);
            }
            return result;
        }

        public async Task<ImportResult> ImportCategories(IList<CategoryRecord> records)
        {
            var result = new ImportResult();
            int counter = 0;
            foreach (var record in records)
            {
                counter++;
                if (!CategoryRecordValid(record, out var errorMsg))
                {
                    result.ErrorMessages.Add($"[{counter}]: {errorMsg}");
                    continue;
                }
                var categoryGroup = await CategoryGroupService.GetByName(record.GroupName);
                if (categoryGroup == null)
                {
                    result.ErrorMessages.Add($"[{counter}]: CategoryGroup with name: {record.GroupName} does not exist.");
                    continue;
                }

                var category = await CategoryService.GetByName(record.GroupName, record.Name);
                if (category != null)
                {
                    result.ErrorMessages.Add($"[{counter}]: Category with name:'{record.Name}', group: {record.GroupName} already exists.");
                    continue;
                }

                category = new CategoryEntity
                {
                    Name = record.Name,
                    Description = record.Description,
                    CategoryGroupId = categoryGroup.Id,
                    CategoryGroup = categoryGroup
                };
                await CategoryService.Create(category);
            }
            return result;
        }

        public async Task<ImportResult> ImportBooks(IList<BookRecord> records)
        {
            var result = new ImportResult();
            int counter = 0;
            foreach (var record in records)
            {
                counter++;
                if (!BookRecordValid(record, out var errorMsg))
                {
                    result.ErrorMessages.Add($"[{counter}]: {errorMsg}");
                    continue;
                }

                var categoryGroup = await CategoryGroupService.GetByName(record.CategoryGroupName);
                if (categoryGroup == null)
                {
                    result.ErrorMessages.Add($"[{counter}]: CategoryGroup with name: {record.CategoryGroupName} does not exist.");
                    continue;
                }

                var category = await CategoryService.GetByName(record.CategoryGroupName, record.CategoryName);
                if (category == null)
                {
                    result.ErrorMessages.Add($"[{counter}]: Category with name: {record.CategoryName} does not exist or belongs to category group: {record.CategoryGroupName}.");
                    continue;
                }
                var book = await BookService.GetByName(record.Title, record.PublishYear, record.Authors);
                if (book != null)
                {
                    result.ErrorMessages.Add($"[{counter}]: Book with title:'{record.Title}', publishYear:'{record.PublishYear}', authors:'{record.Authors}' already exists.");
                    continue;
                }

                book = new BookEntity
                {
                    Name = record.Title,
                    PublishYear = record.PublishYear,
                    Authors = record.Authors,
                    Rating = record.Rating,
                    ImageUri = record.ImageUri,
                    AmazonUri = record.AmazonUri,
                    ContentUri = record.ContentUri,
                    ContentType = record.ContentType,
                    Reflection = record.Reflection,
                    CategoryId = category.Id,
                    Category = category
                };
                await BookService.Create(book);
            }
            return result;
        }

        public async Task<string> ExportCategoryGroups()
        {
            var entities = (await CategoryGroupService.GetPaged(0, 5000)).Results; // read all
            var records = entities.Select(c => new CategoryGroupRecord
            {
                Name = c.Name,
                Description = c.Description
            });
            var content = CsvRecordReader.GetContent<CategoryGroupRecord, CategoryGroupRecordMap>(records);
            return content;
        }

        public async Task<string> ExportCategories()
        {
            var entities = (await CategoryService.GetPaged(0, 5000, loadRelations: true)).Results; // read all
            var records = entities.Select(c => new CategoryRecord
            {
                Name = c.Name,
                Description = c.Description,
                GroupName = c.CategoryGroup.Name
            });
            var content = CsvRecordReader.GetContent<CategoryRecord, CategoryRecordMap>(records);
            return content;
        }

        public async Task<string> ExportBooks()
        {
            var entities = (await BookService.GetPaged(0, 5000)).Results; // read all
            var records = entities.Select(b => new BookRecord
            {
                Title = b.Name,
                PublishYear = b.PublishYear,
                Authors = b.Authors,
                Rating = b.Rating,
                ImageUri = b.ImageUri,
                AmazonUri = b.AmazonUri,
                ContentUri = b.ContentUri,
                ContentType = b.ContentType,
                Reflection = b.Reflection,
                CategoryGroupName = b.Category?.CategoryGroup?.Name,
                CategoryName = b.Category?.Name
            });
            var content = CsvRecordReader.GetContent<BookRecord, BookRecordMap>(records);
            return content;
        }

        private bool CategoryGroupRecordValid(CategoryGroupRecord record, out string errorMessage)
        {
            errorMessage = "";
            if (string.IsNullOrWhiteSpace(record.Name))
            {
                errorMessage = "Category group name is null or empty.";
                return false;
            }
            return true;
        }

        private bool CategoryRecordValid(CategoryRecord record, out string errorMessage)
        {
            errorMessage = "";
            if (string.IsNullOrWhiteSpace(record.GroupName))
            {
                errorMessage = "Category group name is null or empty.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(record.Name))
            {
                errorMessage = "Category name is null or empty.";
                return false;
            }
            return true;
        }

        private bool BookRecordValid(BookRecord record, out string errorMessage)
        {
            errorMessage = "";
            if (string.IsNullOrWhiteSpace(record.CategoryGroupName))
            {
                errorMessage = "Category group name is null or empty.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(record.CategoryName))
            {
                errorMessage = "Category name is null or empty.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(record.Title))
            {
                errorMessage = "Title is null or empty.";
                return false;
            }
            return true;
        }
    }
}
