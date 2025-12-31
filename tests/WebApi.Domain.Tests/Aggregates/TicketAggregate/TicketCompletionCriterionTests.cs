using FluentAssertions;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Common;
using WebApi.Domain.Tests.Helpers;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Domain.Tests.Aggregates.TicketAggregate;

public class TicketCompletionCriterionTests : BaseDomainTest
{
    private readonly TicketCompletionCriterionBuilder _criterionBuilder;

    public TicketCompletionCriterionTests()
    {
        _criterionBuilder = new TicketCompletionCriterionBuilder();
    }

    [Fact]
    public void 正常系_インスタンス生成()
    {
        // Arrange & Act
        var result = new TicketCompletionCriterion(
            "完了基準の例",
            UserContext.Id,
            Clock
        );

        // Assert
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void 異常系_インスタンス生成_Criterionが空の場合(string? criterion)
    {
        // Arrange & Act
        var act = () => new TicketCompletionCriterion(
            criterion!,
            UserContext.Id,
            Clock
        );

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.COMPLETION_CRITERION_REQUIRED");
    }


    [Fact]
    public void 正常系_EditCriterion()
    {
        // Arrange
        var result = _criterionBuilder.Build();

        // Act
        result.EditCriterion("編集後の完了基準内容", UserContext.Id);

        // Assert
        result.Criterion.Should().Be("編集後の完了基準内容");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void 異常系_EditCriterion_Criterionが空の場合(string? criterion)
    {
        // Arrange
        var ticketCompletionCriterion = _criterionBuilder.Build();

        // Act
        var act = () => ticketCompletionCriterion.EditCriterion(criterion!, UserContext.Id);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.COMPLETION_CRITERION_REQUIRED");
    }

    [Fact]
    public void 正常系_Complete()
    {
        // Arrange
        var result = _criterionBuilder.WithIsCompleted(false).Build();

        // Act
        result.Complete(UserContext.Id);

        // Assert
        result.IsCompleted.Should().BeTrue();
    }

    [Fact]
    public void 異常系_Complete_既に完了している場合()
    {
        // Arrange
        var ticketCompletionCriterion = _criterionBuilder.WithIsCompleted(true).Build();

        // Act
        var act = () => ticketCompletionCriterion.Complete(UserContext.Id);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.ALREADY_COMPLETED");
    }

    [Fact]
    public void 正常系_Reopen()
    {
        // Arrange
        var result = _criterionBuilder.WithIsCompleted(true).Build();

        // Act
        result.Reopen(UserContext.Id);

        // Assert
        result.IsCompleted.Should().BeFalse();
    }

    [Fact]
    public void 異常系_Reopen_完了していない場合()
    {
        // Arrange
        var ticketCompletionCriterion = _criterionBuilder.WithIsCompleted(false).Build();

        // Act
        var act = () => ticketCompletionCriterion.Reopen(UserContext.Id);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.NOT_COMPLETED");
    }
}
