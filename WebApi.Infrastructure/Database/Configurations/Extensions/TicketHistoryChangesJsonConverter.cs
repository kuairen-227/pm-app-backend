using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApi.Domain.Aggregates.TicketAggregate;

namespace WebApi.Infrastructure.Database.Configurations.Extensions;

public sealed class TicketHistoryChangesJsonConverter
    : ValueConverter<IReadOnlyCollection<TicketHistoryChange>, string>
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public TicketHistoryChangesJsonConverter()
        : base(
            v => JsonSerializer.Serialize(v, Options),
            v => JsonSerializer.Deserialize<List<TicketHistoryChange>>(v, Options)!
        )
    {
    }
}
