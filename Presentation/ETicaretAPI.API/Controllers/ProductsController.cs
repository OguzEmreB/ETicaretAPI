using ETicaret.Application.Repositories;
using ETicaret.Application.RequestParameters;
using ETicaret.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection.Metadata;
using static System.Net.WebRequestMethods;
using System.Security.Cryptography;
using ETicaretAPI.Infrastructure.Services.Storage;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        readonly IProductImageFileReadRepository _productImageFileReadRepository;
        readonly IInvoiceFileReadRepository _invoiceFileReadRepository;
        readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;
        readonly IFileReadRepository _fileReadRepository;
        readonly IFileWriteRepository _fileWriteRepository;
        readonly IStorageService _storageService;
        readonly IConfiguration _configuration;

        public ProductsController(IProductWriteRepository productWriteRepository,
            IProductReadRepository productReadRepository,
            IWebHostEnvironment webHostEnvironment,
            IProductImageFileWriteRepository productImageFileWriteRepository,
            IProductImageFileReadRepository productImageFileReadRepository,
            IInvoiceFileReadRepository invoiceFileReadRepository,
            IInvoiceFileWriteRepository invoiceFileWriteRepository,
            IFileReadRepository fileReadRepository,
            IFileWriteRepository fileWriteRepository,
            IStorageService storageService,
            IConfiguration configuration)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            this._webHostEnvironment = webHostEnvironment;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _invoiceFileReadRepository = invoiceFileReadRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _fileReadRepository = fileReadRepository;
            _fileWriteRepository = fileWriteRepository;
            _storageService = storageService;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Pagination pagination)
        {
            //await Task.Delay(1500); // spinner görünsün 
            #region deneme

            ////await _productWriteRepository.AddRangeAsync(new()
            ////{
            ////    new(){Id = Guid.NewGuid(),Name="Product 1",Price=100,CreatedDate=DateTime.UtcNow,Stock=10},
            ////    new(){Id = Guid.NewGuid(),Name="Product 2",Price=200,CreatedDate=DateTime.UtcNow,Stock=20},
            ////    new(){Id = Guid.NewGuid(),Name="Product 3",Price=300,CreatedDate=DateTime.UtcNow,Stock=130},
            ////});
            ////await _productWriteRepository.SaveAsync();

            //Product p = await _productReadRepository.GetByIdAsync("f2400d6b-b384-4a27-a10d-85a7c197b88f"); // tracking default true 
            ////Product p1 = await _productReadRepository.GetByIdAsync("f2400d6b-b384-4a27-a10d-85a7c197b88f", false);   // tracking false

            //p.Name = "Mehmet";
            //await _productWriteRepository.SaveAsync();// tracking false olduğu için adı değişmez 


            //p1.Name = "Mehmet";                                                                           
            //await _productWriteRepository.SaveAsync();// tracking false olduğu için adı değişmez 
            #endregion
            #region Deneme2
            //await _productWriteRepository.AddAsync(new() { Name = "C Product", Price = 1.400F, Stock = 12, CreatedDate = DateTime.UtcNow, });
            //await _productWriteRepository.SaveAsync();

            #endregion
            #region Add new Order And Customer

            //var customerId = Guid.NewGuid();
            //await _customerWriteRepository.AddAsync(new() { Id = customerId, Name = "MahmutCustomer" });

            //await _orderWriteRepository.AddAsync(new() { Description = "asdasd", Address = "Ankara , Çankaya",CustomerId = customerId });
            //await _orderWriteRepository.AddAsync(new() { Description = "asdasd2", Address = "Ankara , Çankaya2", CustomerId = customerId });
            //await _orderWriteRepository.SaveAsync();
            #endregion


            #region ReadAll

            var totalCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false).Skip(pagination.Page * pagination.Size).Take(pagination.Size).Select(p => new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.CreatedDate,
                p.UpdatedDate
            }).ToList();
            #endregion
            return Ok(new
            {
                totalCount,
                products
            });
        }


        //[HttpGet]
        //public async Task<IActionResult> Get(string Id)
        //{

        //    return Ok(await _productReadRepository.GetByIdAsync(Id, false));
        //}


        [HttpPost]
        public async Task<IActionResult> Post(VM_Create_Product model)
        {

            await _productWriteRepository.AddAsync(new()
            {
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock
            });
            await _productWriteRepository.SaveAsync();


            return StatusCode((int)HttpStatusCode.Created);

        }



        [HttpPut]
        public async Task<IActionResult> Put(VM_Update_Product model)
        {
            Product product = await _productReadRepository.GetByIdAsync(model.Id);
            product.Stock = model.Stock;
            product.Price = model.Price;
            product.Name = model.Name;
            await _productWriteRepository.SaveAsync();
            return Ok();

        }


        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {

            await _productWriteRepository.RemoveAsync(Id);
            await _productWriteRepository.SaveAsync();

            return Ok();

        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Upload(string id)
        {
            List<(string fileName, string pathOrContainerName)> result = await _storageService.UploadAsync("product-images", Request.Form.Files);

            Product product = await _productReadRepository.GetByIdAsync(id);

            //foreach (var r in result)
            //{
            //    product.ProductImageFiles.Add(new()
            //    {
            //        FileName = r.fileName,
            //        Path = r.pathOrContainerName,
            //        Storage = _storageService.StorageName,
            //        Products = new List<Product>() { product }
            //    });
            //}
            //veya


            await _productImageFileWriteRepository.AddRangeAsync(result.Select(r => new ProductImageFile
            {
                FileName = r.fileName,
                Path = r.pathOrContainerName,
                Storage = _storageService.StorageName,
                Products = new List<Product>() { product }
            }).ToList());

            await _productImageFileWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpGet("[action]/{id}")]

        public async Task<IActionResult> GetProductImages(string id)
        {
            Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
                  .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
            return Ok(product.ProductImageFiles.Select(p => new
            {
                Path = $"{_configuration["BaseStorageUrl:Azure"]}/{p.Path}",
                p.FileName,
                p.Id
            }));
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteProductImage(string id, string imageId)
        {
            Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
      .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

            ProductImageFile productImageFile = product.ProductImageFiles.FirstOrDefault(p => p.Id == Guid.Parse(imageId));
            product.ProductImageFiles.Remove(productImageFile);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }

    }
}
