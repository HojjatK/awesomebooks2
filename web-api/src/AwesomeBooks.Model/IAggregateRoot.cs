using System;

namespace AwesomeBooks.Model.DomainEntities
{
    public interface IAggregateRoot : IHasId, IHasConcurrencyToken
    {
        Guid Uid { get; set; }
    }
}
