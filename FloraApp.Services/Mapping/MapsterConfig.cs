using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Services.Database;
using Mapster;

namespace FloraApp.Services.Mapping
{
    public static class MapsterConfig
    {
        public static void Configure()
        {
            // Entity to Response mappings
            TypeAdapterConfig<Category, CategoryResponse>.NewConfig();
            
            TypeAdapterConfig<Product, ProductResponse>.NewConfig()
                .Map(dest => dest.CategoryName, src => src.Category.Name);
            
            TypeAdapterConfig<User, UserResponse>.NewConfig();
            
            TypeAdapterConfig<CustomBouquet, CustomBouquetResponse>.NewConfig();
            
            // Request to Entity mappings
            TypeAdapterConfig<CategoryUpsertRequest, Category>.NewConfig();
            
            TypeAdapterConfig<ProductUpsertRequest, Product>.NewConfig();
            
            TypeAdapterConfig<UserUpsertRequest, User>.NewConfig()
                .Ignore(dest => dest.PasswordHash)
                .Ignore(dest => dest.PasswordSalt);

            TypeAdapterConfig<CustomBouquetUpsertRequest, CustomBouquet>.NewConfig()
                .Ignore(dest => dest.CreatedAt);
        }
    }
} 