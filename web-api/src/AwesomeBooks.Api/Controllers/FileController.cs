using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using AwesomeBooks.Api.Utilities;
using AwesomeBooks.Model.DomainServices;
using AwesomeBooks.Model.Records;
using System.Net;
using AwesomeBooks.Api.Extensions;

namespace AwesomeBooks.Api.Controllers
{
    /// <summary>
    /// File Controller.
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/file")]
    public class FileController : ControllerBase
    {
        private IFileUploadUtility FileUploadUtility { get; }
        private ICsvRecordReader CsvRecordReader { get; }
        private IEntityImporter EntityImporter { get;  }

        /// <summary>
        /// Constructor
        /// </summary>
        public FileController(
            IFileUploadUtility fileUploadUtility,
            ICsvRecordReader csvRecordReader,
            IEntityImporter entityImporter)
        {
            FileUploadUtility = fileUploadUtility;
            CsvRecordReader = csvRecordReader;
            EntityImporter = entityImporter;
        }

        /// <summary>
        /// Uploads category groups
        /// </summary>
        /// <returns>Import result.</returns>
        [HttpPost]
        [Route("upload/category-group")]
        [ProducesResponseType(typeof(ImportResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UploadCategoryGroups()
        {
            var fileContent = await FileUploadUtility.GetFileContent(Request);
            var records = CsvRecordReader.GetRecords<CategoryGroupRecord, CategoryGroupRecordMap>(fileContent);
            var importResult = await EntityImporter.ImportCategoryGroups(records);
            return this.OkActionResult(importResult);
        }

        /// <summary>
        /// Uploads Categories
        /// </summary>
        /// <returns>Import result.</returns>
        [HttpPost]
        [Route("upload/category")]
        [ProducesResponseType(typeof(ImportResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UploadCategories()
        {
            var fileContent = await FileUploadUtility.GetFileContent(Request);
            var records = CsvRecordReader.GetRecords<CategoryRecord, CategoryRecordMap>(fileContent);
            var importResult = await EntityImporter.ImportCategories(records);
            return this.OkActionResult(importResult);
        }

        /// <summary>
        /// Upload books
        /// </summary>
        /// <returns>Import result</returns>
        [HttpPost]
        [Route("upload/book")]
        [ProducesResponseType(typeof(ImportResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UploadBooks()
        {
            var fileContent = await FileUploadUtility.GetFileContent(Request);
            var records = CsvRecordReader.GetRecords<BookRecord, BookRecordMap>(fileContent);
            var importResult = await EntityImporter.ImportBooks(records);
            return this.OkActionResult(importResult);
        }

        /// <summary>
        /// Downloads Category Groups as Csv
        /// </summary>
        /// <returns>Csv File</returns>
        [HttpGet]
        [Route("download/category-groups")]
        [ProducesResponseType(typeof(Stream), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DownloadCategoryGroups()
        {
            var content = await EntityImporter.ExportCategoryGroups();
            var bytes = Encoding.Default.GetBytes(content);
            return File(new MemoryStream(bytes), "text/csv", "category-groups.csv");
        }

        /// <summary>
        /// Downloads Categories as Csv
        /// </summary>
        /// <returns>Csv File</returns>
        [HttpGet]
        [Route("download/categoris")]
        //[ProducesResponseType(typeof(FileStream), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DownloadCategories()
        {
            var content = await EntityImporter.ExportCategories();
            var bytes = Encoding.Default.GetBytes(content);
            return File(new MemoryStream(bytes), "text/csv", "categories.csv");
        }

        /// <summary>
        /// Downloads Books as Csv
        /// </summary>
        /// <returns>Csv File</returns>
        [HttpGet]
        [Route("download/books")]
        //[ProducesResponseType(typeof(FileStream), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DownloadBooks()
        {
            var content = await EntityImporter.ExportBooks();
            var bytes = Encoding.Default.GetBytes(content);
            return File(new MemoryStream(bytes), "text/csv", "books.csv");
        }
    }
}