using EutonTechnologies.ValidationRules.Types.Analyzers.UnitTests.Helpers;
using VerifyXunit;

namespace EutonTechnologies.ValidationRules.Types.Analyzers.UnitTests
{

    [UsesVerify]
    public class ValidationRulesGeneratorSnapshotTests
    {
        [Fact]
        public Task GeneratesValidationCorrectly()
        {

            string source = @"
using System;
using System.ComponentModel.DataAnnotations;
using EutonTechnologies.ValidatonRules.Types;

namespace RandomNamespace.Dtos;

public class Movie
{
    [Required(ErrorMessage = ""This is my error message"")]
    [MinLength(1)]
    public string Name {get;set;}
}

[ExtendValidation(typeof(Movie))]
public class CreateMovieCommand
{
    public string Name {get;set;}
}
";
            return TestHelper.Verify(source);
        }

        [Fact]
        public Task MismatchType()
        {
            string source = @"
using System;
using System.ComponentModel.DataAnnotations;

namespace RandomNamespace.Dtos;

public class Movie
{
    [Required(ErrorMessage = ""This is my error message"")]
    [MinLength(1)]
    public string Name {get;set;}

    public string Published {get;set;}
}

[ExtendValidation(typeof(Movie))]
public class CreateMovieCommand
{
    public string Name {get;set;}

    public DateTime Published {get;set;}
}
";
            return TestHelper.Verify(source);
        }

        [Fact]
        public Task MismatchTypeCorrected()
        {
            string source = @"
using EutonTechnologies.ValidatonRules.Types;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace RandomNamespace.Dtos;

public class Movie
{
    [Required(ErrorMessage = ""This is my error message"")]
    [MinLength(1)]
    public string Name { get; set; }

    [Required]
    public DateTime Published { get; set; }
}

[ExtendValidation(typeof(Movie))]
public class CreateMovieCommand
{
    public string Name { get; set; }

    [JsonProperty(""published"")]
    public string PublishedStr { get; set; }

    [JsonIgnore]
    public DateTime? Published {
        get{ 
            return DateTime.TryParse(PublishedStr, out var published) ? published : null;
        }
    }
}
";
            return TestHelper.Verify(source);
        }

        [Fact]
        public Task Record()
        {
            string source = @"
using EutonTechnologies.ValidatonRules.Types;
using System.ComponentModel.DataAnnotations;

namespace RandomNamespace.Dtos;

public class Movie
{
    [Required(ErrorMessage = ""This is my error message"")]
    [MinLength(1)]
    public string Name { get; set; }

    [Required]
    public DateTime Published { get; set; }
}

[ExtendValidation(typeof(Movie))]
public record CreateMovieCommand(string Name);
";
            return TestHelper.Verify(source);
        }

        [Fact]
        public Task Inheritance()
        {
            string source = @"
using EutonTechnologies.ValidatonRules.Types;
using System.ComponentModel.DataAnnotations;

namespace RandomNamespace.Dtos;

public abstract class BaseMovie
{
    [Required(ErrorMessage = ""This is my error message"")]
    [MinLength(1)]
    public string Name { get; set; }

    [Required]
    public DateTime Published { get; set; }
}

public class ActionMovie  : BaseMovie
{

}

[ExtendValidation(typeof(ActionMovie))]
public record CreateMovieCommand(string Name);
";
            return TestHelper.Verify(source);
        }
    }
}