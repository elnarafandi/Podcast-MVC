using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Podcast
{
    public class PodcastEditVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public IFormFile UploadImage { get; set; }
        public int TeamMemberId { get; set; }
        public int PodcastCategoryId { get; set; }
    }
    public class PodcastEditVMValidator : AbstractValidator<PodcastEditVM>
    {
        public PodcastEditVMValidator()
        {
            RuleFor(x => x.Title).NotNull().WithMessage("Can't be empty")
                                     .NotEmpty().WithMessage("Can't be empty");
            RuleFor(x => x.Description).NotNull().WithMessage("Can't be empty")
                                    .NotEmpty().WithMessage("Can't be empty");
            RuleFor(x => x.TeamMemberId).NotNull().WithMessage("Can't be empty")
                                       .NotEmpty().WithMessage("Can't be empty");
            RuleFor(x => x.PodcastCategoryId).NotNull().WithMessage("Can't be empty")
                                       .NotEmpty().WithMessage("Can't be empty");
        }
    }
}
