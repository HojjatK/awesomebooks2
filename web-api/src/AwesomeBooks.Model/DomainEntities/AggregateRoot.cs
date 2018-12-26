using System;

namespace AwesomeBooks.Model.DomainEntities
{
    public class AggregateRoot : DomainEntity, IAggregateRoot
    {
        public Guid Uid { get; set; } = Guid.NewGuid();
    }
}
