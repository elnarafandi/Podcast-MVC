using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.TeamMember
{
    public class TeamMemberEditVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Image { get; set; }
        public IFormFile? UploadImage { get; set; }
        public string Information { get; set; }
        public string SocialMedia { get; set; }
    }
    public class TeamMemberEditVMValidator : AbstractValidator<TeamMemberEditVM>
    {
        public TeamMemberEditVMValidator()
        {
            RuleFor(x => x.FirstName).NotNull().WithMessage("Can't be empty")
                                     .NotEmpty().WithMessage("Can't be empty");
            RuleFor(x => x.LastName).NotNull().WithMessage("Can't be empty")
                                    .NotEmpty().WithMessage("Can't be empty");
            RuleFor(x => x.Email).NotNull().WithMessage("Can't be empty")
                                    .NotEmpty().WithMessage("Can't be empty");
            RuleFor(x => x.Information).NotNull().WithMessage("Can't be empty")
                                       .NotEmpty().WithMessage("Can't be empty");
            RuleFor(x => x.SocialMedia).NotNull().WithMessage("Can't be empty")
                                       .NotEmpty().WithMessage("Can't be empty");
        }
    }
}
