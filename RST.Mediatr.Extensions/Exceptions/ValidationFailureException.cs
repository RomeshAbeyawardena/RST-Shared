using RST.Contracts;
using RST.Defaults;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RST.Mediatr.Extensions.Exceptions;

/// <summary>
/// Represents a validation failure exception
/// </summary>
public class ValidationFailureException : ValidationException
{
    private static string GenerateMessage(Action<IDictionaryBuilder<string, string>> builder)
    {
        var dictionary = new Dictionary<string, string>();
        var dictionaryBuilder = new DefaultDictionaryBuilder<string, string>(dictionary);
        builder.Invoke(dictionaryBuilder);
        return GenerateMessage(dictionary);
    }


    private static string GenerateMessage(IDictionary<string, string> validationFailures)
    {
        var stringBuilder = new StringBuilder("Validation failed: ");
        foreach (var (key, value) in validationFailures)
        {
            stringBuilder.AppendLine($"{key}: {value}");
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="validationFailures"></param>
    public ValidationFailureException(Action<IDictionaryBuilder<string, string>> validationFailures)
        : base(GenerateMessage(validationFailures))
    {
        var dictionary = new DefaultDictionaryBuilder<string, string>();
        validationFailures.Invoke(dictionary);
        ValidationFailures = dictionary;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="validationFailures"></param>
    public ValidationFailureException(IDictionary<string, string> validationFailures)
        : base(GenerateMessage(validationFailures))
    {
        ValidationFailures = new ReadOnlyDictionary<string, string>(validationFailures);
    }

    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyDictionary<string, string> ValidationFailures { get; }
}
