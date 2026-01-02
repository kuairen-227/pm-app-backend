using WebApi.Api.Common;
using WebApi.Application.Commands.Tickets.UpdateTicket;

namespace WebApi.Api.Dtos.Tickets;

/// <summary>
/// チケット編集リクエストDTO
/// </summary>
public class UpdateTicketRequest
{
    /// <summary>
    /// タイトル
    /// </summary>
    public PatchField<string> Title { get; set; } = PatchField<string>.NotSpecified();

    /// <summary>
    /// 説明文
    /// </summary>
    public PatchField<string> Description { get; set; } = PatchField<string>.NotSpecified();

    /// <summary>
    /// 担当者
    /// </summary>
    public PatchField<Guid?> AssigneeId { get; set; } = PatchField<Guid?>.NotSpecified();

    /// <summary>
    /// 開始日
    /// </summary>
    public PatchField<DateOnly?> StartDate { get; set; } = PatchField<DateOnly?>.NotSpecified();

    /// <summary>
    /// 終了日
    /// </summary>
    public PatchField<DateOnly?> EndDate { get; set; } = PatchField<DateOnly?>.NotSpecified();

    /// <summary>
    /// ステータス
    /// </summary>
    public PatchField<string> Status { get; set; } = PatchField<string>.NotSpecified();

    /// <summary>
    /// 完了条件
    /// </summary>
    public PatchField<IReadOnlyList<ICompletionCriterionOperationDto>?> CompletionCriterionOperations { get; set; }
        = PatchField<IReadOnlyList<ICompletionCriterionOperationDto>?>.NotSpecified();

    /// <summary>
    /// コメント
    /// </summary>
    public PatchField<string?> Comment { get; set; } = PatchField<string?>.NotSpecified();

    /// <summary>
    /// 通知対象ユーザー
    /// </summary>
    public required IReadOnlyCollection<Guid> NotificationRecipientIds { get; set; }
}
