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
            RuleFor(x => x.FirstName).NotNull().WithMessage("Can't be empty")
                                     .NotEmpty().WithMessage("Can't be empty");
            RuleFor(x => x.LastName).NotNull().WithMessage("Can't be empty")
                                    .NotEmpty().WithMessage("Can't be empty");
            RuleFor(x => x.UserName)
                                    .NotNull().WithMessage("Can't be empty")
                                    .NotEmpty().WithMessage("Can't be empty")
                                    .Length(3, 20).WithMessage("Username must be between 3 and 20 characters")
                                    .Matches(@"^[a-zA-Z0-9-._@+]+$").WithMessage("Username can only contain letters, numbers, and the characters '-._@+'");
            RuleFor(x => x.Email).NotNull().WithMessage("Can't be empty")
                                 .NotEmpty().WithMessage("Can't be empty")
                                 .Matches(@"^(?!.*\.\.)[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Email address format is wrong");
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
