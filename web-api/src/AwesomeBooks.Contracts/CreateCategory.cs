using System.ComponentModel.DataAnnotations;

namespace AwesomeBooks.Contracts
{
    public class CreateCategory
    {   
        [Required(AllowEmptyStrings = false)]
        [MaxLength(ContractConstants.NameMaxLength)]
        public string Name { get; set; }

        [MaxLength(ContractConstants.DescriptionMaxLength)]
        public string Description { get; set; }

        public int CategoryGroupId { get; set; }
    }
}
