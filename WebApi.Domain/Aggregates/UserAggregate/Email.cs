using System.Text.RegularExpressions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.UserAggregate;

public sealed class Email : ValueObject
{
    public string Value { get; }

    private Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("EMAIL_REQUIRED", "Email は必須です");
        if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new DomainException("EMAIL_INVALID", "不正な Email です");

        Value = value;
    }

    public static Email Create(string value) => new Email(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
