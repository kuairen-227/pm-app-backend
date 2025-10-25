using AutoMapper;
using Moq;
using WebApi.Application.Abstractions;
using WebApi.Tests.Helpers.Fixtures;

namespace WebApi.Application.Tests.Helpers.Common;

public abstract class BaseQueryHandlerTest
{
    protected readonly Mock<IMapper> Mapper;
    protected readonly Mock<IUserContext> UserContext;
    protected readonly FakeDateTimeProvider Clock;

    protected BaseQueryHandlerTest()
    {
        UserContext = TestHelpers.CreateUserContext();
        Mapper = new Mock<IMapper>();
        Clock = new FakeDateTimeProvider();
    }
}
