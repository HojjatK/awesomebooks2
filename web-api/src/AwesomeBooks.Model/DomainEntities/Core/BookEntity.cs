using System.ComponentModel.DataAnnotations;

namespace AwesomeBooks.Model.DomainEntities.Core
{
    public class BookEntity : AggregateRoot
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(EntityConstants.NameMaxLength)]
        public string Name { get; set; }

        public int PublishYear { get; set; }

        [MaxLength(EntityConstants.AuthorsMaxLength)]
        public string Authors { get; set; }

        public decimal Rating { get; set; }

        [MaxLength(EntityConstants.UrlMaxLength)]
        public string ImageUri { get; set; }

        [MaxLength(EntityConstants.UrlMaxLength)]
        public string AmazonUri { get; set; }

        [MaxLength(EntityConstants.TypeMaxLength)]
        public string ContentType { get; set; }

        [MaxLength(EntityConstants.UrlMaxLength)]
        public string ContentUri { get; set; }

        [MaxLength(EntityConstants.LongDescriptionMaxLength)]
        public string Reflection { get; set; }

        [Required]
        public virtual CategoryEntity Category { get; set; }

        public int CategoryId { get; set; }
    }
}