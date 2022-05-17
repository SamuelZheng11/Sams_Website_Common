namespace SamsWebsite.Common.Settings
{
    public class MongoDbSettings 
    {
        public string? Host { get; init; }

        public string? Port { get; init; }

        public string ConnectionString => $"mongodb://{Host}:{Port}";

        public string ProdConnectionString => $"mongodb+srv://DbObserver:passATword1@samswebsitedb.55eaz.mongodb.net";
    }
}