using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Account
{
    public class ForgotPasswordVM
    {
        public string Email { get; set; }
    }
    public class ForgotPasswordVMValidator : AbstractValidator<ForgotPasswordVM>
    {
        public ForgotPasswordVMValidator()
        {
            RuleFor(x => x.Email).NotNull().WithMessage("Can't be empty")
                                     .NotEmpty().WithMessage("Can't be empty");

        }
    }
}
