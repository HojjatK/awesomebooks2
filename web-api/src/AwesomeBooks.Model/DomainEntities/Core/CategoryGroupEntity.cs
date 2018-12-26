using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AwesomeBooks.Model.DomainEntities.Core
{
    public class CategoryGroupEntity : AggregateRoot, IHasName
    {   
        [Required(AllowEmptyStrings = false)]
        [MaxLength(EntityConstants.NameMaxLength)]
        public string Name { get; set; }

        [MaxLength(EntityConstants.DescriptionMaxLength)]
        public string Description { get; set; }

        public virtual ICollection<CategoryEntity> Categories { get; private set; }
    }
}
