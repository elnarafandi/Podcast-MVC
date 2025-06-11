using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Account
{
    public class AccountEditVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public IFormFile UploadImage { get; set; }
    }
    public class AccountEditVMValidator : AbstractValidator<AccountEditVM>
    {
        public AccountEditVMValidator()
        {
            RuleFor(x => x.FirstName).NotNull().WithMessage("Can't be empty")
                                     .NotEmpty().WithMessage("Can't be empty");
            RuleFor(x => x.LastName).NotNull().WithMessage("Can't be empty")
                                    .NotEmpty().WithMessage("Can't be empty");

        }
    }
}
