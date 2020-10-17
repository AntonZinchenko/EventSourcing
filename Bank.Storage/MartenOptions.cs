namespace Bank.Storage
{
    public class MartenOptions
    {
        public const string DefaultSchema = "public";
        public string ConnectionString { get; set; }
        public string WriteModelSchema { get; set; } = DefaultSchema;
        public string ReadModelSchema { get; set; } = DefaultSchema;
    }
}
