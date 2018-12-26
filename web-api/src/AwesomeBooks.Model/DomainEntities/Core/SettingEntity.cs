 using System.ComponentModel.DataAnnotations;

namespace AwesomeBooks.Model.DomainEntities.Core
{
    public class SettingEntity : AggregateRoot, IHasName
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(EntityConstants.NameMaxLength)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(EntityConstants.TypeMaxLength)]
        public string Type { get; set; }

        public string Value { get; set; }
    }
}
