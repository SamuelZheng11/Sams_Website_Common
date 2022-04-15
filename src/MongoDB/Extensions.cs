using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SamsWebsite.Common.Settings;

namespace SamsWebsite.Common.MongoDB
{
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            // Configure MongoDB and Serializers for readability
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            services.AddSingleton(serviceProvider => {
                var configuration = serviceProvider.GetService<IConfiguration>();
                var serviceSettings = configuration!.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                var mongoDbSettings = configuration!.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

                var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
                return mongoClient.GetDatabase(serviceSettings.ServiceName);
            });

            return services;
        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) 
            where T : IEntity
        {
            // Register Repositories
            services.AddSingleton<IRepository<T>>(serviceProvider => 
            {
                var database = serviceProvider.GetService<IMongoDatabase>();
                return new MongoRepository<T>(database!, "Education");
            });

            return services;
        }
    }
}