using System.Diagnostics.CodeAnalysis;
using DMS.Monitor.Application.Read;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Monitor.Api.Extensions;

public static class QueryResultExtensions
{
    public static IActionResult ToActionResult<TResult>(this QueryResult<TResult> queryResult)
    {
        if (TryResolve(queryResult, out TResult? result, out List<string> errors))
        {
            return new OkObjectResult(result);
        }
        else
        {
            return new BadRequestObjectResult(errors);
        }
    }

    private static bool TryResolve<TResult>(
        QueryResult<TResult> queryResult,
        out TResult? result,
        out List<string> errors)
    {
        var tempErrors = new List<string>();
        var isResolved = TryResolve(queryResult, out result, e => tempErrors.AddRange(e));
        errors = tempErrors;
        return isResolved;
    }

    private static bool TryResolve<TResult>(
        QueryResult<TResult> actionResult,
        [MaybeNullWhen(false)] out TResult result,
        Action<List<string>> fail)
    {
        TResult? initResult = default;
        actionResult.Resolve(res => initResult = res, fail);
        result = initResult;
        return initResult is not null;
    }
}
