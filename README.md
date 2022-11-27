# Clean-Architecture-Fluent-Validation
Source Generator to automatically validate persistence layer data annotations without manual duplication.

## Overview
This source generator was inspired by Clean Architecture.  One of the problems that we have encountered is needing to have both database constraints and input constraints.  We wouldn't want to create a database with a string with no max length when the field calls for only a few characters.  We also wouldn't want to have to keep both the persistence entity and the data transfer object in sync for these max character lengths.  

So EutonTechnologies.ValidatonRules.Types.Analyzers was born and it will automatically generate the Fluent Validation files that you can call to validate the persistence layer's object.  In order to allow for the most flexibility, Fluent Validation's abstract validator isn't generated automatically but rather the rules that can be easily called by your AbstractValdiator for the dto.  See Usage for more information.

## Installation

```bash
$> dotnet add package EutonTechnologies.ValidationRules.Types.Analyzers
```

## Usage

Add **FluentValidation** [validator](https://docs.fluentvalidation.net/en/latest/start.html)


```cs
//Application Layer Code
namespace MoviesExample.Application.Genres.Commands.CreateGenre
{
	[ExtendValidation(typeof(Genre))]
    public record CreateGenreCommand : IRequest<CreateGenrePayload>
    {
        public string? Name { get; init; }
    }
    public class CreateGenreCommandValidator : AbstractValidator<CreateGenreCommand>
    {
        public CreateGenreCommandValidator()
        {
            RuleFor(v => v.Name).Name();//The Name method is created automatically by the source generator.
        }
    }
}
//Domain Layer Entities

namespace MoviesExample.Domain.Entities;
{
	public class Genre : BaseAuditableEntity
	{
		[Required]
		[MaxLength(20)]
		public string? Name { get; set; }
	}
}

//Generated code by the source generator
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

```

## Examples
  - 📄 [GraphQL](examples/GraphQL/README.md)

## Support

If you are having problems, please let us know by [raising a new issue](https://github.com/Euton-Technologies/Clean-Architecture-Fluent-Validation/issues/new).

## License

This project is licensed with the [MIT license](LICENSE).