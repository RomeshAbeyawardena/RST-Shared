using RST.Contracts;

namespace RST.Defaults
{
    /// <summary>
    /// Represents a set of result methods 
    /// </summary>
    public static class Result
    {
        /// <summary>
        /// Produces a result object
        /// </summary>
        /// <param name="value">The result value</param>
        /// <param name="statusCode">Status code of result</param>
        /// <param name="statusMessage">Status or error message</param>
        /// <param name="isSuccessful">Determines whether the result was successful</param>
        /// <returns></returns>
        public static IResult GetBaseResult(object value, int? statusCode = 200,
            string? statusMessage = null,
            bool isSuccessful = true)
        {
            return new DefaultResult
            {
                IsSuccessful = isSuccessful,
                StatusCode = statusCode,
                StatusMessage = statusMessage,
                Value = value
            };
        }

        /// <summary>
        /// Produces a typed result of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The result value</param>
        /// <param name="statusCode">Status code of result</param>
        /// <param name="statusMessage">Status or error message</param>
        /// <param name="isSuccessful">Determines whether the result was successful</param>
        /// <returns></returns>
        public static IResult<T> GetResult<T>(T value, int? statusCode = 200,
            string? statusMessage = null,
            bool isSuccessful = true)
        {
            return new DefaultResult<T>
            {
                IsSuccessful = isSuccessful,
                StatusCode = statusCode,
                StatusMessage = statusMessage,
                Value = value
            };
        }

        /// <summary>
        /// Produces a paged result 
        /// </summary>
        /// <typeparam name="TResult">Type of result</typeparam>
        /// <param name="result">A list of <typeparamref name="TResult"/></param>
        /// <param name="pageNumber">The current page number</param>
        /// <param name="totalNumberOfPages">Total pages available</param>
        /// <param name="totalItems">Total items of unpaged list</param>
        /// <param name="statusCode">Status code of result</param>
        /// <param name="statusMessage">Status or error message</param>
        /// <param name="isSuccessful">Determines whether the result was successful</param>
        /// <returns></returns>
        public static IPagedResult<TResult> GetPaged<TResult>(
            IEnumerable<TResult> result,
            int pageNumber,
            int totalNumberOfPages,
            int totalItems,
            int? statusCode = 200,
            string? statusMessage = null,
            bool isSuccessful = true)
        {
            return new DefaultPagedResult<TResult>()
            {
                IsSuccessful = isSuccessful,
                StatusCode = statusCode,
                StatusMessage = statusMessage,
                PageNumber = pageNumber,
                TotalItems = totalItems,
                NumberOfPages = totalNumberOfPages,
                Results = result
            };
        }
    }

    /// <summary>
    /// Represents a default implementation for <see cref="IResult{T}"/>
    /// </summary>
    /// <typeparam name="T"><see cref="Type"/> of result</typeparam>
    public record DefaultResult<T> : IResult<T>
    {
        ///<inheritdoc/>
        public string? StatusMessage { get; set; }

        ///<inheritdoc/>
        public int? StatusCode { get; set; }

        object? IResult.Value => Value;

        /// <summary>
        /// Gets or sets the strong typed value
        /// </summary>
        public T? Value { get; set; }

        ///<inheritdoc/>
        public bool IsSuccessful { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <inheritdoc cref="IRepository{T}"/>
    public record DefaultResult : IResult
    {
        ///<inheritdoc/>
        public string? StatusMessage { get; set; }

        ///<inheritdoc/>
        public int? StatusCode { get; set; }

        ///<inheritdoc/>
        public virtual object? Value { get; set; }

        ///<inheritdoc/>
        public bool IsSuccessful { get; set; }
    }
}
