namespace WebApi.Application.Commands.Tickets.UpdateTicket;

public interface ICompletionCriterionOperationDto
{
    CompletionCriterionOperationType Type { get; }
}

public sealed class AddCompletionCriterionOperationDto
    : ICompletionCriterionOperationDto
{
    public CompletionCriterionOperationType Type
        => CompletionCriterionOperationType.Add;

    public string Criterion { get; }

    public AddCompletionCriterionOperationDto(string criterion)
    {
        Criterion = criterion
            ?? throw new ArgumentNullException(nameof(criterion));
    }
}

public sealed class EditCompletionCriterionOperationDto
    : ICompletionCriterionOperationDto
{
    public CompletionCriterionOperationType Type
        => CompletionCriterionOperationType.Edit;

    public Guid CriterionId { get; }
    public string Criterion { get; }

    public EditCompletionCriterionOperationDto(
        Guid criterionId,
        string criterion)
    {
        if (criterionId == Guid.Empty)
            throw new ArgumentException("CriterionId は必須です", nameof(criterionId));

        CriterionId = criterionId;
        Criterion = criterion
            ?? throw new ArgumentNullException(nameof(criterion));
    }
}

public sealed class DeleteCompletionCriterionOperationDto
    : ICompletionCriterionOperationDto
{
    public CompletionCriterionOperationType Type
        => CompletionCriterionOperationType.Delete;

    public Guid CriterionId { get; }

    public DeleteCompletionCriterionOperationDto(Guid criterionId)
    {
        if (criterionId == Guid.Empty)
            throw new ArgumentException("CriterionId は必須です", nameof(criterionId));

        CriterionId = criterionId;
    }
}

public sealed class CompleteCompletionCriterionOperationDto
    : ICompletionCriterionOperationDto
{
    public CompletionCriterionOperationType Type
        => CompletionCriterionOperationType.Complete;

    public Guid CriterionId { get; }

    public CompleteCompletionCriterionOperationDto(Guid criterionId)
    {
        if (criterionId == Guid.Empty)
            throw new ArgumentException("CriterionId は必須です", nameof(criterionId));

        CriterionId = criterionId;
    }
}

public sealed class ReopenCompletionCriterionOperationDto
    : ICompletionCriterionOperationDto
{
    public CompletionCriterionOperationType Type
        => CompletionCriterionOperationType.Reopen;

    public Guid CriterionId { get; }

    public ReopenCompletionCriterionOperationDto(Guid criterionId)
    {
        if (criterionId == Guid.Empty)
            throw new ArgumentException("CriterionId は必須です", nameof(criterionId));

        CriterionId = criterionId;
    }
}
