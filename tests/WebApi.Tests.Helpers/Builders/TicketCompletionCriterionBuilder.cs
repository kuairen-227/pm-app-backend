using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Tests.Helpers.Builders.Common;

namespace WebApi.Tests.Helpers.Builders;

public class TicketCompletionCriterionBuilder : BaseBuilder<TicketCompletionCriterionBuilder, TicketCompletionCriterion>
{
    private string _criterion = "デフォルトの完了基準内容";
    private bool _isCompleted = false;

    public TicketCompletionCriterionBuilder WithCriterion(string criterion)
    {
        _criterion = criterion;
        return this;
    }

    public TicketCompletionCriterionBuilder WithIsCompleted(bool isCompleted)
    {
        _isCompleted = isCompleted;
        return this;
    }

    public override TicketCompletionCriterion Build()
    {
        var criterion = new TicketCompletionCriterion(
            _criterion,
            _createdBy,
            _clock
        );

        if (_isCompleted)
        {
            criterion.Complete(_createdBy);
        }

        return criterion;
    }
}
