using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AwesomeBooks.Contracts
{
    public class Category
    {
        public int Id { get; set; }

        public Guid Uid { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(ContractConstants.NameMaxLength)]
        public string Name { get; set; }

        [MaxLength(ContractConstants.DescriptionMaxLength)]
        public string Description { get; set; }

        public int CategoryGroupId { get; set; }

        public string CategoryGroupName { get; set; }
    }
}
