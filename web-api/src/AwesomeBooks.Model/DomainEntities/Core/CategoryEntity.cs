using System.ComponentModel.DataAnnotations;

namespace AwesomeBooks.Model.DomainEntities.Core
{
    public class CategoryEntity : AggregateRoot, IHasName
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(EntityConstants.NameMaxLength)]
        public string Name { get; set; }

        [MaxLength(EntityConstants.DescriptionMaxLength)]
        public string Description { get; set; }

        public virtual int CategoryGroupId { get; set; }

        [Required]
        public virtual CategoryGroupEntity CategoryGroup { get; set; }
    }
}
