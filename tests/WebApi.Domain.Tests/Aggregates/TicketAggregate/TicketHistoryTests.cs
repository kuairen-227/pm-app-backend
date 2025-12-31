using FluentAssertions;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Common;
using WebApi.Domain.Tests.Helpers;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Domain.Tests.Aggregates.TicketAggregate;

public class TicketHistoryTests : BaseDomainTest
{
    private readonly TicketHistoryBuilder _historyBuilder;

    public TicketHistoryTests()
    {
        _historyBuilder = new TicketHistoryBuilder();
    }

    [Fact]
    public void 正常系_インスタンス生成()
    {
        // Arrange & Act
        var result = _historyBuilder.Build();

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void 異常系_インスタンス生成_ActorIdが空の場合()
    {
        // Arrange & Act
        var act = () => _historyBuilder.WithActorId(Guid.Empty).Build();

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.ACTOR_ID_REQUIRED");
    }

    [Theory]
    [InlineData("タイトル変更前", "タイトル変更後")]
    [InlineData(null, "タイトル変更後")]
    [InlineData("タイトル変更前", null)]
    public void 正常系_AddChange(string? before, string? after)
    {
        // Arrange
        var result = _historyBuilder.Build();

        // Act
        result.AddChange(
            TicketField.Title,
            TicketHistoryChangeValue.From(before),
            TicketHistoryChangeValue.From(after)
        );

        // Assert
        var change = result.Changes.Should().ContainSingle().Subject;
        change.Field.Should().Be(TicketField.Title);
        change.Before?.Value.Should().Be(before);
        change.After?.Value.Should().Be(after);
    }
}
