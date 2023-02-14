﻿using RST.Contracts;

namespace RST.Defaults;

internal class DefaultPagedResult<T, TResult> : DefaultResult<IEnumerable<T>>, IPagedResult<T, TResult>
    where T : struct
{
    public DefaultPagedResult()
    {
        Results = new List<TResult>();
    }

    public T PageNumber { get; set; }
    public T NumberOfPages { get; set; }
    public T TotalItems { get; set; }
    public IEnumerable<TResult> Results { get; set; }
    IEnumerable<TResult>? IResult<IEnumerable<TResult>>.Value => Results;
}