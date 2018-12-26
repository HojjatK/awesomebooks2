namespace AwesomeBooks.Model.DomainEntities
{
    public interface IHasConcurrencyToken
    {
        byte[] Timestamp { get; set; }
    }
}