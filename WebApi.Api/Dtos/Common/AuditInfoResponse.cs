using System.ComponentModel.DataAnnotations;

namespace WebApi.Api.Dtos.Common;

/// <summary>
/// 監査情報レスポンスDTO
/// </summary>
public class AuditInfoResponse
{
    /// <summary>
    /// 作成者
    /// </summary>
    [Required]
    public Guid CreatedBy { get; set; }

    /// <summary>
    /// 作成日時
    /// </summary>
    [Required]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 更新者
    /// </summary>
    [Required]
    public Guid UpdatedBy { get; set; }

    /// <summary>
    /// 更新日時
    /// </summary>
    [Required]
    public DateTime UpdatedAt { get; set; }
}
