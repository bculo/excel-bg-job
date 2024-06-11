using Excel.API.Interfaces.Excel;
using FluentValidation;

namespace Excel.API.Validators;

public class ExcelPersonValidator : AbstractValidator<ExcelPerson>
{
    public ExcelPersonValidator()
    {
        RuleFor(i => i.Age).NotEmpty().NotNull();
        RuleFor(i => i.FirstName).NotEmpty().NotNull();
        RuleFor(i => i.LastName).NotNull();
    }
}