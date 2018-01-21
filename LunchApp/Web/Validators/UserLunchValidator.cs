using System.Linq;
using FluentValidation;
using Services.Interfaces;
using ViewModels;
using ViewModels.UserLunch;

namespace Web.Validators
{
    public class UserLunchValidator : AbstractValidator<UserLunchViewModel>
    {
        private readonly IMenuService _menuService;

        public UserLunchValidator(IMenuService menuService)
        {

            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;
            _menuService = menuService;
            RuleFor(x => _menuService.CheckIfOrderForMenuSubmitted(x.MenuId))
                .Equal(false)
                .WithMessage(LocalizationStrings.MenuIsLocked);
            RuleFor(x => x)
                .Must(HaveCorrectSectionStructure)
                .WithMessage(LocalizationStrings.IncorrectSectionStructure);
            RuleFor(x => x.User.Name).NotEmpty().WithMessage(LocalizationStrings.UserNameIsEmpty);
        }

        public bool HaveCorrectSectionStructure(UserLunchViewModel model)
        {
            var template = _menuService.GetLastMenuAsTemplate();
            var correctSectionsCount = template.Sections.Count;
            return model.Sections.Count == correctSectionsCount &&
                   model.Sections.All(sec => sec.Items.Any(i => i.Checked));
        }
    }
}
