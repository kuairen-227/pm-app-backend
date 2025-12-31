namespace WebApi.Application.Common;

public readonly struct Optional<T>
{
    public bool HasValue { get; }
    public T Value { get; }

    // 値を持つ場合
    public Optional(T value)
    {
        HasValue = true;
        Value = value;
    }

    // 値を持たない場合
    private Optional(bool hasValue)
    {
        HasValue = hasValue;
        Value = default!;
    }

    public static Optional<T> NotSet() => new Optional<T>(false);
}
