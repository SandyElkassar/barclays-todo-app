namespace BarclaysTodo.Api.Services;

public class ServiceResult<T>
{
    private ServiceResult(T? value, bool isSuccess, bool notFound, IReadOnlyCollection<string> errors)
    {
        Value = value;
        IsSuccess = isSuccess;
        NotFound = notFound;
        Errors = errors;
    }

    public T? Value { get; }

    public bool IsSuccess { get; }

    public bool NotFound { get; }

    public IReadOnlyCollection<string> Errors { get; }

    public static ServiceResult<T> Success(T value)
    {
        return new ServiceResult<T>(value, true, false, Array.Empty<string>());
    }

    public static ServiceResult<T> Failure(IReadOnlyCollection<string> errors)
    {
        return new ServiceResult<T>(default, false, false, errors);
    }

    public static ServiceResult<T> NotFoundResult(string error)
    {
        return new ServiceResult<T>(default, false, true, new[] { error });
    }
}