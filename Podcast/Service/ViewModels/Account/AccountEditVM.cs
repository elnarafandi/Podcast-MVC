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
        public string? Image { get; set; }
        public IFormFile? UploadImage { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public bool IsAdmin { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }
    public class AccountEditVMValidator : AbstractValidator<AccountEditVM>
    {
        public AccountEditVMValidator()
        {
            RuleFor(x => x.FirstName).NotNull().WithMessage("Can't be empty")
                                     .NotEmpty().WithMessage("Can't be empty");
            RuleFor(x => x.LastName).NotNull().WithMessage("Can't be empty")
                                    .NotEmpty().WithMessage("Can't be empty");


            RuleFor(x => x.Password)
                                    .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
                                    .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                                    .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
                                    .Matches("[0-9]").WithMessage("Password must contain at least one number")
                                    .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");
            RuleFor(x => x.ConfirmPassword)
                                           .Equal(x => x.Password).WithMessage("Password and confirmation password do not match");

        }
    }
}
