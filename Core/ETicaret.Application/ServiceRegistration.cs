using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationService(this IServiceCollection collection)
        {
            collection.AddMediatR(typeof(ServiceRegistration));
        }
    }
}
