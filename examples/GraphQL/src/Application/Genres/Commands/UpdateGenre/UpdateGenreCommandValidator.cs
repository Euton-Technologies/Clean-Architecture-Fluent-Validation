using FluentValidation;

namespace MoviesExample.Application.Genres.Commands.UpdateGenre;

public class UpdateGenreCommandValidator : AbstractValidator<UpdateGenreCommand>
{
    public UpdateGenreCommandValidator()
    {

        RuleFor(v => v.Name).Name();
    }
}
