using MoviesExample.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace MoviesExample.Application.Movies.Commands.CreateMovie;

public class CreateMovieCommandValidator : AbstractValidator<CreateMovieCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateMovieCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Title).Title()
            .MustAsync(BeUniqueTitle).WithMessage("The specified title already exists.");
    }

    public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
    {
        return await _context.Movies
            .AllAsync(l => l.Title != title, cancellationToken);
    }
}
