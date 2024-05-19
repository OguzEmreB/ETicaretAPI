using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Application.Features.Queries.Product.ProductImageFile.GetProductImage
{
    public class GetProductImageQueryRequest :IRequest<List<GetProductImageQueryResponse>>
    {
        public string Id { get; set; }
    }
}
