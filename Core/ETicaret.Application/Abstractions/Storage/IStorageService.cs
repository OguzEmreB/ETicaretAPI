using ETicaret.Application.Abstractions.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Services.Storage
{
    public interface IStorageService : IStorage
    {
        string StorageName { get; }
    }
}
