namespace WebApi.Api.Common;

/// <summary>
/// PATCH 用フィールド
/// </summary>
/// <remarks>
/// PATCH リクエストにおいて「フィールドが指定されたかどうか」と「指定された値（null を含む）」を表現する型
/// </remarks>
public sealed class PatchField<T>
{
    /// <summary>
    /// フィールドが JSON に明示的に含まれていたかどうか
    /// </summary>
    public bool IsSpecified { get; }

    /// <summary>
    /// 指定された値
    /// </summary>
    /// <remarks>
    /// <see cref="IsSpecified"/> が true の場合のみ意味を持つ。null は「明示的に null が指定された」ことを表す。
    /// </remarks>
    public T? Value { get; }

    private PatchField(bool isSpecified, T? value)
    {
        IsSpecified = isSpecified;
        Value = value;
    }

    /// <summary>
    /// フィールドが JSON に含まれていなかった場合
    /// </summary>
    public static PatchField<T> NotSpecified()
        => new(false, default);

    /// <summary>
    /// フィールドが JSON に含まれていた場合
    /// </summary>
    /// <remarks>
    /// value が null の場合は「明示的な null 指定」を意味する。
    /// </remarks>
    public static PatchField<T> From(T? value)
        => new(true, value);
}
