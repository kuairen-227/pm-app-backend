public readonly struct Optional<T>
{
    private readonly T? _value;
    public bool HasValue { get; }

    public T? Value =>
        HasValue
            ? _value
            : throw new InvalidOperationException("No value");

    private Optional(T? value)
    {
        HasValue = true;
        _value = value;
    }

    public static Optional<T> Of(T? value) => new(value);
    public static Optional<T> None() => default;
}
