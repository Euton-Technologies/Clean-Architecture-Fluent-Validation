using MoviesExample.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace MoviesExample.Application.Movies.Commands.UpdateMovie;

public class UpdateMovieCommandValidator : AbstractValidator<UpdateMovieCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateMovieCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Title).Title()
            .MustAsync(BeUniqueTitle).WithMessage("The specified title already exists.");
    }

    public async Task<bool> BeUniqueTitle(UpdateMovieCommand model, string title, CancellationToken cancellationToken)
    {
        return await _context.Movies
            .Where(l => l.Id != model.Id)
            .AllAsync(l => l.Title != title, cancellationToken);
    }
}
