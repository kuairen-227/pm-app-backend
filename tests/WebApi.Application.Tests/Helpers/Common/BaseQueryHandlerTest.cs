using AutoMapper;
using Moq;
using WebApi.Application.Abstractions;

namespace WebApi.Application.Tests.Helpers.Common;

public abstract class BaseQueryHandlerTest
{
    protected readonly Mock<IUserContext> UserContext;
    protected readonly Mock<IMapper> Mapper;

    protected BaseQueryHandlerTest()
    {
        UserContext = TestHelpers.CreateUserContext();
        Mapper = TestHelpers.CreateMapper();
    }
}
