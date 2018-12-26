using System.ComponentModel.DataAnnotations;

namespace AwesomeBooks.Contracts
{
    public class CreateCategoryGroup
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(ContractConstants.NameMaxLength)]
        public string Name { get; set; }

        [MaxLength(ContractConstants.DescriptionMaxLength)]
        public string Description { get; set; }
    }
}
