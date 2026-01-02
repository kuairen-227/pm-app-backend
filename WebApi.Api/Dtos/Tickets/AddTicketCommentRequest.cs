using WebApi.Api.Common;

namespace WebApi.Api.Dtos.Tickets;

/// <summary>
/// チケットコメント投稿リクエストDTO
/// </summary>
public class AddTicketCommentRequest
{
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
    /// コメント
    /// </summary>
    public PatchField<string?> Comment { get; set; } = PatchField<string?>.NotSpecified();

    /// <summary>
    /// 通知対象ユーザー
    /// </summary>
    public required IReadOnlyCollection<Guid> NotificationRecipientIds { get; set; }
}
