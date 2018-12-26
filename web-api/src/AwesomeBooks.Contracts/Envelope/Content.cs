namespace AwesomeBooks.Contracts.Envelope
{
    public class Content<TMeta> where TMeta : Meta 
    {
        public Content(TMeta meta)
        {
            Meta = meta;
        }

        public TMeta Meta { get; }
    }

    public class Content<TMeta, TData> : Content<TMeta> where TMeta : Meta
    {
        public TData Data { get; }

        public Content(TMeta meta, TData data) : base(meta)
        {
            Data = data;
        }
    }
}