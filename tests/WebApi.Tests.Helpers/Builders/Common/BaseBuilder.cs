using WebApi.Domain.Abstractions;
using WebApi.Tests.Helpers.Fixtures;

namespace WebApi.Tests.Helpers.Builders.Common;

public abstract class BaseBuilder<TBuilder, TEntity>
    where TBuilder : BaseBuilder<TBuilder, TEntity>
{
    protected Guid _createdBy = Guid.NewGuid();
    protected DateTime _createdAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    protected IDateTimeProvider _clock;

    protected BaseBuilder()
    {
        _clock = new FakeDateTimeProvider(_createdAt);
    }

    public TBuilder WithCreatedBy(Guid createdBy)
    {
        _createdBy = createdBy;
        return (TBuilder)this;
    }

    public TBuilder WithCreatedAt(DateTime createdAt)
    {
        _createdAt = createdAt;
        _clock = new FakeDateTimeProvider(createdAt);
        return (TBuilder)this;
    }

    public TBuilder WithClock(IDateTimeProvider clock)
    {
        _clock = clock;
        return (TBuilder)this;
    }

    public abstract TEntity Build();
}
