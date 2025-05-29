using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Account
{
    public class RegisterVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
    public class RegisterVMValidator : AbstractValidator<RegisterVM>
    {
        public RegisterVMValidator()
        {
            RuleFor(x => x.FirstName).NotNull().WithMessage("Can't be empty");
            RuleFor(x => x.LastName).NotNull().WithMessage("Can't be empty");
            RuleFor(x => x.UserName).NotNull().WithMessage("Can't be empty");
            RuleFor(x => x.Email).NotNull().WithMessage("Can't be empty")
                                    .EmailAddress().WithMessage("Email address format is wrong");
            RuleFor(x => x.Password).NotNull().WithMessage("Can't be empty");
            RuleFor(x => x.ConfirmPassword).NotNull().WithMessage("Can't be empty")
                                           .Equal(x => x.Password).WithMessage("Password and confirmation password do not match");
        }
    }
}
