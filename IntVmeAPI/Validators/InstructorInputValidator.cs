using FluentValidation;
using IntVmeAPI.Schema.Mutations;

namespace IntVmeAPI.Validators
{
    public class InstructorInputValidator : AbstractValidator<InstructorInput>
    {
        public InstructorInputValidator()
        {
            RuleFor(i => i.FirstName).NotEmpty();
            RuleFor(i => i.LastName).NotEmpty();
        }
    }
}
