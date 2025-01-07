namespace DMS.Monitor.Application.Read;

public class QueryResult<TResult>
{
    private readonly TResult? _result;
    private readonly List<string>? _errors;
    private readonly ResultState _state;

    private QueryResult(TResult result)
    {
        _result = result;
        _state = ResultState.Success;
    }

    private QueryResult(string error)
    {
        if (string.IsNullOrEmpty(error))
        {
            throw new ArgumentException("Error cannot be empty", nameof(error));
        }

        _errors = [error];
        _state = ResultState.Fail;
    }

    private QueryResult(List<string> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        _errors = errors;
        _state = ResultState.Fail;
    }

    public static QueryResult<TResult> CreateSuccess(TResult result) => new(result);

    public static QueryResult<TResult> CreateFail(List<string> errors) => new(errors);

    public static QueryResult<TResult> CreateFail(string error) => new(error);

    public virtual void Resolve(Action<TResult> success, Action<List<string>> fail)
    {
        if (_state == ResultState.Success)
        {
            success(_result!);
        }
        else if (_state == ResultState.Fail)
        {
            fail(_errors!);
        }
    }

    public static implicit operator QueryResult<TResult>(TResult value) => new(value);

    public static implicit operator QueryResult<TResult>(List<string> errors) => new(errors);

    public static implicit operator QueryResult<TResult>(string error) => new(error);

    private enum ResultState : byte
    {
        Fail = 1,
        Success
    }
}
