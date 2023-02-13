using RST.Contracts;

namespace RST.Defaults
{
    /// <summary>
    /// Represents a set of result methods 
    /// </summary>
    public static class Result
    {
        /// <summary>
        /// Gets a result object
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
                Message= statusMessage,
                Value = value
            };
        }

        /// <summary>
        /// Gets a typed result of <typeparamref name="T"/>
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
                Message = statusMessage,
                Value = value
            };
        }
    }

    internal class DefaultResult<T> : IResult<T>
    {
        public string? Message { get; set; }
        public int? StatusCode { get; set; }
        object? IResult.Value => Value;
        public T? Value { get; set; }
        public bool IsSuccessful { get; set; }
    }

    internal class DefaultResult : IResult
    {
        public string? Message { get; set; }
        public int? StatusCode { get; set; }
        public virtual object? Value { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
