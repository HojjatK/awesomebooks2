using System;
using System.ComponentModel.DataAnnotations;

namespace AwesomeBooks.Contracts
{
    public class Book
    {
        public int Id { get; set; }

        public Guid Uid { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(ContractConstants.NameMaxLength)]
        public string Name { get; set; }

        public int PublishYear { get; set; }

        [MaxLength(ContractConstants.AuthorsMaxLength)]
        public string Authors { get; set; }

        public decimal Rating { get; set; }

        [MaxLength(ContractConstants.UrlMaxLength)]
        public string ImageUri { get; set; }

        [MaxLength(ContractConstants.UrlMaxLength)]
        public string AmazonUri { get; set; }

        [MaxLength(ContractConstants.TypeMaxLength)]
        public string ContentType { get; set; }

        [MaxLength(ContractConstants.UrlMaxLength)]
        public string ContentUri { get; set; }

        [MaxLength(ContractConstants.LongDescriptionMaxLength)]
        public string Reflection { get; set; }
        
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}
