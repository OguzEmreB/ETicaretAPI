﻿using ETicaret.Application.ViewModels.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Application.Validators
{
    public class CreateProductValidator : AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name).NotEmpty().NotNull().WithMessage("Lütfen ürün adını giriniz")
                .MaximumLength(150).MinimumLength(5).WithMessage("Lütfen ürün adını 5 ile 150 karakter arasında giriniz");
            RuleFor(p => p.Stock).NotEmpty().NotNull().WithMessage("Lütfen stok bilgisini giriniz")
                .Must(s => s >= 0).WithMessage("Stok bilgisi negatif olamaz");
            RuleFor(p => p.Price).NotNull().NotEmpty().WithMessage("Lütfen fiyat bilgisini giriniz").Must(s => s >= 0).WithMessage("Fiyat bilgisi negatif olamaz");

        }
    }
}
