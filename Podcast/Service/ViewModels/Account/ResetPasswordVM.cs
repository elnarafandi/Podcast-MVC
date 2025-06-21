using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Account
{
    public class ResetPasswordVM
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
    public class ResetPasswordVMValidator : AbstractValidator<ResetPasswordVM>
    {
        public ResetPasswordVMValidator()
        {
            RuleFor(x => x.Email).NotNull().WithMessage("Can't be empty")
                                     .NotEmpty().WithMessage("Can't be empty");
            RuleFor(x => x.Token).NotNull().WithMessage("Can't be empty")
                                    .NotEmpty().WithMessage("Can't be empty");
            RuleFor(x => x.Password).NotNull().WithMessage("Can't be empty")
                                    .NotEmpty().WithMessage("Can't be empty")
                                    .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
                                    .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                                    .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
                                    .Matches("[0-9]").WithMessage("Password must contain at least one number")
                                    .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");
            RuleFor(x => x.ConfirmPassword).NotNull().WithMessage("Can't be empty")
                                           .NotEmpty().WithMessage("Can't be empty")
                                           .Equal(x => x.Password).WithMessage("Password and confirmation password do not match");

        }
    }
}
