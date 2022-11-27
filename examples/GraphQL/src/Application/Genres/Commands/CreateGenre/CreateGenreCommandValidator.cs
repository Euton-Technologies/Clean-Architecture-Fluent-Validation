using FluentValidation;

namespace MoviesExample.Application.Genres.Commands.CreateGenre;

public class CreateGenreCommandValidator : AbstractValidator<CreateGenreCommand>
{
    public CreateGenreCommandValidator()
    {
        RuleFor(v => v.Name).Name();
    }
}
