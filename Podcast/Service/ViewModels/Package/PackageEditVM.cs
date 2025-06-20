using FluentValidation;
using Service.ViewModels.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Package
{
    public class PackageEditVM
    {
        public decimal Price { get; set; }
    }
    public class PackageEditVMValidator : AbstractValidator<PackageEditVM>
    {
        public PackageEditVMValidator()
        {
            RuleFor(x => x.Price).NotNull().WithMessage("Price can't be empty")
                                    .NotEmpty().WithMessage("Price can't be empty");
        }
    }
}
