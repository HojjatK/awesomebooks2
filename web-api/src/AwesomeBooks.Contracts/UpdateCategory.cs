using System.ComponentModel.DataAnnotations;

namespace AwesomeBooks.Contracts
{
    public class UpdateCategory
    {
        public int Id { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        [MaxLength(ContractConstants.NameMaxLength)]
        public string Name { get; set; }

        [MaxLength(ContractConstants.DescriptionMaxLength)]
        public string Description { get; set; }

        public int CategoryGroupId { get; set; }        
    }
}
