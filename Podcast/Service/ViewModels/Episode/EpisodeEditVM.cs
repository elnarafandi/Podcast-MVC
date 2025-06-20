using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Episode
{
    public class EpisodeEditVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Image {  get; set; }
        public IFormFile? UploadImage { get; set; }
        public string? Audio { get; set; }
        public IFormFile? AudioFile { get; set; }
        public int PodcastId { get; set; }
        public List<int>? GuestIds { get; set; }
    }
    public class EpisodeEditVMValidator : AbstractValidator<EpisodeEditVM>
    {
        public EpisodeEditVMValidator()
        {
            RuleFor(x => x.Title).NotNull().WithMessage("Can't be empty")
                                     .NotEmpty().WithMessage("Can't be empty");
            RuleFor(x => x.Description).NotNull().WithMessage("Can't be empty")
                                    .NotEmpty().WithMessage("Can't be empty");
            RuleFor(x => x.PodcastId).NotNull().WithMessage("Can't be empty")
                                       .NotEmpty().WithMessage("Can't be empty");
        }
    }
}
