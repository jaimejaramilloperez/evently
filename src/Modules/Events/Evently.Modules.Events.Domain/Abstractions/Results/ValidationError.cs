namespace Evently.Modules.Events.Domain.Abstractions.Results;

public sealed record ValidationError : Error
{
    public IList<Error> Errors { get; }

    public static ValidationError FromResults(IEnumerable<Result> results)
    {
        List<Error> errors = results.Where(r => r.IsFailure)
            .Select(r => r.Error)
            .ToList();

        return new(errors);
    }

    public ValidationError(IList<Error> errors)
        : base(
            "General.Validation",
            "One or more validation errors occurred",
            ErrorType.Validation)
    {
        Errors = errors.ToList();
    }
}
