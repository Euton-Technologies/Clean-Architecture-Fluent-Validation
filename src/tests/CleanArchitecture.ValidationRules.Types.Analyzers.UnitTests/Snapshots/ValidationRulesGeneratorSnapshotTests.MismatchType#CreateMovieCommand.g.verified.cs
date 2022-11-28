//HintName: CreateMovieCommand.g.cs
using FluentValidation;

namespace RandomNamespace.Dtos
{
    public static class CreateMovieCommandValidators
    {
        public static IRuleBuilderOptions<CreateMovieCommand, string> Name<T>(this IRuleBuilder<CreateMovieCommand, string?> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("This is my error message")
                .MinimumLength(1).WithMessage("{0} must be at least 1 characters.");
        }
    }
}
