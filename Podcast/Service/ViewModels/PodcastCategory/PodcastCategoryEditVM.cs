using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.PodcastCategory
{
    public class PodcastCategoryEditVM
    {
        public string Name { get; set; }
    }
    public class PodcastCategoryEditVMValidator : AbstractValidator<PodcastCategoryEditVM>
    {
        public PodcastCategoryEditVMValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Can't be empty")
                                .NotEmpty().WithMessage("Can't be empty");
        }
    }
}
