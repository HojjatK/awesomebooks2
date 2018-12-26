namespace AwesomeBooks.Contracts.Envelope
{
    public class Error
    {
        public MetaError Meta { get; }

        public Error(MetaError error)
        {
            Meta = error;
        }
    }
}
