# soulgram-file-manager
NuGet Package to proceed CRUD operation with files in Azure Blob Storage


It's necessary to register dependencies in client app
```c#
private static void AddFileManager(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BlobStorageOptions>(options => configuration.GetSection("BlobStorageOptions").Bind(options));
        services.AddScoped<IContainerNameResolver, ContainerNameResolver>();
        services.AddScoped<IFileManager, FileManager>();
    }
```
