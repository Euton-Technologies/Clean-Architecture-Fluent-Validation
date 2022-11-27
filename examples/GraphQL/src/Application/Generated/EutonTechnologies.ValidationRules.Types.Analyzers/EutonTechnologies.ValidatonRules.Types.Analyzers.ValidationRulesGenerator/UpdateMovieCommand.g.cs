using FluentValidation;

namespace MoviesExample.Application.Movies.Commands.UpdateMovie
{
    public static class UpdateMovieCommandValidators
    {
        public static IRuleBuilderOptions<UpdateMovieCommand, string> Title<UpdateMovieCommand>(this IRuleBuilder<UpdateMovieCommand, string?> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("{0} is required.")
                .MaximumLength(20).WithMessage("{0} must not exceed 20 characters.");
        }
    }
}
