using FluentValidation;
using Utility.Configuration.Options.Abstractions;

namespace Infrastructure.Databases;

public class DatabaseOptions : IConfigurationOptions
{
    public static string SectionName => "Database";

    public string ConnectionString { get; set; } = string.Empty;
}

public class DatabaseOptionsValidator : AbstractValidator<DatabaseOptions>
{
    public DatabaseOptionsValidator()
    {
        RuleFor(o => o.ConnectionString)
            .NotEmpty();
    }
}