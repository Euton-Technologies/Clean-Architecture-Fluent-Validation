using FluentValidation;

namespace MoviesExample.Application.Movies.Commands.CreateMovie
{
    public static class CreateMovieCommandValidators
    {
        public static IRuleBuilderOptions<CreateMovieCommand, string> Title<CreateMovieCommand>(this IRuleBuilder<CreateMovieCommand, string?> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("{0} is required.")
                .MaximumLength(20).WithMessage("{0} must not exceed 20 characters.");
        }
    }
}
