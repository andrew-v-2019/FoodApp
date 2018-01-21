using System;
using System.Linq;
using FluentValidation;
using Services.Extensions;
using Services.Interfaces;
using ViewModels;
using ViewModels.Menu;

namespace Web.Validators
{
    public class MenuValidator : AbstractValidator<UpdateMenuViewModel>
    {
        private readonly IMenuService _menuService;

        public MenuValidator(IMenuService menuService)
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;
            _menuService = menuService;
            RuleFor(x => x).Must(BeTheOneForDate).WithMessage(LocalizationStrings.AnotherMenuForDateExists);
            RuleFor(x => x).Must(BeInFuture).WithMessage(LocalizationStrings.DateShouldBeInFuture);
            RuleFor(x => x)
                .Must(HaveCorrectSectionStructure)
                .WithMessage(LocalizationStrings.IncorrectSectionStructure);
            RuleFor(x => x.MenuId)
                .Must(_menuService.CheckIfMenuIsEditable)
                .WithMessage(LocalizationStrings.MenuIsLocked);
            RuleFor(x => _menuService.CheckIfOrderForMenuSubmitted(x.MenuId))
                .Equal(false)
                .WithMessage(LocalizationStrings.MenuIsLocked);
           
        }

        public bool HaveCorrectSectionStructure(UpdateMenuViewModel model)
        {
            var template = _menuService.GetLastMenuAsTemplate();
            var correctSectionsCount = template.Sections.Count;
            return model.Sections.Count == correctSectionsCount &&
                   model.Sections.All(sec => sec.Items.Any() && !sec.Items.Any(i => string.IsNullOrWhiteSpace(i.Name)));
        }


        public bool BeTheOneForDate(UpdateMenuViewModel model)
        {
            var lunchDate = model.LunchDate.ParseDate();
            return !_menuService.MenuForDateExisits(lunchDate, model.MenuId);
        }

        public bool BeInFuture(UpdateMenuViewModel model)
        {
            var lunchDate = model.LunchDate.ParseDate();
            return lunchDate.Date >= DateTime.Now;
        }
    }
}
