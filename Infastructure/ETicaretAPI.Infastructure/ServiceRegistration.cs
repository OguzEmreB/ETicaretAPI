
using ETicaret.Application.Abstractions.Storage;
using ETicaret.Application.Abstractions.Token;
using ETicaret.Infrastructure.Enums;
using ETicaret.Infrastructure.Services;
using ETicaret.Infrastructure.Services.Storage;
using ETicaret.Infrastructure.Services.Storage.Azure;
using ETicaret.Infrastructure.Services.Storage.Local;
using ETicaret.Infrastructure.Services.Token;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IStorageService,StorageService>();
            serviceCollection.AddScoped<ITokenHandler,TokenHandler>();
        }
        public static void AddStorage<T>(this IServiceCollection serviceCollection)where T : Storage,IStorage
        {
            serviceCollection.AddScoped<IStorage,T>();
        } 
        public static void AddStorage(this IServiceCollection serviceCollection, StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.Local:
                    serviceCollection.AddScoped<IStorage, LocalStorage>();
                    break;
                case StorageType.Azure:
                    serviceCollection.AddScoped<IStorage, AzureStorage>();

                    break;
                case StorageType.AWS:
                    
                    break;
                default:
                    serviceCollection.AddScoped<IStorage, LocalStorage>();
                    break;
            }
        }
    }
}
