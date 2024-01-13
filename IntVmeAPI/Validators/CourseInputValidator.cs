using FluentValidation;
using IntVmeAPI.Schema.Mutations;

namespace IntVmeAPI.Validators
{
    public class CourseInputValidator : AbstractValidator<CourseInput>
    {
        public CourseInputValidator()
        {
            RuleFor(c => c.Name)
                .MinimumLength(3)
                .MaximumLength(20)
                .WithMessage("Course name must be within 3 and 20 characters")
                .WithErrorCode("COURSE_NAME_LENGTH");
        }
    }
}
