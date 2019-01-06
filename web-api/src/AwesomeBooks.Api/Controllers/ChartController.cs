using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AwesomeBooks.Contracts.Reports;
using AwesomeBooks.Model.DomainServices;
using AwesomeBooks.Api.Extensions;
using AwesomeBooks.Model.DomainEntities.Core;

namespace AwesomeBooks.Api.Controllers
{
    /// <summary>
    /// Charts Controller.
    /// </summary>
    [Produces("application/json")]
    [Route(RoutePrefix)]
    public class ChartController : Controller
    {
        private const string RoutePrefix = "api/v1/chart";

        private ICategoryGroupService CategoryGroupService { get; }
        private ICategoryService CategoryService { get; }
        private IBookService BookService { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ChartController(
            ICategoryGroupService categoryGroupService,
            ICategoryService categoryService,
            IBookService bookService)
        {
            CategoryGroupService = categoryGroupService;
            CategoryService = categoryService;
            BookService = bookService;
        }

        /// <summary>
        /// Get number of books per category group.
        /// </summary>        
        [HttpGet("category-group")]
        [ProducesResponseType(typeof(ChartResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetCategoryGroupChart()
        {
            var categoryGroups = (await CategoryGroupService.GetPaged(1, 5000)).Results;
            var categoryGroupItems = categoryGroups.ToDictionary(cg => cg.Id,  cg => new ChartItem { CategoryGroup = cg });

            var categories = (await CategoryService.GetPaged(1, 5000)).Results;
            foreach(var category in categories)
            {
                ChartItem item = null;
                if (categoryGroupItems.TryGetValue(category.CategoryGroupId, out item))
                {
                    item.Categories.Add(category);
                }
            }

            var books = (await BookService.GetPaged(1, 10000)).Results;
            var categoryLookup = categories.ToDictionary(c => c.Id);
            foreach(var book in books)
            {
                CategoryEntity category= null;
                if (categoryLookup.TryGetValue(book.CategoryId, out category))
                {
                    ChartItem item = null;
                    if (categoryGroupItems.TryGetValue(category.CategoryGroupId, out item))
                    {
                        item.Books.Add(book);
                    }
                }
            }
            
            var contract = new ChartResult
            {
                ChartTitle = "Category Group",
                Labels = new List<string>(),
                Data = new List<decimal>()
            };

            foreach(var chartItem in categoryGroupItems.Values)
            {
                contract.Labels.Add(chartItem.CategoryGroup.Name);
                contract.Data.Add(chartItem.Books.Count);
            }
            return this.OkActionResult(contract);
        }

        /// <summary>
        /// Get number of books per category.
        /// </summary>        
        [HttpGet("category")]
        [ProducesResponseType(typeof(ChartResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetCategoryChart()
        {
            var categoryGroups = (await CategoryGroupService.GetPaged(1, 5000)).Results;
            var categoryGroupItems = categoryGroups.ToDictionary(cg => cg.Id, cg => new ChartItem { CategoryGroup = cg });

            var categories = (await CategoryService.GetPaged(1, 5000)).Results;
            foreach (var category in categories)
            {
                ChartItem item = null;
                if (categoryGroupItems.TryGetValue(category.CategoryGroupId, out item))
                {
                    item.Categories.Add(category);
                }
            }

            var books = (await BookService.GetPaged(1, 10000)).Results;
            var categoryLookup = categories.ToDictionary(c => c.Id);
            foreach (var book in books)
            {
                CategoryEntity category = null;
                if (categoryLookup.TryGetValue(book.CategoryId, out category))
                {
                    ChartItem item = null;
                    if (categoryGroupItems.TryGetValue(category.CategoryGroupId, out item))
                    {
                        item.Books.Add(book);
                    }
                }
            }

            var contract = new ChartResult
            {
                ChartTitle = "Category",
                Labels = new List<string>(),
                Data = new List<decimal>()
            };

            foreach (var chartItem in categoryGroupItems.Values)
            {
                foreach(var category in chartItem.Categories)
                {
                    contract.Labels.Add($"{chartItem.CategoryGroup.Name}-{category.Name}");
                    var categoryBooks = chartItem.Books.Where(b => b.CategoryId == category.Id).ToList();
                    contract.Data.Add(categoryBooks.Count);
                }
            }
            return this.OkActionResult(contract);
        }

        /// <summary>
        /// Get number of books per category.
        /// </summary>        
        [HttpGet("category-serie")]
        [ProducesResponseType(typeof(ChartSerieResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetCategorySerieChart()
        {
            var categoryGroups = (await CategoryGroupService.GetPaged(1, 5000)).Results;
            var categoryGroupItems = categoryGroups.ToDictionary(cg => cg.Id, cg => new ChartItem { CategoryGroup = cg });

            var categories = (await CategoryService.GetPaged(1, 5000)).Results;
            foreach (var category in categories)
            {
                ChartItem item = null;
                if (categoryGroupItems.TryGetValue(category.CategoryGroupId, out item))
                {
                    item.Categories.Add(category);
                }
            }

            var books = (await BookService.GetPaged(1, 10000)).Results;
            var categoryLookup = categories.ToDictionary(c => c.Id);
            foreach (var book in books)
            {
                CategoryEntity category = null;
                if (categoryLookup.TryGetValue(book.CategoryId, out category))
                {
                    ChartItem item = null;
                    if (categoryGroupItems.TryGetValue(category.CategoryGroupId, out item))
                    {
                        item.Books.Add(book);
                    }
                }
            }

            var contract = new ChartSerieResult
            {
                ChartTitle = "Categories",
                Labels = new List<string>(),
                Data = new List<ChartSerie>()
            };

            foreach (var chartItem in categoryGroupItems.Values)
            {
                contract.Labels.Add($"{chartItem.CategoryGroup.Name}");
                foreach (var category in chartItem.Categories)
                {
                    var categoryBooks = chartItem.Books.Where(b => b.CategoryId == category.Id).ToList();
                    var serie = new ChartSerie
                    {
                        SerieId = category.Id,
                        SerieName = category.Name,
                        Data = new List<decimal>()
                    };
                    contract.Data.Add(serie);
                }
            }

            foreach(var serie in contract.Data)
            {
                foreach(var chartItem in categoryGroupItems.Values)
                {                    
                    if (chartItem.Categories.ToDictionary(c => c.Id).ContainsKey(serie.SerieId))
                    {
                        // Category belong to category group
                        serie.Data.Add(chartItem.Books.Count(b => b.CategoryId == serie.SerieId));
                    }
                    else
                    {
                        serie.Data.Add(0);
                    }
                }
            }
            
            return this.OkActionResult(contract);
        }

        /// <summary>
        /// Get books published year per category-group.
        /// </summary>        
        [HttpGet("book-publish")]
        [ProducesResponseType(typeof(ChartSerieResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetBookPublishChart()
        {
            var categoryGroups = (await CategoryGroupService.GetPaged(1, 5000)).Results;
            var categoryGroupItems = categoryGroups.ToDictionary(cg => cg.Id, cg => new ChartItem { CategoryGroup = cg });

            var categories = (await CategoryService.GetPaged(1, 5000)).Results;
            foreach (var category in categories)
            {
                ChartItem item = null;
                if (categoryGroupItems.TryGetValue(category.CategoryGroupId, out item))
                {
                    item.Categories.Add(category);
                }
            }

            var books = (await BookService.GetPaged(1, 10000)).Results;
            var categoryLookup = categories.ToDictionary(c => c.Id);
            foreach (var book in books)
            {
                CategoryEntity category = null;
                if (categoryLookup.TryGetValue(book.CategoryId, out category))
                {
                    ChartItem item = null;
                    if (categoryGroupItems.TryGetValue(category.CategoryGroupId, out item))
                    {
                        item.Books.Add(book);
                    }
                }
            }

            var publishYears = books.Select(b => b.PublishYear).Distinct().OrderBy(y => y);
            var contract = new ChartSerieResult
            {
                ChartTitle = "Book publish year",
                Labels = new List<string>(),
                Data = new List<ChartSerie>()
            };

            foreach(var year in publishYears)
            {
                contract.Labels.Add($"{year}");
                foreach (var groupItem in categoryGroupItems.Values)
                {
                    var categoryGroup = groupItem.CategoryGroup;
                    var serie = new ChartSerie
                    {
                        SerieId = categoryGroup.Id,
                        SerieName = categoryGroup.Name,
                        Data = new List<decimal>()
                    };
                    contract.Data.Add(serie);
                }
            }

            foreach(var serie in contract.Data)
            {
                ChartItem chartItem = null;
                if(categoryGroupItems.TryGetValue(serie.SerieId, out chartItem))
                {
                    foreach (var year in publishYears)
                    {
                        serie.Data.Add(chartItem.Books.Count(b => b.PublishYear == year));
                    }
                }
            }
            
            return this.OkActionResult(contract);
        }

        private class ChartItem
        {
            public CategoryGroupEntity CategoryGroup { get; set; }
            public List<CategoryEntity> Categories { get; set; } = new List<CategoryEntity>();
            public List<BookEntity> Books { get; set; } = new List<BookEntity>();
        }
    }
}
