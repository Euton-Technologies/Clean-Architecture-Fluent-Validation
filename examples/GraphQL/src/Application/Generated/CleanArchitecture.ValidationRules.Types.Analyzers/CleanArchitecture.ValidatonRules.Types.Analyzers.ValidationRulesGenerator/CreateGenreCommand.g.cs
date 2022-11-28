using FluentValidation;

namespace MoviesExample.Application.Genres.Commands.CreateGenre
{
    public static class CreateGenreCommandValidators
    {
        public static IRuleBuilderOptions<CreateGenreCommand, string> Name<CreateGenreCommand>(this IRuleBuilder<CreateGenreCommand, string?> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("{0} is required.")
                .MaximumLength(20).WithMessage("{0} must not exceed 20 characters.");
        }
    }
}
