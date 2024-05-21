using ETicaret.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Application.Features.Queries.Product.ProductImageFile.GetProductImage
{
    public class GetProductImageQueryHandler : IRequestHandler<GetProductImageQueryRequest, List<GetProductImageQueryResponse>>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IConfiguration _configuration;

        public GetProductImageQueryHandler(IProductReadRepository productReadRepository, IConfiguration configuration)
        {
            _productReadRepository = productReadRepository;
            _configuration = configuration;
        }

        public async Task<List<GetProductImageQueryResponse>> Handle(GetProductImageQueryRequest request, CancellationToken cancellationToken)
        {
            ETicaret.Domain.Entities.Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
                    .FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.Id));
            return product?.ProductImageFiles.Select(p => new GetProductImageQueryResponse
            {
                Path = $"{_configuration["BaseStorageUrl:Azure"]}/{p.Path}",
              FileName = p.FileName,
               Id=p.Id
            }).ToList();
        }
    }
}
