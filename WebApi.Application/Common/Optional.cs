namespace WebApi.Application.Common;

public readonly struct Optional<T>
{
    private readonly T? _value { get; }
    public bool HasValue { get; }

    private Optional(T value)
    {
        HasValue = true;
        _value = value;
    }

    public static Optional<T> Of(T value) => new Optional<T>(value);
    public static Optional<T> NotSet() => default;

    public bool TryGetValue(out T value)
    {
        if (HasValue && _value is not null)
        {
            value = _value;
            return true;
        }

        value = default!;
        return false;
    }
}
