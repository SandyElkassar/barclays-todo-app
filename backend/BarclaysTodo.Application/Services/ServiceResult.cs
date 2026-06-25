namespace BarclaysTodo.Application.Services;

public class ServiceResult
{
    protected ServiceResult(bool isSuccess, bool notFound, IReadOnlyCollection<string> errors)
    {
        IsSuccess = isSuccess;
        NotFound = notFound;
        Errors = errors;
    }

    public bool IsSuccess { get; }

    public bool NotFound { get; }

    public IReadOnlyCollection<string> Errors { get; }

    public static ServiceResult Success()
    {
        return new ServiceResult(true, false, Array.Empty<string>());
    }

    public static ServiceResult Failure(IReadOnlyCollection<string> errors)
    {
        return new ServiceResult(false, false, errors);
    }

    public static ServiceResult NotFoundResult(string error)
    {
        return new ServiceResult(false, true, new[] { error });
    }
}

public sealed class ServiceResult<T> : ServiceResult
{
    private ServiceResult(T? value, bool isSuccess, bool notFound, IReadOnlyCollection<string> errors)
        : base(isSuccess, notFound, errors)
    {
        Value = value;
    }

    public T? Value { get; }

    public static ServiceResult<T> Success(T value)
    {
        return new ServiceResult<T>(value, true, false, Array.Empty<string>());
    }

    public new static ServiceResult<T> Failure(IReadOnlyCollection<string> errors)
    {
        return new ServiceResult<T>(default, false, false, errors);
    }

    public new static ServiceResult<T> NotFoundResult(string error)
    {
        return new ServiceResult<T>(default, false, true, new[] { error });
    }
}