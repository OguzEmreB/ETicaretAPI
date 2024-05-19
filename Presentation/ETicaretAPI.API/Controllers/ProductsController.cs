using ETicaret.Application.Repositories;
using ETicaret.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ETicaretAPI.Infrastructure.Services.Storage;
using Microsoft.EntityFrameworkCore;
using MediatR;
using ETicaret.Application.Features.Commands.Product.CreateProduct;
using ETicaret.Application.Features.Queries.Product.GetAllProduct;
using ETicaret.Application.Features.Queries.Product.GetByIdProduct;
using ETicaret.Application.Features.Commands.Product.UpdateProduct;
using ETicaret.Application.Features.Commands.Product.RemoveProduct;
using ETicaret.Application.Features.Commands.ProductImageFile.UploadProductImage;
using ETicaret.Application.Features.Commands.ProductImageFile.RemoveProductImage;
using ETicaret.Application.Features.Queries.Product.ProductImageFile.GetProductImage;

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
        readonly IMediator _mediator;

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
            IConfiguration configuration,
            IMediator mediator)
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
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
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
            // CQRS PATTERN
            GetAllProductQueryResponse response = await _mediator.Send(getAllProductQueryRequest);
            return Ok(response);
            #endregion
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] GetByIdProductQueryRequest getByIdProductQueryRequest)
        {
            GetByIdProductQueryResponse response = await _mediator.Send(getByIdProductQueryRequest);
            return Ok(response);
        }


        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateProductCommandRequest updateProductCommandRequest)
        {
            UpdateProductCommandResponse response = await _mediator.Send(updateProductCommandRequest);
            return Ok();

        }


        [HttpPost]
        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {

            CreateProductCommandResponse createProductCommandResponse = await _mediator.Send(createProductCommandRequest);
            return StatusCode((int)HttpStatusCode.Created);

        }





        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] RemoveProductCommandRequest removeProductCommandRequest)
        {

            RemoveProductCommandResponse response = await _mediator.Send(removeProductCommandRequest);

            return Ok();

        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Upload([FromQuery] UploadProductImageCommandRequest uploadProductImageCommandRequest)
        {
            uploadProductImageCommandRequest.Files = Request.Form.Files;
            UploadProductImageCommandResponse response = await _mediator.Send(uploadProductImageCommandRequest);
            return Ok();
        }

        [HttpGet("[action]/{Id}")]

        public async Task<IActionResult> GetProductImages([FromRoute]GetProductImageQueryRequest getProductImageQueryRequest)
        {
            List<GetProductImageQueryResponse >response = await _mediator.Send(getProductImageQueryRequest);
            return Ok(response);
        }

        [HttpDelete("[action]/{Id}")]
        public async Task<IActionResult> DeleteProductImage(
            [FromRoute] RemoveProductImageCommandRequest removeProductImageCommandRequest,
            [FromQuery] string imageId)
        {
            removeProductImageCommandRequest.ImageId = imageId;
            RemoveProductImageCommandResponse response = await _mediator.Send(removeProductImageCommandRequest);
            return Ok();
        }

    }
}
