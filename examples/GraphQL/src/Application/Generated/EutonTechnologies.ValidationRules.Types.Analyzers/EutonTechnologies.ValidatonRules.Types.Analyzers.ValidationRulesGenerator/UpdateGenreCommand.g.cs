using FluentValidation;

namespace MoviesExample.Application.Genres.Commands.UpdateGenre
{
    public static class UpdateGenreCommandValidators
    {
        public static IRuleBuilderOptions<UpdateGenreCommand, string> Name<UpdateGenreCommand>(this IRuleBuilder<UpdateGenreCommand, string?> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("{0} is required.")
                .MaximumLength(20).WithMessage("{0} must not exceed 20 characters.");
        }
    }
}
