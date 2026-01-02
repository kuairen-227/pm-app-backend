using WebApi.Api.Common;
using WebApi.Application.Common;

namespace WebApi.Api.Mapping;

/// <summary>
/// PatchField 用の拡張メソッド
/// </summary>
public static class PatchFieldExtensions
{
    /// <summary>
    /// PatchField を Optional に変換する
    /// </summary>
    public static Optional<T> ToOptional<T>(this PatchField<T> field)
    {
        return field.IsSpecified
            ? Optional<T>.Of(field.Value)
            : Optional<T>.None();
    }
}
