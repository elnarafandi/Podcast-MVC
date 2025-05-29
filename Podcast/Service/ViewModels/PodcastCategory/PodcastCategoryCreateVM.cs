using FluentValidation;
using Service.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.PodcastCategory
{
    public class PodcastCategoryCreateVM
    {
        public string Name { get; set; }
    }
    public class PodcastCategoryCreateVMValidator : AbstractValidator<PodcastCategoryCreateVM>
    {
        public PodcastCategoryCreateVMValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Can't be empty")
                                .NotEmpty().WithMessage("Can't be empty");
        }
    }
}
