using ETicaretAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Application.Repositories
{
    public interface IInvoiceFileReadRepository : IReadRepository<InvoiceFile>
    {
      
    }
}
