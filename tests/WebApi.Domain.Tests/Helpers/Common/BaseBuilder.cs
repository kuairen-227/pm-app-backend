using WebApi.Domain.Abstractions;
using WebApi.Domain.Tests.Helpers.Common;

namespace WebApi.Domain.Helpers.Common;

public abstract class BaseBuilder<TBuilder, TEntity>
    where TBuilder : BaseBuilder<TBuilder, TEntity>
{
    protected Guid _createdBy = Guid.NewGuid();
    protected DateTimeOffset _createdAt = DateTimeOffset.UtcNow;
    protected IDateTimeProvider _clock = new FakeDateTimeProvider();

    public TBuilder WithCreatedBy(Guid createdBy)
    {
        _createdBy = createdBy;
        return (TBuilder)this;
    }

    public TBuilder WithCreatedAt(DateTimeOffset createdAt)
    {
        _createdAt = createdAt;
        return (TBuilder)this;
    }

    public TBuilder WithClock(IDateTimeProvider clock)
    {
        _clock = clock;
        return (TBuilder)this;
    }

    public abstract TEntity Build();
}
