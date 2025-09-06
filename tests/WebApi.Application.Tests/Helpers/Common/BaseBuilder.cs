using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions;

namespace WebApi.Application.Helpers.Common;

public abstract class BaseBuilder<TBuilder, TEntity>
    where TBuilder : BaseBuilder<TBuilder, TEntity>
{
    protected Guid _createdBy = Guid.NewGuid();
    protected IDateTimeProvider _clock = new FakeDateTimeProvider();

    public TBuilder WithCreatedBy(Guid createdBy)
    {
        _createdBy = createdBy;
        return (TBuilder)this;
    }

    public TBuilder WithClock(IDateTimeProvider clock)
    {
        _clock = clock;
        return (TBuilder)this;
    }

    public abstract TEntity Build();
}
