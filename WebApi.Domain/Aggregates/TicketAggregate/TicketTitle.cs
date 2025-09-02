using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.TicketAggregate;

public sealed class TicketTitle : ValueObject
{
    public string Value { get; }

    private TicketTitle(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("TICKET_TITLE_REQUIRED", "TicketTitle は必須です");

        Value = value;
    }

    public static TicketTitle Create(string value) => new TicketTitle(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
